namespace Shine.Infrastructure;

public static class JaegerSpanKind
{
    public const string Unspecified = "unspecified";
    public const string Internal = "internal";
    public const string Server = "server";
    public const string Client = "client";
    public const string Producer = "producer";
    public const string Consumer = "consumer";
}