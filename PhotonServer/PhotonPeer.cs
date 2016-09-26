using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using Photon.SocketServer.Rpc;

namespace PhotonServer
{
    public class PhotonPeer : ClientPeer
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public PhotonPeer(InitRequest initRequest)
            : base(initRequest)
        {
        }

        public bool ValidateOperation(Operation operation, SendParameters sendParameters)
        {
            if (operation.IsValid)
            {
                return true;
            }

            string errorMessage = operation.GetErrorMessage();
            SendOperationResponse(new OperationResponse { OperationCode = operation.OperationRequest.OperationCode, ReturnCode = -1, DebugMessage = errorMessage }, sendParameters);
            return false;
        }

        protected virtual void HandleGameOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //TODO: HandleGameOperation
        }

        protected virtual void HandleJoinOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //TODO: HandleJoinOperation
        }

        protected virtual void HandleLeaveOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //TODO: HandleLeaveOperation
        }

        protected virtual void HandlePingOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            SendOperationResponse(new OperationResponse { OperationCode = operationRequest.OperationCode }, sendParameters);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat($"OnDissconnect: connectionId={ConnectionId}, reason={reasonCode}, reasonDetail={reasonDetail}");
            }

            //TODO: OnDisconnect

        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat($"OperationRequest: Code={operationRequest.OperationCode}");
            }

            switch (1)
            {
                default:
                    //TODO: OnOperationRequest
                    break;
            }
        }

        protected virtual void RemovePeerCurrentRoom()
        {
            //TODO: RemovePeerCurrentRoom
        }
    }
}
