using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util;

namespace RibbonUI.Windows {
    public class ProviderSelectViewModel : INotifyPropertyChanged {
        private ObservableCollection<Plugin> _providers;
        public event PropertyChangedEventHandler PropertyChanged;

        public ProviderSelectViewModel() {
            Providers = new ObservableCollection<Plugin>(App.Systems);
            SelectProviderCommand = new RelayCommand<Plugin>(OnSelectProvider, provider => provider != null);
        }

        public ObservableCollection<Plugin> Providers {
            get { return _providers; }
            set {
                if (Equals(value, _providers)) {
                    return;
                }
                _providers = value;
                OnPropertyChanged();
            }
        }

        public ICommand<Plugin> SelectProviderCommand { get; private set; }
        public Window Window { get; set; }

        private void OnSelectProvider(Plugin obj) {
            Window.Tag = true;
            Window.Close();

            Application.Current.MainWindow = new MainWindow(obj.AssemblyPath);
            Application.Current.MainWindow.Show();
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
