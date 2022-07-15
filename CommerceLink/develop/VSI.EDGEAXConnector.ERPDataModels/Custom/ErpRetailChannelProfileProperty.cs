using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpRetailChannelProfileProperty
    {
        #region Properties

        public int Key { get; set; }
        public string Value { get; set; }
        public long ChannelProfileId { get; set; }
        public long ChannelProfilePropertyId { get; set; }

        #endregion Properties

        #region Constructor
        public ErpRetailChannelProfileProperty()
        {

        }
        #endregion Constructor
    }
}
