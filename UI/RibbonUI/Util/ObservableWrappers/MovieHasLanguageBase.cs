using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {

    public abstract class MovieHasLanguageBase<T> : MovieItemBase<T>, IHasLanguage where T : class, IMovieEntity {

        public MovieHasLanguageBase(T observed) : base(observed) {
            
        }
        
        public abstract ILanguage Language { get; set; }

        public string LanguageImage {
            get {
                if (Language != null && Language.ISO639 != null) {
                    return GetImageSourceFromPath("Images/Languages/" + Language.ISO639.Alpha3 + ".png");
                }
                return null;
            }
        }
    }

}