using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
//using MailKit.Net.Smtp;
using MimeKit;
using Models.Auth;
using System.Reflection;
using MassTransit.RabbitMqTransport;

namespace Repositories
{
    public class EmailService : IEmailService
    {


        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = "example@outlook.com";
            var pw = "example";

            
            
            MailMessage message = new MailMessage();
            message.From = new MailAddress(mail);
            message.Subject = subject;
            message.To.Add(new MailAddress(to));
            message.Body = ($"<html><body> {body} </body></html>");
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
