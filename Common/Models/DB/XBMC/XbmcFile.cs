using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using Frost.Common.Models.DB.XBMC.StreamDetails;

namespace Frost.Common.Models.DB.XBMC {

    /// <summary>This table stores filenames and links to the Path table.</summary>
    [Table("files")]
    public class XbmcFile : IEquatable<XbmcFile> {

        /// <summary>Prefix for movies split into multiple files</summary>
        public const string STACK_PREFIX = "stack://";

        /// <summary>//separator between multiple filepaths in stacked filename (more than 1 file per movie)</summary>
        public const string STACK_FILE_SEPARATOR = " , ";

        #region Constructors

        /// <summary> Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        public XbmcFile() {
            Path = new XbmcPath();
            Movie = new XbmcMovie();
            Bookmark = new XbmcBookmark();
            StreamDetails = new HashSet<XbmcStreamDetails>();
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
        public XbmcFile(string dateAdded, string lastPlayed, long? playCount, string[] fileNames) : this(dateAdded, lastPlayed, playCount) {
            FileNames = fileNames;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcFile"/> class.</summary>
        /// <param name="dateAdded">The date and time the file was added.</param>
        /// <param name="lastPlayed">The date and time the file was last played.</param>
        /// <param name="playCount">The number of times the file has been played.</param>
        /// <param name="fileName">The filename in folder.</param>
        public XbmcFile(string dateAdded, string lastPlayed, long? playCount, string fileName) : this(dateAdded, lastPlayed, playCount) {
            FileNames = new[] {fileName};
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Column("strFilename")]
        public string FileNameString { get; set; }

        /// <summary>Gets or sets the array of filenames</summary>
        /// <value>The array of filename(s) in folder.</value>
        [NotMapped]
        public string[] FileNames {
            get {
                string fn = FileNameString;
                if (string.IsNullOrEmpty(FileNameString)) {
                    return null;
                }

                //check if there are multiple files
                if (fn.StartsWith(STACK_PREFIX)) {
                    fn = fn.Replace(STACK_PREFIX, ""); //remove the stack:// prefix

                    //split the string on SEPARATOR removing whitespace entries
                    string[] fileNames = fn.SplitWithoutEmptyEntries(STACK_FILE_SEPARATOR);
                    for (int i = 0; i < fileNames.Length; i++) {
                        //Convert file paths to Windows compatible ones
                        fileNames[i] = fileNames[i].ToWinPath();
                    }
                    return fileNames;
                }
                //else return just an array of the original filename
                return new[] {FileNameString};
            }
            set {
                StringBuilder sb = new StringBuilder();
                int numFiles = value.Length;

                //join all filePaths with SEPARATOR and prefix with STACK_PREFIX
                for (int i = 0; i < numFiles; i++) {
                    string fn = value[i];

                    //if the path is not empty or null join to stacked filename
                    if (!string.IsNullOrEmpty(fn)) {
                        //if the path is on the network and in Windows style
                        //convert to SAMBA
                        sb.Append(ToSmbPath(fn));

                        //don't append separator to the end of the string
                        if (i < numFiles - 1) {
                            sb.Append(STACK_FILE_SEPARATOR);
                        }
                    }
                }
                FileNameString = sb.ToString();
            }
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
        public string DateAdded { get; set; }

        #endregion

        #region Relation Tables

        /// <summary>Gets or sets the movie this file is from.</summary>
        /// <value>The movie this file is from.</value>
        [InverseProperty("File")]
        public virtual XbmcMovie Movie { get; set; }

        /// <summary>Gets or sets the info about folder path and folder settings of this file</summary>
        /// <value>The info about folder path and folder settings of this file</value>
        public virtual XbmcPath Path { get; set; }

        /// <summary>Gets or sets the bookmark for this file</summary>
        /// <value>The bookmark for this file</value>
        public virtual XbmcBookmark Bookmark { get; set; }

        /// <summary>Gets or sets the stream details in this file (Audio/Video/Subtitle)</summary>
        /// <value>The stream details in this file (Audio/Video/Subtitle)</value>
        public virtual HashSet<XbmcStreamDetails> StreamDetails { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcFile other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return PathId == other.PathId &&
                   FileNameString == other.FileNameString &&
                   PlayCount == other.PlayCount &&
                   LastPlayed == other.LastPlayed &&
                   DateAdded == other.DateAdded;
        }

        /// <summary>Converts a Windows network path to SAMBA path.</summary>
        /// <param name="fn">The filename to convert</param>
        /// <returns>A SAMBA path if the original filename starts with "\\" otherwise returns the same string</returns>
        public static string ToSmbPath(string fn) {
            if (fn.StartsWith(@"\\")) {
                //replace starting "\\" with "smb://"
                return "smb://" + fn.Remove(0, 2);
            }
            return fn;
        }

        internal class Configuration : EntityTypeConfiguration<XbmcFile> {

            public Configuration() {
                HasRequired(m => m.Path)
                    .WithMany(p => p.Files)
                    .HasForeignKey(f => f.PathId);
            }

        }

    }

}
