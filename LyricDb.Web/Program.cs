using LyricDb.Web.Endpoints;
using LyricDb.Web.Extensions;
using Wolverine;
using Wolverine.RabbitMQ;
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
        options.UseRabbitMq(rabbit =>
        {
            rabbit.HostName = context.Configuration.GetValue<string>("RabbitMQ:HostName");
        }).AutoProvision();
        options.ListenToRabbitQueue("lyricdb-backend");
        options.PublishAllMessages().ToRabbitExchange("lyricdb-worker");
    }
);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.SetIsOriginAllowed(t => true);
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