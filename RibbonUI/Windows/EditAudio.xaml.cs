using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Frost.Common.Models.DB.MovieVo.Files;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditAudio : Window {
        private bool _first = true;

        class Codec {
            /// <summary>Initializes a new instance of the <see cref="EditAudio"/> class.</summary>
            public Codec(string name, string id) {
                Name = name;
                Id = id;
            }

            public string Name { get; set; }
            public string Id { get; set; }
        }

        public EditAudio() {
            InitializeComponent();
        }

        private void CodecSelectOnLoaded(object sender, RoutedEventArgs e) {
            List<Codec> lst = new List<Codec>();

            lst.Add(new Codec("Unknown", "unk"));
            lst.Add(new Codec("Windows Media Audio", "wma"));
            lst.Add(new Codec("Windows Media Audio HD", "wmahd"));
            lst.Add(new Codec("Windows Media Audio Pro", "wmapro"));
            lst.Add(new Codec("Dolby Digital TrueHD", "truehd"));
            lst.Add(new Codec("MPEG-1 Audio Layer I (MP1)", "mp1"));
            lst.Add(new Codec("MPEG-1 Audio Layer II (MP2)", "mp2"));
            lst.Add(new Codec("MPEG-1 Audio layer 3 (MP3)", "mp3"));
            lst.Add(new Codec("DTS-HD Master Audio", "dtsma"));
            lst.Add(new Codec("DTS-HD Master High Resolution Audio", "dtshr"));
            lst.Add(new Codec("Digital Theater Systems (DTS)", "dts"));
            lst.Add(new Codec("Dolby Digital", "dd"));
            lst.Add(new Codec("Dolby AC-3", "ac3"));
            lst.Add(new Codec("Ogg Vorbis", "ogg"));
            lst.Add(new Codec("Free Lossless Audio Codec (FLAC)", "flac"));
            lst.Add(new Codec("APE Lossles audio codec (Monkey's Audio)", "ape"));
            lst.Add(new Codec("Audio Interchange File Format (AIFF)", "aiff"));
            lst.Add(new Codec("Audio Interchange File Format Compressed (AIFC)", "aifc"));
            lst.Add(new Codec("Advanced Audio Coding (AAC)", "aac"));

            CodecSelect.ItemsSource = lst;

            string codecId = ((Audio) DataContext).CodecId;
            Codec audioCodec = lst.FirstOrDefault(c => c.Id.Equals(codecId, StringComparison.InvariantCultureIgnoreCase));
            if (audioCodec != null) {
                CodecSelect.SelectedItem = audioCodec;
            }
        }

        private void CodecSelectOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            Audio audio = (Audio) DataContext;
            Codec c = (Codec) CodecSelect.SelectedItem;

            if (_first) {
                _first = false;
                return;
            }

            if (c.Name == "Unknown") {
                audio.Codec = null;
                audio.CodecId = null;
                return;
            }

            audio.Codec = c.Name;
            audio.CodecId = c.Id;

            BindingExpression codecImgSource = CodecImg.GetBindingExpression(Image.SourceProperty);
            if (codecImgSource != null) {
                codecImgSource.UpdateTarget();
            }
        }
    }
}
