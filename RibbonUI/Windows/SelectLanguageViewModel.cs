using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Frost.Common.Models;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Properties;

namespace RibbonUI.Windows {
    class SelectLanguageViewModel : INotifyPropertyChanged {
        private IEnumerable<ILanguage> _languages;
        public event PropertyChangedEventHandler PropertyChanged;

        public SelectLanguageViewModel() {
            AddCommand = new RelayCommand<Window>(w => {
                w.DialogResult = true;
                w.Close();
            });

            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });
        }

        public ICommand<Window> AddCommand { get; private set; }
        public ICommand<Window> CancelCommand { get; private set; }

        public IEnumerable<ILanguage> Languages {
            get { return _languages; }
            set {
                if (Equals(value, _languages)) {
                    return;
                }
                _languages = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
