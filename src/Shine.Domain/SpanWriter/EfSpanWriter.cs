using Luck.DDD.Domain.Repositories;
using Luck.Framework.UnitOfWorks;
using Shine.Domain.AggregateRoots.Trace;
using Shine.Domain.Extensions;
using Shine.Dto;

namespace Shine.Domain.SpanWriter;

public class EfSpanWriter : ISpanWriter
{
    private readonly IAggregateRootRepository<Span, string> _spanWriterRepository;

    private readonly IAggregateRootRepository<SpanLink, string> _spanLinkRepository;
    private readonly IAggregateRootRepository<SpanEvent, string> _spanEventRepository;

    private readonly IUnitOfWork _unitOfWork;

    public EfSpanWriter(IUnitOfWork unitOfWork, IAggregateRootRepository<Span, string> spanWriterRepository,
        IAggregateRootRepository<SpanLink, string> spanLinkRepository,
        IAggregateRootRepository<SpanEvent, string> spanEventRepository)
    {
        _unitOfWork = unitOfWork;
        _spanWriterRepository = spanWriterRepository;
        _spanLinkRepository = spanLinkRepository;
        _spanEventRepository = spanEventRepository;
    }

    public async Task WriteAsync(IEnumerable<ShineSpan> shineSpans)
    {
        foreach (var shineSpan in shineSpans)
        {
            var span = shineSpan.ToSpan();
            _spanWriterRepository.Add(span);
            var linksArray = shineSpan.Links.ToArray();
            for (var i = 0; i < linksArray.Length; i++)
            {
                var spanLink = linksArray[i].ToSpanLink(shineSpan, i);
                _spanLinkRepository.Add(spanLink);
            }

            var eventsArray = shineSpan.Events.ToArray();
            for (var i = 0; i < eventsArray.Length; i++)
            {
                var spanEvent = eventsArray[i].ToSpanEvent(shineSpan, i);
                _spanEventRepository.Add(spanEvent);
            }
        }

        await _unitOfWork.CommitAsync();
    }
}