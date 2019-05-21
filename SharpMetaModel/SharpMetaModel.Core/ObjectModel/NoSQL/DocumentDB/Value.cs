using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Value of key-value pair
    /// </summary>
    public abstract class Value
    {
        /// <summary>
        /// Gets string representation of value
        /// </summary>
        /// <returns></returns>
        public abstract string StringValue();
    }
}
