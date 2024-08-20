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

    public async Task<IEnumerable<JaegerTraceOutputDto>> FindTracesAsync(JaegerTraceQueryParameters query)
    {
        var queryableSpans = _spanRepository.FindAll();

        if (!string.IsNullOrEmpty(query.ServiceName))
        {
            queryableSpans = queryableSpans.Where(s => s.ServiceName == query.ServiceName);
        }

        if (!string.IsNullOrEmpty(query.OperationName))
        {
            queryableSpans = queryableSpans.Where(s => s.SpanName == query.OperationName);
        }

        if (query.StartTimeMinUnixNano.HasValue)
        {
            queryableSpans = queryableSpans.Where(s => s.StartTimeUnixNano >= query.StartTimeMinUnixNano.Value);
        }

        if (query.StartTimeMaxUnixNano.HasValue)
        {
            queryableSpans = queryableSpans.Where(s => s.StartTimeUnixNano <= query.StartTimeMaxUnixNano.Value);
        }

        if (query.DurationMinNanoseconds.HasValue)
        {
            queryableSpans =
                queryableSpans.Where(s => s.DurationNanoseconds >= query.DurationMinNanoseconds.Value);
        }

        if (query.DurationMaxNanoseconds.HasValue)
        {
            queryableSpans =
                queryableSpans.Where(s => s.DurationNanoseconds <= query.DurationMaxNanoseconds.Value);
        }

        if (query.Tags?.Any() ?? false)
        {
            // TODO: This is a hacky way to do this, but it works for now. We should find a better way to match tags.
            var tags = query.Tags.Select(tag => $"{tag.Key}:{tag.Value}").ToHashSet();
            var queryableAttributes =
                _spanAttributeRepository.FindAll()
                    .Where(a => tags.Contains(a.Key + ":" + a.Value));

            var spanIds = queryableAttributes.GroupBy(a => a.SpanId)
                .Where(a => a.Count() == query.Tags.Count())
                .Select(a => a.Key);

            queryableSpans = from span in queryableSpans
                join spanId in spanIds on span.SpanId equals spanId
                select span;
        }

        if (query.NumTraces > 0)
        {
            queryableSpans = queryableSpans
                .OrderByDescending(s => s.Id)
                .Take(query.NumTraces);
        }

        return await QueryJaegerTracesAsync(queryableSpans);
    }

    public async Task<IEnumerable<JaegerTraceOutputDto>> FindTracesByTraceIdAsync(
        string[] traceIds, ulong? startTimeUnixNano = null,
        ulong? endTimeUnixNano = null)
    {
        var queryableSpans = _spanRepository.FindAll();

        if (traceIds?.Any() ?? false)
        {
            queryableSpans = queryableSpans.Where(s => traceIds.Contains(s.TraceId));
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