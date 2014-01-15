using System;
using System.IO;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.DetectFeatures.Util.AspectRatio;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using Frost.SharpOpenSubtitles.Util;

using CompressionMode = Frost.Common.CompressionMode;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;
using ScanType = Frost.SharpMediaInfo.ScanType;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {

        private void GetISOVideoInfo() {
        }

        private void GetVideoInfo(FileVo file) {
            if (file.Extension == "iso") {
                GetISOVideoInfo();
                return;
            }

            MediaListFile mediaFile = _mf.GetOrOpen(file.FullPath);
            FileNameInfo fnInfo = _fnInfos[file.NameWithExtension];
            if (mediaFile != null) {
                string movieHash;
                try {
                    movieHash = MovieHasher.ComputeMovieHashAsHexString(file.FullPath);
                }
                catch (FileNotFoundException e) {
                    movieHash = null;
                }

                foreach (MediaVideo mediaVideo in mediaFile.Video) {
                    Video video = GetFileVideoStreamInfo(fnInfo, mediaVideo);
                    video.File = file;
                    video.MovieHash = movieHash;

                    Movie.Videos.Add(video);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }
        }

        private Video GetFileVideoStreamInfo(FileNameInfo fnInfo, MediaVideo mv) {
            Video v = new Video();

            AddFileNameInfo(fnInfo, v);

            v.Aspect = mv.PixelAspectRatio;
            v.BitDepth = mv.BitDepth;

            //convert from bps to Kbps if value exists otherwise return null
            v.BitRate = mv.BitRate.HasValue ? mv.BitRate / 1024.0f : null;
            v.BitRateMode = (FrameOrBitRateMode) mv.BitRateInfo.Mode;
            v.Format = mv.FormatInfo.Name;
            v.Codec = mv.CodecIDInfo.Hint ?? mv.CodecInfo.NameString ?? v.Codec;
            v.ColorSpace = mv.ColorSpace;
            v.ChromaSubsampling = mv.ChromaSubsampling;
            v.CompressionMode = (CompressionMode) mv.CompressionMode;
            v.Duration = mv.Duration.HasValue ? (long?) mv.Duration.Value.TotalMilliseconds : null;
            v.FPS = mv.FrameRate;
            v.Resolution = !string.IsNullOrEmpty(mv.Standard) ? mv.Standard : GetFileVideoResolution(mv) ?? v.Resolution;
            v.Height = (int?) mv.Height;
            v.Width = (int?) mv.Width;
            v.Language = CheckLanguage(GetLanguage(false, mv.LanguageInfo.Full1, null, fnInfo.SubtitleLanguage, fnInfo.Language));

            v.ScanType = (Common.ScanType) mv.ScanType;
            v.Aspect = mv.DisplayAspectRatio;

            if (v.Aspect.HasValue) {
                AspectRatioInfo knownAspectRatio = AspectRatioDetector.GetKnownAspectRatio((float) v.Aspect);
                if (knownAspectRatio != null) {
                    v.AspectCommercialName = knownAspectRatio.ComercialName;
                }
            }

            return v;
        }

        private void AddFileNameInfo(FileNameInfo fnInfo, Video video) {
            if (fnInfo.VideoSource != null) {
                video.Source = fnInfo.VideoSource;
            }
            else {
                video.Source = fnInfo.DVDRegion != DVDRegion.Unknown
                    ? fnInfo.DVDRegion.ToString()
                    : null;
            }

            video.Codec = fnInfo.VideoCodec;
            video.Resolution = fnInfo.VideoQuality;

            if (fnInfo.Language != null) {
                video.Language = CheckLanguage(new Language(fnInfo.Language));
            }
        }

        private string GetFileVideoResolution(MediaVideo mv) {
            long h = mv.Height ?? 0;
            long w = mv.Width ?? 0;

            switch (mv.ScanType) {
                case ScanType.Progressive:
                    if (h == 1080 && w == 1920) {
                        return "1080p";
                    }
                    if (h == 720 && w == 1280) {
                        return "720p";
                    }
                    break;
                case ScanType.Interlaced:
                case ScanType.MBAFF:
                case ScanType.Mixed:
                    if (h == 1080 && w == 1920) {
                        return "1080i";
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }
    }

}