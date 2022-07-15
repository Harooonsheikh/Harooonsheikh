using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class IngramSalesOrder
    {
        public string OrderPlacedDate { get; set; }

        public string ChannelReferenceId { get; set; }

        public string SalesId { get; set; }

        public List<IngramSalesLine> SalesLines { get; set; }
    }

}
