using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebSite.Utilities;
public class SendGridEmailSender : IEmailSender
{
    public void Send(string key, string fromAddress, string fromName, string subject, string toAddress, string toName, string plainTextContent, string htmlContent)
    {
        var apiKey = Environment.GetEnvironmentVariable(key);
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(fromAddress, fromName);
        var to = new EmailAddress(toAddress, toName);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        
        client.SendEmailAsync(msg).Wait();
    }
}