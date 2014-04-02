using System.Collections.Generic;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;
using Frost.Providers.Xtreamer.Proxies.ChangeTrackers;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtPlot : ChangeTrackingProxy<XjbPhpMovie>, IPlot {
        public XtPlot(XjbPhpMovie movie) : base(movie) {
            OriginalValues = new Dictionary<string, object> {
                { "Summary", movie.PlotSummary },
                { "Full", movie.PlotFull }
            };
        }

        public long Id {
            get { return 0; }
        }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        public string Tagline {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        public string Summary {
            get { return Entity.PlotSummary; }
            set {
                Entity.PlotSummary = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full {
            get { return Entity.PlotFull; }
            set {
                Entity.PlotFull = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        public string Language {
            get { return null; }
            set { }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Summary":
                    case "Full":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }

}