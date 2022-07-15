using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// ProductBaseController class performs Product related activities.
    /// </summary>
    public class ProductBaseController : BaseController
    {
        #region Properties

        /// <summary>
        /// It excludes from variant attributes names
        /// </summary>
        private List<string> ExcludeFromVaiantAttributeNames { get; set; } //= new List<string> { "configurable_attributes", "details", "size_fit", "product_details" };

        /// <summary>
        /// It excludes from simple attributes names
        /// </summary>
        private List<string> ExcludeFromSimpleAttributeNames { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductBaseController(bool initializeService)
            : base(initializeService)
        {
            this.LoadExcludeAttributeNames();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// GetConfigAttributes gets config attribute.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected string GetConfigAttributes(EcomcatalogProductCreateEntity p)
        {
            List<string> configAttributes = new List<string>();

            if (!string.IsNullOrEmpty(p.Color))
            {
                configAttributes.Add("color");
            }

            //if (string.IsNullOrEmpty(p.Style))
            //{
            //    configAttributes.Add("width");
            //}

            if (!string.IsNullOrEmpty(p.Size))
            {
                configAttributes.Add("size");
            }

            //if (string.IsNullOrEmpty(p.Configuration))
            //{
            //    configAttributes.Add("configuration");
            //}

            return string.Join(",", configAttributes);
        }

        /// <summary>
        /// LoadExcludeAttributeNames loads configurable attributes only.
        /// </summary>
        private void LoadExcludeAttributeNames()
        {
            //string configurableAttributes = FileHelper.Read(ConfigurationHelper.ProductConfigurableAttribute);
            //this.ConfigurableAttributeNames = configurableAttributes.Split(',').ToList();
            this.ExcludeFromVaiantAttributeNames = configurationHelper.GetSetting(PRODUCT.Attr_Not_For_Variant).Split(',').ToList();
            this.ExcludeFromSimpleAttributeNames = configurationHelper.GetSetting(PRODUCT.Attr_Not_For_Simple).Split(',').ToList();
        }

        #endregion

        protected int GetCustomAttributeIntegerValue(List<KeyValuePair<string, string>> customAttributes, string attribute)
        {
            int value = 0;
            string stringValue = this.GetCustomAttributeValue(customAttributes, attribute);
            if(!string.IsNullOrWhiteSpace(stringValue))
            {
                value = Convert.ToInt32(stringValue);
            }

            return value;
        }

        protected bool GetCustomAttributeBooleanValue(List<KeyValuePair<string, string>> customAttributes, string attribute)
        {
            bool value = false;
            string stringValue = this.GetCustomAttributeValue(customAttributes, attribute);
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                value = Convert.ToBoolean(stringValue);
            }

            return value;
        }

        protected string GetCustomAttributeValue(List<KeyValuePair<string, string>> customAttributes, string attribute)
        {
            string value = string.Empty;
            
            if(customAttributes!=null && customAttributes.Count > 0)
            {
                KeyValuePair<string, string> item = customAttributes.FirstOrDefault(att => att.Key.Equals(attribute));
                if(!string.IsNullOrWhiteSpace(item.Key))
                {
                    value = item.Value;
                }
            }
            return value;
        }
    }
}
