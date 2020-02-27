using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PCParentService
{
    public partial class PCParentService : ServiceBase
    {
        private int eventId = 1;

        /// <summary>
        /// constructor for new service
        /// </summary>
        public PCParentService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists("PCParentService"))
                System.Diagnostics.EventLog.CreateEventSource("PCParentService", "PCParentLog");

            eventLog1.Source = "PCParentService";
            eventLog1.Log = "PCParentLog";


        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Starting PCParentService, version " + Assembly.GetCallingAssembly());
            // Set up a timer that triggers every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
        }

        #region Timer Logic
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        #endregion
    }
}
