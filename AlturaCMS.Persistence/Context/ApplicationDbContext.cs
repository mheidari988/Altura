using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations for each entity
        modelBuilder.ApplyConfiguration(new ContentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FieldConfiguration());
        modelBuilder.ApplyConfiguration(new ContentTypeFieldConfiguration());
        modelBuilder.ApplyConfiguration(new FormConfiguration());
        modelBuilder.ApplyConfiguration(new FormFieldConfiguration());

        // Apply soft delete and concurrency control configurations
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var entityBuilder = modelBuilder.Entity(entityType.ClrType);

                // Apply the query filter for soft delete
                entityBuilder.HasQueryFilter(ConvertFilterExpression<BaseEntity>(e => !e.IsDeleted));

                // Apply concurrency token
                entityBuilder.Property<byte[]>("RowVersion").IsRowVersion();
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

    private static LambdaExpression ConvertFilterExpression<T>(Expression<Func<T, bool>> filterExpression)
    {
        var parameter = Expression.Parameter(typeof(T));
        var newExpression = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), parameter, filterExpression.Body);
        return Expression.Lambda(newExpression, parameter);
    }
}