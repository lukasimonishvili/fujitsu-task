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
            var emailUser = Environment.GetEnvironmentVariable("lukasimonishvili1998@gmail.com");
            var emailPass = Environment.GetEnvironmentVariable("tzvj nxvs rnhw cnnl");

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailUser, emailPass),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress(emailUser!),
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