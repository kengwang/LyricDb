using LyricDb.Web.Interfaces;

namespace LyricDb.Web.Extensions;

public static class EndpointExtensions
{
    public static void AddEndpoint<TEndpoint>(this WebApplicationBuilder builder) where TEndpoint : IEndpointBase
    {
        TEndpoint.ConfigureBuilder(builder);
    }
    
    public static void UseEndpoint<TEndpoint>(this WebApplication app) where TEndpoint : IEndpointBase
    {
        TEndpoint.ConfigureApp(app);
    }
}