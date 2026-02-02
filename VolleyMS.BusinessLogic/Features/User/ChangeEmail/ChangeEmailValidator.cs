using FluentValidation;

namespace VolleyMS.BusinessLogic.Features.Users.ChangeEmail
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmailCommand>
    {
        public ChangeEmailValidator()
        {
            RuleFor(c => c.email)
                .Must(BeAValidEmail)
                .WithMessage("Please provide a valid email!");
        }

        public bool BeAValidEmail(string? email)
        {
            if(email == null) { return true; } // if null then previous email stays.

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
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
    }
}
