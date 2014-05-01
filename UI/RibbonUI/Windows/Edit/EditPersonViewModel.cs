using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using Microsoft.Win32;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Edit {
    class EditPersonViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _thumbnailPath;
        private MoviePerson _selectedPerson;

        public EditPersonViewModel() {
            ThumbnailSearchCommand = new RelayCommand(ThumbSearch);
            CloseCommand = new RelayCommand<Window>(window => {
                window.DialogResult = true;
                window.Close();
            });
            //OnClosedCommand = new RelayCommand<Window>(OnWindowClose);
        }

        public MainWindow ParentWindow { get; set; }

        public MoviePerson SelectedPerson {
            get { return _selectedPerson; }
            set {
                if (Equals(value, _selectedPerson)) {
                    return;
                }
                _selectedPerson = value;

                if (_selectedPerson != null) {
                    ThumbnailPath = _selectedPerson.Thumb;
                }
                OnPropertyChanged();
            }
        }

        public string ThumbnailPath {
            get { return _thumbnailPath; }
            set {
                if (value == _thumbnailPath) {
                    return;
                }
                _thumbnailPath = value;
                OnPropertyChanged();
            }
        }

        #region ICommands

        public ICommand ThumbnailSearchCommand { get; private set; }

        public ICommand<Window> CloseCommand { get; private set; }

        //public ICommand<Window> OnClosedCommand { get; private set; }

        #endregion

        private void ThumbSearch() {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                ThumbnailPath = ofd.FileName;
            }            
        }

        //private void OnWindowClose(Window window) {
        //    if (window.DialogResult == true) {
        //        return;
        //    }

        //    //DbEntityEntry personEntry = ParentWindow.Container.Entry(SelectedPerson);

        //    //if (personEntry.State != EntityState.Unchanged && personEntry.State != EntityState.Detached) {
        //    //    personEntry.Reload();
        //    //}
        //}

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
