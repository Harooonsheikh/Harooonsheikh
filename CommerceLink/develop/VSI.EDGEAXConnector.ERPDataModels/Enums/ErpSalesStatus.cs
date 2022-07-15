using System.Runtime.Serialization;
namespace VSI.EDGEAXConnector.ERPDataModels
{

    public enum ErpSalesStatus
    {
        [EnumMember(Value = "0")]
        Unknown = 0,
        [EnumMember(Value = "1")]
        Created = 1,
        [EnumMember(Value = "2")]
        Processing = 2,
        [EnumMember(Value = "3")]
        Delivered = 3,
        [EnumMember(Value = "4")]
        Invoiced = 4,
        [EnumMember(Value = "5")]
        Confirmed = 5,
        [EnumMember(Value = "6")]
        Sent = 6,
        [EnumMember(Value = "7")]
        Canceled = 7,
        [EnumMember(Value = "8")]
        Lost = 8,
    }
}
