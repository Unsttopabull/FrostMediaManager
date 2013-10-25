using System;
using System.Collections;
using Common.Models.DB.MovieVo;

namespace Common.Models.PHP {
    public class Coretis_VO_Movie {

        ///<summary>The id for this row in DB</summary>
        public long id;

        #region File info
        ///<summary>The Filename</summary>
        ///<example>Family.Guy.S05E08.Dei.Gesetzeshueter.German.Dubbed.FS.DVDRip.XviD-iNSPiRED.mp4</example>
        public string fileName;

        ///<summary>The File Extension without beginning point</summary>
        ///<example>'mp4' - without beginning point</example>
        public string fileExtension;

        ///<summary>Full path to the file relative to the device</summary>
        ///<example>/tmp/usbmounts/sda1/movies/Family.Guy</example>
        ///<remarks>realy needed full path ?</remarks>
        public string filePathFull;

        ///<summary>Path to file relative to the drive its stored in</summary>
        ///<example>'/movies/Family.Guy'</example>
        public string filePathOnDrive;

        ///<summary>Unique ID of the drive</summary>
        ///<example>989e59b4c82b76f9a7c0d3db3208da87</example>
        public string driveUniqueId;

        ///<summary>File Size in Bytes (longint)</summary>
        public double fileSize;

        ///<summary>UNIXTIMESTAMP FileCreateTime</summary>
        public string fileTScreate;

        ///<summary>UNIXTIMESTAMP FileLastAccessTime</summary>
        public string fileTSaccess;
        #endregion

        #region filename extracted data

        /// <summary>The movie title in the language of the movie 'Family Guy'</summary>
        public string name;

        ///<example>Dei.Gesetzeshueter</example>
        public string nameSub;

        /// <summary></summary>
        /// <remarks>1910 2009 (date('Y')) - not in future !</remarks>
        public string year;

        #endregion

        /// <summary>Languages available</summary>
        /// <example>GERMAN/DE ENGLISH/EN</example>
        public Hashtable languageArr;

        /// <summary>Language and type of the subtitles</summary>
        /// <example>GERMAN SUBBED</example>
        /// <remarks>Language or Keyword "SUBBED" if language is the text-language with undefined audio language</remarks>
        public string subtitle;

        /// <example>DOKU MANGA XXX MOVIE SERIE -> (S01E01 S01 staffel1 staffel.12 season folge1 folge.12 complete)</example>
        public string art;

        /// <summary>The episode in the series</summary>
        /// <example>S01E01 E01</example>
        public string episode;

        #region Type information
        ///<summary>The type of the audio</summary>
        ///<example>AC3 DTS</example>
        public string audioType;

        /// <summary>The source of the audio</summary>
        /// <example>LD MD LINE MIC</example>
        public string audioSource;

        /// <summary>The type of the video</summary>
        /// <example>XVID DVD5 DVD9 DVDR BLUERAY BD HD2DVD X264</example>
        public string videoType;

        ///<summary>With what this video was made from</summary>
        /// <example>TS TC TELESYNC CAM HDRIP DVDRIP BDRIP DTV HD2DVD HDDVDRIP HDTVRIP VHS SCREENER RECODE</example>
        public string videoSource;

        ///<summary>Resolution and format of the video</summary>
        /// <example>720p 1080p 720i 1080i PAL HDTV INTERLACED LETTERBOX</example>
        public string videoFormat;

        ///<summary>Special addithions or types</summary>
        ///<example>INTERNAL DUBBED LIMITED PROPER REPACK RERIP SUBBED</example>
        public string specials;
        #endregion

        #region mplayer extracted infos

        /// <summary>Audio channels</summary>
        public string achannels;

        /// <example>MP3</example>
        public string acodec;

        /// <example>WMV3 DIVX XVID H264 VP6 AVC</example>
        public string vcodec;

        /// <example>1.333</example>
        public double aspect;

        public int width; // in pixel
        public int height; // in pixel
        public int length; // in seconds
        public int fps; // frame count
        //we not use:	public colordepth					= NULL;			// bit count
        #endregion

        #region info-file-paths:

        ///<example>/movies/Kill Bill/folder.jpg</example>
        public string pathCover;

        public string[] pathScreenArr; // array ( '/movies/Kill Bill/Kill Bill-screen.jpg', '...' )
        public string[] pathFanartArr; // array ( '/movies/Kill Bill/Kill Bill-fanart', '...' )

        /// <example>/movies/Kill Bill/Kill Bill.xml</example>
        public string pathInfoXml;

        /// <example></example>
        public Hashtable pathSubtitlesArr; // array ( 'de' => '/movies/Kill Bill/Kill Bill.srt')

        /// <example>/Kill Bill/Kill Bill_xjb_sheet.jpg || /Kill Bill/Kill Bill_sheet.jpg</example>
        public string pathSheet;
        #endregion

        #region movie data from imdb or from xml info file

        ///<summary> average of all scrapped ratings, if more ratings exist, use average of all aviabled</summary>
        public int ratingAverage;

        // imdb is standard - so we save extra

        /// <summary>The imdb id</summary>
        /// <example>tt0266697 - http://www.imdb.com/title/tt0266697/</example>
        public string imdbId;

        /// <summary>The imdb rating</summary>
        /// <remarks>10-100</remarks>
        public int imdbRating;

        ///<summary>The movie id at a online sources</summary>
        ///<example>array ( 'imdb' => 'tt0266697', 'tmbd => '70703', 'allocine' => '60502')</example>
        public Hashtable movieOnlineIdArr;

        // http://www.imdb.com/title/tt0266697/ http://www.themoviedb.org/movie/70703

