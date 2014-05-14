using System.Windows;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.Windows.Edit {

    /// <summary>Interaction logic for EditPerson.xaml</summary>
    public partial class EditPerson : Window {
        public static readonly DependencyProperty SelectedPersonProperty = DependencyProperty.Register("SelectedPerson", typeof(MoviePerson), typeof(EditPerson), new PropertyMetadata(default(MoviePerson), SelectedPersonChanged));

        public EditPerson() {
            InitializeComponent();
        }

        public MoviePerson SelectedPerson {
            get { return (MoviePerson) GetValue(SelectedPersonProperty); }
            set { SetValue(SelectedPersonProperty, value); }
        }

        private static void SelectedPersonChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((EditPersonViewModel) ((EditPerson) d).DataContext).SelectedPerson = (MoviePerson) args.NewValue;
        }

        private void EditPersonOnLoaded(object sender, RoutedEventArgs e) {
            ((EditPersonViewModel) DataContext).ParentWindow = (MainWindow) Owner;
        }
    }
}
