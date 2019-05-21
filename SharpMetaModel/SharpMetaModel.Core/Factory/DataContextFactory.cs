using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMetaModel.Core.Factory
{
    public abstract class DataContextFactory<T> : IDataContextFactory<T>
    {
        public DataContextFactory()
        {

        }
        public abstract T CreateDataContext(string serverURL, int port, string databaseName, string userName, string password);
        public abstract T CreateDataContext(string connectionString, string databaseName);
    }
}
