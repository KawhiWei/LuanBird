using Shine.Dto;

namespace Shine.Domain.JaegerTraceReader;

public interface IJaegerSpanReader 
{
    Task<IEnumerable<string>> GetServicesAsync();


    Task<IEnumerable<string>> GetOperationsAsync(string serviceName);

    Task<IEnumerable<JaegerTraceOutputDto>> FindTracesAsync(JaegerTraceQueryParameters query);


    Task<IEnumerable<JaegerTraceOutputDto>> FindTracesByTraceIdAsync(string[] traceIds, ulong? startTimeUnixNano = null,
        ulong? endTimeUnixNano = null);
}