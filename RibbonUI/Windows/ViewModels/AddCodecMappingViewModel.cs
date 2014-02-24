using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Microsoft.Win32;
using RibbonUI.Commands;
using RibbonUI.Util;

namespace RibbonUI.Windows.ViewModels {

    public class AddCodecMappingViewModel : DependencyObject, INotifyPropertyChanged, IDisposable {
        private readonly bool _isVideo;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isError;
        private string _searchText;
        private ICollectionView _collectionView;
        private KnownCodec _selectedCodec;
        private readonly IDisposable _searchTextObservable;

        public AddCodecMappingViewModel() {
            AddCommand = new RelayCommand<AddCodecMapping>(acm => {
                acm.DialogResult = true;
                acm.Close();
            }, acm => SelectedCodec != null && !IsError && !string.IsNullOrEmpty(SelectedCodec.CodecId) && !string.IsNullOrEmpty(SelectedCodec.Mapping) && !string.IsNullOrEmpty(SelectedCodec.ImagePath));

            CancelCommand = new RelayCommand<Window>(w => {
                w.DialogResult = false;
                w.Close();
            });

            SeachNewLogoCommand = new RelayCommand<string>(mapping => {
                OpenFileDialog ofd = new OpenFileDialog {
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = TranslationManager.T("Image Files") + " (*.bmp, *.jpg, *.jpeg, *.png, *.gif, *.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff"
                };

                if (ofd.ShowDialog() == true) {
                    SelectedCodec.ImagePath = "file://" + ofd.FileName;
                }
            }, mapping => !CheckCodecExists(mapping));

            _searchTextObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                                              .Where(ep => ep.EventArgs.PropertyName == "SearchText")
                                              .Throttle(TimeSpan.FromSeconds(0.5))
                                              .ObserveOn(SynchronizationContext.Current)
                                              .Subscribe(obj => _collectionView.Refresh());

            SelectedCodec = new KnownCodec(null, null);
        }

        public AddCodecMappingViewModel(bool isVideo) : this() {
            _isVideo = isVideo;
            GetKnownCodecs(isVideo);
        }

        public string SearchText {
            get { return _searchText; }
            set {
                if (value == _searchText) {
                    return;
                }
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<KnownCodec> KnownCodecs { get; set; }

        public bool IsError {
            get { return _isError; }
            set {
                if (value.Equals(_isError)) {
                    return;
                }
                _isError = value;
                OnPropertyChanged();
            }
        }

        public bool IsNew {
            get {
                if (CheckCodecExists(SelectedCodec.Mapping)) {
                    SelectedCodec.ImagePath = KnownCodecs.Where(kc => kc.CodecId.Equals(SelectedCodec.Mapping, StringComparison.CurrentCultureIgnoreCase))
                                                         .Select(kc => kc.ImagePath)
                                                         .FirstOrDefault();
                    return false;
                }
                SelectedCodec.ImagePath = null;
                return true;
            }
        }

        public KnownCodec SelectedCodec {
            get { return _selectedCodec; }
            set {
                if (Equals(value, _selectedCodec)) {
                    return;
                }

                if (_selectedCodec != null) {
                    _selectedCodec.PropertyChanged -= SelectedCodecChanged;
                }

                _selectedCodec = value;
                _selectedCodec.PropertyChanged += SelectedCodecChanged;

                OnPropertyChanged("IsNew");
                OnPropertyChanged();
            }
        }

        private void SelectedCodecChanged(object sender, PropertyChangedEventArgs args) {
            switch (args.PropertyName) {
                case "CodecId":
                    if (CheckCodecExists(_selectedCodec.CodecId)) {
                        IsError = true;
                        return;
                    }
                    IsError = false;
                    break;
                case "Mapping":
                    OnPropertyChanged("IsNew");
                    break;
            }
        }

        public ICommand AddCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand SeachNewLogoCommand { get; private set; }

        private void GetKnownCodecs(bool isVideo) {
            if (!Directory.Exists("Images/FlagsE")) {
                KnownCodecs.Clear();
                return;
            }

            DirectoryInfo di = new DirectoryInfo("Images/FlagsE");
            KnownCodecs = isVideo
                              ? new ObservableCollection<KnownCodec>(di.EnumerateFiles("vcodec_*.png").Select(fi => new KnownCodec(fi.FullName, true)))
                              : new ObservableCollection<KnownCodec>(di.EnumerateFiles("acodec_*.png").Select(fi => new KnownCodec(fi.FullName, false)));

            _collectionView = CollectionViewSource.GetDefaultView(KnownCodecs);
            _collectionView.Filter = Filter;

            OnPropertyChanged("KnownCodecs");
        }

        private bool Filter(object obj) {
            return ((KnownCodec) obj).CodecId.IndexOf(SearchText ?? "", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private bool CheckCodecExists(string value) {
            if (_isVideo) {
                return FileFeatures.VideoCodecIdMappings.ContainsKey(value) || KnownCodecs.Any(kc => kc.CodecId == value);
            }
            return FileFeatures.AudioCodecIdMappings.ContainsKey(value) || KnownCodecs.Any(kc => kc.CodecId == value);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool destructor) {
            if (_searchTextObservable != null) {
                _searchTextObservable.Dispose();
            }

            if (_selectedCodec != null) {
                _selectedCodec.PropertyChanged -= SelectedCodecChanged;
            }

            if (!destructor) {
                GC.SuppressFinalize(this);
            }
        }

        ~AddCodecMappingViewModel() {
            Dispose(true);
        }
    }

}