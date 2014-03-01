using System.Collections;
using System.Collections.Generic;

namespace Frost.Models.Xtreamer.PHP {

    public class Coretis_VO_Movie {
        /// <example>\eg{ <c>DOKU MANGA XXX MOVIE SERIE -> (S01E01 S01 staffel1 staffel.12 season folge1 folge.12 complete)</c>}</example>
        public string art;

        /// <summary>The episode in the series</summary>
        /// <example>\eg{ ''<c>S01E01 E01</c>''}</example>
        public string episode;

        ///<summary>The id for this row in DB</summary>
        public long id;

        /// <summary>Languages available</summary>
        /// <example>\eg{ <c>GERMAN/DE</c> <c>ENGLISH/EN</c>}</example>
        public string[] languageArr;

        /// <summary>Defines the unix timestamp of last scraper run on this object</summary>
        /// <remarks>array( 'Scraper_Name' => 'unixtimestamp')</remarks>
        /// <example>\eg{ <code>array ('Coretis_Scraper_Filename' => '946707734')</code>}</example>
        public Dictionary<string, long> scraperLastRun;

        /// <summary>Language and type of the subtitles</summary>
        /// <remarks>Language or Keyword ''<c>SUBBED</c>'' if language is the text-language with undefined audio language</remarks>
        /// <example>\eg{ ''<c>GERMAN SUBBED</c>''}</example>
        public string subtitle;

        #region Type information

        /// <summary>The source of the audio</summary>
        /// <example>\eg{ <c>TS, TC, TELESYNC, CAM, HDRIP, DVDRIP, BDRIP, DTV, HD2DVD, HDDVDRIP, HDTVRIP, VHS, SCREENER, RECODE</c>}</example>
        public string audioSource;

        ///<summary>The type of the audio</summary>
        ///<example>\eg{ <c>AC3 DTS</c>}</example>
        public string audioType;

        ///<summary>Special addithions or types</summary>
        ///<example>\eg{ <c>INTERNAL, DUBBED, LIMITED, PROPER, REPACK, RERIP, SUBBED</c>}</example>
        public string specials;

        /// <summary>Resolution and format of the video</summary>
        /// <example>\eg{ <c>720p, 1080p, 720i, 1080i, PAL, HDTV, INTERLACED, LETTERBOX</c>}</example>
        public string videoFormat;

        ///<summary>With what this video was made from</summary>
        /// <example>\eg{TS TC TELESYNC CAM HDRIP DVDRIP BDRIP DTV HD2DVD HDDVDRIP HDTVRIP VHS SCREENER RECODE}</example>
        public string videoSource;

        /// <summary>The type of the video</summary>
        /// <example>\eg{ <c>XVID, DVD5, DVD9, DVDR, BLUERAY, BD, HD2DVD, X264</c>}</example>
        public string videoType;

        #endregion

        #region mplayer extracted infos

        /// <summary>The audio channels setting.</summary>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string achannels;

        /// <summary>The codec of the audio is encoded in.</summary>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string acodec;

        /// <summary>Aspect; ratio between width and height (width / height)</summary>
        /// <example>\eg{ <c>1.333</c>}</example>
        public double? aspect;

        /// <summary>frame count</summary>
        public int? fps;

        /// <summary>The height of the video in pixel.</summary>
        public int? height;

        /// <summary>The length in seconds</summary>
        public double? length;

        /// <summary>Gets or sets the codec of the video is encoded in.</summary>
        /// <value>The codec of the video is encoded in.</value>
        /// <example>\eg{ <c>WMV3 DIVX XVID H264 VP6 AVC</c>}</example>
        public string vcodec;

        /// <summary>The width of the video in pixel.</summary>
        public int? width; // 

        #endregion

        #region info-file-paths:

        ///<example>\eg{ ''<c>/movies/Kill Bill/folder.jpg</c>''}</example>
        public string pathCover;

        /// <example>\eg{ <code>array ( '/movies/Kill Bill/Kill Bill-fanart', '...' )</code>}</example>
        public string[] pathFanartArr;

        /// <example>\eg{''<c>/movies/Kill Bill/Kill Bill.xml</c>''}</example>
        public string pathInfoXml;

        /// <example>\eg{ <code>array ( '/movies/Kill Bill/Kill Bill-screen.jpg', '...' )</code>}</example>
        public string[] pathScreenArr;

        /// <example>\eg{/Kill Bill/Kill Bill_xjb_sheet.jpg || /Kill Bill/Kill Bill_sheet.jpg}</example>
        public string pathSheet;

        /// <example>\eg{ <code>array ( 'de' => '/movies/Kill Bill/Kill Bill.srt')</code>}</example>
        public string[] pathSubtitlesArr;

        #endregion

