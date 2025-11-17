namespace Domain.IRepository;

public interface IUnitOfWork : IDisposable
{
    ITransactionRepository Transactions { get; }

    Task<int> CommitAsync(); 
}