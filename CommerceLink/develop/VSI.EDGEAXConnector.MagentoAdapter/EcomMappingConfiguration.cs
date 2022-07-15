using AutoMapper;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.MagentoAPI.MageAPI;

namespace VSI.EDGEAXConnector.MagentoAdapter
{

    /// <summary>
    /// Mapper Class
    /// </summary>
    public class EcomMappingConfiguration : Profile
    {

        #region Protected Methods

        /// <summary>
        /// This functions used for mapping.
        /// </summary>
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<EComCategory, catalogCategoryEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>();
            AutoMapper.Mapper.CreateMap<EcomapiEntity, apiEntity>();
            AutoMapper.Mapper.CreateMap<EcomapiMethodEntity, apiMethodEntity>();
            AutoMapper.Mapper.CreateMap<EcomassociativeEntity, associativeEntity>();
            AutoMapper.Mapper.CreateMap<EcomassociativeMultiEntity, associativeMultiEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogAssignedProduct, catalogAssignedProduct>();
            AutoMapper.Mapper.CreateMap<EcomcatalogAttributeEntity, catalogAttributeEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogAttributeOptionEntity, catalogAttributeOptionEntity>();
            //Mapper.CreateMap<EcomcatalogCategoryAttributeCurrentStoreRequest, catalogCategoryAttributeCurrentStoreRequest>();
            //Mapper.CreateMap<EcomcatalogCategoryAttributeCurrentStoreResponse, catalogCategoryAttributeCurrentStoreResponse>();
            //Mapper.CreateMap<EcomcatalogCategoryCurrentStoreRequest, catalogCategoryCurrentStoreRequest>();
            //Mapper.CreateMap<EcomcatalogCategoryCurrentStoreResponse, catalogCategoryCurrentStoreResponse>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryEntity, catalogCategoryEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryEntityNoChildren, catalogCategoryEntityNoChildren>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryInfo, catalogCategoryInfo>();
            AutoMapper.Mapper.CreateMap<EcomcatalogCategoryTree, catalogCategoryTree>();
            AutoMapper.Mapper.CreateMap<EcomcatalogInventoryStockItemEntity, catalogInventoryStockItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogInventoryStockItemUpdateEntity, catalogInventoryStockItemUpdateEntity>();
            //Mapper.CreateMap<EcomcatalogProductAdditionalAttributesEntity, catalogProductAdditionalAttributesEntity>();
            //Mapper.CreateMap<EcomcatalogProductAttributeCurrentStoreRequest, catalogProductAttributeCurrentStoreRequest>();
            //Mapper.CreateMap<EcomcatalogProductAttributeCurrentStoreResponse, catalogProductAttributeCurrentStoreResponse>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeEntity, catalogProductAttributeEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeEntityToCreate, catalogProductAttributeEntityToCreate>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeEntityToUpdate, catalogProductAttributeEntityToUpdate>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeFrontendLabelEntity, catalogProductAttributeFrontendLabelEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeMediaCreateEntity, catalogProductAttributeMediaCreateEntity>();
            //Mapper.CreateMap<EcomcatalogProductAttributeMediaCurrentStoreRequest, catalogProductAttributeMediaCurrentStoreRequest>();
            //Mapper.CreateMap<EcomcatalogProductAttributeMediaCurrentStoreResponse, catalogProductAttributeMediaCurrentStoreResponse>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeMediaTypeEntity, catalogProductAttributeMediaTypeEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeOptionEntityToAdd, catalogProductAttributeOptionEntityToAdd>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeOptionLabelEntity, catalogProductAttributeOptionLabelEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductAttributeSetEntity, catalogProductAttributeSetEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCreateEntity, catalogProductCreateEntity>();
            //Mapper.CreateMap<EcomcatalogProductCurrentStoreRequest, catalogProductCurrentStoreRequest>();
            // Mapper.CreateMap<EcomcatalogProductCurrentStoreResponse, catalogProductCurrentStoreResponse>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionAdditionalFieldsEntity, catalogProductCustomOptionAdditionalFieldsEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionInfoEntity, catalogProductCustomOptionInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionListEntity, catalogProductCustomOptionListEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionToAdd, catalogProductCustomOptionToAdd>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionToUpdate, catalogProductCustomOptionToUpdate>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionTypesEntity, catalogProductCustomOptionTypesEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionValueAddEntity, catalogProductCustomOptionValueAddEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionValueInfoEntity, catalogProductCustomOptionValueInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionValueListEntity, catalogProductCustomOptionValueListEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductCustomOptionValueUpdateEntity, catalogProductCustomOptionValueUpdateEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkAddEntity, catalogProductDownloadableLinkAddEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkAddSampleEntity, catalogProductDownloadableLinkAddSampleEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkEntity, catalogProductDownloadableLinkEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkFileEntity, catalogProductDownloadableLinkFileEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkFileInfoEntity, catalogProductDownloadableLinkFileInfoEntity>();
            //Mapper.CreateMap<EcomcatalogProductDownloadableLinkInfoEntity, catalogProductDownloadableLinkInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductDownloadableLinkSampleEntity, catalogProductDownloadableLinkSampleEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductEntity, catalogProductEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductImageEntity, catalogProductImageEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductImageFileEntity, catalogProductImageFileEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductLinkAttributeEntity, catalogProductLinkAttributeEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductLinkEntity, catalogProductLinkEntity>();
            //Mapper.CreateMap<EcomcatalogProductListRequest, catalogProductListRequest>();
            //Mapper.CreateMap<EcomcatalogProductListResponse, catalogProductListResponse>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductRequestAttributes, catalogProductRequestAttributes>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductReturnEntity, catalogProductReturnEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductSpecialPriceReturnEntity, catalogProductSpecialPriceReturnEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTagAddEntity, catalogProductTagAddEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTagInfoEntity, catalogProductTagInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTagListEntity, catalogProductTagListEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTagUpdateEntity, catalogProductTagUpdateEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTierPriceEntity, catalogProductTierPriceEntity>();
            AutoMapper.Mapper.CreateMap<EcomcatalogProductTypeEntity, catalogProductTypeEntity>();
            AutoMapper.Mapper.CreateMap<EcomcomplexFilter, complexFilter>();
            AutoMapper.Mapper.CreateMap<EcomcustomerAddressEntityCreate, customerAddressEntityCreate>();
            AutoMapper.Mapper.CreateMap<EcomcustomerAddressEntityItem, customerAddressEntityItem>();
            AutoMapper.Mapper.CreateMap<EcomcustomerCustomerEntity, customerCustomerEntity>();
            AutoMapper.Mapper.CreateMap<EcomcustomerCustomerEntityToCreate, customerCustomerEntityToCreate>();
            AutoMapper.Mapper.CreateMap<EcomcustomerGroupEntity, customerGroupEntity>();
            AutoMapper.Mapper.CreateMap<EcomdirectoryCountryEntity, directoryCountryEntity>();
            AutoMapper.Mapper.CreateMap<EcomdirectoryRegionEntity, directoryRegionEntity>();
            AutoMapper.Mapper.CreateMap<EcomexistsFaltureEntity, existsFaltureEntity>();
            AutoMapper.Mapper.CreateMap<Ecomfilters, filters>();
            AutoMapper.Mapper.CreateMap<EcomgiftMessageAssociativeProductsEntity, giftMessageAssociativeProductsEntity>();
            AutoMapper.Mapper.CreateMap<EcomgiftMessageEntity, giftMessageEntity>();
            AutoMapper.Mapper.CreateMap<EcomgiftMessageResponse, giftMessageResponse>();
            AutoMapper.Mapper.CreateMap<EcommagentoInfoEntity, magentoInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomorderItemIdQty, orderItemIdQty>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderAddressEntity, salesOrderAddressEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderCreditmemoCommentEntity, salesOrderCreditmemoCommentEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderCreditmemoData, salesOrderCreditmemoData>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderCreditmemoEntity, salesOrderCreditmemoEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderCreditmemoItemEntity, salesOrderCreditmemoItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderEntity, salesOrderEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderInvoiceCommentEntity, salesOrderInvoiceCommentEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderInvoiceEntity, salesOrderInvoiceEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderInvoiceItemEntity, salesOrderInvoiceItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderItemEntity, salesOrderItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderListEntity, salesOrderListEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderPaymentEntity, salesOrderPaymentEntity>();
            //AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentAddCommentRequest, salesOrderShipmentAddCommentRequest>();
            //AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentAddCommentResponse, salesOrderShipmentAddCommentResponse>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentCommentEntity, salesOrderShipmentCommentEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentEntity, salesOrderShipmentEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentItemEntity, salesOrderShipmentItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderShipmentTrackEntity, salesOrderShipmentTrackEntity>();
            AutoMapper.Mapper.CreateMap<EcomsalesOrderStatusHistoryEntity, salesOrderStatusHistoryEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartAddressEntity, shoppingCartAddressEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartCustomerAddressEntity, shoppingCartCustomerAddressEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartCustomerEntity, shoppingCartCustomerEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartInfoEntity, shoppingCartInfoEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartItemEntity, shoppingCartItemEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartLicenseEntity, shoppingCartLicenseEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartPaymentEntity, shoppingCartPaymentEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartPaymentMethodEntity, shoppingCartPaymentMethodEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartPaymentMethodResponseEntity, shoppingCartPaymentMethodResponseEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartProductEntity, shoppingCartProductEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartShippingMethodEntity, shoppingCartShippingMethodEntity>();
            AutoMapper.Mapper.CreateMap<EcomshoppingCartTotalsEntity, shoppingCartTotalsEntity>();
            AutoMapper.Mapper.CreateMap<EcomstoreEntity, storeEntity>();

        }

        #endregion
    }
}
