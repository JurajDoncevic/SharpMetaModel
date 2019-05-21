using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Defines a database schema
    /// </summary>
    public class Schema
    {
        private readonly string _name;
        private readonly List<Table> _tables;
        private readonly List<Relationship> _relationships;
        private Database _database;
        private readonly bool _isDefault;
        private readonly List<View> _views;

        /// <summary>
        /// Schema name
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// List of tables in schema
        /// </summary>
        public List<Table> Tables { get => _tables; }

        public Table GetTableByName(string name)
        {
            return _tables.Find(t => t.Name.Equals(name));
        }

        public View GetViewByName(string name)
        {
            return _views.Find(v => v.Name.Equals(name));
        }

        /// <summary>
        /// List of relationships in schema
        /// </summary>
        public List<Relationship> Relationships { get => _relationships; }

        /// <summary>
        /// Parent database reference
        /// </summary>
        public Database Database { get => _database; set => _database = value; }

        /// <summary>
        /// Uniquely identifies a schema with a '.' delimited string
        /// </summary>
        public string Identifier
        {
            get
            {
                if (_database != null)
                {
                    return String.Format("{0}.{1}", _database.Name, _name);
                }
                else
                {
                    return null;
                }

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
            if(split.Length == 3)
            {
                return _tables.Find(t => t.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the view with the given identifier
        /// </summary>
        /// <param name="identifier">Table identifier</param>
        /// <returns>Table object or null</returns>
        public View GetViewByIdentifier(string identifier)
        {
            string[] split = identifier.Split('.');
            if (split.Length == 3)
            {
                return _views.Find(t => t.Identifier.Equals(identifier));
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

            if(split.Length == 4)
            {
                return _tables.SelectMany(t => t.Columns).FirstOrDefault(c => c.Identifier.Equals(identifier));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Is this the default schema for the db
        /// </summary>
        public bool IsDefault { get => _isDefault; }

        /// <summary>
        /// List of views in this schema
        /// </summary>
        public List<View> Views => _views;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Schema name</param>
        /// <param name="tables">List of tables in schema</param>
        /// <param name="relationships">List of relationships in schema</param>
        /// <param name="isDefault">Is this the default schema for the parent database</param>
        /// <param name="database">This schema's parent database</param>
        public Schema(string name, List<Table> tables, List<Relationship> relationships, bool isDefault, Database database)
        {
            _name = name;
            _tables = tables;
            _relationships = relationships;
            _database = database;
            _isDefault = isDefault;

            relinkTables();
            relinkRelationships();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Schema name</param>
        /// <param name="tables">List of tables in the schema</param>
        /// <param name="relationships">List of relationships in the schema</param>
        /// <param name="views">List of views in the schema</param>
        /// <param name="isDefault">Is this the default schema for the parent database</param>
        /// <param name="database">This schema's parent database</param>
        public Schema(string name, List<Table> tables, List<Relationship> relationships, List<View> views, bool isDefault, Database database)
        {
            _name = name;
            _tables = tables;
            _relationships = relationships;
            _views = views;
            _database = database;
            _isDefault = isDefault;

            relinkTables();
            relinkRelationships();
            relinkViews();
        }





        /// <summary>
        /// Sets all schema references in this schema's tables to this schema
        /// </summary>
        private void relinkTables()
        {
            if(_tables != null)
                _tables.ForEach(t => t.Schema = this);
        }

        /// <summary>
        /// Sets all schema references in this schema's relationships to this schema
        /// </summary>
        private void relinkRelationships()
        {
            if(_relationships != null)
                _relationships.ForEach(r => r.Schema = this);
        }

        /// <summary>
        /// Sets all schema references in this schema's views to this schema
        /// </summary>
        private void relinkViews()
        {
            if (_views != null)
                _views.ForEach(v => v.Schema = this);
        }

    }
}
