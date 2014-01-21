using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Models.XML.XBMC;
using Frost.DetectFeatures;
using System.Diagnostics;
using Frost.PHPtoNET;
using CompressionMode = Frost.Common.CompressionMode;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;
using Movie = Frost.Common.Models.DB.MovieVo.Movie;
using Subtitle = Frost.Common.Models.DB.MovieVo.Files.Subtitle;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            //EntityFrameworkProfiler.Initialize();

            FileStream debugLog = File.Create("debugDeserialize.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);
            //TestPHPSerialize();
            TestPHPDeserialize2();
            //SerXmlXbmcMovie(new PHPSerializer());
            //TestXjb();
            //time = TestMediaSearcher();
            //TestOpenSubtitlesProtocol();
            //TestDB();
            //TestMovie();

            sw.Stop();

            if (time == default(TimeSpan)) {
                time = sw.Elapsed;
            }

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + time);
            Console.WriteLine(Filler);
            Console.Read();
        }

        private static void TestPHPDeserialize2() {
            PHPDeserializer2 des2 = new PHPDeserializer2();

            XbmcXmlMovie deserialize;
            using(PHPSerializedStream phpStream = new PHPSerializedStream(File.ReadAllBytes("serOut.txt"), Encoding.UTF8)){
                 deserialize = des2.Deserialize<XbmcXmlMovie>(phpStream);
            }
        }

        private static void SerXmlXbmcMovie(PHPSerializer phpSer) {
            XbmcXmlMovie mv = new XbmcXmlMovie {
                Actors = new List<XbmcXmlActor>(new[] {
                    new XbmcXmlActor("alal", "malal", "file://c:/cd.jph"),
                    new XbmcXmlActor("blal", "nalal", "file://c:/cd.jph"),
                    new XbmcXmlActor("clal", "oalal", "file://c:/cd.jph"),
                    new XbmcXmlActor("dlal", "palal", "file://c:/cd.jph")
                }),
                Aired = DateTime.Now,
                CertificationsString = "US:PG-13",
                Countries = new List<string>(new[] { "US", "Mexico", "Canada" }),
                Credits = new List<string>(new[] { "Alfred H", "Malibu C" }),
                DateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Directors = new List<string>(new[] { "Alan C", "Norick B" }),
                FilenameAndPath = "file://c:/naodoa.jpg",
                Genres = new List<string>(new[] { "Comedy", "Adventure", "Sci-Fi" }),
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
                Studios = new List<string>(new[] { "Fox", "WB" }),
                Tagline = "The best dunky ever",
                Title = "Dunky",
                Top250 = 1,
                Trailer = "plugin://langubga.cm",
                Votes = 2000000.ToString(CultureInfo.InvariantCulture),
                Watched = true,
                Year = DateTime.Now.Year,
                FileInfo = new XbmcXmlFileInfo {
                    StreamDetails = new XbmcStreamDetails {
                        Audio = new List<XbmcXmlAudioInfo>(new[] { new XbmcXmlAudioInfo("ac3", 6, "en") }),
                        Subtitles = new List<XbmcXmlSubtitleInfo>(new[] { new XbmcXmlSubtitleInfo("en") }),
                        Video = new List<XbmcXmlVideoInfo>(new[] { new XbmcXmlVideoInfo("xvid", 5.5, 300, 400, 30000) })
                    }
                }
            };

            string serialize = phpSer.Serialize(mv);

            File.WriteAllText("serOut.txt", serialize);
        }

        #region FeatureDetector

        private static TimeSpan TestMediaSearcher() {
            Stopwatch sw = Stopwatch.StartNew();
            FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
            ms.Search();

            sw.Stop();
            return sw.Elapsed;
        }

        private static void OutputMovie(Movie movie) {
            Video video = movie.Videos.FirstOrDefault();
            if (video != null) {
                OutputFileInfo(video.File);
            }

            OutputMovieInfo(movie);

            OutputVideo(movie.Videos.FirstOrDefault());
            OutputAudio(movie.Audios.FirstOrDefault());
            OutputSubtitles(movie.Subtitles);
            Debug.WriteLine(Filler);
        }

        private static void OutputMovieInfo(Movie movie) {
            Debug.WriteLine("Movie:");
            Debug.Indent();

            Debug.WriteLineIf(movie.Title != null, "Title: " + movie.Title);
            Debug.WriteLineIf(movie.OriginalTitle != null, "Original Title: " + movie.OriginalTitle);
            Debug.WriteLineIf(movie.SortTitle != null, "Sort Title: " + movie.SortTitle);

            Debug.WriteLineIf(movie.ReleaseYear != null, "Release Year: " + movie.ReleaseYear);
            //Debug.WriteLineIf(movie.ReleaseDate != default(DateTime), "Release Date: " + movie.ReleaseDate);

            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.Edithion), "Edithion: " + movie.Edithion);
            Debug.WriteLineIf(movie.DvdRegion != DVDRegion.Unknown, "DVD Region: " + movie.DvdRegion);

            Debug.WriteLine("Play count: " + movie.PlayCount);
            //Debug.WriteLineIf(movie.LastPlayed != default(DateTime), "Last Played: " + movie.LastPlayed);
            //Debug.WriteLineIf(movie.LastPlayed != default(DateTime), "Premiered: " + movie.Premiered);
            //Debug.WriteLineIf(movie.Aired != default(DateTime), "Aired: " + movie.Aired);

            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.Trailer), "Trailer: " + movie.Trailer);

            Debug.WriteLineIf(movie.Top250.HasValue, "Top250: " + movie.Top250);
            Debug.WriteLineIf(movie.Runtime.HasValue, "Runtime: " + movie.Runtime);
            Debug.WriteLine("Watched: " + movie.Watched);

            Debug.WriteLineIf(movie.RatingAverage.HasValue, "AVG Rating: " + movie.RatingAverage);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.ImdbID), "ImdbID: " + movie.ImdbID);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.TmdbID), "TmdbID: " + movie.TmdbID);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.ReleaseGroup), "Release Group: " + movie.ReleaseGroup);
            Debug.WriteLineIf(movie.IsMultipart, "Is Multipart: " + movie.IsMultipart);
            Debug.WriteLineIf(!string.IsNullOrEmpty(movie.PartTypes), "Part Type: " + movie.PartTypes);
            Debug.WriteLineIf(movie.Specials.Count > 0, "Specials: " + string.Join(", ", movie.Specials));

            Debug.Unindent();
        }

        private static void OutputAudio(Audio a) {
            if (a == null) {
                return;
            }

            Debug.WriteLine("Audio:");
            Debug.Indent();

            Debug.WriteLineIf(a.Language != null, "Language: " + a.Language);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Source), "Source: " + a.Source);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Type), "Type: " + a.Source);

            Debug.WriteLineIf(a.NumberOfChannels.HasValue, "Number of channels: " + a.NumberOfChannels);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.ChannelSetup), "Channel Setup: " + a.ChannelSetup);
            Debug.WriteLineIf(!string.IsNullOrEmpty(a.ChannelPositions), "Channel Positions: " + a.ChannelPositions);

            Debug.WriteLineIf(!string.IsNullOrEmpty(a.Codec), "Codec: " + a.Codec);
            Debug.WriteLineIf(a.BitRate.HasValue, "BitRate: " + a.BitRate);
            Debug.WriteLineIf(a.BitRateMode != FrameOrBitRateMode.Unknown, "BitRate Mode: " + a.BitRateMode);
            Debug.WriteLineIf(a.SamplingRate.HasValue, "Sampling Rate: " + a.SamplingRate);
            Debug.WriteLineIf(a.CompressionMode != CompressionMode.Unknown, "Compression Mode: " + a.CompressionMode);
            Debug.WriteLineIf(a.Duration.HasValue,
                string.Format("Durration: {0} ({1:hh'h 'mm'm 'ss's 'fff'ms'})", a.Duration ?? 0,
                    TimeSpan.FromMilliseconds(a.Duration ?? 0)));

            Debug.Unindent();
        }

        private static void OutputSubtitles(IEnumerable<Subtitle> subtitles) {
            Debug.WriteLine("Subtitles:");
            Debug.Indent();

            foreach (Subtitle subtitle in subtitles) {
                Debug.WriteLine(subtitle);
            }

            Debug.Unindent();
        }

        private static void OutputVideo(Video video) {
            if (video == null) {
                return;
            }

            Debug.WriteLine("Video:");
            Debug.Indent();

            Debug.WriteLineIf(video.Language != null, "Language: " + video.Language);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Source), "Source: " + video.Source);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Type), "Type: " + video.Type);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Resolution), "Resoulution: " + video.Resolution);
            Debug.WriteLineIf(video.FPS.HasValue, "FPS: " + video.FPS);
            Debug.WriteLineIf(video.BitRate.HasValue, "BitRate: " + video.BitRate);
            Debug.WriteLineIf(video.BitRateMode != FrameOrBitRateMode.Unknown, "BitRateMode: " + video.BitRateMode);
            Debug.WriteLineIf(video.BitDepth.HasValue, "BitDepth: " + video.BitDepth);
            Debug.WriteLineIf(video.CompressionMode != CompressionMode.Unknown, "CompressionMode: " + video.CompressionMode);

            Debug.WriteLineIf(video.Duration.HasValue,
                string.Format("Durration: {0} ({1:hh'h 'mm'm 'ss's 'fff'ms'})",
                    video.Duration ?? 0,
                    TimeSpan.FromMilliseconds(video.Duration ?? 0)
                    )
                );

            Debug.WriteLine("ScanType: " + video.ScanType);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.ColorSpace), "ColorSpace: " + video.ColorSpace);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.ChromaSubsampling),
                "Chroma Subsampling: " + video.ChromaSubsampling);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.Codec), "Codec: " + video.Codec);
            Debug.WriteLineIf(video.Aspect.HasValue, "Aspect: " + video.Aspect);
            Debug.WriteLineIf(!string.IsNullOrEmpty(video.AspectCommercialName), "AspectName: " + video.AspectCommercialName);
            Debug.WriteLineIf(video.Width.HasValue, "Width: " + video.Width);
            Debug.WriteLineIf(video.Height.HasValue, "Height: " + video.Height);

            Debug.Unindent();
        }

        private static void OutputFileInfo(FileVo file) {
            if (file != null) {
                Debug.WriteLine("File: ");
                Debug.Indent();

                Debug.WriteLine("FileName: " + file.Name);
                Debug.WriteLine("Extension: " + file.Extension);
                Debug.WriteLine("FolderPath: " + file.FolderPath);
                Debug.WriteLineIf(file.Size != null, "FileSize: " + (file.Size ?? 0).FormatFileSizeAsString());

                Debug.Unindent();
            }
        }

        #endregion
    }

}