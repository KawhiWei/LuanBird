using Luck.DDD.Domain.Domain.Entities;

namespace Shine.Domain.AggregateRoots.Trace;

public class ShineSpanLink(
    string traceId,
    string spanId,
    int index,
    string linkedTraceId,
    string linkedSpanId,
    string linkedTraceState,
    int linkedTraceFlags)
    : FullEntity
{
    public required string TraceId { get; init; } = traceId;

    public required string SpanId { get; init; } = spanId;

    public required int Index { get; init; } = index;

    public required string LinkedTraceId { get; init; } = linkedTraceId;

    public required string LinkedSpanId { get; init; } = linkedSpanId;

    public required string LinkedTraceState { get; init; } = linkedTraceState;

    public required int LinkedTraceFlags { get; init; } = linkedTraceFlags;
}