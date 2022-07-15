using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Common;
using System.Reflection;
using VSI.EDGEAXConnector.Enums.Enums;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class ChannelPublishingController : BaseController, IChannelPublishingController
    {
        public ChannelPublishingController(string storeKey):base(storeKey)
        {

        }
        public int GetOnlineChannelPublishStatus()
        {
            try
            {
                
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

                // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Inside VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController.GetOnlineChannelPublishStatus() going to call RPFactory.GetManager<IStoreOperationsManager>()");
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10006, currentStore, MethodBase.GetCurrentMethod().Name, "RPFactory.GetManager<IStoreOperationsManager>()");

                var response = GetOnlineChannelPublishStatusRs();
                // REMOVE THIS LINE // CustomLogger.LogTraceInfo("Inside VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController.GetOnlineChannelPublishStatus() going to call storeOperationsManager.GetOnlineChannelPublishStatus()");
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10006, currentStore, MethodBase.GetCurrentMethod().Name, "storeOperationsManager.GetOnlineChannelPublishStatus().Result");

                return response;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SetOnlineChannelPublishingStatus(int publishingStatus, string statusMessage)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                SetOnlineChannelPublishStatus(publishingStatus, statusMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Tuple<IEnumerable<ErpCategory>, Dictionary<long, IEnumerable<ErpAttributeCategory>>> GetCategoryAttributes()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var categoriesInfo = Task.Run(async () => await LoadCategories()).Result;

            List<ErpCategory> listErpCategory = new List<ErpCategory>();
            foreach (var item in categoriesInfo.Item1)
            {
                ErpCategory erpCategory = _mapper.Map<ErpCategory>(item);
                listErpCategory.Add(erpCategory);
            }


            Dictionary<long, IEnumerable<ErpAttributeCategory>> dictionaryAttributeCategory = new Dictionary<long, IEnumerable<ErpAttributeCategory>>();

            foreach (var item in categoriesInfo.Item2)
            {
                List<ErpAttributeCategory> listErpAttributeCategory = new List<ErpAttributeCategory>();

                foreach (AttributeCategory attributeCategory in item.Value)
                {
                    ErpAttributeCategory erpAttributeCategory = _mapper.Map<ErpAttributeCategory>(attributeCategory);

                    listErpAttributeCategory.Add(erpAttributeCategory);
                }

                dictionaryAttributeCategory.Add(item.Key, listErpAttributeCategory);
            }

            var categoriesTupleInfo = new Tuple<IEnumerable<ErpCategory>, Dictionary<long, IEnumerable<ErpAttributeCategory>>>(listErpCategory, dictionaryAttributeCategory);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return categoriesTupleInfo;
        }
        public List<ErpAttributeProduct> GetChannelProductAttributes()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var attributeResults = Task.Run(async () => await LoadProductAttributes()).Result;
            List<ErpAttributeProduct> listErpAttributeProduct = new List<ErpAttributeProduct>();
            foreach (var item in attributeResults)
            {
                ErpAttributeProduct erpAttributeProduct = _mapper.Map<ErpAttributeProduct>(item);
                listErpAttributeProduct.Add(erpAttributeProduct);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return listErpAttributeProduct;           
        }

        #region prvate methods

        /// <summary>
        /// Loads AX product attributes.
        /// </summary>
        /// <param name="factory">The instance of ManagerFactory.</param>
        /// <returns>Returns product attributes.</returns>
        private async Task<IEnumerable<AttributeProduct>> LoadProductAttributes()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            QueryResultSettings getProductAttributesCriteria = new QueryResultSettings();
            getProductAttributesCriteria.Paging = Paging_0_1000;
            getProductAttributesCriteria.Sorting = SortingChannel;
            List<AttributeProduct> attributes = new List<AttributeProduct>();
            IEnumerable<AttributeProduct> currentAttributePage;
            do
            {
                currentAttributePage = await GetChannelProductAttributes(getProductAttributesCriteria);
                attributes.AddRange(currentAttributePage);
                getProductAttributesCriteria.Paging.Skip = getProductAttributesCriteria.Paging.Skip + getProductAttributesCriteria.Paging.Top;
            }
            while (currentAttributePage.Count() == getProductAttributesCriteria.Paging.Top);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return attributes;
        }
        /// <summary>
        /// Loads AX categories and their attributes.
        /// </summary>
        /// <param name="factory">The instance of ManagerFactory.</param>
        /// <returns>Categories and categories' attributes.</returns>
        private async Task<Tuple<IEnumerable<Category>, Dictionary<long, IEnumerable<AttributeCategory>>>> LoadCategories()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ////******** Reading categories *****************
            PagingInfo pagingInfo = Paging_0_1000;
            QueryResultSettings getCategoriesCriteria = new QueryResultSettings() { Paging = pagingInfo };

            List<Category> categories = new List<Category>();
            IEnumerable<Category> currentPageCategories;
            var channelConfiguration = await GetOrgUnitConfigurationRs();

            do
            {
                currentPageCategories = await GetCategories(channelConfiguration, getCategoriesCriteria);
                categories.AddRange(currentPageCategories);
                getCategoriesCriteria.Paging.Skip = getCategoriesCriteria.Paging.Skip + Paging_0_1000.Top;
            }
            while (currentPageCategories.Count() == getCategoriesCriteria.Paging.Top);

            // ******* Reading categories' attributes
            QueryResultSettings getCategoryAttributesCriteria = new QueryResultSettings() { Paging = Paging_0_1000 };
            Dictionary<long, IEnumerable<AttributeCategory>> categoryAttributesMap = new Dictionary<long, IEnumerable<AttributeCategory>>();
            foreach (Category category in categories)
            {
                getCategoryAttributesCriteria.Paging.Skip = 0;
                List<AttributeCategory> allCategoryAttributes = new List<AttributeCategory>();
                IEnumerable<AttributeCategory> categoryAttributes;
                do
                {
                    categoryAttributes = await GetAttributes(category, getCategoryAttributesCriteria);
                    allCategoryAttributes.AddRange(categoryAttributes);
                    getCategoryAttributesCriteria.Paging.Skip = getCategoryAttributesCriteria.Paging.Skip + Paging_0_1000.Top;

                    categoryAttributesMap.Add(category.RecordId, allCategoryAttributes);
                }
                while (categoryAttributes.Count() == getCategoryAttributesCriteria.Paging.Top);
            }

            var result = new Tuple<IEnumerable<Category>, Dictionary<long, IEnumerable<AttributeCategory>>>(categories, categoryAttributesMap);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return result;
        }

        #endregion

        #region RetailServer API
        [Trace]
        private int GetOnlineChannelPublishStatusRs()
        {
            var storeOperationsManager = RPFactory.GetManager<IStoreOperationsManager>();
            return storeOperationsManager.GetOnlineChannelPublishStatus().Result;
        }
        [Trace]
        private void SetOnlineChannelPublishStatus(int publishingStatus, string statusMessage)
        {
            var storeOperationsManager = RPFactory.GetManager<IStoreOperationsManager>();

            storeOperationsManager.SetOnlineChannelPublishStatus(publishingStatus, statusMessage);
        }
        [Trace]
        private async Task<IEnumerable<AttributeProduct>> GetChannelProductAttributes(QueryResultSettings getProductAttributesCriteria)
        {
            IProductManager productManager = RPFactory.GetManager<IProductManager>();
            IEnumerable<AttributeProduct> currentAttributePage = await productManager.GetChannelProductAttributes(getProductAttributesCriteria);
            return currentAttributePage;
        }
        private async Task<IEnumerable<AttributeCategory>> GetAttributes(Category category, QueryResultSettings getCategoryAttributesCriteria)
        {
            ICategoryManager categoryManager = RPFactory.GetManager<ICategoryManager>();
            IEnumerable<AttributeCategory> categoryAttributes = await categoryManager.GetAttributes(category.RecordId, getCategoryAttributesCriteria);
            return categoryAttributes;
        }
        [Trace]
        private async Task<IEnumerable<Category>> GetCategories(ChannelConfiguration channelConfiguration, QueryResultSettings getCategoriesCriteria)
        {
            ICategoryManager categoryManager = RPFactory.GetManager<ICategoryManager>();
            IEnumerable<Category> currentPageCategories = await categoryManager.GetCategories(channelConfiguration.RecordId,null, getCategoriesCriteria);
            return currentPageCategories;
        }
        [Trace]
        private async Task<ChannelConfiguration> GetOrgUnitConfigurationRs()
        {
            IOrgUnitManager orgUnitManager = RPFactory.GetManager<IOrgUnitManager>();
            var channelConfiguration = await orgUnitManager.GetOrgUnitConfiguration();
            return channelConfiguration;
        }
        #endregion

    }
}
