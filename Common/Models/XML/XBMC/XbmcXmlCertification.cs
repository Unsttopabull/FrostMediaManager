using Common.Models.DB.MovieVo;

namespace Common.Models.XML.XBMC {
    public class XbmcXmlCertification {

        public XbmcXmlCertification(string country, string rating) {
            Country = country;
            Rating = rating;
        }

        public string Country { get; set; }

        public string Rating { get; set; }

        public override string ToString() {
            return Country + ":" + Rating;
        }

        public static explicit operator Certification(XbmcXmlCertification cert) {
            return new Certification(cert.Country, cert.Rating);
        }
    }
}