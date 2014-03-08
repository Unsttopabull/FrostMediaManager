using System.Windows;
using Frost.Common.Models;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditPerson.xaml</summary>
    public partial class EditPerson : Window {
        public static readonly DependencyProperty SelectedPersonProperty = DependencyProperty.Register("SelectedPerson", typeof(IPerson), typeof(EditPerson), new PropertyMetadata(default(IPerson), SelectedPersonChanged));

        public EditPerson() {
            InitializeComponent();
        }

        public IPerson SelectedPerson {
            get { return (IPerson) GetValue(SelectedPersonProperty); }
            set { SetValue(SelectedPersonProperty, value); }
        }

        private static void SelectedPersonChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((EditPersonViewModel) ((EditPerson) d).DataContext).SelectedPerson = (IPerson) args.NewValue;
        }

        private void EditPersonOnLoaded(object sender, RoutedEventArgs e) {
            ((EditPersonViewModel) DataContext).ParentWindow = (MainWindow) Owner;
        }
    }
}
