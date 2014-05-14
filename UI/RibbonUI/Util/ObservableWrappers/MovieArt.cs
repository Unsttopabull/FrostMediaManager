using Frost.Common;
using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Util.ObservableWrappers {

    public class MovieArt : ObservableBase<IArt> {

        internal MovieArt() : base(null){
            
        }

        public MovieArt(IArt art) : base(art) {
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path {
            get { return _observedEntity.Path; }
            set {
                _observedEntity.Path = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview {
            get { return _observedEntity.Preview; }
            set {
                _observedEntity.Preview = value;
                OnPropertyChanged();
            }
        }

        public ArtType Type {
            get { return _observedEntity.Type; }
        }

        public string PreviewOrPath {
            get { return _observedEntity.PreviewOrPath; }
        }
    }
}
