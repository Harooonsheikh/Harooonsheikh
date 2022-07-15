using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpItemInfo
    {
        //++TV Properties FDD018
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //++ TV extenstion properties FDD020

        public string TMVCONTRACTVALIDFROM { get; set; }
        public string TMVCONTRACTCALCULATEFROM { get; set; }
        public string  TMVCONTRACTVALIDTO { get; set; }
        public string TMVORIGINALLINEAMOUNT { get; set; }
        public string TMVPARENT { get; set; }
        public string TMVAUTOPROLONGATION { get; set; }
        public string TMVPIT { get; set; }
        public string ITEMID { get; set; }
        public string NAME { get; set; }
        public string INVENTDIMID { get; set; }
        public string TMVTimeQuantity { get; set; }
        public string SALESUNIT { get; set; }

    }
}
