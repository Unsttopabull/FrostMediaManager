using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Frost.Common.Models;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Models;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Files;
using Newtonsoft.Json;
using FileVo = Frost.Models.Frost.DB.Files.File;
using CoretisMovie = Frost.Models.Xtreamer.PHP.Coretis_VO_Movie;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            //EntityFrameworkProfiler.Initialize();

            FileStream debugLog = System.IO.File.Create("debug.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            //SerXmlXbmcMovie(new PHPSerializer());
            //TestPHPDeserialize2();

            //TestXjbDbParser();
            TestMediaSearcher();
            //TestIntefaces();

            //TestFileFeatures();
            //TestGremoVKino();
            //TestDBInsert();
            //TestISOMount();
            //TestDiscUtils();
            //TestImDisk();
            //OutputPo();

            sw.Stop();

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + sw.Elapsed);
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void TestIntefaces() {
            HashSet<Plot> p = new HashSet<Plot>();

            HashSet<IPlot> p2 = new HashSet<IPlot>(p);
            p2.Add(new Plot());
        }

        private static TimeSpan TestMediaSearcher() {
            //using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
            //    int count = mvc.Movies.Count();
            //}

            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<MovieInfo> movies;
            //if (!System.IO.File.Exists("detectedMovies.js")) {
                FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
                ms.PropertyChanged += WriteCount;

                movies = ms.Search();
                ms.PropertyChanged -= WriteCount;

                sw.Stop();

                Console.WriteLine("Detection took: " + sw.Elapsed);

                //JsonSerializer jser = new JsonSerializer();

                //using (JsonWriter jw = new JsonTextWriter(System.IO.File.CreateText("detectedMovies.js"))) {
                //    jser.Serialize(jw, movies);
                //}
            //}
            //else {
            //    JsonSerializer jser = new JsonSerializer();

            //    using (JsonReader jw = new JsonTextReader(System.IO.File.OpenText("detectedMovies.js"))) {
            //        movies = jser.Deserialize<IEnumerable<MovieInfo>>(jw);
            //    }
            //}

            IEnumerable<MovieInfo> videoInfos = movies.Where(m => m.FileInfos.All(f => f.Videos.Count == 0 && !f.Extension.Equals("iso", StringComparison.OrdinalIgnoreCase)));
            IEnumerable<MovieInfo> subtitlesInfos = movies.Where(m => m.FileInfos.All(f => f.Subtitles.Count == 0));
            IEnumerable<MovieInfo> audioInfos = movies.Where(m => m.FileInfos.All(f => f.Audios.Count == 0 && !f.Extension.Equals("iso", StringComparison.OrdinalIgnoreCase)));

            using (Frost.Models.Frost.MovieSaver sv = new Models.Frost.MovieSaver(movies)) {
                sv.Save();
            }

            return sw.Elapsed;
        }

        private static void WriteCount(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "Count") {
                Console.WriteLine(((FeatureDetector) sender).Count);
            }
        }
    }

}