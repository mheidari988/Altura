using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Services.Persistence.Dynamic;
public interface IFieldScriptGenerator
{
    string GenerateFieldScript(ContentField field);
}