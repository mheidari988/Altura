﻿using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class FormFieldConfiguration : BaseEntityConfiguration<FormField>
{
    public override void Configure(EntityTypeBuilder<FormField> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(e => e.Form)
            .WithMany(f => f.FormFields)
            .HasForeignKey(e => e.FormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}