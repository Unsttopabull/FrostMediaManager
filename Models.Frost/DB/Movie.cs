using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Model.Xbmc.DB;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;
using Frost.Models.Xtreamer.DB;

namespace Frost.Models.Frost.DB {

    /// <summary>Represents an information about a movie in the library.</summary>
    public partial class Movie : IMovie {
        /// <summary>Separator between multiple genres, certifications, person names ...</summary>
        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="Movie"/> class.</summary>
        public Movie() {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Audios = new HashSet<Audio>();
            Ratings = new HashSet<Rating>();
            Plots = new HashSet<Plot>();
            Art = new HashSet<Art>();
            Certifications = new HashSet<Certification>();
            Genres = new HashSet<Genre>();
            Awards = new HashSet<Award>();
            Videos = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
            Countries = new HashSet<Country>();
            Studios = new HashSet<Studio>();
            Specials = new HashSet<Special>();

            Directors = new HashSet<Person>();
            Writers = new HashSet<Person>();
            Actors = new HashSet<Actor>();
            PromotionalVideos = new HashSet<PromotionalVideo>();

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary> Initializes a new instance of the <see cref="Movie" /> class.</summary>
        /// <param name="movie">The value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public Movie(IMovie movie) {
            //Contract.Requires<ArgumentNullException>(movie.Subtitles != null, "Movie Subtitle collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Countries != null, "Movie Country collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Studios != null, "Movie Studio collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Videos != null, "Movie Video collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Audios != null, "Movie Audio collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Ratings != null, "Movie Rating collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Plots != null, "Movie Plot collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Art != null, "Movie Art collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Certifications != null, "Movie Certification collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Writers != null, "Movie Writer collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Directors != null, "Movie Director collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Actors != null, "Movie Actor collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Specials != null, "Movie Special collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Genres != null, "Movie Genre collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.Awards != null, "Movie Award collection must be initialized");
            //Contract.Requires<ArgumentNullException>(movie.PromotionalVideos != null, "Movie Promotional video collection must be initialized");

            Title = movie.Title;
            OriginalTitle = movie.Title;
            SortTitle = movie.Title;
            Type = movie.Type;
            Goofs = movie.Goofs;
            Trivia = movie.Trivia;
            ReleaseYear = movie.ReleaseYear;
            ReleaseDate = movie.ReleaseDate;
            Edithion = movie.Edithion;
            DvdRegion = movie.DvdRegion;
            LastPlayed = movie.LastPlayed;
            Premiered = movie.Premiered;
            Aired = movie.Aired;
            Trailer = movie.Trailer;
            Top250 = movie.Top250;
            Runtime = movie.Runtime;
            Watched = movie.Watched;
            PlayCount = movie.PlayCount;
            RatingAverage = movie.RatingAverage;
            ImdbID = movie.ImdbID;
            TmdbID = movie.TmdbID;
            ReleaseGroup = movie.ReleaseGroup;
            IsMultipart = movie.IsMultipart;
            PartTypes = movie.PartTypes;
            DirectoryPath = movie.DirectoryPath;
            NumberOfAudioChannels = movie.NumberOfAudioChannels;
            AudioCodec = movie.AudioCodec;
            VideoResolution = movie.VideoResolution;
            VideoCodec = movie.VideoCodec;

            if (movie.Set != null) {
                Set = new Set(movie.Set);
            }

            Subtitles = new HashSet<Subtitle>(movie.Subtitles.Select(s => new Subtitle(s)));
            Countries = new HashSet<Country>(movie.Countries.Select(s => new Country(s)));
            Studios = new HashSet<Studio>(movie.Studios.Select(s => new Studio(s)));
            Videos = new HashSet<Video>(movie.Videos.Select(s => new Video(s)));
            Audios = new HashSet<Audio>(movie.Audios.Select(s => new Audio(s)));
            Ratings = new HashSet<Rating>(movie.Ratings.Select(s => new Rating(s)));
            Plots = new HashSet<Plot>(movie.Plots.Select(s => new Plot(s)));
            Art = new HashSet<Art>(movie.Art.Select(s => new Art(s)));
            Certifications = new HashSet<Certification>(movie.Certifications.Select(s => new Certification(s)));
            Writers = new HashSet<Person>(movie.Writers.Select(s => new Person(s)));
            Directors = new HashSet<Person>(movie.Directors.Select(s => new Person(s)));
            Actors = new HashSet<Actor>(movie.Actors.Select(s => new Actor(s)));
            Specials = new HashSet<Special>(movie.Specials.Select(s => new Special(s)));
            Genres = new HashSet<Genre>(movie.Genres.Select(s => new Genre(s)));
            Awards = new HashSet<Award>(movie.Awards.Select(s => new Award(s)));
            PromotionalVideos = new HashSet<PromotionalVideo>(movie.PromotionalVideos.Select(s => new PromotionalVideo(s)));
        }

