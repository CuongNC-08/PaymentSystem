using Application.DTOs;
using Domain.Entity;

namespace Application.Service
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(TransactionCreateDto transaction);
        Task<Transaction?> GetTransactionByIdAsync(Guid id);
        Task UpdateTransactionAsync(Transaction transaction);
        Task<DailyStatsDto?> GetDailyStatsAsync();
    }

}