using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Models;
using Frost.Models.Frost.DB;
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

            //FileStream debugLog = File.Create("debug.txt");
            //Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            //Debug.Listeners.Add(new ConsoleTraceListener());
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
            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
            ms.PropertyChanged += WriteCount;

            IEnumerable<MovieInfo> movies = ms.Search();
            ms.PropertyChanged -= WriteCount;

            sw.Stop();

            Console.WriteLine("Detection took: " + sw.Elapsed);

            using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
                mvc.Languages.Load();
                mvc.Specials.Load();
                mvc.Countries.Load();
                mvc.Awards.Load();
                mvc.Studios.Load();
                mvc.Sets.Load();
                mvc.People.Load();

                foreach (MovieInfo movie in movies) {
                    //mvc.Save(movie);
                    //mvc.ChangeTracker.DetectChanges();
                }

                mvc.SaveChanges();
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