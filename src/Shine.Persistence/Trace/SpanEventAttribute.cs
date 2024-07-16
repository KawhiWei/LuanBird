using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public class SpanEventAttribute() : AbstractAttribute
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }

    public required int SpanEventIndex { get; init; }
}