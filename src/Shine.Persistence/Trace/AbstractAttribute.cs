using Luck.DDD.Domain.Domain.Entities;
using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public abstract class AbstractAttribute() : FullEntity
{
    public required string Key { get; init; } 

    public required AttributeValueType ValueType { get; init; } 

    public required string Value { get; init; } 
}