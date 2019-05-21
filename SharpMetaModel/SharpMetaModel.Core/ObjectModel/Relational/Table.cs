using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// A relational database table
    /// </summary>
    public class Table : Tabular
    {
        private readonly string _name;
        private readonly List<Column> _columns;
        private Schema _schema;
        private readonly List<Index> _indexes;
        private readonly long _rowCount;

        /// <summary>
        /// Table name
        /// </summary>
        public override string Name { get => _name; }

        /// <summary>
        /// List of columns belonging to this table
        /// </summary>
        public List<Column> Columns { get => _columns; }

        public Column GetColumnByName(string name)
        {
            return _columns.Find(c => c.Name.Equals(name));
        }

        /// <summary>
        /// List of primary key columns in this table
        /// </summary>
        public List<Column> PrimaryKeys { get { return _columns.Where(c => c.IsPrimaryKey).ToList(); } }

        /// <summary>
        /// List of foreign key columns in this table
        /// </summary>
        public List<Column> ForeignKeys { get { return _columns.Where(c => c.IsForeignKey).ToList(); } }

        /// <summary>
        /// This table's parent schema
        /// </summary>
        public override Schema Schema { get => _schema; set => _schema = value; }

        /// <summary>
        /// Uniquely identifies a table with a '.' delimited string
        /// </summary>
        public string Identifier
        {
            get
            {
                if(_schema != null && _schema.Database != null)
                {
                    return String.Format("{0}.{1}.{2}", _schema.Database.Name, _schema.Name, _name);
                }
                else
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// Gets the column with the given identifier
        /// </summary>
        /// <param name="identifier">Column identifier</param>
        /// <returns>Null or Column</returns>
        public Column GetColumnByIdentifier(string identifier)
        {
            string[] split = identifier.Split('.');
            if(split.Length == 4)
            {
                return _columns.Find(c => c.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
         }

        /// <summary>
        /// List of indexes in this table
        /// </summary>
        public List<Index> Indexes { get => _indexes; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Table name</param>
        /// <param name="columns">List of columns</param>
        /// <param name="indexes">List of indexes</param>
        /// <param name="schema">Table's parent schema</param>
        public Table(string name, List<Column> columns, List<Index> indexes, Schema schema)
        {
            _name = name;
            _columns = columns;
            _schema = schema;
            _indexes = indexes;

            relinkColumns();
            relinkIndexes();
        }



        /// <summary>
        /// Set all table references of columns contained in this table to this table 
        /// </summary>
        private void relinkColumns()
        {
            if(_columns != null)
                _columns.ForEach(c => c.Table = this);
        }

        /// <summary>
        /// Set all table references of indexes contained in this table to this table 
        /// </summary>
        private void relinkIndexes()
        {
            if(_indexes != null)
                _indexes.ForEach(i => i.Table = this);
        }

    }
}
