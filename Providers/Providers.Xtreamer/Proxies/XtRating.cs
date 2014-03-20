using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtRating : IRating {
        private readonly XjbPhpMovie _movie;
        private string _ratingKey;

        public XtRating(XjbPhpMovie movie, string ratingKey) {
            _movie = movie;
            _ratingKey = ratingKey;
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the name of the critic.</summary>
        /// <value>The name of the critic.</value>
        public string Critic {
            get { return _ratingKey; }
            set {
                double ratingValue = _movie.Ratings[_ratingKey];
                _movie.Ratings.Remove(_ratingKey);

                _ratingKey = value;
                _movie.Ratings.Add(_ratingKey, ratingValue);
            }
        }

        /// <summary>Gets or sets the value of the rating.</summary>
        /// <value>The rating value</value>
        public double Value {
            get { return _movie.Ratings[_ratingKey]; }
            set { _movie.Ratings[_ratingKey] = value; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Critic":
                    case "Value":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
