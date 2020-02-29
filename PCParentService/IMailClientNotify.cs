using System.Net.Mail;

namespace PCParentServiceApp
{
    public interface IMailClientNotify
    {
        MailAddress ToEmail { get; set; }
        string FromEmail { get; set; }
        string Subject { get; set; }
        string EmailBody { get; set; }
        string SMTPServer { get; set; }
        int SMTPPort { get; set; }
        bool SendEmailNotification(int notifyType);
    }
}