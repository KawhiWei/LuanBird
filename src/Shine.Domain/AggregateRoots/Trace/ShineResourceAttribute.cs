using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public class ShineResourceAttribute(
    string key,
    AttributeValueType valueType,
    string value,
    string traceId,
    string spanId)
    : AbstractAttribute(key, valueType, value)
{
    public required string TraceId { get; init; } = traceId;

    public required string SpanId { get; init; } = spanId;
}