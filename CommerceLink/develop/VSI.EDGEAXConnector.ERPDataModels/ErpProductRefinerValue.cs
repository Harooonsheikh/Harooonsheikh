using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpProductRefinerValue
    {
        public ErpProductRefinerValue()
        {
        }
        public long RefinerRecordId { get; set; }
        public int RefinerSourceValue { get; set; }
        public int DataTypeValue { get; set; }
        public string LeftValueBoundString { get; set; }
        public string RightValueBoundString { get; set; }
        public String UnitText { get; set; }
        public int RowNumber { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}