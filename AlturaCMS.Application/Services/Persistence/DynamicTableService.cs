﻿using AlturaCMS.Application.Services.Persistence.Dynamic;
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

    public async ValueTask<bool> CreateTableAsync(ContentType contentType)
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

    private string GenerateCreateTableScript(ContentType contentType)
    {
        if (contentType.Fields == null || contentType.Fields.Count == 0)
        {
            throw new ArgumentException("ContentType must have at least one field.");
        }

        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE [{DomainShared.Constants.DynamicSchema}].[{contentType.Name}] (");
        sb.AppendLine("[Id] UNIQUEIDENTIFIER PRIMARY KEY,");

        // remove content type field which has RowVersion name
        // we will add it seperatedly
        contentType.Fields = contentType.Fields
            .Where(c => c.Field?.Name != DomainShared.Constants.DefaultFields.RowVersion.Name).ToList();

        var regularFields = contentType.Fields
            .Where(f => f.Field?.FieldType != FieldType.MultiSelect);

        var multiSelectFields = contentType.Fields
            .Where(f => f.Field?.FieldType == FieldType.MultiSelect);

        foreach (var field in regularFields)
        {
            sb.AppendLine(GenerateFieldScript(field.Field));
        }

        // Add RowVersion field manually
        sb.AppendLine("[RowVersion] ROWVERSION,");
        sb.AppendLine(");");

        foreach (var field in multiSelectFields)
        {
            sb.AppendLine(GeneratePivotTableScript(contentType.Name, field.Field!));
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
        var pivotTableName = $"{contentTypeName}{field.ReferenceTableName}";
        return $@"
            CREATE TABLE [{DomainShared.Constants.DynamicSchema}].[{pivotTableName}] (
                [{contentTypeName}Id] UNIQUEIDENTIFIER REFERENCES [{DomainShared.Constants.DynamicSchema}].[{contentTypeName}]([Id]),
                [{field.ReferenceTableName}Id] UNIQUEIDENTIFIER,
                PRIMARY KEY ([{contentTypeName}Id], [{field.ReferenceTableName}Id])
            );";
    }
}
