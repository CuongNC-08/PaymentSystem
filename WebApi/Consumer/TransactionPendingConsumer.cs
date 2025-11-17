using Application.Contract.Event;
using Application.Service;
using Domain.Enum;
using MassTransit;

namespace WebApi.Consumer;

public class TransactionPendingConsumer : IConsumer<TransactionPendingEvent>
{
    private readonly ILogger<TransactionPendingConsumer> _logger;
    private readonly ITransactionService _transactionService;

    public TransactionPendingConsumer(ILogger<TransactionPendingConsumer> logger,
        ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public async Task Consume(ConsumeContext<TransactionPendingEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received transaction pending event: {TransactionId}", message.TransactionId);
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(new Random().Next(3, 6)));
            var transaction = await _transactionService.GetTransactionByIdAsync(message.TransactionId);
            if (transaction == null || transaction.Status != TransactionStatus.Pending)
            {
                _logger.LogWarning("Transaction {TransactionId} not found or already processed.",
                    message.TransactionId);
            }

            bool isSuccess = new Random().Next(100) < 80;
            transaction.Status = isSuccess ? TransactionStatus.Success : TransactionStatus.Failed;
            await _transactionService.UpdateTransactionAsync(transaction);
            _logger.LogInformation("Transaction {TransactionId} updated to {Status}", message.TransactionId,
                transaction.Status);
            var completedEvent = new TransactionCompletedEvent(message.TransactionId, message.Amount,
                DateTime.UtcNow, transaction.Status);
            await context.Publish(completedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing transaction pending event: {TransactionId}", message.TransactionId);
        }
    }
}