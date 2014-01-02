using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {

    public enum StatusCode {
        Ok = 200,
        InvalidCredentials = 300,
        NoAuthorisation = 301,
        InvalidSession = 302,
        MovieNotFound = 400,
        InvalidFormat = 401,
        InvalidLanguage = 402,
        InvalidHash = 403,
        InvalidArchive = 404,
    }

    public class StatusInfo {

        [XmlRpcMember("status")]
        [XmlRpcEnumMapping(EnumMapping.Number)]
        public StatusCode Status;
    }

}