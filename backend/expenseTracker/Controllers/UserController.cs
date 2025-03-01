using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using expenseTracker.Data;
using expenseTracker.Messages;
using expenseTracker.Models;
using expenseTracker.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace expenseTracker.Controllers;

/// <summary>
/// Controller for user-related actions such as registration, login, and logout.
/// </summary>
[ApiController]
[Route("")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    /// Initializes a new instance of the <see cref="UserController"/> class.
    public UserController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    /// Registers a new user.
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest registerRequest)
    {
        RegisterValidator validator = new RegisterValidator();
        ValidationResult result = validator.Validate(registerRequest);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(x => x.ErrorMessage));
        if (_db.Users.ToList().Exists(u => u.Email == registerRequest.Email))
            return BadRequest(new List<string> { "Email already exists" });

        _db.Users.Add(new User(registerRequest));
        await _db.SaveChangesAsync();
        return Ok();
    }

    // Logs in a user.
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest loginRequest)
    {
        var user = _db.Users.Where(u => u.Email == loginRequest.Email).FirstOrDefault();
        if (user == null)
            return BadRequest(new List<string> { "Invalid Email or Password" });
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            return BadRequest(new List<string> { "Invalid Email or Password" });

        var token = GenerateJwtToken();
        Response.Cookies.Append("accessToken", token,
            new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
        return Ok();
    }

    // Generates a JWT token for the authenticated user.
    private string GenerateJwtToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "LocalMe",
            audience: "LocalMeAudience",
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Logs out the authenticated user.
    [HttpPost("logout")]
    [Authorize]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("accessToken");
        return Ok();
    }
}