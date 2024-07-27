using AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
using MediatR;

namespace AlturaCMS.API.Endpoints;

public static class ContentTypesEndpoints
{
    public static void MapContentTypesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/contenttypes", async (CreateContentTypeCommand command, IMediator mediator, HttpContext context) =>
        {
            var validator = new CreateContentTypeValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await mediator.Send(command);
            return Results.Created($"/api/contenttypes/{result.Id}", result);
        }).WithName("CreateContentType").WithOpenApi();
    }
}