using Luck.AutoDependencyInjection;
using Luck.DDD.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shine.Domain.AggregateRoots.Trace;
using Shine.Domain.JaegerTraceReader;
using Shine.Dto;
using Shine.Persistence.Extensions;

namespace Shine.Persistence.JaegerTraceReader;

[DependencyInjection(ServiceLifetime.Scoped)]
public class JaegerSpanReader : IJaegerSpanReader
{
    private readonly IAggregateRootRepository<Span, string> _spanRepository;

    private readonly IAggregateRootRepository<SpanLink, string> _spanLinkRepository;
    private readonly IAggregateRootRepository<SpanEvent, string> _spanEventRepository;
    private readonly IEntityRepository<SpanAttribute, string> _spanAttributeRepository;
    private readonly IEntityRepository<ResourceAttribute, string> _resourceAttributeRepository;
    private readonly IEntityRepository<SpanEventAttribute, string> _spanEventAttributeRepository;

    public JaegerSpanReader(
        IAggregateRootRepository<Span, string> spanRepository,
        IAggregateRootRepository<SpanLink, string> spanLinkRepository,
        IAggregateRootRepository<SpanEvent, string> spanEventRepository,
        IEntityRepository<SpanAttribute, string> spanAttributeRepository,
        IEntityRepository<ResourceAttribute, string> resourceAttributeRepository,
        IEntityRepository<SpanEventAttribute, string> spanEventAttributeRepository)
    {
        _spanRepository = spanRepository;
        _spanLinkRepository = spanLinkRepository;
        _spanEventRepository = spanEventRepository;
        _spanAttributeRepository = spanAttributeRepository;
        _resourceAttributeRepository = resourceAttributeRepository;
        _spanEventAttributeRepository = spanEventAttributeRepository;
    }

    public async Task<IEnumerable<string>> GetServicesAsync()
    {
        return await _spanRepository.FindAll().Select(x => x.ServiceName).Distinct().ToListAsync();
    }

    public async Task<IEnumerable<string>> GetOperationsAsync(string serviceName)
    {
        var operationList = await _spanRepository.FindAll()
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

    public async Task<IEnumerable<JaegerTraceOutputDto>> FindTracesAsync(
        string[]? traceIDs,
        ulong? startTimeUnixNano,
        ulong? endTimeUnixNano)
    {
        var queryableSpans = _spanRepository.FindAll();

        if (traceIDs?.Any() ?? false)
        {
            queryableSpans = queryableSpans.Where(s => traceIDs.Contains(s.TraceId));
        }

        if (startTimeUnixNano.HasValue)
        {
            queryableSpans = queryableSpans.Where(s => s.StartTimeUnixNano >= startTimeUnixNano.Value);
        }

        if (endTimeUnixNano.HasValue)
        {
            queryableSpans = queryableSpans.Where(s => s.StartTimeUnixNano <= endTimeUnixNano.Value);
        }

        return await QueryJaegerTracesAsync(queryableSpans);
    }

    private async Task<IEnumerable<JaegerTraceOutputDto>> QueryJaegerTracesAsync(
        IQueryable<Span> queryableSpans)
    {
        var spans = await queryableSpans.ToListAsync();

        var spanIds = spans.Select(s => s.SpanId).ToArray();

        var spanAttributes = await _spanAttributeRepository.FindAll()
            .Where(a => spanIds.Contains(a.SpanId))
            .ToListAsync();

        var resourceAttributes = await _resourceAttributeRepository.FindAll()
            .Where(a => spanIds.Contains(a.SpanId))
            .ToListAsync();

        var spanEvents = await _spanEventRepository.FindAll()
            .Where(e => spanIds.Contains(e.SpanId))
            .ToListAsync();

        var spanEventAttributes = await _spanEventAttributeRepository.FindAll()
            .Where(a => spanIds.Contains(a.SpanId))
            .ToListAsync();

        var jaegerTraces = spans.ToJaegerTraces(
            spanAttributes, resourceAttributes, spanEvents, spanEventAttributes).ToArray();

        return jaegerTraces;
    }
}