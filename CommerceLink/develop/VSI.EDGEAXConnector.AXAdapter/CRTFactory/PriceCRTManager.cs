using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class PriceCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public PriceCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public List<ErpProductPrice> GetActiveProductPrice(long channelId, long catalogId, List<long> productIds, DateTime date, string customerAccountNumber, string storeKey)
        {
            var priceController = _crtFactory.CreatePriceController(storeKey);
            return priceController.GetActiveProductPrice(channelId, catalogId, productIds, date, customerAccountNumber);
        }
    }
}
