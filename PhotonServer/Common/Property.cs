using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer.Common
{
    [Serializable]
    public class Property<TKey>
    {
        private object value;
        public object Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged();
                }
                
            }
        }

        public TKey Key { get; private set; }
        public event EventHandler PropertyChanged;


        public Property(TKey key, object value)
        {
            Key = key;
            Value = value;
        }
        private void RaisePropertyChanged()
        {
            PropertyChanged(this, EventArgs.Empty);
        }
    }
}
