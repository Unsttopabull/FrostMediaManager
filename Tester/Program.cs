using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.XML.XBMC;
using Frost.SharpMediaInfo;

namespace Frost.Tester {
    class Program {
        static void Main() {
            //XbmcParser xp = new XbmcParser();
            TestMediaInfo();
            //TestMediaInfo2();

            //TestXml();
        }

        private static void ParseCertString() {
            const string CERT_STRING = @"USA:R / UK:15 / Switzerland:16 / Sweden:11 / Spain:18 / South Korea:18 / Singapore:M18 / Portugal:M / Philippines:R-18 / Peru:14 / Norway:15 / New Zealand:R16 / Netherlands:16 / Mexico:B15 / Malaysia:(Banned) / Japan:PG-12 / Ireland:18 / Iceland:16 / Hong Kong:III / Germany:12 / France:-12 / Finland:K-15 / Chile:14 / Canada:13+ / Brazil:16 / Australia:MA / Argentina:16";
            Certification[] certificationsString = Certification.ParseCertificationsString(CERT_STRING);
        }

        public static void TestXml() {
            XmlSerializer xs = new XmlSerializer(typeof(XbmcXmlMovie));
            XbmcXmlMovie deserialize = (XbmcXmlMovie)xs.Deserialize(new XmlTextReader(@"C:\Users\Martin\Desktop\VIDEO_TS.nfo"));//@"E:\Torrenti\FILMI\Anna.Karenina (2012)\VIDEO_TS\VIDEO_TS.nfo"));

            //Movie movieFromXml = XjbXmlMovie.LoadAsMovie(@"C:\Users\Martin\Desktop\XJB\xml\50.50.2011.DVDScr.XviD-playXD_xjb.xml");

            XbmcXmlMovie mv = new XbmcXmlMovie {
                Actors = new List<XbmcXmlActor>(new[] {
                    new XbmcXmlActor ("alal", "malal", "file://c:/cd.jph"),
                    new XbmcXmlActor ("blal", "nalal", "file://c:/cd.jph"),
                    new XbmcXmlActor ("clal", "oalal", "file://c:/cd.jph"),
                    new XbmcXmlActor ("dlal", "palal", "file://c:/cd.jph")
                }),
                Aired = DateTime.Now,
                CertificationsString = "US:PG-13",
                Countries = new List<string>(new[] {"US", "Mexico", "Canada"}), 
                Credits = new List<string>(new[] {"Alfred H", "Malibu C"}),
                DateAdded = DateTime.Now.ToString("yyyy-dd.MM HH:ii:ss"),
                Directors = new List<string>(new[] {"Alan C", "Norick B"}),
                FilenameAndPath = "file://c:/naodoa.jpg", 
                Genres = new List<string>(new[] {"Comedy", "Adventure", "Sci-Fi"}),
                LastPlayed = DateTime.Now, 
                MPAA = "Rated R", 
                OriginalTitle = "Dunky",
                Outline = "dsfljasdlf dsfa", 
                PlayCount = 11111111, 
                Plot = "fsjlakjglajslgjasdčlgjalsdjgdas", 
                Premiered = DateTime.Now,
                Rating = 9.9f, 
                ReleaseDate = DateTime.Now,
                RuntimeString = "105 min",
                Set = "The Dunkys", 
                SortTitle = "Dunky 1", 
                Studios = new List<string>(new[] {"Fox", "WB"}),
                Tagline = "The best dunky ever", 
                Title = "Dunky", 
                Top250 = 1,
                Trailer = "plugin://langubga.cm",
                Votes = 2000000.ToString(CultureInfo.InvariantCulture),
                Watched = true,
                Year = DateTime.Now.Year,
                FileInfo = new XbmcXmlFileInfo {
                    StreamDetails = new XbmcStreamDetails {
                        Audio = new List<XbmcXmlAudioInfo>(new[] {new XbmcXmlAudioInfo("ac3", 6, "en")}),
                        Subtitles = new List<XbmcXmlSubtitleInfo>(new[] {new XbmcXmlSubtitleInfo("en")}),
                        Video = new List<XbmcXmlVideoInfo>(new[] {new XbmcXmlVideoInfo("xvid", 5.5, 300, 400, 30000)})
                    }
                }
            };

            XmlSerializer xs2 = new XmlSerializer(typeof(XbmcXmlMovie));
            xs.Serialize(new XmlIndentedTextWriter("test.xml"), mv);            

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

        static void TestMediaInfo2() {
            StringBuilder sb = new StringBuilder(100000);
            using (MediaFile mf = new MediaFile(@"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.avi", true)) {
                foreach (KeyValuePair<string, string> kvp in mf.Audio) {
                    sb.AppendLine(kvp.Key + " : " + kvp.Value);
                }
            }
            Console.Write(sb.ToString());
        }

        public static string Format(IEnumerable<byte> data) {
            //storage for the resulting string
            string result = string.Empty;
            //iterate through the byte[]
            foreach (byte value in data) {
                //storage for the individual byte
                string binarybyte = Convert.ToString(value, 2);
                //if the binarybyte is not 8 characters long, its not a proper result
                while (binarybyte.Length < 8) {
                    //prepend the value with a 0
                    binarybyte = "0" + binarybyte;
                }
                //append the binarybyte to the result
                result += binarybyte;
            }
            //return the result
            return result;
        }

        static void TestMediaInfo() {
            const string FILE_PATH = @"E:\Torrenti\FILMI\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi\Despicable.Me.2.2013.CROSubs.BRRip.XviD.AC3-SANTi.avi";
            const string FILE_PATH2 = @"E:\Torrenti\FILMI\Intersections 2013 CROSubs.DVDRip XViD juggs\Intersections 2013 CROSubs.DVDRip XViD juggs.avi";
            const string FILE_PATH3 = @"Z:\Filmi\(500) Days of Summer (2009) - 500 dni z Summer\(500)Days of Summer.[2009].RETAIL.DVDRIP.XVID.[Eng]-DUQA.avi";
            const string FILE_PATH4 = @"E:\Torrenti\FILMI\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi\Oz.the.Great.and.Powerful.2013.SLOSubs.DVDRip.XviD-DrSi.avi";;
            const string FILE_PATH5 = @"E:\Torrenti\FILMI\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG\The Wolverine 2013 SLOSubs.EXTENDED BRRip XviD-ETRG.avi"; ;


            using (MediaFile mf = new MediaFile(FILE_PATH5, true)) {
                mf.Options.InformPreset = InformPreset.HTML;
                mf.Options.ShowAllInfo = true;

                TimeSpan? timeSpan = mf.Audio.Interleave.Duration;
                string timeSpans = mf.Audio.Interleave.VideoFrames;

                foreach (KeyValuePair<string, string> kvp in mf.General) {
                    Console.WriteLine(@"{0} : {1}", kvp.Key, kvp.Value);
                }

                //DllTest(mf);
            }
            Console.WriteLine();
            Console.WriteLine("----------Končal----------");
            //Console.Read();
        }

        private static void DllTest(MediaFile mf) {
            StringBuilder sb = new StringBuilder(148000);
            sb.AppendLine(mf.Options.Custom("Info_Version", "0.7.13;MediaInfoDLL_Example_MSVC;0.7.13"));
            sb.AppendLine();

            sb.AppendLine("Info_Parameters");
            sb.AppendLine(mf.Info.KnownParameters);
            sb.AppendLine();

            sb.AppendLine("Info Codecs");
            sb.AppendLine(mf.Info.KnownCodecs);
            sb.AppendLine();

            sb.AppendLine("Open");

            sb.AppendLine();

            if (mf.IsOpen) {
                sb.AppendLine("Inform with Complete=false");
                mf.Options.ShowAllInfo = false;
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Inform with Complete=true");
                mf.Options.ShowAllInfo = true;
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Custom Inform");
                mf.Options.Inform = "General;Example : FileSize=%FileSize%";
                sb.AppendLine(mf.Inform());
                sb.AppendLine();

                sb.AppendLine("Get with Stream=General and Parameter=\"FileSize\"");
                sb.AppendLine(mf.General["FileSize"]);

                sb.AppendLine("GetI with Stream=General and Parameter=46");
                sb.AppendLine(mf.General[46]);

                sb.AppendLine("Count_Get with StreamKind=Stream_Audio");
                sb.AppendLine(mf.Audio.StreamCount.ToString(CultureInfo.InvariantCulture));

                sb.AppendLine("Get with Stream=General and Parameter=\"AudioCount\"");
                sb.AppendLine(mf.General["AudioCount"]);

                sb.AppendLine("Get with Stream=Audio and Parameter=\"StreamCount\"");
                sb.AppendLine(mf.Audio["StreamCount"]);

                sb.AppendLine("Moj Get Codec");
                sb.AppendLine(mf.Audio.Codec.Name);

                sb.AppendLine("Moj Fomat");
                sb.AppendLine(mf.General.Format.Name);

                sb.AppendLine("Are there menues?");
                sb.AppendLine(mf.Menu.Any.ToString());

                sb.AppendLine("Close");

                Console.Write(sb.ToString());
            }
            else {
                sb.AppendLine("Napaka med odprianjem");
                Console.Write(sb.ToString());
            }
        }

    }
}
