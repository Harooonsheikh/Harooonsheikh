using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public partial class EcomsalesOrderEntity
    {
        public string est_shipping_date { get; set; }
        public string website_from { get; set; }
        public string carrier { get; set; }
        public bool isResidential { get; set; }
        public IntegrationKey _integrationKey { get; set; }
        public string discount_codes { get; set; }
        public string shipping_tax { get; set; }
        public EcomGiftCard[] gift_cards { get; set; }
    }
    public partial class EcomsalesOrderPaymentEntity
    {
        public string payment_transaction_id { get; set; }        
      
    }
}
