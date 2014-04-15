using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Frost.PHPtoNET.Attributes;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Movie")]
    public class XjbPhpMovie {

        [PHPIgnore]
        public static readonly string[] ScrapperNames = {
            "Coretis_Scraper_Filename",
            "Coretis_Scraper_Filesystem",
            "Coretis_Scraper_Mplayer",
            "Coretis_Scraper_Xmlinfo"
        };

        /// <example>\eg{ <c>DOKU MANGA XXX MOVIE SERIE -> (S01E01 S01 staffel1 staffel.12 season folge1 folge.12 complete)</c>}</example>
        [PHPName("art")]
        public string ArtType;

        /// <summary>The episode in the series</summary>
        /// <example>\eg{ ''<c>S01E01 E01</c>''}</example>
        [PHPName("episode")]
        public string Episode;

        ///<summary>The id for this row in DB</summary>
        [PHPName("id")]
        public long Id;

        /// <summary>Languages available</summary>
        /// <example>\eg{ <c>GERMAN/DE</c> <c>ENGLISH/EN</c>}</example>
        [PHPName("languageArr")]
        public List<string> AvailableLanguage;

        /// <summary>Defines the unix timestamp of last scraper run on this object</summary>
        /// <remarks>array( 'Scraper_Name' => 'unixtimestamp')</remarks>
        /// <example>\eg{ <code>array ('Coretis_Scraper_Filename' => '946707734')</code>}</example>
        [PHPName("scraperLastRun")]
        public Dictionary<string, long> ScraperLastRunTimestamp;

        /// <summary>Language and type of the subtitles</summary>
        /// <remarks>Language or Keyword ''<c>SUBBED</c>'' if language is the text-language with undefined audio language</remarks>
        /// <example>\eg{ ''<c>GERMAN, SUBBED</c>''}</example>
        [PHPName("subtitle")]
        public string SubtitleLanguage;

        #region Type information

        /// <summary>The source of the audio</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        [PHPName("audioSource")]
        public string AudioSource;

        ///<summary>The type of the audio</summary>
        ///<example>\eg{ <c>AC3, DTS</c>}</example>
        [PHPName("audioType")]
        public string AudioType;

        ///<summary>Special addithions or types</summary>
        ///<example>\eg{ <c>INTERNAL, DUBBED, LIMITED, PROPER, REPACK, RERIP, SUBBED</c>}</example>
        [PHPName("specials")]
        public string Specials;

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, INTERLACED, LETTERBOX</c>}</example>
        [PHPName("videoFormat")]
        public string VideoResolution;

        ///<summary>With what this video was made from</summary>
        /// <example>\eg{TS TC TELESYNC CAM HDRIP DVDRIP BDRIP DTV HD2DVD HDDVDRIP HDTVRIP VHS SCREENER RECODE}</example>
        [PHPName("videoSource")]
        public string VideoSource;

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        [PHPName("videoType")]
        public string VideoType;

        #endregion

        #region mplayer extracted infos

        /// <summary>The audio channels setting.</summary>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        [PHPName("achannels")]
        public string AudioChannels;

        /// <summary>The codec of the audio is encoded in.</summary>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        [PHPName("acodec")]
        public string AudioCodec;

        /// <summary>Aspect; ratio between width and height (width / height)</summary>
        /// <example>\eg{ <c>1.333</c>}</example>
        [PHPName("aspect")]
        public double? Aspect;

        /// <summary>frame count</summary>
        [PHPName("fps")]
        public int? FPS;

        /// <summary>The height of the video in pixel.</summary>
        [PHPName("height")]
        public int? Height;

        /// <summary>The length in seconds</summary>
        [PHPName("length")]
        public double? Runtime;

        /// <summary>Gets or sets the codec of the video is encoded in.</summary>
        /// <value>The codec of the video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        [PHPName("vcodec")]
        public string VideoCodec;

        /// <summary>The width of the video in pixel.</summary>
        [PHPName("width")]
        public int? Width; // 

        #endregion

        #region info-file-paths:

        ///<example>\eg{ ''<c>/movies/Kill Bill/folder.jpg</c>''}</example>
        [PHPName("pathCover")]
        public string CoverPath;

        /// <example>\eg{ <code>array ( '/movies/Kill Bill/Kill Bill-fanart', '...' )</code>}</example>
        [PHPName("pathFanartArr")]
        public List<string> Fanart;

        /// <example>\eg{''<c>/movies/Kill Bill/Kill Bill.xml</c>''}</example>
        [PHPName("pathInfoXml")]
        public string NfoPath;

        /// <example>\eg{ <code>array ( '/movies/Kill Bill/Kill Bill-screen.jpg', '...' )</code>}</example>
        [PHPName("pathScreenArr")]
        public string[] Screens;

        /// <example>\eg{/Kill Bill/Kill Bill_xjb_sheet.jpg || /Kill Bill/Kill Bill_sheet.jpg}</example>
        [PHPName("pathSheet")]
        public string SheetPath;

        /// <example>\eg{ <code>array ( 'de' => '/movies/Kill Bill/Kill Bill.srt')</code>}</example>
        [PHPName("pathSubtitlesArr")]
        public List<string> Subtitles;

        #endregion

        #region movie data from imdb or from xml info file

        // imdb is standard - so we save extra

        /// <summary>The imdb id</summary>
        /// <example>\eg{''<c>tt0266697</c>'' is in IMDB http://www.imdb.com/title/tt0266697/ }</example>
        [PHPName("imdbId")]
        public string ImdbId;

        /// <summary>The imdb rating</summary>
        /// <remarks>10-100</remarks>
        [PHPName("imdbRating")]
        public double? ImdbRating;

        ///<summary>The movie id at a online sources</summary>
        ///<example>\eg{ <code>array ( 'imdb' => 'tt0266697', 'tmbd => '70703', 'allocine' => '60502')</code>}</example>
        [PHPName("movieOnlineIdArr")]
        public Hashtable OnlineDatabaseIds;

        // http://www.imdb.com/title/tt0266697/ http://www.themoviedb.org/movie/70703

        ///<summary>The ratings at online sources</summary>
        ///<example>\eg{ <code>array ( 'imdb' => '10', 'tmbd => '50', 'allocine' => '100')</code> from 10 to 100 (1-10 for view)}</example>
        [PHPName("ratingArr")]
        public Dictionary<string, double> Ratings;

        ///<summary> average of all scrapped ratings, if more ratings exist, use average of all aviabled</summary>
        [PHPName("ratingAverage")]
        public int RatingAverage;

        #endregion

        #region movie infos from online sources:

        /// <remarks>Keys must be ISO 3166-1 like country codes, see here: http://www.iso.org/iso/english_country_names_and_code_elements </remarks>
        /// <example>\eg{ <code>array( 'us' => 'PG-13' )</code>}</example>
        [PHPName("certificationArr")]
        public Dictionary<string, string> Certifications;

        ///<remarks>In ISO 3166-1</remarks>
        /// <example>\eg{ <code>array('de', 'us', 'uk')</code>}</example>
        [PHPName("countryArr")]
        public List<string> Countries;

        /// <example>\eg{ <code>array( Coretis_VO_Genre object1, Coretis_VO_Genre object2 )</code>}</example>
        [PHPName("genreArr")]
        public List<XjbPhpGenre> Genres;

        /// <example>\eg{ <code>array( Coretis_VO_Person object1, Coretis_VO_Person object2 )</code>}</example>
        [PHPName("personArr")]
        public List<XjbPhpPerson> Cast;

        /// <example>\eg{ <code>array( Coretis_VO_Picture object1, Coretis_VO_Picture object2 )</code>}</example>
        [PHPName("pictureArr")]
        public List<XjbPhpPicture> Art;

        /// <summary>full description</summary>
        [PHPName("plotFull")]
        public string PlotFull;

        /// <summary>a short description</summary>
        [PHPName("plotSummary")]
        public string PlotSummary;

        /// <summary>Gets or sets the name of the studio.</summary>
        ///<example>\eg{''<c>universal pictures</c>''}</example>
        [PHPName("studio")]
        public string Studio;

        /// <summary>The text right to left</summary>
        /// <remarks>1 for using right text alignment</remarks>
        [PHPName("textRightToLeft")]
        public int? TextRightToLeft;

        /// <summary>alternate movie titles</summary>
        [PHPName("titleAlternateArr")]
        public List<string> AlternateTitles;

        /// <summary>the original title of the movie</summary>
        [PHPName("titleOrg")]
        public string OriginalTitle { get; set; }

        /// <summary>the title for sorting movies alphabetical</summary>
        [PHPName("titleSort")]
        public string SortTitle;

        #endregion

        #region File info

        ///<summary>Unique ID of the drive</summary>
        ///<example>\eg{ ''<c>989e59b4c82b76f9a7c0d3db3208da87</c>''}</example>
        [PHPName("driveUniqueId")]
        public string UniqueDriveId;

        ///<summary>The File Extension without beginning point</summary>
        ///<example>\eg{ ''<c>mp4</c>'' - without beginning point}</example>
        [PHPName("fileExtension")]
        public string FileExtension;

        ///<summary>The Filename</summary>
        ///<example>\eg{ ''<c>Family.Guy.S05E08.Dei.Gesetzeshueter.German.Dubbed.FS.DVDRip.XviD-iNSPiRED.mp4</c>''}</example>
        [PHPName("fileName")]
        public string FileName;

        ///<summary>Full path to the file relative to the device</summary>
        ///<example>\eg{ ''<c>/tmp/usbmounts/sda1/movies/Family.Guy</c>''}</example>
        ///<remarks>Realy needed full path ?</remarks>
        [PHPName("filePathFull")]
        public string FullPath;

        ///<summary>Path to file relative to the drive its stored in</summary>
        ///<example>'/movies/Family.Guy'</example>
        [PHPName("filePathOnDrive")]
        public string FilePathOnDrive;

        ///<summary>File Size in Bytes (longint)</summary>
        [PHPName("fileSize")]
        public double FileSize;

        ///<summary>UNIXTIMESTAMP FileLastAccessTime</summary>
        [PHPName("fileTSaccess")]
        public string LastAccessTime;

        ///<summary>UNIXTIMESTAMP FileCreateTime</summary>
        [PHPName("fileTScreate")]
        public string FileCreateTime;

        #endregion

        #region filename extracted data

        /// <summary>The movie title in the language of the movie</summary>
        /// <example>\eg{ ''<c>Family Guy</c>''}</example>
        [PHPName("name")]
        public string Title;

        ///<example>\eg{ ''<c>Dei.Gesetzeshueter</c>''}</example>
        [PHPName("nameSub")]
        public string TranslatedName;

        /// <summary></summary>
        /// <remarks>1910 2009 (date('Y')) - not in future !</remarks>
        [PHPName("year")]
        public string ReleaseYear;

        #endregion

        public void AddSpecial(string special) {
            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(Specials, special, CompareOptions.IgnoreCase) < 0) {
                Specials = string.Join(",", Specials, special);
            }
        }

        public void RemoveSpecial(string special) {
            int idx = CultureInfo.CurrentCulture.CompareInfo.IndexOf(Specials, special, CompareOptions.IgnoreCase);
            if (idx >= 0) {
                Specials = Specials.Remove(idx, special.Length);
            }            
        }
    }

}