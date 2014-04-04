using System;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using CompressionMode = Frost.Common.CompressionMode;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        /// <summary>The audio codec identifier mappings</summary>
        /// <example>MPA2L3 => MP3, 116 => WMA</example>
        public static readonly CodecIdMappingCollection AudioCodecIdMappings;

        private void GetISOAudioInfo() {
        }

        private void GetAudioInfo(FileDetectionInfo file) {
            if (file.Extension == "iso") {
                GetISOAudioInfo();
                return;
            }

            MediaListFile mediaFile = _mf.GetOrOpen(file.FullPath);
            if (mediaFile != null) {
                foreach (MediaAudio mediaVideo in mediaFile.Audio) {
                    AudioDetectionInfo audio = GetFileAudioStreamInfo(_fnInfos[file.NameWithExtension], mediaVideo);

                    file.Audios.Add(audio);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }
        }

        private AudioDetectionInfo GetFileAudioStreamInfo(FileNameInfo fnInfo, MediaAudio ma) {
            AudioDetectionInfo a = new AudioDetectionInfo();

            AddFileNameInfo(fnInfo, a);

            a.BitDepth = ma.BitDepth;
            a.BitRate = ma.BitRate.HasValue ? ma.BitRate / 1024.0f : null;
            a.BitRateMode = (FrameOrBitRateMode) ma.BitRateInfo.Mode;
            a.ChannelPositions = ma.ChannelInfo.Positions;
            a.ChannelSetup = ma.ChannelInfo.PositionsString2;
            a.NumberOfChannels = (int?) ma.NumberOfChannels;
            a.Codec = ma.CodecInfo.NameString ?? ma.CodecIDInfo.Hint ?? a.Codec;
            a.CodecId = GetAudioCodecId(ma.CodecInfo.Name, ma.CodecIDInfo.ID);
            a.CompressionMode = (CompressionMode) ma.CompressionMode;
            a.Duration = ma.Duration.HasValue ? (long?) ma.Duration.Value.TotalMilliseconds : null;
            a.Language = GetLangauge(ma);

            a.SamplingRate = ma.SamplingRate.HasValue ? ma.SamplingRate / 1024 : null;

            return a;
        }

        private string GetAudioCodecId(string codec, string id) {
            if (string.IsNullOrEmpty(id) || id.All(char.IsNumber)) {
                return AudioCodecIdMappings.ContainsKey(codec)
                    ? AudioCodecIdMappings[codec]
                    : codec;
            }

            return AudioCodecIdMappings.ContainsKey(id)
                ? AudioCodecIdMappings[id]
                : id;
        }

        private void AddFileNameInfo(FileNameInfo fnInfo, AudioDetectionInfo audio) {
            if (fnInfo.AudioSource != null) {
                audio.Source = fnInfo.AudioSource;
            }

            audio.Codec = fnInfo.AudioCodec;
            audio.Type = fnInfo.AudioQuality;

            if (fnInfo.Language != null) {
                audio.Language = fnInfo.Language;
            }
        }

        private ISOLanguageCode GetLangauge(MediaAudio ma) {
            if (ma.Language != null && ma.Language != "Undefined") {
                return ISOLanguageCodes.Instance.GetByISOCode(ma.LanguageInfo.ISO639_Alpha3);
            }
            return null;
        }
    }

}