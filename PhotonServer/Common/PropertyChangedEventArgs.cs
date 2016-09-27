using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer.Common
{
    public class PropertyChangedEventArgs<TKey> : EventArgs
    {
        public TKey Key { get; private set; }
        public object Value { get; private set; }

        public PropertyChangedEventArgs(TKey key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
