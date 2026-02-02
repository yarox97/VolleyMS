using FluentValidation;
using Microsoft.EntityFrameworkCore.Update;
using VolleyMS.BusinessLogic.Features.Users.Modify;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Features.Users.Modify
{
    public sealed class ModifyUserValidator: AbstractValidator<ModifyUserCommand> 
    {
        public ModifyUserValidator()
        {
            RuleFor(um => um.Name)
                .MaximumLength(ValidationConfig.MAX_NAME_LENGTH)
                .WithMessage($"Name cannot have more than {ValidationConfig.MAX_NAME_LENGTH} symbols!")
                .Must(BeAValidNameOrSurname)
                .WithMessage("Please provide name in correct format");

            RuleFor(um => um.Surname)
                .MaximumLength(ValidationConfig.MAX_SURNAME_LENGTH)
                .WithMessage($"Name cannot have more than {ValidationConfig.MAX_SURNAME_LENGTH} symbols!")
                .Must(BeAValidNameOrSurname)
                .WithMessage("Please provide surname in correct format");
        }

        public bool BeAValidNameOrSurname(string? name)
        {
            if (name == null) { return true; }

            bool found = false;
            var trimmedName = name.Trim();

            foreach (char character in name)
            {
                if (char.IsDigit(character))
                { found = true; break; }
            }

            return found;
        }
    }
}
