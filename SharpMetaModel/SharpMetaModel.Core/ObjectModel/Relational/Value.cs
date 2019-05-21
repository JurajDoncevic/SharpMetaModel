using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Abstract class of value
    /// </summary>
    public abstract class Value
    {
        /// <summary>
        /// Type of data
        /// </summary>
        public abstract string DataType { get; }

        /// <summary>
        /// Size of data
        /// </summary>
        public abstract int DataSize { get; }
    }
}