using System.Net;
using System.Net.Mail;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _sender = "noreply.VolleyMS@gmail.com";
        private readonly string _password = @"khbh swfr wliz slrw ";

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var fromAddress = new MailAddress(_sender, "VolleyMS");
            var toAddress = new MailAddress(email);

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_sender, _password)
            };          

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailsAsync(IEnumerable<string> emails, string subject, string body)
        {
            var fromAddress = new MailAddress(_sender, "VolleyMS");
            var mailMessage = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var email in emails)
            {
                mailMessage.Bcc.Add(email);
            }

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_sender, _password)
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
