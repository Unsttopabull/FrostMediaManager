using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Comparers;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Art;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.Proxies;

namespace Frost.Providers.Xbmc {

    public class XbmcMovieSaver : IDisposable {
        private static readonly Dictionary<ISOCountryCode, XbmcCountry> Countries;
        private static readonly Dictionary<string, XbmcSet> Sets;
        private static readonly Dictionary<string, XbmcGenre> Genres;
        private static readonly Dictionary<string, XbmcStudio> Studios;
        private static readonly Dictionary<string, XbmcPerson> People;

        private readonly MovieInfo _info;
        private readonly XbmcContainer _mvc;
        private readonly bool _disposeContainer;

        static XbmcMovieSaver() {
            Countries = new Dictionary<ISOCountryCode, XbmcCountry>();
            Sets = new Dictionary<string, XbmcSet>(StringComparer.InvariantCultureIgnoreCase);
            Genres = new Dictionary<string, XbmcGenre>(StringComparer.InvariantCultureIgnoreCase);
            Studios = new Dictionary<string, XbmcStudio>(StringComparer.InvariantCultureIgnoreCase);
            People = new Dictionary<string, XbmcPerson>(StringComparer.InvariantCultureIgnoreCase);
        }

        public XbmcMovieSaver(MovieInfo movie, XbmcContainer db = null) {
            _info = movie;

            if (db != null) {
                _mvc = db;
                _disposeContainer = false;
            }
            else {
                _mvc = new XbmcContainer();
            }
        }

        public XbmcDbMovie Save(bool saveChanges) {
            XbmcDbMovie xbmcDbMovie = Save(_info);

            if (saveChanges) {
                _mvc.SaveChanges();
            }

            return xbmcDbMovie;
        }

        private XbmcDbMovie Save(MovieInfo movie) {
            XbmcDbMovie mv = FromMovieInfo(movie);

            if (!string.IsNullOrEmpty(mv.ImdbID)) {
                if (_mvc.Movies.Any(m => m.ImdbID == mv.ImdbID)) {
                    return null;
                }
            }

            if (_mvc.Movies.Any(m => m.Title == mv.Title || !string.IsNullOrEmpty(mv.OriginalTitle) && m.OriginalTitle == mv.OriginalTitle)) {
                return null;
            }

            mv = _mvc.Movies.Add(mv);

            mv.Set = GetHasName(movie.Set, Sets);

            mv.Art = new HashSet<XbmcArt>(movie.Art.ConvertAll(art => new XbmcArt(ArtTarget.Movie, art.Type, art.Path)));
            mv.Posters = mv.Art.Where(a => a.Type == XbmcArt.POSTER).Select(a => a.Url);
            mv.Fanart = mv.Art.Where(a => a.Type == XbmcArt.FANART).Select(a => a.Url);

            mv.Genres = new HashSet<XbmcGenre>(movie.Genres.ConvertAll(g => GetHasName(g, Genres)));
            mv.GenreString = string.Join(XbmcDbMovie.SEPARATOR, mv.Genres);

            mv.Studios = new HashSet<XbmcStudio>(movie.Studios.ConvertAll(s => GetHasName(s, Studios)));
            mv.StudioNames = string.Join(XbmcDbMovie.SEPARATOR, mv.Studios);

            mv.Countries = new HashSet<XbmcCountry>(movie.Countries.ConvertAll(GetCountry));
            mv.CountryString = string.Join(XbmcDbMovie.SEPARATOR, mv.Countries);

            mv.Writers = new HashSet<XbmcPerson>(movie.Writers.ConvertAll(GetPerson));
            mv.WriterNames = string.Join(XbmcDbMovie.SEPARATOR, mv.Writers);

            mv.Directors = new HashSet<XbmcPerson>(movie.Directors.ConvertAll(GetPerson));
            mv.DirectorsString = string.Join(XbmcDbMovie.SEPARATOR, mv.Directors);

            GetActors(movie, mv);

            string dirPath = movie.DirectoryPath;
            if (!dirPath.EndsWith("/") || !dirPath.EndsWith("\\")) {
                int idxForward = dirPath.IndexOf('\\');

                if (idxForward > 0) {
                    dirPath += "\\";
                }
                else {
                    dirPath += "/";
                }
            }
			
            XbmcPath path = FindPath(dirPath);
            XbmcFile file = new XbmcFile(DateTime.Now.ToString("yyyy-mm-dd hh:ii:ss"), movie.LastPlayed.ToInvariantString(), movie.PlayCount) {
                Path = path
            };

            string[] fnS = movie.FileInfos.Where(fi => !string.IsNullOrEmpty(fi.FullPath) && fi.Videos.Count > 0).Select(fi => fi.FullPath).ToArray();

            if (fnS.Length == 1) {
                FileDetectionInfo info = movie.FileInfos[0];
                if (info != null) {
                    file.FileNameString = info.NameWithExtension;
                }
            }
            else if (fnS.Length > 0) {
                if (movie.Type == MovieType.DVD) {
                    file.FileNameString = "VIDEO_TS.IFO";
                }
                else {
                    file.FileNameString = XbmcFile.GetFileNamesString(fnS, false);
                }
            }
            else if(movie.FileInfos.Count > 0){
                FileDetectionInfo info = movie.FileInfos[0];
                if (info != null) {
                    file.FileNameString = info.NameWithExtension;
                }
            }

            mv.Path = path;

            file = _mvc.Files.Add(file);

            foreach (FileDetectionInfo fileInfo in movie.FileInfos) {
                AddVideos(fileInfo, file);
                AddAudios(fileInfo, file);
                AddSubtitles(fileInfo, file);
            }

            mv.File = file;

            return mv;
        }

