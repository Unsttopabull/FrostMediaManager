using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.Jukebox {

    [Table("movies")]
    public class XjbMovie {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("revision")]
        public int? Revision { get; set; }

        [Column("tmdb_id")]
        public short? TmdbID { get; set; }

        [Column("imdb_id")]
        public string ImdbID { get; set; }

        [Column("filename")]
        public string Filename { get; set; }

        [Column("filepathfull")]
        public string FilePathFull { get; set; }

        [Column("filepathdrive")]
        public string FilePathDrive { get; set; }

        [Column("drive_id")]
        public string DriveID { get; set; }

        [Column("filesize")]
        public int? Filesize { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("episode")]
        public string Episode { get; set; }

        [Column("name_sub")]
        public string NameSub { get; set; }

        [Column("title_sort")]
        public string TitleSort { get; set; }

        [Column("art")]
        public string Art { get; set; }

        [Column("orgname")]
        public string Orgname { get; set; }

        [Column("year")]
        public short? Year { get; set; }

        [Column("rating")]
        public int? Rating { get; set; }

        [Column("runtime")]
        public short? Runtime { get; set; }

        [Column("plot")]
        public string Plot { get; set; }

        [Column("has_cover")]
        public int? HasCover { get; set; }

        [Column("has_fanart")]
        public int? HasFanart { get; set; }

        [Column("first_file_exist")]
        public int? FirstFileExist { get; set; }

        [Column("last_file_exist")]
        public int? LastFileExist { get; set; }

        [Column("movie_vo")]
        public string MovieVo { get; set; }
    }
}
