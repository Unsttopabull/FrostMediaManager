using System;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.DetectFeatures.Util.AspectRatio;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using CompressionMode = Frost.Common.CompressionMode;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;

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
                foreach (MediaVideo mediaVideo in _mediaFile.Video) {
                    Video video = GetFileVideoStreamInfo(mediaVideo);
                    video.MovieHash = _movieHash;

                    Movie.Videos.Add(video);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }
        }

        private Video GetFileVideoStreamInfo(MediaVideo mv) {
            Video currVideo = new Video();
            currVideo.File = _file;

            AddFileNameInfo(currVideo);

            currVideo.Aspect = mv.PixelAspectRatio;
            currVideo.BitDepth = mv.BitDepth;

            //convert from bps to Kbps if value exists otherwise return null
            currVideo.BitRate = mv.BitRate.HasValue ? mv.BitRate / 1024.0f : null;
            currVideo.BitRateMode = (FrameOrBitRateMode) mv.BitRateInfo.Mode;
            currVideo.Format = mv.FormatInfo.Name;
            currVideo.Codec = mv.CodecIDInfo.Hint ?? mv.CodecInfo.NameString ?? currVideo.Codec;
            currVideo.ColorSpace = mv.ColorSpace;
            currVideo.ChromaSubsampling = mv.ChromaSubsampling;
            currVideo.CompressionMode = (CompressionMode) mv.CompressionMode;
            currVideo.Duration = mv.Duration.HasValue ? (long?) mv.Duration.Value.TotalMilliseconds : null;
            currVideo.FPS = mv.FrameRate;
            currVideo.Resolution = !string.IsNullOrEmpty(mv.Standard) ? mv.Standard : GetFileVideoResolution(mv) ?? currVideo.Resolution;
            currVideo.Height = (int?) mv.Height;
            currVideo.Width = (int?) mv.Width;
            currVideo.Language = GetLanguage(false, mv.Language, null, _fnInfo.SubtitleLanguage, _fnInfo.Language);

            currVideo.ScanType = (Common.ScanType) mv.ScanType;
            currVideo.Aspect = mv.DisplayAspectRatio;

            if (currVideo.Aspect.HasValue) {
                AspectRatioInfo knownAspectRatio = AspectRatioDetector.GetKnownAspectRatio((float) currVideo.Aspect);
                if (knownAspectRatio != null) {
                    currVideo.AspectCommercialName = knownAspectRatio.ComercialName;
                }
            }

            return currVideo;
        }

        private void AddFileNameInfo(Video video) {
            video.Source = _fnInfo.VideoSource ?? _fnInfo.DVDRegion.ToString();

            video.Codec = _fnInfo.VideoCodec;
            video.Resolution = _fnInfo.VideoQuality;

            if (_fnInfo.Language != null) {
                video.Language = new Language(_fnInfo.Language);
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