using System;
using System.Collections.Generic;
using System.Linq;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtSubtitle : ChangeTrackingProxy<XjbPhpMovie>, ISubtitle, IEquatable<XtSubtitle> {
        private readonly int _index;
        private readonly string _xtPathRoot;
        private readonly string _xtreamerPath;
        private XtSubtitleFile _subtitleFile;
        private ILanguage _language;
        private string _path;

        public XtSubtitle(XjbPhpMovie movie, int index, string xtreamerPath) : base(movie){
            _index = index;
            _xtreamerPath = xtreamerPath;
            _xtPathRoot = System.IO.Path.GetPathRoot(_xtreamerPath);

            if (movie.Subtitles.Count > index) {
                _path = movie.Subtitles[index];
            }

            OriginalValues = new Dictionary<string, object> { { "Language", Entity.SubtitleLanguage } };
        }

        #region Not Implemented

        public long Id {
            get { return 0; }
        }

        public long? PodnapisiId {
            get { return 0; }
            set { }
        }

        public long? OpenSubtitlesId {
            get { return 0; }
            set { }
        }

        public string MD5 {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired {
            get { return false; }
            set { }
        }

        #endregion

        public IFile File {
            get { return _subtitleFile ?? (_subtitleFile = new XtSubtitleFile(Entity, _index, _xtreamerPath)); }
        }

        public ILanguage Language {
            get { return _language ?? (_language = XtLanguage.FromEnglishName(Entity.SubtitleLanguage)); }
            set {
                _language = value;

                if (value != null) {
                    Entity.SubtitleLanguage = value.Name;
                }
                TrackChanges(_language != null ? _language.Name : null);
            }
        }

        public string Path {
            get { return _path ?? (_path = GetSubtitlePath()); }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "File":
                    case "Language":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public string GetSubtitlePath() {
            if (File == null) {
                return null;
            }

            string fullPath = File.FullPath;
            if (System.IO.Path.GetPathRoot(fullPath) != _xtPathRoot) {
                return null;
            }

            fullPath = fullPath.Replace(_xtPathRoot ?? "", "").TrimStart('\\');
            fullPath = fullPath.Remove(0, fullPath.IndexOfAny(new[] { '\\', '/' }));

            string fullPathSearch = fullPath.Replace('/', ' ').Replace('\\', ' ');
            return Entity.Subtitles.FirstOrDefault(subFile =>
                                                   subFile != null &&
                                                   string.Equals(subFile.Replace('/', ' ').Replace('\\', ' '), fullPathSearch)
                                                   );
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtSubtitle other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(Path, other.Path, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Language != null
                ? Language.ToString()
                : File.ToString();
        }
    }
}