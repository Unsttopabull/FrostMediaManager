using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.PHP;
using Frost.PHPtoNET;

namespace Frost.ProcessDatabase {
    public class XjbDbParser : MediaManager<Coretis_VO_Movie> {

        public XjbDbParser() : base(DBSystem.Xtreamer) {
        }

        public XjbDbParser(string dbLocation) : base(DBSystem.Xtreamer, dbLocation) {
        }

        public override IEnumerable<Coretis_VO_Movie> RawMovies {
            get {
                return DBFound
                    ? ObdelaniFilmi
                    : null;
            }
        }

        public override IEnumerable<Movie> Movies {
            get { return ObdelaniFilmi.ConvertAll(cm => (Movie) cm); }
        }

        protected override void Process(string dbLoc) {
            ChangeConnectionString(dbLoc);

            XjbEntities xjb = new XjbEntities();
            string[] phpFilmi = xjb.Movies.Select(mov => mov.MovieVo).ToArray();

            int stFilmov = phpFilmi.Length;
            ObdelaniFilmi = new List<Coretis_VO_Movie>(stFilmov);


            for (int i = 0; i < stFilmov; i++) {
                phpFilmi[i] = phpFilmi[i].Replace('\n', ' ');
                PHPObjectParser objParser = new PHPObjectParser(new Scanner(phpFilmi[i]));

                Coretis_VO_Movie mv = new Coretis_VO_Movie();
                try {
                    objParser.Obj(ref mv);
                }
                catch (Exception) {
                    continue;
                }

                ObdelaniFilmi.Add(mv);
            }
        }

        protected override void ChangeConnectionString(string databaseLocation) {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.ConnectionStrings.ConnectionStrings["xjbEntities"].ConnectionString = "data source=" + databaseLocation;
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
