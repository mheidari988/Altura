using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
public class CreateContentTypeCommand : IRequest<CreateContentTypeResponse>
{
    public string Name { get; set; } = string.Empty;
    public List<CreateContentTypeFieldDto> Fields { get; set; } = [];
}

public class CreateContentTypeFieldDto
{
    public Guid FieldId { get; set; }
}
