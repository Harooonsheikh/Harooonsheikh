using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
//using Autofac;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Common;
using System.Reflection;
using VSI.EDGEAXConnector.Enums.Enums;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class ChannelPublishingController : BaseController, IChannelPublishingController
    {

        public ChannelPublishingController(string storeKey) : base(storeKey)
        {

        }
        public void ProcessChannelPublishing()
        {

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            //if (channelPublisher == null)
            //{
            //    throw new ArgumentNullException(nameof(channelPublisher));
            //}            


            //ManagerFactory factory = await this.CreateManagerFactory();
            //IOrgUnitManager orgUnitManager = factory.GetManager<IOrgUnitManager>();
            //this.channelConfiguration = await orgUnitManager.GetOrgUnitConfiguration();

            //IStoreOperationsManager storeOperationsManager = factory.GetManager<IStoreOperationsManager>();

            //OnlineChannelPublishStatusType publishingStatus = (OnlineChannelPublishStatusType)(await storeOperationsManager.GetOnlineChannelPublishStatus());

            var channelCRTManager = new ChannelPublishingCRTManager();

            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Inside ProcessChannelPublishing() going to call channelCRTManager.GetOnlineChannelPublishStatus()");
            int publishingStatus = channelCRTManager.GetOnlineChannelPublishStatus(currentStore.StoreKey);
            // REMOVE THIS LINE // CustomLogger.LogTraceInfo("executed channelCRTManager.GetOnlineChannelPublishStatus() publish status is = " + publishingStatus);

            if (publishingStatus != (int)CHANNELPUBLISHINGSTATUS.Published
                && publishingStatus != (int)CHANNELPUBLISHINGSTATUS.InProgress)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL41001,currentStore);
                throw new Exception(message);
            }

            IEnumerable<ErpCategory> categories;
            Dictionary<long, IEnumerable<ErpAttributeCategory>> categoriesAttributes;

            //// always load the categories but process them only if the channel is not published yet.
            try
            {
                var categoriesInfo = channelCRTManager.GetCategoryAttributes(currentStore.StoreKey);

                categories = categoriesInfo.Item1;
                categoriesAttributes = categoriesInfo.Item2;
                int categoriesCount = categories.Count();
                //NetTracer.Information(Resources.NumberOfReadCategoriesAndTheirAttributes, categoriesCount, categoriesAttributes.Count);
                if (categoriesCount == 0)
                {
                    //throw new InvalidDataException(string.Format(
                    //    "Navigation categories count returned is '{0}'. Error details {1}",
                    //    categoriesCount,
                    //    Resources.ErrorNoNavigationCategories));

                    string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL41004), 0, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL41005));

                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message);

                    throw new Exception(message);
                }
            

                //    // Loading product attributes schema from CRT
                //    IEnumerable<AttributeProduct> productAttributes = await this.LoadProductAttributes(factory);
                var productAttributes = channelCRTManager.GetChannelProductAttributes(currentStore.StoreKey);
                //    channelPublisher.OnValidateProductAttributes(productAttributes);

                int listingAttributesCount = productAttributes.Count();
                //    NetTracer.Information(Resources.NumberOfReadAttributes, listingAttributesCount);
                if (listingAttributesCount == 0)
                {
                    //throw new InvalidDataException(string.Format(
                    //    "Listing Attributes Count returned is '{0}'. Error details '{1}'",
                    //    listingAttributesCount,
                    //    Resources.ErrorNoSchemaAttributes));

                    string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL41002)
                        , listingAttributesCount
                        , CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL41003));

                    message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, message);

                    throw new Exception(message);

                }

                //    ChannelLanguage language = this.channelConfiguration.Languages.Single(l => l.IsDefault);
                //    CultureInfo culture = new CultureInfo(language.LanguageId);

                //    PublishingParameters parameters = new PublishingParameters
                //    {
                //        Categories = categories,
                //        CategoriesAttributes = categoriesAttributes,
                //        ProductsAttributes = productAttributes,
                //        ChannelDefaultCulture = culture,
                //        GiftCartItemId = this.channelConfiguration.GiftCardItemId
                //    };

                if (publishingStatus == (int)CHANNELPUBLISHINGSTATUS.InProgress)
                {
                    //channelPublisher.OnChannelInformationAvailable(parameters, true);
                    //await storeOperationsManager.SetOnlineChannelPublishStatus((int)OnlineChannelPublishStatusType.Published, null);
                    channelCRTManager.SetOnlineChannelPublishStatus((int)CHANNELPUBLISHINGSTATUS.Published, "Marked Published through Channel Publishing Sync Job", currentStore.StoreKey);
                    //Trace.WriteLine("Successfully changed the channel's publishing status to Published.");
                }
                else
                {
                    //channelPublisher.OnChannelInformationAvailable(parameters, false);
                }

                //    return parameters;
            }
            catch (Exception ex)
            {
                //    RetailLogger.Log.EcommercePlatformChannelPublishFailure(ex);
                //string error = string.Format("{ 0}. Time of the failure: { 1}", ex.Message, DateTime.UtcNow);

                string error = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL41006), ex.Message.ToString(), DateTime.UtcNow.ToString());

                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, error);

                //    storeOperationsManager.SetOnlineChannelPublishStatus((int)OnlineChannelPublishStatusType.Failed, error);

                channelCRTManager.SetOnlineChannelPublishStatus((int)CHANNELPUBLISHINGSTATUS.Failed, error, currentStore.StoreKey);

                throw;
            }
        }


        /// <summary>
        /// LoadCategories loads catagories using CRT
        /// </summary>
        /// <returns></returns>
        private List<ErpCategory> LoadCategories()
        {
            List<ErpCategory> categories = new List<ErpCategory>();

            var storeCRTManager = new StoreCRTManager();
            categories = storeCRTManager.GetChannelCategoryHierarchy(currentStore.StoreKey);          

            return categories;
        }


    }
}
