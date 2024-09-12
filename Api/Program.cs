using Api.Extensions;
using Api.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ServiceConfigurations(builder.Services, builder.Configuration);

var app = builder.Build();

Configurations(app);

static void ServiceConfigurations(IServiceCollection services, IConfiguration configuration)
{
    services.ConfigureInfrastructure();
    services.ConfigureServices();

    services.AddControllers();

    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "iHairCare", Version = "v1" });

        c.EnableAnnotations();
    });

    services.AddHealthChecks()
       .AddCheck("Self", () => HealthCheckResult.Healthy(), tags: new[] { "ready", "live" });
}

static void Configurations(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/health");

    app.MapControllers();

    app.Run();
}
