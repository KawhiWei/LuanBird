using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shine.Persistence.Trace;

namespace Shine.Persistence.EntityFrameworkCore.Configurations;

public class SpanLinkAttributeConfigurationEfConfiguration : IEntityTypeConfiguration<SpanLinkAttribute>
{
    public void Configure(EntityTypeBuilder<SpanLinkAttribute> builder)
    {
        builder.ToTable("span_link_attribute");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TraceId).HasColumnName("trace_id").IsRequired();
        builder.Property(x => x.SpanLinkIndex).HasColumnName("span_link_index").IsRequired();
        builder.Property(x => x.SpanId).HasColumnName("span_id").IsRequired();
        builder.Property(x => x.Key).HasColumnName("key").IsRequired();
        builder.Property(x => x.ValueType).HasColumnName("value_type").IsRequired();
        builder.Property(x => x.Value).HasColumnName("value").IsRequired();
        builder.Property(x => x.CreationTime).HasColumnName("creation_time");
        builder.Property(x => x.LastModificationTime).HasColumnName("last_modification_time");
        builder.Property(x => x.DeletionTime).HasColumnName("deletion_time");
    }
}