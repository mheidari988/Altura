using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.UpdateContentType;
public class UpdateContentTypeHandler : IRequestHandler<UpdateContentTypeCommand, UpdateContentTypeResponse>
{
    public Task<UpdateContentTypeResponse> Handle(UpdateContentTypeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
