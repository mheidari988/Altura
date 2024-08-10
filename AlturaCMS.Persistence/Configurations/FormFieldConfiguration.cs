﻿using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class FormFieldConfiguration : BaseEntityConfiguration<FormField>
{
    public override void Configure(EntityTypeBuilder<FormField> builder)
    {
        base.Configure(builder);

        builder.ToTable("FormFields", DomainShared.Constants.MetadataSchema);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.DisplayName)
            .HasMaxLength(500);

        builder.Property(e => e.IsRequired)
            .IsRequired();

        builder.Property(e => e.MinLength)
            .IsRequired(false);

        builder.Property(e => e.MaxLength)
            .IsRequired(false);

        builder.Property(e => e.MinValue)
            .HasPrecision(18, 8)
            .IsRequired(false);

        builder.Property(e => e.MaxValue)
            .HasPrecision(18, 8)
            .IsRequired(false);

        builder.Property(e => e.RegexPattern)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.AllowedValues)
            .IsRequired(false)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<string>()));

        builder.HasOne(e => e.Form)
            .WithMany(f => f.FormFields)
            .HasForeignKey(e => e.FormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}