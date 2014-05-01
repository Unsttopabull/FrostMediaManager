using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Add {
    class SelectLanguageViewModel : INotifyPropertyChanged {
        private IEnumerable<MovieLanguage> _languages;
        public event PropertyChangedEventHandler PropertyChanged;

        public SelectLanguageViewModel() {
            if (TranslationManager.IsInDesignMode) {
                Languages = UIHelper.GetLanguages().Select(l => new MovieLanguage(l));
            }

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

        public IEnumerable<MovieLanguage> Languages {
            get { return _languages; }
            set {
                if (Equals(value, _languages)) {
                    return;
                }
                _languages = value;

                if (_languages != null) {
                    ICollectionView collectionView = CollectionViewSource.GetDefaultView(_languages);
                    collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
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
