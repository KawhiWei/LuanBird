namespace Shine.Dto;

public class ShineSpanEvent
{
    public  string Name { get; init; }

    public  IEnumerable<ShineAttribute> Attributes { get; init; }

    public ulong TimestampUnixNano { get; init; }
}