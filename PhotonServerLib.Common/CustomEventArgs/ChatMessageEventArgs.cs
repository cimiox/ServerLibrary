﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonServerLib.Common.CustomEventArgs
{
    public class ChatMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public ChatMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
