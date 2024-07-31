using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.Persistence.Context;
public class DynamicDbContext(string connectionString, Action<ModelBuilder> configureModel) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder) => configureModel(modelBuilder);
}