import { Dropdown } from '../models/common';

export class MockData {
    static CURRENCIES: Dropdown[] = [
        { Value: "EUR", Text: 'Euro' },
        { Value: "USD", Text: 'US Dollar' },
        { Value: "GBP", Text: 'Pound Sterling' },
        { Value: "AED", Text: 'UAE Dirham' },
        { Value: "TRY", Text: 'Turkish Lira' },
        { Value: "PLN", Text: 'Zloty' },
        { Value: "PKR", Text: 'Pakistan Rupee' },
        { Value: "INR", Text: 'Indian Rupee' },
        { Value: "AUD", Text: 'Australian Dollar' },
        { Value: "BRL", Text: 'Brazilian Real' },
        { Value: "KRW", Text: 'South Korea' }
 ];

    static COUNTRIES: Dropdown[] = [
        { Value: "NLD", Text: "Netherland" },
        { Value: "BEL", Text: "Belgium" },
        { Value: "EST", Text: "Estonia" },
        { Value: "PRT", Text: "Portugal" },
        { Value: "MLT", Text: "Malta" },
        { Value: "BGR", Text: "Bulgaria" },
        { Value: "ALB", Text: "Albania" },
        { Value: "KOR", Text: "South Korea" },
        { Value: "JPN", Text: "Japan" },
        { Value: "PRI", Text: "Puerto Rico" },
        { Value: "CRI", Text: "Costa Rica" },
        { Value: "PAN", Text: "Panama" },
        { Value: "DOM", Text: "Dominic Republic" },
        { Value: "URY", Text: "Uruguay" },
        { Value: "ECU", Text: "Ecuador" },
        { Value: "POL", Text: "Poland" },
        { Value: "CHN", Text: "China" },
        { Value: "ASM", Text: "American Samoa" },
        { Value: "TKM", Text: "Turkmenistan" },
        { Value: "TUV", Text: "Tuvalu" },
        { Value: "WLF", Text: "Wallis and Futuna Islands" },
        { Value: "ESH", Text: "Western Sahara" },
        { Value: "ERI", Text: "Eritrea" },
        { Value: "SHN", Text: "Saint Helena" },
        { Value: "PLW", Text: "Palau" },
        { Value: "SLB", Text: "Solomon Islands" },
        { Value: "IOT", Text: "British Indian Ocean Territory" },
        { Value: "MHL", Text: "Marshall Islands" },
        { Value: "FSM", Text: "Micronesia Federated States of" },
        { Value: "VUT", Text: "Vanuatu" },
        { Value: "TJK", Text: "Tajikistan" },
        { Value: "KIR", Text: "Kiribati" },
        { Value: "WSM", Text: "Western Samoa" },
        { Value: "TCA", Text: "Turks and Caicos Islands" },
        { Value: "UZB", Text: "Uzbekistan" },
        { Value: "MNP", Text: "Northern Mariana Islands" },
        { Value: "COK", Text: "Cook Islands" },
        { Value: "KGZ", Text: "Kyrgyzstan" },
        { Value: "MNG", Text: "Mongolia" },
        { Value: "AFG", Text: "Afghanistan" },
        { Value: "MDV", Text: "Maldives" },
        { Value: "YEM", Text: "Yemen" },
        { Value: "MMR", Text: "Myanmar" },
        { Value: "KHM", Text: "Cambodia" },
        { Value: "BRN", Text: "Brunei Darussalam" },
        { Value: "GUM", Text: "Guam" },
        { Value: "VIR", Text: "Virgin Islands US" },
        { Value: "FJI", Text: "Fiji" },
        { Value: "PNG", Text: "Papua New Guinea" },
        { Value: "PYF", Text: "French Polynesia" },
        { Value: "IRQ", Text: "Iraq" },
        { Value: "VGB", Text: "British Virgin Islands" },
        { Value: "BHR", Text: "Bahrain" },
        { Value: "JOR", Text: "Jordan" },
        { Value: "NCL", Text: "New Caledonia" },
        { Value: "OMN", Text: "Oman" },
        { Value: "KWT", Text: "Kuwait" },
        { Value: "NAM", Text: "Namibia" },
        { Value: "QAT", Text: "Qatar" },
        { Value: "LBN", Text: "Lebanon" },
        { Value: "ARE", Text: "United Arab Emirates" },
        { Value: "SAU", Text: "Saudi Arabia" },
        { Value: "USA", Text: "United States of America" },
        { Value: "TKL", Text: "Tokelau" },
        { Value: "TON", Text: "Tonga" },
        { Value: "CCK", Text: "Cocos (Keeling) Islands" },
        { Value: "COL", Text: "Christmas Island" },
        { Value: "NFK", Text: "Norfolk Island" },
        { Value: "NRU", Text: "Nauru" },
        { Value: "ATF", Text: "French Southern Territories" },
        { Value: "NIU", Text: "Niue" },
        { Value: "PCN", Text: "Pitcairn" },
        { Value: "LAO", Text: "Lao PDR" },
        { Value: "HMD", Text: "Heard and Mcdonald Islands" },
        { Value: "ATA", Text: "Antarctica" },
        { Value: "BVT", Text: "Bouvet Island" },
        { Value: "GMB", Text: "Gambia" },
        { Value: "UMI", Text: "US Minor Outlying Islands" },
        { Value: "ARG", Text: "Argentina" },
        { Value: "COL", Text: "Colombia" },
        { Value: "PER", Text: "Peru" },
        { Value: "VEN", Text: "Venezuela (Bolivarian Republic)" },
        { Value: "PRY", Text: "Paraguay" },
        { Value: "TLS", Text: "Timor-Leste" },
        { Value: "ESP", Text: "Spain" },
        { Value: "LUX", Text: "Luxembourg" },
        { Value: "IRL", Text: "Ireland" },
        { Value: "HUN", Text: "Hungary" },
        { Value: "DEU", Text: "Germany" },
        { Value: "CHL", Text: "Chile" },
        { Value: "NLD", Text: "Reunion" },
        { Value: "VEN", Text: "Venezuela" },
        { Value: "TWN", Text: "Taiwan" },
        { Value: "THA", Text: "Thailand" },
        { Value: "SVK", Text: "Slovakia" },
        { Value: "UKR", Text: "Ukraine" },
        { Value: "HRV", Text: "Croatia" },
        { Value: "ITA", Text: "Italy" },
        { Value: "SVN", Text: "Slovenia" },
        { Value: "SRB", Text: "Serbia" },
        { Value: "LVA", Text: "Latvia" },
        { Value: "CYP", Text: "Cyprus" },
        { Value: "FRA", Text: "France" },
        { Value: "GTM", Text: "Guatemala" },
        { Value: "MTQ", Text: "Martinique" },
        { Value: "GLP", Text: "Guadeloupe" },
        { Value: "NOR", Text: "Norway" },
        { Value: "AUS", Text: "Australia" },
        { Value: "FIN", Text: "Finland" },
        { Value: "GRC", Text: "Greece" },
        { Value: "IND", Text: "India" },
        { Value: "BGD", Text: "Bangladesh" },
        { Value: "LKA", Text: "Srilanka" },
        { Value: "NPL", Text: "Nepal" },
        { Value: "BTN", Text: "Bhutan" },
        { Value: "LTU", Text: "Lithuania" },
        { Value: "RUS", Text: "Russia" },
        { Value: "POL", Text: 'Poland' }
    ];


