using Luck.AutoDependencyInjection;
using Shine.Receiver.AppModules;
using Shine.Receiver.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication<AppWebModule>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<OTelTraceReceiverService>();
app.MapGrpcReflectionService();
app.InitializeApplication();
app.Run();
