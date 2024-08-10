using MediatR;

namespace AlturaCMS.Application.Features.Contents.Commands.UpdateContent;
public class UpdateContentHandler : IRequestHandler<UpdateContentCommand, UpdateContentResponse>
{
    public Task<UpdateContentResponse> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
