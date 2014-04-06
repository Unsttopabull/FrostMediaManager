using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Common.Util.ISO;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtMovie : ChangeTrackingProxy<XjbPhpMovie>, IMovie {
        private readonly string _xtreamerPath;
        private readonly IList<IStudio> _studios;
        private readonly IList<IAudio> _audios;
        private readonly IList<IVideo> _videos;
        private readonly IList<IPlot> _plots;
        private readonly IEnumerable<IArt> _arts;
        private readonly string _xtPathRoot;
        private ChangeTrackingCollection<XtSubtitle> _subtitles;
        private ChangeTrackingCollection<XtActor> _actors;
        private ChangeTrackingCollection<XtGenre> _genres;
        private ChangeTrackingCollection<XtSpecial> _specials;
        private ChangeTrackingCollection<XtPerson> _directors;
        private ChangeTrackingCollection<XtPerson> _writers;
        private ChangeTrackingCollection<XtCountry> _countries;
        private bool? _hasNfo;

        public XtMovie(XjbPhpMovie movie, string xtPath) : base(movie) {
            _xtreamerPath = xtPath;
            _xtPathRoot = Path.GetPathRoot(_xtreamerPath);

            if (Entity.Studio != null) {
                _studios = new List<IStudio> { new XtStudio(Entity) };
            }

            _audios = new List<IAudio> { new XtAudio(Entity, _xtreamerPath) };
            _videos = new List<IVideo> { new XtVideo(Entity, _xtreamerPath) };
            _plots = new List<IPlot> { new XtPlot(Entity) };
            List<IArt> arts = new List<IArt> { new XtCover(Entity, _xtreamerPath) };

            if (Entity.Fanart != null) {
                for (int i = 0; i < Entity.Fanart.Length; i++) {
                    arts.Add(new XtFanart(Entity, i, _xtreamerPath));
                }
            }
            _arts = arts;

            OriginalValues = new Dictionary<string, object> {
                { "Title", Entity.Title },
                { "OriginalTitle", Entity.OriginalTitle },
                { "SortTitle", Entity.SortTitle },
                { "ReleaseYear", Entity.ReleaseYear },
                { "Runtime", Entity.Runtime },
                { "RatingAverage", Entity.RatingAverage },
                { "ImdbID", Entity.ImdbId },
                { "DirectoryPath", Entity.FilePathOnDrive },
                { "NumberOfAudioChannels", Entity.AudioChannels },
                { "AudioCodec", Entity.AudioCodec },
                { "VideoResolution", Entity.VideoResolution },
                { "VideoCodec", Entity.VideoCodec },
                { "Specials", Entity.Specials }
            };
        }

        public override bool IsDirty {
            get {
                if (_subtitles != null) {
                    if (_subtitles.IsDirty) {
                        MarkChanged("Subtitles");
                    }
                    else if(_subtitles.Any(s => s.IsDirty)){
                        MarkChanged("Subtitle");
                    }
                }

                if (_actors != null && _actors.IsDirty) {
                    MarkChanged("Actors");
                }

                if (_writers != null && _writers.IsDirty) {
                    MarkChanged("Writers");
                }

                if (_directors != null && _directors.IsDirty) {
                    MarkChanged("Directors");
                }

                if (_countries != null && _countries.IsDirty) {
                    MarkChanged("Countries");
                }

                if (_genres != null && _genres.IsDirty) {
                    MarkChanged("Genres");
                }

                if (((XtPlot) _plots[0]).IsDirty) {
                    MarkChanged("Plots");
                }

                if (((XtAudio) _audios[0]).IsDirty) {
                    MarkChanged("Audios");
                }

                if (((XtVideo) _videos[0]).IsDirty) {
                    MarkChanged("Videos");
                }

                if (_arts.Cast<XtArt>().Any(a => a.IsDirty)) {
                    MarkChanged("Art");
                }

                return base.IsDirty;
            }
        }

        #region Columns

        public long Id {
            get { return Entity.Id; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title {
            get { return Entity.Title; }
            set {
                TrackChanges(value);
                Entity.Title = value;
            }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return Entity.OriginalTitle; }
            set {
                TrackChanges(value);
                Entity.OriginalTitle = value;
            }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return Entity.SortTitle; }
            set {
                TrackChanges(value);
                Entity.SortTitle = value;
            }
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get {
                long year;
                if (long.TryParse(Entity.ReleaseYear, out year)) {
                    return year;
                }
                return null;
            }
            set {
                Entity.ReleaseYear = value.HasValue
                                         ? value.Value.ToString(CultureInfo.InvariantCulture)
                                         : null;
                TrackChanges(Entity.ReleaseYear);
            }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return (long?) Entity.Runtime * 1000; }
            set {
                Entity.Runtime = value / 1000;
                TrackChanges(Entity.Runtime);
            }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return Entity.RatingAverage / 10d; }
            set {
                Entity.RatingAverage = value.HasValue ? (int) Math.Round(value.Value * 10) : 0;
                TrackChanges(Entity.RatingAverage);
            }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return Entity.ImdbId; }
            set {
                TrackChanges(value);
                Entity.ImdbId = value;
            }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return _xtreamerPath + Entity.FilePathOnDrive; }
            set {
                Entity.FilePathOnDrive = value.Replace(_xtreamerPath, "");
                TrackChanges(Entity.FilePathOnDrive);
            }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get {
                int num;
                if (int.TryParse(Entity.AudioChannels, out num)) {
                    return num;
                }
                return null;
            }
            set {
                Entity.AudioChannels = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
                TrackChanges(Entity.AudioChannels);
            }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return Entity.AudioCodec; }
            set {
                Entity.AudioCodec = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return Entity.VideoResolution; }
            set {
                Entity.VideoResolution = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return Entity.VideoCodec; }
            set {
                Entity.VideoCodec = value;
                TrackChanges(value);
            }
        }

        #region Not Implemented

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

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return default(DateTime?); }
            set { }
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

        #endregion

        #endregion

        #region M to 1

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return null; }
            set { }
        }

        #endregion

        #region 1 to M

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get {
                if (Entity.Subtitles == null) {
                    return Enumerable.Empty<ISubtitle>();
                }

                if (_subtitles != null) {
                    return _subtitles;
                }

                return _subtitles = new ChangeTrackingCollection<XtSubtitle>(
                                        Entity.Subtitles.Select((s, i) => new XtSubtitle(Entity, i, _xtreamerPath)).ToList(),
                                        s => Entity.Subtitles.Add(s.GetSubtitlePath()),
                                        null
                                        );
            }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get {
                if (Entity.Countries != null) {
                    if (_countries == null) {
                        _countries = new ChangeTrackingCollection<XtCountry>(
                            Entity.Countries.Select(XtCountry.FromIsoCode).Where(c => c != null).ToList(),
                            c => Entity.Countries.Add(c.ISO3166.Alpha2),
                            null
                            );
                    }
                    return _countries;
                }
                return Enumerable.Empty<ICountry>();
            }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get { return _studios; }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            get { return _videos; }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get { return _audios; }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return Entity.Ratings.Select(kvp => new XtRating(Entity, kvp.Key)); }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return _plots; }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return _arts; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return Entity.Certifications.Select(kvp => new XtCertification(Entity, kvp.Key)); }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get {
                if (Entity.Cast == null) {
                    return null;
                }

                if (_writers == null) {
                    _writers = new ChangeTrackingCollection<XtPerson>(
                        Entity.Cast.Where(person => person.Job == XjbPhpPerson.JOB_WRITER).Select(person => new XtPerson(person)).ToList(),
                        p => Entity.Cast.Add(p.ProxiedEntity),
                        p => Entity.Cast.Remove(p.ProxiedEntity)
                        );
                }
                return _writers;
            }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get {
                if (Entity.Cast == null) {
                    return null;
                }

                if (_directors == null) {
                    _directors = new ChangeTrackingCollection<XtPerson>(
                        Entity.Cast.Where(person => person.Job == XjbPhpPerson.JOB_DIRECTOR).Select(person => new XtPerson(person)).ToList(),
                        p => Entity.Cast.Add(p.ProxiedEntity),
                        p => Entity.Cast.Remove(p.ProxiedEntity)
                        );
                }
                return _directors;
            }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get {
                if (Entity.Cast == null) {
                    return null;
                }
                if (_actors == null) {
                    _actors = new ChangeTrackingCollection<XtActor>(
                        Entity.Cast.Where(person => person.Job == XjbPhpPerson.JOB_ACTOR && !string.IsNullOrEmpty(person.Name)).Select(person => new XtActor(person)).ToList(),
                        a => Entity.Cast.Add(a.ProxiedEntity),
                        a => Entity.Cast.Remove(a.ProxiedEntity)
                        );
                }
                return _actors;
            }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get {
                if (string.IsNullOrEmpty(Entity.Specials)) {
                    return null;
                }

                if (_specials == null) {
                    _specials = new ChangeTrackingCollection<XtSpecial>(
                        Entity.Specials.SplitWithoutEmptyEntries(',').Select(special => new XtSpecial(Entity, special)).ToList(),
                        s => Entity.AddSpecial(s.Name),
                        s => Entity.RemoveSpecial(s.Name)
                        );
                }
                return _specials;
            }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get {
                if (_genres != null) {
                    _genres = new ChangeTrackingCollection<XtGenre>(
                        Entity.Genres.Select(g => new XtGenre(g)).ToList(),
                        g => Entity.Genres.Add(g.ProxiedEntity),
                        g => Entity.Genres.Remove(g.ProxiedEntity)
                        );
                }
                return _genres;
            }
        }

        public IEnumerable<IAward> Awards {
            get { return null; }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return null; }
        }

        #endregion

        #region Utility

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return false; }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return Entity.Subtitles != null && Entity.Subtitles.Count > 0; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value>
        public bool HasArt {
            get {
                return (Entity.Art != null && Entity.Art.Count > 0) ||
                       (!string.IsNullOrEmpty(Entity.CoverPath)) ||
                       (Entity.Fanart != null && Entity.Fanart.Length > 0) ||
                       (Entity.Screens != null && Entity.Screens.Length > 0);
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
                    _hasNfo = Directory.EnumerateFiles(folderPath, "*.nfo").Any() || Directory.EnumerateFiles(folderPath, "*_xjb.xml").Any();
                    return _hasNfo.Value;
                }
                return false;
            }
        }

        #endregion

        #region Add / Remove

        public IActor AddActor(IActor actor) {
            XtActor act = _actors.FirstOrDefault(a => a.Equals(actor));
            if (act == null) {
                act = new XtActor(new XjbPhpPerson(actor));
                _actors.Add(act);
            }
            return act;
        }

        public bool RemoveActor(IActor actor) {
            if (actor == null || string.IsNullOrEmpty(actor.Name)) {
                return false;
            }

            XjbPhpPerson person = Entity.Cast.Find(phpPerson =>
                                                   string.Equals(actor.Name, phpPerson.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                   phpPerson.Job == XjbPhpPerson.JOB_ACTOR);

            return person != null && _actors.Remove(new XtActor(person));
        }

        public IPerson AddDirector(IPerson director) {
            XjbPhpPerson person = Entity.Cast.FirstOrDefault(a =>
                                                             a.Name.Equals(director.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                             a.Job == XjbPhpPerson.JOB_DIRECTOR);

            if (person == null) {
                person = new XjbPhpPerson(director, XjbPhpPerson.JOB_DIRECTOR);
                Entity.Cast.Add(person);
            }
            return new XtPerson(person);
        }

        public bool RemoveDirector(IPerson director) {
            XjbPhpPerson person = Entity.Cast.FirstOrDefault(a =>
                                                             a.Name.Equals(director.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                             a.Job == XjbPhpPerson.JOB_DIRECTOR);

            return person != null && Entity.Cast.Remove(person);
        }

        public ICountry AddCountry(ICountry country) {
            string alpha2 = FindCountry(country);

            if (string.IsNullOrEmpty(alpha2)) {
                return null;
            }

            XtCountry xtCountry = XtCountry.FromIsoCode(alpha2);
            _countries.Add(xtCountry);
            return xtCountry;
        }

        public bool RemoveCountry(ICountry country) {
            string alpha2 = FindCountry(country);
            if (string.IsNullOrEmpty(alpha2)) {
                return false;
            }

            _countries.RemoveWhere(xt => string.Equals(xt.ISO3166.Alpha2, alpha2, StringComparison.OrdinalIgnoreCase));
            return true;
        }

        private string FindCountry(ICountry country) {
            if (country == null || string.IsNullOrEmpty(country.Name)) {
                return null;
            }

            if (country.ISO3166 == null) {
                ISOCountryCode isoCountryCode = ISOCountryCodes.Instance.GetByEnglishName(country.Name);
                if (isoCountryCode != null) {
                    country.ISO3166 = new ISO3166(isoCountryCode.Alpha2, isoCountryCode.Alpha3);
                }
                else {
                    return null;
                }
            }

            return Entity.Countries.Find(countryAlpha2 => !string.IsNullOrEmpty(countryAlpha2) && countryAlpha2.Equals(country.ISO3166.Alpha2));
        }

        public IGenre AddGenre(IGenre genre) {
            XjbPhpGenre genr = Entity.Genres.Find(g => g == genre) ?? new XjbPhpGenre(genre);

            XtGenre xtGenre = new XtGenre(genr);
            _genres.Add(xtGenre);

            return xtGenre;
        }

        public bool RemoveGenre(IGenre genre) {
            XjbPhpGenre genr = Entity.Genres.Find(g => g == genre);
            if (genr == null) {
                return false;
            }

            _genres.RemoveWhere(g => string.Equals(g.Name, genr.Name, StringComparison.CurrentCultureIgnoreCase));
            return true;
        }

        public IPlot AddPlot(IPlot plot) {
            throw new NotSupportedException();
        }

        public bool RemovePlot(IPlot plot) {
            throw new NotSupportedException();
        }

        public ISpecial AddSpecial(ISpecial special) {
            if (special == null || string.IsNullOrEmpty(special.Name)) {
                return null;
            }

            XtSpecial addSpecial = new XtSpecial(Entity, special.Name);
            _specials.Add(addSpecial);
            return addSpecial;
        }

        public bool RemoveSpecial(ISpecial special) {
            if (special == null || string.IsNullOrEmpty(special.Name)) {
                return false;
            }

            _specials.RemoveWhere(xs => string.Equals(xs.Name, special.Name, StringComparison.CurrentCultureIgnoreCase));
            return true;
        }

        public IStudio AddStudio(IStudio studio) {
            throw new NotSupportedException();
        }

        public bool RemoveStudio(IStudio studio) {
            throw new NotSupportedException();
        }

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            string sub = GetSubtitlePath(subtitle);
            if (string.IsNullOrEmpty(sub)) {
                throw new NotSupportedException("The subtitle must be on the Xtreamer Drive");
            }

            XtSubtitle addSubtitle = new XtSubtitle(Entity, Entity.Subtitles.Count - 1, _xtreamerPath);
            _subtitles.Add(addSubtitle);
            return addSubtitle;
        }

        public bool RemoveSubtitle(ISubtitle subtitle) {
            string sub = GetSubtitlePath(subtitle);
            if (string.IsNullOrEmpty(sub)) {
                return false;
            }

            _subtitles.RemoveWhere(s => string.Equals(sub, s.GetSubtitlePath(), StringComparison.OrdinalIgnoreCase));
            return true;
        }

        private string GetSubtitlePath(ISubtitle subtitle) {
            if (subtitle == null || subtitle.File == null) {
                return null;
            }

            string fullPath = subtitle.File.FullPath;
            if (Path.GetPathRoot(fullPath) != _xtPathRoot) {
                return null;
            }

            fullPath = fullPath.Replace(_xtPathRoot ?? "", "").TrimStart('\\');
            fullPath = fullPath.Remove(0, fullPath.IndexOfAny(new[] { '\\', '/' }));

            string fullPathSearch = fullPath.Replace('/', ' ').Replace('\\', ' ');
            return Entity.Subtitles.FirstOrDefault(subFile =>
                                                   subFile != null &&
                                                   string.Equals(subFile.Replace('/', ' ').Replace('\\', ' '), fullPathSearch)
                );
        }

        #endregion

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "ImdbID":
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

        public void Update(MovieInfo movieInfo) {
            
        }

        #region Change Tracking

        internal IEnumerable<XtPerson> GetChangedWriters() {
            return _writers != null
                       ? _writers.Where(w => w.Id > 0 && w.IsDirty)
                       : Enumerable.Empty<XtPerson>();
        }

        internal IEnumerable<XtPerson> GetChangedDirectors() {
            return _directors != null
                       ? _directors.Where(w => w.Id > 0 && w.IsDirty)
                       : Enumerable.Empty<XtPerson>();
        }

        internal IEnumerable<XtActor> GetChangedActors() {
            return _actors != null
                       ? _actors.Where(w => w.Id > 0 && w.IsDirty)
                       : Enumerable.Empty<XtActor>();
        }

        #endregion

    }

}