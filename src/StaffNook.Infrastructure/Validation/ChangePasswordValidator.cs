using FluentValidation;
using StaffNook.Domain.Dtos.Identity;

namespace StaffNook.Infrastructure.Validation;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CurrentPassword).NotEmpty();
        //ToDo:: add password validator
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}