using LyricDb.Web.Interfaces;
using Microsoft.OpenApi.Models;

namespace LyricDb.Web.Endpoints;

public class SwaggerEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddServer(new OpenApiServer
            {
                Description = "Production",
                Url = "https://lyricdb.kengwang.com.cn/api/"
            });
            options.AddServer(new OpenApiServer
            {
                Description = "Development",
                Url = "http://localhost:5140/"
            });
        });
    }

    public static void ConfigureApp(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}