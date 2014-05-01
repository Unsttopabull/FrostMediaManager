using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Add {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddStudios : Window {
        public static readonly DependencyProperty StudiosProperty = DependencyProperty.Register("Studios", typeof(ObservableCollection<MovieStudio>), typeof(AddStudios), new PropertyMetadata(default(ObservableCollection<MovieStudio>), OnStudiosChanged));

        public AddStudios() {
            InitializeComponent();
        }

        public ObservableCollection<MovieStudio> Studios {
            get { return (ObservableCollection<MovieStudio>) GetValue(StudiosProperty); }
            set { SetValue(StudiosProperty, value); }
        }

        private static void OnStudiosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((AddStudiosViewModel) ((AddStudios) d).DataContext).Studios = (ObservableCollection<MovieStudio>) e.NewValue;
        }

        private void StudiosListSelectedChanged(object sender, SelectionChangedEventArgs e) {
            NewStudioName.Text = null;
        }

        private void NewStudioNameOnTextChanged(object sender, TextChangedEventArgs e) {
            if (StudiosList.SelectedIndex != -1) {
                StudiosList.SelectedIndex = -1;
            }
        }

        private void AddStudiosOnClosed(object sender, EventArgs e) {
            if (DataContext != null) {
                ((IDisposable)DataContext).Dispose();
            }
        }
    }

}