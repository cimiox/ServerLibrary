using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonServerLib.Common
{
    public enum ErrorCode : byte
    {
        Ok = 0,
        InvalidParameters,
        NameIsExist,
        RequestNotImplemented
    }
}
