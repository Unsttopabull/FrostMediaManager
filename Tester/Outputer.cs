using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;

namespace Frost.Tester {

    public static class Outputer {
        private static readonly string Filler;

        static Outputer() {
             Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));            
        }

        public static void OutputMovie(Movie movie) {
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

        private static void OutputFileInfo(File file) {
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
    }

}