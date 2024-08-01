using Shine.Domain.Shared.Enums;

namespace Shine.Dto;

public class ShineAttribute
{
    public  string Key { get; init; }

    public AttributeValueType ValueType { get; init; }

    public  string Value { get; init; }
}