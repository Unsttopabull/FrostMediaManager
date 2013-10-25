using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Common.Models.DB.XBMC;
using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo {

    public class Movie {

        public Movie() {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Audio = new HashSet<Audio>();
            Ratings = new HashSet<Rating>();
            Plot = new HashSet<Plot>();
            Art = new HashSet<Art>();
            Certifications = new HashSet<Certification>();
            Cast = new HashSet<MoviePerson>();
            Genres = new HashSet<Genre>();
            Videos = new HashSet<Video>();
            Files = new HashSet<File>();
            Countries = new HashSet<Country>();
            Studios = new HashSet<Studio>();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        [Key]
        public long Id { get; set; }

        [ForeignKey("MainPlot")]
        public long MainPlotID { get; set; }
        public Plot MainPlot { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string SortTitle { get; set; }

        public int? Year { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime LastPlayed { get; set; }

        public DateTime Permiered { get; set; }

        public DateTime Aired { get; set; }

        public string Trailer { get; set; }

        public int Top250 { get; set; }

        public string Specials { get; set; }

        public long? Runtime { get; set; }

        public bool Watched { get; set; }

        public int PlayCount { get; set; }

        public int? FPS { get; set; }

        public double? RatingAverage { get; set; }

        public string ImdbID { get; set; }

        public string TmdbID { get; set; }

        public string Set { get; set; }

        #region Utility Functions

        public long GetFileSizeSum() { return Files.Where(f => f.Size != null).Sum(f => f.Size).Value; }

        public string GetFileSizeFormatted() { return GetFileSizeSum().FormatFileSizeAsString(); }

        public string GetGenreNames() { return string.Join(" / ", Genres.Select(g => g.Name)); }

        public Person[] GetDirectors() {
            return Cast.Where(mp => mp.Job.Equals("director", StringComparison.OrdinalIgnoreCase))
                       .Select(mp => mp.Person)
                       .ToArray();
        }

        public string GetDirectorNames() {
            IEnumerable<string> directors = Cast.Where(mp => mp.Job.Equals("director", StringComparison.OrdinalIgnoreCase)).Select(mp => mp.Person.Name);
            string directorsJoin = string.Join(" / ", directors);
            return string.IsNullOrEmpty(directorsJoin)
                           ? @"N\A"
                           : directorsJoin;
        }

        public string GetCoverPath() {
            Art cover = Art.FirstOrDefault(art => art.Type == "Cover");
            return (cover != null)
                           ? cover.Path
                           : null;
        }

        public Actor[] GetActors() {
            return Cast.Where(mp => mp.Job.Equals("actor", StringComparison.OrdinalIgnoreCase))
                       .Select(mp => mp.Person as Actor)
                       .ToArray();
        }

        public string GetStudioNamesFormatted() { return String.Join(" / ", Studios.Select(stud => stud.Name)); }

        public string[] GetStudioNames() {
            return Studios.Select(s => s.Name).ToArray();
        }

        public XjbXmlActor[] GetXjbXmlActors() {
            Actor[] actors = GetActors();
            int numActors = actors.Length;

            XjbXmlActor[] xmlActors = new XjbXmlActor[numActors];
            for (int i = 0; i < numActors; i++) {
                xmlActors[i] = (XjbXmlActor)actors[i];
            }
            return xmlActors;
        }
        #endregion

        #region Relation tables/properties

        public virtual ICollection<Subtitle> Subtitles { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<Studio> Studios { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Audio> Audio { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Plot> Plot { get; set; }
        public virtual ICollection<Art> Art { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
        public virtual ICollection<MoviePerson> Cast { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        #endregion

        #region Add Functions
        public void AddDirector(string name) {
            Cast.Add(new MoviePerson(name, "director"));
        }

        public void AddGenres(string[] genreNames) {
            if (genreNames == null) {
                return;
            }

            foreach (string genreName in genreNames) {
                Genres.Add(genreName);
            }
        }

        public void AddActors(Actor[] actors) {
            if (actors == null) {
                return;
            }

            foreach (Actor actor in actors) {
                Cast.Add(new MoviePerson(actor));
            }
        }

        public void AddActors(XjbXmlActor[] actors) {
            if (actors == null) {
                return;
            }

            foreach (Actor actor in actors) {
                Cast.Add(new MoviePerson(actor));
            }
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

        #region ConversionOperators
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
