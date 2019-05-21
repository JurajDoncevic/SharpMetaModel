using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Describes an array of values as data in a column
    /// </summary>
    public class ArrayValue : Value
    {
        private readonly string _dataType;
        private readonly int _dataSize;
        private readonly int _arraySize;

        /// <summary>
        /// Data type
        /// </summary>
        public override string DataType { get => _dataType; }

        /// <summary>
        /// Size of data
        /// </summary>
        public override int DataSize { get => _dataSize; }

        /// <summary>
        /// Size of array
        /// </summary>
        public int ArraySize { get => _arraySize; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataType">Data type</param>
        /// <param name="dataSize">Size of data</param>
        /// <param name="arraySize">Size of array</param>
        public ArrayValue(string dataType, int dataSize, int arraySize)
        {
            _dataType = dataType;
            _dataSize = dataSize;
            _arraySize = arraySize;
        }
    }
}
