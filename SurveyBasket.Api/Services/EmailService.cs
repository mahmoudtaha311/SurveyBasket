
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SurveyBasket.Api.Settings;

namespace SurveyBasket.Api.Services;

public class EmailService(IOptions<MailSettings> mailsetting, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSettings _mailsetting = mailsetting.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailsetting.Mail),
            Subject = subject,

        };
        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();

        _logger.LogInformation("sending to {email}", email);

        smtp.Connect(_mailsetting.Host, _mailsetting.port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailsetting.Mail, _mailsetting.Password);
        await smtp.SendAsync(message);
        smtp.Disconnect(true);
    }
}
