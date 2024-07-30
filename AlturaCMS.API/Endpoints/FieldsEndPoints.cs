using AlturaCMS.Application.Features.Fields.Queries.GetFields;
using MediatR;

namespace AlturaCMS.API.Endpoints;

public static class FieldsEndPoints
{
    public static void MapFieldsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/fields", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetFieldsQuery());
            return Results.Ok(result);
        }).WithName("GetFields").WithOpenApi();
    }
}
