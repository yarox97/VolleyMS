using FluentValidation;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration
{
    public sealed class RegistrationValidator : AbstractValidator<RegistrationCommand>
    {
        public RegistrationValidator()
        {
            RuleFor(ur => ur.userName)
                .NotEmpty()
                .MinimumLength(ValidationConfig.MIN_USERNAME_LENGHT)
                .WithMessage($"Username must have at least { ValidationConfig.MIN_USERNAME_LENGHT } symbols!")
                .MaximumLength(ValidationConfig.MAX_USERNAME_LENGTH)
                .WithMessage($"Username cannot have more than { ValidationConfig.MAX_USERNAME_LENGTH } symbols!");

            RuleFor(ur => ur.password)
                .NotEmpty()
                .MinimumLength(ValidationConfig.MIN_PASSWORD_LENGTH)
                .WithMessage($"Password must have at least { ValidationConfig.MIN_PASSWORD_LENGTH} symbols! ");

            RuleFor(ur => ur.email)
                .NotEmpty()
                .Must(BeAValidEmail)
                .WithMessage($"Please provide email in correct format");

            RuleFor(ur => ur.name)
                .NotEmpty()
                .MaximumLength(ValidationConfig.MAX_NAME_LENGTH)
                .WithMessage($"Name cannot have more than {ValidationConfig.MAX_NAME_LENGTH} symbols!")
                .Must(BeAValidNameOrSurname)
                .WithMessage("Please provide name in correct format");

            RuleFor(ur => ur.surname)
                .NotEmpty()
                .MaximumLength(ValidationConfig.MAX_SURNAME_LENGTH)
                .WithMessage($"Name cannot have more than {ValidationConfig.MAX_SURNAME_LENGTH} symbols!")
                .Must(BeAValidNameOrSurname)
                .WithMessage("Please provide surname in correct format");
        }

        public bool BeAValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if(trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address.Equals(trimmedEmail);
            }
            catch { return false; }
        }

        public bool BeAValidNameOrSurname(string name)
        {
            var trimmedName = name.Trim();

            foreach(char character in name)
            {
                if(char.IsDigit(character)) 
                    { return false; }
            }

            return true;
        }
    }
}
