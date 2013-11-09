namespace Frost.MediaInfo.Output.Properties.General {
    public class FileInfo {
        private readonly Media _media;

        public FileInfo(Media media) {
            FileSizeInfo = new FileSizeInfo(media);
            _media = media;
        }

        /// <summary>The time that the file was created on the file system</summary>
        public string CreatedDate { get { return _media["File_Created_Date"]; } }
        /// <summary>The time that the file was created on the file system (Warning: this field depends of local configuration, do not use it in an international database)</summary>
        public string CreatedDateLocal { get { return _media["File_Created_Date_Local"]; } }

        /// <summary>The time that the file was modified on the file system</summary>
        public string ModifiedDate { get { return _media["File_Modified_Date"]; } }
        /// <summary>The time that the file was modified on the file system (Warning: this field depends of local configuration, do not use it in an international database)</summary>
        public string FileModifiedDateLocal { get { return _media["File_Modified_Date_Local"]; } }

        /// <summary>File size in bytes</summary>
        public string FileSize { get { return _media["FileSize"]; } }
        public FileSizeInfo FileSizeInfo { get; private set; }

        /// <summary>Complete name (Folder+Name+Extension)</summary>
        public string CompleteName { get { return _media["CompleteName"]; } }
        /// <summary>Complete name (Folder+Name+Extension) of the last file (in the case of a sequence of files)</summary>
        public string CompleteNameLast { get { return _media["CompleteName_Last"]; } }

        /// <summary>Folder name only</summary>
        public string FolderName { get { return _media["FolderName"]; } }
        /// <summary>Folder name only of the last file (in the case of a sequence of files)</summary>
        public string FolderNameLast { get { return _media["FolderName_Last"]; } }

        /// <summary>File name only</summary>
        public string FileName { get { return _media["FileName"]; } }
        /// <summary>File name only of the last file (in the case of a sequence of files)</summary>
        public string FileNameLast { get { return _media["FileName_Last"]; } }

        /// <summary>File extension only</summary>
        public string FileExtension { get { return _media["FileExtension"]; } }
        /// <summary>File extension only of the last file (in the case of a sequence of files)</summary>
        public string FileExtensionLast { get { return _media["FileExtension_Last"]; } }
    }
}