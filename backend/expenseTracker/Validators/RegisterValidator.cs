using expenseTracker.Messages;
using FluentValidation;

namespace expenseTracker.Validators;


public class RegisterValidator : AbstractValidator<RegisterRequest>
{
	public RegisterValidator()
	{
		RuleFor(user => user.Name)
			.Length(4,16).WithMessage("Name must be between 4 and 16 characters");
		RuleFor(user => user.Email)
			.EmailAddress().WithMessage("Valid email is required")
			.Length(8,320).WithMessage("Email must be between 8 and 320 characters long");
		RuleFor(user => user.Password)
			.MinimumLength(6).WithMessage("Password must be at least 6 characters");
	}
}