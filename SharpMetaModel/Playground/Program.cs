using Microsoft.Extensions.Configuration;
using SharpMetaModel.Core.ObjectModel.Relational;
using SharpMetaModel.MongoDB;
using SharpMetaModel.MySQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Playground
{
    // DataContextFactory.CreateMongoDataContext("ds046037.mlab.com", 46037, "test_db", "test_user", "a123456");
    class Start
    {
        private static IConfiguration _config;

        public static void Main(string[] args)
        {

            _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            MySQLPrintTest();
            MongoPrintTest();
            //MySQLOneOff();
            //MongoOneOff();
            
            //MongoTimeTest(500);
            //MySQLTimeTest(10);
            //MySQLCreateScriptTest();
        }

        

        private static void MongoOneOff()
        {
            MongoDBDataContext mongoDataContext = new MongoDBDataContextFactory().CreateDataContext(_config["ConnectionStrings:MongoDB"], "phone_list_dw_sep");
            
        }

        private static void MySQLOneOff()
        {
            MySQLDataContext mySQLDataContext = new MySQLDataContextFactory().CreateDataContext(_config["ConnectionStrings:MySQL"], "hammurabi");
            
        }

        private static void MySQLTimeTest(int iterations)
        {
            Stopwatch sw = new Stopwatch();
            List<long> times = new List<long>();

            for (int i = 0; i < iterations; i++)
            {
                sw.Start();
                MySQLDataContext mySQLDataContext = new MySQLDataContextFactory().CreateDataContext(_config["ConnectionStrings:MySQL"], "hammurabi");
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
                sw.Reset();
            }

            times.ForEach(t => Console.WriteLine(t));
        }

        private static void MongoTimeTest(int iterations)
        {
            Stopwatch sw = new Stopwatch();
            List<long> times = new List<long>();

            for (int i = 0; i < iterations; i++)
            {
                sw.Start();
                MongoDBDataContext mongoDataContext = new MongoDBDataContextFactory().CreateDataContext(_config["ConnectionStrings:MongoDB"], "phone_list_dw_sep");
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
                sw.Reset();
            }

            times.ForEach(t => Console.WriteLine(t));
        }

        private static void MongoPrintTest()
        {
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            MongoDBDataContext mongoDataContext = new MongoDBDataContextFactory().CreateDataContext(_config["ConnectionStrings:MongoDB"], "phone_list_dw_sep");
            sw1.Stop();
            Console.WriteLine(mongoDataContext.Database.Name);
            mongoDataContext.Database.Collections.ForEach(c =>
            {
                Console.WriteLine("\t|" + c.Name);

                c.Keys.ForEach(k =>
                {
                    Console.WriteLine("\t\t|Key:" + k.Name);

                });
                c.Indexes.ForEach(i =>
                {
                    Console.WriteLine("\t\t|Index on: " + i.OnKeyIdentifiers.Aggregate((i1, i2) => i1 + "," + i2));
                });
            });

            Console.WriteLine(sw1.ElapsedMilliseconds);
        }

        private static void MySQLPrintTest()
        {
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            MySQLDataContext mySQLDataContext = new MySQLDataContextFactory().CreateDataContext(_config["ConnectionStrings:MySQL"], "hammurabi");
            sw1.Stop();


            foreach (Schema schema in mySQLDataContext.Database.Schemas)
            {
                Console.WriteLine("SCHEMA: " + schema.Name);
                foreach (Table table in schema.Tables)
                {
                    Console.WriteLine("\tTABLE: " + table.Name + "\n\tno. rows: " + mySQLDataContext.GetRowCountForTable(table));
                    Console.WriteLine("\t\tCOLUMNS:");
                    foreach (Column column in table.Columns)
                    {
                        Console.WriteLine(string.Format("\t\t{0} is {1} of size {2}. PK:{3}, FK:{4}. NULLABLE: {5}", column.Name, column.Value.DataType, column.Value.DataSize, column.IsPrimaryKey, column.IsForeignKey, column.IsNullable));

                    }
                    Console.WriteLine("\t\tIndexes:");
                    foreach (Index index in table.Indexes)
                    {
                        Console.WriteLine("\t\t{0} on {1}", index.Name, index.IndexedColumns.Select(c => c.Name).Aggregate((n1, n2) => n1 + ", " + n2));
                    }

                }

                foreach (View view in schema.Views)
                {
                    Console.WriteLine("\tVIEW: " + view.Name);
                    Console.WriteLine("\t\tCOLUMNS:");
                    foreach (Column column in view.Columns)
                    {
                        Console.WriteLine(string.Format("\t\t{0} is {1} of size {2}. PK:{3}, FK:{4}. NULLABLE: {5}", column.Name, column.Value.DataType, column.Value.DataSize, column.IsPrimaryKey, column.IsForeignKey, column.IsNullable));
                    }
                }
            }

            Console.WriteLine(sw1.ElapsedMilliseconds);
        }

        private static void MySQLCreateScriptTest()
        {
            MySQLDataContext mySQLDataContext = new MySQLDataContextFactory().CreateDataContext(_config["ConnectionStrings:MySQL"], "hammurabi");


            List<Table> tables =
                mySQLDataContext
                .Database
                .Schemas
                .SelectMany(s => s.Tables)
                .ToList();

            List<View> views =
                mySQLDataContext
                .Database
                .Schemas
                .SelectMany(s => s.Views)
                .ToList();

            Console.WriteLine("TABLES:");
            tables.ForEach(t =>
            {
                Console.WriteLine(mySQLDataContext.GetCreateScriptForTable(t) + "\n-------------------------------------------------------------------\n");
            });


            Console.WriteLine("VIEWS:");
            views.ForEach(v =>
            {
                Console.WriteLine(mySQLDataContext.GetCreateScriptForView(v) + "\n-------------------------------------------------------------------\n");
            });

            Console.WriteLine("DATABASE:");
            Console.WriteLine(mySQLDataContext.GetCreateScriptForDatabase());
        }
    }
}
