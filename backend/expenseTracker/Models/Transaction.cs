using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace expenseTracker.Models;

public class Transaction
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public int UserId { get; set; }
	[Required]
	public int Amount { get; set; }
	[Required]
	[Column(TypeName = "varchar(16)")]
	public string Type { get; set; }// "income" or "expense"
	[Required]
	[Column(TypeName = "varchar(32)")]
	public string Category { get; set; }
	public string? Description { get; set; }
	[Column(TypeName = "Date")]
	public DateTime Date { get; set; } = DateTime.UtcNow;
    
	[Required]
	public User User { get; set; }
}