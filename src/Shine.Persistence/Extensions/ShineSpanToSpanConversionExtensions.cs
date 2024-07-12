using OpenTelemetry.Proto.Trace.V1;
using Shine.Dto;

namespace Shine.Persistence.Extensions;

public static class ShineSpanToSpanConversionExtensions
{
    public static Span ToShineSpan(this ShineSpan shineSpan)
    {
        return new Span();
    }
}