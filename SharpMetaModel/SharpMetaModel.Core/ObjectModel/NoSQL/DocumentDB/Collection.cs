using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    /// <summary>
    /// Represents a NoSQL Document database collection
    /// </summary>
    public class Collection
    {
        private readonly long _documentCount;
        private Database _database;
        private readonly string _name;
        private readonly List<Index> _indexes;
        private readonly List<Key> _keys;

        public List<Key> Keys { get => _keys; }

        /// <summary>
        /// List of documents
        /// </summary>
        public long DocumentCount { get => _documentCount; }

        /// <summary>
        /// This collection's database
        /// </summary>
        public Database Database { get => _database; set => _database = value; }

        /// <summary>
        /// Name of collection
        /// </summary>
        public string Name { get => _name; }

        /// <summary>
        /// Indexes in this collection
        /// </summary>
        public List<Index> Indexes { get => _indexes; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of collection</param>
        /// <param name="documents">List of documents</param>
        /// <param name="indexes">List of indexes</param>
        /// <param name="database">Parent database reference</param>
        public Collection(string name, List<Key> keys, List<Index> indexes, Database database)
        {
            _keys = keys;
            _database = database;
            _name = name;
            _indexes = indexes;

            relinkKeys();
            relinkIndexes();
        }


        /// <summary>
        /// Sets the parent collection of all documents to this collection
        /// </summary>
        private void relinkKeys()
        {
            _keys.ForEach(k => k.Collection = this);
        }

        /// <summary>
        /// Sets the parent collection of all indexes to this collection
        /// </summary>
        private void relinkIndexes()
        {
            _indexes.ForEach(i => i.Collection = this);
        }
    }
}
