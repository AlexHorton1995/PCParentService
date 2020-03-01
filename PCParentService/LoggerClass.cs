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
            eventId = 1;
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("PCParentService"))
                System.Diagnostics.EventLog.CreateEventSource("PCParentService", "PCParentLog");

            eventLog1.Source = "PCParentService";
            eventLog1.Log = "PCParentLog";
            eventLog1.WriteEntry("Starting PCParentService, version " + Assembly.GetCallingAssembly());
            eventLog1.WriteEntry("Process Started...", EventLogEntryType.Information, (int)EventLogTypes.Login);

        }

        public void WriteTransactionToEventViewer()
        {
            var message = "Successful Transaction";
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(message, EventLogEntryType.Information, (int)EventLogTypes.Success, 1);
            }
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

        public void WriteLogoffToEventViewer()
        {

            var message = string.Format(@"User Logged out at {0}", DateTime.Now.Date.ToString("MM-dd-yyyy HH:mm:ss"));
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(message, EventLogEntryType.Information, (int)EventLogTypes.Logoff, 1);
            }
        }


    }

    public enum EventLogTypes
    {
        Login = 100,
        Success = 101,
        TransactSuccess = 102,
        Exception = 103, 
        Logoff = 104
    }
}
