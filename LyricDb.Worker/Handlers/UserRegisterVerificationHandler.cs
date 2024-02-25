using System.Text.Encodings.Web;
using LyricDb.Contracts.Messages;
using LyricDb.Worker.Services;
using MailKit;
using MimeKit;
using Wolverine;

namespace LyricDb.Worker.Handlers;

public class UserRegisterVerificationHandler(
    ILogger<UserRegisterVerificationHandler> logger,
    MailTransport mailService,
    MailTemplateSelector templateSelector)
{
    public async Task Handle(UserRegisterVerificationMessage message, IMessageContext context)
    {
        logger.LogInformation("发送注册验证邮件给 {Name} <{Email}>", message.User.Name, message.User.Email);
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("LyricDB", "mailgun@kengwang.com.cn"));
        emailMessage.To.Add(new MailboxAddress(message.User.Name, message.User.Email));
        emailMessage.Subject = "[LyricDB] 账号注册验证";
        var body = await templateSelector.GetTemplateAsync("registration-verification");
        if (body is null)
        {
            logger.LogError("找不到邮件模板 registration-verification");
            return;
        }
        body = body.Replace("{{Name}}", message.User.Name)
        .Replace("{{Token}}", UrlEncoder.Default.Encode(message.ActivationToken))
        .Replace("{{Id}}", message.User.Id.ToString());
        
        emailMessage.Body = new TextPart("plain")
        {
            Text = body
        };
        await mailService.SendAsync(emailMessage);
    }
}