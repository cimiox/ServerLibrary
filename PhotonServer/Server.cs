using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using PhotonServerLib.Common.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer
{
    public class Server : ApplicationBase
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();


        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Client(initRequest);
        }

        protected override void Setup()
        {
            var file = new FileInfo(Path.Combine(BinaryPath,"log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            log.Debug("SERVER READY");
        }

        protected override void TearDown()
        {
            log.Debug("SERVER STOP");
        }
    }
}
