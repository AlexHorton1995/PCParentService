using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    class LoggerClass : ILoggerClass
    {
        private int eventId = 1;
        private static EventLog eventLog1 = new EventLog();

        public void WriteLoginToEventViewer()
        {
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("PCParentService"))
                System.Diagnostics.EventLog.CreateEventSource("PCParentService", "PCParentLog");

            eventLog1.Source = "PCParentService";
            eventLog1.Log = "PCParentLog";

            eventLog1.WriteEntry("Starting PCParentService, version " + Assembly.GetCallingAssembly());
        }

        public void WriteExceptionToEventViewer(string exception)
        {
            var exMessage = string.Format(@"There was an exception in the PCParent Service: {0}", exception);
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(exMessage, EventLogEntryType.Error, (int)EventLogTypes.Exception, 1);
            }
        }


    }

    public enum EventLogTypes
    {
        Success = 100,
        Exception = 102
    }
}
