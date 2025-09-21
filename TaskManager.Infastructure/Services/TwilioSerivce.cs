using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;

namespace TaskManager.Infastructure.Services
{
    public class TwilioService
    {
        private readonly IConfiguration _configuration;

        public TwilioService(IConfiguration config)
        {
            _configuration = config;
            TwilioClient.Init(_configuration["Twilio:AccountSid"], _configuration["Twilio:AuthToken"]);
        }

        public async Task<MessageResource> SendWhatsappMessage(string to, string body)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_configuration["Twilio:WhatsappFromNumber"]),
                to: new Twilio.Types.PhoneNumber($"whatsapp:{to}"),
                body: body
            );
            return message;
        }
    }

}
