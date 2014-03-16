using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Models {

    public class MovieInfo {
        private static readonly ISOCountryCode Usa = ISOCountryCodes.Instance.GetByISOCode("USA");

        public MovieInfo() {
            FileInfos = new List<FileDetectionInfo>();
            Genres = new List<string>();
            Specials = new List<string>();
            Directors = new List<PersonInfo>();
            Writers = new List<PersonInfo>();
            Countries = new List<ISOCountryCode>();
            Art = new List<ArtInfo>();
            Actors = new List<ActorInfo>();
            Certifications = new List<CertificationInfo>();
            Studios = new List<string>();
            Plots = new List<PlotInfo>();
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

        public string Set { get; set; }

        public string MPAARating {
            get {
                CertificationInfo ci = Certifications.Find(c => c.Country == Usa);
                return ci == null
                    ? null
                    : ci.Rating;
            }
        }

        public List<PlotInfo> Plots { get; set; }

        public List<CertificationInfo> Certifications { get; set; }

        public List<FileDetectionInfo> FileInfos { get; set; }

        public List<string> Genres { get; set; }

        public List<string> Specials { get; set; }

        public List<string> Studios { get; set; }

        public List<PersonInfo> Writers { get; set; }

        public List<PersonInfo> Directors { get; set; }

        public List<ActorInfo> Actors { get; set; }

        public List<ArtInfo> Art { get; set; }

        public List<ISOCountryCode> Countries { get; set; }

        /// <summary>Gets the runtime sum of all the video parts in this movie in miliseconds.</summary>
        /// <returns>Full runtime sum of video parts in this movie in miliseconds.</returns>
        public void CalculateVideoRuntimeSum() {
            long l = FileInfos.SelectMany(f => f.Videos).Where(v => v.Duration.HasValue).Sum(v => v.Duration.Value);

            if (!Runtime.HasValue && l > 0) {
                Runtime = l;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Title;
        }
    }

}