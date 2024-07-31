using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class FieldConfiguration : BaseEntityConfiguration<Field>
{
    public override void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(200);

        builder.Property(e => e.FieldType)
            .IsRequired();

        builder.Property(e => e.IsRequired)
            .IsRequired();

        builder.Property(e => e.MinLength)
            .IsRequired(false);

        builder.Property(e => e.MaxLength)
            .IsRequired(false);

        builder.Property(e => e.MinValue)
            .IsRequired(false);

        builder.Property(e => e.MaxValue)
            .IsRequired(false);

        builder.Property(e => e.RegexPattern)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.AllowedValues)
            .IsRequired(false)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<string>()));

        builder.HasIndex(e => e.Slug)
            .IsUnique();

        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}