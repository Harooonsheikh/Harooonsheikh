using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ERPQuotationReasonGroup
    {

        #region Properties

        public string DESCRIPTION
        {
            get; set;
        }
        public string REASONID
        {
            get; set;
        }
        public string DATAAREAID
        {
            get; set;
        }
        public long RECID
        {
            get; set;
        }

        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        #endregion Properties
    }
}
