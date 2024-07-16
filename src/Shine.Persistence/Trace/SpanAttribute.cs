using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public class SpanAttribute()
    : AbstractAttribute
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }
}