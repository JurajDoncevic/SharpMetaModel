using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core
{
    /// <summary>
    /// Interface for all DataContext classes. Classes of this interface present all possible operations on a data context/connection.
    /// From here a user can access metamodel functionalities and send queries.
    /// </summary>
    public interface IDataContext
    {
        void RefreshMetaModel();

    }
}
