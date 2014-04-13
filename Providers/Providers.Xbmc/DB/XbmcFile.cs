using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;
using System.Linq;
using System.Text;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Xbmc.DB.StreamDetails;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>This table stores filenames and links to the Path table.</summary>
    [Table("files")]
    public class XbmcFile : IFile {
        private DateTime _dateAdded;
        private string _dateAddedString;

        /// <summary>Prefix for movies split into multiple files</summary>
        public const string STACK_PREFIX = "stack://";

        /// <summary>//separator between multiple filepaths in stacked filename (more than 1 file per movie)</summary>
        public const string STACK_FILE_SEPARATOR = " , ";

        #region Constructors

        /// <summary> Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        public XbmcFile() {
            StreamDetails = new HashSet<XbmcDbStreamDetails>();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        /// <param name="dateAdded">The date and time the file was added.</param>
        /// <param name="lastPlayed">The date and time the file was last played.</param>
        /// <param name="playCount">The number of times the file has been played.</param>
        public XbmcFile(string dateAdded, string lastPlayed, long? playCount) : this() {
            DateAdded = dateAdded;
            LastPlayed = lastPlayed;
            PlayCount = playCount;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        /// <param name="dateAdded">The date and time the file was added.</param>
        /// <param name="lastPlayed">The date and time the file was last played.</param>
        /// <param name="playCount">The number of times the file has been played.</param>
        /// <param name="fileNames">The array of filename(s) in folder.</param>
        public XbmcFile(string dateAdded, string lastPlayed, long? playCount, IEnumerable<string> fileNames) : this(dateAdded, lastPlayed, playCount) {
            FileNames = fileNames as string[] ?? fileNames.ToArray();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        /// <param name="dateAdded">The date and time the file was added.</param>
        /// <param name="lastPlayed">The date and time the file was last played.</param>
        /// <param name="playCount">The number of times the file has been played.</param>
        /// <param name="fileName">The filename in folder.</param>
        public XbmcFile(string dateAdded, string lastPlayed, long? playCount, string fileName) : this(dateAdded, lastPlayed, playCount) {
            FileNames = new[] { fileName };
        }

        internal XbmcFile(IFile file) : this() {
            Id = file.Id;
            DateAdded = file.DateAdded.ToString("yyyy-mm-dd hh:ii:ss");

            Path = new XbmcPath { FolderPath = file.FolderPath };
            FileNames = new[] { file.NameWithExtension };
        }

        #endregion

        #region Properties/Columns

        /// <summary>Gets or sets the database file Id.</summary>
        /// <value>The database file Id</value>
        [Key]
        [Column("idFile")]
        public long Id { get; set; }

        /// <summary>Gets or sets the path foreign key.</summary>
        /// <value>The path foreign key.</value>
        [Column("idPath")]
        public long PathId { get; set; }

        /// <summary>Gets or sets the filename and path of the files that contain this movie.</summary>
        /// <value>The filename and path of the files that contain this movie.</value>
        /// <remarks>If Movie consists of more that 1 file the string starts with "stack://" folwed by a full path to the files in the order to be played, separated by " , "</remarks>
        /// <example>\egb{ 
        ///     <list type="bullet">
        /// 		<item><description>''<c>Wall_E.avi</c>''</description></item>
        /// 		<item><description>''<c>stack://smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E_cd1.avi , smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E_cd2.avi</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Wall_E.avi</c>''</description></item>
        /// 	</list>}
        /// </example>
        [Column("strFilename")]
        public string FileNameString { get; set; }

        /// <summary>Gets or sets the array of filenames</summary>
        /// <value>The array of filename(s) in folder.</value>
        [NotMapped]
        public string[] FileNames {
            get {
                return GetFileNames(FileNameString) as string[];
            }
            set {
                FileNameString = GetFileNamesString(value, false);
            }
        }

        internal static IEnumerable<string> GetFileNames(string fn) {
            if (string.IsNullOrEmpty(fn)) {
                return null;
            }

            //check if there are multiple files
            if (!fn.StartsWith(STACK_PREFIX)) {
                return new[] { fn };
            }

            fn = fn.Replace(STACK_PREFIX, ""); //remove the stack:// prefix

            //split the string on SEPARATOR removing whitespace entries
            string[] fileNames = fn.SplitWithoutEmptyEntries(STACK_FILE_SEPARATOR);
            for (int i = 0; i < fileNames.Length; i++) {
                //Convert file paths to Windows compatible ones
                fileNames[i] = fileNames[i].ToWinPath();
            }
            return fileNames;
        }

        internal static string GetFileNamesString(IEnumerable<string> value, bool fullpath = true) {
            if (value == null) {
                return null;
            }

            string[] enumerable = value as string[] ?? value.ToArray();
            if (enumerable.Length == 0) {
                return null;
            }

            if (enumerable.Length == 1) {
                return !fullpath 
                    ? enumerable.FirstOrDefault()
                    : System.IO.Path.GetFileName(enumerable.FirstOrDefault());
            }

            return STACK_PREFIX + string.Join(STACK_FILE_SEPARATOR, enumerable.Where(v => !string.IsNullOrEmpty(v)).Select(ToSmbPath));
        }

        /// <summary>Gets or sets the number of times the file has been played.</summary>
        /// <value>The number of times the file has been played.</value>
        [Column("playCount")]
        public long? PlayCount { get; set; }

        /// <summary>Gets or sets the date and time the file was last played.</summary>
        /// <value>The date and time the file was last played.</value>
        [Column("lastPlayed")]
        public string LastPlayed { get; set; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        [Column("dateAdded")]
        public string DateAdded {
            get { return _dateAddedString; }
            set {
                _dateAddedString = value;
                if (!string.IsNullOrEmpty(_dateAddedString)) {
                    DateTime.TryParseExact(DateAdded, "yyyy-mm-dd hh:ii:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dateAdded);
                }
            }
        }

        #endregion

        #region Relation Tables

        /// <summary>Gets or sets the movie this file is from.</summary>
        /// <value>The movie this file is from.</value>
        //[InverseProperty("File")]
        public virtual XbmcDbMovie Movie { get; set; }

        /// <summary>Gets or sets the info about folder path and folder settings of this file</summary>
        /// <value>The info about folder path and folder settings of this file</value>
        public virtual XbmcPath Path { get; set; }

        ///// <summary>Gets or sets the bookmark for this file</summary>
        ///// <value>The bookmark for this file</value>
        //public virtual XbmcBookmark Bookmark { get; set; }

        /// <summary>Gets or sets the stream details in this file (Audio/Video/Subtitle)</summary>
        /// <value>The stream details in this file (Audio/Video/Subtitle)</value>
        public virtual HashSet<XbmcDbStreamDetails> StreamDetails { get; set; }

        #endregion

        #region Utlity

        [NotMapped]
        public bool AnySubtitles {
            //get { return StreamDetails.OfType<XbmcSubtitleDetails>().Any(); }
            get { return StreamDetails.Any(sd => sd.Type == StreamType.Subtitle); }
            //get { return false; }
        }

        [NotMapped]
        public bool AnyVideos {
            //get { return StreamDetails.OfType<XbmcVideoDetails>().Any(); }
            get { return StreamDetails.Any(sd => sd.Type == StreamType.Video); }
        }

        [NotMapped]
        public bool AnyAudios {
            //get { return StreamDetails.OfType<XbmcAudioDetails>().Any(); }
            get { return StreamDetails.Any(sd => sd.Type == StreamType.Audio); }
        }

        #endregion

        #region IFile

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "AudioDetails":
                    case "VideoDetails":
                    case "SubtitleDetails":
                    case "DateAdded":
                    case "FolderPath":
                    case "FullPath":
                    case "NameWithExtension":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        DateTime IFile.DateAdded {
            get { return _dateAdded; }
        }

        /// <summary>Gets or sets the path to the folder that contains this file</summary>
        /// <value>The full path to the folder that contains this file with trailing '/' without quotes (" or ')</value>
        /// <example>\eg{
        /// 	<list type="bullet">
        ///         <item><description>''<c>C:/Movies/</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/</c>''</description></item>
        /// 	</list>}
        /// </example>
        string IFile.FolderPath {
            get { return Path.FolderPath; }
        }

        /// <summary>Gets the full path to the file.</summary>
        /// <value>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</value>
        string IFile.FullPath {
            get {
                string fileName = FileNames.FirstOrDefault();
                if (fileName != null) {
                    return System.IO.Path.Combine(Path.FolderPath, fileName);
                }
                return null;
            }
        }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E.avi</c>''}</example>
        string IFile.Name {
            get {
                return System.IO.Path.GetFileNameWithoutExtension(FileNames[0]);
            }
        }

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        string IFile.NameWithExtension {
            get { return FileNames[0]; }
        }

        /// <summary>Gets or sets the file size in bytes.</summary>
        /// <value>The file size in bytes.</value>
        long? IFile.Size {
            get { return default(long?); }
        }

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        string IFile.Extension {
            get { return default(string); }
        }

        #endregion

        /// <summary>Converts a Windows network path to SAMBA path.</summary>
        /// <param name="fn">The filename to convert</param>
        /// <returns>A SAMBA path if the original filename starts with "\\" otherwise returns the same string</returns>
        private static string ToSmbPath(string fn) {
            if (fn.StartsWith(@"\\")) {
                //replace starting "\\" with "smb://"
                return "smb://" + fn.Remove(0, 2);
            }
            return fn;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return FileNameString;
        }

        internal class Configuration : EntityTypeConfiguration<XbmcFile> {

            public Configuration() {
                HasMany(f => f.StreamDetails)
                    .WithRequired(sd => sd.File)
                    //.Map(fk => fk.MapKey("idFile"));
                    .HasForeignKey(sd => sd.FileId);

                HasRequired(m => m.Path)
                    .WithMany(p => p.Files)
                    .HasForeignKey(f => f.PathId);

                HasRequired(f => f.Movie)
                    .WithRequiredPrincipal(m => m.File)
                    .Map(fk => fk.MapKey("idFile"));
            }

        }

    }

}
