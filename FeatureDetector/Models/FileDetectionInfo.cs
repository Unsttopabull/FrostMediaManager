using System.Collections.Generic;
using System.IO;

namespace Frost.DetectFeatures.Models {

    public class FileDetectionInfo {

        public FileDetectionInfo() {
            Subtitles = new List<SubtitleDetectionInfo>();
            Videos = new List<VideoDetectionInfo>();
            Audios = new List<AudioDetectionInfo>();
        }

        public FileDetectionInfo(string fileName, string extension, string folderPath, long? fileSize) : this() {
            Name = fileName;
            Extension = extension;
            FolderPath = folderPath;
            Size = fileSize;
        }

        public FileDetectionInfo(FileInfo info) {
            Extension = info.Extension;
            Name = Path.GetFileNameWithoutExtension(info.Name);
            FolderPath = info.DirectoryName + Path.DirectorySeparatorChar;
            Size = info.Length;
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

        public string NameWithExtension {
            get { return Name + "." + Extension; }
        }

        public string FullPath {
            get { return Path.Combine(FolderPath, NameWithExtension); }
        }

        public List<SubtitleDetectionInfo> Subtitles { get; set; }
        public List<VideoDetectionInfo> Videos { get; set; }
        public List<AudioDetectionInfo> Audios { get; set; }
    }

}