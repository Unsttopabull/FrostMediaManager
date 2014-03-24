﻿using System;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    class XtGenre : IGenre, IEquatable<IGenre> {
        private readonly XjbPhpGenre _genre;

        public XtGenre(XjbPhpGenre genre) {
            _genre = genre;
        }

        public long Id {
            get { return _genre.Id; }
        }

        public string Name {
            get { return _genre.Name; }
            set { _genre.Name = value; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case"Name":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns> true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IGenre other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return other.Name == Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            IGenre genre = obj as IGenre;
            return genre != null && Equals(genre);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            return (_genre != null ? _genre.GetHashCode() : 0);
        }
    }
}
