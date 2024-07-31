using Luck.DDD.Domain.Domain.Entities;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanEvent()
    : FullEntity
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }

    public required  int Index { get; init; }

    public required string Name { get; init; }

    public required  ulong TimestampUnixNano { get; init; }
}