using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.Providers.Frost.DB;

namespace Frost.Providers.Frost {

    public class MovieSaver : IDisposable {
        private static readonly Dictionary<ISOCountryCode, Country> Countries;
        private static readonly Dictionary<ISOLanguageCode, Language> Languages;
        private static readonly Dictionary<string, Set> Sets;
        private static readonly Dictionary<string, Genre> Genres;
        private static readonly Dictionary<string, Special> Specials;
        private static readonly Dictionary<string, Studio> Studios;
        private static readonly Dictionary<string, Person> People;
        private static readonly Dictionary<string, PromotionalVideo> PromotionalVideos;

        private readonly MovieInfo _info;
        private readonly FrostDbContainer _mvc;
        private readonly bool _disposeContainer;

        static MovieSaver() {
            Countries = new Dictionary<ISOCountryCode, Country>();
            Languages = new Dictionary<ISOLanguageCode, Language>();
            Sets = new Dictionary<string, Set>(StringComparer.InvariantCultureIgnoreCase);
            Genres = new Dictionary<string, Genre>(StringComparer.InvariantCultureIgnoreCase);
            Specials = new Dictionary<string, Special>(StringComparer.InvariantCultureIgnoreCase);
            Studios = new Dictionary<string, Studio>(StringComparer.InvariantCultureIgnoreCase);
            People = new Dictionary<string, Person>(StringComparer.InvariantCultureIgnoreCase);
            PromotionalVideos = new Dictionary<string, PromotionalVideo>(StringComparer.InvariantCultureIgnoreCase);
        }

        public MovieSaver(MovieInfo movie, FrostDbContainer db = null) {
            _info = movie;

            //_mvc = new MovieVoContainer(true, "movieVo.db3");
            if (db != null) {
                _mvc = db;
                _disposeContainer = false;
            }
            else {
                _mvc = new FrostDbContainer(true);
            }
        }

        public Movie Save(bool saveChanges) {
            Movie mv = Save(_info);

            if (mv == null) {
                return null;
            }

            if (saveChanges) {
                _mvc.SaveChanges();
            }
            return mv;
        }

        private Movie Save(MovieInfo movie) {
            Movie mv = FromMovieInfo(movie);

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
            mv.Plots = new HashSet<Plot>(movie.Plots.ConvertAll(GetPlot));
            mv.Art = new HashSet<Art>(movie.Art.ConvertAll(art => new Art(art.Type, art.Path, art.Preview)));

            mv.Certifications = new HashSet<Certification>(movie.Certifications.ConvertAll(GetCertification));
            mv.Specials = new HashSet<Special>(movie.Specials.ConvertAll(GetSpecial));
            mv.Genres = new HashSet<Genre>(movie.Genres.ConvertAll(g => GetHasName(g, Genres)));
            mv.Studios = new HashSet<Studio>(movie.Studios.ConvertAll(s => GetHasName(s, Studios)));
            mv.Countries = new HashSet<Country>(movie.Countries.ConvertAll(GetCountry));
            mv.Writers = new HashSet<Person>(movie.Writers.ConvertAll(GetPerson));
            mv.Directors = new HashSet<Person>(movie.Directors.ConvertAll(GetPerson));
            mv.Actors = new HashSet<Actor>(movie.Actors.ConvertAll(actorInfo => new Actor(GetPerson(actorInfo), actorInfo.Character)));
            mv.PromotionalVideos = new HashSet<PromotionalVideo>(movie.PromotionalVideos.ConvertAll(GetPromotionalVideo));

            mv.Videos = new HashSet<Video>();
            mv.Audios = new HashSet<Audio>();
            mv.Subtitles = new HashSet<Subtitle>();
            foreach (FileDetectionInfo fileInfo in movie.FileInfos) {
                //Debug.Indent();
                File file = new File(fileInfo.Name, fileInfo.Extension, fileInfo.FolderPath, fileInfo.Size);
                //Debug.Unindent();

                AddVideos(fileInfo, mv, file);
                AddAudios(fileInfo, mv, file);
                AddSubtitles(fileInfo, mv, file);
            }

            return mv;
        }

        private PromotionalVideo GetPromotionalVideo(PromotionalVideoInfo v) {
            PromotionalVideo promoVideo;
            if (PromotionalVideos.TryGetValue(v.Url, out promoVideo)) {
                return promoVideo;
            }
            return new PromotionalVideo(v);
        }

        private void AddSubtitles(FileDetectionInfo fileInfo, Movie mv, File file) {
            foreach (SubtitleDetectionInfo s in fileInfo.Subtitles) {
                mv.Subtitles.Add(new Subtitle(file) {
                    Language = GetLanguage(s.Language),
                    Format = s.Format,
                    EmbededInVideo = s.EmbededInVideo,
                    ForHearingImpaired = s.ForHearingImpaired,
                    Encoding = s.Encoding
                });
            }
        }

        private static void AddAudios(FileDetectionInfo fileInfo, Movie mv, File file) {
            foreach (AudioDetectionInfo a in fileInfo.Audios) {
                mv.Audios.Add(new Audio(file) {
                    Source = a.Source,
                    Type = a.Type,
                    ChannelSetup = a.ChannelSetup,
                    NumberOfChannels = a.NumberOfChannels,
                    ChannelPositions = a.ChannelPositions,
                    Codec = a.Codec,
                    CodecId = a.CodecId,
                    BitRate = a.BitRate,
                    BitRateMode = a.BitRateMode,
                    SamplingRate = a.SamplingRate,
                    BitDepth = a.BitDepth,
                    CompressionMode = a.CompressionMode,
                    Duration = a.Duration
                });
            }
        }

