using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    public abstract class ParsingClient {

        public ParsingClient(string name) {
            Name = name;
        }

        public string Name { get; set; }

        public IEnumerable<ParsedMovie> AvailableMovies { get; protected set; }

        public abstract void Parse();
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