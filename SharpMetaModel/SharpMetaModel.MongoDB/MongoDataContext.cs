using MongoDB.Driver;
using MongoDB.Bson;
using SharpMetaModel.Core;
using SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Key = SharpMetaModel.Core.ObjectModel.NoSQL.DocumentDB.Key;

namespace SharpMetaModel.MongoDB
{
    /// <summary>
    /// DataContext class used to access data context for a MongoDB connection
    /// </summary>
    public class MongoDBDataContext : IDataContext
    {
        private string _databaseName;
        private Database _database;
        private MongoClient _mongoClient;

        /// <summary>
        /// Database name
        /// </summary>
        public string DatabaseName { get => _databaseName; set => _databaseName = value; }

        /// <summary>
        /// Database object
        /// </summary>
        public Database Database { get => _database; set => _database = value; }

        /// <summary>
        /// Mongo client
        /// </summary>
        public MongoClient MongoClient { get => _mongoClient; set => _mongoClient = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseName">Name of database</param>
        /// <param name="mongoClient">Mongo Client</param>
        public MongoDBDataContext(string databaseName, MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _databaseName = databaseName;
            _database = null;


            RefreshMetaModel();
        }

        /// <summary>
        /// Gathers metamodel information
        /// </summary>
        public void RefreshMetaModel()
        {
            //get collection names in db
            List<string> collectionNames = _mongoClient.GetDatabase(_databaseName).ListCollectionNames().ToList();
            List<Collection> collections = new List<Collection>();

            foreach (string collectionName in collectionNames)
            {
                //get collection
                IMongoCollection<BsonDocument> collection = _mongoClient.GetDatabase(_databaseName).GetCollection<BsonDocument>(collectionName);

                //prepare aggregation
                var project = new BsonDocument
                {
                            {"arrayofkeyvalue",
                            new BsonDocument
                            {
                                {
                                "$objectToArray", "$$ROOT"
                                }
                            }
                            }


                };
                var unwind = new BsonDocument
                {
                    {
                    "$unwind", "$arrayofkeyvalue"
                    }
                };
                var group = new BsonDocument
                {


                            {
                                "_id", "null"
                            },
                            {
                                "allkeys",
                                new BsonDocument
                                {
                                    {
                                        "$addToSet", "$arrayofkeyvalue.k"
                                    }
                                }
                            }


                };


                List<string> keyNames = collection.Aggregate()
                                        .Project<BsonDocument>(project)
                                        .Unwind("arrayofkeyvalue")
                                        .Group<BsonDocument>(group)
                                        .First()["allkeys"]
                                        .AsBsonArray
                                        .Values
                                        .Select(_ => _.ToString())
                                        .ToList();

                List<Key> keys = new List<Key>();
                keyNames.ForEach(kn =>
                {
                    keys.Add(new Key(kn, new PrimitiveValue(), null));
                });

                List<Index> indexes = new List<Index>();

                foreach (BsonDocument bsonDocument in collection.Indexes.List().ToList())
                {
                    string collectionIdentifier = bsonDocument.GetValue("ns").AsString;

                    List<string> indexedNames = bsonDocument.GetValue("key").AsBsonDocument.Names.ToList();

                    List<string> onKeyIdentifiers = indexedNames.Select(i => collectionIdentifier + "." + i).ToList();

                    indexes.Add(new Index(onKeyIdentifiers, null));

                }

                collections.Add(new Collection(collectionName, keys, indexes, null));



            }

            _database = new Database(_databaseName, collections);


        }
    }
}
