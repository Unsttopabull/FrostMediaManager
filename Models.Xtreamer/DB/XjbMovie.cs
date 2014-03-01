using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Models.Xtreamer.DB {

    /// <summary>Represents a movie in the Xtreamer Movie Jukebox library.</summary>
    [Table("movies")]
    public class XjbMovie {
        public XjbMovie() {
            Genres = new HashSet<XjbGenre>();
            Cast = new HashSet<XjbMoviePerson>();
        }

        #region Properties/Columns

        /// <summary>Gets or sets the Id of this option in the database.</summary>
        /// <value>The Id of this option in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the revision.</summary>
        /// <value>The revision.</value>
        [Column("revision")]
        public int? Revision { get; set; }

        /// <summary>Gets or sets The Movie Database identifier.</summary>
        /// <value>The TMDB identifier.</value>
        [Column("tmdb_id")]
        public short? TmdbID { get; set; }

        /// <summary>Gets or sets the Internet Movie Database identifier.</summary>
        /// <value>The IMDB identifier.</value>
        /// <example>\eg{<c>"tt0068646"</c>}</example>
        [Column("imdb_id")]
        public string ImdbID { get; set; }

        /// <summary>Gets or sets the filename of the file this movie is contained in.</summary>
        /// <value>The filename of the file this movie is contained in.</value>
        [Column("filename")]
        public string Filename { get; set; }

        /// <summary>Gets or sets the full path to the movie.</summary>
        /// <value>The full path to the movie.</value>
        [Column("filepathfull")]
        public string FilePathFull { get; set; }

        /// <summary>Gets or sets the movie's folder path on the drive without trailing <c>"/"</c>.</summary>
        /// <value>The movie's folder path on the drive without trailing <c>"/"</c>.</value>
        /// <example>eg{<c>"/Filmi/300"</c>}</example>
        [Column("filepathdrive")]
        public string FilePathDrive { get; set; }

        /// <summary>Gets or sets the filesize of the movie.</summary>
        /// <value>The filesize of the movie</value>
        [Column("filesize")]
        public long? Filesize { get; set; }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        [Column("name")]
        public string Title { get; set; }

        /// <summary>Gets or sets the episode.</summary>
        /// <value>The episode</value>
        /// <example>\eg{<c>"S01E01"</c> or <c>"E01"</c>}</example>
        [Column("episode")]
        public string Episode { get; set; }

        /// <summary>Gets or sets the title of the movie in another language.</summary>
        /// <value>The title of the movie in another language.</value>
        [Column("name_sub")]
        public string NameSub { get; set; }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{ ''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        [Column("title_sort")]
        public string SortTitle { get; set; }

        /// <summary>Gets or sets the art.</summary>
        /// <value>The art.</value>
        [Column("art")]
        public string Art { get; set; }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        [Column("orgname")]
        public string OriginalTitle { get; set; }

        /// <summary>Gets or sets the year this movie was released in.</summary>
        /// <value>The year this movie was released in.</value>
        [Column("year")]
        public long? Year { get; set; }

        /// <summary>Gets or sets the movie rating in 0 to 100.</summary>
        /// <value>The movie rating in 0 to 100</value>
        [Column("rating")]
        public int? Rating { get; set; }

        /// <summary>Gets or sets the duration of the movie in seconds.</summary>
        /// <value>The duration of the movie in seconds.</value>
        [Column("runtime")]
        public long? Runtime { get; set; }

        /// <summary>Gets or sets the movie story and plot.</summary>
        /// <value>The movie story and plot.</value>
        [Column("plot")]
        public string Plot { get; set; }

        /// <summary>Gets or sets the if this movie has a cover image available.</summary>
        /// <value>Is <c>true</c> if this movie has a cover image available; otherwise <c>false</c> or <c>null</c>.</value>
        [Column("has_cover")]
        public bool? HasCover { get; set; }

        /// <summary>Gets or sets the if this movie has a fanart images available.</summary>
        /// <value>Is <c>true</c> if this movie has a fanart images available; otherwise <c>false</c> or <c>null</c>.</value>
        [Column("has_fanart")]
        public bool? HasFanart { get; set; }

        /// <summary>Gets or sets the first file exist.</summary>
        /// <value>The first file exist.</value>
        [Column("first_file_exist")]
        public long? FirstFileExist { get; set; }

        /// <summary>Gets or sets the last file exist.</summary>
        /// <value>The last file exist.</value>
        [Column("last_file_exist")]
        public long? LastFileExist { get; set; }

        /// <summary>Gets or sets the movie information as a serialized PHP object.</summary>
        /// <value>The movie information as a serialized PHP object.</value>
        [Column("movie_vo")]
        public string MovieVo { get; set; }

        #endregion

        /// <summary>Gets or sets the foreign key to the drive this movie is contained on.</summary>
        /// <value>The foreign key to the drive this movie is contained on</value>
        [Column("drive_id")]
        public string DriveId { get; set; }

        #region Associations/Related tables

        /// <summary>Gets or sets the drive this movie is contained on.</summary>
        /// <value>The drive this movie is contained on.</value>
        [ForeignKey("DriveId")]
        public virtual XjbDrive Drive { get; set; }

        /// <summary>Gets or sets the genres of this movie.</summary>
        /// <value>The genres of this movie.</value>
        public virtual HashSet<XjbGenre> Genres { get; set; }

        /// <summary>Gets or sets the link to the people that worked on this movie.</summary>
        /// <value>The link to the people that worked on this movie</value>
        public virtual HashSet<XjbMoviePerson> Cast { get; set; }
        #endregion

    }

}
