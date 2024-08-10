using AlturaCMS.Domain.Entities;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class DecimalFieldScriptGenerator : IFieldScriptGenerator
{
    public string GenerateFieldScript(ContentField field)
    {
        var sb = new StringBuilder();
        sb.Append($"[{field.Name}] DECIMAL(18,8)");

        AppendConstraints(sb, field);

        sb.Append(",");
        return sb.ToString();
    }

    private void AppendConstraints(StringBuilder sb, ContentField field)
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
        if (field.MinValue.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] >= {field.MinValue.Value}");
        }
        if (field.MaxValue.HasValue)
        {
            checkConstraints.Add($"[{field.Name}] <= {field.MaxValue.Value}");
        }
        if (checkConstraints.Count > 0)
        {
            sb.Append(" CHECK (" + string.Join(" AND ", checkConstraints) + ")");
        }
    }
}
