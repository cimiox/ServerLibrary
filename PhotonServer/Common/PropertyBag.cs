using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonServer.Common
{
    [Serializable]
    public class PropertyBag<TKey>
    {
        private readonly Dictionary<TKey, Property<TKey>> dictionary;
        public int Count
        {
            get { return dictionary.Count; }
        }

        public event EventHandler<PropertyChangedEventArgs<TKey>> PropertyChanged;

        public PropertyBag()
        {
            dictionary = new Dictionary<TKey, Property<TKey>>();
        }

        public PropertyBag(IEnumerable<KeyValuePair<TKey, object>> values)
            : this()
        {
            foreach (KeyValuePair<TKey, object> item in values)
            {
                Set(item.Key, item.Value);
            }
        }

        public PropertyBag(IDictionary values)
            :this()
        {
            foreach (TKey key in values.Keys)
            {
                Set(key, values[key]);
            }
        }

        public IDictionary<TKey, Property<TKey>> AsDictionary()
        {
            return dictionary;
        }

        public IList<Property<TKey>> GetAll()
        {
            var properties = new Property<TKey>[dictionary.Count];
            dictionary.Values.CopyTo(properties, 0);
            return properties;
        }

        public Hashtable GetProperties()
        {
            var result = new Hashtable(dictionary.Count);
            CopyPropertiesToHashtable(result);
            return result;
        }

        public Hashtable GetProperties(IList<TKey> propertyKeys)
        {
            if (propertyKeys == null)
            {
                return GetProperties();
            }

            var result = new Hashtable(propertyKeys.Count);
            CopyPropertiesToHashtable(result, propertyKeys);
            return result;
        }

        public Hashtable GetProperties(IEnumerable<TKey> propertyKeys)
        {
            if (propertyKeys == null)
            {
                return GetProperties();
            }

            var result = new Hashtable();
            CopyPropertiesToHashtable(result, propertyKeys);
            return result;
        }

        public Hashtable GetProperties(IEnumerable propertyKeys)
        {
            if (propertyKeys == null)
            {
                return GetProperties();
            }

            var result = new Hashtable();
            CopyPropertiesToHashtable(result, propertyKeys);
            return result;
        }

        public Property<TKey> GetProperty(TKey key)
        {
            Property<TKey> value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public void Set(TKey key, object value)
        {
            Property<TKey> property;

            if (dictionary.TryGetValue(key, out property))
            {
                property.Value = value;
            }
            else
            {
                property = new Property<TKey>(key, value);
                property.PropertyChanged += OnPropertyChanged;
                dictionary.Add(key, property);
                RaisePropertyChanged(key, value);
            }
        }

        public void SetProperties(IDictionary values)
        {
            foreach (TKey key in values.Keys)
            {
                Set(key, values[key]);
            }
        }

        public void SetProperties(IDictionary<TKey, object> values)
        {
            foreach (KeyValuePair<TKey, object> keyValue in values)
            {
                Set(keyValue.Key, keyValue.Value);
            }
        }

        public bool TryGetValue(TKey key, out object value)
        {
            Property<TKey> property;
            if (dictionary.TryGetValue(key, out property))
            {
                value = property.Value;
                return true;
            }

            value = null;
            return false;
        }

        private void RaisePropertyChanged(TKey key, object value)
        {
            PropertyChanged(this, new PropertyChangedEventArgs<TKey>(key, value));
        }

        private void OnPropertyChanged(object sender, EventArgs e)
        {
            var property = (Property<TKey>)sender;
            RaisePropertyChanged(property.Key, property.Value);
        }

        private void CopyPropertiesToHashtable(IDictionary hashtable, IEnumerable propertyKeys)
        {
            foreach (TKey key in propertyKeys)
            {
                Property<TKey> property;
                if (dictionary.TryGetValue(key, out property))
                {
                    hashtable.Add(key, property.Value);
                }
            }
        }

        private void CopyPropertiesToHashtable(IDictionary hashtable, IEnumerable<TKey> propertyKeys)
        {
            foreach (TKey key in propertyKeys)
            {
                Property<TKey> property;
                if (dictionary.TryGetValue(key, out property))
                {
                    hashtable.Add(key, property.Value);
                }
            }
        }

        private void CopyPropertiesToHashtable(IDictionary hashtable)
        {
            foreach (KeyValuePair<TKey, Property<TKey>> keyValue in dictionary)
            {
                hashtable.Add(keyValue.Key, keyValue.Value.Value);
            }
        }
    }
}
