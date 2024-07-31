using Luck.EntityFrameworkCore.Repositories;
using Luck.Framework.UnitOfWorks;
using Shine.Domain.Repositories;

namespace Shine.Persistence.EntityFrameworkCore;


public class EfSpanWriterRepositoryRepository : EfCoreAggregateRootRepository<Domain.AggregateRoots.Trace.Span, string>,
    ISpanWriterRepository
{
    public EfSpanWriterRepositoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}