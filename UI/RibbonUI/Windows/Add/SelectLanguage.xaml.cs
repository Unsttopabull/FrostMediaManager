using System.Collections.Generic;
using System.Windows;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.Windows.Add {

    /// <summary>Interaction logic for SelectLanguage.xaml</summary>
    public partial class SelectLanguage : Window {
        public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register("Languages", typeof(IEnumerable<MovieLanguage>), typeof(SelectLanguage), new PropertyMetadata(default(IEnumerable<MovieLanguage>), LanguagesOnChanged));

        public SelectLanguage() {
            InitializeComponent();
        }

        public IEnumerable<MovieLanguage> Languages {
            get { return (IEnumerable<MovieLanguage>) GetValue(LanguagesProperty); }
            set { SetValue(LanguagesProperty, value); }
        }

        private static void LanguagesOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((SelectLanguageViewModel) (((SelectLanguage) d).DataContext)).Languages = (IEnumerable<MovieLanguage>) args.NewValue;
        }
    }
}
