using AlturaCMS.Application.Services.Persistence.Dynamic;
using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence;
public class DynamicTableService : IDynamicTableService
{
    private readonly string _connectionString;

    public DynamicTableService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async ValueTask<bool> CreateTableAsync(Content contentType)
    {
        var createSchemaScript = GenerateCreateSchemaScript(DomainShared.Constants.DynamicSchema);
        var createTableScript = GenerateCreateTableScript(contentType);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var createSchemaCommand = new SqlCommand(createSchemaScript, connection);
        await createSchemaCommand.ExecuteNonQueryAsync();

        using var createTableCommand = new SqlCommand(createTableScript, connection);

        return await createTableCommand.ExecuteNonQueryAsync() == -1;
    }

    private string GenerateCreateSchemaScript(string schema)
    {
        return $@"
            IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
            BEGIN
                EXEC('CREATE SCHEMA [{schema}]');
            END";
    }

    private string GenerateCreateTableScript(Content content)
    {
        if (content.ContentFields == null || content.ContentFields.Count == 0)
        {
            throw new ArgumentException("Content must have at least one field.");
        }

        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE [{DomainShared.Constants.DynamicSchema}].[{content.Name}] (");
        sb.AppendLine("[Id] UNIQUEIDENTIFIER PRIMARY KEY,");

        // remove content field which has RowVersion name
        // we will add it seperatedly
        content.ContentFields = content.ContentFields
            .Where(c => c.Name != DomainShared.Constants.DefaultFields.RowVersion.Name).ToList();

        var regularFields = content.ContentFields
            .Where(f => f.FieldType != FieldType.MultiSelect);

        var multiSelectFields = content.ContentFields
            .Where(f => f.FieldType == FieldType.MultiSelect);

        foreach (var field in regularFields)
        {
            sb.AppendLine(GenerateFieldScript(field));
        }

        // Add RowVersion field manually
        sb.AppendLine("[RowVersion] ROWVERSION,");
        sb.AppendLine(");");

        foreach (var field in multiSelectFields)
        {
            sb.AppendLine(GeneratePivotTableScript(content.Name, field!));
        }

        return sb.ToString();
    }

    private string GenerateFieldScript(ContentField? field)
    {
        ArgumentNullException.ThrowIfNull(field);

        var generator = FieldScriptGeneratorFactory.GetFieldScriptGenerator(field.FieldType);
        return generator.GenerateFieldScript(field);
    }

    private string GeneratePivotTableScript(string contentName, ContentField field)
    {
        var pivotTableName = $"{contentName}{field.ReferenceTableName}";
        return $@"
            CREATE TABLE [{DomainShared.Constants.DynamicSchema}].[{pivotTableName}] (
                [{contentName}Id] UNIQUEIDENTIFIER REFERENCES [{DomainShared.Constants.DynamicSchema}].[{contentName}]([Id]),
                [{field.ReferenceTableName}Id] UNIQUEIDENTIFIER,
                PRIMARY KEY ([{contentName}Id], [{field.ReferenceTableName}Id])
            );";
    }
}
