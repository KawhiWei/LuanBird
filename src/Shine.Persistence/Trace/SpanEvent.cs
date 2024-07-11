using Luck.DDD.Domain.Domain.Entities;

namespace Shine.Persistence.Trace;

public class SpanEvent(string traceId, string spanId, int index, string name, ulong timestampUnixNano)
    : FullEntity
{
    public required string TraceId { get; init; } = traceId;

    public required string SpanId { get; init; } = spanId;

    public required  int Index { get; init; } = index;

    public required string Name { get; init; } = name;

    public required  ulong TimestampUnixNano { get; init; } = timestampUnixNano;
}