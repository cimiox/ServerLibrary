using ExitGames.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer.Diagnostic.OperationLogging
{
    public class LogQueue
    {
        public const int DefaultCapacity = 1000;

        public readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly int capacity;
        private readonly string name;
        private readonly Queue<LogEntry> queue;

        public LogQueue(string name, int capacity)
        {
            this.capacity = capacity;
            queue = new Queue<LogEntry>(capacity);
            this.name = name;
        }

        public void Add(LogEntry value)
        {
            if (Log.IsDebugEnabled)
            {
                if (queue.Count == capacity)
                {
                    queue.Dequeue();
                }

                queue.Enqueue(value);
            }
        }

        public void WriteLog()
        {
            if (Log.IsDebugEnabled)
            {
                LogEntry[] logEntries = queue.ToArray();
                var sb = new StringBuilder(logEntries.Length + 1);
                sb.AppendFormat($"Operation for Game {name}: ").AppendLine();

                foreach (LogEntry entry in logEntries)
                {
                    sb.AppendFormat($"{name}: {entry}").AppendLine();
                }

                Log.Debug(sb.ToString());
            }
        }
    }
}
