namespace expenseTracker.Messages;

public class TransactionResponse
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int Amount { get; set; }
	public string Type { get; set; }// "income" or "expense"
	public string Category { get; set; }
	public DateTime Date { get; set; } = DateTime.UtcNow;
}