using Shine.Dto;

namespace Shine.Domain.Repositories;

public interface ISpanWriter
{
    Task WriteAsync(IEnumerable<ShineSpan> shineSpans);
}