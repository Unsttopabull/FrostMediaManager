﻿using System.Collections.Generic;
using System.Linq;

namespace Frost.Common.Util.ISO {

    /// <summary>Represents a mapping between english country names and their officialy assigned ISO 3166 country codes.</summary>
    public class ISOCountryCodes : ISOCodes<ISOCountryCode> {

        private static readonly ISOCountryCodes _instance = new ISOCountryCodes();

        static ISOCountryCodes() {
            ISOCountryCode[] icc = {
                #region CountryCodes
                new ISOCountryCode("Afghanistan", "AF", "AFG", 004),
                new ISOCountryCode("Åland Islands", "AX", "ALA", 248),
                new ISOCountryCode("Albania", "AL", "ALB", 008),
                new ISOCountryCode("Algeria", "DZ", "DZA", 012),
                new ISOCountryCode("American Samoa", "AS", "ASM", 016),
                new ISOCountryCode("Andorra", "AD", "AND", 020),
                new ISOCountryCode("Angola", "AO", "AGO", 024),
                new ISOCountryCode("Anguilla", "AI", "AIA", 660),
                new ISOCountryCode("Antarctica", "AQ", "ATA", 010),
                new ISOCountryCode("Antigua and Barbuda", "AG", "ATG", 028),
                new ISOCountryCode("Argentina", "AR", "ARG", 032),
                new ISOCountryCode("Armenia", "AM", "ARM", 051),
                new ISOCountryCode("Aruba", "AW", "ABW", 533),
                new ISOCountryCode("Australia", "AU", "AUS", 036),
                new ISOCountryCode("Austria", "AT", "AUT", 040),
                new ISOCountryCode("Azerbaijan", "AZ", "AZE", 031),
                new ISOCountryCode("Bahamas", "BS", "BHS", 044),
                new ISOCountryCode("Bahrain", "BH", "BHR", 048),
                new ISOCountryCode("Bangladesh", "BD", "BGD", 050),
                new ISOCountryCode("Barbados", "BB", "BRB", 052),
                new ISOCountryCode("Belarus", "BY", "BLR", 112),
                new ISOCountryCode("Belgium", "BE", "BEL", 056),
                new ISOCountryCode("Belize", "BZ", "BLZ", 084),
                new ISOCountryCode("Benin", "BJ", "BEN", 204),
                new ISOCountryCode("Bermuda", "BM", "BMU", 060),
                new ISOCountryCode("Bhutan", "BT", "BTN", 064),
                new ISOCountryCode("Bolivia", "BO", "BOL", 068),
                new ISOCountryCode("Bonaire, Sint Eustatius and Saba", "BQ", "BES", 535),
                new ISOCountryCode("Bosnia and Herzegovina", "BA", "BIH", 070),
                new ISOCountryCode("Botswana", "BW", "BWA", 072),
                new ISOCountryCode("Bouvet Island", "BV", "BVT", 074),
                new ISOCountryCode("Brazil", "BR", "BRA", 076),
                new ISOCountryCode("British Indian Ocean Territory", "IO", "IOT", 086),
                new ISOCountryCode("Brunei Darussalam", "BN", "BRN", 096),
                new ISOCountryCode("Bulgaria", "BG", "BGR", 100),
                new ISOCountryCode("Burkina Faso", "BF", "BFA", 854),
                new ISOCountryCode("Burundi", "BI", "BDI", 108),
                new ISOCountryCode("Cambodia", "KH", "KHM", 116),
                new ISOCountryCode("Cameroon", "CM", "CMR", 120),
                new ISOCountryCode("Canada", "CA", "CAN", 124),
                new ISOCountryCode("Cape Verde", "CV", "CPV", 132),
                new ISOCountryCode("Cayman Islands", "KY", "CYM", 136),
                new ISOCountryCode("Central African Republic", "CF", "CAF", 140),
                new ISOCountryCode("Chad", "TD", "TCD", 148),
                new ISOCountryCode("Chile", "CL", "CHL", 152),
                new ISOCountryCode("China", "CN", "CHN", 156),
                new ISOCountryCode("Christmas Island", "CX", "CXR", 162),
                new ISOCountryCode("Cocos (Keeling) Islands", "CC", "CCK", 166),
                new ISOCountryCode("Colombia", "CO", "COL", 170),
                new ISOCountryCode("Comoros", "KM", "COM", 174),
                new ISOCountryCode("Congo", "CG", "COG", 178),
                new ISOCountryCode("Congo (the Democratic Republic of the)", "CD", "COD", 180),
                new ISOCountryCode("Cook Islands", "CK", "COK", 184),
                new ISOCountryCode("Costa Rica", "CR", "CRI", 188),
                new ISOCountryCode("Côte d'Ivoire", "CI", "CIV", 384),
                new ISOCountryCode("Croatia", "HR", "HRV", 191),
                new ISOCountryCode("Cuba", "CU", "CUB", 192),
                new ISOCountryCode("Curaçao", "CW", "CUW", 531),
                new ISOCountryCode("Cyprus", "CY", "CYP", 196),
                new ISOCountryCode("Czech Republic", "CZ", "CZE", 203),
                new ISOCountryCode("Denmark", "DK", "DNK", 208),
                new ISOCountryCode("Djibouti", "DJ", "DJI", 262),
                new ISOCountryCode("Dominica", "DM", "DMA", 212),
                new ISOCountryCode("Dominican Republic", "DO", "DOM", 214),
                new ISOCountryCode("Ecuador", "EC", "ECU", 218),
                new ISOCountryCode("Egypt", "EG", "EGY", 818),
                new ISOCountryCode("El Salvador", "SV", "SLV", 222),
                new ISOCountryCode("Equatorial Guinea", "GQ", "GNQ", 226),
                new ISOCountryCode("Eritrea", "ER", "ERI", 232),
                new ISOCountryCode("Estonia", "EE", "EST", 233),
                new ISOCountryCode("Ethiopia", "ET", "ETH", 231),
                new ISOCountryCode("Falkland Islands [Malvinas]", "FK", "FLK", 238),
                new ISOCountryCode("Faroe Islands", "FO", "FRO", 234),
                new ISOCountryCode("Fiji", "FJ", "FJI", 242),
                new ISOCountryCode("Finland", "FI", "FIN", 246),
                new ISOCountryCode("France", "FR", "FRA", 250),
                new ISOCountryCode("French Guiana", "GF", "GUF", 254),
                new ISOCountryCode("French Polynesia", "PF", "PYF", 258),
                new ISOCountryCode("French Southern Territories", "TF", "ATF", 260),
                new ISOCountryCode("Gabon", "GA", "GAB", 266),
                new ISOCountryCode("Gambia", "GM", "GMB", 270),
                new ISOCountryCode("Georgia", "GE", "GEO", 268),
                new ISOCountryCode("Germany", "DE", "DEU", 276),
                new ISOCountryCode("Ghana", "GH", "GHA", 288),
                new ISOCountryCode("Gibraltar", "GI", "GIB", 292),
                new ISOCountryCode("Greece", "GR", "GRC", 300),
                new ISOCountryCode("Greenland", "GL", "GRL", 304),
                new ISOCountryCode("Grenada", "GD", "GRD", 308),
                new ISOCountryCode("Guadeloupe", "GP", "GLP", 312),
                new ISOCountryCode("Guam", "GU", "GUM", 316),
                new ISOCountryCode("Guatemala", "GT", "GTM", 320),
                new ISOCountryCode("Guernsey", "GG", "GGY", 831),
                new ISOCountryCode("Guinea", "GN", "GIN", 324),
                new ISOCountryCode("Guinea-Bissau", "GW", "GNB", 624),
                new ISOCountryCode("Guyana", "GY", "GUY", 328),
                new ISOCountryCode("Haiti", "HT", "HTI", 332),
                new ISOCountryCode("Heard Island and McDonald Islands", "HM", "HMD", 334),
                new ISOCountryCode("Holy See [Vatican City State]", "VA", "VAT", 336),
                new ISOCountryCode("Honduras", "HN", "HND", 340),
                new ISOCountryCode("Hong Kong", "HK", "HKG", 344),
                new ISOCountryCode("Hungary", "HU", "HUN", 348),
                new ISOCountryCode("Iceland", "IS", "ISL", 352),
                new ISOCountryCode("India", "IN", "IND", 356),
                new ISOCountryCode("Indonesia", "ID", "IDN", 360),
                new ISOCountryCode("Iran", "IR", "IRN", 364),
                new ISOCountryCode("Iraq", "IQ", "IRQ", 368),
                new ISOCountryCode("Ireland", "IE", "IRL", 372),
                new ISOCountryCode("Isle of Man", "IM", "IMN", 833),
                new ISOCountryCode("Israel", "IL", "ISR", 376),
                new ISOCountryCode("Italy", "IT", "ITA", 380),
                new ISOCountryCode("Jamaica", "JM", "JAM", 388),
                new ISOCountryCode("Japan", "JP", "JPN", 392),
                new ISOCountryCode("Jersey", "JE", "JEY", 832),
                new ISOCountryCode("Jordan", "JO", "JOR", 400),
                new ISOCountryCode("Kazakhstan", "KZ", "KAZ", 398),
                new ISOCountryCode("Kenya", "KE", "KEN", 404),
                new ISOCountryCode("Kiribati", "KI", "KIR", 296),
                new ISOCountryCode("Korea (the Democratic People's Republic of)", "KP", "PRK", 408),
                new ISOCountryCode("South Korea", "KR", "KOR", 410),
                new ISOCountryCode("Kuwait", "KW", "KWT", 414),
                new ISOCountryCode("Kyrgyzstan", "KG", "KGZ", 417),
                new ISOCountryCode("Lao People's Democratic Republic", "LA", "LAO", 418),
                new ISOCountryCode("Latvia", "LV", "LVA", 428),
                new ISOCountryCode("Lebanon", "LB", "LBN", 422),
                new ISOCountryCode("Lesotho", "LS", "LSO", 426),
                new ISOCountryCode("Liberia", "LR", "LBR", 430),
                new ISOCountryCode("Libya", "LY", "LBY", 434),
                new ISOCountryCode("Liechtenstein", "LI", "LIE", 438),
                new ISOCountryCode("Lithuania", "LT", "LTU", 440),
                new ISOCountryCode("Luxembourg", "LU", "LUX", 442),
                new ISOCountryCode("Macao", "MO", "MAC", 446),
                new ISOCountryCode("Macedonia", "MK", "MKD", 807),
                new ISOCountryCode("Madagascar", "MG", "MDG", 450),
                new ISOCountryCode("Malawi", "MW", "MWI", 454),
                new ISOCountryCode("Malaysia", "MY", "MYS", 458),
                new ISOCountryCode("Maldives", "MV", "MDV", 462),
                new ISOCountryCode("Mali", "ML", "MLI", 466),
                new ISOCountryCode("Malta", "MT", "MLT", 470),
                new ISOCountryCode("Marshall Islands", "MH", "MHL", 584),
                new ISOCountryCode("Martinique", "MQ", "MTQ", 474),
                new ISOCountryCode("Mauritania", "MR", "MRT", 478),
                new ISOCountryCode("Mauritius", "MU", "MUS", 480),
                new ISOCountryCode("Mayotte", "YT", "MYT", 175),
                new ISOCountryCode("Mexico", "MX", "MEX", 484),
                new ISOCountryCode("Micronesia", "FM", "FSM", 583),
                new ISOCountryCode("Moldova", "MD", "MDA", 498),
                new ISOCountryCode("Monaco", "MC", "MCO", 492),
                new ISOCountryCode("Mongolia", "MN", "MNG", 496),
                new ISOCountryCode("Montenegro", "ME", "MNE", 499),
                new ISOCountryCode("Montserrat", "MS", "MSR", 500),
                new ISOCountryCode("Morocco", "MA", "MAR", 504),
                new ISOCountryCode("Mozambique", "MZ", "MOZ", 508),
                new ISOCountryCode("Myanmar", "MM", "MMR", 104),
                new ISOCountryCode("Namibia", "NA", "NAM", 516),
                new ISOCountryCode("Nauru", "NR", "NRU", 520),
                new ISOCountryCode("Nepal", "NP", "NPL", 524),
                new ISOCountryCode("Netherlands", "NL", "NLD", 528),
                new ISOCountryCode("New Caledonia", "NC", "NCL", 540),
                new ISOCountryCode("New Zealand", "NZ", "NZL", 554),
                new ISOCountryCode("Nicaragua", "NI", "NIC", 558),
                new ISOCountryCode("Niger", "NE", "NER", 562),
                new ISOCountryCode("Nigeria", "NG", "NGA", 566),
                new ISOCountryCode("Niue", "NU", "NIU", 570),
                new ISOCountryCode("Norfolk Island", "NF", "NFK", 574),
                new ISOCountryCode("Northern Mariana Islands", "MP", "MNP", 580),
                new ISOCountryCode("Norway", "NO", "NOR", 578),
                new ISOCountryCode("Oman", "OM", "OMN", 512),
                new ISOCountryCode("Pakistan", "PK", "PAK", 586),
                new ISOCountryCode("Palau", "PW", "PLW", 585),
                new ISOCountryCode("Palestine, State of", "PS", "PSE", 275),
                new ISOCountryCode("Panama", "PA", "PAN", 591),
                new ISOCountryCode("Papua New Guinea", "PG", "PNG", 598),
                new ISOCountryCode("Paraguay", "PY", "PRY", 600),
                new ISOCountryCode("Peru", "PE", "PER", 604),
                new ISOCountryCode("Philippines", "PH", "PHL", 608),
                new ISOCountryCode("Pitcairn", "PN", "PCN", 612),
                new ISOCountryCode("Poland", "PL", "POL", 616),
                new ISOCountryCode("Portugal", "PT", "PRT", 620),
                new ISOCountryCode("Puerto Rico", "PR", "PRI", 630),
                new ISOCountryCode("Qatar", "QA", "QAT", 634),
                new ISOCountryCode("Réunion", "RE", "REU", 638),
                new ISOCountryCode("Romania", "RO", "ROU", 642),
                new ISOCountryCode("Russian Federation", "RU", "RUS", 643),
                new ISOCountryCode("Rwanda", "RW", "RWA", 646),
                new ISOCountryCode("Saint Barthélemy", "BL", "BLM", 652),
                new ISOCountryCode("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", 654),
                new ISOCountryCode("Saint Kitts and Nevis", "KN", "KNA", 659),
                new ISOCountryCode("Saint Lucia", "LC", "LCA", 662),
                new ISOCountryCode("Saint Martin (French part)", "MF", "MAF", 663),
                new ISOCountryCode("Saint Pierre and Miquelon", "PM", "SPM", 666),
                new ISOCountryCode("Saint Vincent and the Grenadines", "VC", "VCT", 670),
                new ISOCountryCode("Samoa", "WS", "WSM", 882),
                new ISOCountryCode("San Marino", "SM", "SMR", 674),
                new ISOCountryCode("Sao Tome and Principe", "ST", "STP", 678),
                new ISOCountryCode("Saudi Arabia", "SA", "SAU", 682),
                new ISOCountryCode("Senegal", "SN", "SEN", 686),
                new ISOCountryCode("Serbia", "RS", "SRB", 688),
                new ISOCountryCode("Seychelles", "SC", "SYC", 690),
                new ISOCountryCode("Sierra Leone", "SL", "SLE", 694),
                new ISOCountryCode("Singapore", "SG", "SGP", 702),
                new ISOCountryCode("Sint Maarten (Dutch part)", "SX", "SXM", 534),
                new ISOCountryCode("Slovakia", "SK", "SVK", 703),
                new ISOCountryCode("Slovenia", "SI", "SVN", 705),
                new ISOCountryCode("Solomon Islands", "SB", "SLB", 090),
                new ISOCountryCode("Somalia", "SO", "SOM", 706),
                new ISOCountryCode("South Africa", "ZA", "ZAF", 710),
                new ISOCountryCode("South Georgia and the South Sandwich Islands", "GS", "SGS", 239),
                new ISOCountryCode("South Sudan", "SS", "SSD", 728),
                new ISOCountryCode("Spain", "ES", "ESP", 724),
                new ISOCountryCode("Sri Lanka", "LK", "LKA", 144),
                new ISOCountryCode("Sudan", "SD", "SDN", 729),
                new ISOCountryCode("Suriname", "SR", "SUR", 740),
                new ISOCountryCode("Svalbard and Jan Mayen", "SJ", "SJM", 744),
                new ISOCountryCode("Swaziland", "SZ", "SWZ", 748),
                new ISOCountryCode("Sweden", "SE", "SWE", 752),
                new ISOCountryCode("Switzerland", "CH", "CHE", 756),
                new ISOCountryCode("Syrian Arab Republic", "SY", "SYR", 760),
                new ISOCountryCode("Taiwan (Province of China)", "TW", "TWN", 158),
                new ISOCountryCode("Tajikistan", "TJ", "TJK", 762),
                new ISOCountryCode("Tanzania, United Republic of", "TZ", "TZA", 834),
                new ISOCountryCode("Thailand", "TH", "THA", 764),
                new ISOCountryCode("Timor-Leste", "TL", "TLS", 626),
                new ISOCountryCode("Togo", "TG", "TGO", 768),
                new ISOCountryCode("Tokelau", "TK", "TKL", 772),
                new ISOCountryCode("Tonga", "TO", "TON", 776),
                new ISOCountryCode("Trinidad and Tobago", "TT", "TTO", 780),
                new ISOCountryCode("Tunisia", "TN", "TUN", 788),
                new ISOCountryCode("Turkey", "TR", "TUR", 792),
                new ISOCountryCode("Turkmenistan", "TM", "TKM", 795),
                new ISOCountryCode("Turks and Caicos Islands", "TC", "TCA", 796),
                new ISOCountryCode("Tuvalu", "TV", "TUV", 798),
                new ISOCountryCode("Uganda", "UG", "UGA", 800),
                new ISOCountryCode("Ukraine", "UA", "UKR", 804),
                new ISOCountryCode("United Arab Emirates", "AE", "ARE", 784),
                new ISOCountryCode("United Kingdom", "GB", "GBR", 826),
                new ISOCountryCode("United States", "US", "USA", 840),
                new ISOCountryCode("United States Minor Outlying Islands", "UM", "UMI", 581),
                new ISOCountryCode("Uruguay", "UY", "URY", 858),
                new ISOCountryCode("Uzbekistan", "UZ", "UZB", 860),
                new ISOCountryCode("Vanuatu", "VU", "VUT", 548),
                new ISOCountryCode("Venezuela, Bolivarian Republic of ", "VE", "VEN", 862),
                new ISOCountryCode("Viet Nam", "VN", "VNM", 704),
                new ISOCountryCode("Virgin Islands (British)", "VG", "VGB", 092),
                new ISOCountryCode("Virgin Islands (U.S.)", "VI", "VIR", 850),
                new ISOCountryCode("Wallis and Futuna", "WF", "WLF", 876),
                new ISOCountryCode("Western Sahara*", "EH", "ESH", 732),
                new ISOCountryCode("Yemen", "YE", "YEM", 887),
                new ISOCountryCode("Zambia", "ZM", "ZMB", 894),
                new ISOCountryCode("Zimbabwe", "ZW", "ZWE", 716)

                #endregion
            };

            Codes.Add("UK", icc[234]);
            Codes.Add("United States of America", icc[235]);
            foreach (ISOCountryCode countryCode in icc) {
                Codes.Add(countryCode.Alpha2, countryCode);
                Codes.Add(countryCode.Alpha3, countryCode);
                Codes.Add(countryCode.EnglishName, countryCode);
            }
        }

        private ISOCountryCodes() {
        }

        public static ISOCountryCodes Instance { get { return _instance; } }

        /// <summary>Gets the ISO code with specified english name.</summary>
        /// <param name="name">The english name.</param>
        /// <returns>An instance of <see cref="ISOCode"/> if found; otherwise <c>null</c>.</returns>
        public override ISOCountryCode GetByEnglishName(string name) {
            return Codes.ContainsKey(name) ? Codes[name] : null;
        }

        /// <summary>Gets all known ISO codes.</summary>
        /// <returns>An information about all known ISO codes.</returns>
        public new static IEnumerable<ISOCountryCode> GetAllKnownCodes() {
            return Codes.Values.Distinct();
        }
    }

}