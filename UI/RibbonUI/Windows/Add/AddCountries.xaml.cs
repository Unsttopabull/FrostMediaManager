using System.Collections.Generic;
using System.Windows;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Add {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddCountries : Window {
        public static readonly DependencyProperty CountriesProperty = DependencyProperty.Register("Countries", typeof(IEnumerable<MovieCountry>), typeof(AddCountries), new PropertyMetadata(default(IEnumerable<MovieCountry>), OnCountriesChanged));

        public AddCountries() {
            InitializeComponent();
        }

        public IEnumerable<MovieCountry> Countries {
            get { return (IEnumerable<MovieCountry>) GetValue(CountriesProperty); }
            set { SetValue(CountriesProperty, value); }
        }

        private static void OnCountriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((AddCountriesViewModel) ((AddCountries) d).DataContext).Countries = (IEnumerable<MovieCountry>) e.NewValue;
        }
    }
}
