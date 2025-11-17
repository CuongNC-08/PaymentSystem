using Domain.Enum;

namespace Domain.Entity;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}