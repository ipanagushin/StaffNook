using FluentValidation;
using StaffNook.Domain.Dtos.Identity;

namespace StaffNook.Infrastructure.Validation;

public class RoleValidator : AbstractValidator<EditRoleDto>
{
    public RoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}