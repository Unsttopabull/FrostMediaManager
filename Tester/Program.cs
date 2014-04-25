using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using log4net;
using log4net.Config;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            if (System.IO.File.Exists("log4Net.config")) {
                XmlConfigurator.Configure(new FileInfo("log4Net.config"));
            }
            else {
                BasicConfigurator.Configure();
            }

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            TestOmdbApi();
            //TestFanartTv();

            sw.Stop();

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + sw.Elapsed);
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void TestOmdbApi() {
            //List<OmdbSearch> omdbSearches = SharpOmdbClient.Search("50/50", 2011).ToList();
        }

        //private static void TestFanartTv() {
        //    FanartTvArtClient cli = new FanartTvArtClient();
        //    IParsedArts movieArtFromImdbId = cli.GetMovieArtFromImdbId("tt0499549");
        //}

        //private static void TestOSubInfoParser() {
        //    OpenSubtitlesInfoClient cli = new OpenSubtitlesInfoClient();
        //    IEnumerable<ParsedMovie> movies = cli.GetByMovieHash(new[] { "51b739e60b4e3ce" });


        //}
    }

}