using System.Windows.Media;
using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {

    public abstract class MovieHasLanguageBase : MovieItemBase, IHasLanguage {
        
        public abstract ILanguage Language { get; set; }

        public ImageSource LanguageImage {
            get {
                if (Language != null && Language.ISO639 != null) {
                    return GetImageSourceFromPath("Images/Languages/" + Language.ISO639.Alpha3 + ".png");
                }
                return null;
            }
        }
    }

}