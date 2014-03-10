using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.Windows {
    public class EditVideoViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Codec> _codecs;
        private ObservableCollection<string> _resolutions;
        private ObservableCollection<string> _colorSpaces;
        private IVideo _selectedVideo;
        private Codec _codecId;

        public EditVideoViewModel() {

            #region Collection Initializers

            Codecs = new ObservableCollection<Codec>() {
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

            Resolutions = new ObservableCollection<string> {
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

            ColorSpaces = new ObservableCollection<string> {
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

            VideoStandards = new ObservableCollection<string> {
                "PAL",
                "NTSC",
                "SECAM"
            };

            #endregion

            CloseCommand = new RelayCommand<Window>(window => {
                window.DialogResult = true;
                window.Close();
            });
        }

        public class Codec {
            public Codec(string name, string id) {
                Name = name;
                Id = id;
            }

            public string Name { get; set; }
            public string Id { get; set; }
        }

        public IVideo SelectedVideo {
            get { return _selectedVideo; }
            set {
                if (Equals(value, _selectedVideo)) {
                    return;
                }
                _selectedVideo = value;

                if (_selectedVideo != null) {
                    Codec codec = Codecs.FirstOrDefault(c => c.Id == _selectedVideo.CodecId);

                    if (codec == null) {
                        codec = new Codec(_selectedVideo.Codec, _selectedVideo.CodecId);
                        Codecs.Add(codec);
                    }
                    SelectedCodec = codec;
                }

                OnPropertyChanged("FormattedVideoResolution");
                OnPropertyChanged();
            }
        }

        public Codec SelectedCodec {
            get { return _codecId; }
            set {
                if (value == _codecId) {
                    return;
                }
                _codecId = value;

                if (_codecId != null) {
                    if (value.Id == "unk") {
                        SelectedVideo.Codec = null;
                        SelectedVideo.CodecId = null;
                        return;
                    }

                    SelectedVideo.Codec = value.Name;
                    SelectedVideo.CodecId = value.Id;
                }

                OnPropertyChanged();
            }
        }

        public string FormattedVideoResolution {
            get {
                if (SelectedVideo == null) {
                    return null;
                }

                if (!string.IsNullOrEmpty(SelectedVideo.ResolutionName)) {
                    return SelectedVideo.ResolutionName;
                }

                if (!SelectedVideo.Resolution.HasValue) {
                    return null;
                }

                string resolution = SelectedVideo.Resolution.Value.ToString(CultureInfo.InvariantCulture);
                if (SelectedVideo.ScanType == ScanType.Interlaced) {
                    return resolution + "i";
                }

                if (SelectedVideo.ScanType == ScanType.Progressive) {
                    return resolution + "p";
                }

                return resolution;
            }

            set {
                if (SelectedVideo == null) {
                    return;
                }

                if (string.IsNullOrEmpty(value)) {
                    return;
                }

                if (value == "Unknown") {
                    SelectedVideo.Resolution = null;
                    SelectedVideo.ResolutionName = null;
                    SelectedVideo.ScanType = ScanType.Unknown;
                    return;
                }

                SelectedVideo.ResolutionName = value;

                int resolution;
                if (value.EndsWith("i")) {
                    if (!value.Equals("SDi", StringComparison.InvariantCultureIgnoreCase)) {
                        int.TryParse(value.Substring(0, value.Length - 1), out resolution);

                        SelectedVideo.Resolution = resolution;
                    }
                    SelectedVideo.ScanType = ScanType.Interlaced;
                }
                else if (value.EndsWith("p")) {
                    if (!value.Equals("SDp", StringComparison.InvariantCultureIgnoreCase)) {
                        int.TryParse(value.Substring(0, value.Length - 1), out resolution);

                        SelectedVideo.Resolution = resolution;
                    }
                    SelectedVideo.ScanType = ScanType.Progressive;
                }
                else {
                    if (!value.Equals("SD", StringComparison.InvariantCultureIgnoreCase)) {
                        int.TryParse(value, out resolution);

                        SelectedVideo.Resolution = resolution;
                    }

                    SelectedVideo.ScanType = ScanType.Unknown;
                }
            }
        }

        public ObservableCollection<Codec> Codecs {
            get { return _codecs; }
            set {
                if (Equals(value, _codecs)) {
                    return;
                }
                _codecs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Resolutions {
            get { return _resolutions; }
            set {
                if (Equals(value, _resolutions)) {
                    return;
                }
                _resolutions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ColorSpaces {
            get { return _colorSpaces; }
            set {
                if (Equals(value, _colorSpaces)) {
                    return;
                }
                _colorSpaces = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> VideoStandards { get; set; }

        public ICommand<Window> CloseCommand { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
