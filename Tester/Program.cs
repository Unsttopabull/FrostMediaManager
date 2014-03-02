using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Models;
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

        private static TimeSpan TestMediaSearcher() {
            //using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
            //    int count = mvc.Movies.Count();
            //}

            Stopwatch sw = Stopwatch.StartNew();

            IEnumerable<MovieInfo> movies;
            //if (System.IO.File.Exists("movies.json")) {
            //    JsonSerializer ser = new JsonSerializer();

            //    using (JsonTextReader jtr = new JsonTextReader(System.IO.File.OpenText("movies.json"))) {
            //        MovieInfo[] movies2 = ser.Deserialize<MovieInfo[]>(jtr);
            //    }
            //}
            //else {
                FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
                ms.PropertyChanged += WriteCount;

                movies = ms.Search();
                ms.PropertyChanged -= WriteCount;

                sw.Stop();

            //    JsonSerializer jser = new JsonSerializer();
            //    using (JsonTextWriter jsw = new JsonTextWriter(System.IO.File.CreateText("movies.json"))) {
            //        jser.Serialize(jsw, movies);
            //    }
            //}

            Console.WriteLine("Detection took: " + sw.Elapsed);

            using (MovieSaver sv = new MovieSaver(movies)) {
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