using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Defines a column from a relational database
    /// </summary>
    public class Column
    {
        private readonly string _name;
        private readonly Value _value;
        private Tabular _table;
        private readonly bool _isNullable;
        private readonly bool _isPrimaryKey;

        /// <summary>
        /// Column name
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// Is this column used as a primary key
        /// </summary>
        public bool IsPrimaryKey { get => _isPrimaryKey; }

        /// <summary>
        /// Is this columns used as a foreign key
        /// </summary>
        public bool IsForeignKey
        {
            get
            {
                if (_table != null
                    && _table.Schema != null
                    && _table.Schema.Relationships != null
                    && _table.Schema.Relationships.Count != 0
                    && _table.Schema.Relationships.Where(r => r.ForeignKeyColumn.Identifier.Equals(this.Identifier)).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// This columns parent table
        /// </summary>
        public Tabular Table { get => _table; set => _table = value;  }

        /// <summary>
        /// Uniquely identifies a column with a '.' delimited string
        /// </summary>
        public string Identifier
        {
            get
            {
                if(_table != null 
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
        /// Finds all relationships where this column acts as a foreign key
        /// </summary>
        public List<Relationship> ForeignKeyRelationship
        {
            get
            {
                if (_table != null 
                    && _table.Schema != null 
                    && _table.Schema.Relationships != null 
                    && _table.Schema.Relationships.Count != 0)
                {
                    return _table.Schema.Relationships.Where(r => r.ForeignKeyColumn.Identifier.Equals(this.Identifier)).ToList();
                }
                else
                {
                    return new List<Relationship>();
                }
                
            }
        }

        /// <summary>
        /// Finds all relationships where this column acts as a primary key
        /// </summary>
        public List<Relationship> PrimaryKeyRelationship
        {
            get
            {
                if (_table != null
                    && _table.Schema != null
                    && _table.Schema.Relationships != null
                    && _table.Schema.Relationships.Count != 0)
                {
                    return _table.Schema.Relationships.Where(r => r.PrimaryKeyColumn.Identifier.Equals(this.Identifier)).ToList();
                }
                else
                {
                    return new List<Relationship>();
                }

            }
        }

        /// <summary>
        /// Gets the value definition in this column
        /// </summary>
        public Value Value { get => _value; }

        /// <summary>
        /// Gets all indexes made on this column
        /// </summary>
        public List<Index> Indexes
        {
            get
            {
                if (_table.GetType() == typeof(Table))
                    return ((Table)_table).Indexes.Where(i => i.IndexedColumns.Contains(this)).ToList();
                else
                    return new List<Index>();
            }
        }

        /// <summary>
        /// Is this column nullable
        /// </summary>
        public bool IsNullable { get => _isNullable; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="table"></param>
        /// <param name="isNullable"></param>
        /// <param name="isPrimaryKey"></param>
        public Column(string name, Value value, Table table, bool isNullable, bool isPrimaryKey)
        {
            _name = name;
            _value = value;
            _table = table;
            _isNullable = isNullable;
            _isPrimaryKey = isPrimaryKey;
        }
    }
}
