using Microsoft.AspNetCore.Mvc;
using Shine.Domain.JaegerTraceReader;
using Shine.Dto;

namespace Shine.Query.Controllers;

[ApiController]
[Route("/jaeger/api")]
public class JaegerTraceController : ControllerBase
{
    private readonly IJaegerSpanReader _jaegerSpanReader;

    public JaegerTraceController(IJaegerSpanReader jaegerSpanReader)
    {
        _jaegerSpanReader = jaegerSpanReader;
    }

    [HttpGet("services")]
    public async Task<JaegerResponse<IEnumerable<string>>> GetSeries()
    {
        return new(await _jaegerSpanReader.GetServicesAsync());
    }

    [HttpGet("services/{serviceName}/operations")]
    public async Task<JaegerResponse<IEnumerable<string>>> GetOperations(string serviceName)
    {
        return new(await _jaegerSpanReader.GetOperationsAsync(serviceName));
    }
}