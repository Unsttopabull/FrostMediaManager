using System.Collections;
using System.Collections.Generic;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;

namespace Frost.DetectFeatures {

    public enum DetectedMovieType {
        DVD,
        BluRay,
        Multipart,
        Single
    }

    public class MovieFiles : IEnumerable<FileNameInfo> {

        /// <summary>Initializes a new instance of the <see cref="MovieFiles"/> class.</summary>
        /// <param name="movieType">Type of the movie.</param>
        /// <param name="fileInformation">The file name information.</param>
        public MovieFiles(DetectedMovieType movieType, FileNameInfo[] fileInformation) {
            MovieType = movieType;
            FileInformation = fileInformation;
        }

        public DetectedMovieType MovieType { get; private set; }

        public FileNameInfo[] FileInformation { get; private set; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<FileNameInfo> GetEnumerator() {
            return ((IEnumerable<FileNameInfo>)FileInformation).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return FileInformation.GetEnumerator();
        }
    }
}
