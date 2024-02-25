using LyricDb.Web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LyricDb.Web.Endpoints;

public class DatabaseEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        builder.Services.AddDbContextPool<PostgresDbContext>(option =>
        {
            option.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });
        builder.Services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
    }

    public static void ConfigureApp(WebApplication app)
    {
        // ingore
    }
}