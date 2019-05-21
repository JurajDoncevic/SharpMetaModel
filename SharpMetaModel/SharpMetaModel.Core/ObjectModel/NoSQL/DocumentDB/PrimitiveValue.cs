using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Represents a primitive value
    /// </summary>
    public class PrimitiveValue : Value
    {
        private readonly object _value;

        /// <summary>
        /// Value representation
        /// </summary>
        public object Value { get => _value; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimitiveValue()
        {
            _value = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">A string value</param>
        public PrimitiveValue(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns a string representation of this value
        /// </summary>
        /// <returns></returns>
        public override string StringValue()
        {
            return _value.ToString();
        }
    }
}
