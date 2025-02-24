using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace expenseTracker.Models;

public class RecurringPayment
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public int UserId { get; set; }
	public decimal Amount { get; set; }
	[Required]
	[Column(TypeName = "varchar(64)")]
	public string Category { get; set; }
	public string? Description { get; set; }
	[Column(TypeName = "Date")]
	public DateTime StartDate { get; set; } = DateTime.UtcNow;
	[Column(TypeName = "Date")]
	public DateTime NextDueDate { get; set; } = DateTime.UtcNow;
	public string Status { get; set; } = "active"; // "active" or "canceled"
	
	[Required]
	public User User { get; set; }

}