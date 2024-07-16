using System.Reflection;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Shine.Persistence.Trace;

namespace Shine.Persistence;

public class ShineContext : LuckDbContextBase
{
    public ShineContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }

    public DbSet<Span> Spans => Set<Span>();

    public DbSet<SpanEvent> SpanEvents => Set<SpanEvent>();

    public DbSet<SpanLink> SpanLinks => Set<SpanLink>();

    public DbSet<SpanAttribute> SpanAttributes => Set<SpanAttribute>();

    public DbSet<ResourceAttribute> ResourceAttributes => Set<ResourceAttribute>();

    public DbSet<SpanEventAttribute> SpanEventAttributes => Set<SpanEventAttribute>();

    public DbSet<SpanLinkAttribute> SpanLinkAttributes => Set<SpanLinkAttribute>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("shine_monitoring");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}