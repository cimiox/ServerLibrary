﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonServerLib.Common;

namespace PhotonServer.Operations
{
    class Move : BaseOperation
    {
        public Move(IRpcProtocol protocol, OperationRequest request) : base(protocol, request)
        {
        }

        [DataMember(Code = (byte)ParameterCode.PosX)]
        public float X { get; set; }

        [DataMember(Code = (byte)ParameterCode.PosY)]
        public float Y { get; set; }

        [DataMember(Code = (byte)ParameterCode.PosZ)]
        public float Z { get; set; }
    }
}
