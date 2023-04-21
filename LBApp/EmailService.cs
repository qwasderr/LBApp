using DocumentFormat.OpenXml.Vml;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Security.Authentication;

namespace LBApp
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Сайт Книжковий Магазин", "maxmarkov02@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("maxmarkov02@gmail.com", "otnekmvzvwpbidgi");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
