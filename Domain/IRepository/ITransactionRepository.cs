using Domain.Entity;

namespace Domain.IRepository;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task UpdateAsync(Transaction transaction);
    Task<(int SuccessCount, int FailedCount)> GetDailyStatsAsync();
}