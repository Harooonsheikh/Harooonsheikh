using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpFiscalIntegrationDocumentAdjustment
    {
        public ObservableCollection<ErpFiscalTransactionTenderLineAdjustment> TenderLineAdjustments { get; set; }
        public ObservableCollection<ErpFiscalTransactionSalesLineAdjustment> SalesLineAdjustments { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
//HK: D365 Update 10.0 Application change start
}
