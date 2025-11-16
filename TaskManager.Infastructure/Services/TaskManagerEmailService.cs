using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using TaskManager.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MailKit.Security;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infastructure.Services
{
    public class TaskManagerEmailService:IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<TaskManagerEmailService> _logger;
        public TaskManagerEmailService(IOptions<EmailSettings> options, ILogger<TaskManagerEmailService> logger)
        {
            _emailSettings = options.Value;
            _logger= logger;    
        }

        public async Task<bool> SendEmailAsync(string toAddress, string subject, string body)
        {
            try
            {
                _logger.LogInformation("Inside SendEmailAsync method. To: {To}, Subject: {Subject}, Body: {Body}",toAddress, subject, body);
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toAddress));
                email.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = body };
                email.Body = builder.ToMessageBody();



                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port,SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                var resposne = await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Inside SendEmailAsync method {exception}:", JsonSerializer.Serialize(ex));
                return false;
            }
           
        }
    }
}
