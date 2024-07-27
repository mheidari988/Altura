using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class ContentTypeConfiguration : BaseEntityConfiguration<ContentType>
{
    public override void Configure(EntityTypeBuilder<ContentType> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}