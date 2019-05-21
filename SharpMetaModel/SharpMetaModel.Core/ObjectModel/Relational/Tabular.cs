using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    public abstract class Tabular
    {
        public abstract string Name { get; }

        public abstract Schema Schema { get; set; }

    }
}
