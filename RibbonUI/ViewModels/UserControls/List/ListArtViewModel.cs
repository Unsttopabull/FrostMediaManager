using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.Common.Models;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.UserControls.List {
    public class ListArtViewModel : INotifyPropertyChanged {
        private ObservableCollection<IArt> _art;
        public event PropertyChangedEventHandler PropertyChanged;

        public ListArtViewModel() {
            RemoveCommand = new RelayCommand<IArt>(RemoveOnClick, art => art != null);
            AddCommand = new RelayCommand<IArt>(AddOnClick, art => art != null);
        }

        public ObservableCollection<IArt> Art {
            get { return _art; }
            set {
                if (Equals(value, _art)) {
                    return;
                }
                _art = value;
                OnPropertyChanged();
            }
        }

        public ICommand<IArt> RemoveCommand { get; private set; }

        public ICommand<IArt> AddCommand { get; private set; }

        private void RemoveOnClick(IArt art) {
            
        }

        private void AddOnClick(IArt art) {
            
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
