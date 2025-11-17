using Domain.Enum;

namespace Application.Contract.Event;

public record TransactionCompletedEvent(
    Guid TransactionId,
    decimal Amount,
    DateTime Timestamp,
    TransactionStatus Status
);