using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.Framework.Infrastructure;
using Shine.Persistence;

namespace Shine.Receiver.AppModules;

[DependsOn(
    typeof(AutoDependencyAppModule),
    typeof(EntityFrameworkCoreModule)
)]
public class AppWebModule : LuckAppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
    }
}