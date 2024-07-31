using Luck.DDD.Domain.Domain.Entities;
using Shine.Domain.Shared.Enums;

namespace Shine.Domain.AggregateRoots.Trace;

public abstract class AbstractAttribute: FullEntity
{
    protected AbstractAttribute(string key, AttributeValueType valueType, string value)
    {
        Key = key;
        ValueType = valueType;
        Value = value;
    }

    public  string Key { get; private set; } 

    public  AttributeValueType ValueType { get; private set; } 

    public  string Value { get; private set; } 
}