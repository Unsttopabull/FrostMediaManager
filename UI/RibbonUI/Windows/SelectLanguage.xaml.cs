using System.Collections.Generic;
using System.Windows;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectLanguage.xaml</summary>
    public partial class SelectLanguage : Window {
        public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register("Languages", typeof(IEnumerable<MovieLanguage>), typeof(SelectLanguage), new PropertyMetadata(default(IEnumerable<MovieLanguage>), LanguagesOnChanged));

        public SelectLanguage() {
            InitializeComponent();
        }

        private static void LanguagesOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((SelectLanguageViewModel) (((SelectLanguage) d).DataContext)).Languages = (IEnumerable<MovieLanguage>) args.NewValue;
        }

        public IEnumerable<MovieLanguage> Languages {
            get { return (IEnumerable<MovieLanguage>) GetValue(LanguagesProperty); }
            set { SetValue(LanguagesProperty, value); }
        }
    }
}
