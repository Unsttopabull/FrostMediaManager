using System;
using System.Globalization;

namespace SharpMediaInfo.Options {
    public class Settings {
        private readonly MediaInfo _mi;

        internal Settings(MediaInfo mi) {
            _mi = mi;

            MpegTS = new MpegTS(_mi);
            SSH = new SSH(_mi);
            SSL = new SSL(_mi);
        }

        public SSH SSH { get; private set; }

        public SSL SSL { get; private set; }

        public MpegTS MpegTS { get; private set; }

        public BlockMethod BlockMethod {
            get {
                return _mi.Option("blockmethod_get") == "1"
                               ? BlockMethod.AfterLocalInfo
                               : BlockMethod.Immediately;
            }
            set {
                _mi.Option("blockmethod", value == BlockMethod.Immediately ? "" : "1");
            }
        }

        public bool ShowAllInfo {
            get {
                return _mi.Option("Complete_Get") == "1";
            }
            set { _mi.Option("complete", value ? "1" : ""); }
        }

        public bool ParseUnknownExtensions {
            get {
                return _mi.Option("ParseUnknownExtensions_Get") == "1";
            }
            set { _mi.Option("ParseUnknownExtensions", value ? "1" : ""); }
        }

        public bool ReadByHuman {
            get { return _mi.Option("readbyhuman_get") == "1"; }
            set {
                _mi.Option("readbyhuman", value ? "1" : "0");
            }
        }

        public bool LegacyStreamDisplay {
            get { return _mi.Option("LegacyStreamDisplay_get") == "1"; }
            set {
                _mi.Option("LegacyStreamDisplay", value ? "1" : "0");
            }
        }

        public bool SkipBinaryData {
            get { return _mi.Option("SkipBinaryData_get") == "1"; }
            set {
                _mi.Option("SkipBinaryData", value ? "1" : "0");
            }            
        }

        public float ParseSpeed {
            get {
                float parseSpeed;
                float.TryParse(_mi.Option("parsespeed_get"), out parseSpeed);

                return parseSpeed;
            }
            set { _mi.Option("parsespeed", value.ToString(CultureInfo.InvariantCulture)); }
        }

        public float Verbosity {
            get {
                float parseSpeed;
                float.TryParse(_mi.Option("verbosity_get"), out parseSpeed);

                return parseSpeed;
            }
            set { _mi.Option("verbosity", value.ToString(CultureInfo.InvariantCulture)); }
        }

        public string LineSeparator {
            get { return _mi.Option("LineSeparator_get"); }
            set { _mi.Option("LineSeparator", value); }
        }

        public string Version {
            get { return _mi.Option("Version_Get"); }
            set { _mi.Option("Version", value); }
        }

        public string ColumnSeparator {
            get { return _mi.Option("ColumnSeparator_Get"); }
            set { _mi.Option("ColumnSeparator", value); }
        }

        public string TagSeparator {
            get { return _mi.Option("TagSeparator_Get"); }
            set { _mi.Option("TagSeparator", value); }
        }

        public string Quote {
            get { return _mi.Option("Quote_Get"); }
            set { _mi.Option("Quote", value); }
        }

        public string DecimalPoint {
            get { return _mi.Option("decimalpoint_get"); }
            set { _mi.Option("decimalpoint", value); }
        }

        public string ThousandsPoint {
            get { return _mi.Option("thousandspoint_get"); }
            set { _mi.Option("thousandspoint", value); }
        }

        public string StreamMax {
            get { return _mi.Option("streammax_get"); }
            set { _mi.Option("streammax", value); }
        }

        public string Language {
            get { return _mi.Option("Language_Get"); }
            set { _mi.Option("Language", value); }
        }

        public string Inform {
            get { return _mi.Option("inform_get");}
            set { _mi.Option("inform", value); }
        }

        public InformPreset InformPreset {
            set {
                string strType;
                switch (value) {
                    case InformPreset.HTML:
                    case InformPreset.XML:
                    case InformPreset.PBCore:
                        strType = value.ToString();
                        break;
                    case InformPreset.ReVTMD:
                        strType = "reVTMD";
                        break;
                    case InformPreset.Mpeg7:
                        strType = "MPEG-7";
                        break;
                    default:
                        strType = "";
                        break;
                }

                _mi.Option("inform", strType);
            }
        }

        public string InformReplace {
            get { return _mi.Option("inform_replace_get"); }
            set { _mi.Option("inform_replace", value); }
        }

        public string TraceLevel {
            get { return _mi.Option("trace_level_get"); }
            set { _mi.Option("trace_level", value); }
        }

        public bool TraceTimesectionOnlyFirstOccurrence  {
            get { return _mi.Option("trace_timesection_onlyfirstoccurrence_get") == "1"; }
            set {
                _mi.Option("trace_timesection_onlyfirstoccurrence", value ? "1" : "");
            }
        }

        public TraceFormat TraceFormat {
            get {
                return _mi.Option("trace_format_get") == "CSV"
                               ? TraceFormat.CSV
                               : TraceFormat.Tree;
            }
            set {
                _mi.Option("trace_format", value == TraceFormat.CSV ? "CSV" : "Tree");
            }
        }

        public string DetailsModificator {
            get { return _mi.Option("detailsmodificator_get"); }
            set { _mi.Option("detailsmodificator", value); }
        }

        public string ShowFiles {
            set { _mi.Option("ShowFiles_Set", value); }
        }

        public bool AllowInternetConnection {
            get { return _mi.Option("Internet") == "1"; }
            set {
                _mi.Option("Internet", value ? "1" : "");
            }
        }

        public string CustomMapping {
            set { _mi.Option("custommapping", value); }
        }

        public bool MultipleValues {
            get { return _mi.Option("multiplevalues_get") == "1"; }
            set {
                _mi.Option("multiplevalues", value ? "1" : "");
            }
        }

        public Demux Demux {
            get {
                Demux dm;
                Enum.TryParse(_mi.Option("demux"), true, out dm);
                return dm;
            }
            set { _mi.Option("demux", value.ToString()); }
        }
    }
}