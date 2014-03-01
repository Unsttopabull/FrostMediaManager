using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Model.Xbmc.DB.Actor;

namespace Frost.Model.Xbmc.DB {

    /// <summary>This table sores information about a movie in the XBMC library.</summary>
    [Table("movie")]
    public class XbmcMovie {

        /// <summary>Separator between multiple genres, certifications, person names ...</summary>
        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="XbmcMovie"/> class.</summary>
        public XbmcMovie() {
            Actors = new HashSet<XbmcMovieActor>();
            Writers = new HashSet<XbmcPerson>();
            Directors = new HashSet<XbmcPerson>();
            Genres = new HashSet<XbmcGenre>();
            Countries = new HashSet<XbmcCountry>();
            Studios = new HashSet<XbmcStudio>();
        }

        #region Properties / Columns

        /// <summary>Gets or sets the database movie Id.</summary>
        /// <value>The database movie Id</value>
        [Key]
        [Column("idMovie")]
        public long Id { get; set; }

        /// <summary>Gets or sets the title of the movie in the local language.</summary>
        /// <value>The title of the movie in the local language.</value>
        /// <example>\eg{ ''<c>Downfall</c>''}</example>
        [Column("c00")]
        public string Title { get; set; }

        /// <summary>Gets or sets the movie plot.</summary>
        /// <value>The full movie plot.</value>
        [Column("c01")]
        public string Plot { get; set; }

        /// <summary>Gets or sets the story summary, an outline.</summary>
        /// <value>A short story summary, the plot outline</value>
        [Column("c02")]
        public string PlotOutline { get; set; }

        /// <summary>Gets or sets the tagline (short one-liner).</summary>
        /// <value>The tagline (short promotional slogan / one-liner / clarification).</value>
        [Column("c03")]
        public string Tagline { get; set; }

        /// <summary>Gets or sets the number of votes the average rating was computed from</summary>
        /// <value>The number of ratings</value>
        [Column("c04")]
        public string Votes { get; set; }

        /// <summary>Gets or sets the average movie rating (1 - 10 float)</summary>
        /// <value>Average movie rating (1 - 10 float)</value>
        /// <example>\eg{ <c>6.6</c>}</example>
        [Column("c05")]
        public string Rating { get; set; }

        /// <summary>Gets or sets the name(s) of the writer(s).</summary>
        /// <remarks>If more than 1 they are separated by " / "</remarks>
        /// <value>The names of script writer(s)</value>
        [Column("c06")]
        public string WriterNames { get; set; }

        /// <summary>Gets or sets the year this movie was released in string format.</summary>
        /// <value>The year this movie was released in string format.</value>
        [Column("c07")]
        public string ReleaseYear { get; set; }

        /// <summary>Gets or sets the serialized cover art or poster image URIs.</summary>
        /// <value>The serialized cover art or poster images.</value>
        /// <remarks>A one-line string without empty spaces containting multiple ''<c>thumb</c>'' tags with optional "preview" attribute</remarks>
        /// <example>\eg{<code><thumb preview="http://some.img.com/1/preview">http://some.img.com/1/</thumb></code>}</example>
        [Column("c08")]
        public string Thumbnails { get; set; }

        /// <summary>Gets or sets the Internet Movie Databse identifier of this movie.</summary>
        /// <value>The Internet Movie Databse identifier of this movie.</value>
        [Column("c09")]
        public string ImdbId { get; set; }

        /// <summary>Gets or sets the title used for sorting (eg. sequels)..</summary>
        /// <value>The title used for sorting</value>
        /// <example>\eg{''<c>Pirates of the Caribbean: The Curse of the Black Pearl</c>'' becomes ''<c>Pirates of the Caribbean 1</c>''}</example>
        [Column("c10")]
        public string SortTitle { get; set; }

        /// <summary>Gets or sets the runtime of the movie</summary>
        /// <value>The runtime of the movie</value>
        [Column("c11")]
        public string Runtime { get; set; }

        /// <summary>Gets or sets the US movie rating and reason for it</summary>
        /// <value>The US Movie rating</value>
        [Column("c12")]
        public string MpaaRating { get; set; }

