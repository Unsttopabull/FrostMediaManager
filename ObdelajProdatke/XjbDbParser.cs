using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common;
using Common.Models.DB.Jukebox;
using Common.Models.DB.MovieVo;
using PHPSerialize;
using CoretisMovie = Common.Models.PHP.Coretis_VO_Movie;

namespace ObdelajProdatke {
    public class XjbDbParser : MediaManager<CoretisMovie> {

        public XjbDbParser() : base(DBSystem.Xtreamer) {
        }

        public XjbDbParser(string dbLocation) : base(DBSystem.Xtreamer, dbLocation) {
        }

        public override IEnumerable<CoretisMovie> RawMovies {
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
            ObdelaniFilmi = new List<CoretisMovie>(stFilmov);


            for (int i = 0; i < stFilmov; i++) {
                phpFilmi[i] = phpFilmi[i].Replace('\n', ' ');
                PHPObjectParser objParser = new PHPObjectParser(new Scanner(phpFilmi[i]));

                CoretisMovie mv = new CoretisMovie();
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
