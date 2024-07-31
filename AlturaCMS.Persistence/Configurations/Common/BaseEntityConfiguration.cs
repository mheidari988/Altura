using AlturaCMS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlturaCMS.Persistence.Configurations.Common;
public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.RowVersion)
            .IsRowVersion();

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(500);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(500);

        // Custom constraint to ensure DeletedDate and DeletedBy are not null if IsDeleted is true
        builder.ToTable(typeof(T).Name, 
            t => t.HasCheckConstraint(
                "CK_BaseEntity_Deleted",
                @"[IsDeleted] = 0 OR ([DeletedDate] IS NOT NULL AND [DeletedBy] IS NOT NULL)"
                )
            );
    }
}