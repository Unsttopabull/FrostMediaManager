using System.Collections.Generic;
using System.Windows;
using Frost.Common.Models;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectLanguage.xaml</summary>
    public partial class SelectLanguage : Window {
        public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register("Languages", typeof(IEnumerable<ILanguage>), typeof(SelectLanguage), new PropertyMetadata(default(IEnumerable<ILanguage>), LanguagesOnChanged));

        public SelectLanguage() {
            InitializeComponent();
        }

        private static void LanguagesOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((SelectLanguageViewModel) (((SelectLanguage) d).DataContext)).Languages = (IEnumerable<ILanguage>) args.NewValue;
        }

        public IEnumerable<ILanguage> Languages {
            get { return (IEnumerable<ILanguage>) GetValue(LanguagesProperty); }
            set { SetValue(LanguagesProperty, value); }
        }
    }
}
