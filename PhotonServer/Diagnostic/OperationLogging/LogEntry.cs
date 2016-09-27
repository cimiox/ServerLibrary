using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer.Diagnostic.OperationLogging
{
    public class LogEntry
    {
        public DateTime UtcCreated { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }

        public LogEntry(DateTime utcCreated, string action, string message)
        {
            UtcCreated = utcCreated;
            Action = action;
            Message = message;
        }

        public LogEntry(string action, string message)
            : this(DateTime.UtcNow, action, message)
        {
        }

        public override string ToString()
        {
            return string.Format($"{UtcCreated} - {Action}: {Message}");
        }
    }
}
