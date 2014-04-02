using System.IO;
using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieItemBase<T> : ObservableBase<T> where T : class, IMovieEntity {

        public MovieItemBase(T observed) : base(observed) {
        }

        protected string GetImageSourceFromPath(string filePath) {
            if (!File.Exists(filePath)) {
                return null;
            }

            return string.Format("file://{0}/{1}", Directory.GetCurrentDirectory(), filePath);
        }
    }

}
