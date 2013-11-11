namespace Frost.SharpMediaInfo.Options {
    public class MpegTS {
        private readonly MediaInfo _mi;

        internal MpegTS(MediaInfo mi) {
            _mi = mi;
        }

        public string MaximumOffset {
            get { return _mi.Option("mpegts_maximumoffset_get"); }
            set { _mi.Option("mpegts_maximumoffset", value); }
        }

        public string MaximumScanDuration {
            get { return _mi.Option("mpegts_maximumscanduration_get"); }
            set { _mi.Option("mpegts_maximumscanduration", value); }
        }

        public double VbrDetectionDelta {
            get {
                double delta;
                double.TryParse(_mi.Option("mpegts_vbrdetection_delta_get"), out delta);

                return delta;
            }
            set { _mi.Option("mpegts_vbrdetection_delta", value.ToString()); }
        }

        public long VbrDetectionOccurences {
            get {
                long numOccurences;
                long.TryParse(_mi.Option("mpegts_vbrdetection_occurences_get"), out numOccurences);
                return numOccurences;
            }
            set { _mi.Option("mpegts_vbrdetection_occurences", value.ToString()); }
        }

        public bool VbrDetectionGiveUp {
            get { return _mi.Option("mpegts_vbrdetection_giveup_get") == "1"; }
            set {
                _mi.Option("mpegts_vbrdetection_giveup", value ? "1" : "0");
            }
        }
    
        public bool ForceStreamDisplay {
            get { return _mi.Option("mpegts_forcestreamdisplay_get") == "1"; }
            set { _mi.Option("mpegts_forcestreamdisplay", value ? "1" : "0"); }
        }
    }
}