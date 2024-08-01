using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.Framework.Extensions;

namespace Shine.Domain.AggregateRoots.Trace;

public class SpanLink : FullAggregateRoot
{
    public SpanLink(string traceId, string spanId, int index, string linkedTraceId, string linkedSpanId,
        string linkedTraceState, uint linkedTraceFlags)
    {
        TraceId = traceId;
        SpanId = spanId;
        Index = index;
        LinkedTraceId = linkedTraceId;
        LinkedSpanId = linkedSpanId;
        LinkedTraceState = linkedTraceState;
        LinkedTraceFlags = linkedTraceFlags;
    }

    public string TraceId { get; private set; }

    public string SpanId { get; private set; }

    public int Index { get; private set; }

    public string LinkedTraceId { get; private set; }

    public string LinkedSpanId { get; private set; }

    public string LinkedTraceState { get; private set; }

    public uint LinkedTraceFlags { get; private set; }

    public ICollection<SpanLinkAttribute> SpanLinkAttributes { get; private set; } = new List<SpanLinkAttribute>();

    public void SetSpanLinkAttributes(IEnumerable<SpanLinkAttribute> spanLinkAttributes)
    {
        SpanLinkAttributes.AddRange(spanLinkAttributes);
    }
}