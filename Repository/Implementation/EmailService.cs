using System.Net.Mail;
using Eletronic_Api.Repository.Abastract;
using MimeKit;

namespace Eletronic_Api.Repository.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendOtp(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PhsarTech", _config["EmailSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Your OTP Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP is: {otp}"
            };

            using var client = new MailKit.Net.Smtp.SmtpClient(); 
            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
