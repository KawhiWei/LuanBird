using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shine.Domain.AggregateRoots.Trace;

namespace Shine.Persistence.EntityFrameworkCore.Configurations;

public class SpanConfigurationEfConfiguration : IEntityTypeConfiguration<Span>
{
    public void Configure(EntityTypeBuilder<Span> builder)
    {
        builder.ToTable("span");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TraceId).HasColumnName("trace_id").IsRequired();
        builder.Property(x => x.SpanId).HasColumnName("span_id").IsRequired();
        builder.Property(x => x.SpanName).HasColumnName("span_name").IsRequired();
        builder.Property(x => x.ParentSpanId).HasColumnName("parent_span_id").IsRequired();
        builder.Property(x => x.StartTimeUnixNano).HasColumnName("start_time_unix_nano").IsRequired();
        builder.Property(x => x.EndTimeUnixNano).HasColumnName("end_time_unix_nano").IsRequired();
        builder.Property(x => x.DurationNanoseconds).HasColumnName("duration_nanoseconds").IsRequired();
        builder.Property(x => x.ServiceName).HasColumnName("service_name").IsRequired();
        builder.Property(x => x.ServiceInstanceId).HasColumnName("service_instance_id").IsRequired();
        builder.Property(x => x.SpanKind).HasColumnName("span_kind").IsRequired();
        builder.Property(x => x.TraceFlags).HasColumnName("trace_flags").IsRequired();
        builder.Property(x => x.TraceState).HasColumnName("trace_state").IsRequired();
        builder.Property(x => x.StatusMessage).HasColumnName("status_message");
        builder.Property(x => x.StatusCode).HasColumnName("status_code");
        builder.Property(x => x.CreationTime).HasColumnName("creation_time");
        builder.Property(x => x.LastModificationTime).HasColumnName("last_modification_time");
        builder.Property(x => x.DeletionTime).HasColumnName("deletion_time");
    }
}