using System.Net;
using System.Text.RegularExpressions;
using Luck.Framework.Extensions;
using Microsoft.AspNetCore.Mvc;
using Shine.Domain.JaegerTraceReader;
using Shine.Dto;
using Shine.Infrastructure.Extensions;

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

    [HttpGet("traces")]
    public async Task<JaegerResponse<IEnumerable<JaegerTraceOutputDto>>> FindTraces(
        [FromQuery] FindTracesRequest request)
    {
        static ulong? ParseAsNanoseconds(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            var m = Regex.Match(input,
                @"^((?<days>\d+)d)?((?<hours>\d+)h)?((?<minutes>\d+)m)?((?<seconds>\d+)s)?((?<milliseconds>\d+)ms)?((?<microseconds>\d+)μs)?$",
                RegexOptions.ExplicitCapture
                | RegexOptions.Compiled
                | RegexOptions.CultureInvariant
                | RegexOptions.RightToLeft);

            if (!m.Success)
            {
                return null;
            }

            var days = m.Groups["days"].Success ? long.Parse(m.Groups["days"].Value) : 0;
            var hours = m.Groups["hours"].Success ? long.Parse(m.Groups["hours"].Value) : 0;
            var minutes = m.Groups["minutes"].Success ? long.Parse(m.Groups["minutes"].Value) : 0;
            var seconds = m.Groups["seconds"].Success ? long.Parse(m.Groups["seconds"].Value) : 0;
            var milliseconds = m.Groups["milliseconds"].Success ? long.Parse(m.Groups["milliseconds"].Value) : 0;
            var microseconds = m.Groups["microseconds"].Success ? long.Parse(m.Groups["microseconds"].Value) : 0;

            return
                (ulong)(((days * 24 * 60 * 60 + hours * 60 * 60 + minutes * 60 + seconds) * 1000 + milliseconds)
                    * 1000 + microseconds) * 1000;
        }

        var startTimeMin = request.Start * 1000;


        var startTimeMax = request.End * 1000;

        var lookBack = ParseAsNanoseconds(request.LookBack);

        if (lookBack.HasValue)
        {
            var now = DateTimeOffset.Now.ToUnixTimeNanoseconds();
            startTimeMin = now - lookBack.Value;
            startTimeMax = now;
        }

        List<JaegerTraceOutputDto> traces;

        if (request.TraceId?.Any() ?? false)
        {
            traces = (await _jaegerSpanReader.FindTracesByTraceIdAsync(request.TraceId, startTimeMin, startTimeMax)).ToList();
        }
        else
        {
            traces = (await _jaegerSpanReader.FindTracesAsync(new JaegerTraceQueryParameters
            {
                ServiceName = request.Service,
                OperationName = request.Operation,
                Tags = (request.Tags ?? "{}").Deserialize<Dictionary<string, object>>()!,
                StartTimeMinUnixNano = startTimeMin,
                StartTimeMaxUnixNano = startTimeMax,
                DurationMinNanoseconds =
                    string.IsNullOrWhiteSpace(request.MinDuration)
                        ? null
                        : ParseAsNanoseconds(request.MinDuration)!,
                DurationMaxNanoseconds =
                    string.IsNullOrWhiteSpace(request.MaxDuration)
                        ? null
                        : ParseAsNanoseconds(request.MaxDuration)!,
                NumTraces = request.Limit
            })).ToList();
        }

        JaegerResponseError? error = null;
        if (traces.Any() is false)
        {
            error = new JaegerResponseError { Code = (int)HttpStatusCode.NotFound, Message = "trace not found" };
        }

        return new JaegerResponse<IEnumerable<JaegerTraceOutputDto>>(traces) { Error = error };
    }
    
    
    [HttpGet("traces/{traceId}")]
    public async Task<JaegerResponse<IEnumerable<JaegerTraceOutputDto>>> GetTrace(string traceId)
    {
        var tranceIdArray = new List<string>()
        {
            traceId
        };
        var traces = await _jaegerSpanReader.FindTracesByTraceIdAsync(tranceIdArray.ToArray());

        JaegerResponseError? error = null;
        var jaegerTraceOutputList = traces.ToList();
        if (jaegerTraceOutputList.Any() is false)
        {
            error = new JaegerResponseError { Code = (int)HttpStatusCode.NotFound, Message = "trace not found" };
        }

        return new JaegerResponse<IEnumerable<JaegerTraceOutputDto>>(jaegerTraceOutputList) { Error = error };
    }
}