using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common;
using Common.Models.DB.MovieVo;
using Common.Models.DB.XBMC;

namespace ObdelajProdatke {
    public class XbmcParser : MediaManager<XbmcMovie> {

        public XbmcParser()
            : base(DBSystem.XBMC) {
        }

        public XbmcParser(string dbLocation)
            : base(DBSystem.XBMC, dbLocation) {
        }

        public override IEnumerable<XbmcMovie> RawMovies {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerable<Movie> Movies {
            get { throw new NotImplementedException(); }
        }

        protected override void Process(string dbLoc) {
            ChangeConnectionString(dbLoc);

            XbmcContainer xc = new XbmcContainer();
            int xbmcMovies = xc.Movies.Count(m => m.Set != null);
//            XbmcMovie[] movies = xbmcMovies.ToArray();
        }

        protected override void ChangeConnectionString(string databaseLocation) {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.ConnectionStrings.ConnectionStrings["XbmcEntities"].ConnectionString = "data source=" + databaseLocation;
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}