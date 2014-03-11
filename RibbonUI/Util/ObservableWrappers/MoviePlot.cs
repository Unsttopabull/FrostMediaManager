using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Models;
using Frost.Common.Properties;

namespace RibbonUI.Util.ObservableWrappers {

    public class MoviePlot : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IPlot _plot;

        public MoviePlot(IPlot plot) {
            _plot = plot;
        }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        public string Tagline {
            get { return ObservedPlot.Tagline; }
            set {
                ObservedPlot.Tagline = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the story summary.</summary>
        /// <value>A short story summary, the plot outline</value>
        public string Summary {
            get { return ObservedPlot.Summary; }
            set {
                ObservedPlot.Summary = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the full plot.</summary>
        /// <value>The full plot.</value>
        public string Full {
            get { return ObservedPlot.Full; }
            set {
                ObservedPlot.Full = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the language of this plot.</summary>
        /// <value>The language of this plot.</value>
        public string Language {
            get { return ObservedPlot.Language; }
            set {
                ObservedPlot.Language = value;
                OnPropertyChanged();
            }
        }

        public IPlot ObservedPlot {
            get { return _plot; }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return ObservedPlot.ToString();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
