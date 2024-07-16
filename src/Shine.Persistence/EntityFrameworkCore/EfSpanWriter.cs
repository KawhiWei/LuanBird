using Microsoft.EntityFrameworkCore;
using Shine.Domain.Repositories;
using Shine.Dto;
using Shine.Persistence.Trace;

namespace Shine.Persistence.EntityFrameworkCore;

public class EfSpanWriter : ISpanWriter
{
    private readonly IDbContextFactory<ShineContext> _dbContextFactory;

    public EfSpanWriter(IDbContextFactory<ShineContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task WriteAsync(IEnumerable<ShineSpan> shineSpans)
    {
        var spans = new List<Span>();
        var spanAttributes = new List<SpanAttribute>();
        var resourceAttributes = new List<ResourceAttribute>();
        var spanEvents = new List<SpanEvent>();
        var spanEventAttributes = new List<SpanEventAttribute>();
        var spanLinks = new List<SpanLink>();
        var spanLinkAttributes = new List<SpanLinkAttribute>();
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.Spans.AddRangeAsync(spans);
        await context.SpanAttributes.AddRangeAsync(spanAttributes);
        await context.ResourceAttributes.AddRangeAsync(resourceAttributes);
        await context.SpanEvents.AddRangeAsync(spanEvents);
        await context.SpanEventAttributes.AddRangeAsync(spanEventAttributes);
        await context.SpanLinks.AddRangeAsync(spanLinks);
        await context.SpanLinkAttributes.AddRangeAsync(spanLinkAttributes);
        await context.SaveChangesAsync();
    }
}