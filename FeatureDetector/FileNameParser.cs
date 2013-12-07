using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures {

    public enum SegmentType {
        Unknown,
        Title,
        DVDRegion,
        ContentType,
        Genre,
        Edithion,
        SubtitleLanguage,
        Language,
        ReleaseYear,

        VideoSource,
        VideoQuality,
        VideoCodec,


        AudioSource,
        AudioCodec,
        AudioQuality,

        Special,
        ReleaseGroup,
        PartIdentifier,

    }

    public class FileNameParser {

        private static readonly ISOLanguageCodes ISOLanguageCodes = ISOLanguageCodes.Instance;
        private static readonly HashSet<string> SegmentExclusion;
        private static readonly HashSet<string> ReleaseGroups;
        private static readonly Dictionary<string, string> CustomLangMappings;
        private static readonly Dictionary<string, SegmentType> KnownSegments;
        private readonly Dictionary<SegmentType, List<string>> _detectedSegments;
        private static readonly Regex ReleaseGroup = new Regex(@"-([^\. _\[\]-]{3,20})");
        private static readonly Regex PartIdentifier = new Regex(@"[\. -](part\d+|cd\d+|disk\d+)[\. -]?", RegexOptions.IgnoreCase);
        private static readonly Regex UrlRegex = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[\-;:&=\+\$,\w]+@)?[A-Za-z0-9\.\-]+|(?:www\.|[\-;:&=\+\$,\w]+@)[A-Za-z0-9\.\-]+)((?:\/[\+~%\/\.\w\-_]*)?\??(?:[\-\+=&;%@\.\w_]*)#?(?:[\.\!\/\\\w]*))?)");
        private readonly string _fileName;
        private List<string> _delimiters;
        private List<string> _segments;
        private bool _releaseGroupFound;

        static FileNameParser() {
            CustomLangMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"JAP", "jpn"},
                {"srbski", "srp"},
                {"hrvatski", "hrv"},
            };

            SegmentExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "the", "an", "to", "man", "men", "mr", "aka", "war", "art", "ii", "be",
                "as"
            };

            KnownSegments = new Dictionary<string, SegmentType>(StringComparer.OrdinalIgnoreCase) {
                {"DOKU", SegmentType.Genre},
                {"MANGA", SegmentType.Genre},
                {"XXX", SegmentType.Genre},
                {"SERIE", SegmentType.ContentType},

                #region Edithions

                {"EXTENDED", SegmentType.Edithion},
                {"UNCUT", SegmentType.Edithion},
                {"REMASTERED", SegmentType.Edithion},
                {"UNRATED", SegmentType.Edithion},
                {"THEATRICAL", SegmentType.Edithion},
                {"CHRONO", SegmentType.Edithion},

                #endregion

                #region Audio

                {"MD", SegmentType.AudioSource},
                {"LINE", SegmentType.AudioSource},
                {"LD", SegmentType.AudioSource},

                #endregion

                #region AudioCodec

                {"AC3", SegmentType.AudioCodec},
                {"AC3D", SegmentType.AudioCodec},
                {"DTS", SegmentType.AudioCodec},
                {"AAC", SegmentType.AudioCodec},

                #endregion

                #region Subtitles

                {"DUBBED", SegmentType.AudioQuality},
                {"SUBBED", SegmentType.VideoQuality},

                #endregion

                {"DL", SegmentType.Special},
                {"DC", SegmentType.Special},

                #region Video Codecs

                {"XVID", SegmentType.VideoCodec},
                {"DIVX", SegmentType.VideoCodec},
                {"x264", SegmentType.VideoCodec},
                {"h264", SegmentType.VideoCodec},

                #endregion

                #region Video

                //fullscreen
                {"FS", SegmentType.VideoQuality},

                //widescreen
                {"WS", SegmentType.VideoQuality},

                //other rips
                {"DHRIP", SegmentType.VideoSource},
                {"HDRIP", SegmentType.VideoSource},

                //Web
                {"WEBRIP", SegmentType.VideoSource},
                {"WEB-Rip", SegmentType.VideoSource},
                {"WEBDL", SegmentType.VideoSource},
                
                //Bluray
                {"BLURAY", SegmentType.VideoSource},
                {"BLUERAY", SegmentType.VideoSource},
                {"BLURAYRIP", SegmentType.VideoSource},
                {"BD", SegmentType.VideoSource},
                {"BDRIP", SegmentType.VideoSource},
                {"BRRIP", SegmentType.VideoSource},
                {"Blu-Ray", SegmentType.VideoSource},
                {"BDR", SegmentType.VideoSource},
                {"BD5", SegmentType.VideoSource},
                {"BD25", SegmentType.VideoSource},
                {"BD9", SegmentType.VideoSource},
                {"BD50", SegmentType.VideoSource},

                //Cam
                {"HDCam", SegmentType.VideoSource},
                {"HDCamRip", SegmentType.VideoSource},
                {"CAM", SegmentType.VideoSource},
                {"CAMRip", SegmentType.VideoSource},

                //DVD
                {"DVD5", SegmentType.VideoSource},
                {"DVD9", SegmentType.VideoSource},
                {"DVDRIP", SegmentType.VideoSource},
                {"DVD", SegmentType.VideoSource},
                {"DVDR", SegmentType.VideoSource},
                {"DVD-Full", SegmentType.VideoSource},
                {"Full-Rip", SegmentType.VideoSource},
                {"ISORip", SegmentType.VideoSource},
                {"HDDVD", SegmentType.VideoSource},
                //regions
                {"R0", SegmentType.DVDRegion},
                {"R1", SegmentType.DVDRegion},
                {"R2", SegmentType.DVDRegion},
                {"R3", SegmentType.DVDRegion},
                {"R4", SegmentType.DVDRegion},
                {"R5", SegmentType.DVDRegion},
                {"R6", SegmentType.DVDRegion},
                {"R7", SegmentType.DVDRegion},
                {"R8", SegmentType.DVDRegion},

                //TV
                {"DTV", SegmentType.VideoSource},
                {"TVRip", SegmentType.VideoSource},
                {"HDTV", SegmentType.VideoSource},
                {"HDTVRip", SegmentType.VideoSource},
                {"PDTV", SegmentType.VideoSource},
                {"DVBRip", SegmentType.VideoSource},
                {"DVB", SegmentType.VideoSource},
                {"SATRIP", SegmentType.VideoSource},

                //straight to video / satelite tv
                {"STV", SegmentType.VideoSource},

                //Video on demand
                {"VODRip", SegmentType.VideoSource},
                {"VODR", SegmentType.VideoSource},

                //Digital Satelite
                {"DSR", SegmentType.VideoSource},
                {"DSRRip", SegmentType.VideoSource},

                //DirectToHome
                {"DTH", SegmentType.VideoSource},
                {"DTHRip", SegmentType.VideoSource},

                //WORKPRINT
                {"WP", SegmentType.VideoSource},
                {"WORKPRINT", SegmentType.VideoSource},
                
                //TELECINE
                {"TC", SegmentType.VideoSource},
                {"TELECINE", SegmentType.VideoSource},

                //TELESYNC
                {"TS", SegmentType.VideoSource},
                {"TELESYNC", SegmentType.VideoSource},
                {"PDVD", SegmentType.VideoSource},

                //Video Quality
                {"720i", SegmentType.VideoQuality},
                {"720p", SegmentType.VideoQuality},
                {"1080i", SegmentType.VideoQuality},
                {"1080p", SegmentType.VideoQuality},
                {"DDC", SegmentType.VideoSource},

                //screeners
                {"SCREENER", SegmentType.VideoSource},
                {"SCR", SegmentType.VideoSource},
                {"DVDSCREENER", SegmentType.VideoSource},
                {"DVDSCR", SegmentType.VideoSource},
                {"BDSCR", SegmentType.VideoSource},

                #endregion

                #region Specials

                {"INTERNAL", SegmentType.Special},
                {"PROPER", SegmentType.Special},
                {"LIMITED", SegmentType.Special},
                {"RECODE", SegmentType.Special},
                {"REPACK", SegmentType.Special},

                #endregion
            };

            #region Release Groups

            ReleaseGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "666",
                "aaf",
                "ac",
                "acclaim",
                "adhd",
                "aen",
                "aeroholics",
                "afo",
                "alliance",
                "amiable",
                "amstel",
                "anarchy",
                "anivcd",
                "archivist",
                "argon",
                "arigold",
                "ariscrapaysites",
                "arrow",
                "arthouse",
                "asister",
                "assass",
                "aterfallet",
                "ats",
                "avcdvd",
                "avchd",
                "avs",
                "awake",
                "axial",
                "axxo",
                "azninvasian",
                "backtorg",
                "baco",
                "bajskorv",
                "bald",
                "ballerina",
                "bamhd",
                "band",
                "baratv",
                "bavaria",
                "bawls",
                "bbdvdr",
                "bbv",
                "bdflix",
                "bdisc",
                "bdp",
                "bestdivx",
                "besthd",
                "betamax",
                "bia",
                "bien",
                "bierbuiken",
                "bifos",
                "biq",
                "bloodweiser",
                "bluntrola",
                "bountyhunters",
                "bow",
                "brastemp",
                "brein",
                "brg",
                "brmp",
                "bulldozer",
                "burger",
                "camelot",
                "camera",
                "caph",
                "carver",
                "cbgb",
                "ccat",
                "cdd",
                "centropy",
                "cfe",
                "chakra",
                "chawin",
                "chrono",
                "chwdgb",
                "cielo",
                "cinefile",
                "cinemaniacs",
                "cinevcd",
                "circle",
                "class",
                "classic",
                "cme",
                "cnldvdr",
                "coalition",
                "cocain",
                "coma",
                "condition",
                "council",
                "counterfeit",
                "cowry",
                "cremosa",
                "crimson",
                "ctu",
                "cult",
                "culthd",
                "cybermen",
                "dash",
                "dcp",
                "debcz",
                "defused",
                "dfa",
                "dfd",
                "dgas",
                "dgx",
                "dhd",
                "diamond",
                "dich",
                "different",
                "dimension",
                "dissent",
                "divxfactory",
                "dmd",
                "dmt",
                "dna",
                "dnb",
                "document",
                "domino",
                "done",
                "dor",
                "dot",
                "down",
                "dpimp",
                "drabbits",
                "dreamlight",
                "drg",
                "droids",
                "drsi",
                "dtfs",
                "dubby",
                "dvdr",
                "dvf",
                "echos",
                "elevation",
                "elia",
                "emerald",
                "episode",
                "eryx",
                "espise",
                "etrg",
                "etach",
                "euhd",
                "exist",
                "expired",
                "exvidint",
                "feature",
                "fhd",
                "fico",
                "ficodvdr",
                "fixit",
                "flair",
                "flaite",
                "flik",
                "fls",
                "fme",
                "foa",
                "football",
                "fov",
                "fqm",
                "fragment",
                "freaks",
                "fsihd",
                "ftc",
                "ftp",
                "fua",
                "fxm",
                "fzero",
                "gaygay",
                "gdr",
                "geneside",
                "genuine",
                "gfw",
                "gnarly",
                "google",
                "gore",
                "haco",
                "hafvcd",
                "haggis",
                "halcyon",
                "hdc",
                "hdcp",
                "hdi",
                "hdv",
                "heh",
                "hls",
                "hnr",
                "hookah",
                "hype",
                "iak",
                "ibex",
                "ifn",
                "ignite",
                "iguana",
                "ihate",
                "iht",
                "ika",
                "ilg",
                "ils",
                "imagine",
                "imbt",
                "immortals",
                "inbev",
                "incite",
                "inclusion",
                "infamous",
                "ingot",
                "insects",
                "inside",
                "inspired",
                "intimid",
                "inya",
                "isg",
                "itg",
                "jackal",
                "jackvid",
                "japhson",
                "jbs",
                "jbw",
                "jfkxvid",
                "jkr",
                "jmt",
                "jupiler",
                "kamera",
                "kart",
                "kickoff",
                "killzone",
                "kingdom",
                "klassigerhd",
                "koczka",
                "kyr",
                "laj",
                "lap",
                "larceny",
                "lchd",
                "leverage",
                "levity",
                "lionsden",
                "lkrg",
                "lmao",
                "lmg",
                "loki",
                "lol",
                "lpd",
                "lrc",
                "ltu",
                "lu",
                "lumix",
                "luso",
                "machd",
                "maf",
                "mccain",
                "meddy",
                "mediamaniacs",
                "melite",
                "micro",
                "mint",
                "miragetv",
                "misfitz",
                "misty",
                "moa",
                "moh",
                "momentum",
                "morich",
                "motion",
                "mptdvd",
                "mrproper",
                "msd",
                "mv",
                "mvm",
                "mvn",
                "mvs",
                "ndn",
                "nedivx",
                "neptune",
                "ngr",
                "nhh",
                "nlsgdvdr",
                "nod",
                "nodlabs",
                "noir",
                "nosegment",
                "notv",
                "notyou",
                "npw",
                "nti",
                "ntx",
                "octi",
                "omerta",
                "omicron",
                "opium",
                "optic",
                "orc",
                "orenji",
                "organic",
                "oro",
                "ositv",
                "particle",
                "pathe",
                "pfa",
                "pix",
                "playoff",
                "pleaders",
                "pmv",
                "pokerus",
                "pornolation",
                "pot",
                "prevail",
                "prism",
                "promise",
                "proxy",
                "pukka",
                "pushercrew",
                "puzzle",
                "pvr",
                "qcf",
                "qim",
                "qrc",
                "quidam",
                "ragedvd",
                "rap",
                "rcdivx",
                "reactor",
                "refined",
                "regexp",
                "reign",
                "reiseradio",
                "remax",
                "replica",
                "repomen",
                "republic",
                "resistance",
                "retreat",
                "rets",
                "riff",
                "ritalin",
                "river",
                "rkctv",
                "rrr",
                "rta",
                "ruby",
                "rustle",
                "rvl",
                "s4a",
                "sadpanda",
                "safire",
                "saints",
                "santi",
                "saphire",
                "savannah",
                "scared",
                "scream",
                "sdh",
                "septic",
                "ser",
                "sex",
                "sexsh",
                "sfm",
                "sibv",
                "silvercam",
                "sink",
                "sinners",
                "sirius",
                "siso",
                "sitv",
                "ska",
                "slugger",
                "smut",
                "sof",
                "solved",
                "sometv",
                "sparel",
                "spooky",
                "sprinter",
                "srp",
                "ssf",
                "stfc",
                "strong",
                "stylezz",
                "submerge",
                "subtitles",
                "superier",
                "supreme",
                "svd",
                "swe",
                "sys",
                "taste",
                "tbs",
                "tcm",
                "tdf",
                "tdm",
                "tekate",
                "television",
                "tesoro",
                "tfin",
                "tgf",
                "tgp",
                "thebatman",
                "thewretched",
                "thugline",
                "tiix",
                "timelords",
                "tko",
                "tlf",
                "tokus",
                "tr0unce",
                "tvd",
                "tvfi",
                "tvtupa",
                "twist",
                "twophat",
                "ubik",
                "ubr",
                "ulf",
                "umf",
                "universal",
                "unskilled",
                "untouched",
                "utopia",
                "valiomedia",
                "vcdvault",
                "vcore",
                "vcf",
                "viahd",
                "videocd",
                "vision",
                "vite",
                "voa",
                "vomit",
                "vst",
                "walmart",
                "wastedtime",
                "waters",
                "wbz",
                "whoknow",
                "wiki",
                "wira",
                "wpi",
                "wrd",
                "xanax",
                "xart",
                "xcite",
                "xcopy",
                "xdominionx",
                "xor",
                "xoxo",
                "xpress",
                "xscr",
                "xstreem",
                "ycdv",
                "yestv",
                "zet",
                "zoom"
            };

            #endregion
        }

        public FileNameParser(string fileName, bool isFolder = false) : this(fileName, isFolder, new[] {".", "-", ")", "(", "[", "]", "{", "}", "_", " "}) {
        }

        public FileNameParser(string fileName, bool isFolder, params string[] delimiters) {
            fileName = UrlRegex.Replace(fileName, "");

            _fileName = isFolder
                ? Path.GetDirectoryName(fileName)
                : Path.GetFileNameWithoutExtension(fileName);

            _delimiters = new List<string>(delimiters);

            _detectedSegments = new Dictionary<SegmentType, List<string>>();
        }

        public ICollection<string> Delimiters {
            get { return _delimiters; }
            set { _delimiters = value.ToList(); }
        }

        public List<string> UndetectedSegments {
            get { return _segments; }
        }

        public Dictionary<SegmentType, List<string>> DetectedSegments {
            get { return _detectedSegments; }
        }

        public void AddKnownSegment(string value, SegmentType type) {
            KnownSegments.Add(value, type);
        }

        public void Parse() {
            _segments = _fileName.SplitWithoutEmptyEntries(_delimiters).ToList();

            _releaseGroupFound = CheckForReleaseGroup();
            CheckForPartIdentifier();

            Console.WriteLine("Segments:");

            foreach (string segment in _segments) {
                Console.WriteLine("\t"+segment);

                if (CheckReleaseYear(segment)) {
                    int yearStart = _fileName.IndexOf(segment, StringComparison.Ordinal) - 1;

                    CheckAddDetectedKey(SegmentType.Title);
                    string title = _fileName.Substring(0, yearStart).Replace('.', ' ');
                    _detectedSegments[SegmentType.Title].Add(title);
                    continue;
                }

                SegmentType type;
                if (KnownSegments.TryGetValue(segment, out type)) {
                    CheckAddDetectedKey(type);

                    _detectedSegments[type].Add(segment);
                    continue;
                }

                if (CheckSubtitlesLanguage(segment)) {
                    continue;
                }

                if (CheckLanguage(segment)) {
                    continue;
                }

                if (!_releaseGroupFound) {
                    CheckReleaseGroup(segment);
                }
            }
            ApplyFixes();

            Console.WriteLine();

            //remove detected segments
            foreach (string segment in DetectedSegments.Values.SelectMany(valueList => valueList)) {
                _segments.Remove(segment);
            }
        }

        private bool CheckForReleaseGroup() {
            Match match = ReleaseGroup.Match(_fileName);
            if (match.Success) {
                string releaseGroup = (match.Groups.Count > 1)
                    ? match.Groups[1].Value
                    : match.Value.Remove(0, 1); // remove the "-" prefix

                if (releaseGroup.Length > 2 && !SegmentExclusion.Contains(releaseGroup)) {
                    CheckAddDetectedKey(SegmentType.ReleaseGroup);

                    _detectedSegments[SegmentType.ReleaseGroup].Add(releaseGroup + "*");
                    _segments.Remove(releaseGroup);
                    return true;
                }
            }
            return false;
        }

        private bool CheckForPartIdentifier() {
            Match match = PartIdentifier.Match(_fileName);
            if (match.Success) {
                string partIdentifier = (match.Groups.Count > 1)
                    ? match.Groups[1].Value
                    : match.Value;

                CheckAddDetectedKey(SegmentType.PartIdentifier);

#if DEBUG
                _detectedSegments[SegmentType.PartIdentifier].Add(partIdentifier + "*");
#else
                _detectedSegments[SegmentType.PartIdentifier].Add(partIdentifier);
#endif

                _segments.Remove(partIdentifier);
                return true;
            }
            return false;
        }

        private void ApplyFixes() {
            //check for any misdetected stuff
            //like having subs language be just "subs" while we detected a standalone lang

        }

        private bool CheckSubtitlesLanguage(string segment) {
            if (segment.EndsWith("subs", StringComparison.OrdinalIgnoreCase) || segment.EndsWith("sub", StringComparison.OrdinalIgnoreCase)) {
                CheckAddDetectedKey(SegmentType.SubtitleLanguage);
                DetectedSegments[SegmentType.SubtitleLanguage].Add(segment);
                return true;
            }
            return false;
        }

        private bool CheckLanguage(string segment) {
            if (segment.Length < 2 || SegmentExclusion.Contains(segment)) {
                return false;
            }

            //ISO codes are 2 or 3 letters long
            if (segment.Length < 4 && ISOLanguageCodes.IsAnISOCode(segment)) {
                CheckAddDetectedKey(SegmentType.Language);
                DetectedSegments[SegmentType.Language].Add(segment);
                return true;
            }

            if (ISOLanguageCodes.IsAnISOEnglishLanguageName(segment) || CustomLangMappings.ContainsKey(segment)) {
                CheckAddDetectedKey(SegmentType.Language);
                DetectedSegments[SegmentType.Language].Add(segment);
                return true;
            }
            return false;
        }

        private bool CheckReleaseYear(string segment) {
            //if a segment is a release year
            if (IsReleaseYear(segment)) {
                CheckAddDetectedKey(SegmentType.ReleaseYear);

                _detectedSegments[SegmentType.ReleaseYear].Add(segment);
                return true;
            }
            return false;
        }

        private void CheckReleaseGroup(string segment) {
            //if not detected before, check if it's a Release Group name
            if (segment.Length > 2 && ReleaseGroups.Contains(segment)) {
                CheckAddDetectedKey(SegmentType.ReleaseGroup);
                _detectedSegments[SegmentType.ReleaseGroup].Add(segment);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckAddDetectedKey(SegmentType type) {
            if (!_detectedSegments.ContainsKey(type)) {
                _detectedSegments.Add(type, new List<string>());
            }
        }

        /// <summary>Determines whether the input string is a valid 4 digit number.</summary>
        /// <param name="segment">The segment to test.</param>
        /// <returns>Is <c>true</c> if the input is a string 4 letters long, containing only numbers.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsReleaseYear(string segment) {
            if (segment.Length != 4) {
                return false;
            }
            if (segment[0] < '1') {
                return false;
            }
            //A year after 1800
            if (segment[0] == '1' && segment[1] < '8') {
                return false;
            }
            //every letter is a digit
            return segment.All(ch => ch >= '0' && ch <= '9');
        }

    }

}