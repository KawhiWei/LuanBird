namespace Shine.Dto;

public class JaegerTraceOutputDto
{
    public required string TraceID { get; set; }

    public required Dictionary<string, JaegerProcessOutputDto> Processes { get; set; }

    public required JaegerSpanOutputDto[] Spans { get; set; }
}