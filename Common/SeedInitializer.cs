using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Properties;

namespace Frost.Common {

    /// <summary>If the databse doesn't exist it creates it and seeds it with data.</summary>
    public class SeedInitializer : IDatabaseInitializer<MovieVoContainer> {

        public void InitializeDatabase(MovieVoContainer context) {
            string dbName = context.Database.Connection.ConnectionString.Split('=')[1];
            try {
                if (!File.Exists(dbName)) {
                    SQLiteConnection.CreateFile(dbName);
                }
                SQLiteCommand.Execute(Resources.MovieVoSQL, SQLiteExecuteType.NonQuery, context.Database.Connection.ConnectionString, new object());
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }
        }

    }

}
