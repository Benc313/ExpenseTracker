using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using expenseTracker.Messages;

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
	public string PasswordHash { get; set; }
	[Required]
	public bool Admin { get; set; } = false;
    
	public List<Transaction> Transactions { get; set; } = new();
	public List<RecurringPayment> RecurringPayments { get; set; } = new();
	
	public User(){}

	public User(RegisterRequest registerRequest)
	{
		Name = registerRequest.Name;
		Email = registerRequest.Email;
		PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
	}
}