using EdgeAXCommerceLink.Commerce.RetailProxy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public static class SalesOrderMappingHelper
    {
        public static SalesOrder MapMissingProperties(ErpSalesOrder erpSalesOrder, SalesOrder rsSalesOrder)
        {
            rsSalesOrder.TransactionTypeValue = (int)erpSalesOrder.TransactionType;

            if (rsSalesOrder.TaxLines == null)
            {
                rsSalesOrder.TaxLines = new ObservableCollection<TaxLine>();
            }

            foreach (SalesLine line in rsSalesOrder.SalesLines)
            {
                if (line.TaxLines == null)
                {
                    line.TaxLines = new ObservableCollection<TaxLine>();
                }
            }

            return rsSalesOrder;
        }
        public static ErpSalesOrder SetNullValues(ErpSalesOrder salesOrder)
        {
            //No collection should be null
            if (salesOrder.AffiliationLoyaltyTierLines == null)
            {
                List<ErpSalesAffiliationLoyaltyTier> affList = new List<ErpSalesAffiliationLoyaltyTier>();

                salesOrder.AffiliationLoyaltyTierLines = affList;

            }

            if (salesOrder.AttributeValues == null)
            {
                List<ErpAttributeValueBase> attList = new List<ErpAttributeValueBase>();

                salesOrder.AttributeValues = attList;

            }

            var chargeLines = salesOrder.ChargeLines;

            foreach (var c in chargeLines)
            {
                if (c.TaxLines == null)
                {
                    List<ErpTaxLine> tlList = new List<ErpTaxLine>();
                    c.TaxLines = tlList;
                }
            }

            if (salesOrder.DiscountCodes == null)
            {
                List<string> dcList = new List<string>();
                salesOrder.DiscountCodes = dcList;
            }

            if (salesOrder.IncomeExpenseLines == null)
            {
                List<ErpIncomeExpenseLine> incExpList = new List<ErpIncomeExpenseLine>();
                salesOrder.IncomeExpenseLines = incExpList;
            }

            if (salesOrder.LoyaltyRewardPointLines == null)
            {
                List<ErpLoyaltyRewardPointLine> lrList = new List<ErpLoyaltyRewardPointLine>();
                salesOrder.LoyaltyRewardPointLines = lrList;
            }

            if (salesOrder.ReasonCodeLines == null)
            {
                List<ErpReasonCodeLine> rcList = new List<ErpReasonCodeLine>();
                salesOrder.ReasonCodeLines = rcList;
            }

            var tLines = salesOrder.TenderLines;

            foreach (var t in tLines)
            {
                if (t.ReasonCodeLines == null)
                {
                   // List<ErpReasonCodeLine> rcList = new List<ErpReasonCodeLine>();
                    t.ReasonCodeLines = new ObservableCollection<ErpReasonCodeLine>();
                }
            }

            var salesLines = salesOrder.SalesLines;

            foreach (var s in salesLines)
            {
                if (s.PeriodicDiscountPossibilities == null)
                {
                    List<ErpDiscountLine> dlList = new List<ErpDiscountLine>();
                    s.PeriodicDiscountPossibilities = dlList;
                }

                if (s.ReasonCodeLines == null)
                {
                    List<ErpReasonCodeLine> rcList = new List<ErpReasonCodeLine>();
                    s.ReasonCodeLines = rcList;
                }

                if (s.TaxLines == null)
                {
                    List<ErpTaxLine> tlList = new List<ErpTaxLine>();
                    s.TaxLines = tlList;
                }
            }


            if (salesOrder.TaxLines == null)
            {
                List<ErpTaxLine> tlList = new List<ErpTaxLine>();
                salesOrder.TaxLines = tlList;
            }

            return salesOrder;
        }
    }
}
