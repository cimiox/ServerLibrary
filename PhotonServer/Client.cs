using Photon.SocketServer;
using System;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using System.Collections.Generic;
using PhotonServerLib.Common;
using PhotonServer.Operations;

namespace PhotonServer
{
    public class Client : ClientPeer
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Client(InitRequest initRequest) : base(initRequest)
        {
            log.Debug("Client ip:" + initRequest.RemoteIP);
        }

        public string CharacterName { get; set; }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            World.Instance.RemoveClient(this);
            log.Debug("Disconnect");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.Login:
                    var loginRequest = new Login(Protocol, operationRequest);

                    if (!loginRequest.IsValid)
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    CharacterName = loginRequest.CharacterName;

                    if (World.Instance.IsContain(CharacterName))
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameIsExist), sendParameters);
                        return;
                    }

                    World.Instance.AddClient(this);

                    var response = new OperationResponse(operationRequest.OperationCode);
                    SendOperationResponse(response, sendParameters);

                    log.Info("User name: " + CharacterName);
                    break;
                case 2:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        log.Debug("recive" + operationRequest.Parameters[1]);
                        EventData eventData = new EventData(1);
                        eventData.Parameters = new Dictionary<byte, object> { { 1, "response for event" } };
                        SendEvent(eventData, sendParameters);
                    }
                    break;
                default:
                    log.Debug("Unknown OperationRequest received:" + operationRequest.OperationCode);
                    break;
            }
        }
    }
}
