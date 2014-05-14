using System;
using System.Collections.Generic;
using System.IO;

namespace Frost.Common.Models.FeatureDetector {

    /// <summary>Represents the information about a file that has been detected by Feature Detector.</summary>
    public class FileDetectionInfo {

        /// <summary>Initializes a new instance of the <see cref="FileDetectionInfo"/> class.</summary>
        public FileDetectionInfo() {
            Subtitles = new List<SubtitleDetectionInfo>();
            Videos = new List<VideoDetectionInfo>();
            Audios = new List<AudioDetectionInfo>();
        }

        /// <summary>Initializes a new instance of the <see cref="FileDetectionInfo"/> class.</summary>
        /// <param name="fileName">Filename of the file.</param>
        /// <param name="extension">The extension without '.'.</param>
        /// <param name="folderPath">The folder path to the file.</param>
        /// <param name="fileSize">Size of the file in bytes.</param>
        public FileDetectionInfo(string fileName, string extension, string folderPath, long? fileSize) : this() {
            Name = fileName;
            Extension = extension;
            FolderPath = folderPath;
            Size = fileSize;
        }

        /// <summary>Initializes a new instance of the <see cref="FileDetectionInfo"/> class.</summary>
        /// <param name="info">The <see cref="FileInfo">FileInfo</see> instance of the file.</param>
        public FileDetectionInfo(FileInfo info) : this() {
            Extension = info.Extension;
            Name = Path.GetFileNameWithoutExtension(info.Name);
            FolderPath = info.DirectoryName + Path.DirectorySeparatorChar;
            Size = info.Length;
            LastAccessTime = info.LastAccessTime;
            CreateTime = info.CreationTime;
        }

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        public string Extension { get; set; }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E.avi</c>''}</example>
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

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        public string NameWithExtension {
            get { return Name + "." + Extension; }
        }

        /// <summary>Gets the full path to the file.</summary>
        /// <value>The full path to the file.</value>
        public string FullPath {
            get { return Path.Combine(FolderPath, NameWithExtension); }
        }

        /// <summary>Gets or sets the file create time.</summary>
        /// <value>The file create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>Gets or sets the file last access time.</summary>
        /// <value>The file last access time.</value>
        public DateTime LastAccessTime { get; set; }

        /// <summary>Gets or sets the information about subtitles in this file.</summary>
        /// <value>The subtitles in this file.</value>
        public List<SubtitleDetectionInfo> Subtitles { get; set; }

        /// <summary>Gets or sets the information about video tracks in this file.</summary>
        /// <value>The video tracks in this file.</value>
        public List<VideoDetectionInfo> Videos { get; set; }

        /// <summary>Gets or sets the information about audio tracks in this file.</summary>
        /// <value>The audio tracks in this file.</value>
        public List<AudioDetectionInfo> Audios { get; set; }
    }

}