using Frost.SharpMediaInfo.Output;

namespace Frost.SharpMediaInfo.Options {
    public class SSH {
        private readonly MediaFile _mf;

        internal SSH(MediaFile mf) {
            _mf = mf;
        }
        
        public string KnownHostsFileName { set { _mf.Option("ssh_knownhostsfilename", value); } }

        public string PublicKeyFileName { set { _mf.Option("ssh_publickeyfilename", value); } }
        
        public string PrivateKeyFileName { set { _mf.Option("ssh_privatekeyfilename", value); } }

        public bool IgnoreSecurity { set { _mf.Option("ignoresecurity", value ? "" : "0");} }
    }
}