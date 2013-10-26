using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.DB.MovieVo;
using Common.Models.DB.XBMC.Actor;
using Common.Models.XML.XBMC;

namespace Common.Models.DB.XBMC {

    [Table("movie")]
    public class XbmcMovie {
        private const string SEPARATOR = " / ";

        public XbmcMovie() {
            File = new XbmcFile();
            Set = new XbmcSet();
            Path = new XbmcPath();
            Actors = new HashSet<XbmcMovieActor>();
            Writers = new HashSet<XbmcPerson>();
            Directors = new HashSet<XbmcPerson>();
            Genres = new HashSet<XbmcGenre>();
            Countries = new HashSet<XbmcCountry>();
            Studios = new HashSet<XbmcStudio>();
        }

        [Key]
        [Column("idMovie")]
        public long Id { get; set; }

        [Required, Column("idFile")]
        public long FileId { get; set; }

        [Required]
        [ForeignKey("FileId")]
        public virtual XbmcFile File { get; set; }

        [Column("c00")]
        public string Title { get; set; }

        [Column("c01")]
        public string Plot { get; set; }

        [Column("c02")]
        public string PlotOutline { get; set; }

        [Column("c03")]
        public string Tagline { get; set; }

        [Column("c04")]
        public string Votes { get; set; }

        [Column("c05")]
        public string Rating { get; set; }

        [Column("c06")]
        public string WriterNames { get; set; }

        [Column("c07")]
        public string ReleaseYear { get; set; }

        [Column("c08")]
        public string Thumbnails { get; set; }

        [Column("c09")]
        public string ImdbId { get; set; }

        [Column("c10")]
        public string TitleSort { get; set; }

        [Column("c11")]
        public string Runtime { get; set; }

        [Column("c12")]
        public string MpaaRating { get; set; }

        [Column("c13")]
        public string ImdbTop250 { get; set; }

        [Column("c14")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GenreString { get; set; }

        [NotMapped]
        public string[] GenreNames {
            get { return GenreString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries); }
            set { GenreString = string.Join(SEPARATOR, value); }
        }

        [Column("c15")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DirectorsString { get; set; }

        [NotMapped]
        public string[] DirectorNames {
            get { return DirectorsString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries); }
            set { DirectorsString = string.Join(SEPARATOR, value); }
        }

        [Column("c16")]
        public string OriginalTitle { get; set; }

        [Column("c17")]
        public string Unknown { get; set; }

        [Column("c18")]
        public string StudioNames { get; set; }

        [Column("c19")]
        public string TrailerUrl { get; set; }

        [Column("c20")]
        public string FanartUrls { get; set; }

        [Column("c21")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CountryString { get; set; }

        [NotMapped]
        public string[] CountryNames {
            get { return CountryString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries); }
            set { CountryString = string.Join(SEPARATOR, value); }
        }

        [Column("c22")]
        public string FolderPath { get; set; }

        public XbmcPath Path { get; set; }

        [Required, Column("idSet")]
        public long? SetId { get; set; }

        [ForeignKey("SetId")]
        public virtual XbmcSet Set { get; set; }

        [InverseProperty("MoviesAsWriter")]
        public virtual ICollection<XbmcPerson> Writers { get; set; }

        [InverseProperty("MoviesAsDirector")]
        public virtual ICollection<XbmcPerson> Directors { get; set; }

        public virtual ICollection<XbmcMovieActor> Actors { get; set; }

        public virtual ICollection<XbmcGenre> Genres { get; set; }
        public virtual ICollection<XbmcCountry> Countries { get; set; }
        public virtual ICollection<XbmcStudio> Studios { get; set; }

        #region Conversion Functions

        public Movie ToMovie(XbmcMovie movie) {
            return (Movie)movie;
        }

        public XbmcXmlMovie ToXmlMovie(XbmcMovie movie) {
            return (XbmcXmlMovie)movie;
        }

        #endregion

        #region Conversion Operators

        public static explicit operator Movie(XbmcMovie movie) {
            throw new NotImplementedException();
        }

        public static explicit operator XbmcXmlMovie(XbmcMovie movie) {
            throw new NotImplementedException();
        }

        #endregion
    }
}