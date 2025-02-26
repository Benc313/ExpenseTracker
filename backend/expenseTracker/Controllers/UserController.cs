using expenseTracker.Data;
using expenseTracker.Messages;
using expenseTracker.Models;
using expenseTracker.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace expenseTracker.Controllers;

[ApiController]
[Route("")]
public class UserController : ControllerBase
{
	public readonly AppDbContext _db;
    
	public UserController(AppDbContext db)
	{
		_db = db;
	}
	
	[HttpPost("register")]
	public async Task<ActionResult> Register(RegisterRequest registerRequest)
	{
		RegisterValidator validator = new RegisterValidator();
		ValidationResult result = validator.Validate(registerRequest);
		if (!result.IsValid)
			return BadRequest(result.Errors.Select(x => x.ErrorMessage));
		if (_db.Users.ToList().Exists(u => u.Email == registerRequest.Email))
			return BadRequest(new List<String>{"Email already exists"});

		_db.Users.Add(new User(registerRequest));
		_db.SaveChangesAsync();
		return Ok(); 
	}
	
}