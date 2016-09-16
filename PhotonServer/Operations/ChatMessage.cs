using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonServerLib.Common;

namespace PhotonServer.Operations
{
    class ChatMessage : BaseOperation
    {
        public ChatMessage(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.ChatMessage)]
        public string Message { get; set; }
    }
}
