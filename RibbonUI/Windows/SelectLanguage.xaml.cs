using System.Windows;
using System.Windows.Data;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectLanguage.xaml</summary>
    public partial class SelectLanguage : Window {
        public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register("Languages", typeof(CollectionViewSource), typeof(SelectLanguage), new PropertyMetadata(default(CollectionViewSource), LanguagesOnChanged));

        public SelectLanguage() {
            InitializeComponent();
        }

        private static void LanguagesOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((SelectLanguageViewModel) (((SelectLanguage) d).DataContext)).Languages = (CollectionViewSource) args.NewValue;
        }

        public CollectionViewSource Languages {
            get { return (CollectionViewSource) GetValue(LanguagesProperty); }
            set { SetValue(LanguagesProperty, value); }
        }
    }
}
