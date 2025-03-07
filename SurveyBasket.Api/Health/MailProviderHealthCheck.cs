using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SurveyBasket.Api.Settings;

namespace SurveyBasket.Api.Health;

public class MailProviderHealthCheck(IOptions<MailSettings> mailsetting) : IHealthCheck
{

    private readonly MailSettings _mailsetting = mailsetting.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtp = new SmtpClient();


            smtp.Connect(_mailsetting.Host, _mailsetting.port, SecureSocketOptions.StartTls, cancellationToken);
            smtp.Authenticate(_mailsetting.Mail, _mailsetting.Password, cancellationToken);
            return await Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception exception)
        {
            return await Task.FromResult(HealthCheckResult.Unhealthy(exception: exception));

        }
    }
}
