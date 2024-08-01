using Shine.Domain.AggregateRoots.Trace;
using Shine.Dto;

namespace Shine.Domain.Extensions;

internal static class ShineSpanToSpanConversionExtensions
{
    internal static Span ToSpan(this ShineSpan shineSpan)
    {
        var span = new Span(shineSpan.TraceId, shineSpan.SpanId, shineSpan.SpanName, shineSpan.ParentSpanId,
            shineSpan.StartTimeUnixNano, shineSpan.EndTimeUnixNano, shineSpan.DurationNanoseconds, shineSpan.StatusCode,
            shineSpan.StatusMessage, shineSpan.SpanKind, shineSpan.Resource.ServiceName,
            shineSpan.Resource.ServiceInstanceId, shineSpan.TraceFlags, shineSpan.TraceState);
        span.SetSpanAttributes(shineSpan.ToSpanAttributes());
        span.SetResourceAttributes(shineSpan.ToResourceAttributes());
        return span;
    }

    internal static IEnumerable<SpanAttribute> ToSpanAttributes(this ShineSpan shineSpan)
    {
        return shineSpan.Attributes.Select(a =>
            new SpanAttribute(shineSpan.TraceId, shineSpan.SpanId, a.Key, a.ValueType, a.Value));
    }

    internal static IEnumerable<ResourceAttribute> ToResourceAttributes(this ShineSpan shineSpan)
    {
        return shineSpan.Resource.Attributes.Select(a =>
            new ResourceAttribute(shineSpan.TraceId, shineSpan.SpanId, a.Key, a.ValueType, a.Value));
    }

    internal static SpanEvent ToSpanEvent(this ShineSpanEvent shineSpanEvent, ShineSpan span, int spanEventIndex)
    {
        var spanEvent = new SpanEvent(span.TraceId, span.SpanId, spanEventIndex, shineSpanEvent.Name,
            shineSpanEvent.TimestampUnixNano);
        spanEvent.SetSpanEventAttributes(shineSpanEvent.ToSpanEventAttributes(spanEvent));

        return spanEvent;
    }

    internal static IEnumerable<SpanEventAttribute> ToSpanEventAttributes(this ShineSpanEvent shineSpanEvent,
        SpanEvent spanEvent)
    {
        return shineSpanEvent.Attributes.Select(a => new
            SpanEventAttribute(spanEvent.TraceId, spanEvent.SpanId, spanEvent.Index, a.Key, a.ValueType, a.Value));
    }

    internal static SpanLink ToSpanLink(this ShineSpanLink link, ShineSpan shineSpan, int spanLinkIndex)
    {
        var spanLink = new SpanLink(shineSpan.TraceId, shineSpan.SpanId, spanLinkIndex, link.LinkedTraceId,
            link.LinkedSpanId, link.LinkedTraceState, link.LinkedTraceFlags);
        spanLink.SetSpanLinkAttributes(link.ToSpanLinkAttributes(spanLink));
        return spanLink;
    }

    internal static IEnumerable<SpanLinkAttribute> ToSpanLinkAttributes(
        this ShineSpanLink shineSpanLink,
        SpanLink spanLink)
    {
        return shineSpanLink.Attributes.Select(a =>
            new SpanLinkAttribute(spanLink.TraceId, spanLink.SpanId, spanLink.Index, a.Key, a.ValueType, a.Value));
    }
}