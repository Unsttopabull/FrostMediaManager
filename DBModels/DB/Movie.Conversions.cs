using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frost.Common;
using Frost.Common.Util;
using Frost.Model.Xbmc.NFO;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;
using Frost.Models.Xtreamer.NFO;
using Frost.Models.Xtreamer.PHP;

namespace Frost.Models.Frost.DB
{
    public partial class Movie
    {

        #region Xtreamer

        public static explicit operator Movie(Coretis_VO_Movie m) {
            Movie mov = new Movie();

            GetMovieTitle(m, mov);
            mov.Genres = m.genreArr.ToObservableHashSet<Genre, Coretis_VO_Genre>();
            AddActors(m, mov);

            //if the full plot summary exists add new plot otherwise we discard all plot info 
            if (!string.IsNullOrEmpty(m.plotFull)) {
                mov.Plots.Add(new Plot(m.plotFull, m.plotSummary, null));
            }

            GetInfo(m, mov);
            AddAudioVideoInfo(m, mov);

            //mov.Files.Add(new File(m.fileName, m.fileExtension, m.filePathOnDrive, (long) m.fileSize));

            AddArt(m, mov);
            return mov;
        }

        private static void AddAudioVideoInfo(Coretis_VO_Movie m, Movie mov) {
            mov.Audios.Add(new Audio(
                m.audioSource,
                m.audioType,
                m.acodec,
                m.achannels
                ));

            mov.Videos.Add(new Video(
                m.vcodec,
                m.width.HasValue ? m.width.Value : default(int),
                m.height.HasValue ? m.height.Value : default(int),
                m.aspect.HasValue ? m.aspect.Value : default(double),
                m.fps,
                m.videoFormat,
                m.videoType,
                m.videoSource
                ));
        }

        private static void AddActors(Coretis_VO_Movie m, Movie mov) {
            foreach (Coretis_VO_Person cPerson in m.personArr) {
                if (cPerson.job.OrdinalEquals("actor")) {
                    mov.ActorsLink.Add(new MovieActor(mov, new Person(cPerson.name, null), cPerson.character));
                    continue;
                }

                Person person = new Person(cPerson.name);
                if (cPerson.job.OrdinalEquals("director")) {
                    mov.Directors.Add(person);
                }
                else if (cPerson.job.OrdinalEquals("writer")) {
                    mov.Writers.Add(person);
                }
            }
        }

        private static void GetMovieTitle(Coretis_VO_Movie m, Movie mov) {
            mov.Title = m.name;
            if (!string.IsNullOrEmpty(m.titleOrg)) {
                mov.OriginalTitle = m.titleOrg;
            }

            if (!string.IsNullOrEmpty(m.titleSort)) {
                mov.SortTitle = m.titleSort;
            }
        }

        private static void AddArt(Coretis_VO_Movie m, Movie mov) {
            if (!string.IsNullOrEmpty(m.pathCover)) {
                mov.Art.Add(new Cover(m.pathCover));
            }

            if (m.pathScreenArr != null) {
                //if the array is not null we add all art as Fanart
                mov.Art.UnionWith(m.pathScreenArr.Select(screen => new Fanart(screen)));
            }

            if (m.pathFanartArr != null) {
                mov.Art.UnionWith(m.pathFanartArr.Select(fanart => new Fanart(fanart)));
            }
        }

