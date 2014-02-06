using System;
using System.Linq;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.XML.Jukebox;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {

        private void GetXtreamerNfoInfo(string xtNfo) {
            XjbXmlMovie xjbMovie = null;
            try {
                xjbMovie = XjbXmlMovie.Load(xtNfo);
            }
            catch (Exception) {
                //Console.Error.WriteLine(string.Format("File \"{0}\" is not a valid NFO.", xtNfo), "ERROR");
            }

            if (xjbMovie != null) {
                if (_nfoPriority == NFOPriority.OnlyNotDetected) {
                    AddNotDetectedNfoInfo(xjbMovie);
                }
                else {
                    OverrideDetectedInfoWithNfo(xjbMovie);
                }
            }
        }

        private void AddNotDetectedNfoInfo(XjbXmlMovie xjbMovie) {
            Movie.Title = Check(Movie.Title, xjbMovie.Title);
            Movie.OriginalTitle = Check(Movie.OriginalTitle, xjbMovie.OriginalTitle);
            Movie.SortTitle = Check(Movie.SortTitle, xjbMovie.SortTitle);
            Movie.ImdbID = Check(Movie.ImdbID, xjbMovie.ImdbId);
            Movie.ReleaseYear = CheckReleaseYear(Movie.ReleaseYear) ? Movie.ReleaseYear : xjbMovie.Year;

            Movie.RatingAverage = Movie.RatingAverage ?? xjbMovie.AverageRating;
            if(string.IsNullOrEmpty(Movie.MPAARating) && !string.IsNullOrEmpty(xjbMovie.MPAA)) {
                Country usa = _mvc.Countries.FirstOrDefault(c => c.Name == "United States") ?? new Country("United States", "us", "usa");

                Movie.Certifications.Add(new Certification(usa, xjbMovie.MPAA));
            }

            GetNfoMovieInfoCommon(xjbMovie);
        }

        private void OverrideDetectedInfoWithNfo(XjbXmlMovie xjbMovie) {
            Movie.Title = xjbMovie.Title ?? Movie.Title;
            Movie.OriginalTitle = xjbMovie.OriginalTitle ?? Movie.OriginalTitle;
            Movie.SortTitle = xjbMovie.SortTitle ?? Movie.SortTitle;
            Movie.ImdbID = xjbMovie.ImdbId ?? Movie.ImdbID;
            Movie.ReleaseYear = CheckReleaseYear(xjbMovie.Year) ? xjbMovie.Year : Movie.ReleaseYear;

            Movie.RatingAverage = Math.Abs(xjbMovie.AverageRating - default(float)) > 0.001 ? xjbMovie.AverageRating : Movie.RatingAverage;
            if(!string.IsNullOrEmpty(xjbMovie.MPAA)) {
                if (!string.IsNullOrEmpty(Movie.MPAARating)) {
                    Movie.Certifications.RemoveWhere(c => c.Country.Name == "United States");
                }
                Country usa = _mvc.Countries.FirstOrDefault(c => c.Name == "United States") ?? new Country("United States", "us", "usa");
                Movie.Certifications.Add(new Certification(usa, xjbMovie.MPAA));
            }

            GetNfoMovieInfoCommon(xjbMovie);
        }

        private void GetNfoMovieInfoCommon(XjbXmlMovie xjbMovie) {
            if (!string.IsNullOrEmpty(xjbMovie.ReleaseDate)) {
                DateTime releaseDate;
                DateTime.TryParse(xjbMovie.ReleaseDate, out releaseDate);
                if (releaseDate != default(DateTime)) {
                    Movie.ReleaseDate = releaseDate;
                }
            }

            if (xjbMovie.Certifications != null) {
                foreach (Certification certification in xjbMovie.Certifications) {
                    AddCertification(certification);
                }
            }

            CheckAddXjbGenres(xjbMovie, true);
            AddStudio(xjbMovie.Studio);

            foreach (string director in xjbMovie.Directors) {
                AddDirector(director);
            }
            
            AddActors(xjbMovie.Actors, true);

            if (xjbMovie.Runtime.HasValue) {
                long ms = xjbMovie.Runtime.Value * 60000;
                if (Movie.IsMultipart) {
                    Movie.Runtime += ms;
                }
                else {
                    Movie.Runtime = ms;
                }
            }

            if (xjbMovie.Plot != null) {
                Movie.Plots.Add(new Plot(xjbMovie.Plot, xjbMovie.Outline, xjbMovie.Tagline, null));
            }
        }


        private string Check(string priority, string otherwise) {
            if (!string.IsNullOrEmpty(priority)) {
                return priority;
            }

            return !string.IsNullOrEmpty(otherwise)
                ? otherwise
                : null;
        }
    
        private void CheckAddXjbGenres(XjbXmlMovie xjbMovie, bool otherwise = false) {
            if (otherwise) {
                Movie.Genres.Clear();
            }

            if (xjbMovie.Genres == null) {
                return;
            }

            foreach (string genreAbbrev in xjbMovie.Genres) {
                if (string.IsNullOrEmpty(genreAbbrev)) {
                    continue;
                }

                Genre genre = XjbGenre.FromGenreAbbreviation(genreAbbrev);
                AddGenre(genre);
            }
        }
    }

}