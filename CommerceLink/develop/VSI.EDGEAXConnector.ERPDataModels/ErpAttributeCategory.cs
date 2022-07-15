using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public sealed class ErpAttributeCategory : ErpAttributeBase
    {
        public ErpAttributeCategory()
        {
        }


        public long Category { get; set; }

        public ErpAttributeValueBase CategoryAttributeValue { get; set; }

        public string CategoryHierarchyName { get; set; }

        public string CategoryName { get; set; }

        public string CategoryPath { get; set; }
    }
}
