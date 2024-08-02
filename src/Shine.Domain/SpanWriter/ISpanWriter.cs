using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Shine.Dto;

namespace Shine.Domain.SpanWriter;

public interface ISpanWriter : IScopedDependency
{
    Task WriteAsync(IEnumerable<ShineSpan> shineSpans);
}