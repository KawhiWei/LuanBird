using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.Framework.Extensions;
using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class Span : FullAggregateRoot
{
    public Span(string traceId, string spanId, string spanName, string parentSpanId, ulong startTimeUnixNano,
        ulong endTimeUnixNano, ulong durationNanoseconds, SpanStatusCode? statusCode, string? statusMessage,
        SpanKind spanKind, string serviceName, string serviceInstanceId, uint traceFlags, string? traceState)
    {
        TraceId = traceId;
        SpanId = spanId;
        SpanName = spanName;
        ParentSpanId = parentSpanId;
        StartTimeUnixNano = startTimeUnixNano;
        EndTimeUnixNano = endTimeUnixNano;
        DurationNanoseconds = durationNanoseconds;
        StatusCode = statusCode;
        StatusMessage = statusMessage;
        SpanKind = spanKind;
        ServiceName = serviceName;
        ServiceInstanceId = serviceInstanceId;
        TraceFlags = traceFlags;
        TraceState = traceState;
    }

    public string TraceId { get; private set; }

    public string SpanId { get; private set; }

    public string SpanName { get; private set; }

    public string ParentSpanId { get; private set; }

    public ulong StartTimeUnixNano { get; private set; }

    public ulong EndTimeUnixNano { get; private set; }

    public ulong DurationNanoseconds { get; private set; }

    public SpanStatusCode? StatusCode { get; private set; }

    public string? StatusMessage { get; private set; }

    public SpanKind SpanKind { get; private set; }

    public string ServiceName { get; private set; }

    public string ServiceInstanceId { get; private set; }

    public uint TraceFlags { get; private set; }

    public string? TraceState { get; private set; }

    public ICollection<SpanAttribute> SpanAttributes { get; private set; } = new List<SpanAttribute>();

    public ICollection<ResourceAttribute> ResourceAttributes { get; private set; } = new List<ResourceAttribute>();
    
    public void SetSpanAttributes(IEnumerable<SpanAttribute> spanAttributes)
    {
        SpanAttributes.AddRange(spanAttributes);
    }
    
    public void SetResourceAttributes(IEnumerable<ResourceAttribute> resourceAttributes)
    {
        ResourceAttributes.AddRange(resourceAttributes);
    }
}