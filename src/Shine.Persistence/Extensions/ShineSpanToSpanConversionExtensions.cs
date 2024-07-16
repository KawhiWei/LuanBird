using Shine.Dto;
using Shine.Persistence.Trace;

namespace Shine.Persistence.Extensions;

internal static class ShineSpanToSpanConversionExtensions
{
    internal static Span ToSpan(this ShineSpan shineSpan)
    {
        var efSpan = new Span
        {
            TraceId = shineSpan.TraceId,
            SpanId = shineSpan.SpanId,
            SpanName = shineSpan.SpanName,
            ParentSpanId = shineSpan.ParentSpanId,
            StartTimeUnixNano = shineSpan.StartTimeUnixNano,
            EndTimeUnixNano = shineSpan.EndTimeUnixNano,
            DurationNanoseconds = shineSpan.DurationNanoseconds,
            StatusCode = shineSpan.StatusCode,
            StatusMessage = shineSpan.StatusMessage,
            SpanKind = shineSpan.SpanKind,
            ServiceName = shineSpan.Resource.ServiceName,
            ServiceInstanceId = shineSpan.Resource.ServiceInstanceId,
            TraceFlags = shineSpan.TraceFlags,
            TraceState = shineSpan.TraceState
        };

        return efSpan;
    }

    internal static IEnumerable<SpanAttribute> ToSpanAttributes(this ShineSpan shineSpan)
    {
        return shineSpan.Attributes.Select(a => new SpanAttribute
        {
            TraceId = shineSpan.TraceId,
            SpanId = shineSpan.SpanId,
            Key = a.Key,
            ValueType = a.ValueType,
            Value = a.Value
        });
    }

    internal static IEnumerable<ResourceAttribute> ToResourceAttributes(this ShineSpan shineSpan)
    {
        return shineSpan.Resource.Attributes.Select(a => new ResourceAttribute
        {
            TraceId = shineSpan.TraceId,
            SpanId = shineSpan.SpanId,
            Key = a.Key,
            ValueType = a.ValueType,
            Value = a.Value
        });
    }

    internal static SpanEvent ToSpanEvent(this ShineSpanEvent spanEvent, ShineSpan span, int spanEventIndex)
    {
        var efSpanEvent = new SpanEvent
        {
            TraceId = span.TraceId,
            SpanId = span.SpanId,
            Index = spanEventIndex,
            Name = spanEvent.Name,
            TimestampUnixNano = spanEvent.TimestampUnixNano
        };

        return efSpanEvent;
    }

    internal static IEnumerable<SpanEventAttribute> ToSpanEventAttributes(this ShineSpanEvent shineSpanEvent,
        SpanEvent spanEvent)
    {
        return shineSpanEvent.Attributes.Select(a => new SpanEventAttribute
        {
            TraceId = spanEvent.TraceId,
            SpanId = spanEvent.SpanId,
            SpanEventIndex = spanEvent.Index,
            Key = a.Key,
            ValueType = a.ValueType,
            Value = a.Value
        });
    }

    internal static SpanLink ToSpanLink(this ShineSpanLink link, ShineSpan shineSpan, int spanLinkIndex)
    {
        var efLink = new SpanLink
        {
            TraceId = shineSpan.TraceId,
            SpanId = shineSpan.SpanId,
            Index = spanLinkIndex,
            LinkedTraceId = link.LinkedTraceId,
            LinkedSpanId = link.LinkedSpanId,
            LinkedTraceState = link.LinkedTraceState,
            LinkedTraceFlags = link.LinkedTraceFlags
        };

        return efLink;
    }

    internal static IEnumerable<SpanLinkAttribute> ToSpanLinkAttributes(
        this ShineSpanLink shineSpanLink,
        SpanLink spanLink)
    {
        return shineSpanLink.Attributes.Select(a => new SpanLinkAttribute
        {
            TraceId = spanLink.TraceId,
            SpanId = spanLink.SpanId,
            SpanLinkIndex = spanLink.Index,
            Key = a.Key,
            ValueType = a.ValueType,
            Value = a.Value
        });
    }
}