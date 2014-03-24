using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtMovie : IMovie {
        private readonly XjbPhpMovie _movie;
        private readonly string _xtreamerPath;
        private readonly IEnumerable<IStudio> _studios;
        private readonly IEnumerable<IAudio> _audios;
        private readonly IEnumerable<IVideo> _videos;
        private readonly IEnumerable<IPlot> _plots;
        private readonly IEnumerable<IArt> _arts;
        private bool? _hasNfo;

        public XtMovie(XjbPhpMovie movie, string xtPath) {
            _movie = movie;
            _xtreamerPath = xtPath;

            if (_movie.Studio != null) {
                _studios = new List<IStudio> { new XtStudio(_movie) };
            }

            _audios = new List<IAudio> { new XtAudio(_movie) };
            _videos = new List<IVideo> { new XtVideo(_movie) };
            _plots = new List<IPlot> { new XtPlot(_movie) };
            List<IArt> arts = new List<IArt> { new XtCover(_movie, _xtreamerPath) };

            if (_movie.Fanart != null) {
                for (int i = 0; i < _movie.Fanart.Length; i++) {
                    arts.Add(new XtFanart(_movie, i, _xtreamerPath));
                }
            }
            _arts = arts;
        }

        public long Id {
            get { return _movie.Id; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Title":
                    case "OriginalTitle":
                    case "HasNfo":
                    case "HasArt":
                    case "HasSubtitles":
                    case "HasTrailer":
                    case "Arts":
                    case "Geres":
                    case "Ratings":
                    case "Specials":
                    case "Studios":
                    case "Countries":
                    case "Subtitles":
                    case "VideoCodec":
                    case "VideoResolution":
                    case "AudioCodec":
                    case "NumberOfChannels":
                    case "DirectoryPath":
                    case "ImdbId":
                    case "RatingAverage":
                    case "Runtime":
                    case "ReleaseYear":
                    case "SortTitle":
                    case "Certifications":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title {
            get { return _movie.Title; }
            set { _movie.Title = value; }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return _movie.OriginalTitle; }
            set { _movie.OriginalTitle = value; }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return _movie.SortTitle; }
            set { _movie.SortTitle = value; }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return default(MovieType); }
            set{ }
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get {
                long year;
                if (long.TryParse(_movie.ReleaseYear, out year)) {
                    return year;
                }
                return null;
            }
            set {
                if (value.HasValue) {
                    _movie.ReleaseYear = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                _movie.ReleaseYear = null;
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion {
            get { return default(DVDRegion); }
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed {
            get { return default(DateTime?); }
            set{ }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return default(DateTime?); }
            set{ }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return default(DateTime?); }
            set{ }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return (long?) _movie.Runtime * 1000; }
            set { _movie.Runtime = value / 1000; }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount {
            get { return 0; }
            set { }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return _movie.RatingAverage; }
            set {
                _movie.RatingAverage = (int) Math.Round(value ?? 0);
            }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return _movie.ImdbId; }
            set { _movie.ImdbId = value; }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart {
            get { return false; }
            set { } 
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes {
            get { return default(string); }
            set { } 
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return _xtreamerPath + _movie.FilePathOnDrive; }
            set { _movie.FilePathOnDrive = value.Replace(_xtreamerPath, ""); }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get {
                int num;
                if (int.TryParse(_movie.AudioChannels, out num)) {
                    return num;
                }
                return null;
            }
            set {
                _movie.AudioChannels = value.HasValue
                    ? value.Value.ToString(CultureInfo.InvariantCulture)
                    : null;
            }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return _movie.AudioCodec; }
            set { _movie.AudioCodec = value; }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return _movie.VideoResolution; }
            set { _movie.VideoResolution = value; }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return _movie.VideoCodec; }
            set { _movie.VideoCodec = value; }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get {
                return _movie.Subtitles != null
                    ? _movie.Subtitles.Select((s, i) => new XtSubtitle(_movie, i))
                    : Enumerable.Empty<ISubtitle>();
            }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get {
                if (_movie.Countries != null) {
                    return _movie.Countries.Select(XtCountry.FromIsoCode).Where(c => c != null);
                }
                return Enumerable.Empty<ICountry>();
            }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios { get { return _studios; } }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos { get { return _videos; } }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios { get { return _audios; }}

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return _movie.Ratings.Select(kvp => new XtRating(_movie, kvp.Key)); } 
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots { get { return _plots; } }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return _arts; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return _movie.Certifications.Select(kvp => new XtCertification(_movie, kvp.Key)); } 
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get {
                if (_movie.Cast != null) {
                    return _movie.Cast.Where(person => person.Job == "writer")
                                 .Select(person => new XtPerson(person));
                }
                return null;
            }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get {
                if (_movie.Cast != null) {
                    return _movie.Cast
                                 .Where(person => person.Job == "director")
                                 .Select(person => new XtPerson(person));
                }
                return null;
            }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get {
                if (_movie.Cast != null) {
                    return _movie.Cast
                                 .Where(person => person.Job == "actor" && !string.IsNullOrEmpty(person.Name))
                                 .Select(person => new XtActor(person));
                }
                return null;
            }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get {
                if (string.IsNullOrEmpty(_movie.Specials)) {
                    return null;
                }
                return _movie.Specials.SplitWithoutEmptyEntries(',')
                                        .Select(special => new XtSpecial(_movie, special));
            }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return _movie.Genres.Select(g => new XtGenre(g)); } 
        }

        public IEnumerable<IAward> Awards {
            get { return null; }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return null; }
        }

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer { get { return false; } }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return _movie.Subtitles != null && _movie.Subtitles.Length > 0; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        public bool HasArt {
            get {
                return (_movie.Art != null && _movie.Art.Length > 0) ||
                       (!string.IsNullOrEmpty(_movie.CoverPath)) ||
                       (_movie.Fanart != null && _movie.Fanart.Length > 0) ||
                       (_movie.Screens != null && _movie.Screens.Length > 0);
            }
        }

        public bool HasNfo {
            get {
                if (_hasNfo.HasValue) {
                    return _hasNfo.Value;
                }

                string folderPath = DirectoryPath;
                if (folderPath == null) {
                    return false;
                }

                if (Directory.Exists(folderPath)) {
                    _hasNfo = Directory.EnumerateFiles(folderPath, "*.nfo").Any() ||  Directory.EnumerateFiles(folderPath, "*_xjb.xml").Any();
                    return _hasNfo.Value;
                }
                return false;                
            }
        }

        public IActor AddActor(IActor actor) {
            throw new NotImplementedException();
        }

        public ICountry AddCountry(ICountry country) {
            throw new NotImplementedException();
        }

        public IPerson AddDirector(IPerson director) {
            throw new NotImplementedException();
        }

        public IGenre AddGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public IPlot AddPlot(IPlot plot) {
            throw new NotImplementedException();
        }

        public ISpecial AddSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public IStudio AddStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            throw new NotImplementedException();
        }

        public void RemoveActor(IActor actor) {
            throw new NotImplementedException();
        }

        public void RemoveCountry(ICountry country) {
            throw new NotImplementedException();
        }

        public void RemoveDirector(IPerson director) {
            throw new NotImplementedException();
        }

        public void RemoveGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public void RemovePlot(IPlot plot) {
            throw new NotImplementedException();
        }

        public void RemoveSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public void RemoveStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public void RemoveSubtitle(ISubtitle subtitle) {
            throw new NotImplementedException();
        }
    }

}
