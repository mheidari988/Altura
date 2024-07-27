using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlturaCMS.Persistence.Context;

/// <summary>
/// Represents the database context for the AlturaCMS application.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<ContentTypeField> ContentTypeFields { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormField> FormFields { get; set; }
    public DbSet<ValidationRules> ValidationRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations for each entity
        modelBuilder.ApplyConfiguration(new ContentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FieldConfiguration());
        modelBuilder.ApplyConfiguration(new ContentTypeFieldConfiguration());
        modelBuilder.ApplyConfiguration(new FormConfiguration());
        modelBuilder.ApplyConfiguration(new FormFieldConfiguration());
        modelBuilder.ApplyConfiguration(new ValidationRulesConfiguration());

        // Soft delete and concurrency control configurations
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var methodInfo = typeof(EF).GetMethod("Property", [typeof(object), typeof(string)])
                    ?? throw new InvalidOperationException("The 'Property' method is not found.");

                var isDeletedPropertyMethod = methodInfo.MakeGenericMethod(typeof(bool));

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethodCall = Expression.Call(isDeletedPropertyMethod, parameter, Expression.Constant("IsDeleted"));
                var falseConstant = Expression.Constant(false);
                var comparison = Expression.MakeBinary(ExpressionType.Equal, propertyMethodCall, falseConstant);
                var lambda = Expression.Lambda(comparison, parameter);

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(lambda);

                modelBuilder.Entity(entityType.ClrType)
                    .Property<byte[]>("RowVersion")
                    .IsRowVersion();
            }
        }
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