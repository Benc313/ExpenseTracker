using expenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace expenseTracker.Data;

public class AppDbContext : DbContext
{
	protected readonly IConfiguration _configuration;
	
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	
	public DbSet<User> Users { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<RecurringPayment> RecurringPayments { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// User
		modelBuilder.Entity<User>()
			.HasMany(u => u.Transactions)
			.WithOne(t => t.User)
			.HasForeignKey(t => t.UserId)
			.OnDelete(DeleteBehavior.Cascade);
        
		modelBuilder.Entity<User>()
			.HasMany(u => u.RecurringPayments)
			.WithOne(r => r.User)
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		
	}
}