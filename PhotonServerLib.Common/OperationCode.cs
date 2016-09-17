using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonServerLib.Common
{
    public enum OperationCode : byte
    {
        Login,
        GetRecentChatMessages,
        SendChatMessage,
        Move,
        WorldEnter,
        WorldExit,
        ListPlayers
    }
}
