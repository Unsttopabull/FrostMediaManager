using Frost.Common.Models.Provider;

namespace Frost.Common.Models.FeatureDetector {

    /// <summary>Represents the information about an art that has been detected by Feature Detector.</summary>
    public class ArtInfo : IArt {

        /// <summary>Initializes a new instance of the <see cref="ArtInfo"/> class.</summary>
        public ArtInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="ArtInfo"/> class.</summary>
        /// <param name="type">The type of the art.</param>
        /// <param name="path">The full path/uri to the art.</param>
        /// <param name="preview">The preview image (eg. downscaled original).</param>
        public ArtInfo(ArtType type, string path, string preview) {
            Type = type;
            Path = path;
            Preview = preview;
        }

        /// <summary>Gets or sets the type of the art.</summary>
        /// <value>The type of the art.</value>
        public ArtType Type { get; set; }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview { get; set; }

        #region IArt

        /// <summary>Unique identifier.</summary>
        long IMovieEntity.Id {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>Gets the value whether the property is editable.</summary>
        /// <value>The <see cref="System.Boolean"/> if the value is editable.</value>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>Returns <c>true</c> if property is editable, otherwise <c>false</c> (Not implemented or read-only).</returns>
        bool IMovieEntity.this[string propertyName] {
            get { throw new System.NotImplementedException(); }
        }

        string IArt.PreviewOrPath {
            get { throw new System.NotImplementedException(); }
        }
        #endregion
    }

}