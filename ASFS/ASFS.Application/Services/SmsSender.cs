using System.Threading.Tasks;
using ASFS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASFS.Application.Services;

public class SmsSender : ISmsSender
{
    private readonly ILogger<SmsSender> _logger;

    public SmsSender(ILogger<SmsSender> logger)
    {
        _logger = logger;
    }

    public Task SendSmsAsync(string phoneNumber, string message)
    {
        // TODO: integrate with SMS gateway (Twilio or client SMS)
        _logger.LogInformation("Sending SMS to {Phone}: {Message}", phoneNumber, message);
        return Task.CompletedTask;
    }
}
