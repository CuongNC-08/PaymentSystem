using Domain.Entity;
using Domain.Enum;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly PaymentDbContext _context;
    private readonly bool _autoSave;

    public TransactionRepository(PaymentDbContext context, bool autoSave = true)
    {
        _context = context;
        _autoSave = autoSave;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        if (_autoSave) await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        if (_autoSave) await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
        => await _context.Transactions.FindAsync(id);

    public async Task<(int SuccessCount, int FailedCount)> GetDailyStatsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var query = _context.Transactions
            .Where(t => t.CreatedDate >= today && t.CreatedDate < tomorrow);

        var successCount = await query.CountAsync(t => t.Status == TransactionStatus.Success);
        var failedCount = await query.CountAsync(t => t.Status == TransactionStatus.Failed);

        return (successCount, failedCount);
    }
}