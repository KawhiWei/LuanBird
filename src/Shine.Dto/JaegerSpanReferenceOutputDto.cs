namespace Shine.Dto;

public class JaegerSpanReferenceOutputDto
{
    public required string TraceId { get; init; }
    public required string SpanId { get; init; }
    public required string RefType { get; init; }
}