        private void GetActors(MovieInfo movie, XbmcDbMovie mv) {
            int count = 0;

            foreach (ActorInfo actor in movie.Actors) {
                XbmcPerson xbmcPerson = GetPerson(actor);

                if (mv.Actors.Any(a => a.Person.Name == xbmcPerson.Name)) {
                    continue;
                }

                mv.Actors.Add(new XbmcMovieActor(xbmcPerson, actor.Character, count++));
            }
        }

        private XbmcPath FindPath(string directoryPath) {
            string alternate = directoryPath.Replace('\\', '/').Replace('/', '\\');
            XbmcPath path = _mvc.Paths.FirstOrDefault(p => p.FolderPath == directoryPath || p.FolderPath == alternate);

            return path ?? _mvc.Paths.Add(new XbmcPath(directoryPath));
        }

        private void AddSubtitles(FileDetectionInfo fileInfo, XbmcFile file) {
            foreach (SubtitleDetectionInfo s in fileInfo.Subtitles) {
                file.StreamDetails.Add(new XbmcDbStreamDetails(StreamType.Subtitle) {
                    SubtitleLanguage = s.Language != null ? s.Language.Alpha3 : null
                });
            }
        }

        private static void AddAudios(FileDetectionInfo fileInfo, XbmcFile file) {
            foreach (AudioDetectionInfo a in fileInfo.Audios) {
                file.StreamDetails.Add(new XbmcDbStreamDetails(StreamType.Audio) {
                    AudioCodec = a.CodecId,
                    AudioChannels = a.NumberOfChannels,
                    AudioLanguage = a.Language != null ? a.Language.Alpha3 : null
                });
            }
        }

        private static void AddVideos(FileDetectionInfo fileInfo, XbmcFile file) {
            foreach (VideoDetectionInfo v in fileInfo.Videos) {
                file.StreamDetails.Add(new XbmcDbStreamDetails(StreamType.Video) {
                    VideoCodec = v.CodecId,
                    Aspect = v.Aspect,
                    VideoWidth = v.Width,
                    VideoHeight = v.Height,
                    VideoDuration = v.Duration.HasValue
                                        ? (long?) Math.Round(v.Duration.Value / 1000.0)
                                        : null
                });
            }
        }

        private XbmcPerson GetPerson(PersonInfo info) {
            ////Debug.WriteLine("Looking for person: " + info.Name);
            //Debug.Indent();

            XbmcPerson p;
            if (People.TryGetValue(info.Name, out p)) {
                ////Debug.WriteLine("Found in dict");
                //Debug.Unindent();
                return p;
            }

            p = _mvc.People.FirstOrDefault(person => person.Name == info.Name);
            if (p != null) {
                People.Add(p.Name, p);

                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();
                return p;
            }

            p = new XbmcPerson(info.Name, info.Thumb);
            People.Add(p.Name, p);

            //Debug.WriteLine("Created new person");
            //Debug.Unindent();

            return p;
        }

