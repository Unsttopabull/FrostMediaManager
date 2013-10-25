using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace WPF_Jukebox {
    static class Extend {

        public static void SetAnimatedSourceFromResource(this Image img, string fullResourcePath) {
            ImageBehavior.SetAnimatedSource(img, new BitmapImage(new Uri("pack://application:,,,/WPF_Jukebox;component/" + fullResourcePath)));
        }

        public static void SetAnimatedSourceFromAssemblyResource(this Image img, string assemblyName, string fullResourcePath) {
            ImageBehavior.SetAnimatedSource(img, new BitmapImage(new Uri("pack://application:,,,/" + assemblyName + ";component/" + fullResourcePath)));
        }

        public static void SetAnimatedSource(this Image img, ImageSource isc) {
            ImageBehavior.SetAnimatedSource(img, isc);
        }

        public static void SetAnimatedSource(this Image img, string uri) {
            ImageBehavior.SetAnimatedSource(img, new BitmapImage(new Uri(uri)));
        }

        public static DataGridRow GetSelectedRow(this DataGrid grid) {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
    }
}
