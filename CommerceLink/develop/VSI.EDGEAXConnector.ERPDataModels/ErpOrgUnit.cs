using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpOrgUnit
    {
        public ErpOrgUnit()
        {

        }

        public string EmailReceiptProfileId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string OrgUnitName { get; set; }
        public string OrgUnitFullAddress { get; set; }
        public ErpAddress OrgUnitAddress { get; set; }
        public string Currency { get; set; }
        public bool? UseDestinationBasedTax { get; set; }
        public bool? UseCustomerBasedTax { get; set; }
        public string FunctionalityProfileId { get; set; }
        public string TaxGroup { get; set; }
        public string OMOperatingUnitNumber { get; set; }
        public long RecordId { get; set; }
        public string OrgUnitNumber { get; set; }
        public string DefaultCustomerAccount { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 12 Platform change start
        public string InventoryLocationId { get; set; }
        //NS: D365 Update 12 Platform change end

        //NS: D365 Update 8.1 Application change start

        public ErpAddress ShippingWarehouseAddress { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
