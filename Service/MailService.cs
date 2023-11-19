using SendGrid.Helpers.Mail.Model;
using System.Net;
using System.Net.Mail;

namespace HotelServer.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail,  string subject, string content);
    }
    public class MailService : IMailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var mail = "tvh050423@gmail.com";
            var pw = "gxjb ouiy ohtz teym";

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw),
            };

            return client.SendMailAsync(
                new MailMessage(from: mail, to: toEmail, subject: subject, content) { IsBodyHtml = true});
        }
    }
}
