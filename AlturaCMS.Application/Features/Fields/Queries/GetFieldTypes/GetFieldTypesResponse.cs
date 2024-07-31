namespace AlturaCMS.Application.Features.Fields.Queries.GetFieldTypes;
public class GetFieldTypesResponse
{
    public List<FieldTypeDto> FieldTypes { get; set; } = [];
}

public class FieldTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}