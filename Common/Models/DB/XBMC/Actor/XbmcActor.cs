using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {

    [NotMapped]
    public class XbmcActor {
        private readonly long _movieID;

        public XbmcActor() {
        }

        internal XbmcActor(XbmcPerson person, string role, long order, long movieId) {
            if (person == null) {
                throw new ArgumentException(@"person must not be null", "person");
            }

            Id = person.Id;
            Name = person.Name;
            ThumbXml = person.ThumbXml;
            Role = role;
            Order = order;
            _movieID = movieId;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string ThumbXml { get; set; }

        public string ThumbURL {
            get {
                return ThumbXml != null
                               ? ThumbXml.Replace("<thumb>", "").Replace("</thumb>", "")
                               : null;
            }
            set { ThumbXml = "<thumb>" + value + "</thumb>"; }
        }

        public string Role { get; set; }

        public long Order { get; set; }

        public XbmcMovieActor ToMovieActor() {
            return (XbmcMovieActor)this;
        }

        public static explicit operator XbmcMovieActor(XbmcActor actor) {
            return new XbmcMovieActor(actor, actor._movieID);
        }

        public static explicit operator XbmcPerson(XbmcActor actor) {
            return new XbmcPerson(actor.Id, actor.Name, actor.ThumbXml);
        }
    }
}