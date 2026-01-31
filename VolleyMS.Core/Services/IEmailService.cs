namespace VolleyMS.Core.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string body);
        public Task SendEmailsAsync(IEnumerable<string> emails, string subject, string body);
    }
}
