using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanAttribute : AbstractAttribute
{
    public SpanAttribute(string traceId, string spanId, string key, AttributeValueType valueType, string value)
        : base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
    }

    public string TraceId { get; private set; }

    public string SpanId { get; private set; }
}