using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Properties;

namespace Frost.Common {

    /// <summary>If the databse doesn't exist it creates it and seeds it with data.</summary>
    public class SeedInitializer : IDatabaseInitializer<MovieVoContainer> {

        public const string CACHE_FILENAME = "movieVo.db3";

        public void InitializeDatabase(MovieVoContainer context) {
            try {
                if (File.Exists(CACHE_FILENAME)) {
                    Console.WriteLine(@"Cache file exists");
                    return;
                }
                File.Delete(CACHE_FILENAME);
                SQLiteConnection.CreateFile(CACHE_FILENAME);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            SQLiteCommand.Execute(Resources.MovieVoSQL, SQLiteExecuteType.NonQuery, context.Database.Connection.ConnectionString, new object());

            //XjbDbParser xjb = new XjbDbParser();
            //xjb.MakeCache(context);
        }

    }

}
