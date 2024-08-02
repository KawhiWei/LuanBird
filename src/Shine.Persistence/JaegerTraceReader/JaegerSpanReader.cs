using Luck.AutoDependencyInjection;
using Luck.DDD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shine.Domain.AggregateRoots.Trace;
using Shine.Domain.JaegerTraceReader;
using Shine.Dto;

namespace Shine.Persistence.JaegerTraceReader;

[DependencyInjection(ServiceLifetime.Scoped)]
public class JaegerSpanReader : IJaegerSpanReader
{
    private readonly IAggregateRootRepository<Span, string> _spanWriterRepository;

    private readonly IAggregateRootRepository<SpanLink, string> _spanLinkRepository;
    private readonly IAggregateRootRepository<SpanEvent, string> _spanEventRepository;

    public JaegerSpanReader(
        IAggregateRootRepository<Span, string> spanWriterRepository,
        IAggregateRootRepository<SpanLink, string> spanLinkRepository,
        IAggregateRootRepository<SpanEvent, string> spanEventRepository)
    {
        _spanWriterRepository = spanWriterRepository;
        _spanLinkRepository = spanLinkRepository;
        _spanEventRepository = spanEventRepository;
    }

    public async Task<IEnumerable<string>> GetServicesAsync()
    {
        return await _spanWriterRepository.FindAll().Select(x => x.ServiceName).Distinct().ToListAsync();
    }

    public async Task<IEnumerable<string>> GetOperationsAsync(string serviceName)
    {
        var operationList = await _spanWriterRepository.FindAll()
            .Where(x => x.ServiceName == serviceName)
            .Select(x => x.SpanName)
            .Distinct()
            .ToListAsync();
        return operationList;
    }

    public Task<IEnumerable<JaegerTraceOutputDto>> FindTracesAsync(JaegerTraceQueryParameters query)
    {
        throw new NotImplementedException();
    }
}