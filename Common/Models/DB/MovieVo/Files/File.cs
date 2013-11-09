using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo.Files {

    /// <summary>Represents an information about a file.</summary>
    public class File : IEquatable<File> {

        /// <summary>Initializes a new instance of the <see cref="File"/> class.</summary>
        public File() {
            Movie = new Movie();

            AudioDetails = new HashSet<Audio>();
            VideoDetails = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
        }

        /// <summary>Initializes a new instance of the <see cref="File"/> class.</summary>
        /// <param name="name">The filename in folder</param>
        /// <param name="extension">The file extension withot begining point</param>
        /// <param name="size">The file size in bytes.</param>
        /// <param name="pathOnDrive">The full path to the folder that contains the file with trailing '/' without quotes (" or ')</param>
        public File(string name, string extension, string pathOnDrive, long? size = null) : this() {
            Extension = extension;
            Name = name;
            FolderPath = pathOnDrive;
            Size = size;
        }

        #region Properties/Columns

        /// <summary>Gets or sets the database file Id.</summary>
        /// <value>The database file Id</value>
        [Key]
        public long Id { get; set; }

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

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        public DateTime DateAdded { get; set; }

        #endregion

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        #region Relation Tables

        /// <summary>Gets or sets the movie this file is from.</summary>
        /// <value>The movie this file is from.</value>
        [Required]
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        /// <summary>Gets or sets the details about audio streams in this file</summary>
        /// <value>The details about audio streams in this file</value>
        public virtual HashSet<Audio> AudioDetails { get; set; }

        /// <summary>Gets or sets the details about video streams in this file</summary>
        /// <value>The details about video streams in this file</value>
        public virtual HashSet<Video> VideoDetails { get; set; }

        /// <summary>Gets or sets the details about subtitles in this file</summary>
        /// <value>The details about subtitles in this file</value>
        public virtual HashSet<Subtitle> Subtitles { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(File other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Extension == other.Extension &&
                   Name == other.Name &&
                   FolderPath == other.FolderPath &&
                   DateAdded == other.DateAdded &&
                   MovieId == other.MovieId;
        }

        /// <summary>Gets the full path to the file.</summary>
        /// <returns>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</returns>
        public string GetFullPath() {
            if (FolderPath != null && Name != null) {
                return FolderPath + Name;
            }
            return null;
        }

    }

}
