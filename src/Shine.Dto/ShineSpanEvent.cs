namespace Shine.Dto;

public class ShineSpanEvent
{
    public required string Name { get; init; }

    public required IEnumerable<ShineAttribute> Attributes { get; init; }

    public ulong TimestampUnixNano { get; init; }
}