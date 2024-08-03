using AlturaCMS.Domain.Entities;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class UniqueIdentifierFieldScriptGenerator : IFieldScriptGenerator
{
    public string GenerateFieldScript(Field field)
    {
        var sb = new StringBuilder();
        sb.Append($"[{field.Name}] UNIQUEIDENTIFIER");

        if (field.IsRequired)
        {
            sb.Append(" NOT NULL");
        }

        // Add foreign key constraint if referenced table and column are provided
        if (field.ReferenceTableName != null)
        {
            sb.Append($" REFERENCES [{DynamicTableService.DynamicTableSchema}].[{field.ReferenceTableName}]([Id])");
        }

        sb.Append(",");
        return sb.ToString();
    }
}
