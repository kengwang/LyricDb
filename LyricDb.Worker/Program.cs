using LyricDb.Worker.Services;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Oakton;
using Serilog;
using Wolverine;
using Wolverine.Transports.Tcp;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureLogging(loggerBuilder =>
{
    Log.Logger = new LoggerConfiguration()
#if !DEBUG
        .WriteTo.File("log/log.log", rollingInterval: RollingInterval.Day)
#else
        .WriteTo.Console()
#endif
        .CreateLogger();
    loggerBuilder.ClearProviders();
    loggerBuilder.AddSerilog(Log.Logger);
});

builder.UseWolverine((context, options) =>
{
    options.ListenAtPort(context.Configuration.GetValue<int>("Wolverine:Worker"));
    options.PublishAllMessages().ToServerAndPort(context.Configuration.GetValue<string>("Wolverine:Web:Host")??"localhost", context.Configuration.GetValue<int>("Wolverine:Web:Port"));
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