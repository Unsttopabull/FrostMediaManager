using System;
using System.Collections.Generic;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;

namespace Frost.ProcessDatabase {
    public abstract class MediaManager<T> where T : class {
        protected List<T> ObdelaniFilmi;

        protected bool DBFound { get; set; }

        public abstract IEnumerable<T> RawMovies { get; }
        public abstract IEnumerable<Movie> Movies { get; }

        protected MediaManager(DBSystem sistem) {
            string dbLoc = DBCheck.FindDB(sistem);
            if (dbLoc != null) {
                Process(dbLoc);
                DBFound = true;
            }
        }

        protected MediaManager(DBSystem sistem, string dbLocation) {
            if (string.IsNullOrEmpty(dbLocation.Trim())) {
                throw new ArgumentException(@"Can not be null or empty", "dbLocation");
            }

            if (DBCheck.CheckDBLocation(sistem, dbLocation) == null) {
                DBFound = false;
                return;
            }

            Process(dbLocation);
            DBFound = true;            
        }

        protected abstract void Process(string dbLoc);
        protected abstract void ChangeConnectionString(string databaseLocation);
    }
}