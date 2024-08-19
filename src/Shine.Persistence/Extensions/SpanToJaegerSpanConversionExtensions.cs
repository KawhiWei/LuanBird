using Shine.Domain.AggregateRoots.Trace;
using Shine.Domain.Shared.Enums;
using Shine.Dto;
using Shine.Infrastructure;

namespace Shine.Persistence.Extensions;

internal static class SpanToJaegerSpanConversionExtensions
{
    public static IEnumerable<JaegerTraceOutputDto> ToJaegerTraces(
        this IEnumerable<Span> spans,
        IEnumerable<SpanAttribute> spanAttributes,
        IEnumerable<ResourceAttribute> resourceAttributes,
        IEnumerable<SpanEvent> spanEvents,
        IEnumerable<SpanEventAttribute> spanEventAttributes)
    {
        var spanAttributesBySpanId = spanAttributes
            .GroupBy(a => a.SpanId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var resourceAttributesBySpanId = resourceAttributes
            .GroupBy(a => a.SpanId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var spanEventsBySpanId = spanEvents
            .GroupBy(e => e.SpanId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var spanEventAttributesBySpanId = spanEventAttributes
            .GroupBy(a => a.SpanId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var jaegerSpans = new List<JaegerSpanOutputDto>();

        foreach (var g in spans.GroupBy(s => s.SpanId))
        {
            var spanId = g.Key;
            var efSpans = g;

            spanAttributesBySpanId.TryGetValue(spanId, out var efSpanAttributes);
            resourceAttributesBySpanId.TryGetValue(spanId, out var efResourceAttributes);
            spanEventsBySpanId.TryGetValue(spanId, out var efSpanEvents);
            spanEventAttributesBySpanId.TryGetValue(spanId, out var efSpanEventAttributes);

            efSpanAttributes ??= Array.Empty<SpanAttribute>();
            efSpanEvents ??= Array.Empty<SpanEvent>();
            efSpanEventAttributes ??= Array.Empty<SpanEventAttribute>();

            jaegerSpans.AddRange(efSpans.ToJaegerSpans(efSpanAttributes, efSpanEvents, efSpanEventAttributes));
        }

        var jaegerTraces = jaegerSpans
            .GroupBy(s => s.TraceId)
            .Select(g =>
            {
                var spansOfCurrentTrace = g.ToArray();
                var jaegerProcesses = new List<JaegerProcessOutputDto>();

                foreach (var span in spansOfCurrentTrace)
                {
                    resourceAttributesBySpanId.TryGetValue(span.TraceId, out var attributes);
                    attributes ??= Array.Empty<ResourceAttribute>();
                    var process = new JaegerProcessOutputDto
                    {
                        ProcessId = span.ProcessId,
                        ServiceName = attributes
                            .FirstOrDefault(a => a.Key == "service.name")?.Value ?? string.Empty,
                        Tags = Array.ConvertAll(attributes, ToJaegerTag)
                    };

                    jaegerProcesses.Add(process);
                }

                return new JaegerTraceOutputDto
                {
                    TraceId = g.Key,
                    Processes = jaegerProcesses
                        .DistinctBy(p => p.ProcessId)
                        .ToDictionary(p => p.ProcessId),
                    Spans = spansOfCurrentTrace
                };
            });

        return jaegerTraces;
    }

    private static IEnumerable<JaegerSpanOutputDto> ToJaegerSpans(
        this IEnumerable<Span> spans,
        IEnumerable<SpanAttribute> spanAttributes,
        IEnumerable<SpanEvent> spanEvents,
        IEnumerable<SpanEventAttribute> spanEventAttributes)
    {
        foreach (var span in spans)
        {
            var jaegerSpan = new JaegerSpanOutputDto
            {
                TraceId = span.TraceId,
                SpanId = span.SpanId,
                OperationName = span.SpanName,
                Flags = span.TraceFlags, // TODO: is this correct?
                StartTime = span.StartTimeUnixNano / 1000,
                Duration = span.DurationNanoseconds / 1000,
                ProcessId = span.ServiceInstanceId,
                References = string.IsNullOrWhiteSpace(span.ParentSpanId) // TODO: should we use span links?
                    ? Array.Empty<JaegerSpanReferenceOutputDto>()
                    :
                    [
                        new JaegerSpanReferenceOutputDto
                        {
                            TraceId = span.TraceId,
                            SpanId = span.ParentSpanId,
                            RefType = JaegerSpanReferenceType.ChildOf,
                        }
                    ],
                Tags = spanAttributes.ToJaegerSpanTags(span).ToArray(),
                Logs = spanEvents.ToJaegerSpanLogs(spanEventAttributes).ToArray()
            };

            yield return jaegerSpan;
        }
    }

    private static JaegerTagOutputDto ToJaegerTag(this AbstractAttribute attribute)
    {
        var jaegerTag = new JaegerTagOutputDto
        {
            Key = attribute.Key,
            Type = attribute.ValueType.ToJaegerTagType(),
            Value = ConvertTagValue(attribute.ValueType, attribute.Value)
        };

        return jaegerTag;
    }

    private static IEnumerable<JaegerTagOutputDto> ToJaegerSpanTags(
        this IEnumerable<SpanAttribute> spanAttributes,
        Span span)
    {
        if (span.StatusCode == SpanStatusCode.Error)
        {
            yield return new JaegerTagOutputDto { Key = "error", Type = JaegerTagType.Bool, Value = true };
        }

        yield return new JaegerTagOutputDto
        {
            Key = "span.kind",
            Type = JaegerTagType.String,
            Value = span.SpanKind.ToJaegerSpanKind()
        };

        foreach (var attribute in spanAttributes)
        {
            yield return attribute.ToJaegerTag();
        }
    }

    private static IEnumerable<JaegerSpanLogOutputDto> ToJaegerSpanLogs(
        this IEnumerable<SpanEvent> spanEvents,
        IEnumerable<SpanEventAttribute> spanEventAttributes)
    {
        var attributesBySpanEvent = spanEventAttributes
            .GroupBy(a => a.SpanEventIndex)
            .ToDictionary(g => g.Key, g => g.ToArray());

        foreach (var spanEvent in spanEvents)
        {
            var jaegerSpanLog = new JaegerSpanLogOutputDto
            {
                Timestamp = spanEvent.TimestampUnixNano / 1000,
                Fields = attributesBySpanEvent.TryGetValue(spanEvent.Index, out var attributes)
                    ? attributes.Select(a => new JaegerTagOutputDto
                    {
                        Key = a.Key,
                        Type = a.ValueType.ToJaegerTagType(),
                        Value = a.Value
                    }).ToArray()
                    : []
            };

            yield return jaegerSpanLog;
        }
    }

    private static string ToJaegerTagType(this AttributeValueType valueType) => valueType switch
    {
        AttributeValueType.StringValue => JaegerTagType.String,
        AttributeValueType.BoolValue => JaegerTagType.Bool,
        AttributeValueType.IntValue => JaegerTagType.Int64,
        AttributeValueType.DoubleValue => JaegerTagType.Float64,
        // TODO: ArrayValue, KvlistValue, BytesValue
        AttributeValueType.ArrayValue => JaegerTagType.String,
        AttributeValueType.KvListValue => JaegerTagType.String,
        AttributeValueType.BytesValue => JaegerTagType.String,
        _ => throw new ArgumentOutOfRangeException()
    };

    private static string ToJaegerSpanKind(this SpanKind spanKind) => spanKind switch
    {
        SpanKind.Internal => JaegerSpanKind.Internal,
        SpanKind.Server => JaegerSpanKind.Server,
        SpanKind.Client => JaegerSpanKind.Client,
        SpanKind.Producer => JaegerSpanKind.Producer,
        SpanKind.Consumer => JaegerSpanKind.Consumer,
        _ => JaegerSpanKind.Unspecified
    };

    private static object ConvertTagValue(this AttributeValueType valueType, string value) => valueType switch
    {
        AttributeValueType.StringValue => value,
        AttributeValueType.BoolValue => bool.Parse(value),
        AttributeValueType.IntValue => long.Parse(value),
        AttributeValueType.DoubleValue => double.Parse(value),
        // TODO: ArrayValue, KvlistValue, BytesValue
        AttributeValueType.ArrayValue => value,
        AttributeValueType.KvListValue => value,
        AttributeValueType.BytesValue => value,
        _ => throw new ArgumentOutOfRangeException()
    };
}