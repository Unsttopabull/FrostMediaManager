using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common;
using Frost.Common.Models;
using Frost.GettextMarkupExtension.Annotations;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieArt : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IArt _art;

        public MovieArt(IArt art) {
            _art = art;
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path {
            get { return _art.Path; }
            set {
                _art.Path = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview {
            get { return _art.Preview; }
            set {
                _art.Preview = value;
                OnPropertyChanged();
            }
        }

        public ArtType Type {
            get { return _art.Type; }
        }

        public string PreviewOrPath {
            get { return _art.PreviewOrPath; }
        }

        public IArt ObservedArt {get { return _art; }}

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
