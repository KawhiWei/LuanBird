using Shine.Domain.Shared.Enums;

namespace Shine.Dto;

public class ShineSpan
{
    public  string TraceId { get; init; }

    public  string SpanId { get; init; }

    public  string SpanName { get; init; }

    public  string ParentSpanId { get; init; }

    public  ulong StartTimeUnixNano { get; init; }

    public  ulong EndTimeUnixNano { get; init; }

    public  ulong DurationNanoseconds { get; init; }

    public SpanStatusCode? StatusCode { get; init; }

    public string? StatusMessage { get; init; }

    public SpanKind SpanKind { get; init; }
    
    public  ShineResource Resource { get; init; }
    public uint TraceFlags { get; init; }

    public string? TraceState { get; init; }
    
    public  IEnumerable<ShineSpanLink> Links { get; init; }

    public  IEnumerable<ShineAttribute> Attributes { get; init; }

    public  IEnumerable<ShineSpanEvent> Events { get; init; }
    
}