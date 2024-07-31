using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public class FieldScriptGeneratorFactory
{
    public static IFieldScriptGenerator GetFieldScriptGenerator(FieldType fieldType)
    {
        return fieldType switch
        {
            FieldType.Text => new NVarCharFieldScriptGenerator(),
            FieldType.RichText => new NVarCharFieldScriptGenerator(),
            FieldType.Number => new IntFieldScriptGenerator(),
            FieldType.Currency => new DecimalFieldScriptGenerator(),
            FieldType.DateTime => new DateTimeFieldScriptGenerator(),
            FieldType.Checkbox => new BitFieldScriptGenerator(),
            FieldType.File => new NVarCharFieldScriptGenerator(),
            FieldType.Select => new UniqueIdentifierFieldScriptGenerator(),
            _ => throw new NotImplementedException($"Field type {fieldType} is not supported.")
        };
    }
}
