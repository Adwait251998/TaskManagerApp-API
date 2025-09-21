using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TaskManager.Application.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;

        public SmsService(IConfiguration config)
        {
            _accountSid = config["Twilio:AccountSid"];
            _authToken = config["Twilio:AuthToken"];
            _fromNumber = config["Twilio:SmsFromNumber"];
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task<bool> SendSms(string to, string body)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_fromNumber),
                to: new Twilio.Types.PhoneNumber(to),
                body: body
            );
            return message.ErrorCode == null && message.Status != MessageResource.StatusEnum.Failed;
        }
    }
}
