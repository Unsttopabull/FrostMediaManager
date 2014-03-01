using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Util;
using Frost.Model.Xbmc.DB;
using Frost.Model.Xbmc.NFO;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;
using Frost.Models.Xtreamer.PHP;
using File = Frost.Models.Frost.DB.Files.File;

namespace Frost.Models.Frost {

    public static class Extensions {
        /// <summary>Adds the file info as an instance <see cref="File">File</see> to the collection if the file with specified filename it exists.</summary>
        /// <param name="files">The files collection to add to.</param>
        /// <param name="filename">The filename to check.</param>
        /// <returns>Returns <b>true</b> if the fille exist and there was no error retrieving its info; otherwise <b>false</b>.</returns>
        public static bool AddFile(this ICollection<File> files, string filename) {
            try {
                FileInfo fi = new FileInfo(filename);

                files.Add(new File(fi.Name, fi.Extension, fi.FullName, fi.Length));
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>Gets the movie's subtitles as a <see cref="IEnumerable{T}"/> with <see cref="ArtBase">Art</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="ArtBase">Art</see> elements.</returns>
        public static IEnumerable<ArtBase> GetArt(this XbmcXmlMovie xm) {
            List<ArtBase> art = new List<ArtBase>();

            if (xm.Thumbs != null) {
                //add all Thumbnails/Posters/Covers
                foreach (XbmcXmlThumb thumb in xm.Thumbs) {
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

            if (xm.Fanart != null && xm.Fanart.Thumbs != null) {
                //add fanart
                art.AddRange(xm.Fanart.Thumbs.Select(thumb => new Fanart(thumb.Path, thumb.Preview)));
            }

            return art;
        }

        /// <summary>Gets the movie's subtitles as an <see cref="IEnumerable{T}"/> with <see cref="Certification">Certification</see> elements.</summary>
        /// <returns>A <see cref="IEnumerable{T}"/> with <see cref="Certification">Certification</see> elements.</returns>
        public static IEnumerable<Certification> GetCertifications(this XbmcXmlMovie xm) {
            return xm.Certifications != null
                       ? xm.Certifications.Select(xmlCert => (Certification) xmlCert)
                       : Enumerable.Empty<Certification>();
        }

        /// <summary>Converts writer names into <see cref="Person"/> instances in a <see cref="HashSet{Person}"/>.</summary>
        /// <returns>A <see cref="HashSet{Person}"/> containing <see cref="Person"/> instances with names of the credited writers.</returns>
        public static ObservableHashSet<Person> GetWriters(this XbmcXmlMovie xm) {
            return new ObservableHashSet<Person>(from c in xm.Credits
                                                 where !string.IsNullOrEmpty(c)
                                                 select new Person(c));
        }

        /// <summary>Converts director names into <see cref="Person"/> instances in a <see cref="HashSet{Person}"/>.</summary>
        /// <returns>A <see cref="HashSet{Person}"/> containing <see cref="Person"/> instances with names of the credited directors.</returns>
        public static ObservableHashSet<Person> GetDirectors(this XbmcXmlMovie xm) {
            return new ObservableHashSet<Person>(from d in xm.Directors
                                                 where !string.IsNullOrEmpty(d)
                                                 select new Person(d));
        }

        /// <summary>Gets the files containing the movie as a <see cref="HashSet{T}"/> with <see cref="File">File</see>elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="File">File</see> elements.</returns>
        public static ObservableHashSet<File> GetFiles(this XbmcXmlMovie xm) {
            ObservableHashSet<File> files = new ObservableHashSet<File>();
            if (string.IsNullOrEmpty(xm.FilenameAndPath)) {
                return files;
            }

            string fn = xm.FilenameAndPath;

            //if file is stacked split into individual filenames
            if (fn.StartsWith(XbmcFile.STACK_PREFIX)) {
                //remove the "stack://" prefix
                fn = fn.Replace(XbmcFile.STACK_PREFIX, "");

                foreach (string fileName in fn.SplitWithoutEmptyEntries(XbmcFile.STACK_FILE_SEPARATOR)) {
                    files.AddFile(fileName.Trim().ToWinPath());
                }
            }
            else {
                //if not then just add the filename as is
                files.AddFile(fn.ToWinPath());
            }
            return files;
        }

        /// <summary>Gets the movie's subtitles as a <see cref="HashSet{T}"/> with <see cref="Subtitle">Subtitle</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Subtitle">Subtitle</see> elements.</returns>
        public static IEnumerable<Subtitle> GetSubtitles(this XbmcXmlMovie xm) {
            return xm.FileInfo.InfoExists(MediaType.Subtitles)
                       ? xm.FileInfo.Subtitles.Select(xmlSub => (Subtitle) xmlSub)
                       : new List<Subtitle>();
        }

        /// <summary>Gets the movie's video stream details as a <see cref="HashSet{T}"/> with <see cref="Video">Video</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Video">Video</see> elements.</returns>
        public static IEnumerable<Video> GetVideo(this XbmcXmlMovie xm) {
            return xm.FileInfo.InfoExists(MediaType.Video)
                       ? xm.FileInfo.Videos.Select(xmlVideo => (Video) xmlVideo)
                       : Enumerable.Empty<Video>();
        }

        /// <summary>Gets the movie's subtitles as a <see cref="HashSet{T}"/> with <see cref="Audio">Audio</see> elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="Audio">Audio</see> elements.</returns>
        public static IEnumerable<Audio> GetAudio(this XbmcXmlMovie xm) {
            return xm.FileInfo.InfoExists(MediaType.Audio)
                       ? xm.FileInfo.Audios.Select(xmlAudio => (Audio) xmlAudio)
                       : Enumerable.Empty<Audio>();
        }

        public static void GetInfo(this Coretis_VO_Movie phpMovie, Movie mov) {
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

        public static void AddNewCast(this Coretis_VO_Movie phpMovie, Movie mov) {
            foreach (Coretis_VO_Person cPerson in phpMovie.personArr) {
                if (cPerson.job.OrdinalEquals("actor")) {
                    Actor actor = new Actor(cPerson.name, null, cPerson.character);
                    mov.ActorsLink.Add(new MovieActor(mov, actor));
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

        public static void GetMovieTitle(this Coretis_VO_Movie phpMovie, Movie mov) {
            mov.Title = phpMovie.name;
            if (!string.IsNullOrEmpty(phpMovie.titleOrg)) {
                mov.OriginalTitle = phpMovie.titleOrg;
            }

            if (!string.IsNullOrEmpty(phpMovie.titleSort)) {
                mov.SortTitle = phpMovie.titleSort;
            }
        }

        public static void AddAudioVideoInfo(this Coretis_VO_Movie phpMovie, Movie mov) {
            mov.Audios.Add(new Audio(
                phpMovie.audioSource,
                phpMovie.audioType,
                phpMovie.acodec,
                phpMovie.achannels
                ));

            mov.Videos.Add(new Video(
                phpMovie.vcodec,
                phpMovie.width.HasValue ? phpMovie.width.Value : default(int),
                phpMovie.height.HasValue ? phpMovie.height.Value : default(int),
                phpMovie.aspect.HasValue ? phpMovie.aspect.Value : default(double),
                phpMovie.fps,
                phpMovie.videoFormat,
                phpMovie.videoType,
                phpMovie.videoSource
                ));
        }

        public static void AddArt(this Coretis_VO_Movie phpMovie, Movie mov) {
            if (!string.IsNullOrEmpty(phpMovie.pathCover)) {
                mov.Art.Add(new Cover(phpMovie.pathCover));
            }

            if (phpMovie.pathScreenArr != null) {
                //if the array is not null we add all art as Fanart
                mov.Art.UnionWith(phpMovie.pathScreenArr.Select(screen => new Fanart(screen)));
            }

            if (phpMovie.pathFanartArr != null) {
                mov.Art.UnionWith(phpMovie.pathFanartArr.Select(fanart => new Fanart(fanart)));
            }
        }
    }

}