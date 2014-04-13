using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using Frost.Common.Models.FeatureDetector;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.Providers.Frost.DB;
using Frost.Providers.Xbmc;
using Frost.Providers.Xbmc.DB;
using Frost.Providers.Xtreamer.DB;

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
            //TestMediaSearcher();
            TestHasher();
            //TestPhpDeserializeAttribute();
            //TestXjbDB();
            //WriteOutMovies();
            //TestDbCheck();
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

        private static void TestHasher() {
            //using (XbmcContainer xbmc = new XbmcContainer()) {
            //    using (StreamWriter sw = File.CreateText("out.txt")) {

            //        xbmc.Paths.Load();

            //        foreach (XbmcPath path in xbmc.Paths) {
            //            string hash = XbmcHasher.Hash(path.FolderPath);

            //            sw.WriteLine("{0},{1},{2}", path.FolderPath, path.Hash, hash);
            //        }
            //    }
            //}

            //string hash = XbmcHasher.Hash("123456789");
        }

        private static void WriteOutMovies() {
            using (StreamWriter sw = new StreamWriter(File.Create("out.cs"))) {
                using (FrostDbContainer db = new FrostDbContainer()) {
                    List<Movie> movies = db.Movies
                                           .Include("Actors")
                                           .Include("Genres")
                                           .Include("Countries")
                                           .Include("Directors")
                                           .Include("Plots")
                                           .Include("Art")
                                           .Take(5)
                                           .ToList();


                    PropertyInfo[] properties = typeof(Movie).GetProperties();
                    foreach (Movie movie in movies) {
                        sw.WriteLine("new DesignMovie {");

                        foreach (PropertyInfo prop in properties) {
                            WriteProperty(prop, movie, sw);
                        }

                        sw.WriteLine("},");
                    }
                }
            }
        }

        private static void WriteProperty(PropertyInfo prop, object movie, StreamWriter sw, int indent = 1) {
            const string FORMAT = "{0}{1} = {2}, ";
            if (prop.PropertyType == typeof(string)) {
                string stringVal = (string) prop.GetValue(movie);
                if (stringVal == null) {
                    sw.WriteLine(FORMAT, string.Join("", Enumerable.Repeat("\t", indent)), prop.Name, "null");
                    return;
                }
                sw.WriteLine("{0}{1} = \"{2}\", ", string.Join("", Enumerable.Repeat("\t", indent)), prop.Name, stringVal);
                return;
            }

            if (prop.PropertyType == typeof(bool)) {
                bool boolVal = (bool) prop.GetValue(movie);

                sw.WriteLine(FORMAT, string.Join("", Enumerable.Repeat("\t", indent)), prop.Name, boolVal ? "true" : "false");
                return;
            }

            if (prop.PropertyType.GetInterface("IEnumerable") != null) {
                WriteEnumerable(prop, movie, sw, indent);
                return;
            }

            sw.WriteLine(FORMAT, string.Join("", Enumerable.Repeat("\t", indent)), prop.Name, prop.GetValue(movie) ?? "null");
        }

        private static void WriteEnumerable(PropertyInfo property, object mov, StreamWriter sw, int indent) {
            string tabs = string.Join("", Enumerable.Repeat("\t", indent));
            sw.WriteLine("{0}{1} = {{ ", tabs, property.Name);

            foreach (var item in (IEnumerable) property.GetValue(mov)) {
                Type type = item.GetType();
                string tabs2 = string.Join("", new[] { tabs, "\t" });
                sw.WriteLine("{0}new {1}{{", tabs2, type.Name);

                foreach (PropertyInfo prop in type.GetProperties()) {
                    WriteProperty(prop, item, sw, indent + 1);
                }

                sw.WriteLine("{0}}},", tabs2);
            }
            sw.WriteLine("{0}}},", tabs);
        }

        //private static void TestPhpDeserializeAttribute() {
        //    string serialized = File.ReadAllText("string.php", Encoding.UTF8);

        //    PHPDeserializer2 deser = new PHPDeserializer2();
        //    XjbPhpMovie mv;
        //    using (PHPSerializedStream phpSerialized = new PHPSerializedStream(serialized, Encoding.UTF8)) {
        //        mv = deser.Deserialize<XjbPhpMovie>(phpSerialized);
        //    }

        //    if (mv != null) {
        //        Console.WriteLine(mv.OriginalTitle);
        //    }
        //}

        public static void TestXjbDB() {
            //using (XjbEntities xjb = new XjbEntities(@"C:\Users\Martin\Desktop\SQLite DBs\xjb.db3")) {
            using (XjbEntities xjb = new XjbEntities(@"\\\\MYXTREAMER\Xtreamer_PRO\sda1\scripts\Xtreamering\var\db\xjb.db")) {
                xjb.Genres.Load();
            }
        }

        private static TimeSpan TestMediaSearcher() {
            Stopwatch sw = Stopwatch.StartNew();

            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI"); //, @"F:\Torrenti\FILMI");
            ms.PropertyChanged += WriteCount;

            List<MovieInfo> movies = ms.Search().ToList();
            ms.PropertyChanged -= WriteCount;

            sw.Stop();

            Console.WriteLine("Detection took: " + sw.Elapsed);

            int count = movies.Count;
            XbmcContainer container = new XbmcContainer("xbmc.db3");
            using (StreamWriter fw = new StreamWriter(File.Create("xbmcSave.log"))) {

                foreach (MovieInfo movieInfo in movies) {
                    Console.WriteLine(movieInfo.Title);
                    XbmcMovieSaver sv = new XbmcMovieSaver(movieInfo, container);

                    XbmcDbMovie xbmcDbMovie;
                    try {
                         xbmcDbMovie = sv.Save(true);
                    }
                    catch (Exception e) {
                        fw.WriteLine(movieInfo.Title);
                        //fw.WriteLine();

                        //JsonSerializer jsonSerializer = JsonSerializer.Create();
                        //jsonSerializer.Serialize(fw, movieInfo);

                        //fw.WriteLine("#################################");
                    }
                }
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