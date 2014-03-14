using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;

namespace Frost.Models.Frost {

    /// <summary>If the databse doesn't exist it creates it and seeds it with data.</summary>
    public class SQLiteInitializer<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext {
        private readonly string _initSQL;
        private readonly bool _dropCreate;


        /// <summary>Initializes a new instance of the <see cref="SQLiteInitializer{TContext}"/> class.</summary>
        /// <param name="initSQL">The initialization SQL DDL/SDL.</param>
        /// <param name="dropCreate"></param>
        public SQLiteInitializer(string initSQL, bool dropCreate = false) {
            _initSQL = initSQL;
            _dropCreate = dropCreate;
        }

        public void InitializeDatabase(TContext context) {
            string dbName = context.Database.Connection.ConnectionString.Split('=')[1];
            try {
                if (_dropCreate && File.Exists(dbName)) {
                    try {
                        File.Delete(dbName);
                    }
                    catch {
                        
                    }
                }

                if (!File.Exists(dbName)) {
                    SQLiteConnection.CreateFile(dbName);
                    SQLiteCommand.Execute(_initSQL, SQLiteExecuteType.NonQuery, context.Database.Connection.ConnectionString, new object());
                    return;
                }

                if (_dropCreate) {
                    SQLiteCommand.Execute(_initSQL, SQLiteExecuteType.NonQuery, context.Database.Connection.ConnectionString, new object());
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }
        }

    }

}
