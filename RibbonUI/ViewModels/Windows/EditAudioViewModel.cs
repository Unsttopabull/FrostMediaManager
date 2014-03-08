using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Annotations;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Files;
using Frost.XamlControls.Commands;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.Windows {
    class EditAudioViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _first = true;
        private string _codecId;
        private Audio _selectedAudio;

        public EditAudioViewModel() {
            CodecSelectOnLoadCommand = new RelayCommand<ComboBox>(CodecSelectOnLoaded);
            CodecSelectSelectionChangedCommand = new RelayCommand<Codec>(CodecSelectOnSelectionChanged, codec => codec != null);
            CloseCommand = new RelayCommand<Window>(window => {
                window.DialogResult = true;
                window.Close();
            });

            SelectedLanguageChanged = new RelayCommand<Language>(Execute);
        }

        private void Execute(Language language) {
            SelectedAudio.Language = language;
            OnPropertyChanged("SelectedAudio");
        }

        public string CodecId {
            get { return _codecId; }
            set {
                if (value == _codecId) {
                    return;
                }
                _codecId = value;
                OnPropertyChanged();
            }
        }

        public Audio SelectedAudio {
            get { return _selectedAudio; }
            set {
                if (Equals(value, _selectedAudio)) {
                    return;
                }
                _selectedAudio = value;

                if (_selectedAudio != null) {
                    CodecId = _selectedAudio.CodecId;
                }
                OnPropertyChanged();
            }
        }

        public ICommand<ComboBox> CodecSelectOnLoadCommand { get; private set; }
        public ICommand<Codec> CodecSelectSelectionChangedCommand { get; private set; }
        public ICommand<Window> CloseCommand { get; private set; }

        public ICommand<Language> SelectedLanguageChanged { get; private set; }


        private void CodecSelectOnLoaded(ComboBox cb) {
            List<Codec> lst = new List<Codec> {
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

            cb.ItemsSource = lst;

            string codecId = SelectedAudio.CodecId;
            Codec audioCodec = lst.FirstOrDefault(c => c.Id.Equals(codecId, StringComparison.InvariantCultureIgnoreCase));
            if (audioCodec != null) {
                cb.SelectedItem = audioCodec;
            }
        }

        private void CodecSelectOnSelectionChanged(Codec c) {
            if (_first) {
                _first = false;
                return;
            }

            if (c.Id == "unk") {
                SelectedAudio.Codec = null;
                SelectedAudio.CodecId = null;
                return;
            }

            SelectedAudio.Codec = c.Name;
            SelectedAudio.CodecId = c.Id;

            CodecId = SelectedAudio.CodecId;
            //BindingExpression codecImgSource = CodecImg.GetBindingExpression(Image.SourceProperty);
            //if (codecImgSource != null) {
            //    codecImgSource.UpdateTarget();
            //}
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal class Codec {
            /// <summary>Initializes a new instance of the <see cref="EditAudio"/> class.</summary>
            public Codec(string name, string id) {
                Name = name;
                Id = id;
            }

            public string Name { get; set; }
            public string Id { get; set; }
        }
    }
}
