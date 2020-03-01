﻿using System;
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
                        logger.WriteTransactionToEventViewer();

                    }
                    catch (SmtpException smtpEx)
                    {
                        //if email isn't sent, write an exception to the event log
                        logger.WriteExceptionToEventViewer(smtpEx.Message);
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
