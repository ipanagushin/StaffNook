using FluentValidation;
using StaffNook.Domain.Dtos.User;

namespace StaffNook.Infrastructure.Validation;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Role).NotEmpty();
    }
}