using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class ResourceAttribute : AbstractAttribute
{
    public ResourceAttribute(string traceId, string spanId, string key, AttributeValueType valueType, string value,
        string shineSpanId) :
        base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
        ShineSpanId = shineSpanId;
    }

    public string TraceId { get; init; }

    public string SpanId { get; init; }

    public string ShineSpanId { get; private set; }
}