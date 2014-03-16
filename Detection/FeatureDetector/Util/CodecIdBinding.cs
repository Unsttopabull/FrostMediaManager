using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Properties;

namespace Frost.DetectFeatures.Util {

    public class CodecIdBinding : INotifyPropertyChanged {
        private string _codecId;
        private string _mapping;

        public CodecIdBinding(string codecId, string mapping) {
            _codecId = codecId;
            _mapping = mapping;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CodecId {
            get { return _codecId; }
            set {
                if (value == _codecId) {
                    return;
                }

                if (!string.IsNullOrEmpty(value)) {
                    _codecId = value;
                }
                OnPropertyChanged();
            }
        }

        public string Mapping {
            get { return _mapping; }
            set {
                if (value == _mapping) {
                    return;
                }

                if (!string.IsNullOrEmpty(value)) {
                    _mapping = value;
                }
                OnPropertyChanged();
            }
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