namespace Application.Service;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string htmlMessage);
}