        #region Properties/Columns

        /// <summary>Gets or sets the database movie Id.</summary>
        /// <value>The database movie Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        [Required]
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
        public MovieType Type { get; set; }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs { get; set; }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear { get; set; }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion { get; set; }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion { get; set; }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed { get; set; }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered { get; set; }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired { get; set; }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer { get; set; }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 { get; set; }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime { get; set; }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched { get; set; }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount { get; set; }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage { get; set; }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID { get; set; }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID { get; set; }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup { get; set; }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart { get; set; }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes { get; set; }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath { get; set; }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels { get; set; }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec { get; set; }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution { get; set; }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec { get; set; }

        #endregion

        #region Foreign Keys

        /// <summary>Gets or sets the Set foreign key.</summary>
        /// <value>The Set foreign key.</value>
        public long? SetId { get; set; }

        #endregion

        #region Relation tables

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public virtual Set Set { get; set; }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public virtual ICollection<Subtitle> Subtitles { get; set; }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public virtual ICollection<Country> Countries { get; set; }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public virtual ICollection<Studio> Studios { get; set; }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public virtual ICollection<Video> Videos { get; set; }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public virtual ICollection<Audio> Audios { get; set; }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public virtual ICollection<Rating> Ratings { get; set; }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public virtual ICollection<Plot> Plots { get; set; }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public virtual ICollection<Art> Art { get; set; }
        
        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public virtual ICollection<Certification> Certifications { get; set; }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public virtual ICollection<Person> Writers { get; set; }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public virtual ICollection<Person> Directors { get; set; }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public virtual ICollection<Actor> Actors { get; set; }
        
        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public virtual ICollection<Special> Specials { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public virtual ICollection<Genre> Genres { get; set; }
        
        public virtual ICollection<Award> Awards { get; set; }

        public virtual ICollection<PromotionalVideo> PromotionalVideos { get; set; }

        #endregion

        #region IMovie

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        IMovieSet IMovie.Set {
            get { return Set; }
            set {
                Set = value != null
                          ? new Set(value)
                          : null;
            }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        IEnumerable<ISubtitle> IMovie.Subtitles {
            get { return Subtitles; }
        }

        public void Remove(ISubtitle subtitle) {
            Subtitle sub = Subtitles.FirstOrDefault(s => s.Equals(subtitle));
            if (sub != null) {
                Subtitles.Remove(sub);
            }
        }

        public ISubtitle Add(ISubtitle subtitle) {
            Subtitle item = new Subtitle(subtitle);
            Subtitles.Add(item);

            return item;
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        IEnumerable<ICountry> IMovie.Countries {
            get { return Countries; }
        }

        public void Remove(ICountry country) {
            Country cntry = Countries.FirstOrDefault(c => c.Equals(country));
            if (cntry != null) {
                Countries.Remove(cntry);
            }
        }

        public ICountry Add(ICountry country) {
            if (country == null) {
                return null;
            }

            Country item = new Country(country);
            Countries.Add(item);

            return item;
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        IEnumerable<IStudio> IMovie.Studios {
            get { return Studios; }
        }

        public void Remove(IStudio studio) {
            Studio stud = Studios.FirstOrDefault(c => c == studio);
            if (studio != null) {
                Studios.Remove(stud);
            }
        }

        public IStudio Add(IStudio studio) {
            Studio item = new Studio(studio);
            Studios.Add(item);

            return item;
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        IEnumerable<IVideo> IMovie.Videos {
            get { return Videos; }
        }


        public void Remove(IVideo video) {
            Video vid = Videos.FirstOrDefault(v => v == video);
            if (vid != null) {
                Videos.Remove(vid);
            }
        }

        public IVideo Add(IVideo video) {
            Video v = new Video(video);
            Videos.Add(v);

            return v;
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        IEnumerable<IAudio> IMovie.Audios {
            get { return Audios; }
        }

        public void Remove(IAudio audio) {
            Audio aud = Audios.FirstOrDefault(v => v == audio);
            if (aud != null) {
                Audios.Remove(aud);
            }
        }

        public IAudio Add(IAudio audio) {
            Audio a = new Audio(audio);

            Audios.Add(a);
            return a;
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        IEnumerable<IRating> IMovie.Ratings {
            get { return Ratings; }
        }

        public void Remove(IRating rating) {
            Rating aud = Ratings.FirstOrDefault(v => v == rating);
            if (aud != null) {
                Ratings.Remove(aud);
            }
        }

        public IRating Add(IRating rating) {
            Rating r = new Rating(rating);
            Ratings.Add(r);

            return r;
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        IEnumerable<IPlot> IMovie.Plots {
            get { return Plots; }
        }


        public void Remove(IPlot plot) {
            Plot aud = Plots.FirstOrDefault(v => v == plot);
            if (aud != null) {
                Plots.Remove(aud);
            }
        }

        public IPlot Add(IPlot plot) {
            Plot p = new Plot(plot);
            Plots.Add(p);

            return p;
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        IEnumerable<IArt> IMovie.Art {
            get { return Art; }
        }

        public void Remove(IArt art) {
            Art aud = Art.FirstOrDefault(v => v == art);
            if (aud != null) {
                Art.Remove(aud);
            }
        }

        public IArt Add(IArt art) {
            Art a = new Art(art);

            Art.Add(a);
            return a;
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        IEnumerable<ICertification> IMovie.Certifications {
            get { return Certifications; }
        }


        public void Remove(ICertification certification) {
            Certification aud = Certifications.FirstOrDefault(v => v == certification);
            if (aud != null) {
                Certifications.Remove(aud);
            }
        }

        public ICertification Add(ICertification certification) {
            Certification cert = new Certification(certification);

            Certifications.Add(cert);
            return cert;
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        IEnumerable<IPerson> IMovie.Writers {
            get { return Writers; }
        }

        public void Remove(IPerson person, PersonType type) {
            Person aud;
            switch (type) {
                case PersonType.Director:
                    aud = Directors.FirstOrDefault(p => p == person);
                    if (aud != null) {
                        Directors.Remove(aud);
                    }
                    break;
                case PersonType.Writer:
                    aud = Writers.FirstOrDefault(p => p == person);
                    if (aud != null) {
                        Writers.Remove(aud);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public IPerson Add(IPerson person, PersonType type) {
            Person p = new Person(person);
            switch (type) {
                case PersonType.Director:
                    Directors.Add(p);
                    break;
                case PersonType.Writer:
                    Writers.Add(p);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return p;
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        IEnumerable<IPerson> IMovie.Directors {
            get { return Directors; }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        IEnumerable<IActor> IMovie.Actors {
            get { return Actors; }
        }

        public void Remove(IActor actor) {
            Actor aud = Actors.FirstOrDefault(v => v == actor);
            if (aud != null) {
                Actors.Remove(aud);
            }
        }

        public IActor Add(IActor actor) {
            Actor a = new Actor(actor);

            Actors.Add(a);
            return a;
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        IEnumerable<ISpecial> IMovie.Specials {
            get { return Specials; }
        }

        public void Remove(ISpecial special) {
            Special aud = Specials.FirstOrDefault(v => v == special);
            if (aud != null) {
                Specials.Remove(aud);
            }
        }

        public ISpecial Add(ISpecial special) {
            Special s = new Special(special);

            Specials.Add(s);
            return s;
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        IEnumerable<IGenre> IMovie.Genres {
            get { return Genres; }
        }

        public void Remove(IGenre genre) {
            Genre aud = Genres.FirstOrDefault(v => v == genre);
            if (aud != null) {
                Genres.Remove(aud);
            }
        }

        public IGenre Add(IGenre genre) {
            Genre g = new Genre(genre);

            Genres.Add(g);
            return g;
        }

        IEnumerable<IAward> IMovie.Awards {
            get { return Awards; }
        }

        public void Remove(IAward award) {
            Award aud = Awards.FirstOrDefault(v => v == award);
            if (aud != null) {
                Awards.Remove(aud);
            }
        }

        public IAward Add(IAward award) {
            Award a = new Award(award);

            Awards.Add(a);
            return a;
        }

        IEnumerable<IPromotionalVideo> IMovie.PromotionalVideos {
            get { return PromotionalVideos; }
        }

        public void Remove(IPromotionalVideo promotionalVideo) {
            PromotionalVideo aud = PromotionalVideos.FirstOrDefault(v => v == promotionalVideo);
            if (aud != null) {
                PromotionalVideos.Remove(aud);
            }
        }

        public IPromotionalVideo Add(IPromotionalVideo promotionalVideo) {
            PromotionalVideo pv = new PromotionalVideo(promotionalVideo);

            PromotionalVideos.Add(pv);
            return pv;
        }

        #endregion

        #region Utility Functions / Properties

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return !string.IsNullOrEmpty(Trailer); }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return Subtitles.Count != 0; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        public bool HasArt {
            get { return Art.Count != 0; }
        }

        public bool HasNfo {
            get {
                if (DirectoryPath == null) {
                    return false;
                }
                return Directory.EnumerateFiles(DirectoryPath, "*.nfo").Any();
            }
        }

        /// <summary>Gets the file size summed from all the movie files.</summary>
        /// <returns>The movie file size in bytes summed from all its files</returns>
        private long GetFileSizeSum() {
            long sumA = Audios.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);
            long sumV = Videos.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);
            long sumS = Subtitles.Where(f => f.File != null && f.File.Size.HasValue).Sum(f => f.File.Size.Value);

            return sumA + sumV + sumS;
        }

        /// <summary>Gets the file size in pretty printed format formatted.</summary>
        /// <returns>A string with pretty printed movie file size</returns>
        /// <example>\eg{ <c>1024</c> is <c>1 Kb</c>}</example>
        public string GetFileSizeFormatted() {
            return GetFileSizeSum().FormatFileSizeAsString();
        }

        /// <summary>Gets the genre names in a formatted string.</summary>
        /// <returns>The genre names in a single string separated with " / "</returns>
        /// <example>\eg{ <c>"Horor / SciFi"</c>}</example>
        public string GetGenreNames() {
            return String.Join(SEPARATOR, Genres.Select(g => g.Name));
        }

        /// <summary>Gets the director names in a formatted string.</summary>
        /// <returns>The director names in a single string separated with " / "</returns>
        public string GetDirectorNames() {
            string directorsJoin = String.Join(SEPARATOR, Directors.Select(d => d.Name));
            return String.IsNullOrEmpty(directorsJoin)
                       ? null
                       : directorsJoin;
        }

        /// <summary>Gets the cover image path name.</summary>
        /// <returns>The path to the fist cover image</returns>
        public string GetCoverPath() {
            Art cover = Art.FirstOrDefault(a => a.Type == ArtType.Cover);
            return (cover != null)
                       ? cover.Path
                       : null;
        }

        /// <summary>Gets the studio names in a formatted string.</summary>
        /// <returns>The studio names in a single string separated with " / "</returns>
        /// <example>\eg{ ''<c>MGM / Fox</c>''}</example>
        public string GetStudioNamesFormatted() {
            return String.Join(SEPARATOR, Studios.Select(stud => stud.Name));
        }

        /// <summary>Gets the names of the studios that produced this movie in an Array.</summary>
        /// <returns>An array of studios that produced the movie</returns>
        public IEnumerable<string> GetStudioNames() {
            return Studios.Select(s => s.Name);
        }

        ///// <summary>Gets the movie actors as an <see cref="IEnumerable{T}"/> of <see cref="Common.Models.XML.Jukebox.XjbXmlActor">XjbXmlActor</see> instances.</summary>
        ///// <returns>An <see cref="IEnumerable{T}"/> of this movie actors as <see cref="Common.Models.XML.Jukebox.XjbXmlActor">XjbXmlActor</see> instances</returns>
        //public IEnumerable<XjbXmlActor> GetXjbXmlActors() {
        //    return ActorsLink.Select(a => (XjbXmlActor) (Actor) a);
        //}

        /// <summary>Gets the runtime sum of all the video parts in this movie in miliseconds.</summary>
        /// <returns>Full runtime sum of video parts in this movie in miliseconds.</returns>

        #endregion
        public override string ToString() {
            return String.Format("{0} ({1})", Title, ReleaseYear);
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return MemberwiseClone();
        }

        #region Serialization

        ///// <summary>Serializes an instance of <see cref="Movie"/> as xml in a system specified by a parameter <c><paramref name="system"/></c>.</summary>
        ///// <param name="system">The system to serialize to.</param>
        ///// <param name="xmlSaveLocation">Where to save the serialized xml.</param>
        ///// <exception cref="ArgumentOutOfRangeException">Throws if the <c><paramref name="system"/></c> is out of range (has an unknown enmum value)</exception>
        //public void Serialize(NFOSystem system, string xmlSaveLocation) {
        //    switch (system) {
        //        case NFOSystem.Xtreamer:
        //            ((XjbXmlMovie) this).Serialize(xmlSaveLocation);
        //            break;
        //        case NFOSystem.XBMC:
        //            ((XbmcXmlMovie) this).Serialize(xmlSaveLocation);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("system");
        //    }
        //}

        ///// <summary>Deserialize an instance of <see cref="Movie"/> from <c>xml</c> in a system specified by a parameter <paramref name="system"/>.</summary>
        ///// <param name="system">The system from which to deserialize from.</param>
        ///// <param name="xmlLocation">Filepath to the serialized xml.</param>
        ///// <exception cref="ArgumentOutOfRangeException">Throws if the <c><paramref name="system"/></c> is out of range (has an unknown enmum value)</exception>
        ///// <exception cref="System.IO.FileNotFoundException">Throws if the file specified with <c><paramref name="xmlLocation"/></c> can't be found</exception>
        //public Movie Load(NFOSystem system, string xmlLocation) {
        //    switch (system) {
        //        case NFOSystem.Xtreamer:
        //            return XjbXmlMovie.LoadAsMovie(xmlLocation);
        //        case NFOSystem.XBMC:
        //            return XbmcXmlMovie.LoadAsMovie(xmlLocation);
        //        default:
        //            throw new ArgumentOutOfRangeException("system");
        //    }
        //}

        #endregion

        #region Conversion Functions

        ///// <summary>Converts this instance to and instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see>.</summary>
        ///// <returns>An instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see> converted from this instance.</returns>
        //public XjbXmlMovie ToXjbXmlMovie() {
        //    return (XjbXmlMovie) this;
        //}

        ///// <summary>Converts this instance to and instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see>.</summary>
        ///// <returns>An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see> converted from this instance.</returns>
        //public XbmcXmlMovie ToXbmcXmlMovie() {
        //    return (XbmcXmlMovie) this;
        //}

        /// <summary>Converts this instance to and instance of <see cref="XbmcMovie">XbmcMovie</see>.</summary>
        /// <returns>An instance of <see cref="XbmcMovie">XbmcMovie</see> converted from this instance.</returns>
        public XbmcMovie ToXbmcMovie() {
            return (XbmcMovie) this;
        }

        /// <summary>Converts this instance to and instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see>.</summary>
        /// <returns>An instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see> converted from this instance.</returns>
        public XjbMovie ToXjbMovie() {
            return (XjbMovie) this;
        }

        #endregion

        #region Conversion Operators

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="Common.Models.DB.Jukebox">XjbMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XjbMovie(Movie movie) {
            XjbMovie xm = new XjbMovie();

            return xm;
        }

        ///// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see>.</summary>
        ///// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        ///// <returns>An instance of <see cref="Common.Models.XML.Jukebox.XjbXmlMovie">XjbXmlMovie</see> converted from <see cref="Movie"/></returns>
        //public static explicit operator XjbXmlMovie(Movie movie) {
        //    return new XjbXmlMovie {
        //        Certifications = movie.Certifications.ToArray(),
        //        Director = movie.GetDirectorNames(),
        //        GenreString = movie.GetGenreNames(),
        //        ImdbId = movie.ImdbID,
        //        OriginalTitle = movie.OriginalTitle,
        //        //Outline = movie.MainPlot.Summary,
        //        //Plot = movie.MainPlot.Full,
        //        AverageRating = (float) (movie.RatingAverage ?? 0),
        //        //TODO: CHECK FOR CORECT FORMAT
        //        ReleaseDate = movie.ReleaseDate.HasValue ? movie.ReleaseDate.Value.ToString(CultureInfo.InvariantCulture) : null,
        //        Runtime = movie.Runtime.HasValue
        //                      ? (movie.Runtime / 60)
        //                      : 0,
        //        SortTitle = movie.Title,
        //        Studio = movie.GetStudioNamesFormatted(),
        //        //Tagline = movie.MainPlot.Summary,
        //        Title = movie.Title,
        //        Year = (int) (movie.ReleaseYear ?? 0),
        //        Actors = movie.GetXjbXmlActors().ToArray(),
        //        MPAA = movie.MPAARating,
        //        Credits = String.Join(SEPARATOR, movie.Writers.Select(p => p.Name)),
        //        Fileinfo = ""
        //    };
        //}

        ///// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see>.</summary>
        ///// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        ///// <returns>An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlMovie">XbmcXmlMovie</see> converted from <see cref="Movie"/></returns>
        //public static explicit operator XbmcXmlMovie(Movie movie) {
        //    throw new NotImplementedException();
        //}

        /// <summary>Converts an instance of <see cref="Movie"/> to and instance of <see cref="XbmcMovie">XbmcMovie</see>.</summary>
        /// <param name="movie">The <see cref="Movie"/> instance to convert.</param>
        /// <returns>An instance of <see cref="XbmcMovie">XbmcMovie</see> converted from <see cref="Movie"/></returns>
        public static explicit operator XbmcMovie(Movie movie) {
            throw new NotImplementedException();
        }

        #endregion
    }

}