using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using WPFLocalizeExtension.Providers;

namespace RibbonUI.Translation {

    public class GettextProvider : DependencyObject, ILocalizationProvider {
        private ObservableCollection<CultureInfo> _availableCultures;

        public GettextProvider() {
            RefreshAvailableCulutres();
        }

        public void RefreshAvailableCulutres() {
            DirectoryInfo executingDir = new DirectoryInfo(Gettext.ResourcesDirectory);
            ObservableCollection<CultureInfo> ci = new ObservableCollection<CultureInfo>();

            IEnumerable<string> cultures = string.IsNullOrEmpty(Gettext.ResourceName)
                                               ? executingDir.EnumerateFiles().Where(fi => fi.Name.EndsWith(".po")).Select(f => f.Name.Substring(0, f.Name.Length - 3))
                                               : executingDir.EnumerateDirectories().Select(f => f.Name);

            foreach (string culureTag in cultures) {
                try {
                    ci.Add(CultureInfo.GetCultureInfo(culureTag));
                }
                catch {
                }
            }

            _availableCultures = ci;
        }

        /// <summary>Uses the key and target to build a fully qualified resource key (Assembly, Dictionary, Key)</summary>
        /// <param name="key">Key used as a base to find the full key</param
        /// ><param name="target">Target used to help determine key information</param>
        /// <returns>Returns an object with all possible pieces of the given key (Assembly, Dictionary, Key)</returns>
        public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(string key, DependencyObject target) {
            return new GettextFullyQualifiedResourceKeyBase(key);
        }

        /// <summary>Get the localized object.</summary>
        /// <param name="key">The key to the value.</param>
        /// <param name="target">The target <see cref="T:System.Windows.DependencyObject"/>.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>The value corresponding to the source/dictionary/key path for the given culture (otherwise NULL).</returns>
        public object GetLocalizedObject(string key, DependencyObject target, CultureInfo culture) {
            return Gettext.T(culture, key);
        }

        /// <summary>An observable list of available cultures.</summary>
        public ObservableCollection<CultureInfo> AvailableCultures {
            get { return _availableCultures; }
        }

        public event ProviderChangedEventHandler ProviderChanged;
        public event ProviderErrorEventHandler ProviderError;
        public event ValueChangedEventHandler ValueChanged;
    }

}