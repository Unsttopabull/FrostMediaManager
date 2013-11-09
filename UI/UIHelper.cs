using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Frost.UI {
    static class UIHelper {
        /// <summary>Finds a Child of a given item in the visual tree. </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject {
            // Confirm parent and childName are valid. 
            if (parent == null) {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++) {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null) {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!String.IsNullOrEmpty(childName)) {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName) {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem) {
                    return (TChildItem) child;
                }

                TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(
                long fileSize,
                [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
                int bufferSize);

        public static string FormatFileSizeAsString(long size) {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(size, sb, sb.Capacity);
            return sb.ToString();
        }

        static internal ImageSource GetImageSourceFromLocalResource(string resourceName) {
            Uri oUri = new Uri("pack://application:,,,/" + resourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }

        static internal ImageSource GetImageSourceFromResource(string assemblyName, string resourceName) {
            Uri oUri = new Uri("pack://application:,,,/" + assemblyName + ";component/" + resourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }


        public static void WriteSeparator(char separator) {
            int windowWidth;
            try {
                windowWidth = Console.WindowWidth;
            }
            catch {
                windowWidth = 10;
            }

            StringBuilder sb = new StringBuilder(windowWidth);
            for (int i = 0; i < windowWidth; i++) {
                sb.Append(separator);
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine();
        }
    }
}
