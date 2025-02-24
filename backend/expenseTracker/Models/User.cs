using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace expenseTracker.Models;

public class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[Required]
	[Column(TypeName = "varchar(16)")]
	public string Name { get; set; }
	[Required]
	[Column(TypeName = "varchar(320)")]
	public string Email { get; set; }
	[Required]
	public string Password { get; set; }
    
	public List<Transaction> Transactions { get; set; } = new();
	public List<RecurringPayment> RecurringPayments { get; set; } = new();
}