        ///<summary>The ratings at online sources</summary>
        ///<example>array ( 'imdb' => '10', 'tmbd => '50', 'allocine' => '100') from 10 to 100 (1-10 for view)</example>
        public Hashtable ratingArr;
        #endregion

        #region movie infos from online sources:

        /// <summary>the original title of the movie</summary>
        public string titleOrg;

        /// <summary>the title for sorting movies alphabetical</summary>
        public string titleSort;

        /// <summary>alternate movie titles</summary>
        public string[] titleAlternateArr;

        /// <summary>a short description</summary>
        public string plotSummary;

        /// <summary>full description</summary>
        public string plotFull;

        public Coretis_VO_Person[] personArr; // array( Coretis_VO_Person object1, Coretis_VO_Person object2 )
        public Coretis_VO_Genre[] genreArr; // array( Coretis_VO_Genre object1, Coretis_VO_Genre object2 )

        public Hashtable certificationArr; // array( 'us' => 'PG-13' ) keys must be ISO 3166-1 like country codes, see here: http://www.iso.org/iso/english_country_names_and_code_elements

        public string[] countryArr; // array('de', 'us', 'uk')	in ISO 3166-1

        ///<summary>The studio</summary>
        ///<example>universal pictures</example>
        public string studio;

        /// <example>array( Coretis_VO_Picture object1, Coretis_VO_Picture object2 )</example>
        public Coretis_VO_Picture[] pictureArr;

        /// <summary>The text right to left</summary>
        /// <remarks>1 for using right text alignment</remarks>
        public int textRightToLeft;

        #endregion

        /// <summary>defines the unix timestamp of last scraper run on this object</summary>
        /// <example>array( 'Scraper_Name' => 'unixtimestamp') example: array ('Coretis_Scraper_Filename' => '946707734')</example>
        public Hashtable scraperLastRun;

        #region Conversion Functions
        private static Movie ConvertToMovie(Coretis_VO_Movie movie) {
            Movie mov = new Movie();

            GetMovieTitle(movie, mov);

            //za vsak najden žanr preverimo èe že obstaja in ga potem dodamo filmu
            foreach (Coretis_VO_Genre genreVo in movie.genreArr) {
                mov.Genres.Add(genreVo.name);
            }

            CheckAddNewCast(movie, mov);
            AddPlot(movie.plotFull, movie.plotSummary, mov);
            GetInfo(movie, mov);
            AddAudioVideoInfo(movie, mov);

            mov.Files.Add(new File(movie.fileName, movie.fileExtension, movie.filePathOnDrive, (long)movie.fileSize));

            AddArt(movie.pathCover, movie.pathFanartArr, movie.pathFanartArr, mov);
            return mov;
        }

        private static void GetInfo(Coretis_VO_Movie movie, Movie mov) {
            int result;
            if (int.TryParse(movie.year, out result)) {
                mov.Year = result;
            }

            mov.Subtitles.Add(new Subtitle(movie.subtitle.TrimIfNotNull())); ;
            mov.Specials = movie.specials.TrimIfNotNull();
            mov.Runtime = (movie.length > 0) ? movie.length : (long?)null;

            mov.FPS = (movie.fps == 0) ? (int?)null : movie.fps;

            mov.RatingAverage = movie.ratingAverage;
            mov.ImdbID = movie.imdbId;
            mov.Studios.Add(new Studio(movie.studio));
        }

        private static void CheckAddNewCast(Coretis_VO_Movie movie, Movie mov) {
            foreach (Coretis_VO_Person cPerson in movie.personArr) {
                if (string.Equals(cPerson.job, "actor", StringComparison.OrdinalIgnoreCase)) {
                    mov.Cast.Add((MoviePerson) new Actor(cPerson.name, cPerson.character));
                }
                else {
                    mov.Cast.Add(new MoviePerson(cPerson.name, cPerson.job));
                }
            }
        }

        private static void GetMovieTitle(Coretis_VO_Movie movie, Movie mov) {
            mov.Title = movie.name;
            if (movie.titleOrg != null) {
                mov.OriginalTitle = movie.titleOrg;
            }
            else if (movie.titleSort != null) {
                mov.OriginalTitle = movie.titleSort;
            }
            mov.SortTitle = movie.titleSort;
        }

        private static void AddPlot(string plotFull, string plotSummary, Movie mov) {
            if (string.IsNullOrEmpty(plotFull)) {
                return;
            }
            mov.Plot.Add(new Plot(plotFull, plotSummary, null));
        }

        private static void AddAudioVideoInfo(Coretis_VO_Movie movie, Movie mov) {
            mov.Audio.Add(new Audio(
                movie.audioSource,
                movie.audioType,
                movie.achannels,
                movie.acodec
            ));

            mov.Videos.Add(new Video(
                movie.vcodec,
                movie.videoFormat,
                movie.videoType,
                movie.videoSource,
                movie.aspect,
                movie.height,
                movie.width
            ));
        }

        private static void AddArt(string pathCover, string[] pathScreenArr, string[] pathFanartArr, Movie mov) {
            if (pathCover != null) {
                mov.Art.Add(new Art("Cover", pathCover));
            }

            int len = pathScreenArr.Length;
            if (len > 0) {
                for (int i = 0; i < len; i++) {
                    mov.Art.Add(new Art("Screen", pathScreenArr[i]));
                }
            }

            len = pathFanartArr.Length;
            if (len > 0) {
                for (int i = 0; i < len; i++) {
                    mov.Art.Add(new Art("Fanart", pathFanartArr[i]));
                }
            }
        }

        #endregion

        #region Conversion operators
        public static explicit operator Movie(Coretis_VO_Movie movie) {
            return ConvertToMovie(movie);
        }
        #endregion
    }
}