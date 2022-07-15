using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpPrice
    {

        #region "Properties"

        public string Currency { get; set; }
        public string Name { get; set; }
        public bool Online { get; set; }
        public string Parent { get; set; }

        public List<ErpProductPrice> Prices { get; set; }

        #endregion

        #region "Constructor"

        public ErpPrice()
        {

        }

        #endregion
    }
}
