using Domain.IRepository;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentDbContext _context;
    private TransactionRepository? _transactionRepository;

    public UnitOfWork(PaymentDbContext context)
    {
        _context = context;
    }

    public ITransactionRepository Transactions 
        => _transactionRepository ??= new TransactionRepository(_context, false); 

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}