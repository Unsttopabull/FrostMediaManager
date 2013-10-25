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
            get { throw new System.NotImplementedException(); }
        }

        public override IEnumerable<Movie> Movies {
            get { throw new System.NotImplementedException(); }
        }

        protected override void Process(string dbLoc) {
            ChangeConnectionString(dbLoc);

            XbmcContainer xc = new XbmcContainer();
        }

        protected override void ChangeConnectionString(string databaseLocation) {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.ConnectionStrings.ConnectionStrings["XbmcEntities"].ConnectionString = "data source=" + databaseLocation;
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}