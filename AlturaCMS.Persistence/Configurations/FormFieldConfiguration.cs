using AlturaCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations;
public class FormFieldConfiguration : IEntityTypeConfiguration<FormField>
{
    public void Configure(EntityTypeBuilder<FormField> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasOne(e => e.ValidationRules)
            .WithMany()
            .HasForeignKey(e => e.ValidationRulesId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Form)
            .WithMany(f => f.FormFields)
            .HasForeignKey(e => e.FormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}