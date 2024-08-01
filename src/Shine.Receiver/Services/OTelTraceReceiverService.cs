using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Shine.Domain.SpanWriter;
using Shine.Receiver.Extensions;

namespace Shine.Receiver.Services;

public class OTelTraceReceiverService
    : TraceService.TraceServiceBase
{
    private readonly ILogger<OTelTraceReceiverService> _logger;
    private readonly ISpanWriter _spanWriter;

    public OTelTraceReceiverService(ILogger<OTelTraceReceiverService> logger, ISpanWriter spanWriter)
    {
        _logger = logger;
        _spanWriter = spanWriter;
    }

    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request,
        ServerCallContext context)
    {
        var shineSpans = request.ResourceSpans
            .SelectMany(resourceSpans => resourceSpans.ScopeSpans
                .SelectMany(scopeSpans => scopeSpans.Spans.Select(span => span.ToShineSpan(resourceSpans.Resource))))
            .ToList();
        // logger.LogInformation($"提交的请求数据：【{shineSpans.Serialize()}】");
        await _spanWriter.WriteAsync(shineSpans);
        return new ExportTraceServiceResponse();
    }
}