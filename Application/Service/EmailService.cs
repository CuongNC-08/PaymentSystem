using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Application.Service;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _config;

    public EmailService(ILogger<EmailService>logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
    public async Task SendEmailAsync( string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(_config["EmailSettings:To"]));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = htmlMessage };
        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["EmailSettings:SmtpHost"], 
            int.Parse(_config["EmailSettings:SmtpPort"] ?? string.Empty), 
            SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(
            _config["EmailSettings:SmtpUser"], 
            _config["EmailSettings:SmtpPass"]);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}
