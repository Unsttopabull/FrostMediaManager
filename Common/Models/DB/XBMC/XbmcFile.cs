using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Common.Models.DB.XBMC.StreamDetails;

namespace Common.Models.DB.XBMC {

    [Table("files")]
    public class XbmcFile {
        private const string STACK_PREFIX = "stack://";
        private const string SEPARATOR = " , ";

        public XbmcFile() {
            Path = new XbmcPath();
            Movie = new XbmcMovie();
            Bookmark = new XbmcBookmark();
            StreamDetails = new HashSet<XbmcStreamDetails>();
        }

        public XbmcFile(string dateAdded, string lastPlayed, long? playCount) : this() {
            DateAdded = dateAdded;
            LastPlayed = lastPlayed;
            PlayCount = playCount;
        }

        [Key]
        [Column("idFile")]
        public long Id { get; set; }

        [Column("idPath")]
        public long PathId { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Column("strFilename")]
        public string FileNameString { get; set; }

        [NotMapped]
        public string[] FileNames {
            get {
                string fn = FileNameString;
                if (string.IsNullOrEmpty(FileNameString)) {
                    return null;
                }

                if (fn.StartsWith(STACK_PREFIX)) {
                    fn = fn.Replace(STACK_PREFIX, "");

                    string[] fileNames = fn.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < fileNames.Length; i++) {
                        fileNames[i] = fileNames[i].ToWinPath();
                    }
                    return fileNames;
                }
                return new[]{ FileNameString };
            }
            set {
                StringBuilder sb = new StringBuilder();
                int numFiles = value.Length;
                for (int i = 0; i < numFiles; i++) {
                    sb.Append(ToSmbPath(value[i]));

                    if (i < numFiles - 1) {
                        sb.Append(SEPARATOR);
                    }
                }
                FileNameString = sb.ToString();
            }
        }

        [Column("playCount")]
        public long? PlayCount { get; set; }

        [Column("lastPlayed")]
        public string LastPlayed { get; set; }

        [Column("dateAdded")]
        public string DateAdded { get; set; }


        #region Relation Tables

        [InverseProperty("File")]
        public virtual XbmcMovie Movie { get; set; }

        public virtual XbmcPath Path { get; set; }

        public virtual XbmcBookmark Bookmark { get; set; }

        public virtual ICollection<XbmcStreamDetails> StreamDetails { get; set; }

        #endregion

        public string ToSmbPath(string fn) {
            if (fn.StartsWith(@"\\")) {
                return "smb://" + fn.Remove(0, 2);
            }
            return fn;
        }
    }
}