using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.MemoryDatabase;
using Luck.Framework.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using Shine.Domain.Repositories;

namespace Shine.Persistence.Tests;

public class EFSpanWriterTests
{
    private readonly string _connectionString = Guid.NewGuid().ToString();
    private readonly ISpanWriterRepository _spanWriterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EFSpanWriterTests()
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
            .AddDefaultRepository();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        serviceProvider = services.BuildServiceProvider();
        _spanWriterRepository = serviceProvider.GetRequiredService<ISpanWriterRepository>();
    }


    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
    }
}