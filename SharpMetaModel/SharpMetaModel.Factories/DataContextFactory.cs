using MongoDB.Driver;
using MySql.Data.MySqlClient;
using SharpMetaModel.Core;
using SharpMetaModel.MongoDB;
using SharpMetaModel.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Factories
{
    /// <summary>
    /// A static factory class for all supported DataContext classes
    /// </summary>
    public static class DataContextFactory
    {

        /// <summary>
        /// Creates a MySQLDataContext
        /// </summary>
        /// <param name="serverURL">Server URL</param>
        /// <param name="port">Port</param>
        /// <param name="databaseName">Database name</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static MySQLDataContext CreateMySQLDataContext(string serverURL, int port, string databaseName, string userName, string password)
        {
            string connectionString = string.Format("server={0};port={1};uid={2};password={3};database={4};", serverURL, port.ToString(), userName, password, databaseName);

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            MySQLDataContext mySQLDataContext = new MySQLDataContext(databaseName, mySqlConnection);

            return mySQLDataContext;
        }

        /// <summary>
        /// Creates a MySQLDataContext
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="databaseName">Database name</param>
        /// <returns></returns>
        public static MySQLDataContext CreateMySQLDataContext(string connectionString, string databaseName)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            MySQLDataContext mySQLDataContext = new MySQLDataContext(databaseName, mySqlConnection);

            return mySQLDataContext;
        }

        /// <summary>
        /// Creates a MongoDataContext
        /// </summary>
        /// <param name="serverURL">Server URL</param>
        /// <param name="port">Port</param>
        /// <param name="databaseName">Database name</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static MongoDataContext CreateMongoDataContext(string serverURL, int port, string databaseName, string userName, string password)
        {
            string connectionString = "";

            if (userName == null || userName.Trim().Length == 0)
                connectionString = string.Format("mongodb://{0}:{1}/{2}", serverURL, port.ToString(), databaseName);
            else
                connectionString = string.Format("mongodb://{0}:{1}@{2}:{3}/{4}", userName, password, serverURL, port.ToString(), databaseName);

            MongoClient client = new MongoClient(connectionString);

            MongoDataContext mongoDataContext = new MongoDataContext(databaseName, client);

            return mongoDataContext;
        }

        /// <summary>
        /// Creates a MongoDataContext
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="databaseName">Database name</param>
        /// <returns></returns>
        public static MongoDataContext CreateMongoDataContext(string connectionString, string databaseName)
        {
            MongoClient client = new MongoClient(connectionString);

            MongoDataContext mongoDataContext = new MongoDataContext(databaseName, client);

            return mongoDataContext;
        }

    }
}
