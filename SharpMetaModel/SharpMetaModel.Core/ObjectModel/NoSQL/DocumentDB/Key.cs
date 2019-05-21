using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Represents a key-value pair
    /// </summary>
    public class Key
    {
        private readonly string _name;
        private readonly Value _value;
        private Collection _collection;


        /// <summary>
        /// Key name
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// Value attributed to the key
        /// </summary>
        public Value Value { get => _value; }

        public Collection Collection { get=>_collection; set=> _collection = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public Key(string name, Value value, Collection collection)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// Is the value a primitive value?
        /// </summary>
        /// <returns></returns>
        public bool IsPrimitiveValue()
        {
            return this._value.GetType().Equals(typeof(PrimitiveValue));
        }

        /// <summary>
        /// Is the value an aggregate value?
        /// </summary>
        /// <returns></returns>
        public bool IsAggregateValue()
        {
            return this._value.GetType().Equals(typeof(AggregateValue));
        }

        /// <summary>
        /// Is the value a JSON object?
        /// </summary>
        /// <returns></returns>
        public bool HasObject()
        {
            return this._value.GetType().Equals(typeof(SubObject));
        }

        /// <summary>
        /// If this object has a JSON object value, it searches for a key-value in it
        /// </summary>
        /// <param name="keyName">Key of key-value pair</param>
        /// <returns></returns>
        public Key FindKey(string keyName)
        {
            if (HasObject())
            {
                return ((SubObject)_value).FindKey(keyName);
            }
            else
            {
                return null;
            }
        }
    }
}
