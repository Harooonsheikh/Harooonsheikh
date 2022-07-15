using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{

    public class PaymentValue
    {
        public string BooleanValue { get; set; }
        public string ByteValue { get; set; }
        public string DecimalValue { get; set; }
        public string DateTimeOffsetValue { get; set; }
        public string IntegerValue { get; set; }
        public string LongValue { get; set; }
        public string StringValue { get; set; }
    }

    public class PaymentExtensionProperty
    {
        public string Key { get; set; }
        public PaymentValue Value { get; set; }
    }

    public class ERPPaymentConnectorResponse
    {
        public object ProfileId { get; set; }
        public long RecordId { get; set; }
        public string Name { get; set; }
        public string ConnectorProperties { get; set; }
        public bool IsTestMode { get; set; }
        public IList<PaymentExtensionProperty> ExtensionProperties { get; set; }
    }

    public class PaymentConnectorData
    {
        public string ServiceAccountId { get; set; }
        public string SupportedTenderTypes { get; set; }
        public ERPPaymentConnectorResponse PaymentService { get; set; }
        public string ConnectorAssembly { get; set; }
    }


}
