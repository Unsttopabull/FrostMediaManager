using Frost.SharpMediaInfo.Output.Properties;
using Frost.SharpMediaInfo.Output.Properties.BitRate;
using Frost.SharpMediaInfo.Output.Properties.Codecs;
using Frost.SharpMediaInfo.Output.Properties.Delay;
using Frost.SharpMediaInfo.Output.Properties.Duration;
using Frost.SharpMediaInfo.Output.Properties.Formats;
using Frost.SharpMediaInfo.Output.Properties.General;

#pragma warning disable 1591

namespace Frost.SharpMediaInfo.Output {
    public class MediaGeneral : Media {

        public MediaGeneral(MediaFile mediaInfo) : base(mediaInfo, StreamKind.General) {
            CachedStreamCount = 1;
            Format = new GeneralFormat(this);
            Codec = new GeneralCodec(this);
            DurationInfo = new GeneralDurationInfo(this);
            DelayInfo = new GeneralDelayInfo(this);
            StreamSizeInfo = new StreamSizeInfo(this);
            OverallBitRateInfo = new GeneralBitRateInfo(this);
            Season = new Season(this);
            Movie = new Movie(this);
            Album = new Album(this);
            Comic = new Comic(this);
            Track = new Track(this);
            Performer = new Performer(this);
            EncodingLibraryInfo = new EncodingLibraryInfo(this);
            Service = new ServiceInfo(this);
            Part = new PartInfo(this);
            CoverInfo = new CoverInfo(this);
            OriginalSourceInfo = new OriginalSourceInfo(this);
            FileInfo = new FileInfo(this);
        }

        /// <summary>Number of general streams</summary>
        public string GeneralCount { get { return this["GeneralCount"]; } }

