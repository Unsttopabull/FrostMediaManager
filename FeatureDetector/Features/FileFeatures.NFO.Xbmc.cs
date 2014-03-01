using System;
using System.IO;
using System.Linq;
using Frost.Model.Xbmc.NFO;
using Frost.Models.Frost;
using Frost.Models.Frost.DB;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {

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

            if(string.IsNullOrEmpty(Movie.MPAARating) && !string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                Movie.Certifications.Add(new Certification(new Country("United States", "us", "usa"), xbmcMovie.MPAA));
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

            if(!string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                if (!string.IsNullOrEmpty(Movie.MPAARating)) {
                    Movie.Certifications.RemoveWhere(c => c.Country.Name == "United States");
                }

                Movie.Certifications.Add(new Certification(new Country("United States", "us", "usa"), xbmcMovie.MPAA));
            }

            AddActors(xbmcMovie.Actors);

            AddNfoMovieCommon(xbmcMovie);
        }

        private void AddNfoMovieCommon(XbmcXmlMovie xbmcMovie) {
            Movie.TmdbID = xbmcMovie.TmdbId;
            Movie.ImdbID = xbmcMovie.ImdbId;
            Movie.Watched = xbmcMovie.Watched;

            AddSet(xbmcMovie.Set);

            Movie.Premiered = FilterDate(xbmcMovie.Premiered);
            Movie.Aired = FilterDate(xbmcMovie.Aired);
            Movie.Trailer = xbmcMovie.GetTrailerUrl();

            Movie.Art.UnionWith(xbmcMovie.GetArt());
            Movie.Certifications.UnionWith(xbmcMovie.GetCertifications());

            foreach (Country country in Country.GetFromNames(xbmcMovie.Countries)) {
                Country item = CheckCountry(country);
                Movie.Countries.Add(item);
            }

            Movie.RatingAverage = Math.Abs(xbmcMovie.Rating - default(float)) > 0.001 ? xbmcMovie.Rating : Movie.RatingAverage;

            foreach (Genre genre in Genre.GetFromNames(xbmcMovie.Genres)) {
                AddGenre(genre);
            }

            foreach (string studio in xbmcMovie.Studios) {
                AddStudio(studio);
            }

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
                Movie.Plots.Add(new Plot(xbmcMovie.Plot, xbmcMovie.Outline, xbmcMovie.Tagline, null));
            }
        }
    }

}