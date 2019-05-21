using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Describes a view
    /// </summary>
    public class View : Tabular
    {
        private readonly string _name;
        private Schema _schema;
        private readonly List<Column> _columns;

        /// <summary>
        /// Uniquely identifies a table with a '.' delimited string
        /// </summary>
        public string Identifier
        {
            get
            {
                if (_schema != null && _schema.Database != null)
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
        /// This view's parent schema
        /// </summary>
        public override Schema Schema { get => _schema; set => _schema = value; }

        /// <summary>
        /// View name
        /// </summary>
        public override string Name => _name;

        /// <summary>
        /// This view's columns
        /// </summary>
        public List<Column> Columns => _columns;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">View name</param>
        /// <param name="columns">List of columns</param>
        /// <param name="schema">Parent schema</param>
        public View(string name, List<Column> columns, Schema schema)
        {
            _name = name;
            _schema = schema;
            _columns = columns;
        }



        /// <summary>
        /// Set all table references of columns contained in this table to this table 
        /// </summary>
        private void relinkColumns()
        {
            if (_columns != null)
                _columns.ForEach(c => c.Table = this);
        }
    }
}
