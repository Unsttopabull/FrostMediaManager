using System.Security.Cryptography;
using System.Text;
using Frost.PodnapisiNET.Models;

namespace Frost.PodnapisiNET {
    public class Session {
        private readonly PodnapisiNetClient _rpc;
        private static MD5 _md5;
        private static SHA256 _sha256;

        public Session(PodnapisiNetClient rpc) {
            _rpc = rpc;
            _md5 = MD5.Create();
            _sha256 = SHA256.Create();
        }

        public LogInInfo Initiate(string useragent) {
            LogInInfo logIn = _rpc.Proxy.Initiate(useragent);
            _rpc.Token = logIn.Session;
            _rpc.Nonce = logIn.Nonce;

            return logIn;
        }

        public FilterInfo Authenticate(string username, string password) {
            _rpc.CheckToken();

            password = HashPassword(password);

            return _rpc.Proxy.Authenticate(_rpc.Token, username, password);
        }

        public string HashPassword(string password, string nonce = null) {
            string md5Hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(password)).ToHexString();

            nonce = nonce ?? _rpc.Nonce;
            string join = md5Hash + nonce;
            byte[] b = Encoding.UTF8.GetBytes(join);


            password = _sha256.ComputeHash(b).ToHexString();
            return password;
        }

        public StatusInfo LogOut() {
            _rpc.CheckToken();

            return _rpc.Proxy.Terminate(_rpc.Token);
        }

        public FilterInfo AuthenticateAnonymous() {
            return _rpc.Proxy.Authenticate(_rpc.Token, "", "");
        }
    }
}
