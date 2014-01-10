using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo.Files {

    /// <summary>Represents an information about a file.</summary>
    public class File : IEquatable<File> {

        /// <summary>Initializes a new instance of the <see cref="File"/> class.</summary>
        public File() {
            AudioDetails = new HashSet<Audio>();
            VideoDetails = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
            DateAdded = DateTime.Now;
        }

        /// <summary>Initializes a new instance of the <see cref="File"/> class.</summary>
        /// <param name="name">The filename in folder (without folder path)</param>
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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        [Required]
        public string Extension { get; set; }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E.avi</c>''}</example>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the path to the folder that contains this file</summary>
        /// <value>The full path to the folder that contains this file with trailing '/' without quotes (" or ')</value>
        /// <example>\eg{
        /// 	<list type="bullet">
        ///         <item><description>''<c>C:/Movies/</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/</c>''</description></item>
        /// 	</list>}
        /// </example>
        [Required]
        public string FolderPath { get; set; }

        /// <summary>Gets or sets the file size in bytes.</summary>
        /// <value>The file size in bytes.</value>
        public long? Size { get; set; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        public DateTime DateAdded { get; set; }

        #endregion

        ///// <summary>Gets or sets the movie foreign key.</summary>
        ///// <value>The movie foreign key.</value>
        //public long MovieId { get; set; }

        #region Relation Tables

        ///// <summary>Gets or sets the movie this file is from.</summary>
        ///// <value>The movie this file is from.</value>
        //public virtual Movie Movie { get; set; }

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
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Id == other.Id && string.Equals(Extension, other.Extension) && string.Equals(Name, other.Name) && string.Equals(FolderPath, other.FolderPath) && Size == other.Size && DateAdded.Equals(other.DateAdded);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((File) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            unchecked {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Extension != null ? Extension.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FolderPath != null ? FolderPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ DateAdded.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>Gets the full path to the file.</summary>
        /// <returns>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</returns>
        public string GetFullPath() {
            if (FolderPath != null && Name != null) {
                return FolderPath + Name;
            }
            return null;
        }

        internal class Configuration : EntityTypeConfiguration<File> {
            public Configuration() {
                ToTable("Files");
            }
        }
    }

}
