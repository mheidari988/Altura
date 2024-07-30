using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.Application.Services.Persistence;

public class DynamicTableService(ApplicationDbContext context) : IDynamicTableService
{
    public void CreateTable(ContentType contentType)
    {
        var modelBuilder = InitializeModelBuilder();
        ConfigureEntity(modelBuilder.Entity(contentType.Name), contentType.Fields);
        ApplyModelChanges(modelBuilder);
    }

    public void AlterTable(ContentType contentType)
    {
        var modelBuilder = InitializeModelBuilder();
        var entityTypeBuilder = modelBuilder.Entity(contentType.Name);

        var existingFields = context.Model.FindEntityType(contentType.Name)?.GetProperties()
                                            .Select(p => p.Name).ToList();

        foreach (var field in contentType.Fields)
        {
            if (existingFields == null || !existingFields.Contains(field.Field.Slug))
            {
                ConfigureField(entityTypeBuilder, field);
            }
        }

        ApplyModelChanges(modelBuilder);
    }

    private static ModelBuilder InitializeModelBuilder() => new(new ConventionSet());

    private void ConfigureEntity(EntityTypeBuilder entityTypeBuilder, IEnumerable<ContentTypeField> fields)
    {
        foreach (var field in fields)
        {
            ConfigureField(entityTypeBuilder, field);
        }
    }

    private void ConfigureField(EntityTypeBuilder entityTypeBuilder, ContentTypeField contentTypeField)
    {
        Field field = contentTypeField.Field ?? throw new ArgumentNullException(nameof(contentTypeField.Field));
        var propertyBuilder = entityTypeBuilder.Property(field.Slug);

        switch (field.Type)
        {
            case FieldType.Text:
                ConfigureTextField(propertyBuilder, field);
                break;
            case FieldType.Number:
                ConfigureNumberField(propertyBuilder, field);
                break;
            case FieldType.Currency:
                propertyBuilder.HasColumnType("decimal(18,2)");
                break;
            case FieldType.DateTime:
                propertyBuilder.HasColumnType("datetime");
                break;
            case FieldType.Checkbox:
                propertyBuilder.HasColumnType("bit");
                break;
            case FieldType.Select:
                ConfigureSelectField(entityTypeBuilder, propertyBuilder, field);
                break;
            case FieldType.MultiSelect:
                CreatePivotTable(entityTypeBuilder, contentTypeField.ContentType!.Name, field);
                break;
            case FieldType.File:
                propertyBuilder.HasColumnType("varchar(255)");
                break;
        }

        ApplyCommonFieldConfigurations(propertyBuilder, field);
    }

    private void ConfigureTextField(PropertyBuilder propertyBuilder, Field field)
    {
        propertyBuilder.HasMaxLength(field.MaxLength ?? 255).HasColumnType("nvarchar");
    }

    private void ConfigureNumberField(PropertyBuilder propertyBuilder, Field field)
    {
        propertyBuilder.HasColumnType("int");

        if (field.MinValue.HasValue)
            propertyBuilder.HasAnnotation("Min", field.MinValue.Value);

        if (field.MaxValue.HasValue)
            propertyBuilder.HasAnnotation("Max", field.MaxValue.Value);
    }

    private void ConfigureSelectField(EntityTypeBuilder entityTypeBuilder, PropertyBuilder propertyBuilder, Field field)
    {
        propertyBuilder.HasColumnType("int");

        if (!string.IsNullOrWhiteSpace(field.Slug))
        {
            entityTypeBuilder.HasOne(field.Slug)
                             .WithMany()
                             .HasForeignKey(field.Slug);
        }
    }

    private void ApplyCommonFieldConfigurations(PropertyBuilder propertyBuilder, Field field)
    {
        if (field.IsRequired)
        {
            propertyBuilder.IsRequired();
        }

        if (field.MinLength.HasValue)
        {
            propertyBuilder.HasAnnotation("MinLength", field.MinLength.Value);
        }

        if (field.MaxLength.HasValue)
        {
            propertyBuilder.HasAnnotation("MaxLength", field.MaxLength.Value);
        }
    }

    private void CreatePivotTable(EntityTypeBuilder entityTypeBuilder, string contentTypeName, Field field)
    {
        var otherEntityName = field.Slug; // Assuming Slug is used as the related entity name
        var pivotTableName = string.Compare(contentTypeName, otherEntityName, StringComparison.OrdinalIgnoreCase) < 0
            ? $"{contentTypeName}{otherEntityName}"
            : $"{otherEntityName}{contentTypeName}";

        var modelBuilder = InitializeModelBuilder();
        var pivotEntityTypeBuilder = modelBuilder.Entity(pivotTableName);

        pivotEntityTypeBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
        pivotEntityTypeBuilder.Property<Guid>($"{contentTypeName}Id");
        pivotEntityTypeBuilder.Property<Guid>($"{otherEntityName}Id");

        pivotEntityTypeBuilder.HasKey("Id");

        pivotEntityTypeBuilder.HasOne(entityTypeBuilder.Metadata.ClrType)
                              .WithMany()
                              .HasForeignKey($"{contentTypeName}Id");

        pivotEntityTypeBuilder.HasOne(field.Slug)
                              .WithMany()
                              .HasForeignKey($"{otherEntityName}Id");

        ApplyPivotTableModelChanges(modelBuilder, pivotTableName, entityTypeBuilder.Metadata.ClrType, field.Slug);
    }

    private void ApplyPivotTableModelChanges(ModelBuilder modelBuilder, string pivotTableName, Type ownerClrType, string relatedEntityName)
    {
        var compiledModel = modelBuilder.FinalizeModel();
        using var dynamicContext = new DynamicDbContext(context.Database.GetDbConnection().ConnectionString, modelBuilder =>
        {
            modelBuilder.Entity(pivotTableName).HasKey("Id");
            modelBuilder.Entity(pivotTableName).HasOne(ownerClrType)
                                               .WithMany()
                                               .HasForeignKey($"{ownerClrType.Name}Id");

            modelBuilder.Entity(pivotTableName).HasOne(relatedEntityName)
                                               .WithMany()
                                               .HasForeignKey($"{relatedEntityName}Id");
        });
        dynamicContext.Database.EnsureCreated();
    }

    private void ApplyModelChanges(ModelBuilder modelBuilder)
    {
        var compiledModel = modelBuilder.FinalizeModel();
        using var dynamicContext = new DynamicDbContext(context.Database.GetDbConnection().ConnectionString, modelBuilder =>
        {
            compiledModel.GetEntityTypes().ToList().ForEach(entityType =>
            {
                modelBuilder.Entity(entityType.Name, entity =>
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        entity.Property(property.ClrType, property.Name);
                    }
                });
            });
        });
        dynamicContext.Database.EnsureCreated();
    }
}