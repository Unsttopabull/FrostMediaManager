using System;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Util.ISO;
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

                    Movie.Audio.Add(audio);
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
            a.BitRateMode = ma.BitRateInfo.Mode;
            a.ChannelPositions = ma.ChannelInfo.Positions;
            a.ChannelSetup = ma.ChannelInfo.PositionsString2;
            a.NumberOfChannels = (int?)ma.NumberOfChannels;
            a.Codec = ma.CodecIDInfo.Hint ?? ma.CodecInfo.NameString ?? a.Codec;
            a.CompressionMode = ma.CompressionMode;
            a.Duration = ma.Duration.HasValue ? (long?)ma.Duration.Value.TotalMilliseconds : null;
            a.Language = ma.Language != null
                                     ? new Language(ma.Language, ma.LanguageInfo.ISO639_Alpha2, ma.LanguageInfo.ISO639_Alpha3)
                                     : null;

            a.SamplingRate = ma.SamplingRate;

            return a;
        }
    }
}