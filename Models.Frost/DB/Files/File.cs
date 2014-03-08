using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB.Files {

    /// <summary>Represents an information about a file.</summary>
    public class File : IFile<Audio, Video, Subtitle> {

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

        /// <summary>Initializes a new instance of the <see cref="File" /> class.</summary>
        /// <param name="info">The file information.</param>
        /// <exception cref="System.ArgumentNullException">Throw if <paramref name="info"/> is <c>null</c>.</exception>
        public File(FileInfo info) {
            if (info == null) {
                throw new ArgumentNullException("info");
            }

            Extension = info.Extension;
            Name = Path.GetFileNameWithoutExtension(info.Name);
            FolderPath = info.DirectoryName + Path.DirectorySeparatorChar;
            Size = info.Length;
        }

        public File(IFile value) {
            Contract.Requires<ArgumentNullException>(value.AudioDetails != null);
            Contract.Requires<ArgumentNullException>(value.VideoDetails != null);
            Contract.Requires<ArgumentNullException>(value.Subtitles != null);

            Extension = value.Extension;
            Name = value.Name;
            FolderPath = value.FolderPath;
            Size = value.Size;
            DateAdded = value.DateAdded;

            AudioDetails = new HashSet<Audio>(value.AudioDetails.Select(a => new Audio(a)));
            VideoDetails = new HashSet<Video>(value.VideoDetails.Select(v => new Video(v)));
            Subtitles = new HashSet<Subtitle>(value.Subtitles.Select(sub => new Subtitle(sub)));
        }

        #region Properties/Columns

        /// <summary>Gets or sets the database file Id.</summary>
        /// <value>The database file Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        #region Relation Tables

        /// <summary>Gets or sets the details about audio streams in this file</summary>
        /// <value>The details about audio streams in this file</value>
        public virtual ICollection<Audio> AudioDetails { get; set; }

        /// <summary>Gets or sets the details about video streams in this file</summary>
        /// <value>The details about video streams in this file</value>
        public virtual ICollection<Video> VideoDetails { get; set; }

        /// <summary>Gets or sets the details about subtitles in this file</summary>
        /// <value>The details about subtitles in this file</value>
        public virtual ICollection<Subtitle> Subtitles { get; set; }

        #endregion

        #region Utility Properties

        /// <summary>Gets the full path to the file.</summary>
        /// <value>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</value>
        public string FullPath {
            get {
                return Path.Combine(FolderPath, Name) + "." + Extension;
            }
        }

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        public string NameWithExtension {
            get { return Name + "." + Extension; }
        }

        #endregion

        #region IFile
        ICollection<IAudio> IFile.AudioDetails {
            get { return new HashSet<IAudio>(AudioDetails); }
        }

        ICollection<IVideo> IFile.VideoDetails {
            get { return new HashSet<IVideo>(VideoDetails); }
        }

        ICollection<ISubtitle> IFile.Subtitles {
            get { return new HashSet<ISubtitle>(Subtitles); }
        }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name + "." + Extension;
        }

        internal class Configuration : EntityTypeConfiguration<File> {
            public Configuration() {
                ToTable("Files");
            }
        }
    }

}