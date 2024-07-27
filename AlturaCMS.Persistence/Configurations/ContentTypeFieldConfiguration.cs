using AlturaCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class ContentTypeFieldConfiguration : IEntityTypeConfiguration<ContentTypeField>
{
    public void Configure(EntityTypeBuilder<ContentTypeField> builder)
    {
        builder.HasKey(e => new { e.ContentTypeId, e.FieldId });

        builder.HasOne(e => e.ContentType)
            .WithMany(ct => ct.Fields)
            .HasForeignKey(e => e.ContentTypeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Field)
            .WithMany()
            .HasForeignKey(e => e.FieldId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}