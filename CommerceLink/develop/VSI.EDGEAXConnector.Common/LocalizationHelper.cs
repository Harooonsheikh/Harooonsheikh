using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Common
{
    public static class LocalizationHelper
    {
        public static string CountryThreeLetterISOCode(string CountryTwoLetterISOCode)
        {
            string twoLetterISORegionName = CountryTwoLetterISOCode;
            string threeLetterISORegionName = "";
            var countryCodesMapping = new Dictionary<string, RegionInfo>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var culture in cultures)
            {
                var region = new RegionInfo(culture.LCID);
                if (region.TwoLetterISORegionName.ToString().Equals(twoLetterISORegionName, StringComparison.OrdinalIgnoreCase))
                {
                    threeLetterISORegionName = region.ThreeLetterISORegionName;
                    break;
                }
            }
            return threeLetterISORegionName;
        }

        public class ISO3166Country
        {
            public ISO3166Country(string name, string alpha2, string alpha3, int numericCode)
            {
                this.Name = name;
                this.Alpha2 = alpha2;
                this.Alpha3 = alpha3;
                this.NumericCode = numericCode;
            }
            public string Name { get; private set; }
            public string Alpha2 { get; private set; }
            public string Alpha3 { get; private set; }
            public int NumericCode { get; private set; }
        }

        public static string CountryCodeTwoLettersToThreeLetters(string countryTwoLetterCode)
        {
            List<ISO3166Country> CountryCollection = new List<ISO3166Country>();

            #region Colletion
            CountryCollection.Add(new ISO3166Country("Afghanistan", "AF", "AFG", 4));
            CountryCollection.Add(new ISO3166Country("Åland Islands", "AX", "ALA", 248));
            CountryCollection.Add(new ISO3166Country("Albania", "AL", "ALB", 8));
            CountryCollection.Add(new ISO3166Country("Algeria", "DZ", "DZA", 12));
            CountryCollection.Add(new ISO3166Country("American Samoa", "AS", "ASM", 16));
            CountryCollection.Add(new ISO3166Country("Andorra", "AD", "AND", 20));
            CountryCollection.Add(new ISO3166Country("Angola", "AO", "AGO", 24));
            CountryCollection.Add(new ISO3166Country("Anguilla", "AI", "AIA", 660));
            CountryCollection.Add(new ISO3166Country("Antarctica", "AQ", "ATA", 10));
            CountryCollection.Add(new ISO3166Country("Antigua and Barbuda", "AG", "ATG", 28));
            CountryCollection.Add(new ISO3166Country("Argentina", "AR", "ARG", 32));
            CountryCollection.Add(new ISO3166Country("Armenia", "AM", "ARM", 51));
            CountryCollection.Add(new ISO3166Country("Aruba", "AW", "ABW", 533));
            CountryCollection.Add(new ISO3166Country("Australia", "AU", "AUS", 36));
            CountryCollection.Add(new ISO3166Country("Austria", "AT", "AUT", 40));
            CountryCollection.Add(new ISO3166Country("Azerbaijan", "AZ", "AZE", 31));
            CountryCollection.Add(new ISO3166Country("Bahamas", "BS", "BHS", 44));
            CountryCollection.Add(new ISO3166Country("Bahrain", "BH", "BHR", 48));
            CountryCollection.Add(new ISO3166Country("Bangladesh", "BD", "BGD", 50));
            CountryCollection.Add(new ISO3166Country("Barbados", "BB", "BRB", 52));
            CountryCollection.Add(new ISO3166Country("Belarus", "BY", "BLR", 112));
            CountryCollection.Add(new ISO3166Country("Belgium", "BE", "BEL", 56));
            CountryCollection.Add(new ISO3166Country("Belize", "BZ", "BLZ", 84));
            CountryCollection.Add(new ISO3166Country("Benin", "BJ", "BEN", 204));
            CountryCollection.Add(new ISO3166Country("Bermuda", "BM", "BMU", 60));
            CountryCollection.Add(new ISO3166Country("Bhutan", "BT", "BTN", 64));
            CountryCollection.Add(new ISO3166Country("Bolivia (Plurinational State of)", "BO", "BOL", 68));
            CountryCollection.Add(new ISO3166Country("Bonaire, Sint Eustatius and Saba", "BQ", "BES", 535));
            CountryCollection.Add(new ISO3166Country("Bosnia and Herzegovina", "BA", "BIH", 70));
            CountryCollection.Add(new ISO3166Country("Botswana", "BW", "BWA", 72));
            CountryCollection.Add(new ISO3166Country("Bouvet Island", "BV", "BVT", 74));
            CountryCollection.Add(new ISO3166Country("Brazil", "BR", "BRA", 76));
            CountryCollection.Add(new ISO3166Country("British Indian Ocean Territory", "IO", "IOT", 86));
            CountryCollection.Add(new ISO3166Country("Brunei Darussalam", "BN", "BRN", 96));
            CountryCollection.Add(new ISO3166Country("Bulgaria", "BG", "BGR", 100));
            CountryCollection.Add(new ISO3166Country("Burkina Faso", "BF", "BFA", 854));
            CountryCollection.Add(new ISO3166Country("Burundi", "BI", "BDI", 108));
            CountryCollection.Add(new ISO3166Country("Cabo Verde", "CV", "CPV", 132));
            CountryCollection.Add(new ISO3166Country("Cambodia", "KH", "KHM", 116));
            CountryCollection.Add(new ISO3166Country("Cameroon", "CM", "CMR", 120));
            CountryCollection.Add(new ISO3166Country("Canada", "CA", "CAN", 124));
            CountryCollection.Add(new ISO3166Country("Cayman Islands", "KY", "CYM", 136));
            CountryCollection.Add(new ISO3166Country("Central African Republic", "CF", "CAF", 140));
            CountryCollection.Add(new ISO3166Country("Chad", "TD", "TCD", 148));
            CountryCollection.Add(new ISO3166Country("Chile", "CL", "CHL", 152));
            CountryCollection.Add(new ISO3166Country("China", "CN", "CHN", 156));
            CountryCollection.Add(new ISO3166Country("Christmas Island", "CX", "CXR", 162));
            CountryCollection.Add(new ISO3166Country("Cocos (Keeling) Islands", "CC", "CCK", 166));
            CountryCollection.Add(new ISO3166Country("Colombia", "CO", "COL", 170));
            CountryCollection.Add(new ISO3166Country("Comoros", "KM", "COM", 174));
            CountryCollection.Add(new ISO3166Country("Congo", "CG", "COG", 178));
            CountryCollection.Add(new ISO3166Country("Congo (Democratic Republic of the)", "CD", "COD", 180));
            CountryCollection.Add(new ISO3166Country("Cook Islands", "CK", "COK", 184));
            CountryCollection.Add(new ISO3166Country("Costa Rica", "CR", "CRI", 188));
            CountryCollection.Add(new ISO3166Country("Côte d'Ivoire", "CI", "CIV", 384));
            CountryCollection.Add(new ISO3166Country("Croatia", "HR", "HRV", 191));
            CountryCollection.Add(new ISO3166Country("Cuba", "CU", "CUB", 192));
            CountryCollection.Add(new ISO3166Country("Curaçao", "CW", "CUW", 531));
            CountryCollection.Add(new ISO3166Country("Cyprus", "CY", "CYP", 196));
            CountryCollection.Add(new ISO3166Country("Czech Republic", "CZ", "CZE", 203));
            CountryCollection.Add(new ISO3166Country("Denmark", "DK", "DNK", 208));
            CountryCollection.Add(new ISO3166Country("Djibouti", "DJ", "DJI", 262));
            CountryCollection.Add(new ISO3166Country("Dominica", "DM", "DMA", 212));
            CountryCollection.Add(new ISO3166Country("Dominican Republic", "DO", "DOM", 214));
            CountryCollection.Add(new ISO3166Country("Ecuador", "EC", "ECU", 218));
            CountryCollection.Add(new ISO3166Country("Egypt", "EG", "EGY", 818));
            CountryCollection.Add(new ISO3166Country("El Salvador", "SV", "SLV", 222));
            CountryCollection.Add(new ISO3166Country("Equatorial Guinea", "GQ", "GNQ", 226));
            CountryCollection.Add(new ISO3166Country("Eritrea", "ER", "ERI", 232));
            CountryCollection.Add(new ISO3166Country("Estonia", "EE", "EST", 233));
            CountryCollection.Add(new ISO3166Country("Ethiopia", "ET", "ETH", 231));
            CountryCollection.Add(new ISO3166Country("Falkland Islands (Malvinas)", "FK", "FLK", 238));
            CountryCollection.Add(new ISO3166Country("Faroe Islands", "FO", "FRO", 234));
            CountryCollection.Add(new ISO3166Country("Fiji", "FJ", "FJI", 242));
            CountryCollection.Add(new ISO3166Country("Finland", "FI", "FIN", 246));
            CountryCollection.Add(new ISO3166Country("France", "FR", "FRA", 250));
            CountryCollection.Add(new ISO3166Country("French Guiana", "GF", "GUF", 254));
            CountryCollection.Add(new ISO3166Country("French Polynesia", "PF", "PYF", 258));
            CountryCollection.Add(new ISO3166Country("French Southern Territories", "TF", "ATF", 260));
            CountryCollection.Add(new ISO3166Country("Gabon", "GA", "GAB", 266));
            CountryCollection.Add(new ISO3166Country("Gambia", "GM", "GMB", 270));
            CountryCollection.Add(new ISO3166Country("Georgia", "GE", "GEO", 268));
            CountryCollection.Add(new ISO3166Country("Germany", "DE", "DEU", 276));
            CountryCollection.Add(new ISO3166Country("Ghana", "GH", "GHA", 288));
            CountryCollection.Add(new ISO3166Country("Gibraltar", "GI", "GIB", 292));
            CountryCollection.Add(new ISO3166Country("Greece", "GR", "GRC", 300));
            CountryCollection.Add(new ISO3166Country("Greenland", "GL", "GRL", 304));
            CountryCollection.Add(new ISO3166Country("Grenada", "GD", "GRD", 308));
            CountryCollection.Add(new ISO3166Country("Guadeloupe", "GP", "GLP", 312));
            CountryCollection.Add(new ISO3166Country("Guam", "GU", "GUM", 316));
            CountryCollection.Add(new ISO3166Country("Guatemala", "GT", "GTM", 320));
            CountryCollection.Add(new ISO3166Country("Guernsey", "GG", "GGY", 831));
            CountryCollection.Add(new ISO3166Country("Guinea", "GN", "GIN", 324));
            CountryCollection.Add(new ISO3166Country("Guinea-Bissau", "GW", "GNB", 624));
            CountryCollection.Add(new ISO3166Country("Guyana", "GY", "GUY", 328));
            CountryCollection.Add(new ISO3166Country("Haiti", "HT", "HTI", 332));
            CountryCollection.Add(new ISO3166Country("Heard Island and McDonald Islands", "HM", "HMD", 334));
            CountryCollection.Add(new ISO3166Country("Holy See", "VA", "VAT", 336));
            CountryCollection.Add(new ISO3166Country("Honduras", "HN", "HND", 340));
            CountryCollection.Add(new ISO3166Country("Hong Kong", "HK", "HKG", 344));
            CountryCollection.Add(new ISO3166Country("Hungary", "HU", "HUN", 348));
            CountryCollection.Add(new ISO3166Country("Iceland", "IS", "ISL", 352));
            CountryCollection.Add(new ISO3166Country("India", "IN", "IND", 356));
            CountryCollection.Add(new ISO3166Country("Indonesia", "ID", "IDN", 360));
            CountryCollection.Add(new ISO3166Country("Iran (Islamic Republic of)", "IR", "IRN", 364));
            CountryCollection.Add(new ISO3166Country("Iraq", "IQ", "IRQ", 368));
            CountryCollection.Add(new ISO3166Country("Ireland", "IE", "IRL", 372));
            CountryCollection.Add(new ISO3166Country("Isle of Man", "IM", "IMN", 833));
            CountryCollection.Add(new ISO3166Country("Israel", "IL", "ISR", 376));
            CountryCollection.Add(new ISO3166Country("Italy", "IT", "ITA", 380));
            CountryCollection.Add(new ISO3166Country("Jamaica", "JM", "JAM", 388));
            CountryCollection.Add(new ISO3166Country("Japan", "JP", "JPN", 392));
            CountryCollection.Add(new ISO3166Country("Jersey", "JE", "JEY", 832));
            CountryCollection.Add(new ISO3166Country("Jordan", "JO", "JOR", 400));
            CountryCollection.Add(new ISO3166Country("Kazakhstan", "KZ", "KAZ", 398));
            CountryCollection.Add(new ISO3166Country("Kenya", "KE", "KEN", 404));
            CountryCollection.Add(new ISO3166Country("Kiribati", "KI", "KIR", 296));
            CountryCollection.Add(new ISO3166Country("Korea (Democratic People's Republic of)", "KP", "PRK", 408));
            CountryCollection.Add(new ISO3166Country("Korea (Republic of)", "KR", "KOR", 410));
            CountryCollection.Add(new ISO3166Country("Kuwait", "KW", "KWT", 414));
            CountryCollection.Add(new ISO3166Country("Kyrgyzstan", "KG", "KGZ", 417));
            CountryCollection.Add(new ISO3166Country("Lao People's Democratic Republic", "LA", "LAO", 418));
            CountryCollection.Add(new ISO3166Country("Latvia", "LV", "LVA", 428));
            CountryCollection.Add(new ISO3166Country("Lebanon", "LB", "LBN", 422));
            CountryCollection.Add(new ISO3166Country("Lesotho", "LS", "LSO", 426));
            CountryCollection.Add(new ISO3166Country("Liberia", "LR", "LBR", 430));
            CountryCollection.Add(new ISO3166Country("Libya", "LY", "LBY", 434));
            CountryCollection.Add(new ISO3166Country("Liechtenstein", "LI", "LIE", 438));
            CountryCollection.Add(new ISO3166Country("Lithuania", "LT", "LTU", 440));
            CountryCollection.Add(new ISO3166Country("Luxembourg", "LU", "LUX", 442));
            CountryCollection.Add(new ISO3166Country("Macao", "MO", "MAC", 446));
            CountryCollection.Add(new ISO3166Country("Macedonia (the former Yugoslav Republic of)", "MK", "MKD", 807));
            CountryCollection.Add(new ISO3166Country("Madagascar", "MG", "MDG", 450));
            CountryCollection.Add(new ISO3166Country("Malawi", "MW", "MWI", 454));
            CountryCollection.Add(new ISO3166Country("Malaysia", "MY", "MYS", 458));
            CountryCollection.Add(new ISO3166Country("Maldives", "MV", "MDV", 462));
            CountryCollection.Add(new ISO3166Country("Mali", "ML", "MLI", 466));
            CountryCollection.Add(new ISO3166Country("Malta", "MT", "MLT", 470));
            CountryCollection.Add(new ISO3166Country("Marshall Islands", "MH", "MHL", 584));
            CountryCollection.Add(new ISO3166Country("Martinique", "MQ", "MTQ", 474));
            CountryCollection.Add(new ISO3166Country("Mauritania", "MR", "MRT", 478));
            CountryCollection.Add(new ISO3166Country("Mauritius", "MU", "MUS", 480));
            CountryCollection.Add(new ISO3166Country("Mayotte", "YT", "MYT", 175));
            CountryCollection.Add(new ISO3166Country("Mexico", "MX", "MEX", 484));
            CountryCollection.Add(new ISO3166Country("Micronesia (Federated States of)", "FM", "FSM", 583));
            CountryCollection.Add(new ISO3166Country("Moldova (Republic of)", "MD", "MDA", 498));
            CountryCollection.Add(new ISO3166Country("Monaco", "MC", "MCO", 492));
            CountryCollection.Add(new ISO3166Country("Mongolia", "MN", "MNG", 496));
            CountryCollection.Add(new ISO3166Country("Montenegro", "ME", "MNE", 499));
            CountryCollection.Add(new ISO3166Country("Montserrat", "MS", "MSR", 500));
            CountryCollection.Add(new ISO3166Country("Morocco", "MA", "MAR", 504));
            CountryCollection.Add(new ISO3166Country("Mozambique", "MZ", "MOZ", 508));
            CountryCollection.Add(new ISO3166Country("Myanmar", "MM", "MMR", 104));
            CountryCollection.Add(new ISO3166Country("Namibia", "NA", "NAM", 516));
            CountryCollection.Add(new ISO3166Country("Nauru", "NR", "NRU", 520));
            CountryCollection.Add(new ISO3166Country("Nepal", "NP", "NPL", 524));
            CountryCollection.Add(new ISO3166Country("Netherlands", "NL", "NLD", 528));
            CountryCollection.Add(new ISO3166Country("New Caledonia", "NC", "NCL", 540));
            CountryCollection.Add(new ISO3166Country("New Zealand", "NZ", "NZL", 554));
            CountryCollection.Add(new ISO3166Country("Nicaragua", "NI", "NIC", 558));
            CountryCollection.Add(new ISO3166Country("Niger", "NE", "NER", 562));
            CountryCollection.Add(new ISO3166Country("Nigeria", "NG", "NGA", 566));
            CountryCollection.Add(new ISO3166Country("Niue", "NU", "NIU", 570));
            CountryCollection.Add(new ISO3166Country("Norfolk Island", "NF", "NFK", 574));
            CountryCollection.Add(new ISO3166Country("Northern Mariana Islands", "MP", "MNP", 580));
            CountryCollection.Add(new ISO3166Country("Norway", "NO", "NOR", 578));
            CountryCollection.Add(new ISO3166Country("Oman", "OM", "OMN", 512));
            CountryCollection.Add(new ISO3166Country("Pakistan", "PK", "PAK", 586));
            CountryCollection.Add(new ISO3166Country("Palau", "PW", "PLW", 585));
            CountryCollection.Add(new ISO3166Country("Palestine, State of", "PS", "PSE", 275));
            CountryCollection.Add(new ISO3166Country("Panama", "PA", "PAN", 591));
            CountryCollection.Add(new ISO3166Country("Papua New Guinea", "PG", "PNG", 598));
            CountryCollection.Add(new ISO3166Country("Paraguay", "PY", "PRY", 600));
            CountryCollection.Add(new ISO3166Country("Peru", "PE", "PER", 604));
            CountryCollection.Add(new ISO3166Country("Philippines", "PH", "PHL", 608));
            CountryCollection.Add(new ISO3166Country("Pitcairn", "PN", "PCN", 612));
            CountryCollection.Add(new ISO3166Country("Poland", "PL", "POL", 616));
            CountryCollection.Add(new ISO3166Country("Portugal", "PT", "PRT", 620));
            CountryCollection.Add(new ISO3166Country("Puerto Rico", "PR", "PRI", 630));
            CountryCollection.Add(new ISO3166Country("Qatar", "QA", "QAT", 634));
            CountryCollection.Add(new ISO3166Country("Réunion", "RE", "REU", 638));
            CountryCollection.Add(new ISO3166Country("Romania", "RO", "ROU", 642));
            CountryCollection.Add(new ISO3166Country("Russian Federation", "RU", "RUS", 643));
            CountryCollection.Add(new ISO3166Country("Rwanda", "RW", "RWA", 646));
            CountryCollection.Add(new ISO3166Country("Saint Barthélemy", "BL", "BLM", 652));
            CountryCollection.Add(new ISO3166Country("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", 654));
            CountryCollection.Add(new ISO3166Country("Saint Kitts and Nevis", "KN", "KNA", 659));
            CountryCollection.Add(new ISO3166Country("Saint Lucia", "LC", "LCA", 662));
            CountryCollection.Add(new ISO3166Country("Saint Martin (French part)", "MF", "MAF", 663));
            CountryCollection.Add(new ISO3166Country("Saint Pierre and Miquelon", "PM", "SPM", 666));
            CountryCollection.Add(new ISO3166Country("Saint Vincent and the Grenadines", "VC", "VCT", 670));
            CountryCollection.Add(new ISO3166Country("Samoa", "WS", "WSM", 882));
            CountryCollection.Add(new ISO3166Country("San Marino", "SM", "SMR", 674));
            CountryCollection.Add(new ISO3166Country("Sao Tome and Principe", "ST", "STP", 678));
            CountryCollection.Add(new ISO3166Country("Saudi Arabia", "SA", "SAU", 682));
            CountryCollection.Add(new ISO3166Country("Senegal", "SN", "SEN", 686));
            CountryCollection.Add(new ISO3166Country("Serbia", "RS", "SRB", 688));
            CountryCollection.Add(new ISO3166Country("Seychelles", "SC", "SYC", 690));
            CountryCollection.Add(new ISO3166Country("Sierra Leone", "SL", "SLE", 694));
            CountryCollection.Add(new ISO3166Country("Singapore", "SG", "SGP", 702));
            CountryCollection.Add(new ISO3166Country("Sint Maarten (Dutch part)", "SX", "SXM", 534));
            CountryCollection.Add(new ISO3166Country("Slovakia", "SK", "SVK", 703));
            CountryCollection.Add(new ISO3166Country("Slovenia", "SI", "SVN", 705));
            CountryCollection.Add(new ISO3166Country("Solomon Islands", "SB", "SLB", 90));
            CountryCollection.Add(new ISO3166Country("Somalia", "SO", "SOM", 706));
            CountryCollection.Add(new ISO3166Country("South Africa", "ZA", "ZAF", 710));
            CountryCollection.Add(new ISO3166Country("South Georgia and the South Sandwich Islands", "GS", "SGS", 239));
            CountryCollection.Add(new ISO3166Country("South Sudan", "SS", "SSD", 728));
            CountryCollection.Add(new ISO3166Country("Spain", "ES", "ESP", 724));
            CountryCollection.Add(new ISO3166Country("Sri Lanka", "LK", "LKA", 144));
            CountryCollection.Add(new ISO3166Country("Sudan", "SD", "SDN", 729));
            CountryCollection.Add(new ISO3166Country("Suriname", "SR", "SUR", 740));
            CountryCollection.Add(new ISO3166Country("Svalbard and Jan Mayen", "SJ", "SJM", 744));
            CountryCollection.Add(new ISO3166Country("Swaziland", "SZ", "SWZ", 748));
            CountryCollection.Add(new ISO3166Country("Sweden", "SE", "SWE", 752));
            CountryCollection.Add(new ISO3166Country("Switzerland", "CH", "CHE", 756));
            CountryCollection.Add(new ISO3166Country("Syrian Arab Republic", "SY", "SYR", 760));
            CountryCollection.Add(new ISO3166Country("Taiwan, Province of China[a]", "TW", "TWN", 158));
            CountryCollection.Add(new ISO3166Country("Tajikistan", "TJ", "TJK", 762));
            CountryCollection.Add(new ISO3166Country("Tanzania, United Republic of", "TZ", "TZA", 834));
            CountryCollection.Add(new ISO3166Country("Thailand", "TH", "THA", 764));
            CountryCollection.Add(new ISO3166Country("Timor-Leste", "TL", "TLS", 626));
            CountryCollection.Add(new ISO3166Country("Togo", "TG", "TGO", 768));
            CountryCollection.Add(new ISO3166Country("Tokelau", "TK", "TKL", 772));
            CountryCollection.Add(new ISO3166Country("Tonga", "TO", "TON", 776));
            CountryCollection.Add(new ISO3166Country("Trinidad and Tobago", "TT", "TTO", 780));
            CountryCollection.Add(new ISO3166Country("Tunisia", "TN", "TUN", 788));
            CountryCollection.Add(new ISO3166Country("Turkey", "TR", "TUR", 792));
            CountryCollection.Add(new ISO3166Country("Turkmenistan", "TM", "TKM", 795));
            CountryCollection.Add(new ISO3166Country("Turks and Caicos Islands", "TC", "TCA", 796));
            CountryCollection.Add(new ISO3166Country("Tuvalu", "TV", "TUV", 798));
            CountryCollection.Add(new ISO3166Country("Uganda", "UG", "UGA", 800));
            CountryCollection.Add(new ISO3166Country("Ukraine", "UA", "UKR", 804));
            CountryCollection.Add(new ISO3166Country("United Arab Emirates", "AE", "ARE", 784));
            CountryCollection.Add(new ISO3166Country("United Kingdom of Great Britain and Northern Ireland", "GB", "GBR", 826));
            CountryCollection.Add(new ISO3166Country("United States of America", "US", "USA", 840));
            CountryCollection.Add(new ISO3166Country("United States Minor Outlying Islands", "UM", "UMI", 581));
            CountryCollection.Add(new ISO3166Country("Uruguay", "UY", "URY", 858));
            CountryCollection.Add(new ISO3166Country("Uzbekistan", "UZ", "UZB", 860));
            CountryCollection.Add(new ISO3166Country("Vanuatu", "VU", "VUT", 548));
            CountryCollection.Add(new ISO3166Country("Venezuela (Bolivarian Republic of)", "VE", "VEN", 862));
            CountryCollection.Add(new ISO3166Country("Viet Nam", "VN", "VNM", 704));
            CountryCollection.Add(new ISO3166Country("Virgin Islands (British)", "VG", "VGB", 92));
            CountryCollection.Add(new ISO3166Country("Virgin Islands (U.S.)", "VI", "VIR", 850));
            CountryCollection.Add(new ISO3166Country("Wallis and Futuna", "WF", "WLF", 876));
            CountryCollection.Add(new ISO3166Country("Western Sahara", "EH", "ESH", 732));
            CountryCollection.Add(new ISO3166Country("Yemen", "YE", "YEM", 887));
            CountryCollection.Add(new ISO3166Country("Zambia", "ZM", "ZMB", 894));
            CountryCollection.Add(new ISO3166Country("Zimbabwe", "ZW", "ZWE", 716));

            #endregion
            
            var country = CountryCollection.Where(c => c.Alpha2.Equals(countryTwoLetterCode)).FirstOrDefault();
            if(country != null)
            {
                return country.Alpha3;
            }
            else
            {
                throw new CommerceLinkError("Customer Address: " + countryTwoLetterCode + " country code not found in EdgeAX CommerceLink LocalizationHelper_CountryCodeTwoLettersToThreeLetters method. Sales order cannot be processed");
            }
        }
    }
}
