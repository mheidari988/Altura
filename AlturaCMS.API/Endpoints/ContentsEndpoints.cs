using AlturaCMS.Application.Features.Contents.Commands.CreateContent;
using AlturaCMS.Application.Features.Contents.Queries.GetContents;
using MediatR;

namespace AlturaCMS.API.Endpoints;

public static class ContentsEndpoints
{
    public static void MapContentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/Contents", async (CreateContentCommand command, IMediator mediator, HttpContext context) =>
        {
            var validator = new CreateContentValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await mediator.Send(command);
            return Results.Created($"/api/Contents/{result.Id}", result);
        }).WithName("CreateContent").WithOpenApi();

        endpoints.MapGet("/api/Contents", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetContentsQuery());
            return Results.Ok(result);
        }).WithName("GetContents").WithOpenApi();
    }
}