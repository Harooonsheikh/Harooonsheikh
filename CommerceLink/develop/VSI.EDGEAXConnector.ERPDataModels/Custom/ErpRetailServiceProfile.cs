using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpRetailServiceProfile
    {
        #region Properties
        public string Name { get; set; }
        public long ServiceProfileId { get; set; }
        public List<ErpRetailServiceProfileProperty> ServiceProfileProperties { get; set; }
        #endregion Properties

        #region Constructor
        public ErpRetailServiceProfile()
        {

        }
        #endregion Constructor
    }
}
