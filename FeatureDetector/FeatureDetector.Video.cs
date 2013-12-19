using System;
using System.Collections.Generic;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.DetectFeatures.Util.AspectRatio;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using Frost.SharpMediaInfo.Output.Properties.General;

namespace Frost.DetectFeatures {
    public partial class FeatureDetector {
        private readonly List<Video> _video;

        private void GetFileInfo(string fileName) {
            if (fileName.EndsWith(".iso")) {
                return;
            }

            MediaListFile mediaFile = _mf.GetOrOpen(fileName);

            if (mediaFile != null) {
                FileInfo fileInfo = mediaFile.General.FileInfo;
                File f = new File(fileInfo.FileName, fileInfo.Extension, fileInfo.FolderPath, fileInfo.FileSize);

                FileNameInfo fnInfo = GetFileNameInfo(fileName);

                List<Video> fileVideoInfo = GetFileVideoInfo(mediaFile.Video);
                foreach (Video video in fileVideoInfo) {
                    video.File = f;
                }

                _video.AddRange(fileVideoInfo);
            }
            else {
                Console.Error.WriteLine("Could not open file: "+fileName);
            }
        }

        private List<Video> GetFileVideoInfo(IEnumerable<MediaVideo> videoInfo) {
            List<Video> videos = new List<Video>();
            foreach (MediaVideo mediaVideo in videoInfo) {
                Video video = GetFileVideoStreamInfo(mediaVideo);
                
                videos.Add(video);
            }
            return videos;
        }

        private Video GetFileVideoStreamInfo(MediaVideo mv) {

            Video currVideo = new Video();
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
            currVideo.Resolution = !string.IsNullOrEmpty(mv.Standard) ? mv.Standard : GetFileVideoResolution(mv);
            currVideo.Height = (int?) mv.Height;
            currVideo.Width = (int?) mv.Width;
            currVideo.Language = new Language(mv.Language, mv.LanguageInfo.ISO639_Alpha2, mv.LanguageInfo.ISO639_Alpha3);
            currVideo.ScanType = mv.ScanType;
            currVideo.Aspect = mv.DisplayAspectRatio;

            if (currVideo.Aspect.HasValue) {
                AspectRatioInfo knownAspectRatio = AspectRatioDetector.GetKnownAspectRatio((float) currVideo.Aspect);
                if (knownAspectRatio != null) {
                    currVideo.AspectCommercialName = knownAspectRatio.ComercialName;
                }
            }

            //currVideo.Type 

            return currVideo;
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
