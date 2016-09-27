using ExitGames.Logging;
using System;
using System.Threading;

namespace PhotonServer.Diagnostic
{
    public class CounterLogger : IDisposable
    {
        private static readonly ILogger counterLog = LogManager.GetLogger("PerfomanceCounter");
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        private const int LogIntervalMs = 5000;

        private static readonly CounterLogger instance = new CounterLogger();
        public static CounterLogger Instance
        {
            get { return instance; }
        }

        private Timer timer;

        private CounterLogger()
        {
        }

        public void Start()
        {
            if (counterLog.IsDebugEnabled)
            {
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Starting counter logger");
                }
            }
            else
            {
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Counter logger not started");
                }
            }
        }

        public void Dispose()
        {
            var t = timer;
            if (t != null)
            {
                t.Dispose();
                timer = null;
            }
        }
    }
}
