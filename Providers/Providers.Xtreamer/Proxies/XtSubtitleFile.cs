using System;
using System.IO;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtSubtitleFile : Proxy<XjbPhpMovie>, IFile {
        private readonly int _index;
        private readonly string _xtreamerPath;

        public XtSubtitleFile(XjbPhpMovie movie, int index, string xtreamerPath) : base(movie){
            _index = index;
            _xtreamerPath = xtreamerPath;
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get { return false; }
        }

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        public string Extension {
            get { return Path.GetExtension(FullPath); }
        }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E</c>''}</example>
        public string Name {
            get { return Path.GetFileNameWithoutExtension(FullPath); }
        }

        /// <summary>Gets or sets the path to the folder that contains this file</summary>
        /// <value>The full path to the folder that contains this file with trailing '/' without quotes (" or ')</value>
        /// <example>\eg{
        /// 	<list type="bullet">
        ///         <item><description>''<c>C:/Movies/</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/</c>''</description></item>
        /// 	</list>}
        /// </example>
        public string FolderPath {
            get {
                string path = Path.GetDirectoryName(FullPath);
                return !string.IsNullOrEmpty(path) 
                    ? path + Path.DirectorySeparatorChar
                    : null;
            }
        }

        /// <summary>Gets or sets the file size in bytes.</summary>
        /// <value>The file size in bytes.</value>
        public long? Size { get; private set; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        public DateTime DateAdded { get; private set; }

        /// <summary>Gets the full path to the file.</summary>
        /// <value>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</value>
        public string FullPath {
            get {
                if (Entity.Subtitles.Count > _index) {
                    return _xtreamerPath + Entity.Subtitles[_index];
                }
                return null;
            }
        }

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        public string NameWithExtension {
            get { return Path.GetFileName(FullPath); }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return NameWithExtension;
        }
    }
}
