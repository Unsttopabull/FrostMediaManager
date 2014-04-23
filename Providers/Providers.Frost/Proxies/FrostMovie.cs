using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {

    public class FrostMovie : ProxyWithService<Movie, FrostMoviesDataDataService>, IMovie {

        public FrostMovie(Movie movie, FrostMoviesDataDataService service) : base(movie, service) {
        }

        #region Columns

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

        #endregion

        #region 1 to 1

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return Entity.Set; }
            set {
                Entity.Set = value != null
                                 ? Service.FindHasName<IMovieSet, Set>(value, true)
                                 : null;
            }
        }

        /// <summary>Gets or sets the default cover.</summary>
        /// <value>The default cover.</value>
        public IArt DefaultCover {
            get { return Entity.DefaultCover; }
            set {
                Entity.DefaultCover = value != null
                    ? Service.FindArt(value, true)
                    : null;   
            }
        }

        /// <summary>Gets or sets the default fanart to be displayed.</summary>
        /// <value>The default fanart.</value>
        public IArt DefaultFanart {
            get { return Entity.DefaultFanart; }
            set {
                Entity.DefaultFanart = value != null
                    ? Service.FindArt(value, true)
                    : null;   
            }
        }

        /// <summary>Gets or sets the main plot.</summary>
        /// <value>The main plot.</value>
        public IPlot MainPlot {
            get { return Entity.MainPlot; }
            set {
                if (value == null) {
                    Entity.MainPlot = null;
                }
                else {
                    Plot p = Service.FindPlot(value, true, Id);

                    if (p.MovieId == null || p.MovieId <= 0) {
                        long? id;
                        using (FrostDbContainer fdc = new FrostDbContainer()) {
                            Movie movie = fdc.Movies.Find(Id);
                            if (movie == null) {
                                return;
                            }

                            Plot item = new Plot(value);
                            movie.Plots.Add(item);
                            try {
                                fdc.SaveChanges();
                            }
                            catch {
                                return;
                            }
                            id = item.MovieId;
                        }

                        if (id.HasValue) {
                            p = Service.FindById<Plot>(id.Value);
                        }
                    }
 
                    if (p != null) {
                        Entity.MainPlot = p;
                    }
                }
            }
        }

        #endregion

        #region 1 to M

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

        #endregion

        #region Utility

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

        #endregion

        #region Add/Remove methods

        /// <summary>Adds the specified writer to the provider data store.</summary>
        /// <param name="writer">The writer to add.</param>
        /// <returns>Returns the added writer. If the <paramref name="writer"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding writers or the writers does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding writers.</exception>
        public IPerson AddWriter(IPerson writer) {
            Person p = Service.FindPerson(writer, true);
            Entity.Directors.Add(p);
            return p;
        }

        /// <summary>Removes the specified writer from the provider data store.</summary>
        /// <param name="writer">The writer to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing writers in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing writers.</exception>
        public bool RemoveWriter(IPerson writer) {
            if (writer is Person) {
                return Entity.Directors.Remove(writer as Person);
            }

            Person p = Service.FindPerson(writer, false);
            return Entity.Directors.Remove(p);
        }

        #region Actors

        public IActor AddActor(IActor actor) {
            Person p = Service.FindPerson(actor, true);

            //if the movie does not yet contain this actor add it to the collection
            Actor act = Entity.Actors.FirstOrDefault(a => a.Person == p && a.Character == actor.Character);
            if (act == null) {
                act = new Actor(p, actor.Character);
                Entity.Actors.Add(act);
            }
            return act;
        }

        public bool RemoveActor(IActor actor) {
            Person p = Service.FindPerson(actor, false);
            if (p == null) {
                return false;
            }

            return Entity.Actors.RemoveWhere(a => a.Person == p && a.Character == actor.Character) > 0;
        }

        #endregion

        #region Person
        public IPerson AddDirector(IPerson director) {
            Person p = Service.FindPerson(director, true);
            Entity.Directors.Add(p);
            return p;
        }

        public bool RemoveDirector(IPerson director) {
            if (director is Person) {
                return Entity.Directors.Remove(director as Person);
            }

            Person p = Service.FindPerson(director, false);
            return Entity.Directors.Remove(p);
        }

        #endregion

        #region Specials

        public ISpecial AddSpecial(ISpecial special) {
            Special s = Service.FindHasName<ISpecial, Special>(special, false);
            Entity.Specials.Add(s);

            return s;
        }

        public bool RemoveSpecial(ISpecial special) {
            Special s = Service.FindHasName<ISpecial, Special>(special, true);
            if (s != null) {
                return Entity.Specials.Remove(s);
            }
            return false;
        }

        #endregion

        #region Genres

        public IGenre AddGenre(IGenre genre) {
            Genre g = Service.FindHasName<IGenre, Genre>(genre, true);

            Entity.Genres.Add(g);
            return g;
        }

        public bool RemoveGenre(IGenre genre) {
            if (genre != null && !string.IsNullOrEmpty(genre.Name)) {
                return Entity.Genres.RemoveWhere(g => g.Name == genre.Name) > 0;
            }
            return false;
        }

        #endregion

        #region Plots

        public IPlot AddPlot(IPlot plot) {
            Plot p = new Plot(plot);
            return Entity.Plots.Add(p)
                ? p 
                : null;
        }

        public bool RemovePlot(IPlot plot) {
            Plot p;
            if (plot is Plot) {
                p = plot as Plot;
                bool removed = Entity.Plots.Remove(p);
                if (removed) {
                    Service.MarkAsDeleted(p);
                }
            }

            p = Service.FindPlot(plot, false, Id);
            if (p != null) {
                bool removed = Entity.Plots.Remove(p);
                if (removed && Service.MarkAsDeleted(p)) {
                    return true;
                }
                return removed;
            }

            return false;
        }

        #endregion

        #region Studio

        public IStudio AddStudio(IStudio studio) {
            Studio s = Service.FindStudio(studio, true);
            Entity.Studios.Add(s);
            return s;
        }

        public bool RemoveStudio(IStudio studio) {
            Studio s = Service.FindStudio(studio, false);
            return Entity.Studios.Remove(s);
        }

        #endregion

        #region Countries

        public ICountry AddCountry(ICountry country) {
            Country c = Service.FindCountry(country, true);
            Entity.Countries.Add(c);

            return c;
        }

        public bool RemoveCountry(ICountry country) {
            Country c = Service.FindCountry(country, false);
            return Entity.Countries.Remove(c);
        }

        #endregion

        #region Subtitles

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            Subtitle sub = Service.FindOrCreateSubtitle(subtitle, true);
            Entity.Subtitles.Add(sub);

            return new FrostSubtitle(sub, Service);
        }

        public bool RemoveSubtitle(ISubtitle subtitle) {
            Subtitle sub = Service.FindOrCreateSubtitle(subtitle, false);
            return Entity.Subtitles.Remove(sub);
        }

        #endregion

        /// <summary>Adds the specified audio to the provider data store.</summary>
        /// <param name="audio">The audio to add.</param>
        /// <returns>Returns the added audio. If the <paramref name="audio"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding audios or the audio does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding audios.</exception>
        public IAudio AddAudio(IAudio audio) {
            if (audio == null) {
                return null;
            }

            Audio a = new Audio(audio);
            Entity.Audios.Add(a);

            return new FrostAudio(a, Service);
        }

        /// <summary>Removes the specified audio from the provider data store.</summary>
        /// <param name="audio">The audio to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing audios in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing audios.</exception>
        public bool RemoveAudio(IAudio audio) {
            if (audio is FrostAudio) {
                return Entity.Audios.Remove((audio as FrostAudio).ProxiedEntity);
            }

            if (audio.Id > 0) {
                return Entity.Audios.RemoveWhere(a => a.Id == audio.Id) > 0;
            }
            return false;
        }

        /// <summary>Adds the specified video to the provider data store.</summary>
        /// <param name="video">The video to add.</param>
        /// <returns>Returns the added video. If the <paramref name="video"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding videos or the video does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding videos.</exception>
        public IVideo AddVideo(IVideo video) {
            if (video == null) {
                return null;
            }

            Video v = new Video(video);
            Entity.Videos.Add(v);

            return new FrostVideo(v, Service);
        }

        /// <summary>Removes the specified video from the provider data store.</summary>
        /// <param name="video">The video to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing videos in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing video.</exception>
        public bool RemoveVideo(IVideo video) {
            if (video is FrostVideo) {
                return Entity.Videos.Remove((video as FrostVideo).ProxiedEntity);
            }

            if (video.Id > 0) {
                return Entity.Videos.RemoveWhere(v => v.Id == video.Id) > 0;
            }
            return false;
        }

        #endregion

        public void Update(MovieInfo movieInfo) {
            
        }
    }
}
