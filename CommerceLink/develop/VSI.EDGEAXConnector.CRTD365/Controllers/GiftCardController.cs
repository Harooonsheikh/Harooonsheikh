using EdgeAXCommerceLink.RetailProxy.Extensions;
using System.Reflection;
using System.Threading.Tasks;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using System;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class GiftCardController : BaseController, IGiftCardController
    {

        public GiftCardController(string storeKey) : base(storeKey)
        {

        }
        public ErpGiftCard GetGiftCardBalance(string giftCardId)
        {
            string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpGiftCard giftCard = new ErpGiftCard();
            try
            {
                var returnGiftCard = ECL_GetGiftCard(giftCardId);
                // Map RetailServer object to ERP object
                giftCard = _mapper.Map<Microsoft.Dynamics.Commerce.RetailProxy.GiftCard, ErpGiftCard>(returnGiftCard);
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string messageException = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, messageException + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return giftCard;
        }
        public ErpGiftCard IssueGiftCard(string giftCardId, decimal amount, string currencyCode, long channelId, string termincalId, string staffId, string transactionId, string receiptId)
        {
            string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpGiftCard giftCard = new ErpGiftCard();
            channelId = baseChannelId;
            try
            {
                var returnGiftCard = ECL_IssueGiftCard(giftCardId, amount, currencyCode, channelId, termincalId, staffId, transactionId, receiptId);
                // Map RetailServer object to ERP object
                giftCard = _mapper.Map<Microsoft.Dynamics.Commerce.RetailProxy.GiftCard, ErpGiftCard>(returnGiftCard);
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string messageException = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, messageException + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return giftCard;
        }
        public ErpGiftCard LockGiftCard(string giftCardId, long channelId, string terminalId)
        {
            string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpGiftCard giftCard = new ErpGiftCard();
            channelId = baseChannelId;
            try
            {
                var returnGiftCard = ECL_LockGiftCard(giftCardId, channelId, terminalId);
                // Map RetailServer object to ERP object
                giftCard = _mapper.Map<Microsoft.Dynamics.Commerce.RetailProxy.GiftCard, ErpGiftCard>(returnGiftCard);
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string messageException = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, messageException + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return giftCard;
        }
        public bool UnlockGiftCard(string giftCardId)
        {
            string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            try
            {
                var response = ECL_UnlockGiftCard(giftCardId);
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                return response;
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string messageException = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, messageException + rpe.Message, rpe);
                throw exp;
            }
        }
        public bool PayGiftCard(string giftCardId, decimal paymentAmount, string paymentCurrencyCode, long channelId, string terminalId, string staffId, string transactionId, string receiptId)
        {
            string message = CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            channelId = baseChannelId;
            try
            {
                var response = ECL_PayGiftCard(giftCardId, paymentAmount, paymentCurrencyCode, channelId, terminalId, staffId, transactionId, receiptId);
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                return response;
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string messageException = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, messageException + rpe.Message, rpe);
                throw exp;
            }
        }
        #region RetailServer API
        [Trace]
        private Microsoft.Dynamics.Commerce.RetailProxy.GiftCard ECL_GetGiftCard(string giftCardId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_GetGiftCard(giftCardId)).Result;
        }
        [Trace]
        private Microsoft.Dynamics.Commerce.RetailProxy.GiftCard ECL_IssueGiftCard(string giftCardId, decimal amount, string currencyCode, long channelId, string termincalId, string staffId, string transactionId, string receiptId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_IssueGiftCard(transactionId, giftCardId, amount, currencyCode, channelId, termincalId, staffId, receiptId)).Result;
        }
        [Trace]
        private Microsoft.Dynamics.Commerce.RetailProxy.GiftCard ECL_LockGiftCard(string giftCardId, long channelId, string terminalId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_LockGiftCard(giftCardId, channelId, terminalId)).Result;
        }
        [Trace]
        private bool ECL_UnlockGiftCard(string giftCardId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<ICartManager>();
            //return Task.Run(async () => await cartManager.ECL_UnlockGiftCard(giftCardId)).Result;
        }
        [Trace]
        private bool ECL_PayGiftCard(string giftCardId, decimal paymentAmount, string paymentCurrencyCode, long channelId,
            string terminalId, string staffId, string transactionId, string receiptId)
        {
            throw new NotImplementedException();
            //var cartManager = RPFactory.GetManager<ICartManager>();
            //var response = Task.Run(async () => await cartManager.ECL_PayGiftCard(transactionId, giftCardId, paymentAmount,
            //    paymentCurrencyCode, channelId, terminalId, staffId, receiptId)).Result;
            //return response;
        }
        #endregion
    }
}
