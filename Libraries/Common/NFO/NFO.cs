using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common.Models.Provider;
using Frost.Common.NFO.Art;
using Frost.Common.NFO.Files;
using Frost.Common.Util.ISO;

namespace Frost.Common.NFO {

    /// <summary>General XBMC NFO metadata file generator</summary>
    public static class NFO {

        /// <summary>Creates a general NFO metadata object ready to be written to a file.</summary>
        /// <param name="movie">The movie for which to build a NFO metadata file.</param>
        /// <returns>Returns the NFO metadata object or <c>null</c> if unsuccessfull.</returns>
        public static NfoMovie FromIMovie(IMovie movie) {
            NfoMovie nfo = new NfoMovie();

            nfo.Actors = GetNfoActors(movie.Actors);
            nfo.Countries = GetHasName(movie.Countries);
            nfo.Certifications = GetNfoCertifications(nfo, movie.Certifications);
            nfo.Directors = GetHasName(movie.Directors);
            nfo.Fanart = GetNfoFanart(movie.Art);
            nfo.FileInfo = GetNfoFileInfo(movie);
            nfo.Genres = GetHasName(movie.Genres);
            nfo.ImdbId = movie.ImdbID;
            nfo.OriginalTitle = movie.OriginalTitle;

            GetPlot(nfo, movie.MainPlot, movie.Plots);

            if (movie.RatingAverage.HasValue) {
                nfo.Rating = (float) movie.RatingAverage;
            }

            if (movie.Runtime.HasValue) {
                nfo.RuntimeInSeconds = (long?) (movie.Runtime / 1000.0);
            }

            if (movie.Set != null) {
                nfo.Set = movie.Set.Name;
            }

            nfo.SortTitle = movie.SortTitle;
            nfo.Studios = GetHasName(movie.Studios);
            nfo.Thumbs = GetThumbs(movie.Art);
            nfo.Title = movie.Title;
            nfo.TmdbId = movie.TmdbID;

            if (movie.Top250.HasValue) {
                nfo.Top250 = (int) movie.Top250;
            }

            nfo.Trailer = movie.Trailer;
            nfo.Watched = movie.Watched;
            if (movie.ReleaseYear.HasValue) {
                nfo.Year = (int) movie.ReleaseYear;
            }

            return nfo;
        }

        private static NfoThumb[] GetThumbs(IEnumerable<IArt> art) {
            if (art == null) {
                return null;
            }

            return art.Where(a => a != null && a.Type != ArtType.Fanart)
                      .Select(thumb => new NfoThumb(thumb.Path, thumb.Type.ToString().ToLowerInvariant(), thumb.Preview))
                      .ToArray();
        }

        private static void GetPlot(NfoMovie nfo, IPlot mainPlot, IEnumerable<IPlot> plots) {
            IPlot plot = null;

            if (mainPlot != null) {
                plot = mainPlot;
            }
            else if(plots != null){
                plot = plots.FirstOrDefault();
            }

            if (plot == null) {
                return;
            }

            nfo.Plot = !string.IsNullOrEmpty(plot.Full) ? plot.Full : plot.Summary;
            nfo.Tagline = plot.Tagline;
            nfo.Outline = plot.Summary;
        }

        private static List<string> GetHasName(IEnumerable<IHasName> names) {
            return names != null
                ? names.Where(g  => g != null).Select(g => g.Name).ToList()
                : null;
        }

        private static NfoFileInfo GetNfoFileInfo(IMovie movie) {
            NfoFileInfo file = new NfoFileInfo();

            if (movie.Audios != null) {
                if (movie.Audios.Any()) {
                    file.Audios = new List<NfoAudioInfo>();
                }

                foreach (IAudio audio in movie.Audios.Where(a => a != null)) {
                    file.Audios.Add(new NfoAudioInfo(audio));
                }
            }

            if (movie.Videos != null) {
                if (movie.Videos.Any()) {
                    file.Videos = new List<NfoVideoInfo>();
                }

                foreach (IVideo video in movie.Videos.Where(v => v != null)) {
                    file.Videos.Add(new NfoVideoInfo(video));
                }
            }

            if (movie.Subtitles != null) {
                if (movie.Subtitles.Any()) {
                    file.Subtitles = new List<NfoSubtitleInfo>();
                }

                foreach (ISubtitle subtitle in movie.Subtitles.Where(subtitle => subtitle != null)) {
                    file.Subtitles.Add(new NfoSubtitleInfo(subtitle));
                }
            }

            return file;
        }

        private static NfoFanart GetNfoFanart(IEnumerable<IArt> art) {
            if (art == null) {
                return null;
            }

            NfoFanart fanart = new NfoFanart {
                Thumbs = art.Where(a => a != null && !string.IsNullOrEmpty(a.Path))
                            .Select(a => new NfoThumb(a.Path, null, a.Preview))
                            .ToList()
            };

            return fanart;
        }

        private static NfoCertification[] GetNfoCertifications(NfoMovie nfo, IEnumerable<ICertification> certifications) {
            if (certifications == null) {
                return null;
            }

            List<NfoCertification> certs = new List<NfoCertification>();
            foreach (ICertification cert in certifications) {
                if (cert.Country == null) {
                    continue;
                }

                string country = null;

                if (cert.Country.ISO3166 != null) {
                    if (!string.IsNullOrEmpty(cert.Country.ISO3166.Alpha3)) {
                        if (cert.Country.ISO3166.Alpha3.Equals("usa", StringComparison.OrdinalIgnoreCase)) {
                            nfo.MPAA = cert.Rating;
                            continue;
                        }
                        country = cert.Country.ISO3166.Alpha3;
                    }
                    else if (!string.IsNullOrEmpty(cert.Country.ISO3166.Alpha2)) {
                        if (cert.Country.ISO3166.Alpha2.Equals("us", StringComparison.OrdinalIgnoreCase)) {
                            nfo.MPAA = cert.Rating;
                            continue;
                        }
                        country = cert.Country.ISO3166.Alpha2;
                    }
                }

                if (cert.Country.Name.Equals("United States", StringComparison.InvariantCultureIgnoreCase)) {
                    nfo.MPAA = cert.Rating;

                    ISOCountryCode isoCountryCode = ISOCountryCodes.Instance.GetByEnglishName(cert.Country.Name);
                    if (isoCountryCode != null) {
                        country = isoCountryCode.Alpha3;
                    }
                }

                if (country != null) {
                    certs.Add(new NfoCertification(country, cert.Rating));
                }
            }

            return certs.ToArray();
        }

        private static List<NfoActor> GetNfoActors(IEnumerable<IActor> actors) {
            if (actors == null) {
                return null;
            }

            return actors.Where(a => a != null && !string.IsNullOrEmpty(a.Name))
                         .Select(a => new NfoActor(a.Name, a.Character, a.Thumb))
                         .ToList();
        }
    }

}