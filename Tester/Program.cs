using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Frost.Common.Models.FeatureDetector;
using Frost.DetectFeatures;
using Frost.InfoParsers.Models;
using Frost.Providers.Frost.DB;
using LightInject;
using log4net;
using log4net.Config;
using File = System.IO.File;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            if (File.Exists("log4Net.config")) {
                XmlConfigurator.Configure(new FileInfo("log4Net.config"));
            }
            else {
                BasicConfigurator.Configure();
            }

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            //TestTraktTv();
            //TestOpenSubtitlesOrg();
            TetsFD();

            sw.Stop();

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + sw.Elapsed);
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void TetsFD() {
            FrostDbContainer dc = new FrostDbContainer();
            DbEntityEntry dbEntityEntry = dc.Entry(new object());
        }

        //private static void TestOpenSubtitlesOrg() {
        //    OpenSubtitlesClient cli = new OpenSubtitlesClient(false);
        //    LogInInfo login = cli.LogInAnonymous("en", "Frost Media Manager v1");
        //    ImdbMovieDetailsInfo info = cli.Movie.GetImdbDetails(0088763);

        //    string serializeObject = JsonConvert.SerializeObject(info);
        //    File.WriteAllText("imdbInfo.js", serializeObject);
        //    cli.LogOut();

        //    //cli.Movie.GetImdbDetails()
        //}

        public static void TestTraktTv() {
            using (ServiceContainer service = new ServiceContainer()) {
                service.RegisterAssembly("Downloaders/Frost.MovieInfoProviders.dll");

                List<IParsingClient> parsingClients = service.GetAllInstances<IParsingClient>().ToList();
            }

            //SharpTraktTv trakt = new SharpTraktTv("dc9b6e2e5526762ae8a050780ef6d04b");
            //MovieMatch[] response = trakt.Search.SearchMovies("50/50", 5);
        }
    }

}