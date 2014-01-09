using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Frost.Common.Models.DB.Jukebox;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.People;
using Frost.Common.Models.XML.Jukebox;
using Frost.Common.Models.XML.XBMC;

namespace Frost.DetectFeatures {

    public partial class FileFeatures {
        private void GetNfoInfo() {
            FileInfo[] xbmcNfo = _directoryInfo.EnumerateFiles("*.nfo").ToArray();
            if (xbmcNfo.Length > 0) {
                GetXbmcNfoInfo(xbmcNfo);
                return;
            }

            FileInfo xtNfo = _directoryInfo.EnumerateFiles(_file.Name + "_xjb.xml").FirstOrDefault();
            if (xtNfo == null) {
                return;
            }

            if (xtNfo.Exists) {
                GetXtreamerNfoInfo(xtNfo.FullName);
            }
            else {
                Debug.WriteLine("File: " + xtNfo.Name +" is not accessible.", "ERROR");
            }
        }

        #region Xtreamer

        private void GetXtreamerNfoInfo(string xtNfo) {
            XjbXmlMovie xjbMovie = null;
            try {
                xjbMovie = XjbXmlMovie.Load(xtNfo);
            }
            catch (Exception) {
                Console.Error.WriteLine(string.Format("File \"{0}\" is not a valid NFO.", xtNfo), "ERROR");
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

        private string Check(string priority, string @else) {
            if (!string.IsNullOrEmpty(priority)) {
                return priority;
            }

            return !string.IsNullOrEmpty(@else)
                ? @else
                : null;
        }

        private void AddNotDetectedNfoInfo(XjbXmlMovie xjbMovie) {
            Movie.Title = Check(Movie.Title, xjbMovie.Title);
            Movie.OriginalTitle = Check(Movie.OriginalTitle, xjbMovie.OriginalTitle);
            Movie.SortTitle = Check(Movie.SortTitle, xjbMovie.SortTitle);
            Movie.ImdbID = Check(Movie.ImdbID, xjbMovie.ImdbId);
            Movie.ReleaseYear = Movie.ReleaseYear ?? xjbMovie.Year;

            if (Movie.ReleaseDate == default(DateTime)) {
                DateTime releaseDate;
                DateTime.TryParse(xjbMovie.ReleaseDate, out releaseDate);
                if (releaseDate != default(DateTime)) {
                    Movie.ReleaseDate = releaseDate;
                }
            }

            Movie.RatingAverage = Movie.RatingAverage ?? xjbMovie.AverageRating;
            if(string.IsNullOrEmpty(Movie.GetMPAARating()) && !string.IsNullOrEmpty(xjbMovie.MPAA)) {
                Movie.Certifications.Add(new Certification("United States", xjbMovie.MPAA));
            }

            if (xjbMovie.Certifications != null) {
                foreach (Certification certification in xjbMovie.Certifications) {
                    Movie.Certifications.Add(certification);
                }
            }

            CheckAddXjbGenres(xjbMovie);

            if(!string.IsNullOrEmpty(xjbMovie.Studio) && !Movie.Studios.Contains(xjbMovie.Studio)) {
                Movie.Studios.Add(xjbMovie.Studio);
            }

            if (!string.IsNullOrEmpty(xjbMovie.Director)) {
                Person director = new Person(xjbMovie.Director);
                if (!Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }

            //minutes to miliseconds
            Movie.Runtime = Movie.Runtime ?? xjbMovie.Runtime * 60000;

            AddActors(xjbMovie.Actors);

            Movie.Plots.Add(new Plot(xjbMovie.Plot, xjbMovie.Outline, xjbMovie.Tagline, null));
        }

        private void CheckAddXjbGenres(XjbXmlMovie xjbMovie, bool @override = false) {
            if (@override) {
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
                if (!Movie.Genres.Contains(genre)) {
                    Movie.Genres.Add(genre);
                }
            }
        }

        private void OverrideDetectedInfoWithNfo(XjbXmlMovie xjbMovie) {
            Movie.Title = xjbMovie.Title ?? Movie.Title;
            Movie.OriginalTitle = xjbMovie.OriginalTitle ?? Movie.OriginalTitle;
            Movie.SortTitle = xjbMovie.SortTitle ?? Movie.SortTitle;
            Movie.ImdbID = xjbMovie.ImdbId ?? Movie.ImdbID;
            Movie.ReleaseYear = xjbMovie.Year != default(int) ? xjbMovie.Year : Movie.ReleaseYear;

            if (!string.IsNullOrEmpty(xjbMovie.ReleaseDate)) {
                DateTime releaseDate;
                DateTime.TryParse(xjbMovie.ReleaseDate, out releaseDate);
                if (releaseDate != default(DateTime)) {
                    Movie.ReleaseDate = releaseDate;
                }
            }

            Movie.RatingAverage = Math.Abs(xjbMovie.AverageRating - default(float)) > 0.001 ? xjbMovie.AverageRating : Movie.RatingAverage;
            if(!string.IsNullOrEmpty(xjbMovie.MPAA)) {
                if (!string.IsNullOrEmpty(Movie.GetMPAARating())) {
                    Movie.Certifications.RemoveWhere(c => c.Country.Name == "United States");
                }
                Movie.Certifications.Add(new Certification("United States", xjbMovie.MPAA));
            }

            foreach (Certification certification in xjbMovie.Certifications) {
                Movie.Certifications.Add(certification);
            }

            CheckAddXjbGenres(xjbMovie, true);

            if(!Movie.Studios.Contains(xjbMovie.Studio)) {
                Movie.Studios.Add(xjbMovie.Studio);
            }

            if (!string.IsNullOrEmpty(xjbMovie.Director)) {
                Person director = new Person(xjbMovie.Director);
                if (!Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }

            Movie.Runtime = xjbMovie.Runtime ?? Movie.Runtime;

            OverrideActors(xjbMovie.Actors);

            Movie.Plots.Add(new Plot(xjbMovie.Plot, xjbMovie.Outline, xjbMovie.Tagline, null));
        }
        #endregion

        private void AddActors<T>(IEnumerable<T> actors) where T : XbmcXmlActor {
            foreach (T actor in actors) {
                if (string.IsNullOrEmpty(actor.Name)) {
                    continue;
                }

                MovieActor movieActor = Movie.ActorsLink.FirstOrDefault(a => a.Person.Name == actor.Name);
                if (movieActor != null) {
                    movieActor.Character = actor.Role ?? movieActor.Character;
                }
                else {
                    Movie.ActorsLink.Add(new MovieActor(Movie, new Person(actor.Name, actor.Thumb), actor.Role));
                }
            }
        }

        private void OverrideActors<T>(IEnumerable<T> actors) where T : XbmcXmlActor {
            foreach (T actor in actors) {
                if (string.IsNullOrEmpty(actor.Name)) {
                    continue;
                }

                MovieActor movieActor = Movie.ActorsLink.FirstOrDefault(al => al.Person.Name == actor.Name);
                if (movieActor != null && movieActor.Character == null) {
                    movieActor.Character = actor.Role;
                }
                else {
                    Movie.ActorsLink.Add(new MovieActor(Movie, new Person(actor.Name, actor.Thumb), actor.Role));
                }
            }
        }

        #region XBMC
        private void GetXbmcNfoInfo(FileInfo[] xbmcNfo) {
            FileInfo sameName = xbmcNfo.FirstOrDefault(fi => fi.Name.Equals(_file.Name + ".nfo"));
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
                Console.Error.WriteLine(string.Format("File \"{0}\" is not a valid NFO.", filePath), "ERROR");
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
            Movie.ReleaseYear = xbmcMovie.Year != default(int) ? xbmcMovie.Year : Movie.ReleaseYear;
            Movie.Top250 = xbmcMovie.Top250 != default(int) ? xbmcMovie.Top250 : Movie.Top250;
            Movie.PlayCount = xbmcMovie.PlayCount != default(int) ? xbmcMovie.PlayCount : Movie.PlayCount;
            Movie.ReleaseDate = xbmcMovie.ReleaseDate != default(DateTime) ? xbmcMovie.ReleaseDate : Movie.ReleaseDate;
            Movie.LastPlayed = xbmcMovie.LastPlayed != default(DateTime) ? FilterDate(xbmcMovie.LastPlayed) : Movie.LastPlayed;
            Movie.TmdbID = xbmcMovie.TmdbId;
            Movie.ImdbID = xbmcMovie.ImdbId;
            Movie.Watched = xbmcMovie.Watched;

            if (!string.IsNullOrEmpty(xbmcMovie.Set)) {
                Movie.Set = new Set(xbmcMovie.Set);
            }

            Movie.Premiered = FilterDate(xbmcMovie.Premiered);
            Movie.Aired = FilterDate(xbmcMovie.Aired);
            Movie.Trailer = xbmcMovie.GetTrailerUrl();

            Movie.Arts.UnionWith(xbmcMovie.GetArt());
            Movie.Certifications.UnionWith(xbmcMovie.GetCertifications());
            Movie.Countries.UnionWith(xbmcMovie.GetCountries());
            
            Movie.RatingAverage = Math.Abs(xbmcMovie.Rating - default(float)) > 0.001 ? xbmcMovie.Rating : Movie.RatingAverage;

            if(string.IsNullOrEmpty(Movie.GetMPAARating()) && !string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                Movie.Certifications.Add(new Certification("United States", xbmcMovie.MPAA));
            }

            foreach (Genre genre in xbmcMovie.GetGenres()) {
                if (!Movie.Genres.Contains(genre)) {
                    Movie.Genres.Add(genre);
                }
            }

            foreach (string studio in xbmcMovie.Studios) {
                if(!Movie.Studios.Contains(studio)) {
                    Movie.Studios.Add(studio);
                }
            }

            foreach (Person director in xbmcMovie.GetDirectors()) {
                if (!string.IsNullOrEmpty(director.Name) && !Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }

            //convert minutes to milisecodns
            Movie.Runtime = xbmcMovie.RuntimeInSeconds * 60000 ?? Movie.GetVideoRuntimeSum();

            OverrideActors(xbmcMovie.Actors);


            Movie.Plots.Add(new Plot(xbmcMovie.Plot, xbmcMovie.Outline, xbmcMovie.Tagline, null));
        }



        private void AddNotDetectedNfoInfo(XbmcXmlMovie xbmcMovie) {
            Movie.Title = Movie.Title ?? xbmcMovie.Title;
            Movie.OriginalTitle = Movie.OriginalTitle ?? xbmcMovie.OriginalTitle;
            Movie.SortTitle = Movie.SortTitle ?? xbmcMovie.SortTitle;
            Movie.ImdbID = Movie.ImdbID ?? xbmcMovie.ImdbId;
            Movie.ReleaseYear = Movie.ReleaseYear != default(int) ? Movie.ReleaseYear : xbmcMovie.Year;
            Movie.Top250 = Movie.Top250 != default(int) ? Movie.Top250 : xbmcMovie.Top250;
            Movie.PlayCount = Movie.PlayCount != default(int) ? Movie.PlayCount : xbmcMovie.PlayCount;
            Movie.ReleaseDate = Movie.ReleaseDate != default(DateTime) ? Movie.ReleaseDate : xbmcMovie.ReleaseDate;
            Movie.LastPlayed = Movie.LastPlayed != default(DateTime) ? Movie.LastPlayed : FilterDate(xbmcMovie.LastPlayed);
            Movie.TmdbID = xbmcMovie.TmdbId;
            Movie.ImdbID = xbmcMovie.ImdbId;
            Movie.Watched = xbmcMovie.Watched;

            if (!string.IsNullOrWhiteSpace(xbmcMovie.Set)) {
                Movie.Set = new Set(xbmcMovie.Set);
            }

            Movie.Premiered = FilterDate(xbmcMovie.Premiered);
            Movie.Aired = FilterDate(xbmcMovie.Aired);
            Movie.Trailer = xbmcMovie.GetTrailerUrl();

            Movie.Arts.UnionWith(xbmcMovie.GetArt());
            Movie.Certifications.UnionWith(xbmcMovie.GetCertifications());
            Movie.Countries.UnionWith(xbmcMovie.GetCountries());
            
            Movie.RatingAverage = Math.Abs(xbmcMovie.Rating - default(float)) > 0.001 ? xbmcMovie.Rating : Movie.RatingAverage;

            if(!string.IsNullOrEmpty(xbmcMovie.MPAA)) {
                if (!string.IsNullOrEmpty(Movie.GetMPAARating())) {
                    Movie.Certifications.RemoveWhere(c => c.Country.Name == "United States");
                }
                Movie.Certifications.Add(new Certification("United States", xbmcMovie.MPAA));
            }

            foreach (Genre genre in xbmcMovie.GetGenres()) {
                if (!Movie.Genres.Contains(genre)) {
                    Movie.Genres.Add(genre);
                }
            }

            foreach (string studio in xbmcMovie.Studios) {
                if(!Movie.Studios.Contains(studio)) {
                    Movie.Studios.Add(studio);
                }
            }

            foreach (Person director in xbmcMovie.GetDirectors()) {
                if (!string.IsNullOrEmpty(director.Name) && !Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }

            //convert seconds to milisecodns
            Movie.Runtime = Movie.GetVideoRuntimeSum() ?? xbmcMovie.RuntimeInSeconds * 1000;

            AddActors(xbmcMovie.Actors);

            Movie.Plots.Add(new Plot(xbmcMovie.Plot, xbmcMovie.Outline, xbmcMovie.Tagline, null));
        }
        #endregion
    }

}