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

        // GET method to return a list of menu items
        endpoints.MapGet("/api/menu", async (IMediator mediator) =>
        {
            List<MenuItem> result = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = 1, Title = "Doctors", Slug = "doctors",
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Id = 101, Title = "General Practitioners", Slug = "doctors/general-practitioners" },
                        new MenuItem { Id = 102, Title = "Specialists", Slug = "doctors/specialists" }
                    }
                },
                new MenuItem { Id = 2, Title = "Hospitals", Slug = "hospitals" },
                new MenuItem { Id = 3, Title = "Departments", Slug = "departments" },
                new MenuItem
                {
                    Id = 4, Title = "Blogs", Slug = "blogs",
                    Children = new List<MenuItem>
                    {
                        new MenuItem { Id = 401, Title = "Health Tips", Slug = "blogs/health-tips" },
                        new MenuItem { Id = 402, Title = "News", Slug = "blogs/news" }
                    }
                },
                new MenuItem { Id = 5, Title = "Videos", Slug = "videos" },
                new MenuItem { Id = 6, Title = "Appointments", Slug = "appointments" },
                new MenuItem { Id = 7, Title = "Patient Records", Slug = "patient-records" },
                new MenuItem { Id = 8, Title = "Medical Research", Slug = "medical-research" },
                new MenuItem { Id = 9, Title = "Pharmacies", Slug = "pharmacies" },
                new MenuItem { Id = 10, Title = "Insurance", Slug = "insurance" },
                new MenuItem { Id = 11, Title = "Lab Results", Slug = "lab-results" },
                new MenuItem { Id = 12, Title = "Health Tips", Slug = "health-tips" },
                new MenuItem { Id = 13, Title = "Wellness Programs", Slug = "wellness-programs" },
                new MenuItem { Id = 14, Title = "Emergency Contacts", Slug = "emergency-contacts" },
                new MenuItem { Id = 15, Title = "Support Groups", Slug = "support-groups" },
                new MenuItem { Id = 16, Title = "Consultations", Slug = "consultations" },
                new MenuItem { Id = 17, Title = "Surgical Procedures", Slug = "surgical-procedures" },
                new MenuItem { Id = 18, Title = "Specialists", Slug = "specialists" },
                new MenuItem { Id = 19, Title = "Nursing Services", Slug = "nursing-services" },
                new MenuItem { Id = 20, Title = "Therapies", Slug = "therapies" },
                new MenuItem { Id = 21, Title = "Vaccinations", Slug = "vaccinations" },
                new MenuItem { Id = 22, Title = "Health Checkups", Slug = "health-checkups" },
                new MenuItem { Id = 23, Title = "Mental Health", Slug = "mental-health" },
                new MenuItem { Id = 24, Title = "Diet and Nutrition", Slug = "diet-and-nutrition" },
                new MenuItem { Id = 25, Title = "Contact Us", Slug = "contact-us" }
            };

            await Task.CompletedTask;
            return Results.Ok(result);
        }).WithName("GetMenu").WithOpenApi();
    }
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public List<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}