    static LANGUAGES: Dropdown[] = [
        { Value: "nl_NL", Text: 'Dutch' },
        { Value: "nl_NL", Text: 'Dutch (Belgium)' },
        { Value: "fr-BE", Text: 'French (Belgium)' },
        { Value: "en_GB", Text: 'English (United States)' },
        {Value:  "pt-PT", Text: 'Portuguese (Portugal)' }
    ];
    static Title: Dropdown[] = [
        { Value: "Mr.", Text: "Mr." },
        { Value: "Mrs.", Text: "Mrs." },
        { Value: "Dr.", Text: "Dr." },
        { Value: "Ms.", Text: "Ms." },
        { Value: "Miss", Text: "Miss" },
    ];

    static POPULARPRODUCTS: Dropdown[] = [
        // { Value: "TVB0001_000000089" , Text: "TeamViewer Business"},
        // { Value: "TVC0001_000000091" , Text: "TeamViewer Corporate"}
    ];

    static DISCOUNTMETHODS: Dropdown[] = [
        { Value: "1" , Text: 'Discount Percentage' },
        { Value: "2" , Text: 'Cash Amount' },
        { Value: "3" , Text: 'Target Amount' }
    ];

    static DISCOUNTORIGINS: Dropdown[] = [
        { Value: "1" ,  Text: 'Manual Discount' },
        { Value: "2",   Text: 'Periodic Discount'}
    ];

    static VATGROUP: Dropdown[] = [
        { Value: "NLD", Text: "C_NL_STA" },
        { Value: "BEL", Text: "C_BE_STA" },
    ];

    static DiscountReason: Dropdown[] = [
        { Value: "01", Text: "01 - No budget" },
        { Value: "02", Text: "02 - Competitor" },
        { Value: "03", Text: "03 - Technical problems" },
        { Value: "04", Text: "04 - Features missing" },
        { Value: "05", Text: "05 - Consolidation" },
        { Value: "06", Text: "06 - Churn prevention" },
        { Value: "07", Text: "07 - Bulk order" },
        { Value: "08", Text: "08 - Non profit" },
        { Value: "09", Text: "09 - Journalist" },
        { Value: "10", Text: "10 - Bought before last version release" },
        { Value: "11", Text: "11 - Pricing adjustment (New Sale)" },
        { Value: "12", Text: "12 - Update/Upgrade/Migration adjustment" },
        { Value: "13", Text: "13 - Employee discount" },
        { Value: "14", Text: "14 - Internal order" },
        { Value: "15", Text: "15 - Not For Resale License (NFR)" },
        { Value: "16", Text: "16 - Replacement License" },
        { Value: "17", Text: "17 - Other" },
        { Value: "18", Text: "18 - MDS included" },
        { Value: "20", Text: "20 - Adjustment" },
        { Value: "99", Text: "99 - Data Migration" },
    ];

    static ExpirayDays: number = 14;

    static PaymentTerms: Dropdown[] = [
        { Value: "0d", Text: "0"},
        { Value: "13d", Text: "13"},
        { Value: "14d", Text: "14"},
        { Value: "30d", Text: "30"},
        { Value: "45d", Text: "45"},
        { Value: "60d", Text: "60"},
        { Value: "90d", Text: "90"}
    ];
}
