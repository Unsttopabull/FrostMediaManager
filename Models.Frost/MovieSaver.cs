﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.Models;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;

namespace Frost.Providers.Frost {

    public class MovieSaver : IMovieSaver {
        private readonly Dictionary<ISOCountryCode, Country> _countries;
        private readonly Dictionary<ISOLanguageCode, Language> _languages;
        private readonly Dictionary<string, Set> _sets;
        private readonly Dictionary<string, Genre> _genres;
        private readonly Dictionary<string, Special> _specials;
        private readonly Dictionary<string, Studio> _studios;
        private readonly Dictionary<string, Person> _people;
        private readonly IEnumerable<MovieInfo> _infos;


        private readonly FrostDbContainer _mvc;

        public MovieSaver(IEnumerable<MovieInfo> movies) {
            _infos = movies;

            _countries = new Dictionary<ISOCountryCode, Country>();
            _languages = new Dictionary<ISOLanguageCode, Language>();
            _sets = new Dictionary<string, Set>(StringComparer.InvariantCultureIgnoreCase);
            _genres = new Dictionary<string, Genre>(StringComparer.InvariantCultureIgnoreCase);
            _specials = new Dictionary<string, Special>(StringComparer.InvariantCultureIgnoreCase);
            _studios = new Dictionary<string, Studio>(StringComparer.InvariantCultureIgnoreCase);
            _people = new Dictionary<string, Person>(StringComparer.InvariantCultureIgnoreCase);

            //_mvc = new MovieVoContainer(true, "movieVo.db3");
            _mvc = new FrostDbContainer(true);
        }

        public void Save() {
            //mvc.Languages.Load();
            //mvc.Specials.Load();
            //mvc.Countries.Load();
            //mvc.Awards.Load();
            //mvc.Studios.Load();
            //mvc.Sets.Load();
            //mvc.People.Load();

            int i = 0;
            foreach (MovieInfo movie in _infos) {
                Debug.WriteLine("{0} ({1})", movie.Title, ++i);
                Debug.Indent();
                Save(movie);

                Debug.Unindent();
            }

            _mvc.SaveChanges();
        }

        private void Save(MovieInfo movie) {
            Movie mv = FromMovieInfo(movie);
            mv = _mvc.Movies.Add(mv);

            mv.Set = GetHasName(movie.Set, _sets);
            mv.Plots = new HashSet<Plot>(movie.Plots.ConvertAll(GetPlot));
            mv.Art = new HashSet<Art>(movie.Art.ConvertAll(art => new Art(art.Type, art.Path, art.Preview)));
            mv.Certifications = new HashSet<Certification>(movie.Certifications.ConvertAll(GetCertification));
            mv.Specials = new HashSet<Special>(movie.Specials.ConvertAll(GetSpecial));
            mv.Genres = new HashSet<Genre>(movie.Genres.ConvertAll(g => GetHasName(g, _genres)));
            mv.Studios = new HashSet<Studio>(movie.Studios.ConvertAll(s => GetHasName(s, _studios)));
            mv.Countries = new HashSet<Country>(movie.Countries.ConvertAll(GetCountry));
            mv.Writers = new HashSet<Person>(movie.Writers.ConvertAll(GetPerson));
            mv.Directors = new HashSet<Person>(movie.Directors.ConvertAll(GetPerson));
            mv.Actors = new HashSet<Actor>(movie.Actors.ConvertAll(actorInfo => new Actor(GetPerson(actorInfo), actorInfo.Character)));

            int videos = movie.FileInfos.Count(f => f.Videos.Count == 0 && !f.Extension.Equals("iso", StringComparison.OrdinalIgnoreCase));
            int audios = movie.FileInfos.Count(f => f.Audios.Count == 0 && !f.Extension.Equals("iso", StringComparison.OrdinalIgnoreCase));

            mv.Videos = new HashSet<Video>();
            mv.Audios = new HashSet<Audio>();
            mv.Subtitles = new HashSet<Subtitle>();
            foreach (FileDetectionInfo fileInfo in movie.FileInfos) {
                Debug.Indent();
                File file = new File(fileInfo.Name, fileInfo.Extension, fileInfo.FolderPath, fileInfo.Size);
                Debug.Unindent();

                AddVideos(fileInfo, mv, file);
                AddAudios(fileInfo, mv, file);
                AddSubtitles(fileInfo, mv, file);
            }
            int ab = 5;
        }

