using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Models;
using Frost.Providers.Frost;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xbmc.Provider;
using CoretisMovie = Frost.Providers.Xtreamer.PHP.Coretis_VO_Movie;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            //EntityFrameworkProfiler.Initialize();

            FileStream debugLog = File.Create("debug.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            //SerXmlXbmcMovie(new PHPSerializer());
            //TestPHPDeserialize2();

            //TestXjbDbParser();
            TestMediaSearcher();
            //TestDataService();
            //TestXbmcContext();

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

        private static void TestXbmcContext() {
            using (XbmcContainer xbmc = new XbmcContainer()) {
                XbmcMovie xbmcMovie = xbmc.Movies.Include("Path").Single(m => m.Id == 323);
                XbmcPath xbmcPath = xbmcMovie.Path;

                //IQueryable<XbmcPath> queryable = from m in xbmc.Movies where m.Id == 323 select m.Path;
                //List<XbmcPath> list = queryable.ToList();
            }
        }

        private static void TestDataService() {
            IMoviesDataService service = new XbmcMoviesDataService();
            IEnumerable<IMovie> movies = service.Movies;
            IEnumerable<XbmcPath> xbmcPaths = movies.Select(m => ((XbmcMovie) m).Path);
            int count = xbmcPaths.Count(p => p == null);

            IEnumerable<IMovieSet> movieSets = service.Sets;
        }

        private static TimeSpan TestMediaSearcher() {
            Stopwatch sw = Stopwatch.StartNew();

            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
            ms.PropertyChanged += WriteCount;

            IEnumerable<MovieInfo> movies = ms.Search();
            ms.PropertyChanged -= WriteCount;

            sw.Stop();

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