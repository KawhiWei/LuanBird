using Luck.DDD.Domain.Domain.Entities;
using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public abstract class AbstractAttribute(string key, AttributeValueType valueType, string value) : FullEntity
{
    public required string Key { get; init; } = key;

    public required AttributeValueType ValueType { get; init; } = valueType;

    public required string Value { get; init; } = value;
}