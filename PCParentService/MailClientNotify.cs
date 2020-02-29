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
        public MailAddress ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string SMTPServer { get; set; }
        public string EmailPassword { get; set; }
        public int SMTPPort { get; set; }
        ILoggerClass logger = new LoggerClass();

        /// <summary>
        /// Sends an email notification as needed
        /// </summary>
        /// <param name="notifyType">Dictates the subject and email that is sent via notification</param>
        /// <returns></returns>
        public bool SendEmailNotification(int notifyType)
        {
            try
            {

                switch (notifyType)
                {
                    case 1: //login
                        Subject = string.Format(@"User logged in at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                    case 2: //restricted
                        Subject = string.Format(@"Restricted usage at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                    case 3: //non-restricted
                        Subject = string.Format(@"Internet usage at {0:HH:mm:ss on MMddyyyy}", DateTime.Now);
                        break;
                }

                using (SmtpClient client = new SmtpClient())
                using (MailMessage msg = new MailMessage())
                {
                    msg.To.Add(ToEmail);
                    msg.From = new MailAddress(FromEmail);
                    msg.Subject = Subject;

                    //if restricted activity, send a high priority message.
                    if (notifyType == 2)
                        msg.Priority = MailPriority.High;

                    client.Port = SMTPPort;
                    client.Host = SMTPServer;
                    client.EnableSsl = true;

#if DEBUG
                    //Set port, host, and to emails here;
                    FromEmail = "*****";
                    ToEmail = new MailAddress("*****");
                    SMTPPort = 587;
                    SMTPServer = "smtp.live.com";
#else
                //Set port, host, and to emails here;
                FromEmail = "*****";
                ToEmail = new MailAddress("*****");
                SMTPPort = 587;
                SMTPServer = "smtp.live.com";
#endif
                    //put the outlook.com email and password here (masked, of course)
                    client.Credentials = new System.Net.NetworkCredential("****", "****");

                    try
                    {
                        client.Send(msg);
                        //TODO - put a logger.writeevent here
                    }
                    catch (SmtpException smtpEx)
                    {
                        //if email isn't sent, write an exception to the event log
                        logger.WriteExceptionToEventViewer(smtpEx.Message);

                        /*
                         {"Transaction failed. The server response was: 5.2.0 STOREDRV.Submission.Exception:SendAsDeniedException.MapiExceptionSendAsDenied; 
                         Failed to process message due to a permanent exception with message Cannot submit message. 
                         0.35250:CF050000, 1.36674:0A000000, 1.61250:00000000, 1.45378:02000000, 1.44866:55020000, 1.36674:0E000000, 1.61250:00000000, 1.45378:5A020000, 1.44866:58010000, 16.55847:18040000, 17.43559:0000000024020000000000000000000000000000, 20.52176:140FD48A0C00001000000000, 20.50032:140FD48A7C1700100A004287, 0.35180:0A000000, 255.23226:00000000, 255.27962:0A000000, 255.27962:0E000000, 255.31418:0A000000, 0.35250:00000000, 1.36674:0A000000, 1.61250:00000000, 1.45378:02000000, 1.44866:5C000000, 1.36674:32000000, 1.61250:00000000, 1.45378:61000000, 1.44866:01000000, 16.55847:C8000000, 17.43559:0000000078030000000000000100000000000000, 20.52176:140FD48A0C00F01F0B060000, 20.50032:140FD48A7C1710100A000000, 0.35180:00000000, 255.23226:02000000, 255.27962:0A000000, 255.27962:32000000, 255.17082:DC040000, 0.27745:00000000, 4.21921:DC040000, 255.27962..."}
                         */
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.WriteExceptionToEventViewer(ex.Message);
                return false;
            }
        }
    }
}
