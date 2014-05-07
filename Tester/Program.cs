﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Frost.InfoParsers;
using Frost.InfoParsers.Models.Info;
using Frost.Providers.Frost.DB;
using Frost.SharpOpenSubtitles;
using Frost.SharpOpenSubtitles.Models.Search;
using Frost.SharpOpenSubtitles.Models.Session;
using LightInject;
using log4net;
using log4net.Config;
using File = System.IO.File;
using SubtitleInfo = Frost.SharpOpenSubtitles.Models.Search.SubtitleInfo;

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
            OpenSubtitlesClient cli = new OpenSubtitlesClient(false);

            LogInInfo status = cli.LogIn(null, null, "en", "Frost Media Manager v1");
            if (status.Status != "200 OK") {
                return;
            }

            SearchSubtitleInfo movieHashInfo;
            try {
                SubtitleImdbLookupInfo lookupinfo = new SubtitleImdbLookupInfo("1276104", "eng");

                movieHashInfo = cli.Subtitle.Search(new[] { lookupinfo });
            }
            finally {
                cli.LogOut();
            }

            SubtitleInfo[] subs = movieHashInfo.Data;
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