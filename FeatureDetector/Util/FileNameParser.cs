using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Util.ISO;

namespace Frost.DetectFeatures.Util {

    public enum SegmentType {
        Unknown,
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
        private static readonly HashSet<char> RegexReservedChars;
        private static readonly Regex ReleaseGroup = new Regex(@"-([^\. _\[\]-]{3,20})");
        private static readonly Regex PartIdentifier = new Regex(@"[\. -](part ?\d+|cd ?\d+|disk ?\d+)[\. -]?", RegexOptions.IgnoreCase);
        private static readonly Regex UrlRegex = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[\-;:&=\+\$,\w]+@)?[A-Za-z0-9\.\-]+|(?:www\.|[\-;:&=\+\$,\w]+@)[A-Za-z0-9\.\-]+)((?:\/[\+~%\/\.\w\-_]*)?\??(?:[\-\+=&;%@\.\w_]*)#?(?:[\.\!\/\\\w]*))?)");
        private readonly Dictionary<SegmentType, List<string>> _detectedSegments;
        private readonly string _fileName;
        private readonly Regex _titleAndReleaseYear;
        private List<char> _delimiters;
        private bool _releaseGroupFound;
        private bool _releaseYearFound;
        private List<string> _segments;
        private bool _titleFound;
        private static TextInfo _textInfo;

        static FileNameParser() {
            RegexReservedChars = new HashSet<char> {'.', '^', '$', '*', '+', '?', '(', ')', '[', ']', '\\', '|', '}', '{', '-'};

            CustomLangMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"JAP", "jpn"},
                {"srbski", "srp"},
                {"hrvatski", "hrv"},
            };

            SegmentExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "the", "an", "to", "man", "men", "mr", "aka", "war", "art", "ii", "be",
                "as", "my", "sin"
            };

            KnownSegments = new Dictionary<string, SegmentType>(StringComparer.OrdinalIgnoreCase) {
                {"DOKU", SegmentType.Genre},
                {"MANGA", SegmentType.Genre},
                {"XXX", SegmentType.Genre},
                {"SERIE", SegmentType.ContentType},

                #region Specials

                {"INTERNAL", SegmentType.Special },
                {"PROPER", SegmentType.Special },
                {"LIMITED", SegmentType.Special },
                {"RECODE", SegmentType.Special },
                {"REPACK", SegmentType.Special },

                #endregion

                #region Edithions

                {"EXTENDED",SegmentType.Edithion},
                {"UNCUT",SegmentType.Edithion},
                {"REMASTERED",SegmentType.Edithion},
                {"UNRATED",SegmentType.Edithion},
                {"THEATRICAL",SegmentType.Edithion},
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
                "fov",
                "fqm",
                "fragment",
                "freaks",
                "fsihd",
                "ftc",
                "ftp",
                "fua",
                "fxm",
                "fxg",
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

        public FileNameParser(string fileName, bool isFolder = false) : this(fileName, isFolder, new[] {'.', '-', ')', '(', '[', ']', '{', '}', '_', ' '}) {
        }

        public FileNameParser(string fileName, bool isFolder, params char[] delimiters) {
            _textInfo = CultureInfo.CurrentCulture.TextInfo;

            _delimiters = new List<char>(delimiters);

            _titleAndReleaseYear = GetTitleAndReleaseYearRegexWithDelimiters();

            fileName = UrlRegex.Replace(fileName, "");

            _fileName = isFolder
                ? Path.GetDirectoryName(fileName)
                : Path.GetFileNameWithoutExtension(fileName);

            _detectedSegments = new Dictionary<SegmentType, List<string>>();
        }

        public string DetectedTitle { get; private set; }

        public ICollection<char> Delimiters {
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

        private Regex GetTitleAndReleaseYearRegexWithDelimiters() {
            StringBuilder sb = new StringBuilder(_delimiters.Count + 10);
            foreach (char delimiter in _delimiters) {
                if (RegexReservedChars.Contains(delimiter)) {
                    sb.Append("\\" + delimiter);
                }
                else {
                    sb.Append(delimiter);
                }
            }

            string pattern = string.Format(@"(.+)[{0}]({1})[{0}]?", sb, @"1[89]\d{2}|2\d{3}");
            return new Regex(pattern);
        }

        public void Parse() {
            _segments = _fileName.SplitWithoutEmptyEntries(_delimiters).ToList();

            if (TryGetTitleAndReleaseYear()) {
                _titleFound = true;
                _releaseYearFound = true;

                RemoveKnownSegmentsFromTitle();
            }
            else {
                int titleEndIndex = _fileName.IndexOfAny(new[] {'-', ')', '(', '[', ']', '{', '}'});
                if (titleEndIndex > 0) {
                    _titleFound = true;
                    string title = _fileName.Substring(0, titleEndIndex)
                                            .Replace('.', ' ')
                                            .Replace('_', '_');


                    DetectedTitle = title.Trim();
                    RemoveKnownSegmentsFromTitle(false);
                }
            }

            _releaseGroupFound = CheckForReleaseGroup();
            CheckForPartIdentifier();

            //Console.WriteLine("Segments:");

            foreach (string segment in _segments) {
                CheckSegmentType(segment);
            }

            ApplyFixes();

            //remove detected segments
            foreach (string segment in DetectedSegments.Values.SelectMany(valueList => valueList)) {
                _segments.Remove(segment);
            }

            if (!_titleFound) {
                string title = GetStringBeforeFirstDetectedSegment();
                if (!string.IsNullOrEmpty(title)) {
                    DetectedTitle = title.Replace('.', ' ')
                                         .Replace('_', ' ')
                                         .Trim();

                    _titleFound = true;
                }
                else {
                    DetectedTitle = string.Join(" ", UndetectedSegments);  
                }
            }

            DetectedTitle = _textInfo.ToTitleCase(DetectedTitle);

            //Console.WriteLine();
        }

        private string GetStringBeforeFirstDetectedSegment() {
            int minIndex = _fileName.Length;

            List<string> langs = null;
            if (DetectedSegments.ContainsKey(SegmentType.Language)) {
                langs = DetectedSegments[SegmentType.Language];
            }

            foreach (List<string> detectedSegmentTypes in DetectedSegments.Values) {
                int minTypeIdx = detectedSegmentTypes.Min(segment => {
                    //skip if its a detected lanugage (as its probably wrong, stuff like 'it', 'my' 'the', 'an') 
                    if (langs != null && langs.Contains(segment)) {
                        return minIndex;
                    }
                    int idx = _fileName.IndexOf(segment, StringComparison.Ordinal);
                    return (idx < 0)
                        ? minIndex
                        : idx;
                });

                if (minTypeIdx < minIndex) {
                    minIndex = minTypeIdx;
                }
            }

            return (minIndex != _fileName.Length)
                ? _fileName.Substring(0, minIndex)
                : null;
        }

        private void RemoveKnownSegmentsFromTitle(bool releaseYearFound = true) {
            int firstSegmentIdx = -1;
            foreach (string titleSegment in DetectedTitle.SplitWithoutEmptyEntries(_delimiters)) {
                SegmentType type;
                if (KnownSegments.TryGetValue(titleSegment, out type)) {
                    CheckAddDetectedKey(type);

                    _detectedSegments[type].Add(titleSegment);

                    if (firstSegmentIdx == -1) {
                        firstSegmentIdx = DetectedTitle.IndexOf(titleSegment, StringComparison.Ordinal);
                    }

                    DetectedTitle = DetectedTitle.Replace(titleSegment, "").Trim();
                }
                else if (CheckLanguage(titleSegment)) {
                    int langIdx = DetectedTitle.IndexOf(titleSegment, StringComparison.Ordinal) - 1;

                    if (langIdx > 0) {
                        char prevLang = DetectedTitle[langIdx];

                        //only if language identifier is in brackets (to avoid colisions with the title)
                        if (prevLang == '(' || prevLang == '[' || prevLang == '{') {
                            CheckAddDetectedKey(SegmentType.Language);
                            DetectedSegments[SegmentType.Language].Add(titleSegment);

                            DetectedTitle = DetectedTitle.Remove(langIdx, titleSegment.Length + 2).Trim();
                        }
                    }
                }

                _segments.Remove(titleSegment);
            }

            //remove everything after the first known segment
            //and preserve the title only
            DetectedTitle = (firstSegmentIdx != -1 && firstSegmentIdx < DetectedTitle.Length)
                ? DetectedTitle.Remove(firstSegmentIdx).Trim()
                : DetectedTitle.Trim();

            if (releaseYearFound) {
                _segments.Remove(_detectedSegments[SegmentType.ReleaseYear].First());
            }
        }

        private void CheckSegmentType(string segment) {
            //Console.WriteLine("\t" + segment);

            if (!_releaseYearFound && CheckReleaseYear(segment)) {
                _releaseYearFound = true;
                if (!_titleFound) {
                    _titleFound = true;

                    int yearStart = _fileName.IndexOf(segment, StringComparison.Ordinal) - 1;

                    DetectedTitle = _fileName.Substring(0, yearStart)
                                             .Replace('.', ' ')
                                             .Replace('_', ' ')
                                             .Trim();
                    return;
                }
            }

            SegmentType type;
            if (KnownSegments.TryGetValue(segment, out type)) {
                CheckAddDetectedKey(type);

                _detectedSegments[type].Add(segment);
                return;
            }

            if (CheckAndAddSubtitlesLanguage(segment)) {
                return;
            }

            if (CheckAndAddLanguage(segment)) {
                return;
            }

            if (!_releaseGroupFound) {
                CheckReleaseGroup(segment);
            }
        }

        private bool TryGetTitleAndReleaseYear() {
            Match match = _titleAndReleaseYear.Match(_fileName);
            if (match.Success) {
                DetectedTitle = match.Groups[1].Value.Replace('.', ' ').Replace('_', ' ');

                CheckAddDetectedKey(SegmentType.ReleaseYear);
                _detectedSegments[SegmentType.ReleaseYear].Add(match.Groups[2].Value.Trim());

                return true;
            }
            return false;
        }

        private bool CheckForReleaseGroup() {
            Match match = ReleaseGroup.Match(_fileName);
            if (match.Success) {
                string releaseGroup = (match.Groups.Count > 1)
                    ? match.Groups[1].Value
                    : match.Value.Remove(0, 1); // remove the "-" prefix

                if (releaseGroup.Length > 2 && !SegmentExclusion.Contains(releaseGroup)) {
                    CheckAddDetectedKey(SegmentType.ReleaseGroup);

                    _detectedSegments[SegmentType.ReleaseGroup].Add(releaseGroup);
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

                _detectedSegments[SegmentType.PartIdentifier].Add(partIdentifier);
                _segments.Remove(partIdentifier);
                return true;
            }
            return false;
        }

        private void ApplyFixes() {
            //check for any misdetected stuff
            //like having subs language be just "subs" while we detected a standalone lang

            //handle misdetected lanugages
            HandleMisdetectedLanguages();
        }

        private void HandleMisdetectedLanguages() {
            if (DetectedSegments.ContainsKey(SegmentType.Language)) {
                List<string> langs = DetectedSegments[SegmentType.Language];
                int langCount = langs.Count;
                if (langCount > 1) {
                    int itIdx = langs.FindIndex(lang => lang.Equals("it", StringComparison.OrdinalIgnoreCase));
                    int myIdx = langs.FindIndex(lang => lang.Equals("my", StringComparison.OrdinalIgnoreCase));

                    if (langCount == 2) {
                        //if IT was detectd but MY not then remove IT
                        if (itIdx != -1 && myIdx == -1) {
                            langs.Remove(langs[itIdx]);
                        }
                        //if MY was detected but IT not then remove MY
                        else if (myIdx != -1 && itIdx == -1) {
                            langs.Remove(langs[myIdx]);
                        }
                        //SPECIAL CASE: bot MY and IT detected
                        //return the one with the bigger index
                        //as it was further in to the file name and
                        //has a higher probability.
                        else if (myIdx != -1 && itIdx != -1) {
                            if (itIdx > myIdx) {
                                langs.Remove(langs[itIdx]);
                            }
                            else {
                                langs.Remove(langs[myIdx]);
                            }
                        }
                    }
                }
            }
        }

        private bool CheckAndAddSubtitlesLanguage(string segment) {
            if (segment.EndsWith("subs", StringComparison.OrdinalIgnoreCase) || segment.EndsWith("sub", StringComparison.OrdinalIgnoreCase)) {
                CheckAddDetectedKey(SegmentType.SubtitleLanguage);
                DetectedSegments[SegmentType.SubtitleLanguage].Add(segment);
                return true;
            }
            return false;
        }

        private bool CheckAndAddLanguage(string segment) {
            if (!CheckLanguage(segment)) {
                return false;
            }

            CheckAddDetectedKey(SegmentType.Language);
            DetectedSegments[SegmentType.Language].Add(segment);
            return true;
        }

        private bool CheckLanguage(string segment) {
            if (segment.Length < 2 || SegmentExclusion.Contains(segment)) {
                return false;
            }

            //ISO codes are 2 or 3 letters long
            if (segment.Length < 4 && ISOLanguageCodes.IsAnISOCode(segment)) {
                return true;
            }

            return ISOLanguageCodes.IsAnISOEnglishLanguageName(segment) ||
                   CustomLangMappings.ContainsKey(segment);
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