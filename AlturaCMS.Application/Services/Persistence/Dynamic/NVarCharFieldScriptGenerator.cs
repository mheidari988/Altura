using AlturaCMS.Domain.Entities;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class NVarCharFieldScriptGenerator : IFieldScriptGenerator
{
    public string GenerateFieldScript(ContentField field)
    {
        var sb = new StringBuilder();
        string len = "MAX";
        if (field.IsUnique)
        {
            len = field.MaxLength.HasValue ? field.MaxLength.Value.ToString() : "500";
        }
        sb.Append($"[{field.Name}] NVARCHAR({len})");

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
        if (field.MinLength.HasValue)
        {
            checkConstraints.Add($"LEN([{field.Name}]) >= {field.MinLength.Value}");
        }
        if (field.MaxLength.HasValue)
        {
            checkConstraints.Add($"LEN([{field.Name}]) <= {field.MaxLength.Value}");
        }
        if (!string.IsNullOrEmpty(field.RegexPattern))
        {
            checkConstraints.Add($"[{field.Name}] LIKE '{field.RegexPattern}'");
        }
        if (checkConstraints.Count > 0)
        {
            sb.Append(" CHECK (" + string.Join(" AND ", checkConstraints) + ")");
        }
    }
}
