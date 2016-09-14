using Photon.SocketServer;
using System;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using System.Collections.Generic;

namespace PhotonServer
{
    class Client : ClientPeer
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Client(InitRequest initRequest) : base(initRequest)
        {
            log.Info("Player connection ip:" + initRequest.RemoteIP);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            log.Debug("Disconnect");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case 1:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        log.Debug("recive" + operationRequest.Parameters[1]);
                        OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                        response.Parameters = new Dictionary<byte, object> { { 1, "response message" } };
                        SendOperationResponse(response, sendParameters);
                    }
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
