using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB
{
    public class Index
    {
        private readonly List<string> _onKeyIdentifiers;
        private Collection _collection;

        /// <summary>
        /// Key identifiers of indexed keys
        /// </summary>
        public List<string> OnKeyIdentifiers { get => _onKeyIdentifiers; }

        /// <summary>
        /// This indexes' database
        /// </summary>
        public Database Database { get => _collection.Database; }

        /// <summary>
        /// This indexes' collection
        /// </summary>
        public Collection Collection { get => _collection; set => _collection = value; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onKeyIdentifiers">List of key identifiers of indexed keys</param>
        /// <param name="collection">Indexes' parent collection</param>
        public Index(List<string> onKeyIdentifiers, Collection collection)
        {
            _onKeyIdentifiers = onKeyIdentifiers;
            _collection = collection;
        }
    }
}
