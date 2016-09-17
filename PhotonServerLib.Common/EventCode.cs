using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonServerLib.Common
{
    public enum EventCode : byte
    {
        ChatMessage,
        Move,
        WorldEnter,
        WorldExit
    }
}
