using System;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.SharpMediaInfo.Output;

using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;

namespace Frost.DetectFeatures {
    public partial class FileFeatures {

        private void GetISOAudioInfo() {
            
        }

        private void GetAudioInfo() {
            if (_file.Extension == "iso") {
                GetISOAudioInfo();
                return;
            }

            if (_mediaFile != null) {
                foreach (MediaAudio mediaVideo in _mediaFile.Audio) {
                    Audio audio = GetFileAudioStreamInfo(mediaVideo);

                    Movie.Audios.Add(audio);
                }
            }
            else {
                Console.Error.WriteLine("Could not process the file as MediaInfo is missing: " + this);
            }            
        }

        private Audio GetFileAudioStreamInfo(MediaAudio ma) {
            Audio a = new Audio();
            a.File = _file;

            a.BitDepth = ma.BitDepth;
            a.BitRate = ma.BitRate.HasValue ? ma.BitRate / 1024.0f : null;
            a.BitRateMode = (FrameOrBitRateMode) ma.BitRateInfo.Mode;
            a.ChannelPositions = ma.ChannelInfo.Positions;
            a.ChannelSetup = ma.ChannelInfo.PositionsString2;
            a.NumberOfChannels = (int?)ma.NumberOfChannels;
            a.Codec = ma.CodecIDInfo.Hint ?? ma.CodecInfo.NameString ?? a.Codec;
            a.CompressionMode = (CompressionMode) ma.CompressionMode;
            a.Duration = ma.Duration.HasValue ? (long?)ma.Duration.Value.TotalMilliseconds : null;
            a.Language = GetLangauge(ma);

            a.SamplingRate = ma.SamplingRate;

            return a;
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