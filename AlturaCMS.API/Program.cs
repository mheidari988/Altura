using AlturaCMS.API.Endpoints;
using AlturaCMS.Application;
using AlturaCMS.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/altura.log", rollingInterval: RollingInterval.Day)
        .Enrich.WithProperty("ApplicationName", "AlturaCMS")
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAlturaPersistence(builder.Configuration);
builder.Services.AddAlturaApplication(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapContentsEndpoints();
app.MapFieldsEndpoints();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (errorFeature != null)
        {
            Log.Error(errorFeature.Error, "Unhandled exception occurred.");
            await context.Response.WriteAsync("An error occurred, please try again later.");
        }
    });
});

app.Run();