using System;
using System.IO;
using System.Linq;
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
using ScanType = Frost.Common.ScanType;
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
            v.CodecId = GetVideoCodecId(v.Codec, mv.CodecIDInfo.ID) ?? mv.CodecIDInfo.Hint;
            v.ColorSpace = mv.ColorSpace;
            v.ChromaSubsampling = mv.ChromaSubsampling;
            v.CompressionMode = (CompressionMode) mv.CompressionMode;
            v.Duration = mv.Duration.HasValue ? (long?) mv.Duration.Value.TotalMilliseconds : null;
            v.FPS = mv.FrameRate;

            v.Height = (int?) mv.Height;
            v.Width = (int?) mv.Width;
            v.Language = CheckLanguage(GetLanguage(false, mv.LanguageInfo.Full1, null, fnInfo.SubtitleLanguage, fnInfo.Language));

            v.ScanType = (ScanType) mv.ScanType;

            v.Aspect = mv.DisplayAspectRatio;

            GetResolution(mv, v);

            if (v.Aspect.HasValue) {
                AspectRatioInfo knownAspectRatio = AspectRatioDetector.GetKnownAspectRatio((float) v.Aspect);
                if (knownAspectRatio != null) {
                    v.AspectCommercialName = knownAspectRatio.ComercialName;
                }
            }

            return v;
        }

        private void GetResolution(MediaVideo mv, Video v) {
            int resolution = 0;
            if (!string.IsNullOrEmpty(mv.Standard)) {
                v.Standard = mv.Standard;

                if (v.Standard.Equals("NTSC", StringComparison.OrdinalIgnoreCase)) {
                    resolution = 480;

                    if (v.ScanType == ScanType.Unknown) {
                        v.ScanType = ScanType.Interlaced;
                    }
                }
                else if (v.Standard.Equals("PAL", StringComparison.OrdinalIgnoreCase)) {
                    resolution = 576;
                    if (v.ScanType == ScanType.Unknown) {
                        v.ScanType = ScanType.Interlaced;
                    }
                }
            }
            else {
                v.ScanType = GetFileVideoResolution(mv, out resolution);
            }
            v.Resolution = resolution == 0 ? (int?) null : resolution;
        }

        private string GetVideoCodecId(string codec, string id) {
            switch (codec) {
                case "MPEG-1 Video":
                    return "mpeg";
                case "MPEG-2 Video":
                    return "mpeg2";
                case "Xvid":
                    return codec;
            }

            if (string.IsNullOrEmpty(id) || id.All(char.IsNumber)) {
                return null;
            }

            switch (id) {
                case "V_MPEG4/ISO/AVC":
                    return "h264";
                default:
                    return id;
            }
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

            if (!string.IsNullOrEmpty(fnInfo.VideoQuality)) {
                int resolution;
                if (fnInfo.VideoQuality.Equals("NTSC", StringComparison.OrdinalIgnoreCase)) {
                    resolution = 480;
                    video.ScanType = ScanType.Interlaced;
                }
                else if (fnInfo.VideoQuality.Equals("PAL", StringComparison.OrdinalIgnoreCase)) {
                    resolution = 576;
                    video.ScanType = ScanType.Interlaced;
                }
                else {
                    string videoResolution = fnInfo.VideoQuality.TrimEnd('p', 'i');
                    if (int.TryParse(videoResolution, out resolution)) {
                        if (fnInfo.VideoQuality.EndsWith("p")) {
                            video.ScanType = ScanType.Progressive;
                        }
                        else if (fnInfo.VideoQuality.EndsWith("i")) {
                            video.ScanType = ScanType.Interlaced;
                        }
                    }
                }

                video.Resolution = resolution != 0 ? (int?) null : resolution;
            }

            if (fnInfo.Language != null) {
                video.Language = CheckLanguage(new Language(fnInfo.Language));
            }
        }

        private ScanType GetFileVideoResolution(MediaVideo mv, out int resolution) {
            long h = mv.Height ?? 0;
            long w = mv.Width ?? 0;

            resolution = 0;
            switch (mv.ScanType) {
                case MediaScanType.Progressive:
                    if (h == 1080 && w == 1920) {
                        resolution = 1080;
                    }
                    if (h == 720 && w == 1280) {
                        resolution = 720;
                    }
                    return ScanType.Progressive;
                case MediaScanType.Interlaced:
                case MediaScanType.MBAFF:
                case MediaScanType.Mixed:
                    if (h == 1080 && w == 1920) {
                        resolution = 1080;
                        return ScanType.Interlaced;
                    }
                    break;
            }
            return ScanType.Unknown;
        }
    }

}