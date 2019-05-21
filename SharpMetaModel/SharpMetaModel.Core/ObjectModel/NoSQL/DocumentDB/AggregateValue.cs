using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Represents an aggregate value (array etc.)
    /// </summary>
    public class AggregateValue : Value
    {
        private readonly List<Value> _values;

        /// <summary>
        /// List of values
        /// </summary>
        public List<Value> Values { get => _values; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Values in aggregate</param>
        public AggregateValue(List<Value> values)
        {
            _values = values;
        }

        /// <summary>
        /// Gets a string representation of the aggregate value
        /// </summary>
        /// <returns></returns>
        public override string StringValue()
        {
            return "[" + _values.Select(v => v.StringValue()).Aggregate<string>((v1, v2) => v1 + "," + v2) + "]";
        }
    }
}
