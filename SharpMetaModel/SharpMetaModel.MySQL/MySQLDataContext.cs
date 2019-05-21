using MySql.Data.MySqlClient;
using SharpMetaModel.Core;
using SharpMetaModel.Core.ObjectModel.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpMetaModel.MySQL
{
    /// <summary>
    /// DataContext class used to access data context for a MySQL connection 
    /// </summary>
    public class MySQLDataContext : IDataContext
    {
        private string _databaseName;
        private Database _database;
        private MySqlConnection _mySqlConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseName">Database name</param>
        /// <param name="mySqlConnection">Non open MySQL connection</param>
        public MySQLDataContext(string databaseName, MySqlConnection mySqlConnection)
        {
            _mySqlConnection = mySqlConnection;
            _databaseName = databaseName;
            _database = null;


            RefreshMetaModel();
        }

        /// <summary>
        /// Database metamodel
        /// </summary>
        public Database Database { get => _database; }

        /// <summary>
        /// Gathers metamodel information
        /// </summary>
        public void RefreshMetaModel()
        {
            if(_mySqlConnection.State == System.Data.ConnectionState.Open)
            {
                _mySqlConnection.Close();
            }
            

            try
            {
                _mySqlConnection.Open();

                Dictionary<string, List<Column>> tableColumns = getColumnsForTables(_databaseName);
                Dictionary<string, List<Index>> tableIndexes = getIndexesForTables(_databaseName, tableColumns);
                List<Table> tables = createTables(tableColumns, tableIndexes);

                List<Relationship> relationships = getRelationships(_databaseName, tableColumns);

                Dictionary<string, List<Column>> viewColumns = getColumnsForViews(_databaseName);
                List<View> views = createViews(viewColumns);

                List<Schema> schemas = new List<Schema>();
                schemas.Add(new Schema(_databaseName, tables, relationships, views, true, null));


                _database = new Database(_databaseName, schemas);

                _mySqlConnection.Close();
            }
            catch(Exception e)
            {
                _mySqlConnection.Close();
                throw;
            }

            
            

        }

        #region PRIVATE
        /// <summary>
        /// Procedure creates table objects
        /// </summary>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        private List<Table> createTables(Dictionary<string, List<Column>> tableColumns, Dictionary<string, List<Index>> tableIndexes)
        {
            List<Table> tables = new List<Table>();

            List<string> tableNames = new List<string>();
            tableNames.AddRange(tableColumns.Keys);
            tableNames.AddRange(tableIndexes.Keys);
            tableNames = tableNames.Distinct().ToList();
            
            tableNames.ForEach(tableName =>
            {
                tables.Add(new Table(tableName, tableColumns[tableName], tableIndexes[tableName], null));
                
            });

            return tables;
        }

        /// <summary>
        /// Procedure creates view objects
        /// </summary>
        /// <param name="viewColumns"></param>
        /// <returns></returns>
        private List<View> createViews(Dictionary<string, List<Column>> viewColumns)
        {
            List<View> views = new List<View>();

            List<string> viewNames = new List<string>();
            viewNames.AddRange(viewColumns.Keys);
            viewNames = viewNames.Distinct().ToList();

            viewNames.ForEach(viewName =>
            {
                views.Add(new View(viewName, viewColumns[viewName], null));
                

            });

            return views;
        }

        /// <summary>
        /// Procedure generates dictionary of table's columns
        /// </summary>
        private Dictionary<string, List<Column>> getColumnsForTables(string schemaName)
        {
            

            Dictionary<string, List<Column>> tableColumns = new Dictionary<string, List<Column>>();

            MySqlCommand command = _mySqlConnection.CreateCommand();

            
            command.CommandText = string.Format("SELECT INFORMATION_SCHEMA.TABLES.TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE, COLUMN_TYPE, COLUMN_KEY "
                                                + "FROM INFORMATION_SCHEMA.COLUMNS "
                                                + "JOIN INFORMATION_SCHEMA.TABLES ON "
                                                + "INFORMATION_SCHEMA.TABLES.TABLE_SCHEMA = INFORMATION_SCHEMA.COLUMNS.TABLE_SCHEMA "
                                                + "AND "
                                                + "INFORMATION_SCHEMA.TABLES.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME "
                                                + "WHERE INFORMATION_SCHEMA.COLUMNS.TABLE_SCHEMA = '{0}' AND TABLE_TYPE = 'BASE TABLE'", schemaName);


            MySqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                string tableName = dataReader.GetString(0);
                string columnName = dataReader.GetString(1);
                string isNullable = dataReader.GetString(2);
                string dataType = dataReader.GetString(3);
                string columnType = dataReader.GetString(4);
                string columnKey = dataReader.GetString(5);

                bool bIsNullable = isNullable.Equals("YES") ? true : false;
                int dataSize = 0;

                if (Regex.IsMatch(columnType, "\\w+\\(([0-9]+)\\)"))
                {
                    string ds = Regex.Matches(columnType, "\\w+\\(([0-9]+)\\)")[0].Groups[1].ToString();
                    dataSize = Int32.Parse(ds);
                }

                bool isPrimaryKey = columnKey.Contains("PRI") ? true : false;

                Column column = new Column(columnName, new SingleValue(dataType.ToUpper(), dataSize), null, bIsNullable, isPrimaryKey);

                if (!tableColumns.ContainsKey(tableName))
                {
                    tableColumns.Add(tableName, new List<Column>());
                }
                tableColumns[tableName].Add(column);


            }

            dataReader.Close();

            return tableColumns;
        }

        /// <summary>
        /// Procedure generates dictionary of view's columns
        /// </summary>
        private Dictionary<string, List<Column>> getColumnsForViews(string schemaName)
        {


            Dictionary<string, List<Column>> viewColumns = new Dictionary<string, List<Column>>();

            MySqlCommand command = _mySqlConnection.CreateCommand();


            command.CommandText = string.Format("SELECT INFORMATION_SCHEMA.TABLES.TABLE_NAME, COLUMN_NAME, IS_NULLABLE, DATA_TYPE, COLUMN_TYPE, COLUMN_KEY "
                                                + "FROM INFORMATION_SCHEMA.COLUMNS "
                                                + "JOIN INFORMATION_SCHEMA.TABLES ON "
                                                + "INFORMATION_SCHEMA.TABLES.TABLE_SCHEMA = INFORMATION_SCHEMA.COLUMNS.TABLE_SCHEMA "
                                                + "AND "
                                                + "INFORMATION_SCHEMA.TABLES.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME "
                                                + "WHERE INFORMATION_SCHEMA.COLUMNS.TABLE_SCHEMA = '{0}' AND TABLE_TYPE = 'VIEW'", schemaName);


            MySqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                string tableName = dataReader.GetString(0);
                string columnName = dataReader.GetString(1);
                string isNullable = dataReader.GetString(2);
                string dataType = dataReader.GetString(3);
                string columnType = dataReader.GetString(4);
                string columnKey = dataReader.GetString(5);

                bool bIsNullable = isNullable.Equals("YES") ? true : false;
                int dataSize = 0;

                if (Regex.IsMatch(columnType, "\\w+\\(([0-9]+)\\)"))
                {
                    string ds = Regex.Matches(columnType, "\\w+\\(([0-9]+)\\)")[0].Groups[1].ToString();
                    dataSize = Int32.Parse(ds);
                }

                bool isPrimaryKey = columnKey.Contains("PRI") ? true : false;

                Column column = new Column(columnName, new SingleValue(dataType.ToUpper(), dataSize), null, bIsNullable, isPrimaryKey);

                if (!viewColumns.ContainsKey(tableName))
                {
                    viewColumns.Add(tableName, new List<Column>());
                }
                viewColumns[tableName].Add(column);


            }

            dataReader.Close();

            return viewColumns;
        }

        /// <summary>
        /// Procedure generates relationships for loaded columns, tables and schemas
        /// </summary>
        private List<Relationship> getRelationships(string schemaName, Dictionary<string, List<Column>> tableColumns)
        {
            

            List<Relationship> relationships = new List<Relationship>();

            MySqlCommand command = _mySqlConnection.CreateCommand();

            command.CommandText = string.Format(
                "SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME "
                + "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE "
                + "WHERE TABLE_SCHEMA = '{0}' "
                + "AND REFERENCED_TABLE_NAME IS NOT NULL",
                schemaName);

            MySqlDataReader dataReader = command.ExecuteReader();

            
            while (dataReader.Read())
            {
                string tableSchema = dataReader.GetString(0);
                string tableName = dataReader.GetString(1);
                string columnName = dataReader.GetString(2);
                string refTableSchema = dataReader.GetString(3);
                string refTableName = dataReader.GetString(4);
                string refColumnName = dataReader.GetString(5);


                Column primaryKeyColumn = tableColumns[tableName].Where(c => c.Name.Equals(refColumnName)).FirstOrDefault();
                Column foreignKeyColumn = tableColumns[tableName].Where(c => c.Name.Equals(columnName)).FirstOrDefault();

                relationships.Add(new Relationship(primaryKeyColumn, foreignKeyColumn, null));


            }

            dataReader.Close();

            return relationships;
        }

        /// <summary>
        /// Procedure generates indexes for found tables
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        private Dictionary<string, List<Index>> getIndexesForTables(string schemaName, Dictionary<string, List<Column>> tableColumns)
        {
            Dictionary<string, List<Index>> tableIndexes = new Dictionary<string, List<Index>>();

            MySqlCommand command = _mySqlConnection.CreateCommand();

            command.CommandText = string.Format(
                "SELECT table_name AS `Table`, "
                + "index_name AS `Index`, "
                + "GROUP_CONCAT(column_name ORDER BY seq_in_index) AS `Columns` "
                + "FROM information_schema.statistics "
                + "WHERE table_schema = '{0}' "
                + "GROUP BY 1, 2 ",
            schemaName);

            MySqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {

                string tableName = dataReader.GetString(0);
                string indexName = dataReader.GetString(1);
                List<string> columnNames = dataReader.GetString(2).Split(',').ToList();

                if (!tableIndexes.ContainsKey(tableName))
                {
                    tableIndexes.Add(tableName, new List<Index>());
                }

                List<Column> indexedColumns = new List<Column>();
                columnNames.ForEach(columnName =>
                {
                    Column column = tableColumns[tableName].Find(c => c.Name.Equals(columnName));

                    indexedColumns.Add(column);
                });

                tableIndexes[tableName].Add(new Index(indexName, indexedColumns, null));
                


            }

            dataReader.Close();

            return tableIndexes;
        }

        #endregion

        public string GetCreateScriptForTable(Table table)
        {
            if (_database.TableExists(table))
            {
                string createScript = "";
                if (_mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }

                try
                {
                    _mySqlConnection.Open();

                    MySqlCommand mySqlCommand = _mySqlConnection.CreateCommand();
                    mySqlCommand.CommandText = string.Format("SHOW CREATE TABLE {0}.{1}", table.Schema.Name, table.Name);

                    MySqlDataReader reader = mySqlCommand.ExecuteReader();


                    while (reader.Read())
                    {

                        createScript += reader.GetString(1);
                    }

                    _mySqlConnection.Close();

                    return createScript;

                }
                catch (Exception e)
                {
                    return createScript;
                }
            }
            else
            {
                return null;
            }
        }

        public string GetCreateScriptForView(View view)
        {
            if (_database.ViewExists(view))
            {
                string createScript = "";
                if (_mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }

                try
                {
                    _mySqlConnection.Open();

                    MySqlCommand mySqlCommand = _mySqlConnection.CreateCommand();
                    mySqlCommand.CommandText = string.Format("SHOW CREATE VIEW {0}.{1}", view.Schema.Name, view.Name);

                    MySqlDataReader reader = mySqlCommand.ExecuteReader();


                    while (reader.Read())
                    {

                        createScript += reader.GetString(1);
                    }

                    _mySqlConnection.Close();

                    return createScript;

                }
                catch (Exception e)
                {
                    return createScript;
                }
            }
            else
            {
                return null;
            }
        }

        public string GetCreateScriptForDatabase()
        {
            string createScript = "";
            if (_mySqlConnection.State == System.Data.ConnectionState.Open)
            {
                _mySqlConnection.Close();
            }

            try
            {
                _mySqlConnection.Open();

                MySqlCommand mySqlCommand = _mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Format("SHOW CREATE DATABASE {0}", _databaseName);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();


                while (reader.Read())
                {

                    createScript += reader.GetString(1);
                }

                _mySqlConnection.Close();

                return createScript;

            }
            catch (Exception e)
            {
                return createScript;
            }



        }

        public long GetRowCountForTable(Table table)
        {
            long rowCount = 0;

            if (!_database.TableExists(table))
                return 0;
            
            if (_mySqlConnection.State == System.Data.ConnectionState.Open)
            {
                _mySqlConnection.Close();
            }

            

            try
            {
                _mySqlConnection.Open();

                MySqlCommand mySqlCommand = _mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Format("SELECT COUNT(*) FROM {0}.{1}", table.Schema.Name, table.Name);

                MySqlDataReader reader = mySqlCommand.ExecuteReader();


                while (reader.Read())
                {
                    rowCount = reader.GetInt64(0);
                }

                _mySqlConnection.Close();

                return rowCount;

            }
            catch (Exception e)
            {
                return rowCount;
            }
        }
    }
}
