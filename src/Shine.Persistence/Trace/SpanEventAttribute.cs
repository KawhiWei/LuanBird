using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public class SpanEventAttribute(string key, AttributeValueType valueType, string value, string traceId, string spanId, int spanEventIndex) : AbstractAttribute(key, valueType, value)
{
    public required string TraceId { get; init; } = traceId;

    public required string SpanId { get; init; } = spanId;

    public required int SpanEventIndex { get; init; } = spanEventIndex;
}