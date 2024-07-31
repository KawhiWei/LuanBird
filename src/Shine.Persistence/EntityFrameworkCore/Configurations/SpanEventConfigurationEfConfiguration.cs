using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shine.Domain.AggregateRoots.Trace;

namespace Shine.Persistence.EntityFrameworkCore.Configurations;

public class SpanEventConfigurationEfConfiguration : IEntityTypeConfiguration<SpanEvent>
{
    public void Configure(EntityTypeBuilder<SpanEvent> builder)
    {
        builder.ToTable("span_event");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TraceId).HasColumnName("trace_id").IsRequired();
        builder.Property(x => x.SpanId).HasColumnName("span_id").IsRequired();
        builder.Property(x => x.Index).HasColumnName("index").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.TimestampUnixNano).HasColumnName("timestamp_unix_nano").IsRequired();
        builder.Property(x => x.CreationTime).HasColumnName("creation_time");
        builder.Property(x => x.LastModificationTime).HasColumnName("last_modification_time");
        builder.Property(x => x.DeletionTime).HasColumnName("deletion_time");
    }
}