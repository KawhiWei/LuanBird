namespace Shine.Dto;

public class JaegerProcessOutputDto
{
    public required string ProcessId { get; init; }

    public required string ServiceName { get; init; }

    public required JaegerTagOutputDto[] Tags { get; init; }
}