using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.ISO;
using RibbonUI.Design.Classes;
using RibbonUI.Design.Models;

namespace RibbonUI.Design.Fakes {

    public class FakeMovie : IMovie {
        private long _id;

        public long Id {
            get { return 11; }
        }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        public string Title { 
            get { return "Anna Karenina"; }
            set { }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        public string OriginalTitle {
            get { return "Anna Karenina"; }
            set { }
        }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        public string SortTitle {
            get { return "Anna Karenina CD1"; }
            set { }
        }

        /// <summary>Gets or sets the type of the movie.</summary>
        /// <value>The type of the movie.</value>
        /// <example>\eg{ DVD, BluRay, ...}</example>
        public MovieType Type {
            get { return MovieType.DVD; }
            set{}
        }

        /// <summary>Gets or sets the goofs.</summary>
        /// <value>The goofs.</value>
        public string Goofs { get; set; }

        /// <summary>Gets or sets the trivia.</summary>
        /// <value>The trivia.</value>
        public string Trivia { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        public long? ReleaseYear {
            get { return 2013; } 
            set{ }
        }

        /// <summary>Gets or sets the Person to Movie link with payload as in character name the person is protraying.</summary>
        /// <value>The Person to Movie link with payload as in character name the person is protraying.</value>
        public IEnumerable<IActor> Actors {
            get {
                return new List<IActor> {
                    new DesignActor { Character = "Janitor", ImdbID = "nm0603635", Name = "Zach Braff", Thumb = "http://cf2.imgobject.com/t/p/original/zCAwpjYyWnbYKmMBxd0qCEZWyuY.jpg" },
                    new DesignActor { Character = "Doctor", ImdbID = "nm0603636", Name = "John Cho", Thumb = "http://cf2.imgobject.com/t/p/original/wlA5pkKGun8BS7GjCqXrzthTOk4.jpg" },
                    new DesignActor { Character = "Patient", ImdbID = "nm0603636", Name = "Colin Farrell", Thumb = "http://cf2.imgobject.com/t/p/original/tFMWlEeZNmgG8WsWgPm4Mjd6Tgc.jpg" },
                };
            }
        }

        /// <summary>Gets or sets the date and time the movie was first shown on TV.</summary>
        /// <value>The date and time the movie was first shown on TV.</value>
        public DateTime? Aired {
            get { return new DateTime(2012, 5, 11); }
            set {  }
        }

        /// <summary>Gets or sets the movie promotional images.</summary>
        /// <value>The movie promotional images</value>
        public IEnumerable<IArt> Art {
            get { return Enumerable.Empty<IArt>(); }
        }

        /// <summary>Gets or sets the audio codec used most frequently in associated audios.</summary>
        /// <value>The audio codec used most frequently in associated audios</value>
        public string AudioCodec {
            get { return "AC3"; }
            set { }
        }

        /// <summary>Gets or sets the information about audio streams of this movie.</summary>
        /// <value>The information about audio streams of this movie</value>
        public IEnumerable<IAudio> Audios {
            get {
                return new List<IAudio> {
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = "3/2/0.1",
                        NumberOfChannels = 6,
                        ChannelPositions = "Front: L C R, Side: L R, LFE",
                        Codec = "AC3",
                        CodecId = "AC3",
                        BitRate = 437.5f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = 16,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6727631,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-2 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 54.6875f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 21,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 7401656,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-2 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 54.6875f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 21,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6655788,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = "3/2/0.1",
                        NumberOfChannels = 6,
                        ChannelPositions = "Front: L C R, Side: L R, LFE",
                        Codec = "AC3",
                        CodecId = "AC3",
                        BitRate = 375,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = 16,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 7267058,
                        Language = null,
                    },
                    new DesignAudio {
                        Source = null,
                        Type = null,
                        ChannelSetup = null,
                        NumberOfChannels = 2,
                        ChannelPositions = null,
                        Codec = "MPEG-1 Audio layer 3",
                        CodecId = "MP3",
                        BitRate = 109.375f,
                        BitRateMode = FrameOrBitRateMode.Unknown,
                        SamplingRate = 46,
                        BitDepth = null,
                        CompressionMode = CompressionMode.Lossy,
                        Duration = 6778992,
                        Language = null,
                    }
                };
            }
        }

        public IEnumerable<IAward> Awards {
            get {
                return new List<DesignAward> {
                    new DesignAward {
                        AwardType = "For best actor",
                        IsNomination = true,
                        Organization = "Oscars"
                    },
                    new DesignAward {
                        AwardType = "For best director",
                        IsNomination = false,
                        Organization = "Oscars"
                    }
                };
            }
        }

        /// <summary>Gets or sets the information about this movie's certification ratings/restrictions in certain countries.</summary>
        /// <value>The information about this movie's certification ratings/restrictions in certain countries.</value>
        public IEnumerable<ICertification> Certifications {
            get {
                return new List<ICertification> {
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("us", "usa"),
                            Name = "United States"
                        },
                        Rating = "R"
                    },
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("gb", "gbr"),
                            Name = "United Kingdom"
                        },
                        Rating = "15"
                    },
                    new DesignCertification {
                        Country = new DesignCountry {
                            ISO3166 = new ISO3166("de", "deu"),
                            Name = "Germany"
                        },
                        Rating = "16"
                    }
                };
            }
        }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public IEnumerable<ICountry> Countries {
            get { 
                return new List<ICountry> {
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "us", Alpha3 = "usa" }, Name = "United States" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "br", Alpha3 = "bra" }, Name = "Brazil" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "bg", Alpha3 = "bgr" }, Name = "Bulgaria" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "ca", Alpha3 = "can" }, Name = "Canada" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "fi", Alpha3 = "fin" }, Name = "Finland" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "fr", Alpha3 = "fra" }, Name = "France" },
                    new DesignCountry { ISO3166 = new ISO3166 { Alpha2 = "gb", Alpha3 = "gbr" }, Name = "United Kingdom" }
                };
            }
        }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        public IEnumerable<IPerson> Directors {
            get {
                return new List<IPerson> {
                    new DesignPerson { ImdbID = "nm0603633", Name = "John Lee Hancock" },
                    new DesignPerson { ImdbID = "nm0603634", Name = "Rian Johnson" }
                };
            }
        }

        /// <summary>Gets or sets the directory path to this movie.</summary>
        public string DirectoryPath {
            get { return "C:/"; }
            set { }
        }

        /// <summary>Gets or sets the DVD region of this movie or source.</summary>
        /// <value>The DVD region of this movie or source.</value>
        public DVDRegion DvdRegion {
            get { return DVDRegion.R0; }
            set { }
        }

        /// <summary>Gets or sets the movie edithion.</summary>
        /// <value>The movie edithion.</value>
        /// <example>\eg{Extended, Directors cut, Retail ...}</example>
        public string Edithion {
            get { return "Limited"; }
            set { }
        }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public IEnumerable<IGenre> Genres {
            get { 
                return new List<IGenre> {
                    new DesignGenre { Name = "Action" },
                    new DesignGenre { Name = "Adventure" },
                    new DesignGenre { Name = "Science Fiction" },
                    new DesignGenre { Name = "Thriller" },
                    new DesignGenre { Name = "Drama" },
                    new DesignGenre { Name = "Mystery" },
                    new DesignGenre { Name = "Romance" },
                    new DesignGenre { Name = "Comedy" },
                    new DesignGenre { Name = "Foreign" },
                    new DesignGenre { Name = "Fantasy" },
                    new DesignGenre { Name = "Family" },
                    new DesignGenre { Name = "Animation" },
                };
            }
        }

        /// <summary>Gets a value indicating whether this movie has available fanart.</summary>
        /// <value>Is <c>true</c> if the movie has available fanart; otherwise, <c>false</c>.</value
        public bool HasArt {
            get { return true; }
        }

        public bool HasNfo {
            get { return true; }
        }

        /// <summary>Gets a value indicating whether this movie has available subtitles.</summary>
        /// <value>Is <c>true</c> if the movie has available subtitles; otherwise, <c>false</c>.</value>
        public bool HasSubtitles {
            get { return true; }
        }

        /// <summary>Gets a value indicating whether this movie has a trailer video availale.</summary>
        /// <value>Is <c>true</c> if the movie has a trailer video available; otherwise, <c>false</c>.</value>
        public bool HasTrailer {
            get { return true; }
        }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        public string ImdbID {
            get { return "tt0936501"; }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this movie is comprised of multiple files.</summary>
        /// <value>Is <c>true</c> if the movie is comprised of multiple files; otherwise, <c>false</c>.</value>
        public bool IsMultipart {
            get { return true; }
            set {  }
        }

        /// <summary>Gets or sets the date and time the movie was last played.</summary>
        /// <value>The date and time the movie was last played.</value>
        public DateTime? LastPlayed {
            get { return DateTime.Now; }
            set {  }
        }

        /// <summary>Gets or sets the number of audio channels used most frequently in associated audios.</summary>
        /// <value>The number of audio channels used most frequently in associated audios</value>
        public int? NumberOfAudioChannels {
            get { return 6; }
            set {  }
        }

        /// <summary>Gets or sets the part types.</summary>
        /// <value>If the movie is Multipart it represents the type of the parts.</value>
        /// <example>\eg{DVD, CD, ...}</example>
        public string PartTypes {
            get { return "CD"; }
            set { }
        }

        /// <summary>Gets or sets the number of times this movie has been played.</summary>
        /// <value>The number of times this movie has been played.</value>
        public long PlayCount {
            get { return 30; }
            set {  }
        }

        /// <summary>Gets or sets this movie's story and plot with summary and a tagline.</summary>
        /// <value>This movie's story and plot with summary and a tagline</value>
        public IEnumerable<IPlot> Plots {
            get {
                return new List<IPlot> {
                    new DesignPlot {
                        Full = "Trapped in a loveless marriage, aristocrat Anna Karenina enters into a life-changing affair with the affluent Count Vronsky.",
                        Tagline = "An epic story of love.",
                        Language = "English",
                        Summary = "They bring you the news so you don't have to get it yourself."
                    },
                };
            }
        }

        /// <summary>Gets or sets the date and time the movie was first publicly shown.</summary>
        /// <value>The date and time the movie was first publicly shown.</value>
        public DateTime? Premiered {
            get { return new DateTime(2013, 12, 1); }
            set { }
        }

        public IEnumerable<IPromotionalVideo> PromotionalVideos {
            get { return Enumerable.Empty<IPromotionalVideo>(); }
        }

        /// <summary>Gets or sets the average movie rating</summary>
        /// <value>Average movie rating</value>
        public double? RatingAverage {
            get { return 5.5; }
            set { }
        }

        /// <summary>Gets or sets the information about this movie's critics and their ratings</summary>
        /// <value>The information about this movie's critics and their ratings</value>
        public IEnumerable<IRating> Ratings {
            get { return Enumerable.Empty<IRating>(); }
        }

        /// <summary>Gets or sets the date the movie was released in the cinemas.</summary>
        /// <value>The date the movie was released in the cinemas.</value>
        public DateTime? ReleaseDate {
            get { return new DateTime(2014,15,20); }
            set {  }
        }

        /// <summary>Gets or sets the release group.</summary>
        /// <value>The release group.</value>
        public string ReleaseGroup {
            get { return "DrSi"; }
            set {  }
        }

        /// <summary>Gets or sets the runtime of the movie in miliseconds</summary>
        /// <value>The runtime of the movie in miliseconds</value>
        public long? Runtime {
            get { return 6425336; }
            set {  }
        }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        public IMovieSet Set {
            get { return new DesignSet { Name = "G.I. Joe (Live-Action Series)" }; }
            set { }
        }

        /// <summary>Gets or sets the special information about this movie release.</summary>
        /// <value>The special information about this movie release</value>
        public IEnumerable<ISpecial> Specials {
            get {
                return new List<ISpecial> {
                    new DesignSpecial{ Name = "Proper"},
                    new DesignSpecial{ Name = "REQ"},
                    new DesignSpecial{ Name = "Limited"},
                };
            }
        }

        /// <summary>Gets or sets the studio(s) that produced the movie.</summary>
        /// <value>The studio(s) that produced the movie.</value>
        public IEnumerable<IStudio> Studios {
            get {
                return new List<IStudio> {
                    new DesignStudio { Name = "20th Century Fox Home entertainment" },
                    new DesignStudio { Name = "A&E" },
                    new DesignStudio { Name = "Animal Planet" },
                    new DesignStudio { Name = "Archlight Films" },
                    new DesignStudio { Name = "Castle Rock" },
                    new DesignStudio { Name = "Cinemax" },
                    new DesignStudio { Name = "Columbia" },
                };
            }
        }

        /// <summary>Gets or sets the movie subtitles.</summary>
        /// <value>The movie subtitles.</value>
        public IEnumerable<ISubtitle> Subtitles {
            get { return Enumerable.Empty<ISubtitle>(); }
        }

        /// <summary>Gets or sets The Movie Databse identifier of this movie.</summary>
        /// <value>The Movie Databse identifier of this movie.</value>
        public string TmdbID {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        public long? Top250 {
            get { return 50; }
            set {  }
        }

        /// <summary>Gets or sets the URL to the movie trailer.</summary>
        /// <value>The URL to the movie trailer.</value>
        public string Trailer {
            get { return "https://www.youtube.com/watch?v=rPGLRO3fZnQ"; }
            set { }
        }

        /// <summary>Gets or sets the video codec used most frequently in associated audios.</summary>
        /// <value>The video codec used most frequently in associated audios</value>
        public string VideoCodec {
            get { return "AVC"; }
            set { }
        }

        /// <summary>Gets or sets the video resolution used most frequently in associated audios.</summary>
        /// <value>The video resolution used most frequently in associated audios</value>
        public string VideoResolution {
            get { return "1080p"; }
            set {  }
        }

        /// <summary>Gets or sets the information about video streams of this movie.</summary>
        /// <value>The information about video streams of this movie</value>
        public IEnumerable<IVideo> Videos {
            get { return Enumerable.Empty<IVideo>(); }
        }

        /// <summary>Gets or sets a value indicating whether has beed played before.</summary>
        /// <value><c>true</c> if this movie has been played before; otherwise, <c>false</c>.</value>
        public bool Watched {
            get { return true; }
            set {  }
        }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        public IEnumerable<IPerson> Writers {
            get {
                return new List<IPerson> {
                    new DesignPerson { ImdbID = "nm0603628", Name = "Pierre Morel" },
                    new DesignPerson { ImdbID = "nm0603629", Name = "John Luessenhop" }
                };
            }
        }

        #region Add/Remove

        public IActor AddActor(IActor actor) {
            throw new NotImplementedException();
        }

        public void RemoveActor(IActor actor) {
            throw new NotImplementedException();
        }

        public IPerson AddDirector(IPerson director) {
            throw new NotImplementedException();
        }

        public void RemoveDirector(IPerson director) {
            throw new NotImplementedException();
        }

        public ISpecial AddSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public void RemoveSpecial(ISpecial special) {
            throw new NotImplementedException();
        }

        public IGenre AddGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public void RemoveGenre(IGenre genre) {
            throw new NotImplementedException();
        }

        public IPlot AddPlot(IPlot plot) {
            throw new NotImplementedException();
        }

        public void RemovePlot(IPlot plot) {
            throw new NotImplementedException();
        }

        public IStudio AddStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public void RemoveStudio(IStudio studio) {
            throw new NotImplementedException();
        }

        public ICountry AddCountry(ICountry country) {
            throw new NotImplementedException();
        }

        public void RemoveCountry(ICountry country) {
            throw new NotImplementedException();
        }

        public ISubtitle AddSubtitle(ISubtitle subtitle) {
            throw new NotImplementedException();
        }

        public void RemoveSubtitle(ISubtitle subtitle) {
            throw new NotImplementedException();
        }

        #endregion

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
    }

}