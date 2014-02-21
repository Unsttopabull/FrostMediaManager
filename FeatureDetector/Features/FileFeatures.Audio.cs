using System;
using System.Collections.Generic;
using System.Linq;
using DrWPF.Windows.Data;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Util;
using Frost.Common.Util.System.Collections.ObjectModel;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using CompressionMode = Frost.Common.CompressionMode;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using FrameOrBitRateMode = Frost.Common.FrameOrBitRateMode;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        public static CodecIdMappingCollection AudioCodecIdBindings;

        private void GetISOAudioInfo() {
        }

        private void GetAudioInfo(FileVo file) {
            if (file.Extension == "iso") {
                GetISOAudioInfo();
                return;
            }

            MediaListFile mediaFile = _mf.GetOrOpen(file.FullPath);
            if (mediaFile != null) {
                foreach (MediaAudio mediaVideo in mediaFile.Audio) {
                    Audio audio = GetFileAudioStreamInfo(_fnInfos[file.NameWithExtension], mediaVideo);
                    audio.File = file;

                    Movie.Audios.Add(audio);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }
        }

        private Audio GetFileAudioStreamInfo(FileNameInfo fnInfo, MediaAudio ma) {
            Audio a = new Audio();

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
                return AudioCodecIdBindings.ContainsKey(codec)
                    ? AudioCodecIdBindings[codec]
                    : codec;
            }

            return AudioCodecIdBindings.ContainsKey(id)
                ? AudioCodecIdBindings[id]
                : id;
        }

        private void AddFileNameInfo(FileNameInfo fnInfo, Audio audio) {
            if (fnInfo.AudioSource != null) {
                audio.Source = fnInfo.AudioSource;
            }

            audio.Codec = fnInfo.AudioCodec;
            audio.Type = fnInfo.AudioQuality;

            if (fnInfo.Language != null) {
                audio.Language = CheckLanguage(new Language(fnInfo.Language));
            }
        }

        private Language GetLangauge(MediaAudio ma) {
            if (ma.Language != null && ma.Language != "Undefined") {
                Language languageToCheck = new Language(ma.LanguageInfo.Full1.Trim(), ma.LanguageInfo.ISO639_Alpha2, ma.LanguageInfo.ISO639_Alpha3);
                Language lang = CheckLanguage(languageToCheck);
                return lang;
            }
            return null;
        }
    }

}