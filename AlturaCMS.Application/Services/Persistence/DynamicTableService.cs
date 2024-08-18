using AlturaCMS.Application.Services.Persistence.Dynamic;
using AlturaCMS.Domain;
using AlturaCMS.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence;
public class DynamicTableService : IDynamicTableService
{
    private readonly string _connectionString;
    private readonly ILogger<DynamicTableService> _logger;

    public DynamicTableService(IConfiguration configuration, ILogger<DynamicTableService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<bool> CreateTableAsync(Content contentType)
    {
        if (contentType == null)
        {
            _logger.LogError("ContentType is null in CreateTableAsync.");
            throw new ArgumentNullException(nameof(contentType));
        }

        try
        {
            var createSchemaScript = GenerateCreateSchemaScript(DomainShared.Constants.DynamicSchema);
            var createTableScript = GenerateCreateTableScript(contentType);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            _logger.LogInformation("Connection to database opened for creating table {TableName}.", contentType.Name);

            using var createSchemaCommand = new SqlCommand(createSchemaScript, connection);
            await createSchemaCommand.ExecuteNonQueryAsync();
            _logger.LogInformation("Schema creation script executed.");

            using var createTableCommand = new SqlCommand(createTableScript, connection);
            var result = await createTableCommand.ExecuteNonQueryAsync() == -1;

            _logger.LogInformation("Table creation script executed for table {TableName}.", contentType.Name);
            return result;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while creating table {TableName}.", contentType?.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating table {TableName}.", contentType?.Name);
            throw;
        }
    }

    private string GenerateCreateSchemaScript(string schema)
    {
        _logger.LogDebug("Generating CREATE SCHEMA script for schema {Schema}.", schema);

        return $@"
            IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
            BEGIN
                EXEC('CREATE SCHEMA [{schema}]');
            END";
    }

    private string GenerateCreateTableScript(Content content)
    {
        _logger.LogDebug("Generating create table script for content {ContentName}.", content.Name);

        if (content.ContentFields == null || content.ContentFields.Count == 0)
        {
            _logger.LogError("Content {ContentName} has no fields.", content.Name);
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

        _logger.LogDebug("Create table script generated for table {TableName}.", content.Name);
        return sb.ToString();
    }

    private string GenerateFieldScript(ContentField? field)
    {
        ArgumentNullException.ThrowIfNull(field);

        _logger.LogDebug("Generating field script for field {FieldName}.", field.Name);

        var generator = FieldScriptGeneratorFactory.GetFieldScriptGenerator(field.FieldType);
        return generator.GenerateFieldScript(field);
    }

    private string GeneratePivotTableScript(string contentName, ContentField field)
    {
        _logger.LogDebug("Generating pivot table script for field {FieldName} in content {ContentName}.", field.Name, contentName);

        var pivotTableName = $"{contentName}{field.ReferenceTableName}";
        return $@"
            CREATE TABLE [{DomainShared.Constants.DynamicSchema}].[{pivotTableName}] (
                [{contentName}Id] UNIQUEIDENTIFIER REFERENCES [{DomainShared.Constants.DynamicSchema}].[{contentName}]([Id]),
                [{field.ReferenceTableName}Id] UNIQUEIDENTIFIER,
                PRIMARY KEY ([{contentName}Id], [{field.ReferenceTableName}Id])
            );";
    }
}
