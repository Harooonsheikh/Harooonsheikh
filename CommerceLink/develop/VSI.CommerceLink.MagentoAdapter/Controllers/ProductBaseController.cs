using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{

    /// <summary>
    /// ProductBaseController class performs Product related activities.
    /// </summary>
    public class ProductBaseController : BaseController
    {
        //#region Properties

        ///// <summary>
        ///// It excludes from variant attributes names
        ///// </summary>
        //private List<string> ExcludeFromVaiantAttributeNames { get; set; } //= new List<string> { "configurable_attributes", "details", "size_fit", "product_details" };

        ///// <summary>
        ///// It excludes from simple attributes names
        ///// </summary>
        //private List<string> ExcludeFromSimpleAttributeNames { get; set; }

        //#endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
     
        public ProductBaseController(bool initializeService, string storeKey)
            : base(initializeService, storeKey)
        {
            //this.LoadExcludeAttributeNames();
        }

        #endregion



        #region Private Methods

        /// <summary>
        /// PrefixSKU concatenates prefix with SKU
        /// </summary>
        /// <param name="sku">SKU of Product</param>
        /// <returns></returns>
        public string PrefixSKU(string sku)
        {
            return configurationHelper.GetSetting(PRODUCT.SKU_Prefix) + sku;
        }

        public string PostfixSKU(string sku)
        {
            return sku + configurationHelper.GetSetting(PRODUCT.SKU_Postfix);
        }

        //#region CSV Methods

        ///// <summary>
        ///// FormatProductCSVData format and arrange Products as per requirements for CSV Export.
        ///// </summary>
        ///// <param name="products"></param>
        ///// <returns></returns>
        //protected List<ProductCSV> ProcessProductCSVData(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<ProductCSV> csvProducts = new List<ProductCSV>();
        //    foreach (var p in products)
        //    {
        //        //csvProducts.AddRange(this.ConvertProduct(p));
        //        if (p._type.ToLower().StartsWith("config"))
        //        {
        //            List<EcomcatalogProductCreateEntity> pVariants = products.Where(m => m.MasterProductSKU.Equals(p.SKU)).ToList();

        //            if (pVariants.Any())
        //            {
        //                p.associated = string.Join(",", pVariants.Select(v => v.SKU).ToList());
        //                p.config_attributes = this.GetConfigAttributes(pVariants.First());
        //            }
        //        }
        //        //else if (p._type.ToLower().StartsWith("group"))
        //        //{
        //        //    //List<ProductCSV> pVariants = GetProductVariants(products, p.SKU);
        //        //    //csvProducts.AddRange(pVariants);
        //        //}

        //        csvProducts.Add(this.CreateProductCSV(p, p._type.Equals("simple", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrWhiteSpace(p.MasterProductSKU)));

        //    }
        //    return csvProducts;
        //}

        ///// <summary>
        ///// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        ///// </summary>
        ///// <param name="products"></param>
        ///// <param name="csvProducts"></param>
        ///// <param name="csvRelatedProducts"></param>
        ///// <param name="csvProductImages"></param>
        //protected void ProcessProductCSVDataExt(List<EcomcatalogProductCreateEntity> products, List<ProductCSV> csvProducts, List<ProductImageCSV> csvProductImages)//List<RelatedProductCSV> csvRelatedProducts, 
        //{
        //    string variantColour = "";
        //    int index = 0;
        //    EcomcatalogProductCreateEntity prevProduct = null;
        //    List<string> categoryIds = new List<string>();
        //    List<string> associatedSKUs = new List<string>();
        //    List<string> newConfigurableSkus = new List<string>();
        //    ProductCSV convertedProduct;

        //    if (products.Any())
        //    {
        //        variantColour = products[0].Color;

        //        foreach (var p in products)
        //        {
        //            try
        //            {
        //                //Simple Prodct without Variant
        //                if (p._type.Equals("simple", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrWhiteSpace(p.MasterProductSKU))
        //                {
        //                    convertedProduct = this.CreateProductCSVDataSimpleExt(p, csvProducts, associatedSKUs, categoryIds, false);
        //                    this.ProcessProductImageCSVDataExt(p, convertedProduct, csvProductImages);

        //                    if (index < products.Count - 1)
        //                    {
        //                        variantColour = products[index + 1].Color;
        //                    }
        //                }
        //                else//Master Product with variants
        //                {
        //                    if (p._type.Equals("configurable", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        if (associatedSKUs != null && associatedSKUs.Count() > 0)
        //                        {
        //                            convertedProduct = this.CreateProductCSVDataConfigurabaleExt(csvProducts, prevProduct, associatedSKUs, newConfigurableSkus, categoryIds);
        //                            this.ProcessProductImageCSVDataExt(prevProduct, convertedProduct, csvProductImages);
        //                            //this.ProcessProductCSVDataRelatedExt(csvRelatedProducts, newConfigurableSkus);
        //                            //csvRelatedProducts.Add(new RelatedProductCSV { sku = p.SKU, xre_skus = string.Join(",", newConfigurableSkus) });
        //                            //newConfigurableSkus.Clear(); //= new List<string>();
        //                            if (index < products.Count - 1)
        //                            {
        //                                variantColour = products[index + 1].Color;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!variantColour.Equals(p.Color))
        //                        {
        //                            convertedProduct = this.CreateProductCSVDataConfigurabaleExt(csvProducts, prevProduct, associatedSKUs, newConfigurableSkus, categoryIds);
        //                            this.ProcessProductImageCSVDataExt(prevProduct, convertedProduct, csvProductImages);
        //                            variantColour = p.Color;
        //                        }
        //                        convertedProduct = this.CreateProductCSVDataSimpleExt(p, csvProducts, associatedSKUs, categoryIds, true);
        //                        prevProduct = p;
        //                    }
        //                }
        //                index++;
        //            }
        //            catch (Exception exp)
        //            {
        //                CustomLogger.LogException(exp);
        //                throw;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// FormatProductImageCSVDataExt format and arrange Product Images as per requirements for CSV Export.
        ///// </summary>
        ///// <param name="ecoProduct"></param>
        ///// <param name="productcsv"></param>
        ///// <param name="csvProductImages"></param>
        //private void ProcessProductImageCSVDataExt(EcomcatalogProductCreateEntity ecoProduct, ProductCSV productcsv, List<ProductImageCSV> csvProductImages)
        //{
        //    ProductImageCSV productImageCSV = new ProductImageCSV
        //    {
        //        //store = productcsv.store,
        //        //websites = productcsv.websites,
        //        sku = productcsv.sku
        //        //category_ids = productcsv.category_ids
        //    };

        //    if (ecoProduct.Images != null)
        //    {
        //        productImageCSV.media_gallery = string.Join(";", ecoProduct.Images.Select(img => string.IsNullOrWhiteSpace(img.label) ? img.url : string.Format("{0}::{1}", img.url, img.label)));
        //    }

        //    //TODO: Image Item processing has to be finalized, it is in test mode with assumption there are 3 images having back and front tag.
        //    //foreach (var image in ecoProduct.Images)
        //    //{
        //    //    if (image.url.Contains(ConfigurationHelper.ProductBackImageFileTag))
        //    //    {
        //    //        productImageCSV.catalog_image_back = image.url;
        //    //    }
        //    //    else if (image.url.Contains(ConfigurationHelper.ProductFrontImageFileTag))
        //    //    {
        //    //        productImageCSV.catalog_image_front = image.url;
        //    //    }
        //    //    else if (image.url.Contains(ConfigurationHelper.ProductSwatchImageFileTag))
        //    //    {
        //    //        productImageCSV.color_swatch = image.url;
        //    //    }
        //    //    else if (image.url.Contains(ConfigurationHelper.ProductThumbnailImageFileTag))
        //    //    {
        //    //        productImageCSV.thumbnail = image.url;
        //    //    }
        //    //    else if (image.url.Contains(ConfigurationHelper.ProductSmallImageFileTag))
        //    //    {
        //    //        productImageCSV.small_image = image.url;
        //    //    }
        //    //    else
        //    //    {
        //    //        productImageCSV.image = image.url;
        //    //    }
        //    //}

        //    csvProductImages.Add(productImageCSV);
        //}

        ///// <summary>
        ///// FormatProductCSVDataRelatedExt process and add related Product in collection.
        ///// </summary>
        ///// <param name="csvRelatedProducts"></param>
        ///// <param name="newConfigurableSkus"></param>
        //private void ProcessProductCSVDataRelatedExt(List<RelatedProductCSV> csvRelatedProducts, List<string> newConfigurableSkus)
        //{
        //    List<string> relatedSkus;
        //    if (newConfigurableSkus != null)
        //    {
        //        if (newConfigurableSkus.Count > 1)
        //        {
        //            foreach (string sku in newConfigurableSkus)
        //            {
        //                relatedSkus = newConfigurableSkus.Where(s => s != sku).ToList();
        //                csvRelatedProducts.Add(new RelatedProductCSV { sku = sku, xre_skus = string.Join(",", relatedSkus) });
        //                relatedSkus.Clear();
        //            }
        //        }
        //        newConfigurableSkus.Clear();
        //    }
        //}
        //#endregion

        //#region Data Converter

        ///// <summary>
        ///// FormatProductCSVDataSimpleExt process and add Simple Product in collection.
        ///// </summary>
        ///// <param name="ecomProduct"></param>
        ///// <param name="csvProducts"></param>
        ///// <param name="associatedSKUs"></param>
        ///// <param name="categoryIds"></param>
        ///// <param name="isVariant"></param>
        //private ProductCSV CreateProductCSVDataSimpleExt(EcomcatalogProductCreateEntity ecomProduct, List<ProductCSV> csvProducts, List<string> associatedSKUs, List<string> categoryIds, bool isVariant)
        //{
        //    if (isVariant)
        //    {
        //        if (ecomProduct.category_ids != null)
        //        {
        //            categoryIds.AddRange(ecomProduct.category_ids);
        //        }
        //        associatedSKUs.Add(ecomProduct.SKU);
        //    }

        //    ecomProduct._type = "simple";
        //    ProductCSV convertedProduct = this.CreateProductCSV(ecomProduct, isVariant);
        //    csvProducts.Add(convertedProduct);
        //    return convertedProduct;
        //}

        ///// <summary>
        ///// FormatProductCSVDataConfigurabaleExt process and add Configurable Product in collection.
        ///// </summary>
        ///// <param name="csvProducts"></param>
        ///// <param name="ecomProduct"></param>
        ///// <param name="associatedSKUs"></param>
        ///// <param name="newConfigurableSkus"></param>
        ///// <param name="categoryIds"></param>
        //private ProductCSV CreateProductCSVDataConfigurabaleExt(List<ProductCSV> csvProducts, EcomcatalogProductCreateEntity ecomProduct, List<string> associatedSKUs, List<string> newConfigurableSkus, List<string> categoryIds)
        //{
        //    //TODO: Replace this logic to get SKU from Search Name
        //    if (!string.IsNullOrWhiteSpace(ecomProduct.Size))
        //    {
        //        if (string.IsNullOrWhiteSpace(ecomProduct.SKU))
        //        {
        //            ecomProduct.SKU = ecomProduct.ItemId;
        //        }
        //        else
        //        {
        //            ecomProduct.SKU = ecomProduct.SKU
        //                    .Replace("-" + ecomProduct.Size.Replace(" ", string.Empty), string.Empty)
        //                    .Replace("_" + ecomProduct.Size.Replace(" ", string.Empty), string.Empty)
        //                    .Trim('-').Trim('_');
        //        }
        //    }

        //    ecomProduct._type = "configurable";
        //    ecomProduct.category_ids = categoryIds.Distinct().ToArray();
        //    ecomProduct.associated = string.Join(",", associatedSKUs);
        //    ecomProduct.Barcode = string.Empty;

        //    ProductCSV convertedProduct = this.CreateProductCSV(ecomProduct, false);
        //    csvProducts.Add(convertedProduct);

        //    newConfigurableSkus.Add(ecomProduct.SKU);
        //    associatedSKUs.Clear();
        //    categoryIds.Clear();

        //    return convertedProduct;
        //}

        ///// <summary>
        ///// Convert a EComProduct class into ProductCSV class to be written into CSV file.
        ///// It creates mutiple rows for single product if the product is listed in multiple categories.
        ///// </summary>
        ///// <param name="ecomProduct"></param>
        ///// <param name="isVariant"></param>
        ///// <returns></returns>
        //private ProductCSV CreateProductCSV(EcomcatalogProductCreateEntity ecomProduct, bool isVariant)
        //{
        //    bool isConfigurable = ecomProduct._type == "configurable";
        //    var productCSV = new ProductCSV();
        //    if (isConfigurable || isVariant == false)
        //    {
        //        productCSV.category_ids = ecomProduct.category_ids == null ? string.Empty : string.Join(",", ecomProduct.category_ids).Trim().Trim(',');
        //        //productCSV.CustomAttributes = ecomProduct.CustomAttributes;
        //        productCSV.visibility = "Catalog, Search";
        //        productCSV.description = ecomProduct.description;
        //        if (!isVariant)
        //        {
        //            productCSV.size = string.Empty;
        //        }
        //    }
        //    else
        //    {
        //        productCSV.category_ids = string.Empty;
        //        //productCSV.CustomAttributes = this.ProcessCustomAttributes(ecomProduct.CustomAttributes);
        //        productCSV.visibility = "Not Visible Individually";
        //        productCSV.description = ecomProduct.name;
        //        productCSV.size = string.IsNullOrWhiteSpace(ecomProduct.Size) ? string.Empty : ecomProduct.Size;
        //    }
        //    productCSV.CustomAttributes = this.ProcessCustomAttributes(ecomProduct.CustomAttributes, isConfigurable, isVariant);

        //    productCSV.sku = ecomProduct.SKU;
        //    productCSV.name = ecomProduct.name;
        //    productCSV.short_description = ecomProduct.short_description;
        //    productCSV.type = ecomProduct._type;
        //    //productCSV.url_key = productCSV.url_path = this.CreateUrlKey(ecomProduct);
        //    productCSV.upc = ecomProduct.Barcode;
        //    productCSV.price = string.IsNullOrWhiteSpace(ecomProduct.price) ? string.Empty : ecomProduct.price;
        //    //productCSV.special_price = string.IsNullOrWhiteSpace(ecomProduct.special_price) ? string.Empty : ecomProduct.special_price;

        //    productCSV.color = string.IsNullOrWhiteSpace(ecomProduct.Color) ? string.Empty : ecomProduct.Color;
        //    //productCSV.upsell = string.Empty;//TODO: to be impleented in next phase left for CRP

        //    productCSV.simples_skus = ecomProduct.associated;

        //    if (ecomProduct._type == "configurable")
        //    {
        //        productCSV.qty = 0;
        //        productCSV.is_in_stock = 1;
        //    }
        //    else
        //    {
        //        productCSV.qty = ecomProduct.AvailableQuantity;
        //        productCSV.is_in_stock = ecomProduct.AvailableQuantity > 0 ? 1 : 0;
        //    }

        //    productCSV.special_price = ecomProduct.special_price;
        //    productCSV.special_from_date = ecomProduct.special_from_date;
        //    productCSV.special_to_date = ecomProduct.special_to_date;
        //    productCSV.ax_discount_code = ecomProduct.ax_discount_code;
        //    return productCSV;
        //}

        ///// <summary>
        ///// ProcessCustomAttributes creates list of custom attributes for products.
        ///// </summary>
        ///// <param name="customAttributes"></param>
        ///// <param name="isConfigurable"></param>
        ///// <param name="isVariant"></param>
        ///// <returns></returns>
        //private List<KeyValuePair<string, string>> ProcessCustomAttributes(List<KeyValuePair<string, string>> customAttributes, bool isConfigurable, bool isVariant)
        //{
        //    List<KeyValuePair<string, string>> processedCustomAttributes;
        //    if (isConfigurable)
        //    {
        //        processedCustomAttributes = customAttributes;
        //    }
        //    else
        //    {
        //        processedCustomAttributes = new List<KeyValuePair<string, string>>();
        //        if (customAttributes != null)
        //        {
        //            foreach (var att in customAttributes)
        //            {
        //                if (isVariant)
        //                {
        //                    if (this.ExcludeFromVaiantAttributeNames.Contains(att.Key, StringComparer.InvariantCultureIgnoreCase))
        //                    {
        //                        processedCustomAttributes.Add(new KeyValuePair<string, string>(att.Key, string.Empty));
        //                    }
        //                    else
        //                    {
        //                        processedCustomAttributes.Add(att);
        //                    }
        //                }
        //                else
        //                {
        //                    if (this.ExcludeFromSimpleAttributeNames.Contains(att.Key, StringComparer.InvariantCultureIgnoreCase))
        //                    {
        //                        processedCustomAttributes.Add(new KeyValuePair<string, string>(att.Key, string.Empty));
        //                    }
        //                    else
        //                    {
        //                        processedCustomAttributes.Add(att);
        //                    }
        //                }
        //            }
        //        }
        //        //var changeAttributes = customAttributes.Where(att=> this.ConfigurableAttributeNames.Contains(att.Key));
        //        //foreach(var att in changeAttributes)
        //        //{
        //        //    att.Value = string.Empty;
        //        //}
        //    }
        //    return processedCustomAttributes;
        //}

        ///// <summary>
        ///// CreateUrlKey creates URL Key.
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //private string CreateUrlKey(EcomcatalogProductCreateEntity product)
        //{
        //    string productKey = string.Format("{0}-{1}-{2}", product.name, product.Color, product.Size).ToLower().Replace(" ", "-").Replace("_", "-");
        //    //product._type.Equals("configurable") ? productKey : productKey + product.RecordId;
        //    //product.name.ToLower().Replace(" ", "-").Replace("_", "-") + "-" + product.RecordId;//product._type.Equals("configurable") ? productKey : productKey + product.RecordId;
        //    return productKey;
        //}
        //#endregion

        ///// <summary>
        ///// GetConfigAttributes gets config attribute.
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //protected string GetConfigAttributes(EcomcatalogProductCreateEntity p)
        //{
        //    List<string> configAttributes = new List<string>();

        //    if (!string.IsNullOrEmpty(p.Color))
        //    {
        //        configAttributes.Add("color");
        //    }

        //    //if (string.IsNullOrEmpty(p.Style))
        //    //{
        //    //    configAttributes.Add("width");
        //    //}

        //    if (!string.IsNullOrEmpty(p.Size))
        //    {
        //        configAttributes.Add("size");
        //    }

        //    //if (string.IsNullOrEmpty(p.Configuration))
        //    //{
        //    //    configAttributes.Add("configuration");
        //    //}

        //    return string.Join(",", configAttributes);
        //}

        ///// <summary>
        ///// LoadExcludeAttributeNames loads configurable attributes only.
        ///// </summary>
        //private void LoadExcludeAttributeNames()
        //{
        //    //string configurableAttributes = FileHelper.Read(ConfigurationHelper.ProductConfigurableAttribute);
        //    //this.ConfigurableAttributeNames = configurableAttributes.Split(',').ToList();
        //    this.ExcludeFromVaiantAttributeNames = ConfigurationHelper.ProductAttributeNotForVariant.Split(',').ToList();
        //    this.ExcludeFromSimpleAttributeNames = ConfigurationHelper.ProductAttributeNotForSimple.Split(',').ToList();
        //}

        #endregion
    }
}
