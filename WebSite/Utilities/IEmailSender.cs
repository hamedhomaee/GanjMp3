namespace WebSite.Utilities;
public interface IEmailSender
{
    public void Send(string key, string fromAddress, string fromName, string subject, string toAddress, string toName, string plainTextContent, string htmlContent);
}