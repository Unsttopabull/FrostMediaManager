using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Properties;
using Frost.Common.Util;

namespace Frost.DetectFeatures.Util {

    public class CodecIdBinding : INotifyPropertyChanged, IEquatable<CodecIdBinding>, IKeyValue {
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

        #region IKeyValue

        string IKeyValue.Key {
            get { return CodecId; }
        }

        string IKeyValue.Value {
            get { return Mapping; }
        }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(CodecIdBinding other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(CodecId, other.CodecId);
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