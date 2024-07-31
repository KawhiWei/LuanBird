using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;

namespace Shine.Domain.Repositories;

public interface ISpanWriterRepository : IAggregateRootRepository<Shine.Domain.AggregateRoots.Trace.Span, string>,
    IScopedDependency
{
}