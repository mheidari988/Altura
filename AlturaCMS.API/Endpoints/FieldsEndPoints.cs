using AlturaCMS.Application.Features.Fields.Queries.GetFieldTypes;
using MediatR;

namespace AlturaCMS.API.Endpoints;

public static class FieldsEndPoints
{
    public static void MapFieldsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/fieldtypes", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetFieldTypesQuery());
            return Results.Ok(result);
        }).WithName("GetFieldTypes").WithOpenApi();
    }
}
