namespace SharpMediaInfo.Options {
    public class SSH {
        private readonly MediaInfo _mi;

        internal SSH(MediaInfo mi) {
            _mi = mi;
        }
        
        public string KnownHostsFileName {
            set { _mi.Option("ssh_knownhostsfilename", value); }
        }

        public string PublicKeyFileName {
            set { _mi.Option("ssh_publickeyfilename", value); }
        }
        
        public string PrivateKeyFileName {
            set { _mi.Option("ssh_privatekeyfilename", value); }
        }

        public bool IgnoreSecurity {
            set { _mi.Option("ignoresecurity", value ? "" : "0");}
        }
    }
}