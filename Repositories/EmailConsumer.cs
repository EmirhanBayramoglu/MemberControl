using MassTransit;
using Repositories.Config;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class EmailConsumer : IConsumer<EmailRequest>
    {
        private readonly IEmailService _emailService;

        public EmailConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailRequest> context)
        {
            var emailRequest = context.Message;
            await _emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
        }
    }
}
