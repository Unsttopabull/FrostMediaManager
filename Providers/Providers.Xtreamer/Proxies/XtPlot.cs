using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtPlot : IPlot {
        private readonly XjbPhpMovie _movie;

        public XtPlot(XjbPhpMovie movie) {
            _movie = movie;
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
            get { return _movie.PlotSummary; }
            set { _movie.PlotSummary = value; }
        }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full {
            get { return _movie.PlotFull; }
            set { _movie.PlotFull = value; } 
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
