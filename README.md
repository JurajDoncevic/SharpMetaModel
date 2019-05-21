# SharpMetaModel
SharpMetaModel is an experimental library for database metamodelling. It is built using C# and .NET CORE (currently 2.2). 
It is mainly used for my doctoral research, so features are likely to appear as I find the time to build them or as a need appears.

SharpMetaModel is inspired by the *[Apache Metamodel](https://metamodel.apache.org/)* library, but it is not a language-ported library.



##Supported databases
SharpMetaModel currently supports the extraction of metamodels from:
* MySQL
* MongoDB

##Features
Schema extraction from supported relational databases.
Schema discovery for supported NoSQL databases.

##Future plans
SharpMetaModel is planned to support a greater set of features and databases. To name some databases that could be supported:
* PostgreSQL
* Neo4J
* MSSQL
* HBase
* LiteDB
* Sqlite

Planned features:
* Editing the schema (CREATE ALTER)
* Query execution
* OR, OD, OG mapping (maybe?)
