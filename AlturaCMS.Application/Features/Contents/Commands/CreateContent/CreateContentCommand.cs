using AlturaCMS.Domain.Entities;
using MediatR;

namespace AlturaCMS.Application.Features.Contents.Commands.CreateContent;
public class CreateContentCommand : IRequest<CreateContentResponse>
{
    public string Name { get; set; } = string.Empty;
    public List<CreateContentTypeFieldDto> Fields { get; set; } = [];
}

public class CreateContentTypeFieldDto
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public FieldType FieldType { get; set; }

    public bool IsRequired { get; set; }

    public bool IsUnique { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public decimal? MinValue { get; set; }

    public decimal? MaxValue { get; set; }

    public DateTime? MinDateTime { get; set; }

    public DateTime? MaxDateTime { get; set; }

    public string? RegexPattern { get; set; } = string.Empty;

    public List<string>? AllowedValues { get; set; } = [];

    public string? ReferenceTableName { get; set; }

    public string? ReferenceDisplayFieldName { get; set; }
}
