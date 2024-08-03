using AlturaCMS.Application.Services.Persistence.Dynamic;
using AlturaCMS.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence;
public class DynamicTableService : IDynamicTableService
{
    private readonly string _connectionString;
    public const string DynamicTableSchema = "Dynamic";

    public DynamicTableService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async ValueTask<bool> CreateTableAsync(ContentType contentType)
    {
        var createSchemaScript = GenerateCreateSchemaScript(DynamicTableSchema);
        var createTableScript = GenerateCreateTableScript(contentType);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var createSchemaCommand = new SqlCommand(createSchemaScript, connection);
        await createSchemaCommand.ExecuteNonQueryAsync();

        using var createTableCommand = new SqlCommand(createTableScript, connection);

        int result = await createTableCommand.ExecuteNonQueryAsync();

        // If result is -1, the table was created successfully
        return result == -1;
    }

    private string GenerateCreateSchemaScript(string schema)
    {
        return $@"
            IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
            BEGIN
                EXEC('CREATE SCHEMA [{schema}]');
            END";
    }

    private string GenerateCreateTableScript(ContentType contentType)
    {
        if (contentType.Fields == null || contentType.Fields.Count == 0)
        {
            throw new ArgumentException("ContentType must have at least one field.");
        }

        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE [{DynamicTableSchema}].[{contentType.Name}] (");
        sb.AppendLine("[Id] UNIQUEIDENTIFIER PRIMARY KEY,");

        foreach (var field in contentType.Fields)
        {
            sb.AppendLine(GenerateFieldScript(field.Field));
        }

        sb.AppendLine(");");

        foreach (var field in contentType.Fields)
        {
            if (field.Field?.FieldType == FieldType.MultiSelect)
            {
                sb.AppendLine(GeneratePivotTableScript(contentType.Name, field.Field));
            }
        }

        return sb.ToString();
    }

    private string GenerateFieldScript(Field? field)
    {
        ArgumentNullException.ThrowIfNull(field);

        var generator = FieldScriptGeneratorFactory.GetFieldScriptGenerator(field.FieldType);
        return generator.GenerateFieldScript(field);
    }

    private string GeneratePivotTableScript(string contentTypeName, Field field)
    {
        var pivotTableName = $"{contentTypeName}_{field.Name}";
        return $@"
            CREATE TABLE [{DynamicTableSchema}].[{pivotTableName}] (
                [{contentTypeName}Id] UNIQUEIDENTIFIER REFERENCES [{DynamicTableSchema}].[{contentTypeName}]([Id]),
                [{field.Name}Id] UNIQUEIDENTIFIER,
                PRIMARY KEY ([{contentTypeName}Id], [{field.Name}Id])
            );";
    }
}
