using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpInventoryProducts
    {
        #region "Properties"

        public string Description { get; set; }
        public bool UseBundleInventory { get; set; }
        public bool DefaultInstock { get; set; }


        public List<ErpProduct> Products { get; set; }

        #endregion

        #region "Constructor"

        public ErpInventoryProducts()
        {

        }

        #endregion
    }
}
