using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpAddressPurpose
    {
        public ErpAddressPurpose()
        { }
        public long RecordId { get; set; }
        public int AddressType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

    }
    
    //NS: D365 Update 8.1 Application change end
}
