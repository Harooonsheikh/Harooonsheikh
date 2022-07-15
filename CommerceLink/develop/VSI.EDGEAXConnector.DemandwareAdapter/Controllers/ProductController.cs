

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware.Catalog;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// ProductController class performs Product related activities.
    /// </summary>
    public class ProductController : ProductBaseController, IProductController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductController()
            : base(true)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento.
        /// </summary>
        /// <param name="products"></param>
        public void PushProducts(ErpCatalog catalog)
        {
            try
            {
                if (catalog != null && catalog.Products.Count > 0)
                {
                   // this.ProcessProductCategories(ecomCategories, products);
                   // this.CreateProducts(ecomCategories, products);
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
        }

        /// <summary>
        /// PushProductImages push product images to Magento
        /// </summary>
        /// <param name="images"></param>
        public void PushProductImages(List<KeyValuePair<string, string>> images)
        {
            throw new NotImplementedException("VSI.EDGEAXConnector.DemandwareAdapter.Controllers.ProductController.PushProductImages");
        }

        #endregion

        #region Private Methods


        private complexTypeCategory[] IntializeCatalogCategory(List<EcomcatalogCategoryEntityCreate> ecomCategories)
        {
            complexTypeCategory category;
            List<complexTypeCategory> categories = new List<complexTypeCategory>();

            foreach (EcomcatalogCategoryEntityCreate eCat in ecomCategories)
            {
                category = new complexTypeCategory();
                category.categoryid = eCat.categoryId.ToString();
                category.displayname = new sharedTypeLocalizedString[1];
                category.displayname[0] = new sharedTypeLocalizedString();
                category.displayname[0].Value = eCat.name;
                category.onlineflag = Convert.ToBoolean(eCat.is_active);
                category.onlineflagSpecified = true;
                category.sitemapincludedflag = Convert.ToBoolean(eCat.is_active);
                category.sitemapincludedflagSpecified = true;


                if (!string.IsNullOrWhiteSpace(eCat.parentCategoryId.ToString()))
                {
                    category.parent = eCat.parentCategoryId.ToString();

                    List<sharedTypeCustomAttribute> customAttributes = new List<sharedTypeCustomAttribute>();
                    sharedTypeCustomAttribute customAttribute = new sharedTypeCustomAttribute();
                    customAttribute.attributeid = "showInMenu";
                    customAttribute.Text = new string[] { Convert.ToBoolean(eCat.is_active).ToString().ToLower() };
                    customAttributes.Add(customAttribute);
                    category.customattributes = customAttributes.ToArray();
                }

                //??category.attributegroups
                categories.Add(category);
            }

            return categories.ToArray();
        }

        private complexTypeProduct InitializeCatalogProductVariant(EcomcatalogProductCreateEntity ecomProduct)
        {
            complexTypeProduct product = new complexTypeProduct();

            product.productid = ecomProduct.SKU;
            product.upc = ecomProduct.Barcode;
            product.minorderquantity = base.GetCustomAttributeIntegerValue(ecomProduct.CustomAttributes, "MinOrderQuantity");
            product.minorderquantitySpecified = true;
            product.stepquantity = base.GetCustomAttributeIntegerValue(ecomProduct.CustomAttributes, "StepQuantity");
            product.stepquantitySpecified = true;
            product.onlineflag = new sharedTypeSiteSpecificBoolean[1];
            product.onlineflag[0] = new sharedTypeSiteSpecificBoolean();
            product.onlineflag[0].Value = base.GetCustomAttributeBooleanValue(ecomProduct.CustomAttributes, "OnlineStatus");
            product.availableflag = true; //TODO: Check it
            product.availableflagSpecified = true;
            product.searchableflag = new sharedTypeSiteSpecificBoolean[1];
            product.searchableflag[0] = new sharedTypeSiteSpecificBoolean();
            product.searchableflag[0].Value = true;
            product.taxclassid = "standard"; //base.GetCustomAttributeValue(ecomProduct.CustomAttributes, "TaxClassId");

            product.customattributes = this.InitializeCatalogProductCustomAttributes(ecomProduct);

            return product;
        }

        private sharedTypeSiteSpecificCustomAttribute[] InitializeCatalogProductCustomAttributes(EcomcatalogProductCreateEntity ecomProduct)
        {
            List<sharedTypeSiteSpecificCustomAttribute> customAttributes = new List<sharedTypeSiteSpecificCustomAttribute>();

            this.InitializeCatalogProductCustomAttribute(customAttributes, "color", ecomProduct.ColorId);
            this.InitializeCatalogProductCustomAttribute(customAttributes, "refinementColor", ecomProduct.ColorId);
            this.InitializeCatalogProductCustomAttribute(customAttributes, "size", ecomProduct.SizeId);
            this.InitializeCatalogProductCustomAttribute(customAttributes, "styleNumber", ecomProduct.StyleId);
            this.InitializeCatalogProductCustomAttribute(customAttributes, "config", ecomProduct.ConfigId);

            return customAttributes.ToArray();
        }

        private sharedTypeSiteSpecificCustomAttribute InitializeCatalogProductCustomAttribute(List<sharedTypeSiteSpecificCustomAttribute> customAttributes, string attributeKey, string attributeValue)
        {
            sharedTypeSiteSpecificCustomAttribute customAttribute = null;
            if (!string.IsNullOrWhiteSpace(attributeValue)) //?? attributeId or attributeValue
            {
                customAttribute = new sharedTypeSiteSpecificCustomAttribute();
                customAttribute.attributeid = attributeKey;
                customAttribute.Text = new string[] { attributeValue };
                //product.customattributes[1] = customAttribute;
                customAttributes.Add(customAttribute);
            }
            return customAttribute;
        }

        private complexTypeProduct InitializeCatalogProductMaster(EcomcatalogProductCreateEntity ecomMasterProduct, List<EcomcatalogProductCreateEntity> ecomProducts)
        {
            complexTypeProduct product = new complexTypeProduct();
            product.productid = ecomMasterProduct.SKU;
            //product.upc = ecomProduct.Barcode;
            product.minorderquantity = base.GetCustomAttributeIntegerValue(ecomMasterProduct.CustomAttributes, "MinOrderQuantity");
            product.minorderquantitySpecified = true;
            product.stepquantity = base.GetCustomAttributeIntegerValue(ecomMasterProduct.CustomAttributes, "StepQuantity");
            product.stepquantitySpecified = true;
            product.onlineflag = new sharedTypeSiteSpecificBoolean[1];
            product.onlineflag[0] = new sharedTypeSiteSpecificBoolean();
            product.onlineflag[0].Value = base.GetCustomAttributeBooleanValue(ecomMasterProduct.CustomAttributes, "OnlineStatus");
            product.availableflag = true; //TODO: Check it
            product.availableflagSpecified = true;
            product.searchableflag = new sharedTypeSiteSpecificBoolean[1];
            product.searchableflag[0] = new sharedTypeSiteSpecificBoolean();
            product.searchableflag[0].Value = true;
            product.taxclassid = "standard"; //base.GetCustomAttributeValue(ecomMasterProduct.CustomAttributes, "TaxClassId");

            product.displayname = new sharedTypeLocalizedString[1];
            product.displayname[0] = new sharedTypeLocalizedString();
            product.displayname[0].Value = ecomMasterProduct.name;
            product.shortdescription = new sharedTypeLocalizedText[1];
            product.shortdescription[0] = new sharedTypeLocalizedText();
            product.shortdescription[0].Value = ecomMasterProduct.short_description;
            product.longdescription = new sharedTypeLocalizedText[1];
            product.longdescription[0] = new sharedTypeLocalizedText();
            product.longdescription[0].Value = ecomMasterProduct.description;

            product.sitemapincludedflag = new sharedTypeSiteSpecificBoolean[1];
            product.sitemapincludedflag[0] = new sharedTypeSiteSpecificBoolean();
            product.sitemapincludedflag[0].Value = true;
            product.sitemapchangefrequency = new sharedTypeSiteSpecificSiteMapChangeFrequency[1];
            product.sitemapchangefrequency[0] = new sharedTypeSiteSpecificSiteMapChangeFrequency();
            product.sitemapchangefrequency[0].Value = simpleTypeSiteMapChangeFrequency.daily;
            product.sitemappriority = new sharedTypeSiteSpecificSiteMapPriority[1];
            product.sitemappriority[0] = new sharedTypeSiteSpecificSiteMapPriority();
            product.sitemappriority[0].Value = 1;

            complexTypePageAttributes pageAttributes = new complexTypePageAttributes();
            pageAttributes.pagetitle = new sharedTypeLocalizedString[1];
            pageAttributes.pagetitle[0] = new sharedTypeLocalizedString();
            pageAttributes.pagetitle[0].Value = ecomMasterProduct.name;
            pageAttributes.pagedescription = new sharedTypeLocalizedString[1];
            pageAttributes.pagedescription[0] = new sharedTypeLocalizedString();
            pageAttributes.pagedescription[0].Value = ecomMasterProduct.description;

            product.pageattributes = pageAttributes;

            product.variations = new complexTypeProductVariations();
            product.variations.attributes = this.InitializeCatalogProductVariationAttributes(ecomMasterProduct, ecomProducts);
            product.variations.variants = this.InitializeCatalogProductVariationsVariants(ecomMasterProduct, ecomProducts);

            product.images = this.InitializeCatalogProductImage(ecomMasterProduct);

            product.customattributes = new sharedTypeSiteSpecificCustomAttribute[1];
            product.customattributes[0] = new sharedTypeSiteSpecificCustomAttribute();
            product.customattributes[0].attributeid = "availableForInStorePickup";
            product.customattributes[0].Text = new string[] { "true" };


            return product;
        }

        private complexTypeVariationAttribute[] InitializeCatalogProductVariationAttributes(EcomcatalogProductCreateEntity ecomMasterProduct, List<EcomcatalogProductCreateEntity> ecomProducts)
        {
            List<complexTypeVariationAttribute> variationAttributes = new List<complexTypeVariationAttribute>();

            var attributeColors = ecomProducts.Where(p => !string.IsNullOrWhiteSpace(p.Color)).Select(p => new Tuple<string, string>(p.ColorId, p.Color)).Distinct().ToList();
            this.InitializeCatalogProductVariationAttribute(variationAttributes, attributeColors, "color");

            var attributeSizes = ecomProducts.Where(p => !string.IsNullOrWhiteSpace(p.Size)).Select(p => new Tuple<string, string>(p.SizeId, p.Size)).Distinct().ToList();
            this.InitializeCatalogProductVariationAttribute(variationAttributes, attributeSizes, "size");

            var attributeStyles = ecomProducts.Where(p => !string.IsNullOrWhiteSpace(p.Style)).Select(p => new Tuple<string, string>(p.StyleId, p.Style)).Distinct().ToList();
            this.InitializeCatalogProductVariationAttribute(variationAttributes, attributeStyles, "styleNumber");

            var attributeConfigs = ecomProducts.Where(p => !string.IsNullOrWhiteSpace(p.Configuration)).Select(p => new Tuple<string, string>(p.ConfigId, p.Configuration)).Distinct().ToList();
            this.InitializeCatalogProductVariationAttribute(variationAttributes, attributeConfigs, "config");

            return variationAttributes.ToArray();
        }

        private complexTypeVariationAttribute InitializeCatalogProductVariationAttribute(List<complexTypeVariationAttribute> variationAttributes, List<Tuple<string, string>> attributeItems, string attributeKey)
        {
            complexTypeVariationAttribute variationAttribute = null;
            if (attributeItems.Count > 0)
            {
                variationAttribute = new complexTypeVariationAttribute();
                variationAttribute.attributeid = variationAttribute.variationattributeid = attributeKey;
                //List<complexTypeVariationAttributeValues> variationAttributeValues = new List<complexTypeVariationAttributeValues>();
                complexTypeVariationAttributeValues variationAttributeValue = new complexTypeVariationAttributeValues();
                List<complexTypeVariationAttributeValue> attributeValues = new List<complexTypeVariationAttributeValue>();
                complexTypeVariationAttributeValue attributeValue;


                for (int i = 0; i < attributeItems.Count(); i++)
                {
                    //attributeValue = new complexTypeVariationAttributeValues();
                    attributeValue = new complexTypeVariationAttributeValue();
                    attributeValue.value = attributeItems[i].Item1;
                    attributeValue.displayvalue = new sharedTypeLocalizedString[1];
                    attributeValue.displayvalue[0] = new sharedTypeLocalizedString();
                    attributeValue.displayvalue[0].Value = attributeItems[i].Item2;

                    attributeValues.Add(attributeValue);
                }

                variationAttributeValue.variationattributevalue = attributeValues.ToArray();
                variationAttribute.variationattributevalues = new complexTypeVariationAttributeValues[1];
                variationAttribute.variationattributevalues[0] = variationAttributeValue;
                variationAttributes.Add(variationAttribute);
            }

            return variationAttribute;
        }

        private complexTypeProductVariationsVariants[] InitializeCatalogProductVariationsVariants(EcomcatalogProductCreateEntity ecomMasterProduct, List<EcomcatalogProductCreateEntity> ecomProducts)
        {
            List<complexTypeProductVariationsVariants> varients = new List<complexTypeProductVariationsVariants>();
            if (ecomProducts.Count > 0)
            {
                complexTypeProductVariationsVariants varient = new complexTypeProductVariationsVariants();
                List<complexTypeProductVariationsVariant> variantItems = new List<complexTypeProductVariationsVariant>();
                complexTypeProductVariationsVariant vItem;
                for (int i = 0; i < ecomProducts.Count; i++)
                {
                    vItem = new complexTypeProductVariationsVariant();
                    vItem.productid = ecomProducts[i].SKU;

                    variantItems.Add(vItem);
                }
                varient.variant = variantItems.ToArray();
                varients.Add(varient);
            }
            return varients.ToArray();
        }

        /// <summary>
        /// FormatProductImageCSVDataExt format and arrange Product Images as per requirements for CSV Export.
        /// </summary>
        /// <param name="ecoProduct"></param>
        /// <param name="productcsv"></param>
        /// <param name="csvProductImages"></param>
        private complexTypeProductImages InitializeCatalogProductImage(EcomcatalogProductCreateEntity ecoProduct)
        {
            complexTypeProductImages productImage = new complexTypeProductImages();
            complexTypeProductImage image;
            if (ecoProduct.Images != null && ecoProduct.Images.Count > 0)
            {
                productImage.imagegroup = new complexTypeProductImageGroup[3];
                complexTypeProductImageGroup imageGroup = new complexTypeProductImageGroup();
                imageGroup.viewtype = "medium";

                List<complexTypeProductImage> images = new List<complexTypeProductImage>();

                foreach (EcomcatalogProductImageEntity img in ecoProduct.Images)
                {
                    image = new complexTypeProductImage();
                    image.path = img.file; //string.Format("medium/{0}", img.file);
                    images.Add(image);
                }

                imageGroup.image = images.ToArray();
                productImage.imagegroup[0] = imageGroup;

                imageGroup = new complexTypeProductImageGroup();
                imageGroup.viewtype = "small";
                imageGroup.image = images.ToArray();
                productImage.imagegroup[1] = imageGroup;

                imageGroup = new complexTypeProductImageGroup();
                imageGroup.viewtype = "large";
                imageGroup.image = images.ToArray();
                productImage.imagegroup[2] = imageGroup;
            }
            return productImage;

            //TODO: Image Item processing has to be finalized, it is in test mode with assumption there are 3 images having back and front tag.
            //foreach (var image in ecoProduct.Images)
            //{
            //    if (image.url.Contains(ConfigurationHelper.ProductBackImageFileTag))
            //    {
            //        productImageCSV.catalog_image_back = image.url;
            //    }
            //    else if (image.url.Contains(ConfigurationHelper.ProductFrontImageFileTag))
            //    {
            //        productImageCSV.catalog_image_front = image.url;
            //    }
            //    else if (image.url.Contains(ConfigurationHelper.ProductSwatchImageFileTag))
            //    {
            //        productImageCSV.color_swatch = image.url;
            //    }
            //    else if (image.url.Contains(ConfigurationHelper.ProductThumbnailImageFileTag))
            //    {
            //        productImageCSV.thumbnail = image.url;
            //    }
            //    else if (image.url.Contains(ConfigurationHelper.ProductSmallImageFileTag))
            //    {
            //        productImageCSV.small_image = image.url;
            //    }
            //    else
            //    {
            //        productImageCSV.image = image.url;
            //    }
            //}
        }


        /// <summary>
        /// CreateProductCSVFile create product csv.
        /// </summary>
        /// <param name="catalogData"></param>
        private void CreateProductFile(catalog catalogData)
        {
            if (catalogData != null)
            {
                //string fileName = FileHelper.GetSalesOrderFileName();
                //XmlSerializer serializer = new XmlSerializer(typeof(VSI.EDGEAXConnector.Demandware.Order.orders));

                //using (StreamReader reader = new StreamReader(fileName))
                //{
                //    VSI.EDGEAXConnector.Demandware.Order.orders salesOrders = (VSI.EDGEAXConnector.Demandware.Order.orders)serializer.Deserialize(reader);
                //}

                string fileName = FileHelper.GetProductCSVFileName();
                var serializer = new XmlSerializer(typeof(catalog));
                using (var stream = new StreamWriter(fileName))
                {
                    serializer.Serialize(stream, catalogData);
                }

                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.ProductSync, "Product Sync CSV generated Successfully", DateTime.UtcNow, null);
            }
        }
        #endregion
    }
}
