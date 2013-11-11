
namespace Frost.SharpMediaInfo.Output.Properties {

    public class BitRateInfo : GeneralBitRateInfo {

        public BitRateInfo(Media media) : base(media, false) {
        }

        /// <summary>Encoded (with forced padding) bit rate in bps, if some container padding is present</summary>
        public string Encoded { get { return MediaStream["BitRate_Encoded"]; } }

        /// <summary>Encoded (with forced padding) bit rate (with measurement), if some container padding is present</summary>
        public string EncodedString { get { return MediaStream["BitRate_Encoded/String"]; } }
    }
}