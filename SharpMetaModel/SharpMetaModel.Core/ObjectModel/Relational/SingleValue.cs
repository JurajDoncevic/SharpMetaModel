using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Describes a single value of a column
    /// </summary>
    public class SingleValue : Value
    {
        private readonly string _dataType;
        private readonly int _dataSize;

        /// <summary>
        /// Data type
        /// </summary>
        public override string DataType { get => _dataType; }

        /// <summary>
        /// Size of data
        /// </summary>
        public override int DataSize { get => _dataSize;  }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataType">Type of data</param>
        /// <param name="dataSize">Size of data</param>
        public SingleValue(string dataType, int dataSize)
        {
            _dataType = dataType;
            _dataSize = dataSize;
        }
    }
}
