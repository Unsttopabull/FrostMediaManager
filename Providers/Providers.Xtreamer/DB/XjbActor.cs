using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xtreamer.DB {

    public class XjbActor : XjbMoviePerson {

        public XjbActor() {
        }

        public XjbActor(XjbPerson person, string character) : base(person){
            Character = character;
        }

        /// <summary>Gets or sets the character the person is portraying in this movie.</summary>
        /// <value>The character the person is portraying in this movie.</value>
        [Column("character")]
        public string Character { get; set; }
    }

}