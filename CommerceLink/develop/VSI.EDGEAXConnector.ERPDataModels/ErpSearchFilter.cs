using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpSearchFilter
    {
        public ErpSearchFilter()
        {

        }

        public string Key { get; set; }
        public int FilterTypeValue { get; set; }
        public ObservableCollection<ErpSearchFilterValue> SearchValues { get; set; }
    }
}
