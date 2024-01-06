using FluentValidation;
using StaffNook.Domain.Dtos.User;
using StaffNook.Infrastructure.Common;

namespace StaffNook.Infrastructure.Validation;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Login).NotEmpty();
        //ToDo:: add password validator
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(StringValidationHelper.IsValidEmailAddress)
            .WithMessage("Неверный формат почты");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(StringValidationHelper.IsValidNumber)
            .WithMessage("Неверный формат номера телефона");
    }
}