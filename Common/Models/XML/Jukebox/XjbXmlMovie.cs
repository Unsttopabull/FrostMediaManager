using System;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Models.DB.MovieVo;
using Common.Models.DB.MovieVo.People;

namespace Common.Models.XML.Jukebox {

    /// <remarks/>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("movie", Namespace = "", IsNullable = false)]
    public class XjbXmlMovie {
        private const string SEPARATOR = " / ";

        public XjbXmlMovie() {
            Votes = "";
            MPAA = "";
            Credits = "";
            Fileinfo = "";
            GenreString = "";
        }

        /// <remarks/>
        [XmlElement("title", Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        /// <remarks/>
        [XmlElement("originaltitle", Form = XmlSchemaForm.Unqualified)]
        public string OriginalTitle { get; set; }

        /// <remarks/>
        [XmlElement("sorttitle", Form = XmlSchemaForm.Unqualified)]
        public string SortTitle { get; set; }

        /// <remarks/>
        [XmlElement("id", Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        /// <remarks/>
        [XmlElement("year", Form = XmlSchemaForm.Unqualified)]
        public int Year { get; set; }

        /// <remarks/>
        [XmlElement("releasedate", Form = XmlSchemaForm.Unqualified)]
        public string ReleaseDate { get; set; }

        /// <remarks/>
        [XmlElement("rating", Form = XmlSchemaForm.Unqualified)]
        public float Rating { get; set; }

        /// <remarks/>
        [XmlElement("votes", Form = XmlSchemaForm.Unqualified)]
        public string Votes { get; set; }

        /// <remarks/>
        [XmlElement("mpaa", Form = XmlSchemaForm.Unqualified)]
        public string MPAA { get; set; }

        /// <remarks/>
        /// <example>us:R / au:M</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("certification", Form = XmlSchemaForm.Unqualified)]
        public string CertificationsString { get; set; }

        [XmlIgnore]
        public Certification[] Certifications {
            get {
                if (string.IsNullOrEmpty(CertificationsString)) {
                    return null;
                }

                //certifications are saved in a string separated by ' / '
                string[] certs = CertificationsString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                Certification[] certsArr = new Certification[certs.Length];

                for (int i = 0; i < certs.Length; i++) {
                    //each cetification consists of key and a value separated by ':'
                    string[] kvp = certs[i].Split(':');

                    //if the record is malformed we discard it
                    if (kvp.Length < 2) {
                        continue;
                    }

                    certsArr[i] = new Certification(kvp[0], kvp[1]);
                }
                return certsArr;
            }
            set {
                Certification[] certifications = value;
                int certLen = certifications.Length;

                if (certLen != 0) {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < certLen; i++) {
                        sb.Append(certifications[i].Country);
                        sb.Append(":");
                        sb.Append(certifications[i].Rating);

                        if (i < certLen - 1) {
                            sb.Append(SEPARATOR);
                        }
                    }
                    CertificationsString = sb.ToString();
                }
                CertificationsString = null;
            }
        }

        /// <summary>genres are saved in a single string with genres divided by " / "</summary>
        /// <example>Drama / Thriller</example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("genre", Form = XmlSchemaForm.Unqualified)]
        public string GenreString { get; set; }

        [XmlIgnore]
        public string[] Genres {
            get {
                return !string.IsNullOrEmpty(GenreString)
                               ? GenreString.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries)
                               : null;
            }
            set {
                GenreString = string.Join(SEPARATOR, value);
            }
        }

        /// <remarks/>
        [XmlElement("studio", Form = XmlSchemaForm.Unqualified)]
        public string Studio { get; set; }

        /// <remarks/>
        [XmlElement("director", Form = XmlSchemaForm.Unqualified)]
        public string Director { get; set; }

        /// <remarks/>
        [XmlElement("credits", Form = XmlSchemaForm.Unqualified)]
        public string Credits { get; set; }

        /// <remarks/>
        [XmlElement("tagline", Form = XmlSchemaForm.Unqualified)]
        public string Tagline { get; set; }

        /// <remarks/>
        [XmlElement("outline", Form = XmlSchemaForm.Unqualified)]
        public string Outline { get; set; }

        /// <remarks/>
        [XmlElement("plot", Form = XmlSchemaForm.Unqualified)]
        public string Plot { get; set; }

        /// <remarks/>
        [XmlElement("runtime", Form = XmlSchemaForm.Unqualified)]
        public string RuntimeString { get; set; }

        [XmlIgnore]
        public long? Runtime {
            get {
                //runtime is saved as a string with seconds integer divided by 60 with appended text " min"
                string runtimeInMinutes = RuntimeString.Replace(" min", "");

                long runtimeVal;
                if (long.TryParse(runtimeInMinutes, out runtimeVal)) {
                    return runtimeVal > 0
                        ? (long?)runtimeVal
                        : null;
                }
                return null;
            }
            set {
                RuntimeString = (value.HasValue)
                                        ? value + " min"
                                        : null;
            }
        }

        /// <remarks/>
        [XmlElement("actor", Form = XmlSchemaForm.Unqualified)]
        public XjbXmlActor[] Actors { get; set; }

        /// <remarks/>
        [XmlElement("fileinfo", Form = XmlSchemaForm.Unqualified)]
        public string Fileinfo { get; set; }


        #region Conversion Functions
        public Movie ToMovie() {
            return (Movie)this;
        }
        #endregion

        #region Serialization
        public void Serialize(string xmlSaveLocation) {
            XmlSerializer xs = new XmlSerializer(typeof(XjbXmlMovie));
            xs.Serialize(new XmlIndentedTextWriter(xmlSaveLocation), this);            
        }

        public static XjbXmlMovie Load(string pathToXml) {
            XmlSerializer xs = new XmlSerializer(typeof(XjbXmlMovie));

            return (XjbXmlMovie)xs.Deserialize(new XmlTextReader(pathToXml));
        }

        public static Movie LoadAsMovie(string pathToXml) {
            return (Movie)Load(pathToXml);
        }
        #endregion

        //conversion operator by explicit casting
        public static explicit operator Movie(XjbXmlMovie xmlMovie) {
            long? runtimeInSec = null;
            if (xmlMovie.Runtime.HasValue) {
                //convert mins to seconds
                runtimeInSec = xmlMovie.Runtime.Value * 60;
            }

            Movie mv = new Movie {
                Title = xmlMovie.Title,
                OriginalTitle = xmlMovie.OriginalTitle,
                Year = xmlMovie.Year,
                RatingAverage = xmlMovie.Rating,
                Certifications = xmlMovie.Certifications,
                ImdbID = xmlMovie.ID,
                Runtime = runtimeInSec
            };

            if (!string.IsNullOrEmpty(xmlMovie.Studio)) {
                mv.Studios.Add(new Studio(xmlMovie.Studio));
            }

            mv.Plot.Add(new Plot(xmlMovie.Plot, xmlMovie.Outline, xmlMovie.Tagline, null));

            mv.Directors.Add(new Person(xmlMovie.Director));

            mv.AddGenres(xmlMovie.Genres);
            mv.AddActors(xmlMovie.Actors);

            return mv;
        }
    }
}