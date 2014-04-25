using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.PHPtoNET;
using Frost.Providers.Xtreamer.DB;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer {

    internal enum PersonType {
        Actor,
        Writer,
        Director
    }

    public class XtMovieSaver : IDisposable {
        private static readonly Dictionary<string, XjbGenre> Genres;
        private static readonly Dictionary<string, XjbPerson> People;
        private static readonly Dictionary<string, string> DriveIds;

        private readonly string _xtPathRoot;
        private readonly MovieInfo _info;
        private readonly XjbEntities _mvc;
        private readonly bool _disposeContainer;
        private static readonly PHPSerializer PHPSerializer = new PHPSerializer();

        static XtMovieSaver() {
            Genres = new Dictionary<string, XjbGenre>(StringComparer.InvariantCultureIgnoreCase);
            People = new Dictionary<string, XjbPerson>(StringComparer.InvariantCultureIgnoreCase);
            DriveIds = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        public XtMovieSaver(string xtPathRoot, MovieInfo movie, XjbEntities db = null) {
            _xtPathRoot = xtPathRoot;
            _info = movie;

            //_mvc = new MovieVoContainer(true, "movieVo.db3");
            if (db != null) {
                _mvc = db;
                _disposeContainer = false;
            }
            else {
                _mvc = new XjbEntities();
            }
        }

        public XjbMovie Save() {
            if (!_info.DirectoryPath.StartsWith(_xtPathRoot)) {
                throw new NotSupportedException("The movie must be on the Xtreamer drive");
            }

            if (_info.IsMultipart) {
                Debug.WriteLine("Skipping multipart movie: "+ _info.Title);
                return null;
            }

            if (_info.FileInfos.Count(f => f.Videos.Count > 0) == 0 && _info.Type != MovieType.ISO) {
                Debug.WriteLine("Skipping movie without video: "+ _info.Title);
                return null;
            }

            XjbMovie xjbMovie = SaveDB(_info);
            if (xjbMovie != null) {
                _mvc.SaveChanges();

                string phpMovie = SavePHP(xjbMovie, _info);

                xjbMovie.MovieVo = phpMovie;

                return xjbMovie;
            }
            return null;
        }

        private string GetPathOnDrive(string fullPath) {
            string replace = fullPath.Replace(_xtPathRoot, "");

            int indexOfForward = replace.IndexOf("/", 1, StringComparison.Ordinal);
            if (indexOfForward > 0) {
                replace = replace.Remove(0, indexOfForward);
            }
            else {
                int indexOfBack = replace.IndexOf("\\", 1, StringComparison.Ordinal);
                replace = replace.Replace('\\', '/').Remove(0, indexOfBack);
            }
            return replace;
        }

        private string GetFolderOnDrive(string fullPath) {
            string dir = Path.GetDirectoryName(GetPathOnDrive(fullPath));

            return dir;
        }

        public string GetDriveId(string fullPath) {
            string replace = fullPath.Replace(_xtPathRoot, "");

            int indexOfForward = replace.IndexOf("/", 1, StringComparison.Ordinal);
            if (indexOfForward > 0) {
                replace = replace.Substring(0, indexOfForward);
            }
            else {
                int indexOfBack = replace.IndexOf("\\", 1, StringComparison.Ordinal);
                replace = replace.Replace('\\', '/').Substring(0, indexOfBack);
            }

            string id;
            if (DriveIds.TryGetValue(replace, out id)) {
                return id;
            }

            string driveIdFile = _xtPathRoot + replace+"\\.xjb-drive-id";
            if (File.Exists(driveIdFile)) {
                try {
                    id = File.ReadAllText(driveIdFile);
                    DriveIds.Add(replace, id);

                    XjbDrive xjbDrive = _mvc.Drives.Find(id);
                    if (xjbDrive != null) {
                        return id;
                    }

                    _mvc.Drives.Add(new XjbDrive { Id = id });
                    return id;
                }
                catch {
                    return null;
                }
            }
            return null;
        }

        private string SavePHP(XjbMovie xjbMovie, MovieInfo info) {
            XjbPhpMovie phpMovie = new XjbPhpMovie {
                Id = xjbMovie.Id,
                ImdbId = info.ImdbID,
                Title = info.Title,
                OriginalTitle = info.OriginalTitle,
                SortTitle = info.SortTitle,
                ReleaseYear = info.ReleaseYear.ToInvariantString(),
                RatingAverage = info.RatingAverage.HasValue
                                    ? (int) Math.Round(info.RatingAverage.Value)
                                    : 0,
                Countries = info.Countries.Select(c => c.Alpha2).ToList(),
                Studio = info.Studios.FirstOrDefault(),
                Specials = string.Join(",", info.Specials)
            };

            PlotInfo plot = info.Plots.FirstOrDefault();

            if (plot != null) {
                phpMovie.PlotFull = plot.Full;
                phpMovie.PlotSummary = plot.Summary;
            }

            AudioDetectionInfo[] audios = info.FileInfos.SelectMany(fd => fd.Audios).ToArray();
            VideoDetectionInfo[] videos = info.FileInfos.SelectMany(fd => fd.Videos).ToArray();

            FileDetectionInfo file = info.FileInfos.FirstOrDefault(fi => fi.Videos.Count > 0);
            if (file != null) {
                phpMovie.FileName = file.NameWithExtension;
                phpMovie.FileExtension = file.Extension;
                phpMovie.FilePathOnDrive = GetPathOnDrive(file.FullPath);
                phpMovie.FileSize = file.Size.HasValue ? file.Size.Value : 0;
                phpMovie.LastAccessTime = file.LastAccessTime.ToUnixTimestamp().ToInvariantString();
                phpMovie.FileCreateTime = file.CreateTime.ToUnixTimestamp().ToInvariantString();
            }

            string[] videoLangs = audios.Where(a => a.Language != null)
                                        .Select(a => a.Language.EnglishName)
                                        .ToArray();

            string[] audioLangs = videos.Where(a => a.Language != null)
                                        .Select(a => a.Language.EnglishName)
                                        .ToArray();

            string[] subtitleLangs = info.FileInfos
                                         .SelectMany(fd => fd.Subtitles)
                                         .Where(s => s.Language != null)
                                         .Select(s => s.Language.EnglishName)
                                         .ToArray();

            phpMovie.AvailableLanguage = videoLangs.Union(audioLangs)
                                                   .Union(subtitleLangs)
                                                   .ToList();

            long unixTimeNow = (long) DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            phpMovie.ScraperLastRunTimestamp = new Dictionary<string, long>();
            foreach (string scrapperName in XjbPhpMovie.ScrapperNames) {
                phpMovie.ScraperLastRunTimestamp.Add(scrapperName, unixTimeNow);
            }

            if (subtitleLangs.Length > 0) {
                phpMovie.SubtitleLanguage = subtitleLangs[0];
            }

            GetMostCommonAudio(phpMovie, audios);
            GetMostCommonVideo(phpMovie, videos);
            GetArt(phpMovie, info.Art);

            phpMovie.Subtitles = info.FileInfos
                                     .Where(fd => fd.Subtitles.Count > 0)
                                     .Select(info1 => GetPathOnDrive(info1.FullPath))
                                     .Where(pathOnDrive => pathOnDrive != null)
                                     .ToList();

            phpMovie.Genres = xjbMovie.Genres.Select(g => new XjbPhpGenre(g)).ToList();

            phpMovie.Cast = xjbMovie.Cast.Select(person => {
                if (person is XjbActor) {
                    return new XjbPhpPerson(person as XjbActor);
                }
                if (person is XjbDirector) {
                    return new XjbPhpPerson(person as XjbDirector);
                }
                if (person is XjbWriter) {
                    return new XjbPhpPerson(person as XjbWriter);
                }
                return new XjbPhpPerson(person);
            }).ToList();

            return PHPSerializer.Serialize(phpMovie);
        }

        private void GetArt(XjbPhpMovie phpMovie, List<ArtInfo> art) {
            if (art.Count == 0) {
                return;
            }

            ArtInfo cover = art.FirstOrDefault(a => a.Type == ArtType.Cover);
            if (cover != null) {
                phpMovie.CoverPath = GetPathOnDrive(cover.Path);
            }
            else {
                ArtInfo poster = art.FirstOrDefault(a => a.Type == ArtType.Poster);
                if (poster != null) {
                    phpMovie.CoverPath = GetPathOnDrive(poster.Path);
                }
            }

            phpMovie.Fanart = new List<string>();

            foreach (ArtInfo artInfo in art.Where(a => a.Type == ArtType.Fanart)) {
                string pathOnDrive = GetPathOnDrive(artInfo.Path);
                if (pathOnDrive != null) {
                    phpMovie.Fanart.Add(pathOnDrive);
                }
            }
        }

        private static void GetMostCommonAudio(XjbPhpMovie phpMovie, IEnumerable<AudioDetectionInfo> audios) {
            Dictionary<string, int> sources = new Dictionary<string, int>();
            Dictionary<string, int> types = new Dictionary<string, int>();
            Dictionary<string, int> codecs = new Dictionary<string, int>();
            Dictionary<int, int> channels = new Dictionary<int, int>();

            int maxSource = 0;
            int maxType = 0;
            int maxCodec = 0;
            int maxChannel = 0;

            string source = null;
            string type = null;
            string codec = null;
            string channelsCommon = null;

            foreach (AudioDetectionInfo audio in audios) {
                maxSource = CheckOccurence(audio.Source, sources, maxSource, ref source);
                maxType = CheckOccurence(audio.Type, types, maxType, ref type);
                maxCodec = CheckOccurence(audio.CodecId, codecs, maxCodec, ref codec);

                int? numChan = audio.NumberOfChannels;
                if (numChan.HasValue) {
                    if (channels.ContainsKey(numChan.Value)) {
                        if (++channels[numChan.Value] <= maxChannel) {
                            continue;
                        }

                        maxChannel = channels[numChan.Value];
                        channelsCommon = numChan.Value.ToString(CultureInfo.InvariantCulture);
                    }
                    else {
                        channels.Add(numChan.Value, 1);

                        if (maxChannel == 0) {
                            channelsCommon = numChan.Value.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            phpMovie.AudioType = type;
            phpMovie.AudioSource = source;
            phpMovie.AudioChannels = channelsCommon;
            phpMovie.AudioCodec = codec;
        }

        private static int CheckOccurence(string currValue, IDictionary<string, int> countingDict, int maxOccurence, ref string maxValue) {
            if (string.IsNullOrEmpty(currValue)) {
                return maxOccurence;
            }

            if (countingDict.ContainsKey(currValue)) {
                if (++countingDict[currValue] <= maxOccurence) {
                    return maxOccurence;
                }

                maxOccurence = countingDict[currValue];
                maxValue = currValue;
            }
            else {
                countingDict.Add(currValue, 1);

                if (maxOccurence == 0) {
                    maxValue = currValue;
                }
            }
            return maxOccurence;
        }

        private static void GetMostCommonVideo(XjbPhpMovie phpMovie, VideoDetectionInfo[] videos) {
            phpMovie.VideoResolution = GetMostCommonString(videos, v => v.ResolutionName);
            phpMovie.VideoSource = GetMostCommonString(videos, v => v.Source);
            phpMovie.VideoType = GetMostCommonString(videos, v => v.Type);
            phpMovie.Aspect = GetMostCommon(videos, v => v.Aspect);
            phpMovie.FPS = (int?) GetMostCommon(videos, v => v.FPS);
            phpMovie.Height = GetMostCommon(videos, v => v.Height);
            phpMovie.Height = GetMostCommon(videos, v => v.Height);
            phpMovie.Width = GetMostCommon(videos, v => v.Width);
            phpMovie.Width = GetMostCommon(videos, v => v.Width);

            long? duration = GetMostCommon(videos, v => v.Duration);

            phpMovie.Runtime = duration.HasValue ? duration.Value / 1000.0 : (double?) null;
            phpMovie.VideoCodec = GetMostCommonString(videos, v => v.CodecId);
        }

        private static string GetMostCommonString<T>(IEnumerable<T> audios, Func<T, string> propSelector) where T : class {
            IGrouping<string, T> group = audios.Where(a => !string.IsNullOrEmpty(propSelector(a)))
                                               .GroupBy(propSelector)
                                               .OrderByDescending(g => g.Count())
                                               .FirstOrDefault();

            if (group == null) {
                return null;
            }

            T audio = group.FirstOrDefault();
            return audio != null
                       ? propSelector(audio)
                       : null;
        }

        private static TReturn GetMostCommon<T, TReturn>(IEnumerable<T> audios, Func<T, TReturn> propSelector) where T : class {
            IGrouping<TReturn, T> group = audios.GroupBy(propSelector)
                                                .OrderByDescending(g => g.Count())
                                                .FirstOrDefault();

            if (group == null) {
                return default(TReturn);
            }

            T audio = group.FirstOrDefault();
            return audio != null
                       ? propSelector(audio)
                       : default(TReturn);
        }

        private XjbMovie SaveDB(MovieInfo movie) {
            XjbMovie mv = FromMovieInfo(movie);

            if (mv == null) {
                return null;
            }

            if (!string.IsNullOrEmpty(mv.ImdbID)) {
                if (_mvc.Movies.Any(m => m.ImdbID == mv.ImdbID)) {
                    return null;
                }
            }

            if (_mvc.Movies.Any(m => m.Title == mv.Title || !string.IsNullOrEmpty(mv.OriginalTitle) && m.OriginalTitle == mv.OriginalTitle)) {
                return null;
            }

            mv = _mvc.Movies.Add(mv);

            AddPeople(mv, movie.Actors, PersonType.Actor);
            AddPeople(mv, movie.Writers, PersonType.Writer);
            AddPeople(mv, movie.Directors, PersonType.Director);

            mv.Genres = new HashSet<XjbGenre>(movie.Genres.ConvertAll(g => GetGenre(g, Genres)));

            return mv;
        }

        private void AddPeople(XjbMovie mv, IEnumerable<PersonInfo> people, PersonType personType) {
            foreach (PersonInfo personInfo in people.Where(p => !string.IsNullOrEmpty(p.Name))) {
                XjbPerson p = GetPerson(personInfo);

                XjbMoviePerson mp = null;
                switch (personType) {
                    case PersonType.Actor:
                        ActorInfo actor = personInfo as ActorInfo;
                        if (actor != null) {
                            mp = new XjbActor(p, actor.Character);
                        }
                        break;
                    case PersonType.Writer:
                        mp = new XjbWriter(p);
                        break;
                    case PersonType.Director:
                        mp = new XjbDirector(p);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("personType");
                }

                if (mp != null) {
                    mv.Cast.Add(mp);
                }
            }
        }

        private XjbPerson GetPerson(PersonInfo info) {
            XjbPerson p;
            if (People.TryGetValue(info.Name, out p)) {
                return p;
            }

            p = _mvc.People.FirstOrDefault(person => person.Name == info.Name);
            if (p != null) {
                People.Add(p.Name, p);
                return p;
            }

            p = new XjbPerson(info.Name);
            People.Add(p.Name, p);
            return p;
        }

        private XjbGenre GetGenre(string name, Dictionary<string, XjbGenre> genres) {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }

            XjbGenre genre;
            if (genres.TryGetValue(name, out genre)) {
                return genre;
            }

            genre = _mvc.Genres.FirstOrDefault(s => s.Name == name);
            if (genre != null) {
                genres.Add(genre.Name, genre);
                return genre;
            }

            genre = new XjbGenre(name);
            genres.Add(genre.Name, genre);

            return genre;
        }

        private XjbMovie FromMovieInfo(MovieInfo movie) {
            XjbMovie mv = new XjbMovie {
                Revision = 1,
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                SortTitle = movie.SortTitle,
                Runtime = movie.Runtime / 1000,
                ImdbID = movie.ImdbID,
                Year = movie.ReleaseYear,
                Rating = movie.RatingAverage.HasValue ? (int) Math.Round(movie.RatingAverage.Value * 10) : 0
            };

            switch (movie.Type) {
                case MovieType.DVD: {
                    FileDetectionInfo fdi = movie.FileInfos.FirstOrDefault(fi => string.Equals(fi.NameWithExtension, "VIDEO_TS.IFO", StringComparison.InvariantCultureIgnoreCase));
                    mv.Filename = "VIDEO_TS.IFO";

                    if (fdi != null) {
                        mv.FolderPathOnDrive = GetFolderOnDrive(fdi.FullPath);
                        mv.Filesize = fdi.Size;
                        mv.DriveId = GetDriveId(fdi.FullPath);
                    }
                    break;
                }
                case MovieType.ISO: {
                    FileDetectionInfo fi = movie.FileInfos.FirstOrDefault(fd => string.Equals("iso", fd.Extension, StringComparison.InvariantCultureIgnoreCase));

                    if (fi != null) {
                        mv.Filename = fi.NameWithExtension;
                        mv.FolderPathOnDrive = GetFolderOnDrive(fi.FullPath);
                        mv.Filesize = fi.Size;
                        mv.DriveId = GetDriveId(fi.FullPath);
                    }
                    break;
                }
                default: {
                    FileDetectionInfo fdi = movie.FileInfos.FirstOrDefault(fi => fi.Videos.Count > 0);
                    if (fdi == null) {
                        return null;
                    }

                    if (string.IsNullOrEmpty(fdi.NameWithExtension)) {
                        return null;
                    }

                    mv.Filename = fdi.NameWithExtension;
                    mv.FolderPathOnDrive = GetFolderOnDrive(fdi.FullPath);
                    mv.Filesize = fdi.Size;
                    mv.DriveId = GetDriveId(fdi.FullPath);
                    break;
                }
            }

            PlotInfo plot = movie.Plots.FirstOrDefault();
            if (plot != null) {
                mv.Plot = plot.Full;
            }

            mv.HasFanart = movie.Art.Any(a => a.Type == ArtType.Fanart);
            mv.HasCover = movie.Art.Any(a => a.Type == ArtType.Cover || a.Type == ArtType.Poster);

            mv.FilePathFull = null;

            return mv;
        }

        public static void Reset() {
            if (Genres != null) {
                Genres.Clear();
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

        ~XtMovieSaver() {
            Dispose(true);
        }

        #endregion
    }

}