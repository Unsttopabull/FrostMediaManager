using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Country {

    internal class RemoveCountryMessage {

        public RemoveCountryMessage(ICountry country) {
            Country = country;
        }

        public ICountry Country { get; set; }
    }

}