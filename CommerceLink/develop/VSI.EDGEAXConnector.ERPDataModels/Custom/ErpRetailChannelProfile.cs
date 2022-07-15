using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpRetailChannelProfile
    {
        #region Properties
        public string Name { get; set; }
        public long ChannelProfileId { get; set; }
        public int ChannelProfileTypeId { get; set; }
        public List<ErpRetailChannelProfileProperty> ChannelProfileProperties { get; set; }
        #endregion Properties

        #region Constructor
        public ErpRetailChannelProfile()
        {

        }
        #endregion Constructor
    }
}
