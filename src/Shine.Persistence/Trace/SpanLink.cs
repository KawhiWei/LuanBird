using Luck.DDD.Domain.Domain.Entities;

namespace Shine.Persistence.Trace;

public class SpanLink : FullEntity
{
    public required string TraceId { get; init; } 

    public required string SpanId { get; init; } 

    public required int Index { get; init; } 

    public required string LinkedTraceId { get; init; }

    public required string LinkedSpanId { get; init; } 

    public required string LinkedTraceState { get; init; }

    public required uint LinkedTraceFlags { get; init; }
}