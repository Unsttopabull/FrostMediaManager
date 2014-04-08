using System;
using System.Linq;
using Frost.Common.Models.FeatureDetector;
using Frost.Providers.Xtreamer.DB;
using Frost.Providers.Xtreamer.NFO;

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
            Movie.ReleaseYear = CheckReleaseYear(Movie.ReleaseYear) ? Movie.ReleaseYear : (CheckReleaseYear(xjbMovie.Year) ? xjbMovie.Year : (long?)null);

            Movie.RatingAverage = Movie.RatingAverage ?? xjbMovie.AverageRating;
            if (string.IsNullOrEmpty(Movie.MPAARating) && !string.IsNullOrEmpty(xjbMovie.MPAA)) {
                Movie.Certifications.Add(new CertificationInfo(Usa, xjbMovie.MPAA));
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
            if (!string.IsNullOrEmpty(xjbMovie.MPAA)) {
                CertificationInfo mpaa = Movie.Certifications.FirstOrDefault(c => c.Country == Usa);
                if (mpaa != null) {
                    mpaa.Rating = xjbMovie.MPAA;
                }
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

            if (!string.IsNullOrEmpty(xjbMovie.CertificationsString)) {
                foreach (XjbCertification certification in xjbMovie.Certifications) {
                    if (Movie.Certifications.All(c => c.Country != certification.Country)) {
                        Movie.Certifications.Add(new CertificationInfo(certification.Country, certification.Rating));
                    }
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

            if (!string.IsNullOrEmpty(xjbMovie.Plot)) {
                Movie.Plots.Add(new PlotInfo(xjbMovie.Plot, xjbMovie.Outline, xjbMovie.Tagline, null));
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

                string xjbGenre = XjbGenre.GenreNameFromAbbreviation(genreAbbrev);
                if (!Movie.Genres.Contains(xjbGenre)) {
                    Movie.Genres.Add(xjbGenre);
                }
            }
        }
    }

}