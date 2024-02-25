using LyricDb.Web.Interfaces;

namespace LyricDb.Web.Endpoints;

public class SwaggerEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void ConfigureApp(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}