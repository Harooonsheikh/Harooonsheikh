using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpSalesOrderCustomStatus
    {
        public string orderNo { get; set; }
        public string salesId { get; set; }
        public simpleTypeOrderOrderStatus status { get; set; }
        public simpleTypeOrderShippingStatus shippingStatus { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingURL { get; set; }
        public Boolean IncludeERPOrderNumberInStatus { get; set; }
        public Boolean IncludeTrackingInfoInStatus { get; set; }
    }

    public enum simpleTypeOrderOrderStatus
    {

        /// <remarks/>
        CREATED,

        /// <remarks/>
        NEW,

        /// <remarks/>
        OPEN,

        /// <remarks/>
        CANCELLED,

        /// <remarks/>
        COMPLETED,

        /// <remarks/>
        REPLACED,

        /// <remarks/>
        FAILED,
    }

    public enum simpleTypeOrderShippingStatus
    {

        /// <remarks/>
        NOT_SHIPPED,

        /// <remarks/>
        PART_SHIPPED,

        /// <remarks/>
        SHIPPED,
    }
}
