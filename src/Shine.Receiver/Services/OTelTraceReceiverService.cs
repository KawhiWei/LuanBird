using Grpc.Core;
using OpenTelemetry.Proto.Collector.Trace.V1;
using Shine.Dto.Extensions;

namespace Shine.Receiver.Services;

public class OTelTraceReceiverService : TraceService.TraceServiceBase
{
    public override async Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request,
        ServerCallContext context)
    {
        await Task.CompletedTask;
        var shineSpans = request.ResourceSpans
            .SelectMany(resourceSpans => resourceSpans.ScopeSpans
                .SelectMany(scopeSpans => scopeSpans.Spans.Select(span => span.ToShineSpan(resourceSpans.Resource))));
        return new ExportTraceServiceResponse();
    }
}