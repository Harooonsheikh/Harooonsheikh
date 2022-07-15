using System.Collections.Generic;
namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcustomerCustomerEntity
    {
        #region Joie Fields
        public string website { get; set; }
        public string store { get; set; }
        public string gender { get; set; }
        public string birthday_month { get; set; }

        public long edgeaxintegrationkey { get; set; }
        #endregion
        #region JJ Fields


        #endregion
        public List<EcomcustomerAddressEntityItem> addresses { get; set; }
	}
}
