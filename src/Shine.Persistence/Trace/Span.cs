using Luck.DDD.Domain.Domain.AggregateRoots;
using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public class Span : FullAggregateRoot
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }

    public required string SpanName { get; init; }

    public required string ParentSpanId { get; init; }

    public required ulong StartTimeUnixNano { get; init; }

    public required ulong EndTimeUnixNano { get; init; }

    public required ulong DurationNanoseconds { get; init; }

    public required SpanStatusCode? StatusCode { get; init; }

    public required string? StatusMessage { get; init; }

    public required SpanKind SpanKind { get; init; }

    public required string ServiceName { get; init; }

    public required string ServiceInstanceId { get; init; }

    public required uint TraceFlags { get; init; }

    public required string? TraceState { get; init; }
}