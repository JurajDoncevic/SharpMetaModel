using MySql.Data.MySqlClient;
using SharpMetaModel.Core;
using SharpMetaModel.Core.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMetaModel.MySQL
{
    public class MySQLDataContextFactory : DataContextFactory<MySQLDataContext>
    {
        public MySQLDataContextFactory() : base()
        {

        }

        public override MySQLDataContext CreateDataContext(string serverURL, int port, string databaseName, string userName, string password)
        {
            string connectionString = string.Format("server={0};port={1};uid={2};password={3};database={4};", serverURL, port.ToString(), userName, password, databaseName);

            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            MySQLDataContext mySQLDataContext = new MySQLDataContext(databaseName, mySqlConnection);

            return mySQLDataContext;
        }

        public override MySQLDataContext CreateDataContext(string connectionString, string databaseName)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

            MySQLDataContext mySQLDataContext = new MySQLDataContext(databaseName, mySqlConnection);

            return mySQLDataContext;
        }
    }
}
