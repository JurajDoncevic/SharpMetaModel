using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    public class SubObject : Value
    {
        private List<Key> _keys;
        private Key _parentKey;
        public List<Key> Keys { get => _keys; }
        public Key ParentKey { get=>_parentKey; set => _parentKey = value; }

        public SubObject()
        {

        }

        public SubObject(List<Key> keys, Key parentKey)
        {
            _keys = keys;
            _parentKey = parentKey;
        }

        public override string StringValue()
        {
            return 
               "<object with keys: " +
               _keys.Select(_ => _.Name).Aggregate((k1, k2) => k1 + ", " + k2) +
               ">";
        }

        public Key FindKey(string keyName)
        {
            Key retVal = null;
            foreach(Key k in _keys)
            {
                if (k.Name.Equals(keyName))
                {
                    return k;
                }
                if (k.HasObject())
                {
                    retVal = ((SubObject)k.Value).FindKey(keyName);
                }
            };

            return retVal;
        }
    }
}
