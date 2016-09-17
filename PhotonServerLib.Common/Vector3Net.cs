using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonServerLib.Common
{
    public class Vector3Net
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3Net(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
