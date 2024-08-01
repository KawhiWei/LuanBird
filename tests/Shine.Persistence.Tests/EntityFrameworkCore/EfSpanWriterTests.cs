using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.MemoryDatabase;
using Microsoft.Extensions.DependencyInjection;
using Shine.Domain.Shared.Enums;
using Shine.Domain.SpanWriter;
using Shine.Dto;
using Shine.Infrastructure.Extensions;

namespace Shine.Persistence.Tests.EntityFrameworkCore;

public class EfSpanWriterTests
{
    private readonly string _connectionString = Guid.NewGuid().ToString();
    private readonly ISpanWriter _spanWriter;

    public EfSpanWriterTests()
    {
        var services = new ServiceCollection();
        services.AddLuckDbContext<ShineContext>(x =>
            {
                x.ConnectionString = _connectionString;
                x.Type = DataBaseType.MemoryDataBase;
            })
            .AddLogging()
            .AddUnitOfWork()
            .AddMemoryDriven()
            .AddDefaultRepository()
            .AddScoped<ISpanWriter, EfSpanWriter>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        _spanWriter = serviceProvider.GetRequiredService<ISpanWriter>();
    }

    [Fact]
    public async Task Test1()
    {
        var now = DateTimeOffset.Now;
        var spans = new[]
        {
            new ShineSpan
            {
                TraceId = "TraceId1",
                SpanId = "SpanId1",
                SpanName = "SpanName1",
                ParentSpanId = "ParentSpanId1",
                StartTimeUnixNano = now.ToUnixTimeNanoseconds(),
                EndTimeUnixNano = now.AddMinutes(1).ToUnixTimeNanoseconds(),
                DurationNanoseconds = 60_000_000_000,
                StatusCode = SpanStatusCode.Ok,
                StatusMessage = "StatusMessage1",
                SpanKind = SpanKind.Server,
                Resource =
                    new ShineResource
                    {
                        ServiceName = "ServiceName1",
                        ServiceInstanceId = "ServiceInstanceId1",
                        Attributes =
                            new[]
                            {
                                new ShineAttribute
                                {
                                    Key = "ServiceVersion",
                                    ValueType = AttributeValueType.StringValue,
                                    Value = "ServiceVersion1"
                                }
                            }
                    },
                TraceFlags = 1,
                TraceState = "TraceState1",
                Attributes =
                    new[]
                    {
                        new ShineAttribute
                        {
                            Key = "SpanAttributeKey1",
                            ValueType = AttributeValueType.StringValue,
                            Value = "SpanAttributeValue1"
                        },
                        new ShineAttribute
                        {
                            Key = "SpanAttributeKey2",
                            ValueType = AttributeValueType.BoolValue,
                            Value = "True"
                        },
                        new ShineAttribute
                        {
                            Key = "SpanAttributeKey3",
                            ValueType = AttributeValueType.IntValue,
                            Value = "31"
                        },
                        new ShineAttribute
                        {
                            Key = "SpanAttributeKey4",
                            ValueType = AttributeValueType.DoubleValue,
                            Value = "11.1"
                        }
                    },
                Events = new[]
                {
                    new ShineSpanEvent
                    {
                        Name = "EventName1",
                        TimestampUnixNano = now.AddMinutes(-1).ToUnixTimeNanoseconds(),
                        Attributes =
                            new[]
                            {
                                new ShineAttribute
                                {
                                    Key = "EventAttributeKey1",
                                    ValueType = AttributeValueType.StringValue,
                                    Value = "EventAttributeValue1"
                                },
                                new ShineAttribute
                                {
                                    Key = "EventAttributeKey2",
                                    ValueType = AttributeValueType.BoolValue,
                                    Value = "True"
                                },
                                new ShineAttribute
                                {
                                    Key = "EventAttributeKey3",
                                    ValueType = AttributeValueType.IntValue,
                                    Value = "31"
                                },
                                new ShineAttribute
                                {
                                    Key = "EventAttributeKey4",
                                    ValueType = AttributeValueType.DoubleValue,
                                    Value = "11.1"
                                }
                            }
                    },
                    new ShineSpanEvent
                    {
                        Name = "EventName2",
                        TimestampUnixNano = now.AddMinutes(-1).ToUnixTimeNanoseconds(),
                        Attributes = new[]
                        {
                            new ShineAttribute
                            {
                                Key = "EventAttributeKey1",
                                ValueType = AttributeValueType.StringValue,
                                Value = "EventAttributeValue1"
                            }
                        }
                    }
                },
                Links =
                    new[]
                    {
                        new ShineSpanLink
                        {
                            LinkedTraceId = "LinkedTraceId1",
                            LinkedSpanId = "LinkedSpanId1",
                            LinkedTraceState = "LinkedTraceState1",
                            LinkedTraceFlags = 1,
                            Attributes =
                                new[]
                                {
                                    new ShineAttribute
                                    {
                                        Key = "LinkAttributeKey1",
                                        ValueType = AttributeValueType.StringValue,
                                        Value = "LinkAttributeValue1"
                                    },
                                    new ShineAttribute
                                    {
                                        Key = "LinkAttributeKey2",
                                        ValueType = AttributeValueType.IntValue,
                                        Value = "21"
                                    },
                                }
                        },
                        new ShineSpanLink
                        {
                            LinkedTraceId = "LinkedTraceId2",
                            LinkedSpanId = "LinkedSpanId2",
                            LinkedTraceState = "LinkedTraceState2",
                            LinkedTraceFlags = 2,
                            Attributes =
                                new[]
                                {
                                    new ShineAttribute
                                    {
                                        Key = "LinkAttributeKey3",
                                        ValueType = AttributeValueType.BoolValue,
                                        Value = "True"
                                    }
                                }
                        }
                    },
            },
            new ShineSpan
            {
                TraceId = "TraceId2",
                SpanId = "SpanId2",
                SpanName = "SpanName2",
                ParentSpanId = "ParentSpanId2",
                StartTimeUnixNano = now.ToUnixTimeNanoseconds(),
                EndTimeUnixNano = now.AddMinutes(2)
                    .ToUnixTimeNanoseconds(),
                DurationNanoseconds = 120_000_000_000,
                StatusCode = SpanStatusCode.Error,
                StatusMessage = "StatusMessage2",
                SpanKind = SpanKind.Client,
                Resource =
                    new ShineResource
                    {
                        ServiceName = "ServiceName2",
                        ServiceInstanceId = "ServiceInstanceId2",
                        Attributes =
                            new[]
                            {
                                new ShineAttribute
                                {
                                    Key = "ServiceVersion",
                                    ValueType = AttributeValueType.StringValue,
                                    Value = "ServiceVersion2"
                                }
                            }
                    },
                TraceFlags = 1,
                TraceState = "TraceState2",
                Links = [],
                Attributes = [],
                Events = new[]
                {
                    new ShineSpanEvent
                    {
                        Name = "EventName3",
                        TimestampUnixNano = now.AddMinutes(-1).ToUnixTimeNanoseconds(),
                        Attributes = []
                    }
                }
            }
        };
        await _spanWriter.WriteAsync(spans);
    }
}