using Shine.Domain.Shared.Enums;
using Shine.Dto;

namespace Shine.Persistence.Trace;

internal static class ShineSpanToSpanConversionExtensions
{
    internal static Span ToSpan(this ShineSpan shineSpan)
    {
        var span = new Span(
            shineSpan.TraceId,
            shineSpan.SpanId,
            shineSpan.SpanName,
            shineSpan.ParentSpanId,
            shineSpan.StartTimeUnixNano,
            shineSpan.EndTimeUnixNano,
            shineSpan.DurationNanoseconds,
            shineSpan.StatusCode,
            shineSpan.StatusMessage,
            shineSpan.SpanKind,
            shineSpan.Resource.ServiceName,
            shineSpan.Resource.ServiceInstanceId,
            shineSpan.TraceFlags,
            shineSpan.TraceState
        )
        {
            TraceId = null,
            SpanId = null,
            SpanName = null,
            ParentSpanId = null,
            StartTimeUnixNano = 0,
            EndTimeUnixNano = 0,
            DurationNanoseconds = 0,
            StatusCode = null,
            StatusMessage = null,
            SpanKind = SpanKind.Unspecified,
            ServiceName = null,
            ServiceInstanceId = null,
            TraceFlags = 0,
            TraceState = null
        };

        return span;
    }
}