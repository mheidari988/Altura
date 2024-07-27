using AlturaCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class ValidationRulesConfiguration : IEntityTypeConfiguration<ValidationRules>
{
    public void Configure(EntityTypeBuilder<ValidationRules> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.RegexPattern)
            .HasMaxLength(200);
        builder.Property(e => e.AllowedValues)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}