namespace AlturaCMS.API.Endpoints;

public static class FormsEndpoints
{
    public static void MapFormsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/forms", () =>
        {
            // Your implementation here
            return Results.Ok("Forms endpoint");
        }).WithOpenApi();

        endpoints.MapPost("/api/forms", () =>
        {
            // Your implementation here
            return Results.Ok("Form created");
        }).WithOpenApi();
    }
}