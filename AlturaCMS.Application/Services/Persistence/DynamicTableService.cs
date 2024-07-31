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
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task CreateTableAsync(ContentType contentType)
    {
        var createTableScript = GenerateCreateTableScript(contentType);

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(createTableScript, connection);
        await command.ExecuteNonQueryAsync();
    }

    private string GenerateCreateTableScript(ContentType contentType)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE [{contentType.Name}] (");
        sb.AppendLine("[Id] UNIQUEIDENTIFIER PRIMARY KEY,"); // Assuming each table has an Id column

        foreach (var field in contentType.Fields)
        {
            sb.AppendLine(GenerateFieldScript(field.Field));
        }

        sb.AppendLine(");");

        foreach (var field in contentType.Fields)
        {
            if (field.Field.FieldType == FieldType.MultiSelect)
            {
                sb.AppendLine(GeneratePivotTableScript(contentType.Name, field.Field));
            }
        }

        return sb.ToString();
    }

    private string GenerateFieldScript(Field field)
    {
        var fieldType = field.FieldType switch
        {
            FieldType.Text => "NVARCHAR(MAX)",
            FieldType.RichText => "NVARCHAR(MAX)",
            FieldType.Number => "INT",
            FieldType.Currency => "DECIMAL(18, 2)",
            FieldType.Checkbox => "BIT",
            FieldType.DateTime => "DATETIME",
            FieldType.File => "NVARCHAR(MAX)",
            FieldType.Select => "UNIQUEIDENTIFIER", // Assuming it references another table
            FieldType.MultiSelect => null, // Handled separately
            _ => throw new ArgumentOutOfRangeException()
        };

        if (fieldType == null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.Append($"[{field.Name}] {fieldType}");

        if (field.IsRequired)
        {
            sb.Append(" NOT NULL");
        }

        var checkConstraints = new List<string>();

        if (field.MinLength.HasValue)
        {
            checkConstraints.Add($"LEN([{field.Name}]) >= {field.MinLength.Value}");
        }

        if (field.MaxLength.HasValue)
        {
            checkConstraints.Add($"LEN([{field.Name}]) <= {field.MaxLength.Value}");
        }

        if (field.MinValue.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] >= {field.MinValue.Value}");
        }

        if (field.MaxValue.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] <= {field.MaxValue.Value}");
        }

        if (!string.IsNullOrEmpty(field.RegexPattern))
        {
            checkConstraints.Add($"[{field.Name}] LIKE '{field.RegexPattern}'");
        }

        if (checkConstraints.Count > 0)
        {
            sb.Append(" CHECK (" + string.Join(" AND ", checkConstraints) + ")");
        }

        sb.Append(",");

        return sb.ToString();
    }


    private string GeneratePivotTableScript(string contentTypeName, Field field)
    {
        var pivotTableName = $"{contentTypeName}_{field.Name}";
        return $@"
CREATE TABLE [{pivotTableName}] (
    [{contentTypeName}Id] UNIQUEIDENTIFIER REFERENCES [{contentTypeName}]([Id]),
    [{field.Name}Id] UNIQUEIDENTIFIER,
    PRIMARY KEY ([{contentTypeName}Id], [{field.Name}Id])
);";
    }
}