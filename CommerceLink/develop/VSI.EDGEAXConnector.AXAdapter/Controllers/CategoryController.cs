using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.CRTClasses;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// CategoryController class performs Category related activities.
    /// </summary>
    public class CategoryController : BaseController, ICategoryController
    {
        #region Properties and Constants
        private const string CategoryDelimeter = "/";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CategoryController(string storeKey) : base(storeKey)
        {
            //// initialize the channel: instantiate CRT, create CRT data managers, load channel settings

            //Utility.InitializeCommerceRuntime();

            //this.runtime = Utility._CommerceRuntime;

            //this.currentChannelState = ChannelState.InitializeChannel(runtime, new Guid());

            //AutoMapper.Mapper.Initialize(cnf => cnf.AddProfile(new ErpMappingConfiguration()));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// GetCategory get category from AX.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ErpCategory GetCategory(string key)
        {
            try
            {
                var cat = new ErpCategory()
                {
                    Name = "Jeans",
                    RecordId = 22,
                    ParentCategory = 11
                };
                //NS: Remove
                //NS: Comment Start
                //var erpCat = Mapper.Map<Category, ErpCategory>(cat);
                //NS: Comment End
                return cat;
            }
            catch (System.Exception exp)
            {
               CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
            }

            return null;
        }

        /// <summary>
        /// GetAllCategories get all Categories from AX.
        /// </summary>
        /// <returns></returns>
        public List<ErpCategory> GetAllCategories()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore,MethodBase.GetCurrentMethod().Name);

            string categoryAssignment = configurationHelper.GetSetting(ECOM.Category_Assignment);

            List<ErpCategory> categories = new List<ErpCategory>();

            if (categoryAssignment.Equals(CATEGORYASSIGNMENT.SINGLE.ToString()))
            {
                ErpCategory erpCategory = new ErpCategory();
                erpCategory.Name = configurationHelper.GetSetting(ECOM.Root_Category_Id).ToString();
                categories.Add(erpCategory);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(categories));
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return categories;
            }
            else if (categoryAssignment.Equals(CATEGORYASSIGNMENT.ALL.ToString()))
            {
                //TO-DO currently No categories
                categories = this.LoadCategories();
                var erpCategories = _mapper.Map<List<ErpCategory>, List<ErpCategory>>(categories);
                Stopwatch sw = Stopwatch.StartNew();
                this.ProcessCatagoryData(erpCategories);
                sw.Stop();

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCategories));
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return erpCategories;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(categories));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return categories;

            //TODO: Why it has been fetched
            //QueryResultSettings getCategoryAttributesCriteria = new QueryResultSettings(new PagingInfo(200, 0));
            //var channelProductAttributes = currentChannelState.ProductManager.GetChannelProductAttributes(getCategoryAttributesCriteria);
            ////var channelAttributes = currentChannelState.ProductManager.get(getCategoryAttributesCriteria);           

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// ProcessCatagoryData process and apply custom logics.
        /// </summary>
        /// <param name="categories"></param>
        private void ProcessCatagoryData(List<ErpCategory> categories)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpTextValueTranslation friendlyName;
            ErpCommerceProperty isActive;
            ErpCommerceProperty friendlyNameExt;
            foreach (var cat in categories)
            {
                if (cat.NameTranslations != null)
                {
                    //Replace Name with Friendly Name to avoid prefix or post fix.
                    friendlyName = cat.NameTranslations.FirstOrDefault(nm => nm != null && nm.Language.Equals(configurationHelper.GetSetting(APPLICATION.Default_Culture), StringComparison.CurrentCultureIgnoreCase));
                    if (friendlyName != null && !string.IsNullOrWhiteSpace(friendlyName.Text))
                    {
                        cat.Name = friendlyName.Text;
                    }
                }

                if (cat.ExtensionProperties != null)
                {
                    //ISACTIVE extension property
                    isActive = cat.ExtensionProperties.FirstOrDefault(pr => pr != null && pr.Key.Equals("ISACTIVE", StringComparison.CurrentCultureIgnoreCase));
                    if (isActive != null && isActive.Value != null && isActive.Value.IntegerValue.HasValue)
                    {
                        cat.IsActive = isActive.Value.IntegerValue.Value;
                    }

                    //FRIENDLYNAME extension property
                    friendlyNameExt = cat.ExtensionProperties.FirstOrDefault(pr => pr != null && pr.Key.Equals("FRIENDLYNAME", StringComparison.CurrentCultureIgnoreCase));
                    if (friendlyNameExt != null && friendlyNameExt.Value != null && !string.IsNullOrWhiteSpace(friendlyNameExt.Value.StringValue))
                    {
                        cat.Name = friendlyNameExt.Value.StringValue;
                    }
                }

                this.ProcessCatagoryAttributes(cat);
            }

            // We are going to loop again as Parent Category Name may not bee processed before child. We can merge both loops in one.
            foreach (var cat in categories)
            {
                cat.FullName = this.ProcessCatagoryFullName(cat, categories);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// ProcessCatagoryFullName processes FullName. FullName contains Category Name with hierarchy.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private string ProcessCatagoryFullName(ErpCategory category, List<ErpCategory> categories)
        {
            if (category.ParentCategory != 0)
            {
                var parentCategory = categories.FirstOrDefault(cat => cat.RecordId == category.ParentCategory);

                if (parentCategory != null)
                {
                    return this.ProcessCatagoryFullName(parentCategory, categories)+ CategoryController.CategoryDelimeter + category.Name;
                }
                else
                {
                    return category.Name;
                }
            }
            else
            {
                return category.Name;
            }
        }

        /// <summary>
        /// ProcessCatagoryAttributes processes category attributes.
        /// </summary>
        /// <param name="cat"></param>
        private void ProcessCatagoryAttributes(ErpCategory cat)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(cat));

            var attributes = this.LoadCategoryAttributes(cat);

            ErpAttributeTextValue textAttribute;
            ErpAttributeFloatValue floatAttribute;
            ErpAttributeIntValue intAttribute;
            ErpAttributeBooleanValue booleanAttribute;
            cat.CustomAttributes = new List<KeyValuePair<string, string>>();
            foreach (var att in attributes)
            {
                if (att.AttributeValueRecordId > 0)
                {
                    if (att.CategoryAttributeValue is ErpAttributeTextValue)
                    {
                        textAttribute = att.CategoryAttributeValue as ErpAttributeTextValue;
                        if (textAttribute != null)
                        {
                            cat.CustomAttributes.Add(new KeyValuePair<string, string>(att.KeyName, textAttribute.TextValue));
                        }
                    }
                    else if (att.CategoryAttributeValue is ErpAttributeIntValue)
                    {
                        intAttribute = att.CategoryAttributeValue as ErpAttributeIntValue;
                        if (intAttribute != null)
                        {
                            cat.CustomAttributes.Add(new KeyValuePair<string, string>(att.KeyName, intAttribute.Value.ToString()));
                        }
                    }
                    else if (att.CategoryAttributeValue is ErpAttributeFloatValue)
                    {
                        floatAttribute = att.CategoryAttributeValue as ErpAttributeFloatValue;
                        if (floatAttribute != null)
                        {
                            cat.CustomAttributes.Add(new KeyValuePair<string, string>(att.KeyName, floatAttribute.Value.ToString()));
                        }
                    }
                    else if (att.CategoryAttributeValue is ErpAttributeBooleanValue)
                    {
                        booleanAttribute = att.CategoryAttributeValue as ErpAttributeBooleanValue;
                        if (booleanAttribute != null)
                        {
                            cat.CustomAttributes.Add(new KeyValuePair<string, string>(att.KeyName, booleanAttribute.Value.ToString()));
                        }
                    }
                }
            }
            attributes.Clear();

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// LoadCategories loads catagories using CRT
        /// </summary>
        /// <returns></returns>
        private List<ErpCategory> LoadCategories()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpCategory> categories = new List<ErpCategory>();

            var storeCRTManager = new StoreCRTManager();
            categories = storeCRTManager.GetChannelCategoryHierarchy( currentStore.StoreKey);

            // SK - Commented and old code is placed

            //List<ErpCategory> categories = new List<ErpCategory>();

            //var storeCRTManager = new StoreCRTManager();
            //categories = storeCRTManager.GetChannelCategoryHierarchy();





            //NS: Remove
            /*
            //NS: Comment Start
            //Reading categories
            QueryResultSettings getCategoriesCriteria = new QueryResultSettings(PagingInfo.AllRecords); // KAR
            PagedResult<Category> currentPageCategories;
            do
            {
                currentPageCategories = base.currentChannelState.ChannelManager.GetChannelCategoryHierarchy(getCategoriesCriteria);
                categories.AddRange(currentPageCategories.Results);
                getCategoriesCriteria.Paging.Skip += getCategoriesCriteria.Paging.Top;
            }
            while (currentPageCategories.Results.Count() == getCategoriesCriteria.Paging.Top);
            //NS: Comment Start
            */

            //TODO: We have commented and chaned it as it was never used.
            //// ******* Reading categories' attributes
            //IEnumerable<AttributeCategory> categoryAttributes;
            //QueryResultSettings getCategoryAttributesCriteria = new QueryResultSettings(new PagingInfo(MaximumPageSize, 0));
            //Dictionary<long, IEnumerable<AttributeCategory>> categoryAttributesMap = new Dictionary<long, IEnumerable<AttributeCategory>>();
            //foreach (Category category in categories)
            //{
            //    getCategoryAttributesCriteria.Paging.Skip = 0;
            //    List<AttributeCategory> allCategoryAttributes = new List<AttributeCategory>();
            //    do
            //    {
            //        categoryAttributes = channelManager.GetChannelCategoryAttributes(getCategoryAttributesCriteria, category.RecordId);
            //        allCategoryAttributes.AddRange(categoryAttributes);
            //        getCategoryAttributesCriteria.Paging.Skip = getCategoryAttributesCriteria.Paging.Skip + 200;

            //        categoryAttributesMap.Add(category.RecordId, allCategoryAttributes);
            //    }
            //    while (categoryAttributes.Count() == getCategoryAttributesCriteria.Paging.Top);
            //}
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(categories));

            return categories;
        }

        /// <summary>
        /// LoadCategories loads catagories using CRT
        /// </summary>
        /// <returns></returns>
        private List<ErpAttributeCategory> LoadCategoryAttributes(ErpCategory category)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(category));

            List<ErpAttributeCategory> allCategoryAttributes = new List<ErpAttributeCategory>();

            var storeCRTManager = new StoreCRTManager();
            allCategoryAttributes = storeCRTManager.GetChannelCategoryAttributes(category, currentStore.StoreKey);

            //NS: Remove
            /*
            //NS: Comment Start

            //ReadOnlyCollection<AttributeCategory> categoryAttributes; // KAR
            List<AttributeCategory> categoryAttributes;
            QueryResultSettings getCategoryAttributesCriteria = new QueryResultSettings(PagingInfo.AllRecords);

            Dictionary<long, IEnumerable<AttributeCategory>> categoryAttributesMap = new Dictionary<long, IEnumerable<AttributeCategory>>();

            List<AttributeCategory> allCategoryAttributes = new List<AttributeCategory>();
            //categoryAttributes = base.currentChannelState.ChannelManager.GetChannelCategoryAttributes(getCategoryAttributesCriteria, category.RecordId); //KAR
            //allCategoryAttributes.AddRange(categoryAttributes); //KAR
                                            
            //NS: Comment End
            */

            //do
            //{
            //categoryAttributes = base.currentChannelState.ChannelManager.GetChannelCategoryAttributes(getCategoryAttributesCriteria, category.RecordId);
            //allCategoryAttributes.AddRange(categoryAttributes);
            ////getCategoryAttributesCriteria.Paging.Skip = getCategoryAttributesCriteria.Paging.Skip + PagingInfo.MaximumPageSize;
            //}
            //while (categoryAttributes.Count() == getCategoryAttributesCriteria.Paging.Top);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(allCategoryAttributes));

            return allCategoryAttributes;
        }

        #endregion
    }
}

