using LyricDb.Web.Endpoints;
using LyricDb.Web.Extensions;
using Wolverine;
using Wolverine.Transports.Tcp;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHttpContextAccessor();
builder.AddEndpoint<LoggerEndpoint>();
builder.AddEndpoint<DatabaseEndpoint>();
builder.AddEndpoint<MapperEndpoint>();
builder.AddEndpoint<UserEndpoint>();
builder.AddEndpoint<SongEndpoint>();
builder.AddEndpoint<LyricEndpoint>();
builder.AddEndpoint<SwaggerEndpoint>();
builder.AddEndpoint<ALRCEndpoint>();
builder.Host.UseWolverine(
    (context, options) =>
    {
        options.PublishAllMessages().ToServerAndPort(context.Configuration.GetValue<string>("Wolverine:Worker:Server")??"localhost", context.Configuration.GetValue<int>("Wolverine:Worker:Port"));
        options.ListenAtPort(context.Configuration.GetValue<int>("Wolverine:Web"));
    }
); 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.SetIsOriginAllowed(t=>true);
        config.AllowCredentials();
        config.AllowAnyHeader();
        config.AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseEndpoint<SwaggerEndpoint>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoint<UserEndpoint>();
app.UseEndpoint<LyricEndpoint>();
app.UseEndpoint<SongEndpoint>();
app.Run();