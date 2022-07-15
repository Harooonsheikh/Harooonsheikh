using System;
using System.Linq;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public  class CodesDAL : BaseClass
    {
        public CodesDAL(string storeKey) : base(storeKey)
        {

        }
        public  string GetTaxCode(TaxGroups groupType)
        {
            int type = Convert.ToInt32(ConfigObjecsType.TaxCodes);
            int grpType = Convert.ToInt32(groupType);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == grpType && k.EntityType == type && k.StoreId== StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  string GetShippingChargeCode()
        {
            int type = Convert.ToInt32(ConfigObjecsType.Charges);
            int chargeType =  Convert.ToInt32(Charges.ShippingCharges);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == chargeType  && k.EntityType == type && k.StoreId== StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  string GetMonogramCodes()
        {
            int type = Convert.ToInt32(ConfigObjecsType.Charges);
            int chargeType = Convert.ToInt32(Charges.MonogramCharges);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == chargeType && k.EntityType == type && k.StoreId==StoreId);
                return row != null ? row.ErpValue : "";
            }

        }
        public  ConfigurableObject GetGiftCardCode()
        {
            int giftCardEntity = Convert.ToInt32(ConfigObjecsType.GiftCards);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.EntityType == giftCardEntity && k.StoreId==StoreId);
                return row;
            }
        }
        public  string GetErpGiftCardCode(string comKey)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ComValue == comKey && k.StoreId==StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  string GetDiscountChargeCode()
        {
            int type = Convert.ToInt32(ConfigObjecsType.Charges);
            int chargeType = Convert.ToInt32(Charges.DiscountCharges);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == chargeType && k.EntityType == type && k.StoreId==StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  string GetShippingDiscountChargeCode()
        {
            int type = Convert.ToInt32(ConfigObjecsType.Charges);
            int chargeType = Convert.ToInt32(Charges.ShippingDiscountCharges);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == chargeType && k.EntityType == type && k.StoreId==StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
    }
}
