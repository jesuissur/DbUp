﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Preprocessors;
using DbUp.Engine.Transactions;
using DbUp.Support.SqlServer;

namespace DbUp.Support.SqlAnywhere
{
    public class SqlAnywhereTableJournal : SqlTableJournal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAnywhereTableJournal"/> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager.</param>
        /// <param name="logger">The log.</param>
        /// <param name="schema">The schema that contains the table.</param>
        /// <param name="table">The table name.</param>
        /// <param name="sqlProcessor">the sql preprocessor to transform sql before execution</param>
        /// <example>
        /// var journal = new TableJournal("DBN=MyDatabaseName;UID=DBA;PWD=sql", "schemaNameForVersionTable", "MyVersionTable");
        /// </example>
        public SqlAnywhereTableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table, IScriptPreprocessor sqlProcessor)
            : base(connectionManager, logger, schema, table)
        {
            SqlProcessor = sqlProcessor ?? new NullPreprocessor();
        }

        protected override bool VerifyTableExistsCommand(IDbCommand command, string tableName, string schemaName)
        {
            command.CommandText = string.Format("select object_id('{0}')", CreateTableName(schemaName, tableName));
            command.CommandType = CommandType.Text;
            var result = command.ExecuteScalar() as int?;
            return result.HasValue;
        }
    }
}
