using Frost.Common.Models.Provider;

namespace Frost.Common.Models.FeatureDetector {

    public class ArtInfo : IArt {

        public ArtInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="ArtInfo"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="path">The path.</param>
        /// <param name="preview">The preview.</param>
        public ArtInfo(ArtType type, string path, string preview) {
            Type = type;
            Path = path;
            Preview = preview;
        }

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