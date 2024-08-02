namespace Shine.Dto;

public class JaegerTraceOutputDto
{
    public required string TraceId { get; set; }

    public required Dictionary<string, JaegerProcessOutputDto> Processes { get; set; }

    public required JaegerSpanOutputDto[] Spans { get; set; }
}