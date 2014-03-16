using Frost.Common.Models;

namespace Frost.Providers.Xbmc.DB.Proxy {

    public class XbmcPlot : IPlot {
        private readonly XbmcMovie _movie;

        public XbmcPlot(XbmcMovie movie) {
            _movie = movie;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public XbmcPlot(string tagline, string summary, string full, string language, XbmcMovie movie) {
            Tagline = tagline;
            Summary = summary;
            Full = full;
            Language = language;
            _movie = movie;
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Tagline":
                    case "Summary":
                    case "Full":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        public string Tagline {
            get { return _movie.Tagline; }
            set { _movie.Tagline = value; }
        }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        public string Summary {
            get { return _movie.PlotOutline; }
            set { _movie.PlotOutline = value; }
        }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full {
            get { return _movie.Plot; }
            set { _movie.Plot = value; } 
        }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        public string Language {
            get { return null; }
            set { } 
        }
    }
}
