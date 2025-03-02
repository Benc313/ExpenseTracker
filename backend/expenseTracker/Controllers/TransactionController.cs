using expenseTracker.Data;
using expenseTracker.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace expenseTracker.Controllers;

[ApiController]
[Route("")]
public class TransactionController : ControllerBase
{
	private readonly AppDbContext _db;
	public TransactionController(AppDbContext db)
	{
		_db = db;
	}

	
	[HttpGet("transaction/{userId}/current-month")]
	public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransactionsForCurrentMonth(int userId)
	{
		if(_db.Users.Find(userId) == null)
			return NotFound("User not found");
		var currentMonth = DateTime.UtcNow.Month;
		var currentYear = DateTime.UtcNow.Year;

		var transactions = await _db.Transactions
			.Where(t => t.UserId == userId && t.Date.Month == currentMonth && t.Date.Year == currentYear)
			.Select(t => new TransactionResponse
			{
				Id = t.Id,
				UserId = t.UserId,
				Amount = t.Amount,
				Type = t.Type,
				Category = t.Category,
				Date = t.Date
			})
			.ToListAsync();

		return Ok(transactions);
	}

}