using System.ServiceProcess;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    public partial class PCParentService : ServiceBase
    {
        //turn this into a console application now.
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            GetBrowserWindows();
            OnStop();
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        ILoggerClass logger = new LoggerClass();
        IMailClientNotify notify = new MailClientNotify();
        IBrowserSniffer sniffer = new BrowserSniffer();


        /// <summary>
        /// constructor for new service
        /// </summary>
        public PCParentService()
        {
            InitializeComponent();

            //creates a new Event Viewer Log 
            logger.CreateNewEventViewerLog();
        }

        protected override void OnStart(string[] args)
        {
            try
            {

#if DEBUG
                // Update the service state to Start Pending.
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    System.Threading.Thread.Sleep(30000);
                }
#endif

                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                serviceStatus.dwWaitHint = 100000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                //log the login to event viewer
                logger.WriteLoginToEventViewer();

                // Set up a timer that triggers every minute.
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 60000; // 1 minutes
                timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
                timer.Start();

                //send notification on start of service.
                notify.SendEmailNotification(1);

                // Update the service state to Running.
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            }
            catch (System.Exception ex)
            {
                logger.WriteExceptionToEventViewer(ex.Message);
            }
        }

        public void GetBrowserWindows()
        {
            var getSniffer = sniffer.PrintBrowserTabName();
            foreach (var browser in getSniffer)
            {
                logger.WriteTransactionToEventViewer(string.Format("Detected Browser Window(s): {0} at {1} for user {2}", browser, DateTime.Now.Date, Environment.UserName));
            }
        }


        protected override void OnContinue()
        {
            logger.WriteTransactionToEventViewer("In OnContinue Logic");
        }

        protected override void OnStop()
        {
            try
            {
                logger.WriteLogoffToEventViewer();
            }
            catch (System.Exception ex)
            {

                logger.WriteExceptionToEventViewer(ex.Message);
            }

        }

        #region Timer Logic
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            var getSniffer = sniffer.PrintBrowserTabName();
            foreach (var browser in getSniffer)
            {
                logger.WriteTransactionToEventViewer(string.Format("Detected Browser Window(s): {0} at {1} for user {2}", browser, DateTime.Now.Date, Environment.UserName));
            }

            //Process[] processlist = Process.GetProcesses();
            //foreach (Process p in processlist)
            //{
            //    if (p.ProcessName.Contains("firefox") || p.ProcessName.Contains("chrome"))
            //    {
            //    }
            //}
        }
        #endregion


    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
