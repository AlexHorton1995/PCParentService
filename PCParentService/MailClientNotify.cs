using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    internal class MailClientNotify : IMailClientNotify
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }



        /// <summary>
        /// Sends an email notification as needed
        /// </summary>
        /// <param name="notifyType">Dictates the subject and email that is sent via notification</param>
        /// <returns></returns>
        public bool SendEmailNotification(int notifyType)
        {
            try
            {
                //creates an SMTPClient to send the email.
                SmtpClient client = new SmtpClient(SMTPServer);
                ToEmail = "alex.d.horton95@gmail.com";

                switch (notifyType)
                {
                    case 1: //login
                        FromEmail = "login@PCParentService.com";
                        Subject = string.Format(@"User logged in at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                    case 2: //restricted
                        FromEmail = "restricted@PCParentService.com";
                        Subject = string.Format(@"Restricted usage at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                    case 3: //non-restricted
                        FromEmail = "nonrestricted@PCParentService.com";
                        Subject = string.Format(@"Internet usage at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                }


                //creates a new instance of MailMessage
                MailMessage message = new MailMessage(FromEmail, ToEmail, Subject, EmailBody);

                //if restricted activity, send a high priority message.
                if (notifyType == 2)
                    message.Priority = MailPriority.High;

                /*
                    // Add credentials if the SMTP server requires them.
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                 */

                try
                {
                    //send the message
                    client.Send(message);
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("There was an exception in the PCParent Service", EventLogEntryType.Information, 101, 1);
                    }

                }
                catch (SmtpException smtpEx)
                {


                    //log to the application log if an exception
                    //TODO - PUT LOGGING CODE HERE

                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
