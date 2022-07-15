using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    /// <summary>
    /// CartManager
    /// </summary>
    public class CartManager : IJobManager
    {
        #region Properties
        public static readonly string IDENTIFIER = "CartSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;
        ConfigurationHelper configurationHelper;
        public EmailSender emailSender = null;
        public StoreDto store;

        #endregion

        #region Constructor
        /// <summary>
        /// CartManager constructor initialize the class object.
        /// </summary>
        /// <param name="erpAdapterFactory"></param>
        /// <param name="eComAdapterFactory"></param>
        public CartManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory = new EComAdapterFactory();
        }

        public string GetGroup()
        {
            return GROUP;
        }

        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DiscountSync);
            return job;
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.CartSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.emailSender = new EmailSender(store.StoreKey);
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
        }

        public void JobLog(JobSchedule jobLog, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobLog.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }
        public void SetStore(StoreDto store)
        {
            this.store = store;
        }

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncCarts() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);

                //Getting Carts                
                var erpCartController = _erpAdapterFactory.CreateCartController(this.store.StoreKey);
                var erpCarts = erpCartController.GetAbandonedCarts();

                var customerAccountIds = erpCarts.Select(cart => cart.CustomerId).Distinct().ToList();

                var erpContactPersonController = _erpAdapterFactory.CreateContactPersonController(this.store.StoreKey);
                var erpContactPersons = erpContactPersonController.GetContactPersons(customerAccountIds);

                var erpQuotations = this.CreateCustomerQuotations(erpCarts, erpContactPersons);

                if (erpQuotations != null && erpQuotations.Count > 0)
                {
                    var erpQuotationController = this._erpAdapterFactory.CreateQuotationController(this.store.StoreKey);
                    erpQuotationController.CreateCustomerQuotations(erpQuotations, this.store.StoreKey);
                }
                else
                {
                    CustomLogger.LogWarn(string.Format("No Abandoned Cart found, Please check Logs"), store.StoreId, store.CreatedBy);
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail(string.Empty, ex.ToString(), string.Empty, (int)Common.Enums.EmailTemplateId.SimpleNotification);
                throw;
            }
        }
        #endregion

        //private const string Delimeter = ",";
        //private const string LeadTitle = "Abandoned Shopping Cart - {0} - {1:yyyyMMddHHmm}";
        //private const string LeadSource = "Abandoned Purchase";
        //private const string LeadProductLine = "TeamViewer";
        //private const string TVShoppingcartContentColumnName = "tv_shoppingcart_content_";
        //private const string OrderType = "Quote";
        //private const string SalesOriginId = "Abon Purch";

        #region Public Methods
        /// <summary>
        /// SyncCart initialize the sync process to get channel configurations from AX and to push to Ecom.
        /// </summary>
        /// <returns></returns>
       

        public void UpdateJobStatus(JobSchedule jobstatus, Common.Enums.SynchJobStatus status)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// CreateCustomerQuotations creates Quotation Data obect.
        /// </summary>
        /// <param name="erpCarts"></param>
        /// <param name="erpContactPersons"></param>
        /// <returns></returns>
        private List<ErpCustomerOrderInfo> CreateCustomerQuotations(List<ErpCart> erpCarts, List<ErpContactPerson> erpContactPersons)
        {
            var erpQuotations = new List<ErpCustomerOrderInfo>();

            IntegrationManager integrationManager = new IntegrationManager(store.StoreKey);

            if (erpCarts != null)
            {
                foreach (var cart in erpCarts)
                {
                    if (erpContactPersons != null)
                    {
                        var contactPerson = erpContactPersons.FirstOrDefault(cp => cp.CustAccount == cart.CustomerId);

                        if (contactPerson != null)
                        {
                            var erpCustomerOrderInfo = new ErpCustomerOrderInfo
                            {
                                //AddressRecordId
                                ChannelRecordId = cart.ChannelId.HasValue ? cart.ChannelId.Value.ToString() : string.Empty,
                                ChannelReferenceId = cart.Id,
                                Comment = cart.Comment,
                                CommissionSalesGroup = cart.CommissionSalesGroup,
                                CreationDateString = cart.BusinessDate.HasValue ? cart.BusinessDate.Value.ToString("dd-MMM-yyyy HH:mm") : string.Empty,
                                CustomerAccount = cart.CustomerId,
                                //??CustomerRecordId = cart.CustomerId, // It is DirPartyId 
                                DataAreaId = string.Empty, //?? to be updated in CRT Controller
                                DeliveryMode = cart.DeliveryMode,
                                Email = contactPerson.Email,
                                OrderType = this.configurationHelper.GetSetting(APPLICATION.AbandonedCartOrderType),
                                //??SourceId = CartManager.LeadSource, // used SalesOriginId in CustomAttributes
                                StoreId = cart.ChannelId.HasValue ? cart.ChannelId.Value.ToString() : string.Empty,
                                TotalManualDiscountAmount = cart.TotalManualDiscountAmount.HasValue ? cart.TotalManualDiscountAmount.Value : 0,
                                TotalManualDiscountPercentage = cart.TotalManualDiscountPercentage.HasValue ? cart.TotalManualDiscountPercentage.Value : 0,
                                TransactionId = cart.Id,
                                WarehouseId = cart.WarehouseId,
                                ExtensionProperties = new ObservableCollection<ErpCommerceProperty>
                                {
                                    new ErpCommerceProperty {Key= "ContactPersonId", Value = new ErpCommercePropertyValue { StringValue = contactPerson.ContactPersonId } },
                                    new ErpCommerceProperty{Key= "SalesOriginId",  Value = new ErpCommercePropertyValue { StringValue = this.configurationHelper.GetSetting(APPLICATION.AbandonedCartSalesOriginId) } },
                                    new ErpCommerceProperty { Key = "QuotationName",  Value = new ErpCommercePropertyValue { StringValue = string.Format(this.configurationHelper.GetSetting(APPLICATION.AbandonedCartTitle), contactPerson.LastName, cart.ModifiedDateTime) } }
                                }
                            };

                            if (cart.CartLines != null)
                            {
                                erpCustomerOrderInfo.Items = new ObservableCollection<ErpItemInfo>();
                                foreach (var cartLine in cart.CartLines)
                                {
                                    var erpItemInfo = new ErpItemInfo
                                    {
                                        Catalog = cartLine.CatalogId.HasValue ? cartLine.CatalogId.Value : 0,
                                        Comment = cartLine.Comment,
                                        DeliveryMode = cartLine.DeliveryMode,
                                        Discount = cartLine.LineDiscount.HasValue ? cartLine.LineDiscount.Value : 0,
                                        //??Discounts
                                        Giftcard = cartLine.IsGiftCardLine.HasValue ? cartLine.IsGiftCardLine.Value : false,
                                        InvoiceId = cartLine.InvoiceId,
                                        ItemId = cartLine.ItemId, //??
                                        ITEMID = cartLine.ItemId, //?? why is it duplicate
                                        ItemTaxGroup = cartLine.ItemTaxGroupId,
                                        LineDscAmount = cartLine.LineDiscount.HasValue ? cartLine.LineDiscount.Value : 0, //??
                                        LineManualDiscountAmount = cartLine.LineManualDiscountAmount.HasValue ? cartLine.LineManualDiscountAmount.Value : 0,
                                        LineManualDiscountPercentage = cartLine.LineManualDiscountPercentage.HasValue ? cartLine.LineManualDiscountPercentage.Value : 0,
                                        LineNumber = cartLine.LineNumber.HasValue ? cartLine.LineNumber.Value : 0,
                                        NetAmount = cartLine.NetAmountWithoutTax.HasValue ? cartLine.NetAmountWithoutTax.Value : 0,
                                        Price = cartLine.Price.HasValue ? cartLine.Price.Value : 0,
                                        Quantity = cartLine.Quantity.HasValue ? cartLine.Quantity.Value : 0,
                                        VariantId = cartLine.ProductId.HasValue ? integrationManager.GetProductVariantId(cartLine.ProductId.Value.ToString()) : string.Empty, //??
                                        WarehouseId = string.IsNullOrWhiteSpace(cartLine.WarehouseId) ? cart.WarehouseId : cartLine.WarehouseId,
                                        SourceId = this.configurationHelper.GetSetting(APPLICATION.AbandonedCartSalesOriginId),
                                        ExtensionProperties = new ObservableCollection<ErpCommerceProperty>
                                        {
                                            new ErpCommerceProperty{Key= "TMVContractValidFrom",  Value = new ErpCommercePropertyValue { StringValue = DateTime.Now.ToString("dd-MMM-yyyy HH:mm") } }
                                        }
                                    };

                                    erpCustomerOrderInfo.Items.Add(erpItemInfo);
                                }
                            }
                            erpQuotations.Add(erpCustomerOrderInfo);
                        }//if (contactPerson != null)
                        else
                        {
                            integrationManager.MarkAbandonedCartFailedContactPerson(cart.Id);
                        }
                    }//if (erpContactPersons != null)
                }
            }

            return erpQuotations;
        }
        #endregion

    }
}
