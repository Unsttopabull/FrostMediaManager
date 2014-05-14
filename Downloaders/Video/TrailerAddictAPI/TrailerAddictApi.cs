using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Frost.SharpTrailerAddictAPI.Models;

namespace Frost.SharpTrailerAddictAPI {

    /// <summary>The client for communicating with TralerAddict.NET API</summary>
    public static class TrailerAddictApi {
        private const string API_URL = "http://api.traileraddict.com/?{0}={1}{2}{3}";
        private static readonly Type TrailersType = typeof(Trailers);

        /// <summary>Gets the specified number of currently featured trailers with specified embed width.</summary>
        /// <param name="width">The width of embed videos.</param>
        /// <param name="count">The count of featured trailers to return.</param>
        /// <returns>Returns the currently featured trailers with specified embed width.</returns>
        public static Trailers GetFeatured(int count = 1, int width = 480) {
            return Download(string.Format(API_URL, "featured", true, count > 1 ? "&count=" + count : null, width == -1 ? null : width.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>Gets the specified number of videos with specified embed width for the movie with <paramref name="movieTitle"/> title.</summary>
        /// <param name="movieTitle">The title of the movie to search videos for.</param>
        /// <param name="width">The width of embed videos.</param>
        /// <param name="count">The count of videos to return.</param>
        /// <returns>Returns the specified number of trailers set width for the <paramref name="movieTitle"/> movie.</returns>
        public static Trailers GetMovieVideosByTitle(string movieTitle, int count = 1, int width = 480) {
            movieTitle = movieTitle.Replace("'", "");

            string format = string.Format(API_URL, "film", WebUtility.UrlEncode(movieTitle), count > 1 ? "&count=" + count : null, width <= 0 ? null : "&width=" + width.ToString(CultureInfo.InvariantCulture));
            return Download(format);
        }

        /// <summary>Gets the specified number of videos with specified embed width for the movie with <paramref name="imdbId"/>.</summary>
        /// <param name="imdbId">The Imdb movie Id (without leading 'tt').</param>
        /// <param name="width">The width of embed videos.</param>
        /// <param name="count">The count of videos to return.</param>
        /// <returns>Returns the specified number of trailers set width for the movie with <paramref name="imdbId"/>.</returns>
        public static Trailers GetMovieVideosByImdbId(string imdbId, int count = 1, int width = 480) {
            return Download(string.Format(API_URL, "imdb",
                                imdbId, 
                                count > 1 ? "&count=" + count : null,
                                width == -1 ? null : width.ToString(CultureInfo.InvariantCulture)
                            )
                    );
        }

        /// <summary>Gets the specified number of videos with specified embed width for the actor with <paramref name="actorName"/> name.</summary>
        /// <param name="actorName">The name of the actor to search videos for.</param>
        /// <param name="width">The width of embed videos.</param>
        /// <param name="count">The count of videos to return.</param>
        /// <returns>Returns the specified number of trailers set width for the <paramref name="actorName"/> actor.</returns>
        public static Trailers GetActorVideos(string actorName, int count = 1, int width = 480) {
            return Download(string.Format(API_URL, "actor",
                                WebUtility.UrlEncode(actorName), 
                                count > 1 ? "&count=" + count : null,
                                width == -1 ? null : width.ToString(CultureInfo.InvariantCulture)
                            )
                    );
        }

        private static Trailers Download(string uri) {
            using (WebClient wc = new WebClient()) {
                string xml = wc.DownloadString(uri);

                if (string.IsNullOrEmpty(xml)) {
                    return null;
                }

                Trailers deserialize;
                try {
                    XmlSerializer xs = new XmlSerializer(TrailersType);
                    deserialize = (Trailers) xs.Deserialize(new StringReader(xml));
                }
                catch (Exception e) {
                    throw new TrailerAddictException("Error parsing response", e);
                }

                return deserialize;
            }
        }
    }

}