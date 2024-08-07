namespace Shine.Dto;

public class JaegerResponse<T>(T data)
{
    public JaegerResponseError? Error { get; init; }
    public T Data { get; init; } = data;
}