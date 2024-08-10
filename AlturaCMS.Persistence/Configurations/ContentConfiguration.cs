using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class ContentConfiguration : BaseEntityConfiguration<Content>
{
    public override void Configure(EntityTypeBuilder<Content> builder)
    {
        base.Configure(builder);

        builder.ToTable("Contents", DomainShared.Constants.MetadataSchema);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(e => e.ContentFields)
            .WithOne(cf => cf.Content)
            .HasForeignKey(cf => cf.ContentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}