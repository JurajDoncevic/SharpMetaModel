using MongoDB.Driver;
using SharpMetaModel.Core;
using SharpMetaModel.Core.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMetaModel.MongoDB
{
    public class MongoDBDataContextFactory : DataContextFactory<MongoDBDataContext>
    {
        public MongoDBDataContextFactory() : base()
        {

        }

        public override MongoDBDataContext CreateDataContext(string serverURL, int port, string databaseName, string userName, string password)
        {
            string connectionString = "";

            if (userName == null || userName.Trim().Length == 0)
                connectionString = string.Format("mongodb://{0}:{1}/{2}", serverURL, port.ToString(), databaseName);
            else
                connectionString = string.Format("mongodb://{0}:{1}@{2}:{3}/{4}", userName, password, serverURL, port.ToString(), databaseName);

            MongoClient client = new MongoClient(connectionString);

            MongoDBDataContext mongoDataContext = new MongoDBDataContext(databaseName, client);

            return mongoDataContext;
        }

        public override MongoDBDataContext CreateDataContext(string connectionString, string databaseName)
        {
            MongoClient client = new MongoClient(connectionString);

            MongoDBDataContext mongoDataContext = new MongoDBDataContext(databaseName, client);

            return mongoDataContext;
        }
    }
}
