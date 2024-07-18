using Shine.Receiver.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<OTelTraceReceiverService>();
app.MapGrpcReflectionService();
app.Run();
