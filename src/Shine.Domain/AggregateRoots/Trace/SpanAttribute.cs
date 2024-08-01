using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanAttribute : AbstractAttribute
{
    public SpanAttribute(string traceId, string spanId, string key, AttributeValueType valueType, string value,
        string shineSpanId)
        : base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
        ShineSpanId = shineSpanId;
    }

    public string TraceId { get; private set; }

    public string SpanId { get; private set; }

    public string ShineSpanId { get; private set; }
}