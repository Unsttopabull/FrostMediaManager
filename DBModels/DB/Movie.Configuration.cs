using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Frost.Common;
using Frost.Common.Util;
using Frost.Model.Xbmc.NFO;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;
using Frost.Models.Xtreamer.NFO;
using Frost.Models.Xtreamer.PHP;

namespace Frost.Models.Frost.DB {

    public partial class Movie {
        internal class Configuration : EntityTypeConfiguration<Movie> {
            public Configuration() {
                ToTable("Movies");

				//Movie <--> Set
                HasOptional(m => m.Set)
                    .WithMany(s => s.Movies)
                    .HasForeignKey(movie => movie.SetId);

				//Movie <--> Ratings
                HasMany(m => m.Ratings)
                    .WithRequired(r => r.Movie)
                    .HasForeignKey(r => r.MovieId)
                    .WillCascadeOnDelete();

                //Movie <--> Plots
                HasMany(m => m.Plots)
                    .WithRequired(p => p.Movie)
                    .HasForeignKey(p => p.MovieId)
                    .WillCascadeOnDelete();
            }
        }

        #region Xtreamer

        public static explicit operator Movie(Coretis_VO_Movie m) {
            Movie mov = new Movie();

            m.GetMovieTitle(mov);
            mov.Genres = m.genreArr.ToObservableHashSet<Genre, Coretis_VO_Genre>();
            m.AddNewCast(mov);

            //if the full plot summary exists add new plot otherwise we discard all plot info 
            if (!string.IsNullOrEmpty(m.plotFull)) {
                mov.Plots.Add(new Plot(m.plotFull, m.plotSummary, null));
            }

            m.GetInfo(mov);
            m.AddAudioVideoInfo(mov);

            //mov.Files.Add(new File(m.fileName, m.fileExtension, m.filePathOnDrive, (long) m.fileSize));

            m.AddArt(mov);
            return mov;
        }

        /// <summary>Converts an instance of <see cref="XjbXmlMovie"/> to <see cref="Common.Models.DB.MovieVo.Movie">Movie</see> by explicit casting</summary>
        /// <param name="xm">The <see cref="XjbXmlMovie"/> to convert</param>
        /// <returns><see cref="XjbXmlMovie"/> converted to an instance of <see cref="Common.Models.DB.MovieVo.Movie">Movie</see></returns>
        public static explicit operator Movie(XjbXmlMovie xm) {
            long? runtimeInSec = null;
            if (xm.Runtime.HasValue) {
                //convert mins to seconds
                runtimeInSec = xm.Runtime.Value * 60;
            }

            Movie mv = new Movie {
                Title = xm.Title,
                OriginalTitle = xm.OriginalTitle,
                ReleaseYear = xm.Year,
                RatingAverage = xm.AverageRating,
                Certifications = new ObservableHashSet<Certification>(Certification.ParseCertificationsString(xm.Certifications)),
                ImdbID = xm.ImdbId,
                Runtime = runtimeInSec
            };

            if (!string.IsNullOrEmpty(xm.Studio)) {
                mv.Studios.Add(new Studio(xm.Studio));
            }

            //add all available plot info (constructor will omit null/empty ones)
            mv.Plots.Add(new Plot(xm.Plot, xm.Outline, xm.Tagline, language: null));

            if (!string.IsNullOrEmpty(xm.Director)) {
                mv.Directors.Add(new Person(xm.Director));
            }

            //Add Genres, if a genre already exists it wont be duplicated
            mv.Genres.UnionWith(Genre.GetFromNames(xm.Genres));

            //Convert and Add XjbXmlActor array to HashSet<Actor>
            if (xm.Actors != null) {
                foreach (XjbXmlActor actor in xm.Actors) {
                    mv.ActorsLink.Add(new MovieActor(mv, new Person(actor.Name, actor.Thumb), actor.Role));
                }
            }

            return mv;
        }

        /// <summary>Loads the serialized movie info XML file from the specified path and converts it into <see cref="Movie"/>.</summary>
        /// <param name="pathToXml">The path to info XML file.</param>
        /// <returns>Instance of <see cref="Movie"/> converted from deserialized info XML in the given path</returns>
        public static Movie LoadXjbNfoAsMovie(string pathToXml) {
            return (Movie) XjbXmlMovie.Load(pathToXml);
        }

        #endregion

        #region XBMC

        /// <summary>Converts an instance of <see cref="XbmcXmlMovie"/> to <see cref="Common.Models.DB.MovieVo.Movie">Movie</see> by explicit casting</summary>
        /// <param name="mx">The <see cref="XbmcXmlMovie"/> to convert.</param>
        /// <returns><see cref="XbmcXmlMovie"/> converted to an instance of <see cref="Common.Models.DB.MovieVo.Movie">Movie</see></returns>
        public static explicit operator Movie(XbmcXmlMovie mx) {
            Movie mv = new Movie {
                Aired = mx.Aired,
                ImdbID = mx.ImdbId,
                LastPlayed = mx.LastPlayed,
                OriginalTitle = mx.OriginalTitle,
                Premiered = mx.Premiered,
                PlayCount = mx.PlayCount,
                RatingAverage = mx.Rating,
                ReleaseDate = mx.ReleaseDate,
                Runtime = mx.RuntimeInSeconds * 1000,
                Set = new Set(mx.Set),
                SortTitle = mx.SortTitle,
                Title = mx.Title,
                TmdbID = mx.TmdbId,
                Top250 = mx.Top250,
                Trailer = mx.GetTrailerUrl(),
                Watched = mx.Watched,
                ReleaseYear = mx.Year,
                Art = new ObservableHashSet<ArtBase>(mx.GetArt()),
                Directors = mx.GetDirectors(),
                Writers = mx.GetWriters(),
                Genres = new ObservableHashSet<Genre>(Genre.GetFromNames(mx.Genres)),
                Studios = new ObservableHashSet<Studio>(Studio.GetFromNames(mx.Studios)),
                Countries = new ObservableHashSet<Country>(Country.GetFromNames(mx.Countries)),
                Certifications = new ObservableHashSet<Certification>(mx.GetCertifications()),
                Audios = new ObservableHashSet<Audio>(mx.GetAudio()),
                Videos = new ObservableHashSet<Video>(mx.GetVideo()),
                Subtitles = new ObservableHashSet<Subtitle>(mx.GetSubtitles()),
                //Files = mx.GetFiles()
            };
            mv.ActorsLink.UnionWith(mx.Actors.Select(a => new MovieActor(mv, new Person(a.Name, a.Thumb), a.Role)));
            mv.Plots.Add(new Plot(mx.Plot, mx.Outline, mx.Tagline, null));

            return mv;
        }

        /// <summary>Deserializes an instance of <see cref="XbmcXmlMovie"/> from XML at the specified location and converts it to <see cref="Movie"/></summary>
        /// <param name="xmlLocation">The file path of the serialied xml.</param>
        /// <returns>An instance of <see cref="Movie"/> converted from deserialized instance of <see cref="XbmcXmlMovie"/> at the specified location</returns>
        public static Movie LoadXbmcNfoAsMovie(string xmlLocation) {
            return (Movie) XbmcXmlMovie.Load(xmlLocation);
        }
        #endregion
    }

}