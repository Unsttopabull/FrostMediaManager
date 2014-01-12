using System;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.DetectFeatures.Util.AspectRatio;
using Frost.SharpMediaInfo.Output;
using Frost.SharpOpenSubtitles.Util;
using CompressionMode = Frost.Common.CompressionMode;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;
using ScanType = Frost.SharpMediaInfo.ScanType;

namespace Frost.DetectFeatures {

    public partial class FileFeatures {

        private void GetISOVideoInfo() {
        }

        private void GetVideoInfo() {
            if (_file.Extension == "iso") {
                GetISOVideoInfo();
                return;
            }

            if (_mediaFile != null) {
                string movieHash = MovieHasher.ComputeMovieHashAsHexString(_filePath);
                foreach (MediaVideo mediaVideo in _mediaFile.Video) {
                    Video video = GetFileVideoStreamInfo(mediaVideo);
                    video.MovieHash = movieHash;

                    Movie.Videos.Add(video);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }
        }

        private Video GetFileVideoStreamInfo(MediaVideo mv) {
            Video v = new Video();
            v.File = _file;

            AddFileNameInfo(v);

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
            v.Language = CheckLanguage(GetLanguage(false, mv.LanguageInfo.Full1, null, _fnInfo.SubtitleLanguage, _fnInfo.Language));

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

        private void AddFileNameInfo(Video video) {
            if (_fnInfo.VideoSource != null) {
                video.Source = _fnInfo.VideoSource;
            }
            else {
                video.Source = _fnInfo.DVDRegion != DVDRegion.Unknown
                    ? _fnInfo.DVDRegion.ToString()
                    : null;
            }

            video.Codec = _fnInfo.VideoCodec;
            video.Resolution = _fnInfo.VideoQuality;

            if (_fnInfo.Language != null) {
                video.Language = CheckLanguage(new Language(_fnInfo.Language));
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