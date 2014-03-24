using System;
using System.Collections.Generic;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtFile : IFile {
        private XjbPhpMovie _movie;

        public XtFile(XjbPhpMovie movie) {
            _movie = movie;
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get { throw new NotImplementedException(); }
        }

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        public string Extension { get; set; }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E</c>''}</example>
        public string Name { get; set; }

        /// <summary>Gets or sets the path to the folder that contains this file</summary>
        /// <value>The full path to the folder that contains this file with trailing '/' without quotes (" or ')</value>
        /// <example>\eg{
        /// 	<list type="bullet">
        ///         <item><description>''<c>C:/Movies/</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/</c>''</description></item>
        /// 	</list>}
        /// </example>
        public string FolderPath { get; set; }

        /// <summary>Gets or sets the file size in bytes.</summary>
        /// <value>The file size in bytes.</value>
        public long? Size { get; set; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        public DateTime DateAdded { get; set; }

        /// <summary>Gets or sets the details about audio streams in this file</summary>
        /// <value>The details about audio streams in this file</value>
        public IEnumerable<IAudio> AudioDetails { get; private set; }

        /// <summary>Gets or sets the details about video streams in this file</summary>
        /// <value>The details about video streams in this file</value>
        public IEnumerable<IVideo> VideoDetails { get; private set; }

        /// <summary>Gets or sets the details about subtitles in this file</summary>
        /// <value>The details about subtitles in this file</value>
        public IEnumerable<ISubtitle> Subtitles { get; private set; }

        /// <summary>Gets the full path to the file.</summary>
        /// <value>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</value>
        public string FullPath { get; private set; }

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        public string NameWithExtension { get; private set; }
    }
}
