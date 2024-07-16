using Shine.Domain.Shared.Enums;

namespace Shine.Persistence.Trace;

public class SpanLinkAttribute() : AbstractAttribute
{
    public required string TraceId { get; init; }

    public required string SpanId { get; init; }

    public required int SpanLinkIndex { get; init; }
}