        public static void GetInfo(Coretis_VO_Movie phpMovie, Movie mov) {
            //convert the year to int if valid number
            int result;
            if (int.TryParse(phpMovie.year, out result)) {
                mov.ReleaseYear = result;
            }

            if (phpMovie.subtitle != null) {
                //if subtitle language contains the word SUBBED
                //it is embeded in the movie video
                bool embeded = phpMovie.subtitle.Contains("SUBBED");

                string subtitleLang = phpMovie.subtitle;
                if (embeded) {
                    //remove the "SUBBED" from the string and trim empty space
                    subtitleLang = phpMovie.subtitle.Replace("SUBBED", "").Trim();
                }

                mov.Subtitles.Add(new Subtitle(null, subtitleLang, null, embeded));
            }

            //Split the specials string where "/" or "," 
            //and convert the resulting string array to a HashSet<Special>
            mov.Specials = phpMovie.specials.SplitWithoutEmptyEntries("/", ",").ToObservableHashSet<Special, string>();

            mov.Runtime = (phpMovie.length > 0)
                              ? (long?) phpMovie.length
                              : null;

            mov.RatingAverage = phpMovie.ratingAverage;
            mov.ImdbID = phpMovie.imdbId;
            mov.Studios.Add(new Studio(phpMovie.studio));
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
                Certifications = new ObservableHashSet<Certification>(Certification.ParseCertificationsString(xm.CertificationsString)),
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
        public static Movie LoadFromXtreamerNfo(string pathToXml) {
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
                Art = new ObservableHashSet<ArtBase>(GetArt(mx)),
                Directors = new ObservableHashSet<Person>(from d in mx.Directors
                                                          where !string.IsNullOrEmpty(d)
                                                          select new Person(d)),

                Writers = new ObservableHashSet<Person>(from c in mx.Credits
                                                        where !string.IsNullOrEmpty(c)
                                                        select new Person(c)),

                Genres = new ObservableHashSet<Genre>(Genre.GetFromNames(mx.Genres)),
                Studios = new ObservableHashSet<Studio>(Studio.GetFromNames(mx.Studios)),
                Countries = new ObservableHashSet<Country>(Country.GetFromNames(mx.Countries)),

                //Files = mx.GetFiles()
            };

            if (mx.Certifications != null) {
                mv.Certifications = new ObservableHashSet<Certification>(mx.Certifications.Select(xmlCert => (Certification) xmlCert));
            }

            if (mx.FileInfo.InfoExists(MediaType.Audio)) {
                mv.Audios = new ObservableHashSet<Audio>(mx.FileInfo.Audios.Select(xmlAudio => (Audio) xmlAudio));
                
            }

            if (mx.FileInfo.InfoExists(MediaType.Subtitles)) {
                mv.Subtitles = new ObservableHashSet<Subtitle>(mx.FileInfo.Subtitles.Select(xmlSub => (Subtitle) xmlSub));
            }
            
            if(mx.FileInfo.InfoExists(MediaType.Video)){
                mv.Videos = new ObservableHashSet<Video>(mx.FileInfo.Videos.Select(xmlVideo => (Video) xmlVideo));
            }

            mv.ActorsLink.UnionWith(mx.Actors.Select(a => new MovieActor(mv, new Person(a.Name, a.Thumb), a.Role)));
            mv.Plots.Add(new Plot(mx.Plot, mx.Outline, mx.Tagline, null));

            return mv;
        }

        private static IEnumerable<ArtBase> GetArt(XbmcXmlMovie mx) {
            List<ArtBase> art = new List<ArtBase>();

            if (mx.Thumbs != null) {
                //add all Thumbnails/Posters/Covers
                foreach (XbmcXmlThumb thumb in mx.Thumbs) {
                    ArtBase a;

                    if (string.IsNullOrEmpty(thumb.Aspect)) {
                        a = new Art(thumb.Path, thumb.Preview);
                    }
                    else {
                        switch (thumb.Aspect.ToLower()) {
                            case "poster":
                                a = new Poster(thumb.Path, thumb.Preview);
                                break;
                            case "cover":
                                a = new Cover(thumb.Path, thumb.Preview);
                                break;
                            default:
                                a = new Art(thumb.Path, thumb.Preview);
                                break;
                        }
                    }

                    art.Add(a);
                }
            }

            if (mx.Fanart != null && mx.Fanart.Thumbs != null) {
                //add fanart
                art.AddRange(mx.Fanart.Thumbs.Select(thumb => new Fanart(thumb.Path, thumb.Preview)));
            }
            return art;
        }

        /// <summary>Deserializes an instance of <see cref="XbmcXmlMovie"/> from XML at the specified location and converts it to <see cref="Movie"/></summary>
        /// <param name="xmlLocation">The file path of the serialied xml.</param>
        /// <returns>An instance of <see cref="Movie"/> converted from deserialized instance of <see cref="XbmcXmlMovie"/> at the specified location</returns>
        public static Movie LoadFromXbmcNfo(string xmlLocation) {
            return (Movie) XbmcXmlMovie.Load(xmlLocation);
        }
        #endregion
    }
}
