using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.Framework.Extensions;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanEvent : FullAggregateRoot
{
    public SpanEvent(string traceId, string spanId, int index, string name, ulong timestampUnixNano)
    {
        TraceId = traceId;
        SpanId = spanId;
        Index = index;
        Name = name;
        TimestampUnixNano = timestampUnixNano;
    }

    public string TraceId { get; private set; }

    public string SpanId { get; private set; }

    public int Index { get; private set; }

    public string Name { get; private set; }

    public ulong TimestampUnixNano { get; private set; }

    public ICollection<SpanEventAttribute> SpanEventAttributes { get; private set; } = new List<SpanEventAttribute>();

    public void SetSpanEventAttributes(IEnumerable<SpanEventAttribute> spanEventAttributes)
    {
        SpanEventAttributes.AddRange(spanEventAttributes);
    }
}