﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Actor;
using Frost.Providers.Xbmc.DB.Proxy;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.Provider;

namespace Frost.Providers.Xbmc.Proxies {
    class XbmcMovie : IMovie {
        private const string YT_SITE_URL = "www.youtube.com/watch?v=";

        private readonly XbmcDbMovie _movie;
        private readonly XbmcMoviesDataService _service;
        private readonly IEnumerable<IPlot> _plots;
        private readonly IEnumerable<XbmcCertification> _certifications;

        private int? _numChannels;
        private int? _vRes;
        private string _aCodec;
        private string _vCodec;
        private long? _releaseYear;
        private long? _runtime;
        private long? _top250;
        private double? _avgRating;

        public XbmcMovie(XbmcDbMovie movie, XbmcMoviesDataService service) {
            _movie = movie;
            _service = service;


            _plots = new[] { new XbmcPlot(_movie) };
            _numChannels = -1;
            _certifications = new[] { new XbmcCertification(_movie) };
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Writers":
                    case "Directors":
                    case "Actors":
                    case "Genres":
                    case "Certifications":
                    case "HasTrailer":
                    case "HasSubtitles":
                    case "HasArt":
                    case "HasNfo":
                    case "Subtitles":
                    case "Countries":
                    case "Studios":
                    case "Videos":
                    case "Audios":
                    case "Plots":
                    case "Art":
                    case "ReleaseYear":
                    case "Trailer":
                    case "Top250":
                    case "Runtime":
                    case "DirectoryPath":
                    case "Set":
                    case "Title":
                    case "RatingAverage":
                    case "SortTitle":
                    case "ImdbId":
                    case "FolderPath":
                    case "OriginalTitle":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        IEnumerable<IPerson> IMovie.Writers {
            get { return _movie.Writers; }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        IEnumerable<IPerson> IMovie.Directors {
            get { return _movie.Directors; }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        IEnumerable<IActor> IMovie.Actors {
            get { return _movie.Actors; }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        IEnumerable<IGenre> IMovie.Genres {
            get { return _movie.Genres; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        IEnumerable<ICertification> IMovie.Certifications {
            get { return _certifications; }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        IEnumerable<ISpecial> IMovie.Specials {
            get { return default(IEnumerable<ISpecial>); }
        }

        IEnumerable<IAward> IMovie.Awards {
            get { return default(IEnumerable<IAward>); }
        }

        IEnumerable<IPromotionalVideo> IMovie.PromotionalVideos {
            get { return default(IEnumerable<IPromotionalVideo>); }
        }

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        bool IMovie.HasTrailer {
            get { return !string.IsNullOrEmpty(_movie.TrailerUrl); }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        bool IMovie.HasSubtitles {
            get { return _movie.File.StreamDetails.OfType<XbmcSubtitleDetails>().Any(); }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        bool IMovie.HasArt {
            get { return _movie.Art.Any(); }
        }

        bool IMovie.HasNfo {
            get {
                if (_movie.Path.FolderPath == null) {
                    return false;
                }

                string folderPath = _movie.Path.FolderPath;
                if (Directory.Exists(folderPath)) {
                    return Directory.EnumerateFiles(folderPath, "*.nfo").Any();
                }
                return false;
            }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        IEnumerable<ISubtitle> IMovie.Subtitles {
            get { return _movie.File.StreamDetails.OfType<XbmcSubtitleDetails>(); }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        IEnumerable<ICountry> IMovie.Countries {
            get { return _movie.Countries; }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        IEnumerable<IStudio> IMovie.Studios {
            get { return _movie.Studios; }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        IEnumerable<IVideo> IMovie.Videos {
            get { return _movie.File.StreamDetails.OfType<XbmcVideoDetails>(); }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        IEnumerable<IAudio> IMovie.Audios {
            get { return _movie.File.StreamDetails.OfType<XbmcAudioDetails>(); }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        IEnumerable<IRating> IMovie.Ratings {
            get { return default(IEnumerable<IRating>); }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        IEnumerable<IPlot> IMovie.Plots {
            get { return _plots; }
            
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        IEnumerable<IArt> IMovie.Art {
            get { return _movie.Art; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title { get; set; }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle { get; set; }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle { get; set; }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        MovieType IMovie.Type {
            get { return default(MovieType); }
            set { }
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        string IMovie.Goofs {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        string IMovie.Trivia {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        long? IMovie.ReleaseYear {
            get {
                if (string.IsNullOrEmpty(_movie.ReleaseYear)) {
                    return null;
                }

                if (_releaseYear.HasValue) {
                    return _releaseYear;
                }

                int year;
                if (!int.TryParse(_movie.ReleaseYear, out year)) {
                    _releaseYear = null;
                }
                return _releaseYear = year;
            }
            set {
                if (value.HasValue) {
                    _movie.ReleaseYear = value.Value.ToString(CultureInfo.InvariantCulture);
                    _releaseYear = value;
                }
                else {
                    _movie.ReleaseYear = null;
                    _releaseYear = null;
                }
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        DateTime? IMovie.ReleaseDate {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        string IMovie.Edithion {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        DVDRegion IMovie.DvdRegion {
            get { return default(DVDRegion); }
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        DateTime? IMovie.LastPlayed {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        DateTime? IMovie.Premiered {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        DateTime? IMovie.Aired {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        string IMovie.Trailer {
            get {
                //if the trailer is not empty or null and starts with the YouTube plugin prefix
                //we extract the video Id and return the desktop YouTube video URL
                if (!string.IsNullOrEmpty(_movie.TrailerUrl)) {
                    if (_movie.TrailerUrl.StartsWith(XbmcDbMovie.YT_TRAILER_PREFIX)) {
                        string ytId = _movie.TrailerUrl.Replace(XbmcDbMovie.YT_TRAILER_PREFIX, "");
                        return YT_SITE_URL + ytId;
                    }
                    //otherwise we just return trailer as is
                    return _movie.TrailerUrl;
                }
                return null;
            }
            set {
                int idx = value.IndexOf(YT_SITE_URL, StringComparison.InvariantCultureIgnoreCase);
                if (idx >= 0) {
                    string newTrailer = value.Remove(0, YT_SITE_URL.Length + idx);
                    _movie.TrailerUrl = XbmcDbMovie.YT_TRAILER_PREFIX + newTrailer;
                }
                else {
                    _movie.TrailerUrl = value;
                }
            }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        long? IMovie.Top250 {
            get {
                if (string.IsNullOrEmpty(_movie.ImdbTop250)) {
                    return null;
                }

                if (_top250.HasValue) {
                    return _top250;
                }

                long top250;
                if (long.TryParse(_movie.ImdbTop250, out top250)) {
                    return _top250 = top250;
                }

                return _top250 = null;
            }
            set {
                if (value.HasValue) {
                    _movie.ImdbTop250 = value.Value.ToString(CultureInfo.InvariantCulture);
                    _top250 = value;
                }

                _top250 = null;
                _movie.ImdbTop250 = null;
            }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        long? IMovie.Runtime {
            get {
                if (string.IsNullOrEmpty(_movie.Runtime)) {
                    return null;
                }

                if (_runtime.HasValue) {
                    return _runtime;
                }

                long runtime;
                if (!long.TryParse(_movie.Runtime, out runtime)) {
                   return _runtime = runtime * 1000;
                }

                _runtime = null;
                return null;
            }
            set {
                if (value.HasValue) {
                    _runtime = value;
                    _movie.Runtime = (_runtime.Value / 1000).ToString(CultureInfo.InvariantCulture);
                }
                else {
                    _runtime = null;
                    _movie.Runtime = null;
                }
            }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        bool IMovie.Watched {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        long IMovie.PlayCount {
            get { return default(long); }
            set { }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        double? IMovie.RatingAverage {
            get {
                if (string.IsNullOrEmpty(_movie.Rating)) {
                    return null;
                }

                if (_avgRating.HasValue) {
                    return _avgRating;
                }

                double avgRating;
                if (double.TryParse(_movie.Rating, out avgRating)) {
                    return _avgRating = avgRating;
                }
                return _avgRating = null;
            }
            set {
                if (value.HasValue) {
                    _avgRating = value;
                    _movie.Rating = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else {
                    _avgRating = null;
                    _movie.Rating = null;
                }
            }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID { get; set; }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        string IMovie.TmdbID {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        string IMovie.ReleaseGroup {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        bool IMovie.IsMultipart {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        string IMovie.PartTypes {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        string IMovie.DirectoryPath {
            get { return _movie.Path.FolderPath; }
            set { _movie.Path.FolderPath = value; }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        int? IMovie.NumberOfAudioChannels {
            get {
                if (_numChannels != -1) {
                    return _numChannels;
                }
                var mostFrequent = ((IMovie) this).Audios.Where(a => a.NumberOfChannels.HasValue)
                                                         .GroupBy(a => a.NumberOfChannels)
                                                         .OrderByDescending(g => g.Count())
                                                         .FirstOrDefault();

                if (mostFrequent != null) {
                    IAudio a = mostFrequent.FirstOrDefault();
                    if (a == null) {
                        return null;
                    }

                    _numChannels = a.NumberOfChannels;
                    return _numChannels;
                }
                return null;
            }
            set { }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        string IMovie.AudioCodec {
            get {
                if (_aCodec != null) {
                    return _aCodec;
                }
                var mostFrequent = ((IMovie) this).Audios.Where(a => a.CodecId != null)
                                                         .GroupBy(a => a.CodecId)
                                                         .OrderByDescending(g => g.Count())
                                                         .FirstOrDefault();

                if (mostFrequent != null) {
                    IAudio a = mostFrequent.FirstOrDefault();
                    if (a == null) {
                        return null;
                    }

                    _aCodec = a.CodecId;
                    return _aCodec;
                }
                return null;
            }
            set { }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        string IMovie.VideoResolution {
            get {
                if (_vRes != null) {
                    return _vRes.ToString();
                }
                var mostFrequent = ((IMovie) this).Videos.Where(a => a.Resolution.HasValue)
                                                         .GroupBy(a => a.Resolution)
                                                         .OrderByDescending(g => g.Count())
                                                         .FirstOrDefault();

                if (mostFrequent != null) {
                    IVideo a = mostFrequent.FirstOrDefault();
                    if (a == null) {
                        return null;
                    }

                    _vRes = a.Resolution;
                    return _vRes.ToString();
                }
                return null;                
            }
            set { }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        string IMovie.VideoCodec {
            get {
                if (_vCodec != null) {
                    return _vCodec;
                }
                var mostFrequent = ((IMovie) this).Videos.Where(a => a.CodecId != null)
                                                         .GroupBy(a => a.CodecId)
                                                         .OrderByDescending(g => g.Count())
                                                         .FirstOrDefault();

                if (mostFrequent != null) {
                    IVideo a = mostFrequent.FirstOrDefault();
                    if (a == null) {
                        return null;
                    }

                    _vCodec = a.CodecId;
                    return _vCodec;
                }
                return null;
            }
            set { }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return _movie.Set; }
            set {
                XbmcSet set = _service.FindSet(value, true);
                _movie.Set = set;
            }
        }

        #region Add/Remove methods

        #region Actors

        public IActor AddActor(IActor actor) {
            XbmcMovieActor a = _service.FindActor(actor, true);
            _movie.Actors.Add(a);

            return a;
        }

        public void RemoveActor(IActor actor) {
            XbmcMovieActor a = _service.FindActor(actor, false);
            _movie.Actors.Remove(a);
        }

        #endregion

        #region Person
        public IPerson AddDirector(IPerson director) {
            XbmcPerson p = _service.FindPerson(director, true);
            _movie.Directors.Add(p);
            return p;
        }

        public void RemoveDirector(IPerson director) {
            XbmcPerson p = _service.FindPerson(director, false);
            _movie.Directors.Remove(p);
        }

        #endregion

        #region Specials

        public ISpecial AddSpecial(ISpecial special) {
            return null;
        }

        public void RemoveSpecial(ISpecial special) {
        }

        #endregion

        #region Genres

        public IGenre AddGenre(IGenre genre) {
            XbmcGenre g = _service.FindGenre(genre, false);
            _movie.Genres.Add(g);
            return g;
        }

        public void RemoveGenre(IGenre genre) {
            XbmcGenre g = _service.FindGenre(genre, true);
            if (g != null) {
                _movie.Genres.Remove(g);
            }
        }

        #endregion

        #region Plots

        public IPlot AddPlot(IPlot plot) {
            return null;
        }

        public void RemovePlot(IPlot plot) {
        }

        #endregion

        #region Studio

        public IStudio AddStudio(IStudio studio) {
            XbmcStudio s = _service.FindStudio(studio, true);
            _movie.Studios.Add(s);
            return s;
        }

        public void RemoveStudio(IStudio studio) {
            XbmcStudio s = _service.FindStudio(studio, false);
            _movie.Studios.Remove(s);
        }

        #endregion

        #region Countries

        public ICountry AddCountry(ICountry country) {
            XbmcCountry c = _service.FindCountry(country, true);
            _movie.Countries.Add(c);

            return c;
        }

        public void RemoveCountry(ICountry country) {
            XbmcCountry c = _service.FindCountry(country, false);
            _movie.Countries.Remove(c);
        }

        #endregion

        #region Subtitles

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            if (_movie.File == null || _movie.File.StreamDetails == null) {
                return null;
            }

            XbmcSubtitleDetails sub = _service.FindSubtitle(subtitle, true);
            _movie.File.StreamDetails.Add(sub);

            return sub;
        }

        public void RemoveSubtitle(ISubtitle subtitle) {
            if (_movie.File == null || _movie.File.StreamDetails == null) {
                return;
            }

            XbmcSubtitleDetails sub = _service.FindSubtitle(subtitle, false);
            _movie.File.StreamDetails.Remove(sub);
        }

        #endregion

        #endregion

    }
}