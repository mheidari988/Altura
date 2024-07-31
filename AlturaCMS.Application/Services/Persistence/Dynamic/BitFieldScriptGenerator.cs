using AlturaCMS.Domain.Entities;
using System.Text;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class BitFieldScriptGenerator : IFieldScriptGenerator
{
    public string GenerateFieldScript(Field field)
    {
        var sb = new StringBuilder();
        sb.Append($"[{field.Name}] BIT");

        if (field.IsRequired)
        {
            sb.Append(" NOT NULL");
        }

        if (field.IsUnique)
        {
            sb.Append(" UNIQUE");
        }

        sb.Append(",");
        return sb.ToString();
    }
}
