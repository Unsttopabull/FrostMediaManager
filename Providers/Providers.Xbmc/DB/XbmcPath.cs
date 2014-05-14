using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>Table holds information about folder path and folder settings and the type of content inside.</summary>
    [Table("path")]
    public class XbmcPath {
        private static readonly Regex VideoExtensions =
            new Regex(
                @"(\.m4v|\.3g2|\.3gp|\.nsv|\.tp|\.ts|\.ty|\.strm|\.pls|\.rm|\.rmvb|\.m3u|\.m3u8|\.ifo|\.mov|\.qt|\.divx|\.xvid|\.bivx|\.vob|\.nrg|\.img|\.iso|\.pva|\.wmv|\.asf|\.asx|\.ogm|\.m2v|\.avi|\.bin|\.dat|\.mpg|\.mpeg|\.mp4|\.mkv|\.avc|\.vp3|\.svq3|\.nuv|\.viv|\.dv|\.fli|\.flv|\.rar|\.001|\.wpl|\.zip|\.vdr|\.dvr-ms|\.xsp|\.mts|\.m2t|\.m2ts|\.evo|\.ogv|\.sdp|\.avs|\.rec|\.url|\.pxml|\.vc1|\.h264|\.rcv|\.rss|\.mpls|\.webm|\.bdmv|\.wtv|\.pvr)$",
                RegexOptions.IgnoreCase);

        /// <summary>Initializes a new instance of the <see cref="XbmcPath"/> class.</summary>
        public XbmcPath() {
            Movies = new HashSet<XbmcDbMovie>();
            Files = new HashSet<XbmcFile>();
        }

        public XbmcPath(string path) : this() {
            FolderPath = path;

            GetHash();
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
        public string FolderPath { get; set; }

        /// <summary>Gets or sets the type of the content stored on the path.</summary>
        /// <value>The type of the content stored on the path.</value>
        /// <example>\eg{''<c>movies</c>'', ''<c>tvshows</c>'' ...}</example>
        [Column("strContent")]
        public string ContentType { get; set; }

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
        public virtual HashSet<XbmcDbMovie> Movies { get; set; }

        #endregion

        #region Hash

        private void GetHash() {
            DirectoryInfo di;
            try {
                di = new DirectoryInfo(FolderPath);
            }
            catch (Exception) {
                di = null;
            }

            if (di != null) {
                Hash = CanFastHash(di)
                           ? GetFastHash(di)
                           : GetPathHash(di);
            }
        }

        private static string GetPathHash(DirectoryInfo directory) {
            List<byte> bytesToHash = new List<byte>();

            DirectoryInfo[] directoryInfos = directory.GetDirectories();
            List<FileInfo> fileInfos = new List<FileInfo>();

            foreach (FileInfo info in directory.GetFiles()) {
                if (VideoExtensions.IsMatch(info.Name)) {
                    fileInfos.Add(info);
                }
            }

            List<FileSystemInfo> fsInfos = new List<FileSystemInfo>();
            fsInfos.AddRange(directoryInfos);
            fsInfos.AddRange(fileInfos);

            foreach (FileSystemInfo fsInfo in fsInfos) {
                string fullName = fsInfo.FullName;

                if (fsInfo is DirectoryInfo && !fullName.EndsWith("\\")) {
                    fullName += "\\";
                }

                bytesToHash.AddRange(Encoding.UTF8.GetBytes(fullName));

                FileInfo info = fsInfo as FileInfo;
                bytesToHash.AddRange(info != null ? BitConverter.GetBytes(info.Length) : BitConverter.GetBytes((long) 0));

                long fileTime = NativeMethods.GetLocalLastWriteFileTime(fullName);

                bytesToHash.AddRange(BitConverter.GetBytes(fileTime));
            }

            byte[] byteHash;
            using (MD5 md5 = MD5.Create()) {
                byteHash = md5.ComputeHash(bytesToHash.ToArray());
            }

            StringBuilder sb = new StringBuilder(byteHash.Length * 2);
            foreach (byte hashByte in byteHash) {
                sb.Append(hashByte.ToString("X2"));
            }
            return sb.ToString();
        }

        private bool CanFastHash(DirectoryInfo directory) {
            return directory.GetDirectories().Length == 0;
        }

        private static string GetFastHash(DirectoryInfo directory) {
            DateTime epoch = new DateTime(1970, 1, 1);

            long time;
            if (directory.LastWriteTime != default(DateTime)) {
                time = (long) directory.LastWriteTime.Subtract(epoch).TotalSeconds;
            }
            else {
                time = (long) directory.CreationTime.Subtract(epoch).TotalSeconds;
            }

            return string.Format(CultureInfo.InvariantCulture, "fast{0}", time);
        }

        #endregion


        internal class Configuration : EntityTypeConfiguration<XbmcPath> {
            /// <summary>Initializes a new instance of the <see cref="Configuration"/> class.</summary>
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