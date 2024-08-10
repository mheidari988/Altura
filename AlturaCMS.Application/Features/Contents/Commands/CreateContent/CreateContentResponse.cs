namespace AlturaCMS.Application.Features.Contents.Commands.CreateContent;
public class CreateContentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ContentFieldDto> ContentFields { get; set; } = [];
}

public class ContentFieldDto
{
    public string FieldName { get; set; } = string.Empty;
}