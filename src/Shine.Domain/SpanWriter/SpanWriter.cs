using Luck.Framework.UnitOfWorks;
using Shine.Domain.Extensions;
using Shine.Domain.Repositories;
using Shine.Dto;

namespace Shine.Domain.SpanWriter;

public class SpanWriter : ISpanWriter
{
    private readonly ISpanWriterRepository _spanWriterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SpanWriter(ISpanWriterRepository spanWriterRepository, IUnitOfWork unitOfWork)
    {
        _spanWriterRepository = spanWriterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task WriteAsync(IEnumerable<ShineSpan> shineSpans)
    {
        foreach (var shineSpan in shineSpans)
        {
            var span = shineSpan.ToSpan();
            _spanWriterRepository.Add(span);
        }

        await _unitOfWork.CommitAsync();
    }
}