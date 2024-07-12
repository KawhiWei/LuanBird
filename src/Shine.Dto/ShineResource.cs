namespace Shine.Dto;

public class ShineResource
{
    public required string ServiceName { get; init; }

    public required string ServiceInstanceId { get; init; }

    public required IEnumerable<ShineAttribute> Attributes { get; init; }
}