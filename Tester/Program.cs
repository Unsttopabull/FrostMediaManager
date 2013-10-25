using ObdelajProdatke.XBMC;

namespace Tester {
    class Program {
        static void Main() {
            //XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlMovie));
            //XbmcXmlMovie deserialize = (XbmcXmlMovie) xs.Deserialize(new XmlTextReader(@"C:\Users\Martin\Desktop\VIDEO_TS.nfo"));//@"E:\Torrenti\FILMI\Anna.Karenina (2012)\VIDEO_TS\VIDEO_TS.nfo"));

            //Movie movieFromXml = XjbXmlMovie.LoadAsMovie(@"C:\Users\Martin\Desktop\XJB\xml\50.50.2011.DVDScr.XviD-playXD_xjb.xml");
            XbmcParser xp = new XbmcParser();
            //XbmcContainer xc = new XbmcContainer();

            //movie mv = new movie();
            //mv.actor = new[] {
            //        new actor {name="alal", role= "malal", thumb = "file://c:/cd.jph"},
            //        new actor {name="blal", role= "nalal", thumb = "file://c:/cd.jph"},
            //        new actor {name="clal", role= "oalal", thumb = "file://c:/cd.jph"},
            //        new actor {name="dlal", role= "palal", thumb = "file://c:/cd.jph"},
            //};
            //mv.aired = DateTime.Now;
            //mv.certification = "US:PG-13";
            //mv.country = new string[] { "US", "Mexico", "Canada" };
            //mv.credits = new string[] { "Alfred H", "Malibu C" };
            //mv.dateadded = DateTime.Now.ToString("yyyy-dd.MM HH:ii:ss");
            //mv.director = new string[] { "Alan C", "Norick B" };
            //mv.filenameandpath = "file://c:/naodoa.jpg";
            //mv.genre = new string[] { "Comedy", "Adventure", "Sci-Fi" };
            //mv.lastplayed = DateTime.Now;
            //mv.mpaa = "Rated R";
            //mv.originaltitle = "Dunky";
            //mv.outline = "dsfljasdlf dsfa";
            //mv.playcount = 11111111;
            //mv.plot = "fsjlakjglajslgjasdčlgjalsdjgdas";
            //mv.premiered = DateTime.Now;
            //mv.rating = 9.9f;
            //mv.releasedate = DateTime.Now.ToString("dd.MM.yyyy");
            //mv.runtime = "105 min";
            //mv.set = "The Dunkys";
            //mv.sorttitle = "Dunky 1";
            //mv.studio = new string[] { "Fox", "WB" };
            //mv.tagline = "The best dunky ever";
            //mv.title = "Dunky";
            //mv.top250 = 1;
            //mv.trailer = "plugin://langubga.cm";
            //mv.votes = 2000000.ToString(CultureInfo.InvariantCulture);
            //mv.watched = true;
            //mv.year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            //mv.fileinfo = new movieFileinfo {
            //    streamdetails = new streamdetails {
            //        Audio = new[] {
            //            new audioInfo {
            //                channels = 6,
            //                codec = "ac3",
            //                language = "en"
            //            }
            //        },
            //        Subtitle = new[] {
            //            new subtitleInfo {
            //                language = "en"
            //            }
            //        },
            //        Video = new[] {
            //            new videoInfo {
            //                aspect = 5.5f,
            //                codec = "xvid",
            //                durationinseconds = 300000,
            //                height = 300,
            //                width = 400
            //            }
            //        }
            //    }
            //};

            //XmlSerializer xs = new XmlSerializer(typeof(movie));
            //xs.Serialize(new XmlIndentedTextWriter("test.xml"), mv);
        }
    }
}
