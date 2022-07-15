using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VSI.CommerceLink.EcomDataModel
{
    public enum EcomAddressType
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Invoice = 1,
        [EnumMember]
        Delivery = 2,
        [EnumMember]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dlv", Justification = "Maintaining name compatability w/POS")]
        AltDlv = 3,
        [EnumMember]
        SWIFT = 4,
        [EnumMember]
        Payment = 5,
        [EnumMember]
        Service = 6,
        [EnumMember]
        Home = 7,
        [EnumMember]
        Other = 8,
        [EnumMember]
        Business = 9,
        [EnumMember]
        RemitTo = 10,
        [EnumMember]
        ShipCarrierThirdPartyShipping = 11,
        [EnumMember]
        Statement = 12,
        [EnumMember]
        FixedAsset = 15,
        [EnumMember]
        Onetime = 16,
        [EnumMember]
        Recruit = 17,
        [EnumMember]
        SMS = 18,
        [EnumMember]
        Lading_W = 101,
        [EnumMember]
        Unlading_W = 102,
        [EnumMember]
        Consignment_IN = 103,
    }
}
