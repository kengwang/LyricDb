namespace LyricDb.Web.Interfaces;

public interface IEndpointBase
{
    public static abstract void ConfigureBuilder(WebApplicationBuilder builder);
    public static abstract void ConfigureApp(WebApplication app);
}