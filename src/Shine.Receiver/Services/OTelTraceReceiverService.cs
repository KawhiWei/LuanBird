using Grpc.Core;
using Luck.Framework.Extensions;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Shine.Domain.SpanWriter;
using Shine.Receiver.Extensions;

namespace Shine.Receiver.Services;

public class OTelTraceReceiverService(ILogger<OTelTraceReceiverService> logger, ISpanWriter spanWriter)
    : TraceService.TraceServiceBase
{
    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request,
        ServerCallContext context)
    {
        var shineSpans = request.ResourceSpans
            .SelectMany(resourceSpans => resourceSpans.ScopeSpans
                .SelectMany(scopeSpans => scopeSpans.Spans.Select(span => span.ToShineSpan(resourceSpans.Resource))))
            .ToList();
        // logger.LogInformation($"提交的请求数据：【{shineSpans.Serialize()}】");
        await spanWriter.WriteAsync(shineSpans);
        return new ExportTraceServiceResponse();
    }
}