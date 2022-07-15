using System;
using Mapster;
using MapsterMapper;
using VSI.CommerceLink.MagentoAPI.MageAPI;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.CommerceLink.MagentoAdapter
{
    /// <summary>
    /// Singleton class to get instance of Mapster Mapper
    /// </summary>
    public sealed class AutoMapBootstrapper
    {
        private static readonly Lazy<IMapper>
            LazyInstance = new Lazy<IMapper>
            (() =>
            {
                var config = GetConfig();
                config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
                return new Mapper(config);
            });

        /// <summary>
        /// Gets the instance of AutoMapper
        /// </summary>
        public static IMapper MapperInstance => LazyInstance.Value;

        /// <inheritdoc />
        private AutoMapBootstrapper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private static TypeAdapterConfig GetConfig()
        {
            TypeAdapterConfig config = new TypeAdapterConfig();
            config.ForType<ErpProduct, ErpProduct>().IgnoreNullValues(true); 
            config.ForType<EComCategory, catalogCategoryEntity>();
            config.ForType<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>();
            config.ForType<EcomapiEntity, apiEntity>();
            config.ForType<EcomapiMethodEntity, apiMethodEntity>();
            config.ForType<EcomassociativeEntity, associativeEntity>();
            config.ForType<EcomassociativeMultiEntity, associativeMultiEntity>();
            config.ForType<EcomcatalogAssignedProduct, catalogAssignedProduct>();
            config.ForType<EcomcatalogAttributeEntity, catalogAttributeEntity>();
            config.ForType<EcomcatalogAttributeOptionEntity, catalogAttributeOptionEntity>();
            config.ForType<EcomcatalogCategoryEntity, catalogCategoryEntity>();
            config.ForType<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>();
            config.ForType<EcomcatalogCategoryEntityNoChildren, catalogCategoryEntityNoChildren>();
            config.ForType<EcomcatalogCategoryInfo, catalogCategoryInfo>();
            config.ForType<EcomcatalogCategoryTree, catalogCategoryTree>();
            config.ForType<EcomcatalogInventoryStockItemEntity, catalogInventoryStockItemEntity>();
            config.ForType<EcomcatalogInventoryStockItemUpdateEntity, catalogInventoryStockItemUpdateEntity>();
            config.ForType<EcomcatalogProductAttributeEntity, catalogProductAttributeEntity>();
            config.ForType<EcomcatalogProductAttributeEntityToCreate, catalogProductAttributeEntityToCreate>();
            config.ForType<EcomcatalogProductAttributeEntityToUpdate, catalogProductAttributeEntityToUpdate>();
            config.ForType<EcomcatalogProductAttributeFrontendLabelEntity, catalogProductAttributeFrontendLabelEntity>();
            config.ForType<EcomcatalogProductAttributeMediaCreateEntity, catalogProductAttributeMediaCreateEntity>();
            config.ForType<EcomcatalogProductAttributeMediaTypeEntity, catalogProductAttributeMediaTypeEntity>();
            config.ForType<EcomcatalogProductAttributeOptionEntityToAdd, catalogProductAttributeOptionEntityToAdd>();
            config.ForType<EcomcatalogProductAttributeOptionLabelEntity, catalogProductAttributeOptionLabelEntity>();
            config.ForType<EcomcatalogProductAttributeSetEntity, catalogProductAttributeSetEntity>();
            config.ForType<EcomcatalogProductCreateEntity, catalogProductCreateEntity>();
            config.ForType<EcomcatalogProductCustomOptionAdditionalFieldsEntity, catalogProductCustomOptionAdditionalFieldsEntity>();
            config.ForType<EcomcatalogProductCustomOptionInfoEntity, catalogProductCustomOptionInfoEntity>();
            config.ForType<EcomcatalogProductCustomOptionListEntity, catalogProductCustomOptionListEntity>();
            config.ForType<EcomcatalogProductCustomOptionToAdd, catalogProductCustomOptionToAdd>();
            config.ForType<EcomcatalogProductCustomOptionToUpdate, catalogProductCustomOptionToUpdate>();
            config.ForType<EcomcatalogProductCustomOptionTypesEntity, catalogProductCustomOptionTypesEntity>();
            config.ForType<EcomcatalogProductCustomOptionValueAddEntity, catalogProductCustomOptionValueAddEntity>();
            config.ForType<EcomcatalogProductCustomOptionValueInfoEntity, catalogProductCustomOptionValueInfoEntity>();
            config.ForType<EcomcatalogProductCustomOptionValueListEntity, catalogProductCustomOptionValueListEntity>();
            config.ForType<EcomcatalogProductCustomOptionValueUpdateEntity, catalogProductCustomOptionValueUpdateEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkAddEntity, catalogProductDownloadableLinkAddEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkAddSampleEntity, catalogProductDownloadableLinkAddSampleEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkEntity, catalogProductDownloadableLinkEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkFileEntity, catalogProductDownloadableLinkFileEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkFileInfoEntity, catalogProductDownloadableLinkFileInfoEntity>();
            config.ForType<EcomcatalogProductDownloadableLinkSampleEntity, catalogProductDownloadableLinkSampleEntity>();
            config.ForType<EcomcatalogProductEntity, catalogProductEntity>();
            config.ForType<EcomcatalogProductImageEntity, catalogProductImageEntity>();
            config.ForType<EcomcatalogProductImageFileEntity, catalogProductImageFileEntity>();
            config.ForType<EcomcatalogProductLinkAttributeEntity, catalogProductLinkAttributeEntity>();
            config.ForType<EcomcatalogProductLinkEntity, catalogProductLinkEntity>();
            config.ForType<EcomcatalogProductRequestAttributes, catalogProductRequestAttributes>();
            config.ForType<EcomcatalogProductReturnEntity, catalogProductReturnEntity>();
            config.ForType<EcomcatalogProductSpecialPriceReturnEntity, catalogProductSpecialPriceReturnEntity>();
            config.ForType<EcomcatalogProductTagAddEntity, catalogProductTagAddEntity>();
            config.ForType<EcomcatalogProductTagInfoEntity, catalogProductTagInfoEntity>();
            config.ForType<EcomcatalogProductTagListEntity, catalogProductTagListEntity>();
            config.ForType<EcomcatalogProductTagUpdateEntity, catalogProductTagUpdateEntity>();
            config.ForType<EcomcatalogProductTierPriceEntity, catalogProductTierPriceEntity>();
            config.ForType<EcomcatalogProductTypeEntity, catalogProductTypeEntity>();
            config.ForType<EcomcomplexFilter, complexFilter>();
            config.ForType<EcomcustomerAddressEntityCreate, customerAddressEntityCreate>();
            config.ForType<EcomcustomerAddressEntityItem, customerAddressEntityItem>();
            config.ForType<EcomcustomerCustomerEntity, customerCustomerEntity>();
            config.ForType<EcomcustomerCustomerEntityToCreate, customerCustomerEntityToCreate>();
            config.ForType<EcomcustomerGroupEntity, customerGroupEntity>();
            config.ForType<EcomdirectoryCountryEntity, directoryCountryEntity>();
            config.ForType<EcomdirectoryRegionEntity, directoryRegionEntity>();
            config.ForType<EcomexistsFaltureEntity, existsFaltureEntity>();
            config.ForType<Ecomfilters, filters>();
            config.ForType<EcomgiftMessageAssociativeProductsEntity, giftMessageAssociativeProductsEntity>();
            config.ForType<EcomgiftMessageEntity, giftMessageEntity>();
            config.ForType<EcomgiftMessageResponse, giftMessageResponse>();
            config.ForType<EcommagentoInfoEntity, magentoInfoEntity>();
            config.ForType<EcomorderItemIdQty, orderItemIdQty>();
            config.ForType<EcomsalesOrderAddressEntity, salesOrderAddressEntity>();
            config.ForType<EcomsalesOrderCreditmemoCommentEntity, salesOrderCreditmemoCommentEntity>();
            config.ForType<EcomsalesOrderCreditmemoData, salesOrderCreditmemoData>();
            config.ForType<EcomsalesOrderCreditmemoEntity, salesOrderCreditmemoEntity>();
            config.ForType<EcomsalesOrderCreditmemoItemEntity, salesOrderCreditmemoItemEntity>();
            config.ForType<EcomsalesOrderEntity, salesOrderEntity>();
            config.ForType<EcomsalesOrderInvoiceCommentEntity, salesOrderInvoiceCommentEntity>();
            config.ForType<EcomsalesOrderInvoiceEntity, salesOrderInvoiceEntity>();
            config.ForType<EcomsalesOrderInvoiceItemEntity, salesOrderInvoiceItemEntity>();
            config.ForType<EcomsalesOrderItemEntity, salesOrderItemEntity>();
            config.ForType<EcomsalesOrderListEntity, salesOrderListEntity>();
            config.ForType<EcomsalesOrderPaymentEntity, salesOrderPaymentEntity>();
            config.ForType<EcomsalesOrderShipmentCommentEntity, salesOrderShipmentCommentEntity>();
            config.ForType<EcomsalesOrderShipmentEntity, salesOrderShipmentEntity>();
            config.ForType<EcomsalesOrderShipmentItemEntity, salesOrderShipmentItemEntity>();
            config.ForType<EcomsalesOrderShipmentTrackEntity, salesOrderShipmentTrackEntity>();
            config.ForType<EcomsalesOrderStatusHistoryEntity, salesOrderStatusHistoryEntity>();
            config.ForType<EcomshoppingCartAddressEntity, shoppingCartAddressEntity>();
            config.ForType<EcomshoppingCartCustomerAddressEntity, shoppingCartCustomerAddressEntity>();
            config.ForType<EcomshoppingCartCustomerEntity, shoppingCartCustomerEntity>();
            config.ForType<EcomshoppingCartInfoEntity, shoppingCartInfoEntity>();
            config.ForType<EcomshoppingCartItemEntity, shoppingCartItemEntity>();
            config.ForType<EcomshoppingCartLicenseEntity, shoppingCartLicenseEntity>();
            config.ForType<EcomshoppingCartPaymentEntity, shoppingCartPaymentEntity>();
            config.ForType<EcomshoppingCartPaymentMethodEntity, shoppingCartPaymentMethodEntity>();
            config.ForType<EcomshoppingCartPaymentMethodResponseEntity, shoppingCartPaymentMethodResponseEntity>();
            config.ForType<EcomshoppingCartProductEntity, shoppingCartProductEntity>();
            config.ForType<EcomshoppingCartShippingMethodEntity, shoppingCartShippingMethodEntity>();
            config.ForType<EcomshoppingCartTotalsEntity, shoppingCartTotalsEntity>();
            config.ForType<EcomstoreEntity, storeEntity>();

            config.ForType<ErpProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>()
                .MapWith(a => a);

            config.ForType<ErpProductVariantDictionary, ErpProductVariantDictionary>()
                .MapWith(a => a);
            config.ForType<ErpProductToKitVariantDictionary, ErpProductToKitVariantDictionary>()
                .MapWith(a => a);
            config.ForType<ErpProductPropertySchema, ErpProductPropertySchema>()
                .MapWith(a => a);
            config.ForType<ErpProductPropertyDictionary, ErpProductPropertyDictionary>()
                .MapWith(a => a);

            config.ForType<ErpProductDimensionDictionary, ErpProductDimensionDictionary>()
                .MapWith(a => a);
            config.ForType<ErpKitVariantToComponentDictionary, ErpKitVariantToComponentDictionary>()
                .MapWith(a => a);
            config.ForType<ErpKitLineProductPropertyDictionary, ErpKitLineProductPropertyDictionary>()
                .MapWith(a => a);
            config.ForType<ErpProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>()
                .MapWith(a => a);
            config.ForType<ErpProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>()
                .MapWith(a => a);
            config.ForType<ErpProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>()
                .MapWith(a => a);
            
            return config;

        }
    }
}
