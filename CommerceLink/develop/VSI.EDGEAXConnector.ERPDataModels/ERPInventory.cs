using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
  public   class ERPInventory
    {
       
           
            
            public string itemid { get; set; }
            public string color { get; set; }
            public string name { get; set; }
            public string size { get; set; }
            public string sku { get; set; }
            public string type { get; set; }
            public decimal qty { get; set; }
            public int is_in_stock { get; set; }

            

    }
}
