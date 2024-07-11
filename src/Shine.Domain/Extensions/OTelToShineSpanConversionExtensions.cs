using Shine.Dto;
using System.Buffers.Binary;
using Google.Protobuf;
using OpenTelemetry.Proto.Resource.V1;
using OpenTelemetry.Proto.Trace.V1;

namespace Shine.Domain.Extensions;

public static class OTelToShineSpanConversionExtensions
{
    public static ShineSpan ToShineSpan(this Span span, Resource resource)
    {
        return new ShineSpan();
    }

    private static string ConvertByteStringToSpanId(ByteString byteString)
    {
        return byteString.Length == 0 ? string.Empty : ConvertBytesToLong(byteString.Span).ToString("x016");
    }

    private static string ConvertByteStringToTraceId(ByteString byteString)
    {
        if (byteString.Length == 0)
        {
            return string.Empty;
        }

        var high = ConvertBytesToLong(byteString.Span[..8]);
        var low = ConvertBytesToLong(byteString.Span[8..16]);
        return high == 0 ? low.ToString("x016") : $"{high:x016}{low:x016}";
    }

    private static long ConvertBytesToLong(ReadOnlySpan<byte> bytes) =>
        BitConverter.IsLittleEndian
            ? BinaryPrimitives.ReadInt64BigEndian(bytes)
            : BinaryPrimitives.ReadInt64LittleEndian(bytes);
}