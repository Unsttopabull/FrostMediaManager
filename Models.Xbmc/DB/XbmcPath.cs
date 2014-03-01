using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Model.Xbmc.DB {

    /// <summary>Table holds information about folder path and folder settings and the type of content inside.</summary>
    [Table("path")]
    public class XbmcPath : IEquatable<XbmcPath> {

        /// <summary>Initializes a new instance of the <see cref="XbmcPath"/> class.</summary>
        public XbmcPath() {
            Movies = new HashSet<XbmcMovie>();
            Files = new HashSet<XbmcFile>();
        }

        #region Properties / Columns

        /// <summary>Gets or sets the id of the path in the database.</summary>
        /// <value>The id of the path in the database</value>
        [Key]
        [Column("idPath")]
        public long Id { get; set; }

        /// <summary>Gets or sets the URI to the folder</summary>
        /// <value>The URI to the folder</value>
        /// <example>\egb{
        /// 	<list type="bullet">
        /// 		<item><description>"E:\Movies\"</description></item>
        /// 		<item><description>"smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/"</description></item>
        ///         <item><description>"smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Inception/VIDEO_TS/"</description></item>
        /// 	</list>}
        /// </example>
        [Column("strPath")]
        public string PathName { get; set; }

        /// <summary>Gets or sets the type of the content stored on the path.</summary>
        /// <value>The type of the content stored on the path.</value>
        /// <example>\eg{''<c>movies</c>'', ''<c>tvshows</c>'' ...}</example>
        [Column("strContent")]
        public string Content { get; set; }

        /// <summary>Gets or sets xml file of a scraper used for this path.</summary>
        /// <value>The xml file of scraper used for this path.</value>
        /// <example>\eg{''<c>metadata.tvdb.com</c>'', or ''<c>metadata.themoviedb.org</c>'', ...}</example>
        [Column("strScraper")]
        public string Scraper { get; set; }

        /// <summary>Gets or sets the hash.</summary>
        /// <value>The hash.</value>
        [Column("strHash")]
        public string Hash { get; set; }

        /// <summary>Gets or sets the flag if the path is to be scaned recursively when searching for content.</summary>
        /// <value>Is <c>true</c> when the path is to be scaned recursively when searching for content; otherwise <c>false</c>.</value>
        [Column("scanRecursive")]
        public bool? ScanRecursive { get; set; }

        /// <summary>Gets or sets the flag wheter to use folder names instead of file names when looking up content information.</summary>
        /// <value>Is <c>true</c> when XBMC should use folder names instead of file names when looking up content information; otherwise <c>false</c>.</value>
        [Column("useFolderNames")]
        public bool? UseFolderNames { get; set; }

        /// <summary>Gets or sets the custom settings to be used by the selected scraper.</summary>
        /// <value>The custom settings to be used by the selected scraper</value>
        [Column("strSettings")]
        public string Settings { get; set; }

        /// <summary>Gets or sets the whether the movie should no be automaticaly updated.</summary>
        /// <value>Is <c>true</c> when the movie should no be automaticaly updated; otherwise <c>false</c>.</value>
        [Column("noUpdate")]
        public bool? NoUpdate { get; set; }

        /// <summary>Gets or sets the whether to exclude the path from scans and updates, even if it's a subfolder of a folder that has contents set..</summary>
        /// <value>Is <c>true</c> if XBMC should exclude the path from scans and updates, even if it's a subfolder of a folder that has contents set; otherwise <c>false</c>.</value>
        [Column("exclude")]
        public bool? Exclude { get; set; }

        /// <summary>Gets or sets the date the path was added.</summary>
        /// <value>The date the path was added.</value>
        [Column("dateAdded")]
        public string DateAdded { get; set; }

        #endregion

        #region Associations / Related tables

        /// <summary>Gets or sets the files on this path.</summary>
        /// <value>The files on this path.</value>
        public virtual HashSet<XbmcFile> Files { get; set; }

        /// <summary>Gets or sets the movies on this path.</summary>
        /// <value>The movies on this path.</value>
        public virtual HashSet<XbmcMovie> Movies { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcPath other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return PathName == other.PathName &&
                   Content == other.Content &&
                   Scraper == other.Scraper &&
                   Hash == other.Hash &&
                   ScanRecursive == other.ScanRecursive &&
                   UseFolderNames == other.UseFolderNames &&
                   Settings == other.Settings &&
                   NoUpdate == other.NoUpdate &&
                   Exclude == other.Exclude &&
                   DateAdded == other.DateAdded;
        }

        internal class Configuration : EntityTypeConfiguration<XbmcPath> {

            /// <summary>
            /// Initializes a new instance of the <see cref="Configuration"/> class.
            /// </summary>
            public Configuration() {
                //foreign key on "movie" is TEXT but id on "path" is INTEGER
                //EF detects mismatching types on entities and errors out
                //so we map it here and remove it from entity
                HasMany(p => p.Movies)
                    .WithRequired(m => m.Path)
                    .Map(m => m.MapKey("c23"));
            }

        }

    }

}
