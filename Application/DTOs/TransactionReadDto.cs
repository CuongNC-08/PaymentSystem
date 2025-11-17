using Domain.Enum;

namespace Application.DTOs;

public class TransactionReadDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
}