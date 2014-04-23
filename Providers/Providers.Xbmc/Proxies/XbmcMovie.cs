using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.DB.Art;
using Frost.Providers.Xbmc.DB.People;
using Frost.Providers.Xbmc.DB.StreamDetails;
using Frost.Providers.Xbmc.NFO;
using Frost.Providers.Xbmc.Provider;

namespace Frost.Providers.Xbmc.Proxies {

    public class XbmcMovie : ProxyWithService<XbmcDbMovie, XbmcMoviesDataService>, IMovie {
        private const string YT_SITE_URL = "www.youtube.com/watch?v=";
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
        private readonly XbmcPlot _xbmcPlot;

        public XbmcMovie(XbmcDbMovie movie, XbmcMoviesDataService service) : base(movie, service){
            _xbmcPlot = new XbmcPlot(Entity);
            _plots = new[] { _xbmcPlot };
            _numChannels = -1;

            if (!string.IsNullOrEmpty(Entity.MpaaRating)) {
                _certifications = new[] { new XbmcCertification(Entity) };
            }
        }

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
                    case "ImdbID":
                    case "FolderPath":
                    case "OriginalTitle":
                        return true;
                    default:
                        return false;
                }
            }
        }

        #region 1 to M

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get { return Entity.Writers; }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get { return Entity.Directors; }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get { return Entity.Actors; }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return Entity.Genres; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return _certifications; }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get { return default(IEnumerable<ISpecial>); }
        }

        public IEnumerable<IAward> Awards {
            get { return default(IEnumerable<IAward>); }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return default(IEnumerable<IPromotionalVideo>); }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            //get { return Entity.File.StreamDetails.OfType<XbmcSubtitleDetails>(); }
            get {
                return Entity.File.StreamDetails
                                  .Where(sd => sd.Type == StreamType.Subtitle)
                                  .Select(sd => new XbmcSubtitleDetails(sd));
            }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get { return Entity.Countries; }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get { return Entity.Studios; }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            //get { return Entity.File.StreamDetails.OfType<XbmcVideoDetails>(); }
            get {
                return Entity.File.StreamDetails
                                  .Where(sd => sd.Type == StreamType.Video)
                                  .Select(sd => new XbmcVideoDetails(sd));
            }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            //get { return Entity.File.StreamDetails.OfType<XbmcAudioDetails>(); }
            get {
                return Entity.File.StreamDetails
                                  .Where(sd => sd.Type == StreamType.Audio)
                                  .Select(sd => new XbmcAudioDetails(sd));
            }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return default(IEnumerable<IRating>); }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return _plots; }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return Entity.Art.Where(a => (a.Type == XbmcArt.FANART || a.Type == XbmcArt.POSTER) && a.MediaType == XbmcArt.MOVIE); }
            //get { return null; }
        }

        #endregion

        #region 1 to 1

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return Entity.Set; }
            set {
                if (value == null) {
                    Entity.Set = null;
                }
                else {
                    XbmcSet set = Service.FindSet(value, true);
                    Entity.Set = set;
                }
            }
        }

        /// <summary>Gets or sets the default cover.</summary>
        /// <value>The default cover.</value>
        public IArt DefaultCover {
            get {
                return Entity.Art.FirstOrDefault(a => a.Type == XbmcArt.POSTER);
            }
            set {
                if (value == null) {
                    Entity.Art.RemoveWhere(a => a.Type == XbmcArt.POSTER);
                    Entity.PostersXml = null;
                }
                else {
                    var dc = DefaultCover;
                    if (dc == null) {
                        XbmcArt art = Service.FindArt(value, true);
                        art.Type = XbmcArt.POSTER;

                        Entity.Art.Add(art);
                    }
                    else {
                        dc.Path = value.Path;
                    }
                }                
            } 
        }

        /// <summary>Gets or sets the default fanart to be displayed.</summary>
        /// <value>The default fanart.</value>
        public IArt DefaultFanart {
            get {
                return Entity.Art.FirstOrDefault(a => a.Type == XbmcArt.FANART);
            }
            set {
                if (value == null) {
                    Entity.Art.RemoveWhere(a => a.Type == XbmcArt.FANART);
                    Entity.FanartXml = null;
                }
                else {
                    var dc = DefaultFanart;
                    if (dc == null) {
                        XbmcArt art = Service.FindArt(value, true);
                        art.Type = XbmcArt.FANART;

                        Entity.Art.Add(art);
                    }
                    else {
                        dc.Path = value.Path;
                    }
                }                
            } 
        }

        /// <summary>Gets or sets the main plot.</summary>
        /// <value>The main plot.</value>
        public IPlot MainPlot {
            get { return _xbmcPlot; }
            set {
                if (value == null) {
                    return;
                }

                _xbmcPlot.Full = value.Full;
                _xbmcPlot.Tagline = value.Tagline;
                _xbmcPlot.Summary = value.Summary;
            }
        }

        #endregion

        #region Columns

        public long Id {
            get { return Entity.Id; }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return Entity.ImdbID; }
            set { Entity.ImdbID = value; }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get {
                string originalTitle = Entity.OriginalTitle;
                return !string.IsNullOrEmpty(originalTitle)
                           ? originalTitle
                           : null;
            }
            set { Entity.OriginalTitle = value; }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get {
                string sortTitle = Entity.SortTitle;
                return !string.IsNullOrEmpty(sortTitle)
                           ? sortTitle
                           : null;
            }
            set { Entity.SortTitle = value; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title {
            get { return Entity.Title; }
            set { Entity.Title = value; }
        }

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return !string.IsNullOrEmpty(Entity.TrailerUrl); }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            //get { return Entity.File.StreamDetails.Any(sd => sd.Type == StreamType.Subtitle); }
            //get { return Entity.File.AnySubtitles; }
            get { return Entity.File.StreamDetails.Count(sd => sd.Type == StreamType.Subtitle) > 0; }
            //get { return false; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value>
        public bool HasArt {
            get { return Entity.Art.Any(); }
            //get { return Entity.Art != null && Entity.Art.Any(); }
            //get { return false; }
        }

        public bool HasNfo {
            get {
                if (Entity.Path.FolderPath == null) {
                    return false;
                }

                string folderPath = Entity.Path.FolderPath;
                if (Directory.Exists(folderPath)) {
                    return Directory.EnumerateFiles(folderPath, "*.nfo").Any();
                }
                return false;
            }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return default(MovieType); }
            set { }
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
                if (string.IsNullOrEmpty(Entity.ReleaseYear)) {
                    return null;
                }

                if (_releaseYear.HasValue) {
                    return _releaseYear;
                }

                int year;
                if (!int.TryParse(Entity.ReleaseYear, out year)) {
                    _releaseYear = null;
                }
                return _releaseYear = year;
            }
            set {
                if (value.HasValue) {
                    Entity.ReleaseYear = value.Value.ToString(CultureInfo.InvariantCulture);
                    _releaseYear = value;
                }
                else {
                    Entity.ReleaseYear = null;
                    _releaseYear = null;
                }
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate {
            get { return default(DateTime?); }
            set { }
        }

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
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return default(DateTime?); }
            set { }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get {
                //if the trailer is not empty or null and starts with the YouTube plugin prefix
                //we extract the video Id and return the desktop YouTube video URL
                if (!string.IsNullOrEmpty(Entity.TrailerUrl)) {
                    if (Entity.TrailerUrl.StartsWith(XbmcDbMovie.YT_TRAILER_PREFIX)) {
                        string ytId = Entity.TrailerUrl.Replace(XbmcDbMovie.YT_TRAILER_PREFIX, "");
                        return YT_SITE_URL + ytId;
                    }
                    //otherwise we just return trailer as is
                    return Entity.TrailerUrl;
                }
                return null;
            }
            set {
                SetTrailer(ProxiedEntity, value);
            }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get {
                if (string.IsNullOrEmpty(Entity.ImdbTop250)) {
                    return null;
                }

                if (_top250.HasValue) {
                    return _top250;
                }

                long top250;
                if (long.TryParse(Entity.ImdbTop250, out top250)) {
                    return _top250 = top250;
                }

                return _top250 = null;
            }
            set {
                if (value.HasValue) {
                    Entity.ImdbTop250 = value.Value.ToString(CultureInfo.InvariantCulture);
                    _top250 = value;
                }
                else {
                    _top250 = null;
                    Entity.ImdbTop250 = null;
                }
            }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get {
                if (string.IsNullOrEmpty(Entity.Runtime)) {
                    return null;
                }

                if (_runtime.HasValue) {
                    return _runtime;
                }

                long runtime;
                if (!long.TryParse(Entity.Runtime, out runtime)) {
                    return _runtime = runtime * 1000;
                }

                _runtime = null;
                return null;
            }
            set {
                if (value.HasValue) {
                    _runtime = value;
                    Entity.Runtime = ((long) Math.Round(_runtime.Value / 1000.0)).ToString(CultureInfo.InvariantCulture);
                }
                else {
                    _runtime = null;
                    Entity.Runtime = null;
                }
            }
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
            get { return default(long); }
            set { }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get {
                if (string.IsNullOrEmpty(Entity.Rating)) {
                    return null;
                }

                if (_avgRating.HasValue) {
                    return _avgRating;
                }

                double avgRating;
                if (double.TryParse(Entity.Rating, out avgRating)) {
                    return _avgRating = avgRating;
                }
                return _avgRating = null;
            }
            set {
                if (value.HasValue) {
                    _avgRating = value;
                    Entity.Rating = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else {
                    _avgRating = null;
                    Entity.Rating = null;
                }
            }
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
            get {
                if (Entity.Path.FolderPath.StartsWith(@"smb:")) {
                    return Entity.Path.FolderPath.Replace(@"smb:", "");
                }
                return Entity.Path.FolderPath;
            }
            set {
                if (value.StartsWith(@"\\")) {
                    value = value.Insert(0, "smb:");
                }
                Entity.Path.FolderPath = value;
            }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get {
                if (_numChannels != -1) {
                    return _numChannels;
                }

                if (Audios == null) {
                    return null;
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
        public string AudioCodec {
            get {
                if (_aCodec != null) {
                    return _aCodec;
                }

                if (((IMovie) this).Audios == null) {
                    return null;
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
        public string VideoResolution {
            get {
                if (_vRes != null) {
                    return _vRes.ToString();
                }
                if (((IMovie) this).Videos == null) {
                    return null;
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
        public string VideoCodec {
            get {
                if (_vCodec != null) {
                    return _vCodec;
                }

                if (((IMovie) this).Videos == null) {
                    return null;
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

        #endregion

        public static void SetTrailer(XbmcDbMovie xbmcMovie, string value) {
            if (value == null) {
                xbmcMovie.TrailerUrl = null;
                return;
            }

            int idx = value.IndexOf(YT_SITE_URL, StringComparison.InvariantCultureIgnoreCase);
            if (idx >= 0) {
                string newTrailer = value.Remove(0, YT_SITE_URL.Length + idx);
                xbmcMovie.TrailerUrl = XbmcDbMovie.YT_TRAILER_PREFIX + newTrailer;
            }
            else {
                xbmcMovie.TrailerUrl = value;
            }
        }

        #region Add/Remove methods

        #region Actors

        public IActor AddActor(IActor actor) {
            XbmcPerson p = Service.FindPerson(actor, true);

            //if the movie does not yet contain this actor add it to the collection
            XbmcMovieActor act = Entity.Actors.FirstOrDefault(a => a.Person == p && a.Role == actor.Character);
            if (act == null) {
                act = new XbmcMovieActor(p, actor.Character, Entity.Actors.Count);
                Entity.Actors.Add(act);
            }
            return act;
        }

        public bool RemoveActor(IActor actor) {
            XbmcMovieActor act = actor is XbmcMovieActor 
                ? actor as XbmcMovieActor
                : Entity.Actors.FirstOrDefault(a => a.Person.Name == actor.Name && a.Role == actor.Character);

            if (act != null) {
                Entity.Actors.Remove(act);

                int idx = 0;
                foreach (XbmcMovieActor a in Entity.Actors) {
                    a.Order = idx++;
                }
            }
            return false;
        }

        #endregion

        #region Person

        /// <summary>Adds the specified writer to the provider data store.</summary>
        /// <param name="writer">The writer to add.</param>
        /// <returns>Returns the added writer. If the <paramref name="writer"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding writers or the writers does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding writers.</exception>
        public IPerson AddWriter(IPerson writer) {
            XbmcPerson p = Service.FindPerson(writer, true);
            Entity.Writers.Add(p);
            return p;
        }

        /// <summary>Removes the specified writer from the provider data store.</summary>
        /// <param name="writer">The writer to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing writers in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing writers.</exception>
        public bool RemoveWriter(IPerson writer) {
            if (writer is XbmcPerson) {
                return Entity.Writers.Remove(writer as XbmcPerson);
            }

            XbmcPerson p = Service.FindPerson(writer, false);
            return Entity.Writers.Remove(p);
        }

        public IPerson AddDirector(IPerson director) {
            XbmcPerson p = Service.FindPerson(director, true);
            Entity.Directors.Add(p);
            return p;
        }

        public bool RemoveDirector(IPerson director) {
            if (director is XbmcPerson) {
                return Entity.Directors.Remove(director as XbmcPerson);
            }

            XbmcPerson p = Service.FindPerson(director, false);
            return Entity.Directors.Remove(p);
        }

        #endregion

        #region Specials

        public ISpecial AddSpecial(ISpecial special) {
            throw new NotSupportedException();
        }

        public bool RemoveSpecial(ISpecial special) {
            throw new NotSupportedException();
        }

        #endregion

        #region Genres

        public IGenre AddGenre(IGenre genre) {
            XbmcGenre g = Service.FindGenre(genre, false);
            Entity.Genres.Add(g);
            return g;
        }

        public bool RemoveGenre(IGenre genre) {
            XbmcGenre g = Service.FindGenre(genre, true);
            if (g != null) {
                return Entity.Genres.Remove(g);
            }
            return false;
        }

        #endregion

        #region Plots

        public IPlot AddPlot(IPlot plot) {
            throw new NotSupportedException();
        }

        public bool RemovePlot(IPlot plot) {
            throw new NotSupportedException();
        }

        #endregion

        #region Studio

        public IStudio AddStudio(IStudio studio) {
            XbmcStudio s = Service.FindStudio(studio, true);
            Entity.Studios.Add(s);
            return s;
        }

        public bool RemoveStudio(IStudio studio) {
            XbmcStudio s = Service.FindStudio(studio, false);
            return Entity.Studios.Remove(s);
        }

        #endregion

        #region Countries

        public ICountry AddCountry(ICountry country) {
            XbmcCountry c = Service.FindCountry(country, true);
            Entity.Countries.Add(c);

            return c;
        }

        public bool RemoveCountry(ICountry country) {
            XbmcCountry c = Service.FindCountry(country, false);
            return Entity.Countries.Remove(c);
        }

        #endregion

        #region Subtitles

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            if (Entity.File == null || Entity.File.StreamDetails == null) {
                return null;
            }

            XbmcSubtitleDetails sub = Service.FindSubtitle(subtitle, true);
            Entity.File.StreamDetails.Add(sub.ProxiedEntity);

            return sub;
        }

        public bool RemoveSubtitle(ISubtitle subtitle) {
            if (Entity.File == null || Entity.File.StreamDetails == null) {
                return false;
            }

            XbmcSubtitleDetails sub = Service.FindSubtitle(subtitle, false);
            return Entity.File.StreamDetails.Remove(sub.ProxiedEntity);
        }

        #endregion

        /// <summary>Adds the specified audio to the provider data store.</summary>
        /// <param name="audio">The audio to add.</param>
        /// <returns>Returns the added audio. If the <paramref name="audio"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding audios or the audio does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding audios.</exception>
        public IAudio AddAudio(IAudio audio) {
            XbmcDbStreamDetails a = new XbmcDbStreamDetails(audio);
            Entity.File.StreamDetails.Add(a);

            return new XbmcAudioDetails(a);
        }

        /// <summary>Removes the specified audio from the provider data store.</summary>
        /// <param name="audio">The audio to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing audios in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing audios.</exception>
        public bool RemoveAudio(IAudio audio) {
            if (audio is XbmcAudioDetails) {
                return Entity.File.StreamDetails.Remove((audio as XbmcAudioDetails).ProxiedEntity);
            }

            if (audio.Id > 0) {
                return Entity.File.StreamDetails.RemoveWhere(v => v.Id == audio.Id) > 0;
            }
            return false;
        }

        /// <summary>Adds the specified video to the provider data store.</summary>
        /// <param name="video">The video to add.</param>
        /// <returns>Returns the added video. If the <paramref name="video"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding videos or the video does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding videos.</exception>
        public IVideo AddVideo(IVideo video) {
            XbmcDbStreamDetails a = new XbmcDbStreamDetails(video);
            Entity.File.StreamDetails.Add(a);

            return new XbmcVideoDetails(a);
        }

        /// <summary>Removes the specified video from the provider data store.</summary>
        /// <param name="video">The video to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing videos in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing video.</exception>
        public bool RemoveVideo(IVideo video) {
            if (video is XbmcVideoDetails) {
                return Entity.File.StreamDetails.Remove((video as XbmcVideoDetails).ProxiedEntity);
            }

            if (video.Id > 0) {
                return Entity.File.StreamDetails.RemoveWhere(v => v.Id == video.Id) > 0;
            }
            return false;
        }

        #endregion

        public void Update(MovieInfo movieInfo) {
            
        }
    }

}