        /// <summary>Number of video streams</summary>
        public string VideoCount { get { return this["VideoCount"]; } }
        /// <summary>Video Codecs in this file, separated by /</summary>
        public string VideoFormatList { get { return this["Video_Format_List"]; } }
        /// <summary>Video Codecs in this file with popular name (hint), separated by /</summary>
        public string VideoFormatWithHintList { get { return this["Video_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string VideoCodecList { get { return this["Video_Codec_List"]; } }
        /// <summary>Video languagesin this file, full names, separated by /</summary>
        public string VideoLanguageList { get { return this["Video_Language_List"]; } }

        /// <summary>Number of audio streams</summary>
        public string AudioCount { get { return this["AudioCount"]; } }
        /// <summary>Audio Codecs in this file,separated by /</summary>
        public string AudioFormatList { get { return this["Audio_Format_List"]; } }
        /// <summary>Audio Codecs in this file with popular name (hint), separated by /</summary>
        public string AudioFormatWithHintList { get { return this["Audio_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string AudioCodecList { get { return this["Audio_Codec_List"]; } }
        /// <summary>Audio languages in this file separated by /</summary>
        public string AudioLanguageList { get { return this["Audio_Language_List"]; } }

        /// <summary>Number of text streams</summary>
        public string TextCount { get { return this["TextCount"]; } }
        /// <summary>Text Codecs in this file, separated by /</summary>
        public string TextFormatList { get { return this["Text_Format_List"]; } }
        /// <summary>Text Codecs in this file with popular name (hint),separated by /</summary>
        public string TextFormatWithHintList { get { return this["Text_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string TextCodecList { get { return this["Text_Codec_List"]; } }
        /// <summary>Text languages in this file, separated by /</summary>
        public string TextLanguageList { get { return this["Text_Language_List"]; } }

        /// <summary>Number of other streams</summary>
        public string OtherCount { get { return this["OtherCount"]; } }
        /// <summary>Other formats in this file, separated by /</summary>
        public string OtherFormatList { get { return this["Other_Format_List"]; } }
        /// <summary>Other formats in this file with popular name (hint), separated by /</summary>
        public string OtherFormatWithHintList { get { return this["Other_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string OtherCodecList { get { return this["Other_Codec_List"]; } }
        /// <summary>Chapters languages in this file, separated by /</summary>
        public string OtherLanguageList { get { return this["Other_Language_List"]; } }

        /// <summary>Number of image streams</summary>
        public string ImageCount { get { return this["ImageCount"]; } }
        /// <summary>Image Codecs in this file, separated by /</summary>
        public string ImageFormatList { get { return this["Image_Format_List"]; } }
        /// <summary>Image Codecs in this file with popular name (hint), separated by /</summary>
        public string ImageFormatWithHintList { get { return this["Image_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string ImageCodecList { get { return this["Image_Codec_List"]; } }
        /// <summary>Image languages in this file, separated by /</summary>
        public string ImageLanguageList { get { return this["Image_Language_List"]; } }

        /// <summary>Number of menu streams</summary>
        public string MenuCount { get { return this["MenuCount"]; } }
        /// <summary>Menu Codecsin this file, separated by /</summary>
        public string MenuFormatList { get { return this["Menu_Format_List"]; } }
        /// <summary>Menu Codecs in this file with popular name (hint),separated by /</summary>
        public string MenuFormatWithHintList { get { return this["Menu_Format_WithHint_List"]; } }
        /// <summary>Deprecated, do not use in new projects</summary>
        public string MenuCodecList { get { return this["Menu_Codec_List"]; } }
        /// <summary>Menu languages in this file, separated by /</summary>
        public string MenuLanguageList { get { return this["Menu_Language_List"]; } }

        /// <summary>Format used</summary>
        public GeneralFormat Format { get; private set; }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string InternetMediaType { get { return this["InternetMediaType"]; } }

        /// <summary>If Audio and video are muxed</summary>
        public string Interleaved { get { return this["Interleaved"]; } }

        /// <summary>Deprecated, do not use in new projects</summary>
        public GeneralCodec Codec { get; private set; }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this["Duration"]; } }
        public GeneralDurationInfo DurationInfo { get; private set; }

        /// <summary>Bit rate of all streams in bps</summary>
        public string OverallBitRate { get { return this["OverallBitRate"]; } }
        public GeneralBitRateInfo OverallBitRateInfo { get; private set; }

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this["Delay"]; } }
        public GeneralDelayInfo DelayInfo { get; private set; }

        /// <summary>Stream size in bytes</summary>
        public string StreamSize { get { return this["StreamSize"]; } }
        public StreamSizeInfo StreamSizeInfo { get; private set; }

        public string HeaderSize { get { return this["HeaderSize"]; } }
        public string DataSize { get { return this["DataSize"]; } }
        public string FooterSize { get { return this["FooterSize"]; } }

        public string IsStreamable { get { return this["IsStreamable"]; } }

        /// <summary>The gain to apply to reach 89dB SPL on playback</summary>
        public string AlbumReplayGainGain { get { return this["Album_ReplayGain_Gain"]; } }
        /// <summary>The maximum absolute peak value of the item</summary>
        public string AlbumReplayGainPeak { get { return this["Album_ReplayGain_Peak"]; } }

        public string Encryption { get { return this["Encryption"]; } }

        /// <summary>(Generic)Title of file</summary>
        public string Title { get { return this["Title"]; } }
        /// <summary>(Generic)More info about the title of file</summary>
        public string TitleMore { get { return this["Title/More"]; } }
        /// <summary>(Generic)Url</summary>
        public string TitleUrl { get { return this["Title/Url"]; } }

        /// <summary>Univers movies belong to, e.g. Starwars, Stargate, Buffy, Dragonballs</summary>
        public string Domain { get { return this["Domain"]; } }
        /// <summary>Name of the series, e.g. Starwars movies, Stargate SG-1, Stargate Atlantis, Buffy, Angel</summary>
        public string Collection { get { return this["Collection"]; } }

        public Season Season { get; private set; }
        public Movie Movie { get; private set; }
        public Album Album { get; private set; }
        public Comic Comic { get; private set; }
        public PartInfo Part { get; private set; }
        public Track Track { get; private set; }

        public string Grouping { get { return this["Grouping"]; } }
        public string Chapter { get { return this["Chapter"]; } }
        public string SubTrack { get { return this["SubTrack"]; } }

        public string OriginalAlbum { get { return this["Original/Album"]; } }
        public string OriginalMovie { get { return this["Original/Movie"]; } }
        public string OriginalPart { get { return this["Original/Part"]; } }
        public string OriginalTrack { get { return this["Original/Track"]; } }

        public string Compilation { get { return this["Compilation"]; } }
        public string CompilationString { get { return this["Compilation/String"]; } }

        public Performer Performer { get; private set; }

        public string Accompaniment { get { return this["Accompaniment"]; } }

        public string Composer { get { return this["Composer"]; } }
        public string ComposerNationality { get { return this["Composer/Nationality"]; } }

        public string Arranger { get { return this["Arranger"]; } }
        public string Lyricist { get { return this["Lyricist"]; } }
        public string OriginalLyricist { get { return this["Original/Lyricist"]; } }
        public string Conductor { get { return this["Conductor"]; } }

        public string Director { get { return this["Director"]; } }
        public string AssistantDirector { get { return this["AssistantDirector"]; } }
        public string DirectorOfPhotography { get { return this["DirectorOfPhotography"]; } }

        public string SoundEngineer { get { return this["SoundEngineer"]; } }
        public string ArtDirector { get { return this["ArtDirector"]; } }
        public string ProductionDesigner { get { return this["ProductionDesigner"]; } }
        public string Choregrapher { get { return this["Choregrapher"]; } }
        public string CostumeDesigner { get { return this["CostumeDesigner"]; } }

        public string Actor { get { return this["Actor"]; } }
        public string ActorCharacter { get { return this["Actor_Character"]; } }

        public string WrittenBy { get { return this["WrittenBy"]; } }
        public string ScreenplayBy { get { return this["ScreenplayBy"]; } }
        public string EditedBy { get { return this["EditedBy"]; } }
        public string CommissionedBy { get { return this["CommissionedBy"]; } }

        public string Producer { get { return this["Producer"]; } }
        public string CoProducer { get { return this["CoProducer"]; } }
        public string ExecutiveProducer { get { return this["ExecutiveProducer"]; } }

        public string MusicBy { get { return this["MusicBy"]; } }
        public string DistributedBy { get { return this["DistributedBy"]; } }
        public string OriginalSourceFormDistributedBy { get { return this["OriginalSourceForm/DistributedBy"]; } }
        public string MasteredBy { get { return this["MasteredBy"]; } }
        public string EncodedBy { get { return this["EncodedBy"]; } }
        public string RemixedBy { get { return this["RemixedBy"]; } }
        public string ProductionStudio { get { return this["ProductionStudio"]; } }
        public string ThanksTo { get { return this["ThanksTo"]; } }

        public string Publisher { get { return this["Publisher"]; } }
        public string PublisherURL { get { return this["Publisher/URL"]; } }

        public string Label { get { return this["Label"]; } }
        public string Genre { get { return this["Genre"]; } }
        public string Mood { get { return this["Mood"]; } }
        public string ContentType { get { return this["ContentType"]; } }
        public string Subject { get { return this["Subject"]; } }
        public string Description { get { return this["Description"]; } }
        public string Keywords { get { return this["Keywords"]; } }
        public string Summary { get { return this["Summary"]; } }
        public string Synopsis { get { return this["Synopsis"]; } }
        public string Period { get { return this["Period"]; } }

        public string LawRating { get { return this["LawRating"]; } }
        public string LawRatingReason { get { return this["LawRating_Reason"]; } }
        public string Icra { get { return this["ICRA"]; } }

        public string ReleasedDate { get { return this["Released_Date"]; } }
        public string OriginalReleasedDate { get { return this["Original/Released_Date"]; } }

        public string RecordedDate { get { return this["Recorded_Date"]; } }
        public string EncodedDate { get { return this["Encoded_Date"]; } }
        public string TaggedDate { get { return this["Tagged_Date"]; } }
        public string WrittenDate { get { return this["Written_Date"]; } }
        public string MasteredDate { get { return this["Mastered_Date"]; } }

        public FileInfo FileInfo { get; private set; }

        public string RecordedLocation { get { return this["Recorded_Location"]; } }
        public string WrittenLocation { get { return this["Written_Location"]; } }
        public string ArchivalLocation { get { return this["Archival_Location"]; } }

        public string EncodedApplication { get { return this["Encoded_Application"]; } }
        public string EncodedApplicationUrl { get { return this["Encoded_Application/Url"]; } }

        /// <summary>Software used to create the file</summary>
        public string EncodedLibrary { get { return this["Encoded_Library"]; } }
        public EncodingLibraryInfo EncodingLibraryInfo { get; private set; }

        public string Cropped { get { return this["Cropped"]; } }
        public string Dimensions { get { return this["Dimensions"]; } }
        public string DotsPerInch { get { return this["DotsPerInch"]; } }
        public string Lightness { get { return this["Lightness"]; } }

        public OriginalSourceInfo OriginalSourceInfo { get; private set; }

        public string TaggedApplication { get { return this["Tagged_Application"]; } }

        public string BPM { get { return this["BPM"]; } }
        public string ISRC { get { return this["ISRC"]; } }
        public string ISBN { get { return this["ISBN"]; } }

        public string BarCode { get { return this["BarCode"]; } }
        public string LCCN { get { return this["LCCN"]; } }

        public string CatalogNumber { get { return this["CatalogNumber"]; } }
        public string LabelCode { get { return this["LabelCode"]; } }
        public string Owner { get { return this["Owner"]; } }

        public string Copyright { get { return this["Copyright"]; } }
        public string CopyrightUrl { get { return this["Copyright/Url"]; } }
        public string ProducerCopyright { get { return this["Producer_Copyright"]; } }

        public string TermsOfUse { get { return this["TermsOfUse"]; } }

        public ServiceInfo Service { get; private set; }

        public string NetworkName { get { return this["NetworkName"]; } }
        public string OriginalNetworkName { get { return this["OriginalNetworkName"]; } }

        public string Country { get { return this["Country"]; } }
        public string TimeZone { get { return this["TimeZone"]; } }

        public string Cover { get { return this["Cover"]; } }
        public CoverInfo CoverInfo { get; private set; }

        public string Lyrics { get { return this["Lyrics"]; } }
        public string Comment { get { return this["Comment"]; } }
        public string Rating { get { return this["Rating"]; } }

        public string AddedDate { get { return this["Added_Date"]; } }

        public string PlayedFirstDate { get { return this["Played_First_Date"]; } }
        public string PlayedLastDate { get { return this["Played_Last_Date"]; } }
        public string PlayedCount { get { return this["Played_Count"]; } }

        public string EPGPositionsBegin { get { return this["EPG_Positions_Begin"]; } }
        public string EPGPositionsEnd { get { return this["EPG_Positions_End"]; } }
    }
}