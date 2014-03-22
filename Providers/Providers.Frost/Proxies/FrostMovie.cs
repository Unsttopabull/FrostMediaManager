using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {

    public class FrostMovie : ProxyBase<Movie>, IMovie {

        public FrostMovie(Movie movie, FrostMoviesDataDataService service) : base(movie, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Title":
                    case "OriginalTitle":
                    case "SortTitle":
                    case "Type":
                    case "Goofs":
                    case "Trivia":
                    case "ReleaseYear":
                    case "ReleaseDate":
                    case "Edithion":
                    case "DvdRegion":
                    case "LastPlayed":
                    case "Premiered":
                    case "Aired":
                    case "Trailer":
                    case "Top250":
                    case "Runtime":
                    case "Watched":
                    case "PlayCount":
                    case "RatingAverage":
                    case "ImdbID":
                    case "TmdbID":
                    case "ReleaseGroup":
                    case "IsMultipart":
                    case "PartTypes":
                    case "DirectoryPath":
                    case "NumberOfAudioChannels":
                    case "AudioCodec":
                    case "VideoResolution":
                    case "VideoCodec":
                    case "Countries":
                    case "Studios":
                    case "Videos":
                    case "Audios":
                    case "Ratings":
                    case "Plots":
                    case "Art":
                    case "Certifications":
                    case "Writers":
                    case "Directors":
                    case "Actors":
                    case "Specials":
                    case "Genres":
                    case "Awards":
                    case "PromotionalVideos":
                    case "HasTrailer":
                    case "HasSubtitles":
                    case "HasArt":
                    case "HasNfo":
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
            get { return Entity.Title; }
            set { Entity.Title = value; }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return Entity.OriginalTitle; }
            set { Entity.OriginalTitle = value; }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return Entity.SortTitle; }
            set { Entity.SortTitle = value; }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return Entity.Type; }
            set { Entity.Type = value; }
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs {
            get { return Entity.Goofs; }
            set { Entity.Goofs = value; }
        }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia {
            get { return Entity.Trivia; }
            set { Entity.Trivia = value; }
        }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get { return Entity.ReleaseYear; }
            set { Entity.ReleaseYear = value; }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate {
            get { return Entity.ReleaseDate; }
            set { Entity.ReleaseDate = value; }
        }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion {
            get { return Entity.Edithion; }
            set { Entity.Edithion = value; }
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion {
            get { return Entity.DvdRegion; }
            set { Entity.DvdRegion = value; }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed {
            get { return Entity.LastPlayed; }
            set { Entity.LastPlayed = value; }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return Entity.Premiered; }
            set { Entity.Premiered = value; }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return Entity.Aired; }
            set { Entity.Aired = value; }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get { return Entity.Trailer; }
            set { Entity.Trailer = value; }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get { return Entity.Top250; }
            set { Entity.Top250 = value; }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return Entity.Runtime; }
            set { Entity.Runtime = value; }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched {
            get { return Entity.Watched; }
            set { Entity.Watched = value; }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount {
            get { return Entity.PlayCount; }
            set { Entity.PlayCount = value; }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return Entity.RatingAverage; }
            set { Entity.RatingAverage = value; }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return Entity.ImdbID; }
            set { Entity.ImdbID = value; }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID {
            get { return Entity.TmdbID; }
            set { Entity.TmdbID = value; }
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup {
            get { return Entity.ReleaseGroup; }
            set { Entity.ReleaseGroup = value; }
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart {
            get { return Entity.IsMultipart; }
            set { Entity.IsMultipart = value; }
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes {
            get { return Entity.PartTypes; }
            set { Entity.PartTypes = value; }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return Entity.DirectoryPath; }
            set { Entity.DirectoryPath = value; }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get { return Entity.NumberOfAudioChannels; }
            set { Entity.NumberOfAudioChannels = value; }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return Entity.AudioCodec; }
            set { Entity.AudioCodec = value; }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return Entity.VideoResolution; }
            set { Entity.VideoResolution = value; }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return Entity.VideoCodec; }
            set { Entity.VideoCodec = value; }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return Entity.Set; }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get { return Entity.Subtitles.Select(s => new FrostSubtitle(s, Service)); }
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
            get { return Entity.Videos.Select(v => new FrostVideo(v, Service)); }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get { return Entity.Audios.Select(a => new FrostAudio(a, Service)); }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return Entity.Ratings; }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get { return Entity.Plots; }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return Entity.Art; }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get { return Entity.Certifications.Select(c => new FrostCertification(c, Service)); }
        }

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

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get { return Entity.Specials; }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { return Entity.Genres; }
        }

        public IEnumerable<IAward> Awards {
            get { return Entity.Awards; }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return Entity.PromotionalVideos; }
        }

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return Entity.HasTrailer; }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return Entity.HasSubtitles; }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value>
        public bool HasArt {
            get { return Entity.HasArt; }
        }

        public bool HasNfo {
            get { return Entity.HasNfo; }
        }

        #region Add/Remove methods

        #region Actors

        public IActor AddActor(IActor actor) {
            Actor a = Service.FindOrCreateActor(actor, true);
            Entity.Actors.Add(a);

            return a;
        }

        public void RemoveActor(IActor actor) {
            Actor a = Service.FindOrCreateActor(actor, false);
            Entity.Actors.Remove(a);
        }

        #endregion

        #region Person
        public IPerson AddDirector(IPerson director) {
            Person p = Service.FindOrCreatePerson(director, true);
            Entity.Directors.Add(p);
            return p;
        }

        public void RemoveDirector(IPerson director) {
            Person p = Service.FindOrCreatePerson(director, false);
            Entity.Directors.Remove(p);
        }

        #endregion

        #region Specials

        public ISpecial AddSpecial(ISpecial special) {
            Special s = Service.FindHasName<ISpecial, Special>(special, false);
            Entity.Specials.Add(s);

            return s;
        }

        public void RemoveSpecial(ISpecial special) {
            Special s = Service.FindHasName<ISpecial, Special>(special, true);
            if (s != null) {
                Entity.Specials.Remove(s);
            }
        }

        #endregion

        #region Genres

        public IGenre AddGenre(IGenre genre) {
            Genre g = Service.FindHasName<IGenre, Genre>(genre, false);
            Entity.Genres.Add(g);
            return g;
        }

        public void RemoveGenre(IGenre genre) {
            Genre g = Service.FindHasName<IGenre, Genre>(genre, true);
            if (g != null) {
                Entity.Genres.Remove(g);
            }
        }

        #endregion

        #region Plots

        public IPlot AddPlot(IPlot plot) {
            Plot p = Service.FindOrCreatePlot(plot, true);
            Entity.Plots.Add(p);
            return p;
        }

        public void RemovePlot(IPlot plot) {
            Plot p = Service.FindOrCreatePlot(plot, false);
            Entity.Plots.Remove(p);
        }

        #endregion

        #region Studio

        public IStudio AddStudio(IStudio studio) {
            Studio s = Service.FindStudio(studio, true);
            Entity.Studios.Add(s);
            return s;
        }

        public void RemoveStudio(IStudio studio) {
            Studio s = Service.FindStudio(studio, false);
            Entity.Studios.Remove(s);
        }

        #endregion

        #region Countries

        public ICountry AddCountry(ICountry country) {
            Country c = Service.FindOrCreateCountry(country, true);
            Entity.Countries.Add(c);

            return c;
        }

        public void RemoveCountry(ICountry country) {
            Country c = Service.FindOrCreateCountry(country, false);
            Entity.Countries.Remove(c);
        }

        #endregion

        #region Subtitles

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            Subtitle sub = Service.FindOrCreateSubtitle(subtitle, true);
            Entity.Subtitles.Add(sub);

            return new FrostSubtitle(sub, Service);
        }

        public void RemoveSubtitle(ISubtitle subtitle) {
            Subtitle sub = Service.FindOrCreateSubtitle(subtitle, false);
            Entity.Subtitles.Remove(sub);
        }

        #endregion

        #endregion
    }
}
