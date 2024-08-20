namespace Shine.Dto;

public class JaegerSpanOutputDto
{
    public required string TraceID { get; init; }

    public required string SpanID { get; init; }

    public required string OperationName { get; init; }

    public uint Flags { get; init; }

    public ulong StartTime { get; init; }

    public ulong Duration { get; init; }

    public required string ProcessID { get; init; }

    public required JaegerSpanReferenceOutputDto[] References { get; init; }

    public required JaegerTagOutputDto[] Tags { get; init; }

    public required JaegerSpanLogOutputDto[] Logs { get; init; }
}