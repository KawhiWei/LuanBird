namespace Shine.Dto;

public class MochaResource
{
    public  string ServiceName { get; init; }

    public  string ServiceInstanceId { get; init; }

    public  IEnumerable<ShineAttribute> Attributes { get; init; }
}