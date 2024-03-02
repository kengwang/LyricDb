using LyricDb.Web.Interfaces;
using Serilog;

namespace LyricDb.Web.Endpoints;

public class LoggerEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
#if !DEBUG
            .WriteTo.File("log/log.log", rollingInterval: RollingInterval.Day)
#endif
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    public static void ConfigureApp(WebApplication app)
    {
        // ignore
    }
}