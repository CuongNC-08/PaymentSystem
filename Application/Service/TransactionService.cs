using System.Text.Json;
using Application.DTOs;
using Domain.Entity;
using Domain.IRepository;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Service;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(IUnitOfWork unitOfWork, IDistributedCache cache, ILogger<TransactionService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto dto)
    {
        var transaction = dto.Adapt<Transaction>();
        await _unitOfWork.Transactions.AddAsync(transaction);
        await _unitOfWork.CommitAsync();
        return transaction;
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        return await _unitOfWork.Transactions.GetByIdAsync(id);
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        await _unitOfWork.Transactions.UpdateAsync(transaction);
        await _unitOfWork.CommitAsync();
    }

    public async Task<DailyStatsDto?> GetDailyStatsAsync()
    {
        const string cacheKey = "daily_stats";
        string? stats = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(stats))
        {
            _logger.LogInformation("Cache HIT for stats");
            return JsonSerializer.Deserialize<DailyStatsDto>(stats);
        }

        _logger.LogInformation("Cache MISS for stats");
        var statsResult = await _unitOfWork.Transactions.GetDailyStatsAsync();
        var statsDto = new DailyStatsDto
        {
            SuccessCount = statsResult.SuccessCount,
            FailedCount = statsResult.FailedCount
        };
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };
        _logger.LogInformation("Cache SET for stats");
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(statsDto), options);
        return statsDto;
    }
}