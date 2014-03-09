using System.Windows;
using System.Windows.Data;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddCountries : Window {
        public static readonly DependencyProperty CountriesProperty = DependencyProperty.Register("Countries", typeof(CollectionViewSource), typeof(AddCountries), new PropertyMetadata(default(CollectionViewSource), OnCountriesChanged));

        public AddCountries() {
            InitializeComponent();
        }

        public CollectionViewSource Countries {
            get { return (CollectionViewSource) GetValue(CountriesProperty); }
            set { SetValue(CountriesProperty, value); }
        }

        private static void OnCountriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((AddCountriesViewModel) ((AddCountries) d).DataContext).Countries = (CollectionViewSource) e.NewValue;
        }
    }
}
