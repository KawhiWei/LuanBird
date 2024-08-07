using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shine.Persistence;

public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
{
    protected override void AddDbContextWithUnitOfWork(IServiceCollection services)
    {
        var connectionString = services.GetConfiguration().GetConnectionString("EF");
        services.AddLuckDbContext<ShineContext>(x =>
        {
            x.ConnectionString = connectionString!;
            x.Type = DataBaseType.PostgreSql;
        });
    }

    protected override void AddDbDriven(IServiceCollection service)
    {
        service.AddPostgreSQLDriven();
    }
}