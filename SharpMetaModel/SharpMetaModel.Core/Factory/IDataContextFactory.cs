using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMetaModel.Core.Factory
{
    interface IDataContextFactory<T>
    {
        T CreateDataContext(string serverURL, int port, string databaseName, string userName, string password);
        T CreateDataContext(string connectionString, string databaseName);
    }
}
