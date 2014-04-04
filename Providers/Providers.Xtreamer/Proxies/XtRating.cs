using System.Collections.Generic;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtRating : ChangeTrackingProxy<XjbPhpMovie>, IRating {
        private string _ratingKey;

        public XtRating(XjbPhpMovie movie, string ratingKey) : base(movie){
            _ratingKey = ratingKey;

            OriginalValues = new Dictionary<string, object> {
                { "Critic", ratingKey },
                { "Value", Entity.Ratings[ratingKey] }
            };
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the name of the critic.</summary>
        /// <value>The name of the critic.</value>
        public string Critic {
            get { return _ratingKey; }
            set {
                double ratingValue = Entity.Ratings[_ratingKey];
                Entity.Ratings.Remove(_ratingKey);

                _ratingKey = value;
                Entity.Ratings.Add(_ratingKey, ratingValue);
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the value of the rating.</summary>
        /// <value>The rating value</value>
        public double Value {
            get { return Entity.Ratings[_ratingKey]; }
            set {
                Entity.Ratings[_ratingKey] = value;
                TrackChanges(value);
            }
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
