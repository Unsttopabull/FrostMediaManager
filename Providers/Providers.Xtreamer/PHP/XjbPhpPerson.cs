using System;
using System.Collections;
using System.Xml.Serialization;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;
using Frost.Common.Properties;
using Frost.PHPtoNET.Attributes;
using Frost.Providers.Xtreamer.DB;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Person")]
    public class XjbPhpPerson : IEquatable<XjbPhpPerson> {

        public const string JOB_DIRECTOR = "director";
        public const string JOB_PRODUCER = "producer";
        public const string JOB_EXECUTIVE_PRODUCER = "executive producer";
        public const string JOB_SCREENPLAY = "screenplay";
        public const string JOB_ACTOR = "actor";
        public const string JOB_WRITER = "writer";
        public const string JOB_AUTHOR = "author";
        public const string JOB_ORIGINAL_MUSIC_COMPOSER = "original music composer";
        public const string JOB_DIRECTOR_OF_PHOTOGRAPHY = "director of photography";
        public const string JOB_EDITOR = "editor";
        public const string JOB_CASTING = "casting";
        public const string JOB_OTHER = "other";

        public XjbPhpPerson() {
        }

        public XjbPhpPerson(string name, string job, string character = null) {
            Name = name;
            Job = job;
            Character = character;
        }

        public XjbPhpPerson(XjbDirector director) : this(director as XjbMoviePerson){
            Job = JOB_DIRECTOR;
        }

        public XjbPhpPerson(XjbWriter writer) : this(writer as XjbMoviePerson){
            Job = JOB_WRITER;
        }

        public XjbPhpPerson(XjbActor actor) : this(actor as XjbMoviePerson){
            Job = JOB_ACTOR;
            Character = actor.Character;
        }

        public XjbPhpPerson(XjbMoviePerson moviePerson) {
            if (moviePerson.PersonId != null) {
                Id = (int) moviePerson.PersonId;
            }

            Name = moviePerson.Person.Name;            
        }

        public XjbPhpPerson(IActor actor) {
            Name = actor.Name;
            Job = JOB_ACTOR;
            Character = actor.Character;
        }

        public XjbPhpPerson(IPerson person, string job) {
            Name = person.Name;
            Job = job;
        }

        ///<summary>The id for this row in DB</summary>
        [PHPName("id")]
        public int Id { get; set; }

        ///<summary>Name of the Character</summary>
        ///<example>eg{''<c>Sarah Connor</c>''}</example>
        [PHPName("character")]
        public string Character { get; set; }

        ///<summary>Type of job</summary>
        ///<example>
        ///eg{''<c>Actor, Art Direction, Author, Best Boy Electric, Boom Operator, Camera Operator,
        ///Casting, Costume Design, Director, Director of Photography, Editor, Executive Producer,
        ///Makeup Artist, Music, Novel, Original Music Composer, Producer, Production Design,
        ///Produzent, Screenplay, Set Decoration, Set Designer, Sound Designer, Sound Editor, Visual Effects, Writer, ...</c>''}
        ///</example>
        [PHPName("job")]
        public string Job { get; set; }

        ///<summary>string	Name of the Person in format "Firstname Surname"</summary>
        ///<example>eg{''<c>Teddy Chan</c>''}</example>
        [PHPName("name")]
        public string Name { get; set; }

        ///<summary>The person id at a online sources</summary>
        ///<example>\eg{ <code>array ( "imdb" => "nm0269463", "tmbd => "70703")</code>}</example>
        ///<remarks>http://www.themoviedb.org/person/70703</remarks>
        [XmlIgnore]
        [PHPName("personOnlineIdArr")]
        public Hashtable PersonOnlineIds;

        #region Equality Comparers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XjbPhpPerson other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0  && other.Id != 0) {
                return Id == other.Id;
            }

            return string.Equals(Character, other.Character) &&
                   string.Equals(Job, other.Job) &&
                   string.Equals(Name, other.Name);
        }

        #endregion
    }

}
