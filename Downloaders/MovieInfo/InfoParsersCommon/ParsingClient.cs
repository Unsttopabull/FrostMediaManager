using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    public abstract class ParsingClient {

        public ParsingClient(string name, bool canIndex, bool supportsMovieHash) {
            Name = name;
            CanIndex = canIndex;
            SupportsMovieHash = supportsMovieHash;
        }

        public string Name { get; private set; }

        public bool CanIndex { get; private set; }

        public bool SupportsMovieHash { get; private set; }

        public IEnumerable<ParsedMovie> AvailableMovies { get; protected set; }

        public abstract IEnumerable<ParsedMovie> GetByImdbId(string imdbId);

        public abstract IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes);

        public abstract IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear);

        public abstract void Index();
        public abstract ParsedMovieInfo ParseMovieInfo(ParsedMovie movie);

        public static HtmlDocument DownloadWebPage(string url, Encoding enc = null) {
            string html;
            using (WebClient webCl = new WebClient { Encoding = enc ?? Encoding.UTF8 }) {
                try {
                    html = webCl.DownloadString(url);
                }
                catch (WebException e) {
                    Console.Error.WriteLine(e.Message);
                    return null;
                }
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(html));
            return hd;
        }

        //public void PullAllMovieInfo() {
        //    Task[] tsk = new Task[AvailableMovies.Count];
        //    for (int i = 0; i < AvailableMovies.Count; i++) {
        //        Console.WriteLine(AvailableMovies[i]);
        //        while (tsk.Count(t => t != null && !t.IsCompleted) >= 5) {
        //            Thread.Sleep(500);
        //        }
        //        tsk[i] = ParseMovieInfo(AvailableMovies[i]);
        //    }

        //    Task.WaitAll(tsk.ToArray());
        //}
    }

}