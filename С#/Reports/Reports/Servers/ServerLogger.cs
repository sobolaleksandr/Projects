using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Servers
{
    public class ServerLogger
    {
        public EventLog log;

        public ServerLogger()
        {
            CreateLogger();
        }

        void CreateLogger()
        {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("Reports"))
            {
                EventLog.CreateEventSource("Reports", "ReportLog");
                CreateLogger();
            }

            log = new EventLog
            {
                Source = "Reports"
            };
        }
    }
}
