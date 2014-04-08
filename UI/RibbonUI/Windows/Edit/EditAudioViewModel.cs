using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design.Fakes;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {
    class EditAudioViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private MovieAudio _selectedAudio;
        private Codec _selectedCodec;
        private bool _channelInfoEditable;

        public EditAudioViewModel() {
            Codecs = new ObservableCollection<Codec> {
                new Codec(TranslationManager.T("Unknown"), "unk"),
                new Codec("Windows Media Audio", "wma"),
                new Codec("Windows Media Audio HD", "wmahd"),
                new Codec("Windows Media Audio Pro", "wmapro"),
                new Codec("Dolby Digital TrueHD", "truehd"),
                new Codec("MPEG-1 Audio Layer I (MP1)", "mp1"),
                new Codec("MPEG-1 Audio Layer II (MP2)", "mp2"),
                new Codec("MPEG-1 Audio layer 3 (MP3)", "mp3"),
                new Codec("DTS-HD Master Audio", "dtsma"),
                new Codec("DTS-HD Master High Resolution Audio", "dtshr"),
                new Codec("Digital Theater Systems (DTS)", "dts"),
                new Codec("Dolby Digital", "dd"),
                new Codec("Dolby AC-3", "ac3"),
                new Codec("Ogg Vorbis", "ogg"),
                new Codec("Free Lossless Audio Codec (FLAC)", "flac"),
                new Codec("APE Lossles audio codec (Monkey's Audio)", "ape"),
                new Codec("Audio Interchange File Format (AIFF)", "aiff"),
                new Codec("Audio Interchange File Format Compressed (AIFC)", "aifc"),
                new Codec("Advanced Audio Coding (AAC)", "aac")
            };

            CloseCommand = new RelayCommand<Window>(window => {
                window.DialogResult = true;
                window.Close();
            });

            SelectedLanguageChanged = new RelayCommand<ILanguage>(language => {
                SelectedAudio.Language = language;
            });

            if (TranslationManager.IsInDesignMode) {
                SelectedAudio = new MovieAudio(new FakeAudio());
            }
        }

        public bool ChannelInfoEditable {
            get { return _channelInfoEditable; }
            private set {
                if (value.Equals(_channelInfoEditable)) {
                    return;
                }
                _channelInfoEditable = value;
                OnPropertyChanged();
            }
        }

        public MovieAudio SelectedAudio {
            get { return _selectedAudio; }
            set {
                if (Equals(value, _selectedAudio)) {
                    return;
                }
                _selectedAudio = value;

                if (_selectedAudio != null) {
                    ChannelInfoEditable = _selectedAudio["ChannelSetup"] || _selectedAudio["NumberOfChannels"];

                    if (_selectedAudio.CodecId != null) {
                        Codec audioCodec = Codecs.FirstOrDefault(c => c.Id.Equals(_selectedAudio.CodecId, StringComparison.InvariantCultureIgnoreCase));
                        if (audioCodec != null) {
                            SelectedCodec = audioCodec;
                        }                    
                    }
                }
                OnPropertyChanged();
            }
        }

        public Codec SelectedCodec {
            get { return _selectedCodec; }
            set {
                if (Equals(value, _selectedCodec)) {
                    return;
                }
                _selectedCodec = value;

                if (_selectedCodec != null && SelectedAudio != null) {
                    if (_selectedCodec.Id == "unk") {
                        SelectedAudio.Codec = null;
                        SelectedAudio.CodecId = null;
                        return;
                    }

                    SelectedAudio.Codec = _selectedCodec.Name;
                    SelectedAudio.CodecId = _selectedCodec.Id;                
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Codec> Codecs { get; set; }
        public ICommand<Window> CloseCommand { get; private set; }
        public ICommand<ILanguage> SelectedLanguageChanged { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
