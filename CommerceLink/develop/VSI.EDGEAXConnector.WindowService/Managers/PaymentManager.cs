using System;
using System.Text;
using VSI.EDGEAXConnector.Logging;


namespace VSI.EDGEAXConnector.WindowService
{
    public class PaymentManager
    {

        private readonly IErpAdapterFactory _erpAdapterFactory;

        DateTime StartTime = DateTime.MinValue;
        //TransactionLogging Log = new TransactionLogging();

        public PaymentManager(IErpAdapterFactory erpAdapterFactory)
        {
            _erpAdapterFactory = erpAdapterFactory;
            //AutoMapper.Mapper.AddProfile(new VSI.EDGEAXConnector.Mapper.MappingConfig.Configuration());
        }


        public void ProcessPayment()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("Payment Manager=>ProcessPayment() Started at [{0}]", StartTime) + Environment.NewLine);
                var erpPaymentController = _erpAdapterFactory.CreatePaymentController("TODO");
               // erpPaymentController.ProcessPayment();
                traceInfo.Append(string.Format("Payment Manager=>ProcessPayment() Completed at [{0}]", StartTime) + Environment.NewLine);
                CustomLogger.LogTraceInfo(traceInfo.ToString(), 1, "System");

            }

            catch (Exception ex)
            {
                CustomLogger.LogException("ProcessPayment Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), 1, "System");
              

            }

        }
    }
}
