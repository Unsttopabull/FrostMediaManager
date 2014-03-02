using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.Models;
using Frost.Model.Xbmc.NFO;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private static readonly ISOCountryCode Usa = ISOCountryCodes.Instance.GetByISOCode("USA");

        private void GetXbmcNfoInfo(string fileName, FileInfo[] xbmcNfo) {
            FileInfo sameName = xbmcNfo.FirstOrDefault(fi => fi.Name.Equals(fileName + ".nfo"));
            if (sameName != null) {
                GetXbmcNfo(sameName.FullName);
                return;
            }

            FileInfo movieNfo = xbmcNfo.FirstOrDefault(fi => fi.Name.Equals("movie.nfo", StringComparison.OrdinalIgnoreCase));
            if (movieNfo != null) {
                GetXbmcNfo(movieNfo.FullName);
            }
        }

        private void GetXbmcNfo(string filePath) {
            XbmcXmlMovie xbmcMovie = null;
            try {
                xbmcMovie = XbmcXmlMovie.Load(filePath);
            }
            catch (Exception) {
                //Console.Error.WriteLine(string.Format("File \"{0}\" is not a valid NFO.", filePath), "ERROR");
            }

            if (xbmcMovie != null) {
                if (_nfoPriority == NFOPriority.OnlyNotDetected) {
                    AddNotDetectedNfoInfo(xbmcMovie);
                }
                else {
                    OverrideDetectedInfoWithNfo(xbmcMovie);
                }
            }
        }

        private DateTime FilterDate(DateTime dt) {
            if(dt != default(DateTime) && dt != new DateTime(1601,1, 1)) {
                return dt;
            }
            return default(DateTime);
        }

        private void OverrideDetectedInfoWithNfo(XbmcXmlMovie xbmcMovie) {
            Movie.Title = xbmcMovie.Title ?? Movie.Title;
            Movie.OriginalTitle = xbmcMovie.OriginalTitle ?? Movie.OriginalTitle;
            Movie.SortTitle = xbmcMovie.SortTitle ?? Movie.SortTitle;
            Movie.ImdbID = xbmcMovie.ImdbId ?? Movie.ImdbID;
            Movie.ReleaseYear = CheckReleaseYear(xbmcMovie.Year) ? xbmcMovie.Year : Movie.ReleaseYear;
            Movie.Top250 = xbmcMovie.Top250 != default(int) ? xbmcMovie.Top250 : Movie.Top250;
            Movie.PlayCount = xbmcMovie.PlayCount != default(int) ? xbmcMovie.PlayCount : Movie.PlayCount;
            Movie.ReleaseDate = xbmcMovie.ReleaseDate != default(DateTime) ? xbmcMovie.ReleaseDate : Movie.ReleaseDate;
            Movie.LastPlayed = xbmcMovie.LastPlayed != default(DateTime) ? FilterDate(xbmcMovie.LastPlayed) : Movie.LastPlayed;

            if(!string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                CertificationInfo mpaa = Movie.Certifications.FirstOrDefault(c => c.Country == Usa);
                if (mpaa != null) {
                    mpaa.Rating = xbmcMovie.MPAA;
                }
            }

            AddActors(xbmcMovie.Actors, true);

            AddNfoMovieCommon(xbmcMovie);
        }

        private void AddNotDetectedNfoInfo(XbmcXmlMovie xbmcMovie) {
            Movie.Title = Movie.Title ?? xbmcMovie.Title;
            Movie.OriginalTitle = Movie.OriginalTitle ?? xbmcMovie.OriginalTitle;
            Movie.SortTitle = Movie.SortTitle ?? xbmcMovie.SortTitle;
            Movie.ImdbID = Movie.ImdbID ?? xbmcMovie.ImdbId;
            Movie.ReleaseYear = CheckReleaseYear(Movie.ReleaseYear) ? Movie.ReleaseYear : xbmcMovie.Year;
            Movie.Top250 = Movie.Top250 != default(int) ? Movie.Top250 : xbmcMovie.Top250;
            Movie.PlayCount = Movie.PlayCount != default(int) ? Movie.PlayCount : xbmcMovie.PlayCount;
            Movie.ReleaseDate = Movie.ReleaseDate != default(DateTime) ? Movie.ReleaseDate : xbmcMovie.ReleaseDate;
            Movie.LastPlayed = Movie.LastPlayed != default(DateTime) ? Movie.LastPlayed : FilterDate(xbmcMovie.LastPlayed);

            if(!string.IsNullOrEmpty(xbmcMovie.MPAA) && string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                
            }

            AddActors(xbmcMovie.Actors);

            AddNfoMovieCommon(xbmcMovie);
        }

        public static IEnumerable<ArtInfo> GetArt(XbmcXmlMovie xm) {
            List<ArtInfo> art = new List<ArtInfo>();

            if (xm.Thumbs != null) {
                //add all Thumbnails/Posters/Covers
                foreach (XbmcXmlThumb thumb in xm.Thumbs) {
                    ArtInfo a;

                    if (string.IsNullOrEmpty(thumb.Aspect)) {
                        a = new ArtInfo(ArtType.Unknown, thumb.Path, thumb.Preview);
                    }
                    else {
                        switch (thumb.Aspect.ToLower()) {
                            case "poster":
                                a = new ArtInfo(ArtType.Poster,thumb.Path, thumb.Preview);
                                break;
                            case "cover":
                                a = new ArtInfo(ArtType.Cover, thumb.Path, thumb.Preview);
                                break;
                            default:
                                a = new ArtInfo(ArtType.Unknown, thumb.Path, thumb.Preview);
                                break;
                        }
                    }

                    art.Add(a);
                }
            }

            if (xm.Fanart != null && xm.Fanart.Thumbs != null) {
                //add fanart
                art.AddRange(xm.Fanart.Thumbs.Select(thumb => new ArtInfo(ArtType.Fanart, thumb.Path, thumb.Preview)));
            }

            return art;
        }

        private void AddNfoMovieCommon(XbmcXmlMovie xbmcMovie) {
            Movie.TmdbID = xbmcMovie.TmdbId;
            Movie.ImdbID = xbmcMovie.ImdbId;
            Movie.Watched = xbmcMovie.Watched;

            AddSet(xbmcMovie.Set);

            Movie.Premiered = FilterDate(xbmcMovie.Premiered);
            Movie.Aired = FilterDate(xbmcMovie.Aired);
            Movie.Trailer = xbmcMovie.GetTrailerUrl();

            Movie.Art.AddRange(GetArt(xbmcMovie));

            if (xbmcMovie.Certifications != null) {
                foreach (XbmcXmlCertification certification in xbmcMovie.Certifications) {
                    ISOCountryCode country = ISOCountryCodes.Instance.GetByEnglishName(certification.Country);

                    Movie.Certifications.Add(new CertificationInfo(country, certification.Rating));
                }
            }

            if (xbmcMovie.Countries != null) {
                foreach (string country in xbmcMovie.Countries) {
                    ISOCountryCode isoCountry = ISOCountryCodes.Instance.GetByEnglishName(country);
                    if (isoCountry == null) {
                        continue;
                    }

                    Movie.Countries.Add(isoCountry);
                }
            }

            Movie.RatingAverage = Math.Abs(xbmcMovie.Rating - default(float)) > 0.001 ? xbmcMovie.Rating : Movie.RatingAverage;

            Movie.Genres.AddRange(xbmcMovie.Genres);
            Movie.Studios.AddRange(xbmcMovie.Studios);

            foreach (string director in xbmcMovie.Directors) {
                AddDirector(director);
            }

            if (xbmcMovie.RuntimeInSeconds.HasValue) {
                //convert seconds to miliseconds
                long? ms = xbmcMovie.RuntimeInSeconds * 1000;

                if (Movie.IsMultipart) {
                    Movie.Runtime += ms;
                }
                else {
                    Movie.Runtime = ms;
                }
            }

            if (xbmcMovie.Plot != null) {
                Movie.Plots.Add(new PlotInfo(xbmcMovie.Plot, xbmcMovie.Outline, xbmcMovie.Tagline, null));
            }
        }
    }

}