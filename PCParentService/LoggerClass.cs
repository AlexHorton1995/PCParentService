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

        public void CreateNewEventViewerLog()
        {
            eventId = 1;
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("PCParentService"))
                System.Diagnostics.EventLog.CreateEventSource("PCParentService", "PCParentLog");
            
            eventLog1.Source = "PCParentService";
            eventLog1.Log = "PCParentLog";
        }


        public void WriteLoginToEventViewer()
        {
            eventLog1.WriteEntry("Starting PCParentService, version " + Assembly.GetCallingAssembly());
            eventLog1.WriteEntry(string.Format("Process Started...{0} has logged in...", Environment.UserName), EventLogEntryType.Information, (int)EventLogTypes.Login);
        }

        public void WriteTransactionToEventViewer(string transaction)
        {
            using (EventLog eventLog = new EventLog("PCParentLog"))
            {
                eventLog.Source = "PCParentService";
                eventLog.WriteEntry(transaction, EventLogEntryType.Information, (int)EventLogTypes.Success, 1);
            }
        }

        public void WriteExceptionToEventViewer(string exception)
        {
            var exMessage = string.Format(@"There was an exception in the PCParent Service: {0}", exception);
            using (EventLog eventLog = new EventLog("PCParentLog"))
            {
                eventLog.Source = "PCParentService";
                eventLog.WriteEntry(exMessage, EventLogEntryType.Error, (int)EventLogTypes.Exception, 1);
            }
        }

        public void WriteLogoffToEventViewer()
        {

            var message = string.Format(@"User {0} Logged out at {1}", Environment.UserName, DateTime.Now.Date.ToString("MM-dd-yyyy HH:mm:ss"));
            using (EventLog eventLog = new EventLog("PCParentLog"))
            {
                eventLog.Source = "PCParentService";
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