        private void AddSubtitles(FileDetectionInfo fileInfo, Movie mv, File file) {
            foreach (SubtitleDetectionInfo s in fileInfo.Subtitles) {
                mv.Subtitles.Add(new Subtitle(file) {
                    Language = GetLanguage(s.Language),
                    Format = s.Format,
                    EmbededInVideo = s.EmbededInVideo,
                    ForHearingImpaired = s.ForHearingImpaired,
                    Encoding =  s.Encoding
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
            Debug.WriteLine("Looking for person: " + info.Name);
            Debug.Indent();

            Person p;
            if (_people.TryGetValue(info.Name, out p)) {
                Debug.WriteLine("Found in dict");
                Debug.Unindent();
                return p;
            }

            p = _mvc.People.FirstOrDefault(person => person.Name == info.Name);
            if (p != null) {
                _people.Add(p.Name, p);

                Debug.WriteLine("Found in DB");
                Debug.Unindent();
                return p;
            }

            p = new Person(info.Name, info.Thumb);
            _people.Add(p.Name, p);

            Debug.WriteLine("Created new person");
            Debug.Unindent();

            return p;
        }

        private Language GetLanguage(ISOLanguageCode langCode) {
            if (langCode == null) {
                return null;
            }

            Debug.WriteLine("Looking for language: " + langCode.EnglishName);
            Debug.Indent();

            Language lang;
            if (_languages.TryGetValue(langCode, out lang)) {
                Debug.WriteLine("Found in dict");
                Debug.Unindent();

                return lang;
            }

            lang = _mvc.Languages.FirstOrDefault(l => l.ISO639.Alpha3 == langCode.Alpha3);
            if (lang != null) {
                _languages.Add(langCode, lang);

                Debug.WriteLine("Found in DB");
                Debug.Unindent();
                return lang;
            }

            lang = new Language(langCode);
            _languages.Add(langCode, lang);

            Debug.WriteLine("Created new Language");
            Debug.Unindent();

            return lang;
        }

        private Country GetCountry(ISOCountryCode countryCode) {
            Debug.WriteLine("Looking for country: " + countryCode.EnglishName);
            Debug.Indent();

            Country lang;
            if (_countries.TryGetValue(countryCode, out lang)) {
                Debug.WriteLine("Found in dict");
                Debug.Unindent();

                return lang;
            }

            lang = _mvc.Countries.FirstOrDefault(l => l.ISO3166.Alpha3 == countryCode.Alpha3);
            if (lang != null) {
                Debug.WriteLine("Found in DB");
                Debug.Unindent();

                _countries.Add(countryCode, lang);
                return lang;
            }

            Debug.WriteLine("Created new country");
            Debug.Unindent();

            lang = new Country(countryCode);
            _countries.Add(countryCode, lang);

            return lang;
        }

        private T GetHasName<T>(string name, IDictionary<string, T> dictToSearch) where T : class, IHasName, new() {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }

            Debug.WriteLine("Looking for: " + typeof(T).Name);
            Debug.Indent();

            T hasName;
            if (dictToSearch.TryGetValue(name, out hasName)) {
                Debug.WriteLine("Found in dict");
                Debug.Unindent();

                return hasName;
            }

            hasName = _mvc.Set<T>().FirstOrDefault<T>(s => s.Name == name);
            if (hasName != null) {
                Debug.WriteLine("Found in DB");
                Debug.Unindent();

                dictToSearch.Add(hasName.Name, hasName);
                return hasName;
            }

            Debug.WriteLine("Created new one");
            Debug.Unindent();

            hasName = new T { Name = name };
            dictToSearch.Add(hasName.Name, hasName);

            return hasName;
        }

        private Special GetSpecial(string special) {
            Debug.WriteLine("Looking for special: " + special);
            Debug.Indent();

            Special spec;
            if (_specials.TryGetValue(special, out spec)) {
                Debug.WriteLine("Found in dict");
                Debug.Unindent();

                return spec;
            }

            spec = _mvc.Specials.FirstOrDefault(s => s.Value == special);
            if (spec != null) {
                Debug.WriteLine("Found in DB");
                Debug.Unindent();

                _specials.Add(spec.Value, spec);
                return spec;
            }

            Debug.WriteLine("Created new special");
            Debug.Unindent();

            spec = new Special(special);
            _specials.Add(spec.Value, spec);

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

            if (_mvc != null) {
                _mvc.Dispose();
            }

            if (_countries != null) {
                _countries.Clear();
            }

            if (_languages != null) {
                _languages.Clear();
            }

            if (_sets != null) {
                _sets.Clear();
            }

            if (_genres != null) {
                _genres.Clear();
            }

            if (_specials != null) {
                _specials.Clear();
            }

            if (_studios != null) {
                _studios.Clear();
            }

            if (_people != null) {
                _people.Clear();
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