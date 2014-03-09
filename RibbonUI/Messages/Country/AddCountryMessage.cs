using Frost.Common.Models;

namespace RibbonUI.Messages.Country {

    internal class AddCountryMessage {

        public AddCountryMessage(ICountry country) {
            Country = country;
        }

        public ICountry Country { get; set; }
    }

}