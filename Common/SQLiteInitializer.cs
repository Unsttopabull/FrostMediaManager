using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using Frost.Common.Models.DB.MovieVo;

namespace Frost.Common {

    /// <summary>If the databse doesn't exist it creates it and seeds it with data.</summary>
    public class SQLiteInitializer : IDatabaseInitializer<MovieVoContainer> {
        private readonly string _initSQL;

        /// <summary>Initializes a new instance of the <see cref="SQLiteInitializer"/> class.</summary>
        /// <param name="initSQL">The initialization SQL DDL/SDL.</param>
        public SQLiteInitializer(string initSQL) {
            _initSQL = initSQL;
        }

        public void InitializeDatabase(MovieVoContainer context) {
            string dbName = context.Database.Connection.ConnectionString.Split('=')[1];
            try {
                if (!File.Exists(dbName)) {
                    SQLiteConnection.CreateFile(dbName);
                }
                SQLiteCommand.Execute(_initSQL, SQLiteExecuteType.NonQuery, context.Database.Connection.ConnectionString, new object());
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }
        }

    }

}
