using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using Photon.SocketServer.Diagnostics;
using PhotonServer.Diagnostic;
using System.IO;

namespace PhotonServer
{
    public class Server : ApplicationBase
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return null;
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "Log");

            string path = Path.Combine(this.BinaryPath, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            Log.InfoFormat($"Created Application instance: type: {Instance.GetType()}");
            Log.Debug("SERVER RUNNIG");
            Intialize();
        }

        private void Intialize()
        {
            CounterPublisher.DefaultInstance.AddStaticCounterClass(typeof(Counter), "PhotonServer");

            Protocol.AllowRawCustomValues = true;
        }

        protected override void TearDown()
        {
        }
    }
}
