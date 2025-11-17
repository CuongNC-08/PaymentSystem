using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
}