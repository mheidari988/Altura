using AlturaCMS.Domain.Entities;
using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
public class CreateContentTypeCommand : IRequest<CreateContentTypeResponse>
{
    public string Name { get; set; } = string.Empty;
    public List<CreateContentTypeFieldDto> Fields { get; set; } = [];
}

public class CreateContentTypeFieldDto
{
    public string Slug { get; set; } = string.Empty;

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

}
