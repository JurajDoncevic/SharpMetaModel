using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    public class Database
    {
        private readonly string _name;
        private readonly List<Schema> _schemas;

        /// <summary>
        /// Database name
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// List of schemas in database
        /// </summary>
        public List<Schema> Schemas { get => _schemas; }

        public Schema GetSchemaByName(string name)
        {
            return _schemas.Find(s => s.Name.Equals(name));
        }

        /// <summary>
        /// Uniquely identifies a database with a '.' delimited string
        /// </summary>
        public string Identifier
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the table with the given identifier
        /// </summary>
        /// <param name="identifier">Table identifier</param>
        /// <returns>Table object or null</returns>
        public Table GetTableByIdentifier(string identifier)
        {
            string[] split = identifier.Split('.');
            if (split.Length == 3)
            {
                return _schemas.SelectMany(s => s.Tables).SingleOrDefault(t => t.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the column with the given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Column GetColumnByIdentifier(string identifier)
        {
            string[] split = identifier.Split('.');

            if (split.Length == 4)
            {
                return _schemas.SelectMany(s => s.Tables).SelectMany(t => t.Columns).FirstOrDefault(c => c.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the column with the given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public View GetViewByIdentifier(string identifier)
        {
            string[] split = identifier.Split('.');

            if (split.Length == 4)
            {
                return _schemas.SelectMany(s => s.Views).FirstOrDefault(c => c.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Does this table exist in the database?
        /// </summary>
        /// <param name="table">Table object</param>
        /// <returns></returns>
        public bool TableExists(Table table)
        {
            return _schemas.SelectMany(s => s.Tables).Contains(table);
        }

        /// <summary>
        /// Does this table exist int the database?
        /// </summary>
        /// <param name="column">Column object</param>
        /// <returns></returns>
        public bool ColumnExists(Column column)
        {
            return _schemas.SelectMany(s => s.Tables).SelectMany(t => t.Columns).Contains(column);
        }

        /// <summary>
        /// Does this view exist
        /// </summary>
        /// <param name="view">View object</param>
        /// <returns></returns>
        public bool ViewExists(View view)
        {
            return _schemas.SelectMany(s => s.Views).Contains(view);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Database name</param>
        /// <param name="schemas">List of schemas</param>
        public Database(string name, List<Schema> schemas)
        {
            _name = name;
            _schemas = schemas;
            relinkSchemas();
        }

        /// <summary>
        /// Sets all database references for this database's schemas to this database
        /// </summary>
        private void relinkSchemas()
        {
            if(_schemas != null)
                _schemas.ForEach(s => s.Database = this);
        }
    }
}
