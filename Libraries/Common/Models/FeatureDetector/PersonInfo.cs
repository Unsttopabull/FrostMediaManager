namespace Frost.Common.Models.FeatureDetector {

    public class PersonInfo {
        private string _thumb;

        /// <summary>Initializes a new instance of the <see cref="PersonInfo"/> class.</summary>
        public PersonInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="PersonInfo"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        public PersonInfo(string name) {
            Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="PersonInfo"/> class.</summary>
        /// <param name="name">The full name of the actor.</param>
        /// <param name="thumb">The thumbnail image.</param>
        /// <param name="imdbId">The IMDB Id (nm1234567)</param>
        public PersonInfo(string name, string thumb, string imdbId = null) : this(name) {
            if (!string.IsNullOrEmpty(thumb)) {
                _thumb = thumb;
            }

            ImdbID = imdbId;
        }
        
        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get {
                return string.IsNullOrEmpty(_thumb)
                           ? null
                           : _thumb;
            }
            set { _thumb = value; }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", Name, ImdbID ?? "N/A");
        }
    }

}