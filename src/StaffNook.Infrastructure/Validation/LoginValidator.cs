using FluentValidation;
using StaffNook.Domain.Dtos.Identity;

namespace StaffNook.Infrastructure.Validation;

public class LoginValidator : AbstractValidator<LoginRequestDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}