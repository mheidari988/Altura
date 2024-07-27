namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
public class CreateContentTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ContentTypeFieldDto> Fields { get; set; } = [];
}

public class ContentTypeFieldDto
{
    public Guid FieldId { get; set; }
    public string FieldName { get; set; } = string.Empty;
}