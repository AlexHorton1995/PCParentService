using System.ServiceProcess;
using System.Timers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    public partial class PCParentService : ServiceBase
    {
        ILoggerClass logger;

        /// <summary>
        /// constructor for new service
        /// </summary>
        public PCParentService()
        {
            InitializeComponent();
            logger = new LoggerClass();
            logger.WriteLoginToEventViewer();
        }

        protected override void OnStart(string[] args)
        {
            // Set up a timer that triggers every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

            IMailClientNotify notify = new MailClientNotify();

            //send notification on start of service.
            notify.SendEmailNotification(1);
        }

        protected override void OnStop()
        {
        }

        #region Timer Logic
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
        }
        #endregion
    }
}
