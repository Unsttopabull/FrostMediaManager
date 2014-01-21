using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frost.Common;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo;
using Frost.PHPtoNET;

using CoretisMovie = Frost.Common.Models.PHP.Coretis_VO_Movie;

namespace Frost.ProcessDatabase {
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
            string[] phpFilmi;
            using (XjbEntities xjb = new XjbEntities(dbLoc)) {
                phpFilmi = xjb.Movies.Select(mov => mov.MovieVo).ToArray();
            }

            int stFilmov = phpFilmi.Length;
            ObdelaniFilmi = new List<CoretisMovie>(stFilmov);

            PHPDeserializer2 objParser = new PHPDeserializer2();
            for (int i = 0; i < stFilmov; i++) {
                phpFilmi[i] = phpFilmi[i].Replace('\n', ' ');
                
                CoretisMovie mv;
                using (PHPSerializedStream phpStream = new PHPSerializedStream(phpFilmi[i], Encoding.UTF8)) {
                    try {
                        mv = objParser.Deserialize<CoretisMovie>(phpStream);
                    }
                    catch (Exception) {
                        continue;
                    }
                }

                ObdelaniFilmi.Add(mv);
            }
        }
    }
}
