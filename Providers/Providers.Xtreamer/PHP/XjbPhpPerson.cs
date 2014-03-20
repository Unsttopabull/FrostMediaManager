using System.Collections;
using System.Xml.Serialization;
using Frost.PHPtoNET.Attributes;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Person")]
    public class XjbPhpPerson {

        public const string JOB_DIRECTOR = "director";
        public const string JOB_PRODUCER = "producer";
        public const string JOB_EXECUTIVE_PRODUCER = "executive producer";
        public const string JOB_SCREENPLAY = "screenplay";
        public const string JOB_ACTOR = "actor";
        public const string JOB_AUTHOR = "author";
        public const string JOB_ORIGINAL_MUSIC_COMPOSER = "original music composer";
        public const string JOB_DIRECTOR_OF_PHOTOGRAPHY = "director of photography";
        public const string JOB_EDITOR = "editor";
        public const string JOB_CASTING = "casting";
        public const string JOB_OTHER = "other";

        public XjbPhpPerson() {
        }

        public XjbPhpPerson(string name, string character, string job = "actor") : this(name, job) {
            Character = character;
        }

        public XjbPhpPerson(string name, string job) {
            Name = name;
            Job = job;
        }

        ///<summary>The id for this row in DB</summary>
        [PHPName("id")]
        public string Id { get; set; }

        ///<summary>Name of the Character</summary>
        ///<example>eg{''<c>Sarah Connor</c>''}</example>
        [PHPName("character")]
        public string Character;

        ///<summary>Type of job</summary>
        ///<example>
        ///eg{''<c>Actor, Art Direction, Author, Best Boy Electric, Boom Operator, Camera Operator,
        ///Casting, Costume Design, Director, Director of Photography, Editor, Executive Producer,
        ///Makeup Artist, Music, Novel, Original Music Composer, Producer, Production Design,
        ///Produzent, Screenplay, Set Decoration, Set Designer, Sound Designer, Sound Editor, Visual Effects, Writer, ...</c>''}
        ///</example>
        [PHPName("job")]
        public string Job;

        ///<summary>string	Name of the Person in format "Firstname Surname"</summary>
        ///<example>eg{''<c>Teddy Chan</c>''}</example>
        [PHPName("name")]
        public string Name;

        ///<summary>The person id at a online sources</summary>
        ///<example>\eg{ <code>array ( "imdb" => "nm0269463", "tmbd => "70703")</code>}</example>
        ///<remarks>http://www.themoviedb.org/person/70703</remarks>
        [XmlIgnore]
        [PHPName("personOnlineIdArr")]
        public Hashtable PersonOnlineIds;

    }

}
