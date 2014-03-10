using System.Collections.Generic;
using System.Windows;
using Frost.Common.Models;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddCountries : Window {
        public static readonly DependencyProperty CountriesProperty = DependencyProperty.Register("Countries", typeof(IEnumerable<ICountry>), typeof(AddCountries), new PropertyMetadata(default(IEnumerable<ICountry>), OnCountriesChanged));

        public AddCountries() {
            InitializeComponent();
        }

        public IEnumerable<ICountry> Countries {
            get { return (IEnumerable<ICountry>) GetValue(CountriesProperty); }
            set { SetValue(CountriesProperty, value); }
        }

        private static void OnCountriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((AddCountriesViewModel) ((AddCountries) d).DataContext).Countries = (IEnumerable<ICountry>) e.NewValue;
        }
    }
}
