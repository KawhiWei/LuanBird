namespace Shine.Dto;

public class JaegerSpanLogOutputDto
{
    public ulong Timestamp { get; init; }

    public required JaegerTagOutputDto[] Fields { get; init; }
}