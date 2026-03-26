using Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("lukasimonishvili1998@gmail.com", "tzvj nxvs rnhw cnnl"),
            EnableSsl = true,
        };

        var mail = new MailMessage
        {
            From = new MailAddress("lukasimonishvili1998@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtpClient.SendMailAsync(mail);
    }
}