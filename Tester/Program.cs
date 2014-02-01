using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.XML.XBMC;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.MovieInfoParsers.GremoVKino;
using Frost.PHPtoNET;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using CoretisMovie = Frost.Common.Models.PHP.Coretis_VO_Movie;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            //EntityFrameworkProfiler.Initialize();

            FileStream debugLog = File.Create("debugDeserialize.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            //SerXmlXbmcMovie(new PHPSerializer());
            //TestPHPDeserialize2();

            //TestXjbDbParser();
            time = TestMediaSearcher();
            //TestGremoVKino();

            sw.Stop();

            if (time == default(TimeSpan)) {
                time = sw.Elapsed;
            }

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + time);
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void TestGremoVKino() {
            GremoVKinoClient cli = new GremoVKinoClient();
            const string MOVIE_TITLE = "47 Ronin";
            List<GremoVKinoMovie> kinoMovies = cli.Parse(MOVIE_TITLE);
            GremoVKinoMovie kinoMovie = kinoMovies.FirstOrDefault(km => km.OriginalName == MOVIE_TITLE || km.SloveneName == MOVIE_TITLE);
            if (kinoMovie != null) {
                Task<GremoVKinoMovieInfo> movieInfo = kinoMovie.ParseMovieInfo();
                movieInfo.Wait();
            }
        }

        private static void TestXjbDbParser() {
            Process("xjb.db3");
        }

        private static void Process(string dbLoc) {
            string[] phpFilmi;
            using (XjbEntities xjb = new XjbEntities(dbLoc)) {
                phpFilmi = xjb.Movies.Select(mov => mov.MovieVo).ToArray();
            }

            int stFilmov = phpFilmi.Length;
            List<CoretisMovie> obdelaniFilmi = new List<CoretisMovie>(stFilmov);

            PHPDeserializer2 objParser = new PHPDeserializer2();

            for (int i = 0; i < stFilmov; i++) {
                phpFilmi[i] = phpFilmi[i].Replace('\n', ' ');
                File.WriteAllText(string.Format("movies/{0}.txt", i), phpFilmi[i]);

                Console.Write(@"Deserializing element with index: {0}", i);

                CoretisMovie mv;
                using (PHPSerializedStream phpStream = new PHPSerializedStream(phpFilmi[i], Encoding.UTF8)) {
                    try {
                        mv = objParser.Deserialize<CoretisMovie>(phpStream);
                    }
                    catch (Exception) {
                        Console.Write(@" ERROR");
                        continue;
                    }
                }
                Console.WriteLine();

                obdelaniFilmi.Add(mv);
            }
        }

        private static void TestPHPDeserialize2() {
            PHPDeserializer2 des2 = new PHPDeserializer2();

            XbmcXmlMovie deserialize;
            using (PHPSerializedStream phpStream = new PHPSerializedStream(File.ReadAllBytes("serOut.txt"), Encoding.UTF8)) {
                deserialize = des2.Deserialize<XbmcXmlMovie>(phpStream);
            }
        }

        private static void SerXmlXbmcMovie(PHPSerializer phpSer) {
            XbmcXmlMovie mv = new XbmcXmlMovie {
                Actors = new List<XbmcXmlActor>(new[] {
                    new XbmcXmlActor("alal", "malal", "file://c:/cd.jph"),
                    new XbmcXmlActor("blal", "nalal", "file://c:/cd.jph"),
                    new XbmcXmlActor("clal", "oalal", "file://c:/cd.jph"),
                    new XbmcXmlActor("dlal", "palal", "file://c:/cd.jph")
                }),
                Aired = DateTime.Now,
                CertificationsString = "US:PG-13",
                Countries = new List<string>(new[] { "US", "Mexico", "Canada" }),
                Credits = new List<string>(new[] { "Alfred H", "Malibu C" }),
                DateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Directors = new List<string>(new[] { "Alan C", "Norick B" }),
                FilenameAndPath = "file://c:/naodoa.jpg",
                Genres = new List<string>(new[] { "Comedy", "Adventure", "Sci-Fi" }),
                LastPlayed = DateTime.Now,
                MPAA = "Rated R",
                OriginalTitle = "Dunky",
                Outline = "dsfljasdlf dsfa",
                PlayCount = 11111111,
                Plot = "fsjlakjglajslgjasdčlgjalsdjgdas",
                Premiered = DateTime.Now,
                Rating = 9.9f,
                ReleaseDate = DateTime.Now,
                RuntimeString = "105 min",
                Set = "The Dunkys",
                SortTitle = "Dunky 1",
                Studios = new List<string>(new[] { "Fox", "WB" }),
                Tagline = "The best dunky ever",
                Title = "Dunky",
                Top250 = 1,
                Trailer = "plugin://langubga.cm",
                Votes = 2000000.ToString(CultureInfo.InvariantCulture),
                Watched = true,
                Year = DateTime.Now.Year,
                FileInfo = new XbmcXmlFileInfo {
                    StreamDetails = new XbmcStreamDetails {
                        Audio = new List<XbmcXmlAudioInfo>(new[] { new XbmcXmlAudioInfo("ac3", 6, "en") }),
                        Subtitles = new List<XbmcXmlSubtitleInfo>(new[] { new XbmcXmlSubtitleInfo("en") }),
                        Video = new List<XbmcXmlVideoInfo>(new[] { new XbmcXmlVideoInfo("xvid", 5.5, 300, 400, 30000) })
                    }
                }
            };

            string serialize = phpSer.Serialize(mv);

            File.WriteAllText("serOut.txt", serialize);
        }

        private static TimeSpan TestMediaSearcher() {
            using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
                int count = mvc.Movies.Count();
            }

            Stopwatch sw = Stopwatch.StartNew();
            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
            ms.Search();

            sw.Stop();
            return sw.Elapsed;
        }
    }

}