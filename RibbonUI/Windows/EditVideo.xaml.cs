using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB.Files;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditVideo : Window {
        private bool _first = true;

        class Codec {
            public Codec(string name, string id) {
                Name = name;
                Id = id;
            }

            public string Name { get; set; }
            public string Id { get; set; }
        }

        public EditVideo() {
            InitializeComponent();
        }

        private void CodecSelectOnLoaded(object sender, RoutedEventArgs e) {
            List<Codec> lst = new List<Codec> {
                new Codec(TranslationManager.T("Unknown"), "unk"),
                new Codec("Windows Media Video", "wmv"),
                new Codec("Windows Media Video HD", "wmva"),
                new Codec("Nero Digital Standard", "nds"),
                new Codec("Nero Digital AVC", "ndx"),
                new Codec("MPEG-4", "mpeg4"),
                new Codec("MPEG-4 AVC (H.264)", "h264"),
                new Codec("MPEG-4 Part 14 (MP4)", "mp4"),
                new Codec("Microsoft ISO MPEG-4", "m4s2"),
                new Codec("PacketVideo Corporation MPEG-4", "pvmm"),
                new Codec("Dicas MPEGable MPEG-4", "dm4v"),
                new Codec("Based on XviD MPEG-4 (Xvix)", "xvix"),
                new Codec("Geox", "geox"),
                new Codec("DivX", "divx"),
                new Codec("DivX 2", "div3"),
                new Codec("DivX 3", "div3"),
                new Codec("DivX 4", "div4"),
                new Codec("DivX 5", "div5"),
                new Codec("DivX 6", "div6"),
                new Codec("DivX MPEG-4", "dx50"),
                new Codec("Xvid", "xvid"),
                new Codec("Windows Media Video 9 Advanced Profile (VC1)", "wvc1"),
                new Codec("Pinnacle PIM1", "pim1"),
                new Codec("MPEG-1 Video", "mpeg"),
                new Codec("MPEG-2 Video", "mpeg2"),
                new Codec("Matrox MPEG-2 (I-frame)", "mmes"),
                new Codec("LEADTools MPEG-2", "lmp2"),
                new Codec("Flash video", "flv"),
                new Codec("Etymonix MPEG-2 I-frame", "em2v"),
                new Codec("Advanced Video Codec (AVC)", "avc"),
                new Codec("Advanced Video Codec (AVC1)", "avc1"),
                new Codec("3ivX MPEG-4", "3ivx"),
                new Codec("ZyGo Video", "zygo"),
                new Codec("Apple QuickTime 1.x Video (Road pizza)", "rpza"),
                new Codec("Sorenson Media Video (Apple QuickTime)", "svq"),
                new Codec("Apple Graphics (SMC)", "smc"),
                new Codec("Avid JPEG", "avrn")
            };


            CodecSelect.ItemsSource = lst;

            string codecId = ((Video) DataContext).CodecId;
            Codec videoCodec = lst.FirstOrDefault(c => c.Id.Equals(codecId, StringComparison.InvariantCultureIgnoreCase));
            if (videoCodec != null) {
                CodecSelect.SelectedItem = videoCodec;
            }
        }

        private void CodecSelectOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            Video video = (Video) DataContext;
            Codec c = (Codec) CodecSelect.SelectedItem;

            if (_first) {
                _first = false;
                return;
            }

            if (c.Name == TranslationManager.T("Unknown")) {
                video.Codec = null;
                video.CodecId = null;
            }
            else {
                video.Codec = c.Name;
                video.CodecId = c.Id;
            }

            BindingExpression codecImgSource = CodecImg.GetBindingExpression(Image.SourceProperty);
            if (codecImgSource != null) {
                codecImgSource.UpdateTarget();
            }
        }

        private void CloseOnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void ResolutionSelectOnLoaded(object sender, RoutedEventArgs e) {
            List<string> resolutions = new List<string> {
                TranslationManager.T("Unknown"),
                "SDp",
                "SDi",
                "SD",
                "480p",
                "480i",
                "480",
                "540",
                "540p",
                "540i",
                "576p",
                "576i",
                "576",
                "720",
                "720p",
                "720i",
                "768",
                "768p",
                "768i",
                "1080p",
                "1080i",
                "1080"
            };

            ResolutionSelect.ItemsSource = resolutions;
        }

        private void StandardSelectOnLoaded(object sender, RoutedEventArgs e) {
            List<string> standards = new List<string> { "PAL", "NTSC", "SECAM" };
            ((ComboBox) sender).ItemsSource = standards;
        }

        private void OnColorSpaceSelectLoaded(object sender, RoutedEventArgs e) {
            List<string> colorSpaces = new List<string> {
                "YUV",
                "YIQ",
                "YPbPr",
                "YDbDr",
                "HSV",
                "HSL",
                "RGB",
                "sRGB",
                "CYMK"
            };

            ((ComboBox) sender).ItemsSource = colorSpaces;
        }
    }
}
