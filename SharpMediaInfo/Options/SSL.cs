namespace Frost.SharpMediaInfo.Options {
    public class SSL {
        private readonly MediaInfo _mi;

        internal SSL(MediaInfo mi) {
            _mi = mi;
        }

        public string CertificateFileName { set { _mi.Option("ssl_certificatefilename", value); } }

        public string CertificateFormat { set { _mi.Option("ssl_certificateFormat", value); } }

        public string PrivateKeyFilename { set { _mi.Option("ssl_privatekeyfilename", value); } }

        public string PrivateKeyFormat { set { _mi.Option("ssl_privatekeyformat", value); } }

        public string CertificateAuthorityFilename { set { _mi.Option("ssl_certificateauthorityfilename", value); } }

        public string CertificateAuthorityPath { set { _mi.Option("ssl_certificateauthoritypath", value); } }

        public string CertificateRevocationListFilename { set { _mi.Option("ssl_certificaterevocationlistfilename", value); } }

        public string IgnoreSecurity { set { _mi.Option("ssl_ignoresecurity", value); } }
    }
}