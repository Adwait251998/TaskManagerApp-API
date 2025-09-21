using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TaskManager.Application.Services
{
    public class WhatsAppService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioNumber;

        public WhatsAppService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _twilioNumber = configuration["Twilio:WhatsappFromNumber"];

            TwilioClient.Init(_accountSid, _authToken);
        }


        public void SendMessage(string to, string message)
        {
            MessageResource.Create(
                to: new PhoneNumber($"whatsapp:{to}"),
                from: new PhoneNumber(_twilioNumber),
                body: message
            );
        }
    }
}
