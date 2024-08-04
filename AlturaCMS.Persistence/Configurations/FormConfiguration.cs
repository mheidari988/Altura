using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class FormConfiguration : BaseEntityConfiguration<Form>
{
    public override void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.ToTable("Forms", DomainShared.Constants.MetadataSchema);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(500);
    }
}