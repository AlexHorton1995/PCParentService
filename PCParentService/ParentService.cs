using System.ServiceProcess;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    public partial class PCParentService : ServiceBase
    {
        ILoggerClass logger = new LoggerClass();
        IMailClientNotify notify = new MailClientNotify();

        /// <summary>
        /// constructor for new service
        /// </summary>
        public PCParentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Set up a timer that triggers every minute.
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 60000; // 60 seconds
                timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
                timer.Start();

                //log the login to event viewer
                logger.WriteLoginToEventViewer();

                //send notification on start of service.
                notify.SendEmailNotification(1);
            }
            catch (System.Exception ex)
            {
                logger.WriteExceptionToEventViewer(ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                //stop logic here
            }
            catch (System.Exception ex)
            {

                logger.WriteExceptionToEventViewer(ex.Message);
            }

        }

        #region Timer Logic
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.

            Process[] processlist = Process.GetProcesses();

            foreach (Process p in processlist)
            {
                if (p.ProcessName.Contains("firefox"))

                Console.WriteLine("Process: {0} ID: {1}", p.ProcessName, p.Id);
            }

            logger.WriteTransactionToEventViewer();
        }
        #endregion
    }
}
