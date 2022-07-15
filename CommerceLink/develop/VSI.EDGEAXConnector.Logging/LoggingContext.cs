using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Logging
{
    public class LoggingContext
    {
        public string MethodName { get; set; }
        public DataDirectionType DataDirectionId { get; set; }
        public string DataPacket { get; set; }
        public DateTime CreatedOn { get; set; }
        public int StoreId { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string EcomTransactionId { get; set; }
        public string RequestInitiatedIP { get; set; }
        public string OutputPacket { get; set; }
        public DateTime OutputSentAt { get; set; }
        public string IdentifierKey { get; set; }
        public string IdentifierValue { get; set; }
        public int IsSuccess { get; set; }
        public decimal TotalProcessingDuration { get; set; }
    }
}
