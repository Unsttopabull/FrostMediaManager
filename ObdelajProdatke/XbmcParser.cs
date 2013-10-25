using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using Common;
using System.Linq;
using Common.Models.DB.XBMC;
using Common.Models.DB.XBMC.StreamDetails;
using MovieVo = Common.Models.DB.MovieVo.Movie;

namespace ObdelajProdatke.XBMC {
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

        public override IEnumerable<MovieVo> Movies {
            get { throw new System.NotImplementedException(); }
        }

        protected override void Process(string dbLoc) {
            ChangeConnectionString(dbLoc);

            XbmcContainer xc = new XbmcContainer();
            //string traceSql = xc.Movies.Select(mv => mv).TraceSql();
            //ICollection<XbmcPerson> q = xc.Movies.Where(mv => mv.Id == 1).Select(mv => mv.Actors).FirstOrDefault();
            //var cNames = xc.Countries.Select(c => c.Name).ToArray();

            //var q = xc.Movies.Where(mv => mv.Id == 5).Select(mv => mv.Actors);//.Select(p => new {p.Person.Id, p.Person.Name, Thumb = p.Person.ThumbXml, p.Role}));
            //var xbmcMoviePersons = q.FirstOrDefault();

            //IQueryable<XbmcMovie> xbmcMovies = xc.Movies.Where(m => m.Set != null);
            //XbmcMovie[] movies = xbmcMovies.ToArray();
            //XbmcSet[] sets = movies.Select(m => m.Set).ToArray();
            //var z = xc.Movies.FirstOrDefault(m => m.Id == 5);


            //IQueryable<XbmcMovie> xbmcMovies = xc.Movies.Where(m => m.Id == 5);
            //XbmcMovie queryable = xc.Movies.FirstOrDefault(m => m.Id == 5);
            IQueryable<XbmcFile> p = xc.Files.Where(file => file.Id == 39);

            IQueryable<IEnumerable<XbmcSubtitleDetails>> firstOrDefault = p.Select(f => f.StreamDetails.OfType<XbmcSubtitleDetails>());
            XbmcSubtitleDetails[] xbmcSubtitleDetailses = firstOrDefault.FirstOrDefault().ToArray();

            //XbmcStreamDetails[] xbmcSD = xc.StreamDetails.Where(sd => sd.StreamType == StreamType.Subtitles).ToArray();
            //IQueryable<XbmcFile> queryable = xc.Files.Where(xf => xf.FileId == 1);//.Select(f => f.VideoDetails);
            //XbmcVideoDetails[] xbmcVideoDetailses = queryable.FirstOrDefault().ToArray();

            //XbmcMovie xbmcMovies = xc.Movies.Where(m => m.Id == 1).FirstOrDefault();

            //ICollection<XbmcPerson> xbmcPersons = queryable.Actors;


            //ICollection<XbmcMovie> x2 = xc.Sets.Where(s => s.SetId == 1).Select(xs => xs.Movies).FirstOrDefault();
            //XbmcMovie xbmcSets = xc.Movies.Include("Set").FirstOrDefault(m => m.Title == "Iron Man 3");

            //XbmcSet xbmcMovies = xc.Sets.FirstOrDefault();

            //var z = x.ToArray();

            //var z = s.ToArray();

        }

        protected override void ChangeConnectionString(string databaseLocation) {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.ConnectionStrings.ConnectionStrings["XbmcEntities"].ConnectionString = "data source=" + databaseLocation;
            conf.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}