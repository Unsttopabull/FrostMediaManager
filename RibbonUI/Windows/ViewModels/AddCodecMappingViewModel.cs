using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Frost.Common.Annotations;
using RibbonUI.Commands;
using RibbonUI.Util;

namespace RibbonUI.Windows.ViewModels {
    public class AddCodecMappingViewModel : DependencyObject, INotifyPropertyChanged {
        private string _newCodecId;
        private string _newCodecIdLogo;
        private bool _isError;
        public event PropertyChangedEventHandler PropertyChanged;

        public AddCodecMappingViewModel() {
            AddCommand = new RelayCommand<AddCodecMapping>(acm => acm.Close(), OnCanExecute);
        }

        private bool OnCanExecute(AddCodecMapping acm) {
            return SelectedCodec != null && !IsError && !string.IsNullOrEmpty(SelectedCodec.CodecId) && !string.IsNullOrEmpty(SelectedCodec.ImagePath);
        }

        public AddCodecMappingViewModel(bool isVideo) : this() {
            GetKnownCodecs(isVideo);
        }

        private void GetKnownCodecs(bool isVideo) {
            if (!Directory.Exists("Images/FlagsE")) {
                KnownCodecs.Clear();
                return;
            }

            DirectoryInfo di = new DirectoryInfo("Images/FlagsE");
            KnownCodecs = isVideo
                ? new ObservableCollection<KnownCodec>(di.EnumerateFiles("vcodec_*.png").Select(fi => new KnownCodec(fi.FullName, true)))
                : new ObservableCollection<KnownCodec>(di.EnumerateFiles("acodec_*.png").Select(fi => new KnownCodec(fi.FullName, false)));

            OnPropertyChanged("KnownCodecs");
        }

        public ObservableCollection<KnownCodec> KnownCodecs { get; set; }

        public string NewCodecId {
            get { return _newCodecId; }
            set {
                if (Equals(value, _newCodecId)) {
                    return;
                }

                _newCodecId = value;

                if (string.IsNullOrEmpty(value)) {
                    return;
                }

                SelectedCodec = !string.IsNullOrEmpty(NewCodecIdLogo)
                    ? new KnownCodec(value, NewCodecIdLogo)
                    : new KnownCodec(value, null);

                OnPropertyChanged("SelectedCodec");
            }
        }

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

        public string NewCodecIdLogo {
            get { return _newCodecIdLogo; }
            set {
                if (Equals(value, _newCodecIdLogo)) {
                    return;
                }

                _newCodecIdLogo = value;

                if (!string.IsNullOrEmpty(NewCodecId)) {
                    SelectedCodec.ImagePath = value;
                    OnPropertyChanged("SelectedCodec");
                }
            }
        }

        public KnownCodec SelectedCodec { get; set; }

        public ICommand AddCommand { get; private set; }

        public void CheckCodecExists(EventPattern<TextChangedEventArgs> args) {
            TextBox tb = (TextBox) args.EventArgs.Source;

            string newCodec = tb.Text;
            if (KnownCodecs.Any(kc => kc.CodecId.Equals(newCodec, StringComparison.CurrentCultureIgnoreCase))) {
                IsError = true;
            }
            else if(IsError) {
                IsError = false;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void CheckCodecExists2(object sender, TextChangedEventArgs e) {
            TextBox tb = (TextBox) e.Source;

            string newCodec = tb.Text;
            if (KnownCodecs.Any(kc => kc.CodecId.Equals(newCodec, StringComparison.CurrentCultureIgnoreCase))) {
                IsError = true;
            }
            else if(IsError) {
                IsError = false;
            }
        }
    }
}
