using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    internal enum LanguageCheck {
        NotALangauge,
        ISOCode,
        ISOEnglishName,
        Custom
    }

    public class FileNameParser {
        private static readonly ISOLanguageCodes ISOLanguageCodes = ISOLanguageCodes.Instance;
        private static readonly HashSet<string> SegmentExclusion;
        private static readonly HashSet<string> ReleaseGroups;
        private static readonly Dictionary<string, string> CustomLangMappings;
        private static readonly Dictionary<string, SegmentType> KnownSegments;
        private static readonly HashSet<char> RegexReservedChars;
        private static readonly Regex ReleaseGroup = new Regex(@"-([^\. _\[\]-]{3,20})");
        private static readonly Regex PartIdentifier = new Regex(@"[\. -](part ?(\d+)|cd ?(\d+)|disk ?(\d+))[\. -]?", RegexOptions.IgnoreCase);
        private static readonly Regex UrlRegex = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[\-;:&=\+\$,\w]+@)?[A-Za-z0-9\.\-]+|(?:www\.|[\-;:&=\+\$,\w]+@)[A-Za-z0-9\.\-]+)((?:\/[\+~%\/\.\w\-_]*)?\??(?:[\-\+=&;%@\.\w_]*)#?(?:[\.\!\/\\\w]*))?)");
        private static TextInfo _textInfo;

        private readonly string _fileName;
        private readonly Regex _titleAndReleaseYear;
        private List<char> _delimiters;
        private readonly List<string> _detectedSegments;
        private bool _releaseGroupFound;
        private bool _releaseYearFound;
        private bool _titleFound;
        private readonly FileNameInfo _fileNameInfo;

        static FileNameParser() {
            RegexReservedChars = new HashSet<char> {'.', '^', '$', '*', '+', '?', '(', ')', '[', ']', '\\', '|', '}', '{', '-'};

            CustomLangMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"JAP", "jpn"},
                {"srbski", "srp"},
                {"SER", "srp"},
                {"hrvatski", "hrv"},
                {"SLO", "slv"},
                {"CRO", "hrv"}
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

                {"INTERNAL", SegmentType.Special},
                {"PROPER", SegmentType.Special},
                {"LIMITED", SegmentType.Special},
                {"RECODE", SegmentType.Special},
                {"REPACK", SegmentType.Special},

                #endregion

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
                {"STV", SegmentType.ContentType},

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

            _fileNameInfo = new FileNameInfo(_fileName.SplitWithoutEmptyEntries(_delimiters));
            _detectedSegments = new List<string>(_fileNameInfo.UndetectedSegments.Count);
        }

        public ICollection<char> Delimiters {
            get { return _delimiters; }
            set { _delimiters = value.ToList(); }
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

        public Task<FileNameInfo> ParseAsync() {
            return Task.Run(() => Parse());
        }

        public FileNameInfo Parse() {
            if (TryGetTitleAndReleaseYear()) {
                RemoveKnownSegmentsFromTitle();
            }
            else {
                int titleEndIndex = _fileName.IndexOfAny(new[] {'-', ')', '(', '[', ']', '{', '}'});
                if (titleEndIndex > 0) {
                    _titleFound = true;
                    string title = _fileName.Substring(0, titleEndIndex)
                                            .Replace('.', ' ')
                                            .Replace('_', '_');


                    _fileNameInfo.Title = title.Trim();
                    RemoveKnownSegmentsFromTitle(false);
                }
            }

            _releaseGroupFound = CheckAddReleaseGroup();
            CheckAddPartIdentifier();

            //Console.WriteLine("Segments:");

            foreach (string segment in _fileNameInfo.UndetectedSegments) {
                CheckSegmentType(segment);
            }

            ApplyFixes();

            //remove detected segments
            foreach (string segment in _detectedSegments) {
                _fileNameInfo.UndetectedSegments.Remove(segment);
            }

            if (!_titleFound) {
                string title = GetStringBeforeFirstDetectedSegment();
                if (!string.IsNullOrEmpty(title)) {
                    _fileNameInfo.Title = title.Replace('.', ' ')
                                         .Replace('_', ' ')
                                         .Trim();

                    _titleFound = true;
                }
                else {
                    _fileNameInfo.Title = string.Join(" ", _fileNameInfo.UndetectedSegments);
                }
            }

            _fileNameInfo.Title = _textInfo.ToTitleCase(_fileNameInfo.Title);

            return _fileNameInfo;
            //Console.WriteLine();
        }

        private string GetStringBeforeFirstDetectedSegment() {
            if (_detectedSegments.Count == 0) {
                return null;
            }

            int minIndex = _detectedSegments.Min(segment => {
                int idx = _fileName.IndexOf(segment, StringComparison.Ordinal);
                return (idx < 0)
                    ? _fileName.Length
                    : idx;
            });

            return (minIndex != _fileName.Length)
                ? _fileName.Substring(0, minIndex)
                : null;
        }

        private void RemoveKnownSegmentsFromTitle(bool releaseYearFound = true) {
            int firstSegmentIdx = -1;
            foreach (string titleSegment in _fileNameInfo.Title.SplitWithoutEmptyEntries(_delimiters)) {
                SegmentType type;
                if (KnownSegments.TryGetValue(titleSegment, out type)) {
                    AddSegmentType(titleSegment, type);

                    if (firstSegmentIdx == -1) {
                        firstSegmentIdx = _fileNameInfo.Title.IndexOf(titleSegment, StringComparison.Ordinal);
                    }

                    _fileNameInfo.Title = _fileNameInfo.Title.Replace(titleSegment, "").Trim();
                }
                else if (CheckLanguage(titleSegment) != LanguageCheck.NotALangauge) {
                    int langIdx = _fileNameInfo.Title.IndexOf(titleSegment, StringComparison.Ordinal) - 1;

                    if (langIdx > 0) {
                        char prevLang = _fileNameInfo.Title[langIdx];

                        //only if language identifier is in brackets (to avoid collisions with the title)
                        if ((prevLang == '(' || prevLang == '[' || prevLang == '{') && CheckAndAddLanguage(titleSegment)) {
                            _fileNameInfo.Title = _fileNameInfo.Title.Remove(langIdx, titleSegment.Length + 2).Trim();
                        }
                    }
                }

                _fileNameInfo.UndetectedSegments.Remove(titleSegment);
            }

            //remove everything after the first known segment
            //and preserve the title only
            _fileNameInfo.Title = (firstSegmentIdx != -1 && firstSegmentIdx < _fileNameInfo.Title.Length)
                ? _fileNameInfo.Title.Remove(firstSegmentIdx).Trim()
                : _fileNameInfo.Title.Trim();

            if (releaseYearFound) {
                _fileNameInfo.UndetectedSegments.Remove(_fileNameInfo.ReleaseYear.ToString("yyyy"));
            }
        }

        private bool TryGetTitleAndReleaseYear() {
            Match match = _titleAndReleaseYear.Match(_fileName);
            if (match.Success) {
                _titleFound = true;
                _fileNameInfo.Title = match.Groups[1].Value.Replace('.', ' ').Replace('_', ' ');

                DateTime releaseYear;
                if (DateTime.TryParseExact(match.Groups[2].Value.Trim(), "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseYear)) {
                    

                    _releaseYearFound = true;
                    _fileNameInfo.ReleaseYear = releaseYear;

                    _detectedSegments.Add(match.Groups[2].Value);
                }
                else {
                    Console.Error.WriteLine("Failed parsing date: "+match.Groups[2].Value.Trim());
                }
                return true;
            }
            return false;
        }

        private void ApplyFixes() {
            //check for any misdetected stuff
            //like having subs language be just "subs" while we detected a standalone lang

            //handle misdetected lanugages
            //HandleMisdetectedLanguages();
        }

        //private void HandleMisdetectedLanguages() {
        //    if (_fileNameInfo.DetectedSegments.ContainsKey(SegmentType.Language)) {
        //        List<string> langs = _fileNameInfo.DetectedSegments[SegmentType.Language];
        //        int langCount = langs.Count;
        //        if (langCount > 1) {
        //            int itIdx = langs.FindIndex(lang => lang.Equals("it", StringComparison.OrdinalIgnoreCase));
        //            int myIdx = langs.FindIndex(lang => lang.Equals("my", StringComparison.OrdinalIgnoreCase));

        //            if (langCount == 2) {
        //                //if IT was detectd but MY not then remove IT
        //                if (itIdx != -1 && myIdx == -1) {
        //                    langs.Remove(langs[itIdx]);
        //                }
        //                    //if MY was detected but IT not then remove MY
        //                else if (myIdx != -1 && itIdx == -1) {
        //                    langs.Remove(langs[myIdx]);
        //                }
        //                    //SPECIAL CASE: bot MY and IT detected
        //                    //return the one with the bigger index
        //                    //as it was further in to the file name and
        //                    //has a higher probability.
        //                else if (myIdx != -1 && itIdx != -1) {
        //                    if (itIdx > myIdx) {
        //                        langs.Remove(langs[itIdx]);
        //                    }
        //                    else {
        //                        langs.Remove(langs[myIdx]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        #region Segment Type Checks

        private void CheckSegmentType(string segment) {
            //Console.WriteLine("\t" + segment);

            if (!_releaseYearFound && CheckAddReleaseYear(segment)) {
                _releaseYearFound = true;
                if (!_titleFound) {
                    _titleFound = true;

                    int yearStart = _fileName.IndexOf(segment, StringComparison.Ordinal) - 1;

                    _fileNameInfo.Title = _fileName.Substring(0, yearStart)
                                             .Replace('.', ' ')
                                             .Replace('_', ' ')
                                             .Trim();
                    return;
                }
            }

            SegmentType type;
            if (KnownSegments.TryGetValue(segment, out type)) {
                AddSegmentType(segment, type);
                return;
            }

            if (CheckAndAddSubtitleLanguage(segment)) {
                return;
            }

            if (CheckAndAddLanguage(segment)) {
                return;
            }

            if (!_releaseGroupFound) {
                CheckAddReleaseGroup(segment);
            }
        }

        private void AddSegmentType(string segment, SegmentType type) {
            switch (type) {
                case SegmentType.DVDRegion:
                    DVDRegion region;
                    Enum.TryParse(segment, true, out region);
                    _fileNameInfo.DVDRegion = region;
                    break;
                case SegmentType.ContentType:
                    break;
                case SegmentType.Genre:
                    _fileNameInfo.Genre = segment;
                    break;
                case SegmentType.Edithion:
                    _fileNameInfo.Edithion = segment;
                    break;
                case SegmentType.VideoSource:
                    _fileNameInfo.VideoSource = segment;
                    break;
                case SegmentType.VideoQuality:
                    _fileNameInfo.VideoQuality = segment;
                    break;
                case SegmentType.VideoCodec:
                    _fileNameInfo.VideoCodec = segment;
                    break;
                case SegmentType.AudioSource:
                    _fileNameInfo.AudioSource = segment;
                    break;
                case SegmentType.AudioCodec:
                    _fileNameInfo.AudioCodec = segment;
                    break;
                case SegmentType.AudioQuality:
                    _fileNameInfo.AudioQuality = segment;
                    break;
                case SegmentType.Special:
                    _fileNameInfo.Specials.Add(segment);
                    break;
            }
            _detectedSegments.Add(segment);
        }

        private void CheckAddPartIdentifier() {
            Match match = PartIdentifier.Match(_fileName);
            if (!match.Success) {
                return;
            }

            string partIdentifier = match.Groups[1].Value;
            string partNumStr = match.Groups[3].Value;

            int partNum;
            if (int.TryParse(partNumStr, out partNum)) {
                _fileNameInfo.Part = partNum;
                _fileNameInfo.PartType = partIdentifier.Replace(partNumStr, "");
            }
            _detectedSegments.Add(partIdentifier);

            _fileNameInfo.UndetectedSegments.Remove(partIdentifier);
        }

        private bool CheckAndAddSubtitleLanguage(string segment) {
            string lang;
            if (segment.EndsWith("subs", StringComparison.OrdinalIgnoreCase)) {
                int idx = segment.LastIndexOf("subs", StringComparison.OrdinalIgnoreCase);
                lang = segment.Remove(idx);
            }
            else if (segment.EndsWith("sub", StringComparison.OrdinalIgnoreCase)) {
                int idx = segment.LastIndexOf("sub", StringComparison.OrdinalIgnoreCase);
                lang = segment.Remove(idx);
            }
            else {
                return false;
            }

            _fileNameInfo.HasSubtitles = true;
            CheckAndAddLanguage(lang, true);

            _detectedSegments.Add(segment);
            return true;
        }

        private bool CheckAndAddLanguage(string segment, bool subtitleLang = false) {
            LanguageCheck checkLanguage = CheckLanguage(segment);
            if (checkLanguage == LanguageCheck.NotALangauge) {
                return false;
            }

            ISOLanguageCode isoCode = null;
            switch (checkLanguage) {
                case LanguageCheck.ISOCode:
                    isoCode = ISOLanguageCodes.GetByISOCode(segment);
                    break;
                case LanguageCheck.ISOEnglishName:
                    isoCode = ISOLanguageCodes.GetByEnglishName(segment);
                    break;
                case LanguageCheck.Custom:
                    isoCode = ISOLanguageCodes.GetByISOCode(CustomLangMappings[segment]);
                    break;
            }

            if (subtitleLang) {
                _fileNameInfo.SubtitleLanguage = isoCode;
            }
            else {
                _fileNameInfo.Language = isoCode;
            }
            return true;
        }

        private LanguageCheck CheckLanguage(string segment) {
            if (segment.Length < 2 || SegmentExclusion.Contains(segment)) {
                return LanguageCheck.NotALangauge;
            }

            if (CustomLangMappings.ContainsKey(segment)) {
                return LanguageCheck.Custom;
            }

            //ISO codes are 2 or 3 letters long
            if (segment.Length < 4 && ISOLanguageCodes.IsAnISOCode(segment)) {
                return LanguageCheck.ISOCode;
            }

            if (ISOLanguageCodes.IsAnISOEnglishLanguageName(segment)) {
                return LanguageCheck.ISOEnglishName;
            }

            return LanguageCheck.NotALangauge;
        }

        private bool CheckAddReleaseYear(string segment) {
            if (IsReleaseYear(segment)) {
                DateTime releaseYear;
                if (DateTime.TryParse(segment, out releaseYear)) {
                    _fileNameInfo.ReleaseYear = releaseYear;

                    _detectedSegments.Add(segment);
                    return true;
                }
            }
            return false;
        }

        private bool CheckAddReleaseGroup() {
            Match match = ReleaseGroup.Match(_fileName);
            if (match.Success) {
                string releaseGroup = match.Groups[1].Value;

                if (releaseGroup.Length > 2 && !SegmentExclusion.Contains(releaseGroup)) {
                    _fileNameInfo.ReleaseGroup = releaseGroup;
                    _fileNameInfo.UndetectedSegments.Remove(releaseGroup);

                    _detectedSegments.Add(releaseGroup);
                    return true;
                }
            }
            return false;
        }

        private void CheckAddReleaseGroup(string segment) {
            //if not detected before, check if it's a Release Group name
            if (segment.Length > 2 && ReleaseGroups.Contains(segment)) {
                if (_fileNameInfo.ReleaseGroup != null) {
                    int idxPrev = _fileName.IndexOf(_fileNameInfo.ReleaseGroup, StringComparison.Ordinal);
                    int idxCurr = _fileName.IndexOf(segment, StringComparison.Ordinal);

                    //take the one furthest in to the string 
                    //(release group is usually specified last)
                    if (idxPrev > idxCurr) {
                        return;
                    }
                }
                _fileNameInfo.ReleaseGroup = segment;
                _detectedSegments.Add(segment);
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

        #endregion
    }
}