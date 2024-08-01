namespace Shine.Dto;

public class ShineSpanLink
{
    public  string LinkedTraceId { get; init; }

    public  string LinkedSpanId { get; init; }

    public  IEnumerable<ShineAttribute> Attributes { get; init; }

    public  string LinkedTraceState { get; init; }

    public uint LinkedTraceFlags { get; init; }
}