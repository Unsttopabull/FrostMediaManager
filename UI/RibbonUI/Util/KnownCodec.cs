using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Frost.RibbonUI.Properties;

namespace Frost.RibbonUI.Util {
    public class KnownCodec : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _codecId;
        private string _imagePath;
        private string _mapping;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public KnownCodec(string codecId, string imagePath) {
            CodecId = codecId;
            ImagePath = imagePath;
        }

        public KnownCodec(string filePath, bool isVideo) {
            ImagePath = "file://" + filePath;

            string codecId = Path.GetFileNameWithoutExtension(filePath);
            if (isVideo) {
                if (codecId != null) {
                    CodecId = codecId.Replace("vcodec_", "");
                }
            }
            else {
                if (codecId != null) {
                    CodecId = codecId.Replace("acodec_", "");
                }                
            }
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

        public string ImagePath {
            get { return _imagePath; }
            set {
                if (value == _imagePath) {
                    return;
                }
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public string Mapping {
            get { return _mapping; }
            set {
                if (value == _mapping) {
                    return;
                }
                _mapping = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("CodecId: {0}, Mapping: {1}", CodecId, Mapping);
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
