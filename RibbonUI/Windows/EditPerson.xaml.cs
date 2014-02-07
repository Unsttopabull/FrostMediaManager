using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Windows;
using Microsoft.Win32;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditPerson.xaml</summary>
    public partial class EditPerson : Window {
        public EditPerson() {
            InitializeComponent();
        }

        private void ThumbSearchOnClick(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                ThumbBox.Text = ofd.FileName;
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void OnWindowClose(object sender, EventArgs e) {
            if (DialogResult == true) {
                return;
            }

            DbEntityEntry personEntry = ((MainWindow) Owner).Container.Entry(DataContext);

            if (personEntry.State != EntityState.Unchanged && personEntry.State != EntityState.Detached) {
                personEntry.Reload();
            }
        }
    }
}
