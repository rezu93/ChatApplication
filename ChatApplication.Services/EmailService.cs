using ChatApplication.Domain;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ChatApplication.Services
{
    public class EmailService : IEmailService
    {

        private readonly string _apiKey;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["Application:APIKeyForSendGrid"];
        }

        public void SendEmail(string email, string message)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("test-email@gmail.com", "Administrator");
            var subject = "You've got a new message!";
            var to = new EmailAddress(email);
            var plainTextContent = $"You've got a new message, that is: {message}";
            var htmlContent = $"<strong>You've got a new message, <br /> that is: {message}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception("Something went wrong");
            }
        }
    }
}