        /// <summary>Gets or sets the movie ranking on IMDB Top 250 list.</summary>
        /// <value>The movie ranking on IMDB Top 250 list.</value>
        [Column("c13")]
        public string ImdbTop250 { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres separated by " / "</value>
        /// <remarks>Genre names are saved in a single string with genres divided by " / "</remarks>
        /// <example>\eg{ ''<c>Drama / Thriller</c>''}</example>
        [Column("c14")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GenreString { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        [NotMapped]
        public string[] GenreNames {
            get { return GenreString.SplitWithoutEmptyEntries(SEPARATOR); }
            set { GenreString = string.Join(SEPARATOR, value); }
        }

        /// <summary>Gets or sets names of the directors separated by " / ".</summary>
        /// <value>Full names of the directors separated by " / "</value>
        [Column("c15")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DirectorsString { get; set; }

        /// <summary>Gets or sets the array containing names of the directors.</summary>
        /// <value>Full names of the directors</value>
        [NotMapped]
        public string[] DirectorNames {
            get { return DirectorsString.SplitWithoutEmptyEntries(SEPARATOR); }
            set { DirectorsString = string.Join(SEPARATOR, value); }
        }

        /// <summary>Gets or sets the title in the original language.</summary>
        /// <value>The title in the original language.</value>
        /// <example>\eg{ ''<c>Der Untergang</c>''}</example>
        [Column("c16")]
        public string OriginalTitle { get; set; }

        /// <summary>Gets or sets the unknown value (unused).</summary>
        /// <value>The unknown value (unused).</value>
        [Column("c17")]
        public string Unknown { get; set; }

        /// <summary>Gets or sets the names of the studios that produced this movie separated by " / ".</summary>
        /// <value>The names of the studios that produced this movie separated by " / ".</value>
        /// <example>\eg{ ''<c>MGM / Fox</c>''}</example>
        [Column("c18")]
        public string StudioNames { get; set; }

        /// <summary>Gets or sets the movie trailer URL (can be YouTube plugin URI).</summary>
        /// <value>The movie trailer URL. Can be YouTube plugin URI</value>
        /// <remarks>If YouTube plugin URI it starts with ''<c>plugin://plugin.video.youtube/?action=play_video&amp;videoid=</c>''</remarks>
        /// <example>\eg{<code>plugin://plugin.video.youtube/?action=play_video&amp;videoid=fkxcftwKwfI</code>}</example>
        [Column("c19")]
        public string TrailerUrl { get; set; }

        /// <summary>Gets or sets the serialized fanart image URIs.</summary>
        /// <value>The serialized fanart images.</value>
        /// <remarks>A one-line string without empty spaces with parent tag <c>"fanart"</c> containting multiple <c>"thumb"</c> tags with optional <c>"preview"</c> attribute</remarks>
        /// <example>\eg{<code><fanart><thumb preview="http://some.img.com/preview">http://some.img.com</thumb></fanart></code>}</example>
        [Column("c20")]
        public string FanartUrls { get; set; }

        /// <summary>Gets or sets the names of the countries that this movie was shot or/and produced in separated by " / ".</summary>
        /// <value>The names of the countries that this movie was shot or/and produced in separated by " / ".</value>
        /// <example>\eg{ ''<c>United States of America / Mexico</c>''}</example>
        [Column("c21")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CountryString { get; set; }

        /// <summary>Gets or sets an array of country names that this movie was shot or/and produced in separated by " / ".</summary>
        /// <value>An array of country names that this movie was shot or/and produced in separated by " / ".</value>
        [NotMapped]
        public string[] CountryNames {
            get { return CountryString.SplitWithoutEmptyEntries(SEPARATOR); }
            set { CountryString = string.Join(SEPARATOR, value); }
        }

        /// <summary>Gets or sets the path to the folder containing the files of this movie.</summary>
        /// <value>The path to the folder containing the files of this movie.</value>
        /// <example>\egb{
        /// 	<list type="bullet">
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/Some Movie</c>''</description></item>
        /// 		<item><description>''<c>E:/Movies/Some Movie</c></description>''</item>
        /// 	</list>}
        /// </example>
        [Column("c22")]
        public string FolderPath { get; set; }

        #endregion

        /// <summary>Gets or sets the foreign key to the movie set.</summary>
        /// <value>The foreign key to the movie set.</value>
        [Column("idSet")]
        public long? SetId { get; set; }

        #region Relation Tables

        /// <summary>Gets or sets the information about file(s) that contain this movie.</summary>
        /// <value>The information about file(s) that contain this movie</value>
        [InverseProperty("Movie")]
        public virtual XbmcFile File { get; set; }

        /// <summary>Gets or sets the info about folder path and folder settings of this file</summary>
        /// <value>The info about folder path and folder settings of this file</value>
        public XbmcPath Path { get; set; }

        /// <summary>Gets or sets the set this movie is a part of.</summary>
        /// <value>The set this movie is a part of.</value>
        [ForeignKey("SetId")]
        public virtual XbmcSet Set { get; set; }

        /// <summary>Gets or sets the name of the credited writer(s).</summary>
        /// <value>The names of the credited script writer(s)</value>
        [InverseProperty("MoviesAsWriter")]
        public virtual HashSet<XbmcPerson> Writers { get; set; }

        /// <summary>Gets or sets the movie directors.</summary>
        /// <value>People that directed this movie.</value>
        [InverseProperty("MoviesAsDirector")]
        public virtual HashSet<XbmcPerson> Directors { get; set; }

        /// <summary>Gets or sets the actors that starred in the movie.</summary>
        /// <value>The actors that preformed in this movie.</value>
        public virtual HashSet<XbmcMovieActor> Actors { get; set; }

        /// <summary>Gets or sets the movie genres.</summary>
        /// <value>The movie genres.</value>
        public virtual HashSet<XbmcGenre> Genres { get; set; }

        /// <summary>Gets or sets the countries that this movie was shot or/and produced in.</summary>
        /// <summary>The countries that this movie was shot or/and produced in.</summary>
        public virtual HashSet<XbmcCountry> Countries { get; set; }

        /// <summary>Gets or sets the studios that produced this movie.</summary>
        /// <value>The studios that produced this movie.</value>
        public virtual HashSet<XbmcStudio> Studios { get; set; }

        #endregion

        internal class Configuration : EntityTypeConfiguration<XbmcMovie> {

            public Configuration() {
                // Movie <--> File
                HasRequired(m => m.File)
                    .WithRequiredDependent(f => f.Movie)
                    .Map(m => m.MapKey("idFile"));
            }

        }

    }

}
