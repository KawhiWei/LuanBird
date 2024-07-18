using Grpc.Core;
using Luck.Framework.Extensions;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Shine.Receiver.Extensions;

namespace Shine.Receiver.Services;

public class OTelTraceReceiverService(ILogger<OTelTraceReceiverService> logger) : TraceService.TraceServiceBase
{
    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request,
        ServerCallContext context)
    {
        await Task.CompletedTask;
        var shineSpans = request.ResourceSpans
            .SelectMany(resourceSpans => resourceSpans.ScopeSpans
                .SelectMany(scopeSpans => scopeSpans.Spans.Select(span => span.ToShineSpan(resourceSpans.Resource))));
        logger.LogInformation($"提交的请求数据：【{shineSpans.Serialize()}】");
        return new ExportTraceServiceResponse();
    }
}