        private XbmcCountry GetCountry(ISOCountryCode countryCode) {
            //Debug.WriteLine("Looking for country: " + countryCode.EnglishName);
            //Debug.Indent();

            XbmcCountry lang;
            if (Countries.TryGetValue(countryCode, out lang)) {
                //Debug.WriteLine("Found in dict");
                //Debug.Unindent();

                return lang;
            }

            lang = _mvc.Countries.FirstOrDefault(l => l.Name == countryCode.Alpha3);
            if (lang != null) {
                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();

                Countries.Add(countryCode, lang);
                return lang;
            }

            //Debug.WriteLine("Created new country");
            //Debug.Unindent();

            lang = new XbmcCountry(countryCode);
            Countries.Add(countryCode, lang);

            return lang;
        }

        private T GetHasName<T>(string name, IDictionary<string, T> dictToSearch) where T : class, IHasName, new() {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }

            //Debug.WriteLine("Looking for: " + typeof(T).Name);
            //Debug.Indent();

            T hasName;
            if (dictToSearch.TryGetValue(name, out hasName)) {
                //Debug.WriteLine("Found in dict");
                //Debug.Unindent();

                return hasName;
            }

            hasName = _mvc.Set<T>().FirstOrDefault<T>(s => s.Name == name);
            if (hasName != null) {
                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();

                dictToSearch.Add(hasName.Name, hasName);
                return hasName;
            }

            //Debug.WriteLine("Created new one");
            //Debug.Unindent();

            hasName = new T { Name = name };
            dictToSearch.Add(hasName.Name, hasName);

            return hasName;
        }

        private XbmcDbMovie FromMovieInfo(MovieInfo movie) {
            XbmcDbMovie mv = new XbmcDbMovie {
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                SortTitle = movie.SortTitle,
                ReleaseYear = movie.ReleaseYear.ToInvariantString(),
                ImdbTop250 = movie.Top250.ToInvariantString() ?? "0",
                Runtime = movie.Runtime.HasValue ? ((long) Math.Round((double) (movie.Runtime / 1000.0))).ToInvariantString() : null,
                Rating = movie.RatingAverage.ToInvariantString(),
                ImdbID = movie.ImdbID,
                MpaaRating = movie.MPAARating
            };


            string[] fnS = movie.FileInfos.Where(fi => !string.IsNullOrEmpty(fi.FullPath) && fi.Videos.Count > 0).Select(fi => fi.FullPath).ToArray();
            if (fnS.Length == 1) {
                FileDetectionInfo info = movie.FileInfos[0];
                if (info != null) {
                    mv.FilePathsString = info.FullPath;
                }
            }
            else if (fnS.Length > 0) {
                if (movie.Type == MovieType.DVD) {
                    mv.FilePathsString = movie.DirectoryPath.Replace("VIDEO_TS", "");
                }
                else {
                    mv.FilePathsString = XbmcFile.GetFileNamesString(fnS);
                }
            }
            else if (movie.FileInfos.Count > 0) {
                FileDetectionInfo info = movie.FileInfos[0];
                if (info != null) {
                    mv.FilePathsString = info.FullPath;
                }                
            }

            XbmcMovie.SetTrailer(mv, movie.Trailer);

            PlotInfo plot = movie.Plots.FirstOrDefault();
            if (plot != null) {
                mv.Plot = plot.Full;
                mv.PlotOutline = plot.Summary;
                mv.Tagline = plot.Tagline;
            }

            return mv;
        }

        public static void Reset() {
            if (Countries != null) {
                Countries.Clear();
            }

            if (Sets != null) {
                Sets.Clear();
            }

            if (Genres != null) {
                Genres.Clear();
            }

            if (Studios != null) {
                Studios.Clear();
            }

            if (People != null) {
                People.Clear();
            }
        }

        #region IDisposable

        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(false);
        }

        private void Dispose(bool finializer) {
            if (IsDisposed) {
                return;
            }

            if (_mvc != null && _disposeContainer) {
                _mvc.Dispose();
            }

            if (!finializer) {
                GC.SuppressFinalize(this);
            }
            IsDisposed = true;
        }

        ~XbmcMovieSaver() {
            Dispose(true);
        }

        #endregion
    }

}