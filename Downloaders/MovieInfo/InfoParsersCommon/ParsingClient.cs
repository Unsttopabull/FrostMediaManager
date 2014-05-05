using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Frost.InfoParsers.Models.Info;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    public abstract class ParsingClient : IParsingClient {

        protected ParsingClient(string name, bool canIndex, bool supportsMovieHash, bool supportsImdbId) {
            Name = name;
            CanIndex = canIndex;
            SupportsMovieHash = supportsMovieHash;
            IsImdbSupported = supportsImdbId;
        }

        public string Name { get; private set; }

        public Uri Icon { get; protected set; }

        public bool CanIndex { get; private set; }

        public bool SupportsMovieHash { get; private set; }
        public bool IsImdbSupported { get; private set; }
        public bool IsTitleSupported { get; private set; }
        public bool IsTmdbSupported { get; private set; }

        public IEnumerable<ParsedMovie> AvailableMovies { get; protected set; }

        public abstract IEnumerable<ParsedMovie> GetByImdbId(string imdbId);

        public abstract IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes);

        public abstract IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear);

        public abstract void Index();
        public abstract ParsedMovieInfo ParseMovieInfo(ParsedMovie movie);

        protected static HtmlDocument DownloadWebPage(string url, Encoding enc = null) {
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

        protected static string GetAssemblyCurrentDirectory() {
            try {
                 return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }
            catch {
                return null;
            }            
        }
    }

}