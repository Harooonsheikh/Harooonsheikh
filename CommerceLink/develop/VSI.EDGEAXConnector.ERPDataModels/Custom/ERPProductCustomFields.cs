using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpProductCustomFields
    {

        #region Properties

        public long RECID { get; set; }

        public long TMVProductType
        {
            get; set;
        }
        public int TMVCancellationPeriod
        {
            get; set;
        }
        public int TMVTerminationPeriod
        {
            get; set;
        }
        public string TMVAutoRenewal
        {
            get; set;
        }
        public long ReplenishmentWeight
        {
            get; set;
        }

        public ObservableCollection<ErpCommerceProperty> ERPExtensionProperties { get; set; }


        #endregion Properties
    }
}
