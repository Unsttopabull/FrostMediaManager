using SharpTraktTvAPI.Models.Account.SettingsModels;

namespace SharpTraktTvAPI.Models.Account {

    public class SettingsResponse {
        public string status { get; set; }
        public string message { get; set; }
        public Profile profile { get; set; }
        public SettingsModels.AccountSettings account { get; set; }
        public Viewing viewing { get; set; }
        public Connections connections { get; set; }
        public SharingText sharing_text { get; set; }
    }

}