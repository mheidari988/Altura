using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFields;
public class GetFieldsResponse
{
    public List<FieldDto> Fields { get; set; } = [];
}

public class FieldDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }
    public string? RegexPattern { get; set; } = string.Empty;
    public List<string>? AllowedValues { get; set; } = [];
}