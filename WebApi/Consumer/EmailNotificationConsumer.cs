using Application.Contract.Event;
using Application.Service;
using Domain.Enum;
using MassTransit;

namespace WebApi.Consumer;

public class EmailNotificationConsumer : IConsumer<TransactionCompletedEvent>
{
    private readonly ILogger<TransactionPendingConsumer> _logger;
    private readonly IEmailService _emailService;

    public EmailNotificationConsumer(ILogger<TransactionPendingConsumer> logger,
        IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received transaction completed event: {TransactionId}", message.TransactionId);
        try
        {
            string subject, body;
            if (message.Status == TransactionStatus.Success)
            {
                subject = "Transaction Success";
                body = $"Your transaction {message.TransactionId} has been completed successfully.";
            }
            else
            {
                subject = "Transaction Failed";
                body = $"Your transaction {message.TransactionId} has failed.";
            }

            await _emailService.SendEmailAsync(subject, body);
            _logger.LogInformation("Email sent for transaction {TransactionId}", message.TransactionId);
        }
        catch (Exception)
        {
            _logger.LogError("Error sending email for transaction {TransactionId}", message.TransactionId);
        }
    }
}