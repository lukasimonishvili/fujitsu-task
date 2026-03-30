using Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io")
            {
                Port = 2525,
                Credentials = new NetworkCredential(
                    "78772b354c4394",
                    "d71b9d065b594b"
                ),
                EnableSsl = true,
                Timeout = 5000
            };

            var mail = new MailMessage
            {
                From = new MailAddress("simonishvililuka@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await smtpClient.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("EMAIL FAILED: " + ex.Message);
        }
    }
}