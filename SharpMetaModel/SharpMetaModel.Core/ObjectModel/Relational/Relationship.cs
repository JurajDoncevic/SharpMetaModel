using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMetaModel.Core.ObjectModel.Relational
{
    /// <summary>
    /// Class that describes a relationship between tables
    /// </summary>
    public class Relationship
    {
        private readonly Column _primaryKeyColumn;
        private readonly Column _foreignKeyColumn;
        private Schema _schema;

        /// <summary>
        /// Primary key column of relationship
        /// </summary>
        public Column PrimaryKeyColumn { get => _primaryKeyColumn; }

        /// <summary>
        /// Foreign key column of relationship
        /// </summary>
        public Column ForeignKeyColumn { get => _foreignKeyColumn; }

        /// <summary>
        /// Parent schema
        /// </summary>
        public Schema Schema { get => _schema; set => _schema = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="primaryKeyColumn">Primary key column reference</param>
        /// <param name="foreignKeyColumn">Foreign key column reference</param>
        /// <param name="schema">Parent schema</param>
        public Relationship(Column primaryKeyColumn, Column foreignKeyColumn, Schema schema)
        {
            _schema = schema;
            _primaryKeyColumn = primaryKeyColumn;
            _foreignKeyColumn = foreignKeyColumn;
        }
    }
}
