namespace Application.Contract.Event;

public record TransactionPendingEvent(
    Guid TransactionId,
    decimal Amount
);