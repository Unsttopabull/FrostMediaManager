using System.Security.Cryptography;
using System.Text;
using CookComputing.XmlRpc;
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

            string md5Hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(password)).ToHexString();
            password = _sha256.ComputeHash(Encoding.UTF8.GetBytes(md5Hash + _rpc.Nonce)).ToHexString();


            return _rpc.Proxy.Authenticate(_rpc.Token, username, password);
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