        private static void AddVideos(FileDetectionInfo fileInfo, Movie mv, File file) {
            foreach (VideoDetectionInfo v in fileInfo.Videos) {
                mv.Videos.Add(new Video(file) {
                    MovieHash = v.MovieHash,
                    Source = v.Source,
                    Type = v.Type,
                    Resolution = v.Resolution,
                    ResolutionName = v.ResolutionName,
                    Standard = v.Standard,
                    FPS = v.FPS,
                    BitRate = v.BitRate,
                    BitRateMode = v.BitRateMode,
                    BitDepth = v.BitDepth,
                    CompressionMode = v.CompressionMode,
                    Duration = v.Duration,
                    ScanType = v.ScanType,
                    ColorSpace = v.ColorSpace,
                    ChromaSubsampling = v.ChromaSubsampling,
                    Format = v.Format,
                    Codec = v.Codec,
                    CodecId = v.CodecId,
                    Aspect = v.Aspect,
                    AspectCommercialName = v.AspectCommercialName,
                    Width = v.Width,
                    Height = v.Height
                });
            }
        }

        public Plot GetPlot(PlotInfo p) {
            return new Plot(p.Full, p.Summary, p.Tagline, p.Language != null ? p.Language.EnglishName : null);
        }

        private Person GetPerson(PersonInfo info) {
            ////Debug.WriteLine("Looking for person: " + info.Name);
            //Debug.Indent();

            Person p;
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

            p = new Person(info.Name, info.Thumb, info.ImdbID);
            People.Add(p.Name, p);

            //Debug.WriteLine("Created new person");
            //Debug.Unindent();

            return p;
        }

        private Language GetLanguage(ISOLanguageCode langCode) {
            if (langCode == null) {
                return null;
            }

            //Debug.WriteLine("Looking for language: " + langCode.EnglishName);
            //Debug.Indent();

            Language lang;
            if (Languages.TryGetValue(langCode, out lang)) {
                //Debug.WriteLine("Found in dict");
                //Debug.Unindent();

                return lang;
            }

            lang = _mvc.Languages.FirstOrDefault(l => l.ISO639.Alpha3 == langCode.Alpha3);
            if (lang != null) {
                Languages.Add(langCode, lang);

                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();
                return lang;
            }

            lang = new Language(langCode);
            Languages.Add(langCode, lang);

            //Debug.WriteLine("Created new Language");
            //Debug.Unindent();

            return lang;
        }

        private Country GetCountry(ISOCountryCode countryCode) {
            //Debug.WriteLine("Looking for country: " + countryCode.EnglishName);
            //Debug.Indent();

            Country lang;
            if (Countries.TryGetValue(countryCode, out lang)) {
                //Debug.WriteLine("Found in dict");
                //Debug.Unindent();

                return lang;
            }

            lang = _mvc.Countries.FirstOrDefault(l => l.ISO3166.Alpha3 == countryCode.Alpha3);
            if (lang != null) {
                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();

                Countries.Add(countryCode, lang);
                return lang;
            }

            //Debug.WriteLine("Created new country");
            //Debug.Unindent();

            lang = new Country(countryCode);
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

        private Special GetSpecial(string special) {
            //Debug.WriteLine("Looking for special: " + special);
            //Debug.Indent();

            Special spec;
            if (Specials.TryGetValue(special, out spec)) {
                //Debug.WriteLine("Found in dict");
                //Debug.Unindent();

                return spec;
            }

            spec = _mvc.Specials.FirstOrDefault(s => s.Value == special);
            if (spec != null) {
                //Debug.WriteLine("Found in DB");
                //Debug.Unindent();

                Specials.Add(spec.Value, spec);
                return spec;
            }

            //Debug.WriteLine("Created new special");
            //Debug.Unindent();

            spec = new Special(special);
            Specials.Add(spec.Value, spec);

            return spec;
        }

        private Certification GetCertification(CertificationInfo certification) {
            return new Certification {
                Rating = certification.Rating,
                Country = GetCountry(certification.Country)
            };
        }

        private Movie FromMovieInfo(MovieInfo movie) {
            Movie mv = new Movie {
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                SortTitle = movie.SortTitle,
                Type = movie.Type,
                Goofs = movie.Goofs,
                Trivia = movie.Trivia,
                ReleaseYear = movie.ReleaseYear,
                ReleaseDate = movie.ReleaseDate,
                Edithion = movie.Edithion,
                DvdRegion = movie.DvdRegion,
                LastPlayed = movie.LastPlayed,
                Premiered = movie.Premiered,
                Aired = movie.Aired,
                Trailer = movie.Trailer,
                Top250 = movie.Top250,
                Runtime = movie.Runtime,
                Watched = movie.Watched,
                PlayCount = movie.PlayCount,
                RatingAverage = movie.RatingAverage,
                ImdbID = movie.ImdbID,
                TmdbID = movie.TmdbID,
                ReleaseGroup = movie.ReleaseGroup,
                IsMultipart = movie.IsMultipart,
                PartTypes = movie.PartTypes,
                DirectoryPath = movie.DirectoryPath,
                NumberOfAudioChannels = movie.NumberOfAudioChannels,
                AudioCodec = movie.AudioCodec,
                VideoResolution = movie.VideoResolution,
                VideoCodec = movie.VideoCodec
            };

            return mv;
        }

        public static void Reset() {

            if (Countries != null) {
                Countries.Clear();
            }

            if (Languages != null) {
                Languages.Clear();
            }

            if (Sets != null) {
                Sets.Clear();
            }

            if (Genres != null) {
                Genres.Clear();
            }

            if (Specials != null) {
                Specials.Clear();
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

        ~MovieSaver() {
            Dispose(true);
        }

        #endregion
    }

}