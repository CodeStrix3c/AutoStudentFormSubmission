using System.Threading.Tasks;
using ASFS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASFS.Application.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        // TODO: integrate with SMTP, SendGrid, etc.
        _logger.LogInformation("Sending email to {To}: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}
