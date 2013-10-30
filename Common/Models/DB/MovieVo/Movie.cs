using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Common.Models.DB.Jukebox;
using Common.Models.DB.MovieVo.Arts;
using Common.Models.DB.MovieVo.People;
using Common.Models.DB.XBMC;
using Common.Models.XML.Jukebox;
using Common.Models.XML.XBMC;

namespace Common.Models.DB.MovieVo {

    public class Movie {
        private const string SEPARATOR = " / ";

        public Movie() {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Audio = new HashSet<Audio>();
            Ratings = new HashSet<Rating>();
            Plot = new HashSet<Plot>();
            Art = new HashSet<Art>();
            Certifications = new HashSet<Certification>();
            Genres = new HashSet<Genre>();
            Videos = new HashSet<Video>();
            Files = new HashSet<File>();
            Countries = new HashSet<Country>();
            Studios = new HashSet<Studio>();
            Specials = new HashSet<Special>();

            Directors = new HashSet<Person>();
            Writers = new HashSet<Person>();
            ActorsLink = new HashSet<MovieActor>();

            Actors = new HashSet<Actor>();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        [Key]
        public long Id { get; set; }

        [ForeignKey("MainPlot")]
        public long MainPlotID { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string SortTitle { get; set; }

        public int? Year { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime LastPlayed { get; set; }

        public DateTime Permiered { get; set; }

        public DateTime Aired { get; set; }

        public string Trailer { get; set; }

        public int? Top250 { get; set; }

        public long? Runtime { get; set; }

        public bool Watched { get; set; }

        public int PlayCount { get; set; }

        public double? RatingAverage { get; set; }

        public string ImdbID { get; set; }

        public string TmdbID { get; set; }

        public long? SetId { get; set; }

        #region Relation tables/properties

        public virtual Plot MainPlot { get; set; }

        [ForeignKey("SetId")]
        public virtual Set Set { get; set; }

        public virtual HashSet<Subtitle> Subtitles { get; set; }
        public virtual HashSet<Country> Countries { get; set; }
        public virtual HashSet<Studio> Studios { get; set; }

        public virtual HashSet<Video> Videos { get; set; }
        public virtual HashSet<File> Files { get; set; }

        public virtual HashSet<Audio> Audio { get; set; }
        public virtual HashSet<Rating> Ratings { get; set; }
        public virtual HashSet<Plot> Plot { get; set; }
        public virtual HashSet<Art> Art { get; set; }
        public virtual HashSet<Certification> Certifications { get; set; }

        [InverseProperty("MoviesAsWriter")]
        public virtual HashSet<Person> Writers { get; set; }

        [InverseProperty("MoviesAsDirector")]
        public virtual HashSet<Person> Directors { get; set; }

        [InverseProperty("MoviesLink")]
        public virtual HashSet<MovieActor> ActorsLink { get; set; }

        public virtual HashSet<Special> Specials { get; set; }
        public virtual HashSet<Genre> Genres { get; set; }

        [NotMapped]
        public HashSet<Actor> Actors {
            get { return new HashSet<Actor>(ActorsLink.Select(ma => (Actor)ma)); }
            set {
                ActorsLink.UnionWith(value.Select(a => new MovieActor(a)));
            }
        }
        #endregion

        #region Utility Functions

        public long GetFileSizeSum() { return Files.Where(f => f.Size != null).Sum(f => f.Size).Value; }

        public string GetFileSizeFormatted() { return GetFileSizeSum().FormatFileSizeAsString(); }

        public string GetGenreNames() { return string.Join(SEPARATOR, Genres.Select(g => g.Name)); }

        public string GetDirectorNames() {
            IEnumerable<string> directors = Directors.Select(d => d.Name);
            string directorsJoin = string.Join(SEPARATOR, directors);
            return string.IsNullOrEmpty(directorsJoin)
                           ? @"N\A"
                           : directorsJoin;
        }

        public string GetCoverPath() {
            Cover cover = Art.OfType<Cover>().FirstOrDefault();
            return (cover != null)
                           ? cover.Path
                           : null;
        }

        public string GetStudioNamesFormatted() {
            return String.Join(SEPARATOR, Studios.Select(stud => stud.Name));
        }

        public string[] GetStudioNames() {
            return Studios.Select(s => s.Name).ToArray();
        }

        public XjbXmlActor[] GetXjbXmlActors() {
            return Actors.Select(a => (XjbXmlActor)a).ToArray();
        }
        #endregion

        public override string ToString() {
            return string.Format("{0} ({1})", Title, Year);
        }

        #region Serialization
        public void Serialize(XmlSystem system, string xmlSaveLocation) {
            switch (system) {
                case XmlSystem.Xtreamer:
                    ((XjbXmlMovie)this).Serialize(xmlSaveLocation);
                    break;
                case XmlSystem.XBMC:
                    ((XbmcXmlMovie)this).Serialize(xmlSaveLocation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("system");
            }
        }

        public Movie Load(XmlSystem system, string xmlLocation) {
            switch (system) {
                case XmlSystem.Xtreamer:
                    return XjbXmlMovie.LoadAsMovie(xmlLocation);
                case XmlSystem.XBMC:
                    return XbmcXmlMovie.LoadAsMovie(xmlLocation);
                default:
                    throw new ArgumentOutOfRangeException("system");
            }
        }
        #endregion

        #region Conversion Functions
        public XjbXmlMovie ToXjbXmlMovie() {
            return (XjbXmlMovie)this;
        }

        public XbmcXmlMovie ToXbmcXmlMovie() {
            return (XbmcXmlMovie)this;
        }

        public XbmcMovie ToXbmcMovie() {
            return (XbmcMovie)this;
        }

        public XjbMovie ToXjbMovie() {
            return (XjbMovie)this;
        }
        #endregion

        #region Conversion Operators
        public static explicit operator XjbMovie(Movie movie) {
            throw new NotImplementedException();
        }

        public static explicit operator XjbXmlMovie(Movie movie) {
            var z = new XjbXmlMovie {
                Certifications = movie.Certifications.ToArray(),
                Director = movie.GetDirectorNames(),
                GenreString = movie.GetGenreNames(),
                ID = movie.ImdbID,
                OriginalTitle = movie.OriginalTitle,
                Outline = movie.MainPlot.Summary,
                Plot = movie.MainPlot.Full,
                Rating = (float)(movie.RatingAverage ?? 0),
                //TODO: CHECK FOR CORECT FORMAT
                ReleaseDate = movie.ReleaseDate.ToString(CultureInfo.InvariantCulture),
                Runtime = movie.Runtime.HasValue ? (movie.Runtime / 60) : 0,
                SortTitle = movie.Title,
                Studio = movie.GetStudioNamesFormatted(),
                Tagline = movie.MainPlot.Summary,
                Title = movie.Title,
                Year = movie.Year ?? 0,
                Actors = movie.GetXjbXmlActors(),
                //FileInfo
                //MPAA
                //Votes = 
                //Credits =
            };
            return z;
        }

        public static explicit operator XbmcXmlMovie(Movie movie) {
            throw new NotImplementedException();
        }

        public static explicit operator XbmcMovie(Movie movie) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
