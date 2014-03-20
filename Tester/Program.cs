using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.DetectFeatures.Models;
using Frost.ProcessDatabase;
using Frost.Providers.Frost;
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
            //TestPhpDeserializeAttribute();
            TestXjbDB();
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

        public static void TestDbCheck() {
            //Shares.LDAP();
           
            string xjb = DBCheck.FindDB(DBSystem.Xtreamer);
            string findXjbDriveLocation = DBCheck.FindXjbDriveLocation(xjb);
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