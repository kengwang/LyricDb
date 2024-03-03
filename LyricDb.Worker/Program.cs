using LyricDb.Worker.Services;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Oakton;
using Serilog;
using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.Transports.Tcp;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureLogging(loggerBuilder =>
{
    Log.Logger = new LoggerConfiguration()
#if !DEBUG
        .WriteTo.File("log/log.log", rollingInterval: RollingInterval.Day)
#endif
        .WriteTo.Console()
        .CreateLogger();
    loggerBuilder.ClearProviders();
    loggerBuilder.AddSerilog(Log.Logger);
});

builder.UseWolverine((context, options) =>
{
    options.UseRabbitMq(rabbit =>
    {
        rabbit.Uri = new Uri("amqp://guest:guest@rabbitmq:5672/");
    }).AutoProvision();
    options.ListenToRabbitQueue("lyricdb");
    options.PublishAllMessages().ToRabbitExchange("lyricdb");
});
builder.ConfigureServices((_, services) =>
{
    services.AddScoped<MailTransport>(serviceProvider =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var username = configuration.GetValue<string>("MailSender:UserName");
        var password = configuration.GetValue<string>("MailSender:Password");
        var host = configuration.GetValue<string>("MailSender:Host");
        var port = configuration.GetValue<int>("MailSender:Port");
        var smtpClient = new SmtpClient();
        smtpClient.Connect(host, port, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(username, password);
        return smtpClient;
    });
    services.AddSingleton<MailTemplateSelector>();
});
await builder.RunOaktonCommands(args);