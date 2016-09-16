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
    public class Login : BaseOperation
    {
        public Login(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.CharacterName)]
        public string CharacterName { get; set; }
    }
}
