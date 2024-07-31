using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class ResourceAttribute : AbstractAttribute
{
    public ResourceAttribute(string traceId, string spanId, string key, AttributeValueType valueType, string value) :
        base(key, valueType, value)
    {
        TraceId = traceId;
        SpanId = spanId;
    }
    
    public string TraceId { get; init; }

    public string SpanId { get; init; }

    
}