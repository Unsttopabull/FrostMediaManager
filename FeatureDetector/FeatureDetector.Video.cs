using System.Collections.Generic;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;

namespace Frost.DetectFeatures {
    public partial class FeatureDetector {
        private readonly List<Video> _video;

        private Video GetFileVideoInfo(int filePos, int streamNumber = 0) {
            if (_video[streamNumber] != null) {
                return _video[streamNumber];
            }

            MediaVideo mv = _mf[filePos].Video;
            _video[streamNumber] = new Video();

            Video currVideo = _video[streamNumber];
            currVideo.Aspect = mv.PixelAspectRatio;
            currVideo.BitDepth = mv.BitDepth;

            //convert from bps to Kbps if value exists otherwise return null
            currVideo.BitRate = mv.BitRate.HasValue ? mv.BitRate / 1024.0f : null;
            currVideo.BitRateMode = mv.BitRateInfo.Mode;
            currVideo.Codec = mv.CodecIDInfo.Hint ?? mv.CodecInfo.Name;
            currVideo.ColorSpace = mv.ColorSpace;
            currVideo.CompressionMode = mv.CompressionMode;
            currVideo.Duration = mv.Duration.HasValue ? (long?) mv.Duration.Value.TotalMilliseconds : null;
            currVideo.FPS = mv.FrameRate;
            currVideo.Resolution = !string.IsNullOrEmpty(mv.Standard) ? mv.Standard : GetFileVideoResolution(filePos);
            currVideo.Height = (int?) mv.Height;
            currVideo.Width = (int?) mv.Width;
            currVideo.Language = new Language(mv.Language, mv.LanguageInfo.ISO639_Alpha2, mv.LanguageInfo.ISO639_Alpha3);
            currVideo.ScanType = mv.ScanType;
            currVideo.Type = GetFileVideoType(filePos, streamNumber);

            return currVideo;
        }

        private string GetFileVideoType(int filePos, int streamNumber) {
            string fileName = _mf[filePos].General.FileInfo.FileName;
            if (_mf[filePos].General.FormatInfo.Name.Contains("DVD") || fileName.Contains("HD2DVD", true)) {
                return "DVD";
            }

            if (fileName.Contains("BRRip", true)) {
                _video[streamNumber].Source = "Bluray";
                return "BRRip";
            }

            if (fileName.Contains("DVDRip", true)) {
                _video[streamNumber].Source = "DVD";
                return "DVDRip";
            }

            if (fileName.ContainsAny(true, "BLUERAY", "BR", "BD", "Blu-ray")) {
                return "Bluray";
            }

            if (fileName.Contains("X264")) {
                return "X264";
            }
            return null;
        }

        private string GetFileVideoResolution(int filePos) {
            long h = _mf[filePos].Video.Height ?? 0;
            long w = _mf[filePos].Video.Width ?? 0;

            switch (_mf[filePos].Video.ScanType) {
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
