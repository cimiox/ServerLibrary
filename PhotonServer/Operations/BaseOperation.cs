using Photon.SocketServer.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonServerLib.Common;

namespace PhotonServer.Operations
{
    public class BaseOperation : Operation
    {
        public BaseOperation(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        public virtual OperationResponse GetResponse(ErrorCode errorCode, string debugMessage = "")
        {
            var response = new OperationResponse(OperationRequest.OperationCode);
            response.ReturnCode = (short)errorCode;
            response.DebugMessage = debugMessage;
            return response;
        }
    }
}
