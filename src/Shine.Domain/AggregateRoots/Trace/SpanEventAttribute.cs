using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanEventAttribute : AbstractAttribute
{
    public SpanEventAttribute(string traceId, string spanId, int spanEventIndex, string key,
        AttributeValueType valueType, string value) : base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
        SpanEventIndex = spanEventIndex;
    }

    public  string TraceId { get; private set; }

    public  string SpanId { get; private set; }

    public  int SpanEventIndex { get; private set;}
    
}