using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    public class Index
    {
        private readonly List<Column> _indexedColumns;
        private Table _table;
        private readonly string _name;

        public string Identifier
        {
            get
            {
                if (_table != null
                    && _table.Schema != null
                    && _table.Schema.Database != null)
                {
                    return String.Format("{0}.{1}.{2}.{3}", _table.Schema.Database.Name, _table.Schema.Name, _table.Name, _name);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// List of columns in this index
        /// </summary>
        public List<Column> IndexedColumns { get => _indexedColumns; }

        /// <summary>
        /// This indexes' table
        /// </summary>
        public Table Table { get => _table; set => _table = value; }

        public string Name { get => _name; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Index name</param> 
        /// <param name="indexedColumns">Columns indexed by this index</param>
        /// <param name="table">This indexes' parent table</param>
        public Index(string name, List<Column> indexedColumns, Table table)
        {
            _indexedColumns = indexedColumns;
            _table = table;
            _name = name;
        }
    }
}
