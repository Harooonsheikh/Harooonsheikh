using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpReceiptRetrievalCriteria
    {
        public ErpReceiptRetrievalCriteria()
        {

        }

        public bool? IsCopy { get; set; }
        public bool? IsRemoteTransaction { get; set; }
        public bool? IsPreview { get; set; }
        public bool? QueryBySalesId { get; set; }
        public int? ReceiptTypeValue { get; set; }
        public long? ShiftId { get; set; }
        public string ShiftTerminalId { get; set; }
        public string HardwareProfileId { get; set; }
        public ObservableCollection<decimal> SalesLineNums { get; set; }
    }
}
