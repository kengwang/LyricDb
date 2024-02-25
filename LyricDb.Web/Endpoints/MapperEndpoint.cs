using System.Reflection;
using LyricDb.Web.Interfaces;

namespace LyricDb.Web.Endpoints;

public class MapperEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // get all types that implement IMapper<,>
        var types = typeof(MapperEndpoint).Assembly.DefinedTypes;
        foreach (var type in types)
        {
            var list = type.ImplementedInterfaces
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>))
                .ToList();
            foreach (var imp in list)
            {
                builder.Services.AddScoped(imp, type);
            }
        }
    }

    public static void ConfigureApp(WebApplication app)
    {
        // ignore
    }
}