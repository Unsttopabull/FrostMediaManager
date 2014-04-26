using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {

    public class StatusInfo {

        [XmlRpcMember("status")]
        [XmlRpcEnumMapping(EnumMapping.Number)]
        public StatusCode Status;
    }

}