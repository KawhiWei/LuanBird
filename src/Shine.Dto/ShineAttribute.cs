using Shine.Domain.Shared.Enums;

namespace Shine.Dto;

public class ShineAttribute
{
    public required string Key { get; init; }

    public AttributeValueType ValueType { get; init; }

    public required string Value { get; init; }
}