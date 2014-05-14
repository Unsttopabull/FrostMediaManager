using System.Collections.Generic;
using System.Windows;
using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Windows.Add {

    /// <summary>Interaction logic for AddGenre.xaml</summary>
    public partial class AddGenre : Window {
        public static readonly DependencyProperty GenresProperty = DependencyProperty.Register("Genres", typeof(IEnumerable<IGenre>), typeof(AddGenre), new PropertyMetadata(default(IEnumerable<IGenre>), OnGenresChanged));

        public AddGenre() {
            InitializeComponent();
        }

        public IEnumerable<IGenre> Genres {
            get { return (IEnumerable<IGenre>) GetValue(GenresProperty); }
            set { SetValue(GenresProperty, value); }
        }

        private static void OnGenresChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((AddGenreViewModel) ((AddGenre) d).DataContext).Genres = (IEnumerable<IGenre>) e.NewValue;
        }
    }
}
