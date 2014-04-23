using System;
using System.Collections.Generic;
using Frost.Common.Models.FeatureDetector;

namespace Frost.Common.Models.Provider {

    public interface IMovie : IMovieEntity {

        #region Columns

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        string Title { get; set; }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        string OriginalTitle { get; set; }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        string SortTitle { get; set; }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        MovieType Type { get; set; }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        string Goofs { get; set; }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        string Trivia { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        long? ReleaseYear { get; set; }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        DateTime? ReleaseDate { get; set; }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        string Edithion { get; set; }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        DVDRegion DvdRegion { get; set; }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        DateTime? LastPlayed { get; set; }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        DateTime? Premiered { get; set; }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        DateTime? Aired { get; set; }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        string Trailer { get; set; }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        long? Top250 { get; set; }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        long? Runtime { get; set; }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        bool Watched { get; set; }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        long PlayCount { get; set; }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        double? RatingAverage { get; set; }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        string ImdbID { get; set; }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        string TmdbID { get; set; }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        string ReleaseGroup { get; set; }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        bool IsMultipart { get; set; }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        string PartTypes { get; set; }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        string DirectoryPath { get; set; }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        int? NumberOfAudioChannels { get; set; }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        string AudioCodec { get; set; }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        string VideoResolution { get; set; }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        string VideoCodec { get; set; }

        #endregion

        #region M to 1

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        IMovieSet Set { get; set; }

        /// <summary>Gets or sets the main plot.</summary>
        /// <value>The main plot.</value>
        IPlot MainPlot { get; set; }

        /// <summary>Gets or sets the default fanart to be displayed.</summary>
        /// <value>The default fanart.</value>
        IArt DefaultFanart { get; set; }

        /// <summary>Gets or sets the default cover.</summary>
        /// <value>The default cover.</value>
        IArt DefaultCover { get; set; }

        #endregion

        #region 1 to M

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        IEnumerable<ISubtitle> Subtitles { get; }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        IEnumerable<ICountry> Countries { get; }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        IEnumerable<IStudio> Studios { get; }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        IEnumerable<IVideo> Videos { get; }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        IEnumerable<IAudio> Audios { get; }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        IEnumerable<IRating> Ratings { get; }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        IEnumerable<IPlot> Plots { get; }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        IEnumerable<IArt> Art { get; }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        IEnumerable<ICertification> Certifications { get; }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        IEnumerable<IPerson> Writers { get; }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        IEnumerable<IPerson> Directors { get; }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        IEnumerable<IActor> Actors { get; }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        IEnumerable<ISpecial> Specials { get; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        IEnumerable<IGenre> Genres { get; }

        IEnumerable<IAward> Awards { get; }

        IEnumerable<IPromotionalVideo> PromotionalVideos { get; }

        #endregion

        #region Utility

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        bool HasTrailer { get; }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        bool HasSubtitles { get; }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        bool HasArt { get; }

        bool HasNfo { get; }

        #endregion

        #region Add / Remove

        /// <summary>Adds the specified actor to the provider data store.</summary>
        /// <param name="actor">The actor to add.</param>
        /// <returns>Returns the added actor. If the <paramref name="actor"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding actors or the actor does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding actors.</exception>
        IActor AddActor(IActor actor);

        /// <summary>Removes the specified actor from the provider data store.</summary>
        /// <param name="actor">The actor to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing actors in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing actors.</exception>
        bool RemoveActor(IActor actor);

        /// <summary>Adds the specified writer to the provider data store.</summary>
        /// <param name="writer">The writer to add.</param>
        /// <returns>Returns the added writer. If the <paramref name="writer"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding writers or the writers does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding writers.</exception>
        IPerson AddWriter(IPerson writer);

        /// <summary>Removes the specified writer from the provider data store.</summary>
        /// <param name="writer">The writer to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing writers in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing writers.</exception>
        bool RemoveWriter(IPerson writer);

        /// <summary>Adds the specified director to the provider data store.</summary>
        /// <param name="director">The director to add.</param>
        /// <returns>Returns the added director. If the <paramref name="director"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding directors or the director does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding directors.</exception>
        IPerson AddDirector(IPerson director);

        /// <summary>Removes the specified director from the provider data store.</summary>
        /// <param name="director">The director to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing directors in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing directors.</exception>
        bool RemoveDirector(IPerson director);

        /// <summary>Adds the specified special to the provider data store.</summary>
        /// <param name="special">The special to add.</param>
        /// <returns>Returns the added director. If the <paramref name="special"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding specials or the special does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding specials.</exception>
        ISpecial AddSpecial(ISpecial special);

        /// <summary>Removes the specified special from the provider data store.</summary>
        /// <param name="special">The special to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing specials in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing specials.</exception>
        bool RemoveSpecial(ISpecial special);

        /// <summary>Adds the specified genre to the provider data store.</summary>
        /// <param name="genre">The genre to add.</param>
        /// <returns>Returns the added genre. If the <paramref name="genre"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c>.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding genres or the genre does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding genres.</exception>
        IGenre AddGenre(IGenre genre);

        /// <summary>Removes the specified genre from the provider data store.</summary>
        /// <param name="genre">The genre to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing genres in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing genres.</exception>
        bool RemoveGenre(IGenre genre);

        /// <summary>Adds the specified plot to the provider data store.</summary>
        /// <param name="plot">The plot to add.</param>
        /// <returns>Returns the added plot. If the <paramref name="plot"/> is a duplicate it returns the existing instance in the provider store. Otherwise returns <c>null</c></returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding plots or the plot does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding plots.</exception>
        IPlot AddPlot(IPlot plot);

        /// <summary>Removes the specified genre from the provider data store.</summary>
        /// <param name="plot">The plot to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing plots in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing plots.</exception>
        bool RemovePlot(IPlot plot);

        /// <summary>Adds the specified studio to the provider data store.</summary>
        /// <param name="studio">The studio to add.</param>
        /// <returns>Returns the added studio. If the <paramref name="studio"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding studios or the studio does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding studios.</exception>
        IStudio AddStudio(IStudio studio);

        /// <summary>Removes the specified genre from the provider data store.</summary>
        /// <param name="studio">The studio to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing studio in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing studio.</exception>
        bool RemoveStudio(IStudio studio);

        /// <summary>Adds the specified award to the provider data store.</summary>
        /// <param name="award">The award to add.</param>
        /// <returns>Returns the added award. If the <paramref name="award"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding awards or the award does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding awards.</exception>
        IAward AddAward(IAward award);

        /// <summary>Removes the specified award from the provider data store.</summary>
        /// <param name="award">The award to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing awards in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing awards.</exception>
        bool RemoveAward(IAward award);

        /// <summary>Adds the specified country to the provider data store.</summary>
        /// <param name="country">The country to add.</param>
        /// <returns>Returns the added country. If the <paramref name="country"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding countries or the country does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding countries.</exception>
        ICountry AddCountry(ICountry country);

        /// <summary>Removes the specified genre from the provider data store.</summary>
        /// <param name="country">The country to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing countries in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing countries.</exception>
        bool RemoveCountry(ICountry country);

        /// <summary>Adds the specified subtitle to the provider data store.</summary>
        /// <param name="subtitle">The subtitle to add.</param>
        /// <returns>Returns the added subtitle. If the <paramref name="subtitle"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding subtitles or the subtitle does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding subtitles.</exception>
        ISubtitle AddSubtitle(ISubtitle subtitle);

        /// <summary>Removes the specified genre from the provider data store.</summary>
        /// <param name="subtitle">The country to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing countries in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing countries.</exception>
        bool RemoveSubtitle(ISubtitle subtitle);

        /// <summary>Adds the specified audio to the provider data store.</summary>
        /// <param name="audio">The audio to add.</param>
        /// <returns>Returns the added audio. If the <paramref name="audio"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding audios or the audio does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding audios.</exception>
        IAudio AddAudio(IAudio audio);

        /// <summary>Removes the specified audio from the provider data store.</summary>
        /// <param name="audio">The audio to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing audios in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing audios.</exception>
        bool RemoveAudio(IAudio audio);

        /// <summary>Adds the specified video to the provider data store.</summary>
        /// <param name="video">The video to add.</param>
        /// <returns>Returns the added video. If the <paramref name="video"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding videos or the video does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding videos.</exception>
        IVideo AddVideo(IVideo video);

        /// <summary>Removes the specified video from the provider data store.</summary>
        /// <param name="video">The video to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing videos in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing video.</exception>
        bool RemoveVideo(IVideo video);

        /// <summary>Adds the specified promotional video to the provider data store.</summary>
        /// <param name="video">The promotional video to add.</param>
        /// <returns>Returns the added promotional video. If the <paramref name="video"/> is a duplicate it returns the existing instance in the provider store.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support adding promotional videos or the promotional video does not meet a certain criteria.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented adding promotional videos.</exception>
        IPromotionalVideo AddPromotionalVideo(IPromotionalVideo video);

        /// <summary>Removes the specified promotional video from the provider data store.</summary>
        /// <param name="video">The promotional video to remove.</param>
        /// <returns>Returns true if the provider successfuly removed the item, otherwise false.</returns>
        /// <exception cref="NotSupportedException">Throws when the provider does not support removing promotional videos in a particual scenario.</exception>
        /// <exception cref="NotImplementedException">Throws when the provider has not implemented removing promotional videos.</exception>
        bool RemovePromotionalVideo(IPromotionalVideo video);

        #endregion

        /// <summary>Updates the movie with detected information.</summary>
        /// <param name="movieInfo">The detected movie information.</param>
        void Update(MovieInfo movieInfo);
    }

}