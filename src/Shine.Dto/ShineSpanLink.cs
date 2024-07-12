namespace Shine.Dto;

public class ShineSpanLink
{
    public required string LinkedTraceId { get; init; }

    public required string LinkedSpanId { get; init; }

    public required IEnumerable<ShineAttribute> Attributes { get; init; }

    public required string LinkedTraceState { get; init; }

    public uint LinkedTraceFlags { get; init; }
}