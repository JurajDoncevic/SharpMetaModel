using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Represents a NoSQL Document database
    /// </summary>
    public class Database
    {
        private readonly string _name;
        private readonly List<Collection> _collections;
        /// <summary>
        /// Database name
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// List of collections inside this database
        /// </summary>
        public List<Collection> Collections { get => _collections; }

        /// <summary>
        /// List of indexes inside this database
        /// </summary>
        public List<Index> Indexes { get => _collections.SelectMany(c => c.Indexes).ToList(); }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Database name</param>
        /// <param name="collections">List of all collections inside this database</param>
        public Database(string name, List<Collection> collections)
        {
            _name = name;
            _collections = collections;
            relinkCollections();
        }


        /// <summary>
        /// Sets all database references for this database's collections to this database
        /// </summary>
        private void relinkCollections()
        {
            _collections.ForEach(c => c.Database = this);
        }
    }
}
