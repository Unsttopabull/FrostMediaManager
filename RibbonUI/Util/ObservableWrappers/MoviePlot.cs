using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {

    public class MoviePlot : ObservableBase<IPlot> {

        public MoviePlot(IPlot plot) : base(plot) {
        }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        public string Tagline {
            get { return _observedEntity.Tagline; }
            set {
                _observedEntity.Tagline = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        public string Summary {
            get { return _observedEntity.Summary; }
            set {
                _observedEntity.Summary = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full {
            get { return _observedEntity.Full; }
            set {
                _observedEntity.Full = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        public string Language {
            get { return _observedEntity.Language; }
            set {
                _observedEntity.Language = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return _observedEntity.ToString();
        }
    }

}
