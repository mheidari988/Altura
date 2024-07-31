using AlturaCMS.Domain.Entities;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class DateTimeFieldScriptGenerator : IFieldScriptGenerator
{
    public string GenerateFieldScript(Field field)
    {
        var sb = new StringBuilder();
        sb.Append($"[{field.Name}] DATETIME2");

        AppendConstraints(sb, field);

        sb.Append(",");
        return sb.ToString();
    }

    private void AppendConstraints(StringBuilder sb, Field field)
    {
        var checkConstraints = new List<string>();

        if (field.IsRequired)
        {
            sb.Append(" NOT NULL");
        }
        if (field.IsUnique)
        {
            sb.Append(" UNIQUE");
        }
        if (field.MinDateTime.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] >= '{field.MinDateTime.Value:yyyy-MM-dd HH:mm:ss}'");
        }
        if (field.MaxDateTime.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] <= '{field.MaxDateTime.Value:yyyy-MM-dd HH:mm:ss}'");
        }
        if (checkConstraints.Count > 0)
        {
            sb.Append(" CHECK (" + string.Join(" AND ", checkConstraints) + ")");
        }
    }
}
