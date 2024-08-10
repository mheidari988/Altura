using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.Persistence.Context;

/// <summary>
/// Represents the database context for the AlturaCMS application.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Content> ContentTypes { get; set; }
    public DbSet<ContentField> Fields { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormField> FormFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations for each entity
        modelBuilder.ApplyConfiguration(new ContentConfiguration());
        modelBuilder.ApplyConfiguration(new ContentFieldConfiguration());
        modelBuilder.ApplyConfiguration(new FormConfiguration());
        modelBuilder.ApplyConfiguration(new FormFieldConfiguration());

        // Apply soft delete and concurrency control configurations
        ApplyBaseEntityConfiguration<Content>(modelBuilder);
        ApplyBaseEntityConfiguration<ContentField>(modelBuilder);
        ApplyBaseEntityConfiguration<Form>(modelBuilder);
        ApplyBaseEntityConfiguration<FormField>(modelBuilder);
    }

    private void ApplyBaseEntityConfiguration<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TEntity>().Property<byte[]>("RowVersion").IsRowVersion();
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChanges();
    }
}