using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanLinkAttribute : AbstractAttribute
{
    public SpanLinkAttribute(string traceId, string spanId, int spanLinkIndex, string key, AttributeValueType valueType,
        string value) : base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
        SpanLinkIndex = spanLinkIndex;
    }

    public  string TraceId { get; private set; }

    public  string SpanId { get; private set; }

    public  int SpanLinkIndex { get; private set; }
}