        #region movie data from imdb or from xml info file

        // imdb is standard - so we save extra

        /// <summary>The imdb id</summary>
        /// <example>\eg{''<c>tt0266697</c>'' is in IMDB http://www.imdb.com/title/tt0266697/ }</example>
        public string imdbId;

        /// <summary>The imdb rating</summary>
        /// <remarks>10-100</remarks>
        public double? imdbRating;

        ///<summary>The movie id at a online sources</summary>
        ///<example>\eg{ <code>array ( 'imdb' => 'tt0266697', 'tmbd => '70703', 'allocine' => '60502')</code>}</example>
        public Hashtable movieOnlineIdArr;

        // http://www.imdb.com/title/tt0266697/ http://www.themoviedb.org/movie/70703

        ///<summary>The ratings at online sources</summary>
        ///<example>\eg{ <code>array ( 'imdb' => '10', 'tmbd => '50', 'allocine' => '100')</code> from 10 to 100 (1-10 for view)}</example>
        public Dictionary<string, double> ratingArr;

        ///<summary> average of all scrapped ratings, if more ratings exist, use average of all aviabled</summary>
        public int ratingAverage;

        #endregion

        #region movie infos from online sources:

        /// <remarks>Keys must be ISO 3166-1 like country codes, see here: http://www.iso.org/iso/english_country_names_and_code_elements </remarks>
        /// <example>\eg{ <code>array( 'us' => 'PG-13' )</code>}</example>
        public Dictionary<string, string> certificationArr;

        ///<remarks>In ISO 3166-1</remarks>
        /// <example>\eg{ <code>array('de', 'us', 'uk')</code>}</example>
        public string[] countryArr;

        /// <example>\eg{ <code>array( Coretis_VO_Genre object1, Coretis_VO_Genre object2 )</code>}</example>
        public Coretis_VO_Genre[] genreArr;

        /// <example>\eg{ <code>array( Coretis_VO_Person object1, Coretis_VO_Person object2 )</code>}</example>
        public Coretis_VO_Person[] personArr;

        /// <example>\eg{ <code>array( Coretis_VO_Picture object1, Coretis_VO_Picture object2 )</code>}</example>
        public Coretis_VO_Picture[] pictureArr;

        /// <summary>full description</summary>
        public string plotFull;

        /// <summary>a short description</summary>
        public string plotSummary;

        /// <summary>Gets or sets the name of the studio.</summary>
        ///<example>\eg{''<c>universal pictures</c>''}</example>
        public string studio;

        /// <summary>The text right to left</summary>
        /// <remarks>1 for using right text alignment</remarks>
        public int? textRightToLeft;

        /// <summary>alternate movie titles</summary>
        public string[] titleAlternateArr;

        /// <summary>the original title of the movie</summary>
        public string titleOrg;

        /// <summary>the title for sorting movies alphabetical</summary>
        public string titleSort;

        #endregion

        #region File info

        ///<summary>Unique ID of the drive</summary>
        ///<example>\eg{ ''<c>989e59b4c82b76f9a7c0d3db3208da87</c>''}</example>
        public string driveUniqueId;

        ///<summary>The File Extension without beginning point</summary>
        ///<example>\eg{ ''<c>mp4</c>'' - without beginning point}</example>
        public string fileExtension;

        ///<summary>The Filename</summary>
        ///<example>\eg{ ''<c>Family.Guy.S05E08.Dei.Gesetzeshueter.German.Dubbed.FS.DVDRip.XviD-iNSPiRED.mp4</c>''}</example>
        public string fileName;

        ///<summary>Full path to the file relative to the device</summary>
        ///<example>\eg{ ''<c>/tmp/usbmounts/sda1/movies/Family.Guy</c>''}</example>
        ///<remarks>Realy needed full path ?</remarks>
        public string filePathFull;

        ///<summary>Path to file relative to the drive its stored in</summary>
        ///<example>'/movies/Family.Guy'</example>
        public string filePathOnDrive;

        ///<summary>File Size in Bytes (longint)</summary>
        public double fileSize;

        ///<summary>UNIXTIMESTAMP FileLastAccessTime</summary>
        public string fileTSaccess;

        ///<summary>UNIXTIMESTAMP FileCreateTime</summary>
        public string fileTScreate;

        #endregion

        #region filename extracted data

        /// <summary>The movie title in the language of the movie</summary>
        /// <example>\eg{ ''<c>Family Guy</c>''}</example>
        public string name;

        ///<example>\eg{ ''<c>Dei.Gesetzeshueter</c>''}</example>
        public string nameSub;

        /// <summary></summary>
        /// <remarks>1910 2009 (date('Y')) - not in future !</remarks>
        public string year;

        #endregion
    }

}