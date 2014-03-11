using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieItemBase {
        protected ImageSource GetImageSourceFromPath(string filePath) {
            if (!File.Exists(filePath)) {
                return null;
            }

            filePath = string.Format("file://{0}/{1}", Directory.GetCurrentDirectory(), filePath);
            BitmapImage bitmapImage = new BitmapImage(new Uri(filePath, UriKind.Absolute));
            return bitmapImage;
        }
    }

}
