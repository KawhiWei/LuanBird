using Shine.Domain.Shared.Enums;

namespace Shine.Dto;

public class ShineSpan
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }

    public required string SpanName { get; init; }

    public required string ParentSpanId { get; init; }

    public required ulong StartTimeUnixNano { get; init; }

    public required ulong EndTimeUnixNano { get; init; }

    public required ulong DurationNanoseconds { get; init; }

    public SpanStatusCode? StatusCode { get; init; }

    public string? StatusMessage { get; init; }

    public SpanKind SpanKind { get; init; }

    public required string ServiceName { get; init; }

    public required string ServiceInstanceId { get; init; }

    public uint TraceFlags { get; init; }

    public string? TraceState { get; init; }
}