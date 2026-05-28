using System.Net;
using System.Net.Mail;

namespace APS_LostProperty.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "saininiket1@gmail.com";
            var password = "sirn efkp vzni qqha";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}