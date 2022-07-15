using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using Mapster;
using Microsoft.Dynamics.Commerce.RetailProxy;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Enums;

namespace VSI.EDGEAXConnector.CRTD365
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig GetCrtConfig()
        {
            TypeAdapterConfig config = new TypeAdapterConfig();

            //=============================
            //Mapping for RS to ERP
            //=============================
            #region Mapping for CRT to ERP
            config.ForType<ContactPerson, ErpContactPerson>();
            //config.ForType<ContactPersonResponseSet, ErpContactPerson>();
            //config.ForType<SaveContactPersonResponseSet, ErpContactPersonNAL>();
            //config.ForType<ContactPersonNALResponseSet, ErpContactPersonNAL>();

            config.ForType<CommercePropertyValue, ErpCommercePropertyValue>();
            config.ForType<CommerceProperty, ErpCommerceProperty>();
            config.ForType<ClassInfo, ClassInfo>();
            config.ForType<AddressFormatLineType, ErpAddressFormatLineType>();
            config.ForType<AddressType, ErpAddressType>();
            config.ForType<Address, ErpAddress>();
            config.ForType<AttributeDataType, ErpAttributeDataType>();
            config.ForType<AttributeGroupType, ErpAttributeGroupType>();
            config.ForType<BarcodeEntryMethodType, ErpBarcodeEntryMethodType>();
            config.ForType<BarcodeMaskType, ErpBarcodeMaskType>();
            config.ForType<CardType, ErpCardType>();
            config.ForType<CartType, ErpCartType>();
            config.ForType<CashType, ErpCashType>();
            config.ForType<ChargeMethod, ErpChargeMethod>();
            config.ForType<ChargeModule, ErpChargeModule>();
            config.ForType<ChargeType, ErpChargeType>();
            config.ForType<CommerceEntityDataLevel, ErpCommerceEntityDataLevel>();
            config.ForType<CommerceListType, ErpCommerceListType>();
            config.ForType<ConcurrencyMode, ErpConcurrencyMode>();
            config.ForType<ContactInformationType, ErpContactInformationType>();
            config.ForType<CustomerDiscountType, ErpCustomerDiscountType>();
            config.ForType<CustomerOrderMode, ErpCustomerOrderMode>();
            config.ForType<CustomerOrderType, ErpCustomerOrderType>();
            config.ForType<CustomerType, ErpCustomerType>();
            config.ForType<DeviceType, ErpDeviceType>();
            config.ForType<DiscountLineType, ErpDiscountLineType>();
            config.ForType<DistanceUnit, ErpDistanceUnit>();
            config.ForType<DocumentStatus, ErpDocumentStatus>();
            config.ForType<DualDisplayType, ErpDualDisplayType>();
            config.ForType<EmployeeActivityType, ErpEmployeeActivityType>();
            config.ForType<EmployeePriceOverrideType, ErpEmployeePriceOverrideType>();
            config.ForType<GiftCardOperationType, ErpGiftCardOperationType>();
            config.ForType<IncomeExpenseAccountType, ErpIncomeExpenseAccountType>();
            config.ForType<LineDiscountCalculationType, ErpLineDiscountCalculationType>();
            config.ForType<LineMultilineDiscountOnItem, ErpLineMultilineDiscountOnItem>();
            config.ForType<LogOnConfiguration, ErpLogOnConfiguration>();
            config.ForType<LoyaltyCardTenderType, ErpLoyaltyCardTenderType>();
            config.ForType<LoyaltyRewardPointEntryType, ErpLoyaltyRewardPointEntryType>();
            config.ForType<LoyaltyRewardPointType, ErpLoyaltyRewardPointType>();
            config.ForType<ManualDiscountType, ErpManualDiscountType>();
            config.ForType<NumberSequenceSeedType, ErpNumberSequenceSeedType>();
            config.ForType<PeriodicDiscountOfferType, ErpPeriodicDiscountOfferType>();
            config.ForType<PrintBehavior, ErpPrintBehavior>();
            config.ForType<PrinterLogoAlignmentType, ErpPrinterLogoAlignmentType>();
            config.ForType<PrinterLogotype, ErpPrinterLogotype>();
            //c.ForType<ProductAttributeRelationType, ErpProductAttributeRelationType>();
            //c.ForType<ProductAttributeValueSource, ErpProductAttributeValueSource>();
            config.ForType<ProductPropertyType, ErpProductPropertyType>();
            config.ForType<ProductSource, ErpProductSource>();
            //c.ForType<PublishingAction, ErpPublishingAction>();
            config.ForType<PurchaseTransferOrderType, ErpPurchaseTransferOrderType>();
            config.ForType<ReasonCodeInputRequiredType, ErpReasonCodeInputRequiredType>();
            config.ForType<ReasonCodeInputType, ErpReasonCodeInputType>();
            config.ForType<ReasonCodeLineType, ErpReasonCodeLineType>();
            config.ForType<ReasonCodeSourceType, ErpReasonCodeSourceType>();
            //c.ForType<ReasonCodeTableRefType, ErpReasonCodeTableRefType>();
            //c.ForType<ReceiptTaxDetailsTypeIndia, ErpReceiptTaxDetailsTypeIndia>();
            //c.ForType<ReceiptTransactionType, ErpReceiptTransactionType>();
            config.ForType<ReceiptType, ErpReceiptType>();
            config.ForType<RetailAffiliationType, ErpRetailAffiliationType>();
            //c.ForType<RetailChannelProductAttributeId, ErpRetailChannelProductAttributeId>();
            config.ForType<RetailChannelType, ErpRetailChannelType>();
            config.ForType<RetailOperation, ErpRetailOperation>();
            //c.ForType<RetailProductChannelProductAttributeId, ErpRetailProductChannelProductAttributeId>();
            config.ForType<RoundingMethod, ErpRoundingMethod>();
            config.ForType<SalesInvoiceType, ErpSalesInvoiceType>();
            config.ForType<SalesStatus, ErpSalesStatus>();
            config.ForType<SalesTransactionType, ErpSalesTransactionType>();
            config.ForType<SearchLocation, ErpSearchLocation>();
            config.ForType<ShiftStatus, ErpShiftStatus>();
            //c.ForType<ShipmentPublishingActionStatus, ErpShipmentPublishingActionStatus>();
            //c.ForType<ShippingOptionType, ErpShippingOptionType>();NewCRT
            //c.ForType<SortOrderType, ErpSortOrderType>();
            config.ForType<StatementMethod, ErpStatementMethod>();
            //c.ForType<TaxableBasisIndia, ErpTaxableBasisIndia>();
            config.ForType<TaxOverrideBy, ErpTaxOverrideBy>();
            config.ForType<TaxOverrideType, ErpTaxOverrideType>();
            //c.ForType<TaxTypeIndia, ErpTaxTypeIndia>();
            //c.ForType<TenderDropAndDeclareType, ErpTenderDropAndDeclareType>();//KAR
            config.ForType<TenderLineStatus, ErpTenderLineStatus>();
            //c.ForType<ThresholdDiscountMethod, ErpThresholdDiscountMethod>();
            //c.ForType<TransactionOperationType, ErpTransactionOperationType>();
            //c.ForType<TransactionServiceProtocol, ErpTransactionServiceProtocol>();
            config.ForType<TransactionStatus, ErpTransactionStatus>();
            config.ForType<TransactionType, ErpTransactionType>();
            config.ForType<TriggerFunctionType, ErpTriggerFunctionType>();
            //c.ForType<AccentColor, ErpAccentColor>();
            //c.ForType<AddressBookPartyData, ErpAddressBookPartyData>();
            config.ForType<AddressFormattingInfo, ErpAddressFormattingInfo>();
            config.ForType<Affiliation, ErpAffiliation>();
            // c.ForType<PagedResult<Affiliation>, PagedResult<ErpAffiliation>>();
            config.ForType<AffiliationLoyaltyTier, ErpAffiliationLoyaltyTier>();
            config.ForType<ARGBColor, ErpARGBColor>();
            //c.ForType<AttributeBase, ErpAttributeBase>();
            config.ForType<AttributeCategory, ErpAttributeCategory>();
            //c.ForType<AttributeGroup, ErpAttributeGroup>();
            //.c.ForType<AttributeNameTranslation, ErpAttributeNameTranslation>();
            config.ForType<AttributeValueBase, ErpAttributeValueBase>();
            // .ConstructUsing((Func<AttributeValueBase, ErpAttributeValueBase>)(x => new ErpAttributeValueBase()));
            config.ForType<Barcode, ErpBarcode>();
            //c.ForType<BarcodeMask, ErpBarcodeMask>();
            //c.ForType<BarcodeMaskSegment, ErpBarcodeMaskSegment>();
            config.ForType<ButtonGrid, ErpButtonGrid>();
            config.ForType<ButtonGridButton, ErpButtonGridButton>();
            config.ForType<ButtonGridZone, ErpButtonGridZone>();
            config.ForType<CardTypeInfo, ErpCardTypeInfo>();
            config.ForType<Cart, ErpCart>();
            config.ForType<Coupon, ErpCoupon>();
            config.ForType<CartLine, ErpCartLine>();
            config.ForType<DeliverySpecification, ErpDeliverySpecification>();
            //c.ForType<CartLineData, ErpCartLineData>();
            config.ForType<CartTenderLine, ErpCartTenderLine>();
            config.ForType<CashDeclaration, ErpCashDeclaration>();
            config.ForType<Category, ErpCategory>();
            config.ForType<ChannelConfiguration, ErpChannelConfiguration>();
            config.ForType<ChannelLanguage, ErpChannelLanguage>();
            //c.ForType<ChannelPriceConfiguration, ErpChannelPriceConfiguration>();
            //c.ForType<ChannelProfile, ErpChannelProfile>();
            config.ForType<ChannelProfileProperty, ErpChannelProfileProperty>();
            config.ForType<ChannelProperty, ErpChannelProperty>();
            //c.ForType<ChargeConfiguration, ErpChargeConfiguration>();
            config.ForType<ChargeLine, ErpChargeLine>();
            config.ForType<CityInfo, ErpCityInfo>();
            config.ForType<CommerceEntity, ErpCommerceEntity>();
            //c.ForType<CommerceEntityChangeTrackingInformation, ErpCommerceEntityChangeTrackingInformation>();
            config.ForType<CommerceList, ErpCommerceList>();
            config.ForType<CommerceListLine, ErpCommerceListLine>()
                .Map(dest => dest.ItemId, src => src.ProductId);
            config.ForType<CommerceProperty, ErpCommerceProperty>();
            config.ForType<CommercePropertyValue, ErpCommercePropertyValue>();
            config.ForType<ComponentKitVariantSet, ErpComponentKitVariantSet>();
            //c.ForType<ContactInfo, ErpContactInfo>();
            config.ForType<ContactInformation, ErpContactInformation>();
            config.ForType<CountryRegionInfo, ErpCountryRegionInfo>();
            config.ForType<CountyInfo, ErpCountyInfo>();
            config.ForType<CreditMemo, ErpCreditMemo>();
            config.ForType<Currency, ErpCurrency>();
            config.ForType<CurrencyAmount, ErpCurrencyAmount>();
            config.ForType<CurrencyRequest, ErpCurrencyRequest>();
            config.ForType<Customer, ErpCustomer>();
            config.ForType<CustomerAttribute, ErpCustomerAttribute>();

            config.ForType<ErpCustomer, Customer>(); //.ForMember(dest => dest.ExtensionProperties); //Commented by UY.
            config.ForType<CustomerAffiliation, ErpCustomerAffiliation>();
            config.ForType<CustomerGroup, ErpCustomerGroup>();
            //c.ForType<CustomerLoyaltyCard, ErpCustomerLoyaltyCard>();
            config.ForType<DeliveryOption, ErpDeliveryOption>();
            config.ForType<Device, ErpDevice>();
            config.ForType<DeviceConfiguration, ErpDeviceConfiguration>();
            config.ForType<DiscountCode, ErpDiscountCode>();
            config.ForType<DiscountLine, ErpDiscountLine>();
            config.ForType<DistrictInfo, ErpDistrictInfo>();
            config.ForType<Employee, ErpEmployee>();
            config.ForType<EmployeeActivity, ErpEmployeeActivity>();
            config.ForType<EmployeePermissions, ErpEmployeePermissions>();
            //c.ForType<ExchangeRate, ErpExchangeRate>();
            //c.ForType<FormulaIndia, ErpFormulaIndia>();
            config.ForType<GiftCard, ErpGiftCard>();
            config.ForType<GlobalCustomer, ErpGlobalCustomer>();
            config.ForType<HardwareProfile, ErpHardwareProfile>();
            config.ForType<HardwareProfileCashDrawer, ErpHardwareProfileCashDrawer>();
            config.ForType<HardwareProfilePrinter, ErpHardwareProfilePrinter>();
            config.ForType<HardwareProfileScanner, ErpHardwareProfileScanner>();
            config.ForType<ImageZone, ErpImageZone>();
            config.ForType<IncomeExpenseAccount, ErpIncomeExpenseAccount>();
            config.ForType<IncomeExpenseLine, ErpIncomeExpenseLine>();
            //c.ForType<Item, ErpItem>();
            config.ForType<ItemAvailability, ErpItemAvailability>();
            //c.ForType<ItemAvailableQuantity, ErpItemAvailableQuantity>();
            config.ForType<ItemBarcode, ErpItemBarcode>();
            //c.ForType<ItemDimensions, ErpItemDimensions>();
            //c.ForType<ItemMaxRetailPriceIndia, ErpItemMaxRetailPriceIndia>();
            //c.ForType<ItemReservation, ErpItemReservation>();
            config.ForType<KitComponent, ErpKitComponent>();
            config.ForType<KitComponentKey, ErpKitComponentKey>();
            //c.ForType<KitConfigToComponentAssociation, ErpKitConfigToComponentAssociation>();
            config.ForType<KitDefinition, ErpKitDefinition>();
            config.ForType<KitLineDefinition, ErpKitLineDefinition>();
            config.ForType<KitLineProductProperty, ErpKitLineProductProperty>();
            //c.ForType<KitLineProductPropertyDictionary, ErpKitLineProductPropertyDictionary>();
            config.ForType<KitTransactionLine, ErpKitTransactionLine>();
            config.ForType<KitVariantContent, ErpKitVariantContent>();
            //c.ForType<KitVariantToComponentDictionary, ErpKitVariantToComponentDictionary>();
            config.ForType<LinkedProduct, ErpLinkedProduct>();
            //c.ForType<ListingPublishStatus, ErpListingPublishStatus>();
            config.ForType<LocalizedString, ErpLocalizedString>();
            config.ForType<LoyaltyCard, ErpLoyaltyCard>();
            config.ForType<LoyaltyCardTier, ErpLoyaltyCardTier>();
            config.ForType<LoyaltyGroup, ErpLoyaltyGroup>();
            config.ForType<LoyaltyRewardPoint, ErpLoyaltyRewardPoint>();
            config.ForType<LoyaltyRewardPointLine, ErpLoyaltyRewardPointLine>();
            //c.ForType<LoyaltySchemeLineEarn, ErpLoyaltySchemeLineEarn>();
            //c.ForType<LoyaltySchemeLineRedeem, ErpLoyaltySchemeLineRedeem>();
            config.ForType<LoyaltyTier, ErpLoyaltyTier>();
            //c.ForType<MixAndMatchLineGroup, ErpMixAndMatchLineGroup>();
            //c.ForType<Notification, ErpNotification>();//KAR
            config.ForType<OperationPermission, ErpOperationPermission>();
            config.ForType<OrgUnitAvailability, ErpOrgUnitAvailability>();
            //c.ForType<OrgUnitContact, ErpOrgUnitContact>();
            config.ForType<OrgUnitLocation, ErpOrgUnitLocation>();
            config.ForType<ParameterSet, ErpParameterSet>();
            config.ForType<PaymentCard, ErpPaymentCard>();
            //c.ForType<PaymentConnectorConfiguration, ErpPaymentConnectorConfiguration>();
            //c.ForType<PeriodicDiscount, ErpPeriodicDiscount>();
            config.ForType<PickingList, ErpPickingList>();
            config.ForType<PickingListLine, ErpPickingListLine>();
            //c.ForType<PriceAdjustment, ErpPriceAdjustment>();
            //c.ForType<PriceGroup, ErpPriceGroup>();
            //c.ForType<PriceLine, ErpPriceLine>();
            //c.ForType<PriceParameters, ErpPriceParameters>();
            config.ForType<Printer, ErpPrinter>();
            config.ForType<Product, ErpProduct>()
                .Map(desc => desc.VariantId, src => string.Empty);

            config.ForType<ProductSearchResult, ErpProduct>()
                .Map(dest => dest.EntityName, src => src.Name);

            //.ForMember(desc => desc.Image); //TODO: Remove Ignore Part
            //c.ForType<ProductAttributeSchemaEntry, ErpProductAttributeSchemaEntry>();
            config.ForType<ProductAvailableQuantity, ErpProductAvailableQuantity>();
            config.ForType<ProductCatalog, ErpProductCatalog>();
            //c.ForType<ProductCatalogAssociation, ErpProductCatalogAssociation>();
            //c.ForType<ProductCategoryAssociation, ErpProductCategoryAssociation>();
            //c.ForType<ProductChangeTrackingAnchorSet, ErpProductChangeTrackingAnchorSet>();
            config.ForType<ProductChangeTrackingInformation, ErpProductChangeTrackingInformation>();
            config.ForType<ProductCompositionInformation, ErpProductCompositionInformation>();
            //c.ForType<ProductDimensionDictionary, ErpProductDimensionDictionary>();
            config.ForType<ProductDimensionSet, ErpProductDimensionSet>();

            //c.ForType<ProductDimensionValueDictionary, ErpProductDimensionValueDictionaryEntry>();
            //for CU 9 
            //c.ForType<ProductDimensionValueDictionaryEntry, ErpProductDimensionValueDictionaryEntry>();

            config.ForType<ProductDimensionValueSet, ErpProductDimensionValueSet>();
            //c.ForType<ProductExistenceId, ErpProductExistenceId>();
            //c.ForType<ProductIdentity, ErpProductIdentity>();
            config.ForType<DateTimeOffset, DateTime>()
                .MapWith(offset => offset.DateTime);
            config.ForType<DateTimeOffset?, DateTime?>()
                .MapWith(offset => offset.HasValue ? offset.Value.DateTime : default(DateTime));
            config.ForType<DateTimeOffset?, DateTime>()
                .MapWith(offset => offset.HasValue ? offset.Value.DateTime : default(DateTime));

            //.ConvertUsing<NullableDateTimeConverter>();
            config.ForType<ProductPrice, ErpProductPrice>();
            // .ForMember(dest => dest.ValidFrom,opt => opt.MapFrom(src => Convert.ToDateTime(src.ValidFrom)));
            config.ForType<ProductProperty, ErpProductProperty>();
            //c.ForType<ProductPropertyDictionary, ErpProductPropertyDictionary>();
            //c.ForType<ProductPropertySchema, ErpProductPropertySchema>();
            config.ForType<ProductPropertyTranslation, ErpProductPropertyTranslation>();
            //c.ForType<ProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>();
            config.ForType<ProductRules, ErpProductRules>();
            //c.ForType<ProductToKitVariantDictionary, ErpProductToKitVariantDictionary>();
            //c.ForType<ProductVariant, ErpProductVariant>();
            config.ForType<ProductVariant, ErpProductVariant>()
                .ConstructUsing((Expression<Func<ProductVariant, ErpProductVariant>>)(x => new ErpProductVariant()))
                .Ignore(dest => dest.ExtensionProperties)
                //.ForMember(dest => dest.IndexedProperties)
                .Ignore(dest => dest.Images)
                .Ignore(dest => dest.PropertiesAsList);
            //c.ForType<ProductVariantDictionary, ErpProductVariantDictionary>();
            config.ForType<ProductVariantInformation, ErpProductVariantInformation>();
            config.ForType<ProjectionDomain, ErpProjectionDomain>();
            config.ForType<PurchaseOrder, ErpPurchaseOrder>();
            config.ForType<PurchaseOrderLine, ErpPurchaseOrderLine>();
            //c.ForType<QuantityDiscountLevel, ErpQuantityDiscountLevel>();
            config.ForType<ReasonCode, ErpReasonCode>();
            config.ForType<ReasonCodeLine, ErpReasonCodeLine>();

            config.ForType<ReasonCodeRequirement, ErpReasonCodeRequirement>();
            //c.ForType<ReasonCodeSettings, ErpReasonCodeSettings>();
            //c.ForType<ReasonCodeSpecific, ErpReasonCodeSpecific>();
            config.ForType<ReasonSubCode, ErpReasonSubCode>();
            config.ForType<Receipt, ErpReceipt>();
            //c.ForType<ReceiptHeaderInfoIndia, ErpReceiptHeaderInfoIndia>();
            //c.ForType<ReceiptHeaderTaxInfoIndia, ErpReceiptHeaderTaxInfoIndia>();
            //c.ForType<ReceiptInfo, ErpReceiptInfo>();
            //c.ForType<ReceiptMask, ErpReceiptMask>();
            //c.ForType<ReceiptProfile, ErpReceiptProfile>();
            config.ForType<RelatedProduct, ErpRelatedProduct>();
            //c.ForType<ReportConfiguration, ErpReportConfiguration>();
            config.ForType<ReportZone, ErpReportZone>();
            //c.ForType<RetailCategoryMember, ErpRetailCategoryMember>();
            //c.ForType<RetailDiscount, ErpRetailDiscount>();
            //c.ForType<RetailDiscountLine, ErpRetailDiscountLine>();
            //c.ForType<RetailDiscountPriceGroup, ErpRetailDiscountPriceGroup>();
            //c.ForType<RetailImage, ErpRetailImage>();
            config.ForType<RichMediaLocations, ErpRichMediaLocations>();
            config.ForType<RichMediaLocationsRichMediaLocation, ErpRichMediaLocationsRichMediaLocation>();
            config.ForType<SalesAffiliationLoyaltyTier, ErpSalesAffiliationLoyaltyTier>();
            // .ConstructUsing((Func<SalesAffiliationLoyaltyTier, ErpSalesAffiliationLoyaltyTier>)(x => new ErpSalesAffiliationLoyaltyTier()));
            config.ForType<SalesInvoice, ErpSalesInvoice>();
            config.ForType<SalesInvoiceLine, ErpSalesInvoiceLine>();

            //++US
            config.ForType<SalesLine, ErpSalesLine>()
                .Ignore(desc => desc.LineMultilineDiscOnItem);


            //c.ForType<SalesParameters, ErpSalesParameters>();
            config.ForType<SalesTaxGroup, ErpSalesTaxGroup>();
            //c.ForType<SalesTransaction, ErpSalesTransaction>();
            //c.ForType<SalesTransactionData, ErpSalesTransactionData>();
            config.ForType<Shift, ErpShift>();
            config.ForType<ShiftAccountLine, ErpShiftAccountLine>();
            config.ForType<ShiftTenderLine, ErpShiftTenderLine>();
            //c.ForType<Shipment, ErpShipment>();
            //c.ForType<ShipmentLine, ErpShipmentLine>();
            //c.ForType<ShipmentLineMapping, ErpShipmentLineMapping>();
            //c.ForType<ShipmentProgress, ErpShipmentProgress>();
            //c.ForType<ShipmentPublishingStatus, ErpShipmentPublishingStatus>();
            //c.ForType<ShippingAdapterConfig, ErpShippingAdapterConfig>();
            config.ForType<StateProvinceInfo, ErpStateProvinceInfo>();
            config.ForType<StockCountJournal, ErpStockCountJournal>();
            config.ForType<StockCountJournalTransaction, ErpStockCountJournalTransaction>();
            //c.ForType<StorageLookup, ErpStorageLookup>();
            config.ForType<SupportedLanguage, ErpSupportedLanguage>();
            //c.ForType<TaxableItem, ErpTaxableItem>();
            //c.ForType<TaxCodeInterval, ErpTaxCodeInterval>();
            //c.ForType<TaxCodeUnit, ErpTaxCodeUnit>();
            //c.ForType<TaxComponentIndia, ErpTaxComponentIndia>();
            config.ForType<TaxLine, ErpTaxLine>();
            config.ForType<TaxOverride, ErpTaxOverride>();
            //c.ForType<TaxParameters, ErpTaxParameters>();
            //c.ForType<TaxSummarySettingIndia, ErpTaxSummarySettingIndia>();
            config.ForType<TenderDetail, ErpTenderDetail>();
            config.ForType<TenderLine, ErpTenderLine>();
            //c.ForType<TenderLineBase, ErpTenderLineBase>();
            config.ForType<TenderType, ErpTenderType>();
            //c.ForType<Terminal, ErpTerminal>();
            config.ForType<TextValueTranslation, ErpTextValueTranslation>();
            //c.ForType<ThresholdDiscountTier, ErpThresholdDiscountTier>();
            config.ForType<TillLayout, ErpTillLayout>();
            //c.ForType<TrackingInfo, ErpTrackingInfo>();
            //c.ForType<TradeAgreement, ErpTradeAgreement>();
            config.ForType<Transaction, ErpTransaction>();
            //c.ForType<TransactionProperty, ErpTransactionProperty>();
            //c.ForType<TransactionServiceProfile, ErpTransactionServiceProfile>();
            config.ForType<TransferOrder, ErpTransferOrder>();
            config.ForType<TransferOrderLine, ErpTransferOrderLine>();
            config.ForType<UnitOfMeasure, ErpUnitOfMeasure>();
            config.ForType<UnitOfMeasureConversion, ErpUnitOfMeasureConversion>();
            //c.ForType<ValidationPeriod, ErpValidationPeriod>();
            //c.ForType<Weight, ErpWeight>();
            config.ForType<ZipCodeInfo, ErpZipCodeInfo>();
            //c.ForType<ZoneReference, ErpZoneReference>();


            config.ForType<SimpleProduct, ErpProduct>();
            config.ForType<SimpleProduct, ErpProductVariant>();
            config.ForType<AttributeProduct, ErpAttributeProduct>();

            config.ForType<List<KeyValuePair<long, IEnumerable<Product>>>, List<KeyValuePair<long, IEnumerable<ErpProduct>>>>();

            ////For Quotation
            //config.ForType<CustomerOrderInfo, ErpCustomerOrderInfo>();
            //config.ForType<Preauthorization, ErpPreauthorization>();
            //config.ForType<AffiliationInfo, ErpAffiliationInfo>();
            //config.ForType<ChargeInfo, ErpChargeInfo>();
            //config.ForType<ItemInfo, ErpItemInfo>();
            //config.ForType<PaymentInfo, ErpPaymentInfo>();
            //config.ForType<TaxInfo, ErpTaxInfo>();
            //config.ForType<DiscountInfo, ErpDiscountInfo>();
            //config.ForType<QuotationReasonGroup, ERPQuotationReasonGroup>();

            ////NS: TMV FDD-013
            //config.ForType<ChannelCustomProperties, ErpChannelCustomProperties>();
            //config.ForType<RetailServiceProfile, ErpRetailServiceProfile>();
            //config.ForType<RetailServiceProfileProperty, ErpRetailServiceProfileProperty>();
            //config.ForType<RetailChannelProfile, ErpRetailChannelProfile>();
            //config.ForType<RetailChannelProfileProperty, ErpRetailChannelProfileProperty>();

            //config.ForType<ProductCustomField, ErpProductCustomFields>();
            //config.ForType<OfferTypeGroup, ERPOfferTypeGroup>();
            //config.ForType<UpsellItem, ErpUpsellItem>();
            //config.ForType<RetailDiscountItem, ErpRetailDiscountItem>();
            //config.ForType<RetailDiscountWithAffiliationItem, ErpRetailDiscountWithAffiliationItem>();
            //config.ForType<RetailQuantityDiscountItem, ErpRetailQuantityDiscountItem>();
            //config.ForType<RetailQuantityDiscountWithAffiliationItem, ErpRetailQuantityDiscountWithAffiliationItem>();
            //config.ForType<RetailInventItemSalesSetup, ErpRetailInventItemSalesSetup>();

            //c.ForType<List<UpsellItem>, List<ErpUpsellItem>>();

            //c.ForType<, ErpCustInvoiceJour>();

            //NS: D365 Update 12 Platform change start

            config.ForType<AuditEvent, ErpAuditEvent>();
            config.ForType<Comment, ErpComment>();
            config.ForType<DenominationDetail, ErpDenominationDetail>();
            config.ForType<ExtensibleAuditEventType, ErpExtensibleAuditEventType>();
            config.ForType<FulfillmentLine, ErpFulfillmentLine>();
            config.ForType<FulfillmentLineDeliveryType, ErpFulfillmentLineDeliveryType>();
            config.ForType<FulfillmentLineSearchCriteria, ErpFulfillmentLineSearchCriteria>();
            config.ForType<FulfillmentLineStatus, ErpFulfillmentLineStatus>();
            config.ForType<InsufficientCredentialLengthException, ErpInsufficientCredentialLengthException>();
            config.ForType<NotificationDetail, ErpNotificationDetail>();
            config.ForType<NotificationItem, ErpNotificationItem>();
            config.ForType<OrderSearchCriteria, ErpOrderSearchCriteria>();
            config.ForType<OrgUnit, ErpOrgUnit>();
            config.ForType<PaymentMerchantInformation, ErpPaymentMerchantInformation>();
            config.ForType<ProductBehavior, ErpProductBehavior>();
            config.ForType<ProductDimension, ErpProductDimension>();
            config.ForType<ProductDimensionCombination, ErpProductDimensionCombination>();
            config.ForType<ProductDimensionValue, ErpProductDimensionValue>();
            config.ForType<ReceiptRetrievalCriteria, ErpReceiptRetrievalCriteria>();
            config.ForType<ReturnLabelContent, ErpReturnLabelContent>();
            config.ForType<SearchFilter, ErpSearchFilter>();
            config.ForType<SearchFilterValue, ErpSearchFilterValue>();
            config.ForType<ShipFulfillmentLine, ErpShipFulfillmentLine>();
            config.ForType<StaffPasswordExpiredException, ErpStaffPasswordExpiredException>();
            config.ForType<ThirdPartyGiftCardInfo, ErpThirdPartyGiftCardInfo>();
            config.ForType<TransactionSearchCriteria, ErpTransactionSearchCriteria>();
            config.ForType<TransferOrderJournal, ErpTransferOrderJournal>();
            config.ForType<TransferOrderJournalLine, ErpTransferOrderJournalLine>();
            //Enums
            config.ForType<AuditEventType, ErpAuditEventType>();
            config.ForType<AutoExitMethodType, ErpAutoExitMethodType>();
            config.ForType<EmployeeLogonType, ErpEmployeeLogonType>();
            config.ForType<InfoCodeActivity, ErpInfoCodeActivity>();
            config.ForType<RetailDenominationsToDisplay, ErpRetailDenominationsToDisplay>();
            config.ForType<SearchFilterType, ErpSearchFilterType>();

            //NS: D365 Update 12 Platform change end

            //NS: D365 Update 8.1 Application change start
            config.ForType<TaxMeasure, TaxMeasure>();
            config.ForType<TransactionServiceAuthenticationType, TransactionServiceAuthenticationType>();
            config.ForType<TaxCalculationType, TaxCalculationType>();
            config.ForType<InvoiceType, InvoiceType>();
            config.ForType<CustomerSearchField, CustomerSearchField>();
            config.ForType<AddressPurpose, AddressPurpose>();
            config.ForType<AuditEventFiscalTransaction, AuditEventFiscalTransaction>();
            config.ForType<AuditEventUploadType, AuditEventUploadType>();
            config.ForType<CustomerSearchByFieldCriteria, CustomerSearchByFieldCriteria>();
            config.ForType<CustomerSearchByFieldCriterion, CustomerSearchByFieldCriterion>();
            config.ForType<EnvironmentConfiguration, EnvironmentConfiguration>();
            config.ForType<FiscalTransaction, FiscalTransaction>();
            config.ForType<InvoiceSearchCriteria, InvoiceSearchCriteria>();
            config.ForType<OrderInvoice, OrderInvoice>();
            config.ForType<OrgUnitAddress, OrgUnitAddress>();
            config.ForType<OrgUnitAvailabilitySearchCriteria, OrgUnitAvailabilitySearchCriteria>();
            config.ForType<OrgUnitLocationSearchCriteria, OrgUnitLocationSearchCriteria>();
            config.ForType<PackingSlipData, PackingSlipData>();
            config.ForType<ProductRefinerValue, ProductRefinerValue>();
            config.ForType<RejectFulfillmentLine, RejectFulfillmentLine>();
            config.ForType<ShiftTaxLine, ShiftTaxLine>();
            config.ForType<TaxLineGTE, TaxLineGTE>();
            config.ForType<TaxMeasure, TaxMeasure>();
            config.ForType<WarehouseDetails, WarehouseDetails>();
            config.ForType<CustomerSearchFieldType, CustomerSearchFieldType>();
            //config.ForType<CreditCardCust, ErpCreditCardCust>();
            //config.ForType<CustBankAccount, ErpBankAccountCust>();
            //NS: D365 Update 8.1 Application change end

            #endregion

            //=============================
            //Mapping for ERP to RS
            //=============================
            #region Mapping for ERP to CRT
            config.ForType<ErpContactPerson, ContactPerson>();
            config.ForType<ErpContactPersonNAL, ContactPerson>();
            //config.ForType<ErpContactPerson, ContactPersonResponseSet>();
            //config.ForType<ErpContactPersonNAL, SaveContactPersonResponseSet>();
            //config.ForType<ErpContactPersonNAL, ContactPersonNALResponseSet>();
            config.ForType<ErpCommercePropertyValue, CommercePropertyValue>();
            config.ForType<ErpCommerceProperty, CommerceProperty>();
            //c.ForType<ErpClassInfo, ClassInfo>();
            //c.ForType<ErpAddressFilter, AddressFilter>();
            config.ForType<ErpAddressFormatLineType, AddressFormatLineType>();
            config.ForType<ErpAddressType, AddressType>();
            //c.ForType<ErpAlignment, Alignment>();
            config.ForType<ErpAttributeDataType, AttributeDataType>();
            config.ForType<ErpAttributeGroupType, AttributeGroupType>();
            config.ForType<ErpBarcodeEntryMethodType, BarcodeEntryMethodType>();
            config.ForType<ErpBarcodeMaskType, BarcodeMaskType>();
            //c.ForType<ErpBarcodeSegmentType, BarcodeSegmentType>();
            //c.ForType<ErpCalculationModes, CalculationModes>();
            config.ForType<ErpCardType, CardType>();
            config.ForType<ErpCartType, CartType>();
            config.ForType<ErpCashType, CashType>();
            //c.ForType<ErpChangeAction, ChangeAction>();
            //c.ForType<ErpChargeAccountType, ChargeAccountType>();
            //c.ForType<ErpChargeDeliveryType, ChargeDeliveryType>();
            //c.ForType<ErpChargeItemType, ChargeItemType>();
            //c.ForType<ErpChargeLevel, ChargeLevel>();
            config.ForType<ErpChargeMethod, ChargeMethod>();
            config.ForType<ErpChargeModule, ChargeModule>();
            config.ForType<ErpChargeType, ChargeType>();
            config.ForType<ErpCommerceEntityDataLevel, CommerceEntityDataLevel>();
            //c.ForType<ErpCommerceListOperationType, CommerceListOperationType>();
            config.ForType<ErpCommerceListType, CommerceListType>();
            config.ForType<ErpConcurrencyMode, ConcurrencyMode>();
            config.ForType<ErpContactInformationType, ContactInformationType>();
            //c.ForType<ErpContactInfoType, ContactInfoType>();
            //c.ForType<ErpCountryRegionISOCode, CountryRegionISOCode>();
            config.ForType<ErpCustomerDiscountType, CustomerDiscountType>();
            config.ForType<ErpCustomerOrderMode, CustomerOrderMode>();
            config.ForType<ErpCustomerOrderType, CustomerOrderType>();
            config.ForType<ErpCustomerType, CustomerType>();
            //c.ForType<ErpDatabaseErrorCodes, DatabaseErrorCodes>();
            //c.ForType<ErpDataStoreType, DataStoreType>();
            //c.ForType<ErpDateValidationType, DateValidationType>();
            //c.ForType<ErpDayMonthYear, DayMonthYear>();
            config.ForType<ErpDeviceType, DeviceType>();
            //c.ForType<ErpDiscountCalculationMode, DiscountCalculationMode>();
            config.ForType<ErpDiscountLineType, DiscountLineType>();
            //c.ForType<ErpDiscountOfferMethod, DiscountOfferMethod>();
            config.ForType<ErpDistanceUnit, DistanceUnit>();
            config.ForType<ErpDocumentStatus, DocumentStatus>();
            config.ForType<ErpDualDisplayType, DualDisplayType>();
            config.ForType<ErpEmployeeActivityType, EmployeeActivityType>();
            config.ForType<ErpEmployeePriceOverrideType, EmployeePriceOverrideType>();
            config.ForType<ErpGiftCardOperationType, GiftCardOperationType>();
            config.ForType<ErpIncomeExpenseAccountType, IncomeExpenseAccountType>();
            //c.ForType<ErpItemAvailabilityPreferences, ItemAvailabilityPreferences>();
            //c.ForType<ErpKeyInPrices, KeyInPrices>();
            //c.ForType<ErpKeyInQuantities, KeyInQuantities>();
            config.ForType<ErpLineDiscountCalculationType, LineDiscountCalculationType>();
            config.ForType<ErpLineMultilineDiscountOnItem, LineMultilineDiscountOnItem>();
            //c.ForType<ErpListingPublishingActionStatus, ListingPublishingActionStatus>();
            config.ForType<ErpLogOnConfiguration, LogOnConfiguration>();
            //c.ForType<ErpLogOnType, LogOnType>();//KAR
            //c.ForType<ErpLoyaltyActivityType, LoyaltyActivityType>();
            config.ForType<ErpLoyaltyCardTenderType, LoyaltyCardTenderType>();
            config.ForType<ErpLoyaltyRewardPointEntryType, LoyaltyRewardPointEntryType>();
            config.ForType<ErpLoyaltyRewardPointType, LoyaltyRewardPointType>();
            //c.ForType<ErpLoyaltyRewardType, LoyaltyRewardType>();
            //c.ForType<ErpLoyaltyTransactionType, LoyaltyTransactionType>();
            config.ForType<ErpManualDiscountType, ManualDiscountType>();
            //c.ForType<ErpMultipleBuyDiscountMethod, MultipleBuyDiscountMethod>();
            //c.ForType<ErpNonSalesTenderType, NonSalesTenderType>();//KAR
            config.ForType<ErpNumberSequenceSeedType, NumberSequenceSeedType>();
            //c.ForType<ErpOnlineChannelPublishStatusType, OnlineChannelPublishStatusType>();
            //c.ForType<ErpPendingOrderStatus, PendingOrderStatus>();
            config.ForType<ErpPeriodicDiscountOfferType, PeriodicDiscountOfferType>();
            //c.ForType<ErpPriceDiscountAccountCode, PriceDiscountAccountCode>();
            //c.ForType<ErpPriceDiscountItemCode, PriceDiscountItemCode>();
            //c.ForType<ErpPriceDiscountType, PriceDiscountType>();
            //c.ForType<ErpPriceMethod, PriceMethod>();
            //c.ForType<ErpPriceType, PriceType>();
            //c.ForType<ErpPricingCalculationMode, PricingCalculationMode>();
            config.ForType<ErpPrintBehavior, PrintBehavior>();
            config.ForType<ErpPrinterLogoAlignmentType, PrinterLogoAlignmentType>();
            config.ForType<ErpPrinterLogotype, PrinterLogotype>();
            //c.ForType<ErpProductAttributeRelationType, ProductAttributeRelationType>();
            //c.ForType<ErpProductAttributeValueSource, ProductAttributeValueSource>();
            config.ForType<ErpProductPropertyType, ProductPropertyType>();
            config.ForType<ErpProductSource, ProductSource>()
                .ConstructUsing((Expression<Func<ErpProductSource, ProductSource>>)(x => new ProductSource()));
            //c.ForType<ErpPublishingAction, PublishingAction>();
            config.ForType<ErpPurchaseTransferOrderType, PurchaseTransferOrderType>();
            config.ForType<ErpReasonCodeInputRequiredType, ReasonCodeInputRequiredType>();
            config.ForType<ErpReasonCodeInputType, ReasonCodeInputType>();
            config.ForType<ErpReasonCodeLineType, ReasonCodeLineType>();
            config.ForType<ErpReasonCodeSourceType, ReasonCodeSourceType>();
            //c.ForType<ErpReasonCodeTableRefType, ReasonCodeTableRefType>();
            //c.ForType<ErpReceiptTaxDetailsTypeIndia, ReceiptTaxDetailsTypeIndia>();
            //c.ForType<ErpReceiptTransactionType, ReceiptTransactionType>();
            config.ForType<ErpReceiptType, ReceiptType>();
            config.ForType<ErpRetailAffiliationType, RetailAffiliationType>();
            //c.ForType<ErpRetailChannelProductAttributeId, RetailChannelProductAttributeId>();
            config.ForType<ErpRetailChannelType, RetailChannelType>();
            config.ForType<ErpRetailOperation, RetailOperation>();
            //c.ForType<ErpRetailProductChannelProductAttributeId, RetailProductChannelProductAttributeId>();
            config.ForType<ErpRoundingMethod, RoundingMethod>();
            config.ForType<ErpSalesInvoiceType, SalesInvoiceType>();
            config.ForType<ErpSalesStatus, SalesStatus>();
            config.ForType<ErpSalesTransactionType, SalesTransactionType>();
            config.ForType<ErpSearchLocation, SearchLocation>();
            config.ForType<ErpShiftStatus, ShiftStatus>();
            //c.ForType<ErpShipmentPublishingActionStatus, ShipmentPublishingActionStatus>();
            //c.ForType<ErpShippingOptionType, ShippingOptionType>();NewCRT
            //c.ForType<ErpSortOrderType, SortOrderType>();
            config.ForType<ErpStatementMethod, StatementMethod>();
            //c.ForType<ErpTaxableBasisIndia, TaxableBasisIndia>();
            config.ForType<ErpTaxOverrideBy, TaxOverrideBy>();
            config.ForType<ErpTaxOverrideType, TaxOverrideType>();
            //c.ForType<ErpTaxTypeIndia, TaxTypeIndia>();
            //c.ForType<ErpTenderDropAndDeclareType, TenderDropAndDeclareType>();//KAR
            config.ForType<ErpTenderLineStatus, TenderLineStatus>();
            //c.ForType<ErpThresholdDiscountMethod, ThresholdDiscountMethod>();
            //c.ForType<ErpTransactionOperationType, TransactionOperationType>();
            //c.ForType<ErpTransactionServiceProtocol, TransactionServiceProtocol>();
            config.ForType<ErpTransactionStatus, TransactionStatus>();
            config.ForType<ErpTransactionType, TransactionType>();
            config.ForType<ErpTriggerFunctionType, TriggerFunctionType>();
            //c.ForType<ErpAccentColor, AccentColor>();
            config.ForType<ErpAddress, Address>()
                .Ignore(x => x.ExtensionProperties); // AF
            //c.ForType<ErpAddress, Address>()
            //    .ForMember(x => x.Name, opt => opt.UseValue("Address"))
            //    .ForMember(x => x.ExtensionProperties);
            //c.ForType<ErpAddressBookPartyData, AddressBookPartyData>();
            config.ForType<ErpAddressFormattingInfo, AddressFormattingInfo>();
            config.ForType<ErpAffiliation, Affiliation>();
            config.ForType<PagedResult<ErpAffiliation>, PagedResult<Affiliation>>();
            config.ForType<ErpAffiliationLoyaltyTier, AffiliationLoyaltyTier>();
            config.ForType<ErpARGBColor, ARGBColor>();
            //c.ForType<ErpAttributeBase, AttributeBase>();
            //c.ForType<ErpAttributeCategory, AttributeCategory>();
            //c.ForType<ErpAttributeGroup, AttributeGroup>();
            //c.ForType<ErpAttributeNameTranslation, AttributeNameTranslation>();
            config.ForType<ErpAttributeValueBase, AttributeValueBase>();
            //  .ConstructUsing((Func<ErpAttributeValueBase, AttributeValueBase>)(x => new AttributeValueBase()));
            config.ForType<ErpBarcode, Barcode>();
            //c.ForType<ErpBarcodeMask, BarcodeMask>();
            //c.ForType<ErpBarcodeMaskSegment, BarcodeMaskSegment>();
            config.ForType<ErpButtonGrid, ButtonGrid>();
            config.ForType<ErpButtonGridButton, ButtonGridButton>();
            config.ForType<ErpButtonGridZone, ButtonGridZone>();
            config.ForType<ErpCardTypeInfo, CardTypeInfo>();
            config.ForType<ErpCart, Cart>()
                .Ignore(desc => desc.IsDiscountFullyCalculated)
                .Ignore(desc => desc.BeginDateTime)
                .Ignore(desc => desc.AmountDue)
                .Ignore(desc => desc.TotalAmount)
                .Ignore(desc => desc.SubtotalAmountWithoutTax)
                .Ignore(desc => desc.SubtotalAmount);
            config.ForType<ErpCoupon, Coupon>();
            config.ForType<ErpCartLine, CartLine>()
                .Ignore(desc => desc.QuantityInvoiced)
                .Ignore(desc => desc.QuantityOrdered)
                .Ignore(desc => desc.StaffId)
                .Ignore(desc => desc.SalesStatusValue)
                .Ignore(desc => desc.OriginalPrice)
                .Ignore(desc => desc.LinkedParentLineId)
                .Ignore(desc => desc.LineNumber);

            //c.ForType<ErpCartLineData, CartLineData>();
            config.ForType<ErpCartTenderLine, CartTenderLine>();
            config.ForType<ErpDeliverySpecification, DeliverySpecification>();
            config.ForType<ErpCashDeclaration, CashDeclaration>();
            config.ForType<ErpCategory, Category>();
            //.ForMember(desc => desc.Image)
            //.ForMember(desc => desc.ExtensionProperties)
            //.ForMember(desc => desc.ExtensionData)
            //.ForMember(desc => desc.NameTranslations); //TODO: Remove Ignore Part;
            //c.ForType<ErpCategoryNameTranslation, CategoryNameTranslation>();
            //c.ForType<ErpChannel, Channel>();
            config.ForType<ErpChannelConfiguration, ChannelConfiguration>();
            config.ForType<ErpChannelLanguage, ChannelLanguage>();
            //c.ForType<ErpChannelPriceConfiguration, ChannelPriceConfiguration>();
            //c.ForType<ErpChannelProfile, ChannelProfile>();
            config.ForType<ErpChannelProfileProperty, ChannelProfileProperty>();
            config.ForType<ErpChannelProperty, ChannelProperty>();
            //c.ForType<ErpChargeConfiguration, ChargeConfiguration>();
            config.ForType<ErpChargeLine, ChargeLine>()
                .Ignore(dest => dest.ExtensionProperties);
            config.ForType<ErpCityInfo, CityInfo>();
            config.ForType<ErpCommerceEntity, CommerceEntity>();
            //c.ForType<ErpCommerceEntityChangeTrackingInformation, CommerceEntityChangeTrackingInformation>();
            config.ForType<ErpCommerceList, CommerceList>();
            config.ForType<ErpCommerceListLine, CommerceListLine>()
                .Map(dest => dest.ProductId, src => src.ItemId);
            config.ForType<ErpCommerceProperty, CommerceProperty>();
            config.ForType<ErpCommercePropertyValue, CommercePropertyValue>();
            config.ForType<ErpComponentKitVariantSet, ComponentKitVariantSet>();
            //c.ForType<ErpContactInfo, ContactInfo>()
            //    .ForMember(dest => dest.ExtensionProperties);
            config.ForType<ErpContactInformation, ContactInformation>()
                .Ignore(dest => dest.ExtensionProperties);
            config.ForType<ErpCountryRegionInfo, CountryRegionInfo>();
            config.ForType<ErpCountyInfo, CountyInfo>();
            config.ForType<ErpCreditMemo, CreditMemo>();
            config.ForType<ErpCurrency, Currency>();
            config.ForType<ErpCurrencyAmount, CurrencyAmount>();
            config.ForType<ErpCurrencyRequest, CurrencyRequest>();
            config.ForType<ErpCustomer, Customer>();

            config.ForType<ErpCustomer, Customer>()
                .Ignore(dest => dest.ExtensionProperties);
            config.ForType<ErpCustomerAttribute, CustomerAttribute>();

            config.ForType<ErpCustomerAffiliation, CustomerAffiliation>();
            config.ForType<ErpCustomerGroup, CustomerGroup>();
            //c.ForType<ErpCustomerLoyaltyCard, CustomerLoyaltyCard>();
            config.ForType<ErpDeliveryOption, DeliveryOption>();
            config.ForType<ErpDevice, Device>();
            config.ForType<ErpDeviceConfiguration, DeviceConfiguration>();
            config.ForType<ErpDiscountCode, DiscountCode>();
            config.ForType<ErpDiscountLine, DiscountLine>()
                .Ignore(dest => dest.ExtensionProperties);
            config.ForType<ErpDistrictInfo, DistrictInfo>();
            config.ForType<ErpEmployee, Employee>();
            config.ForType<ErpEmployeeActivity, EmployeeActivity>();
            config.ForType<ErpEmployeePermissions, EmployeePermissions>();
            //c.ForType<ErpExchangeRate, ExchangeRate>();
            //c.ForType<ErpFormulaIndia, FormulaIndia>();
            config.ForType<ErpGiftCard, GiftCard>();
            config.ForType<ErpGlobalCustomer, GlobalCustomer>();
            config.ForType<ErpHardwareProfile, HardwareProfile>();
            config.ForType<ErpHardwareProfileCashDrawer, HardwareProfileCashDrawer>();
            config.ForType<ErpHardwareProfilePrinter, HardwareProfilePrinter>();
            config.ForType<ErpHardwareProfileScanner, HardwareProfileScanner>();
            config.ForType<ErpImageZone, ImageZone>();
            config.ForType<ErpIncomeExpenseAccount, IncomeExpenseAccount>();
            config.ForType<ErpIncomeExpenseLine, IncomeExpenseLine>();
            //c.ForType<ErpItem, Item>();
            config.ForType<ErpItemAvailability, ItemAvailability>();
            //c.ForType<ErpItemAvailableQuantity, ItemAvailableQuantity>();
            config.ForType<ErpItemBarcode, ItemBarcode>();
            //c.ForType<ErpItemDimensions, ItemDimensions>();
            //c.ForType<ErpItemMaxRetailPriceIndia, ItemMaxRetailPriceIndia>();
            //c.ForType<ErpItemReservation, ItemReservation>();
            config.ForType<ErpKitComponent, KitComponent>();
            config.ForType<ErpKitComponentKey, KitComponentKey>();
            //c.ForType<ErpKitConfigToComponentAssociation, KitConfigToComponentAssociation>();
            config.ForType<ErpKitDefinition, KitDefinition>();
            config.ForType<ErpKitLineDefinition, KitLineDefinition>();
            config.ForType<ErpKitLineProductProperty, KitLineProductProperty>();
            //c.ForType<ErpKitLineProductPropertyDictionary, KitLineProductPropertyDictionary>();
            config.ForType<ErpKitTransactionLine, KitTransactionLine>();
            config.ForType<ErpKitVariantContent, KitVariantContent>();
            //c.ForType<ErpKitVariantToComponentDictionary, KitVariantToComponentDictionary>();
            config.ForType<ErpLinkedProduct, LinkedProduct>();
            //c.ForType<ErpListingPublishStatus, ListingPublishStatus>();
            config.ForType<ErpLocalizedString, LocalizedString>();
            config.ForType<ErpLoyaltyCard, LoyaltyCard>();
            config.ForType<ErpLoyaltyCardTier, LoyaltyCardTier>();
            config.ForType<ErpLoyaltyGroup, LoyaltyGroup>();
            config.ForType<ErpLoyaltyRewardPoint, LoyaltyRewardPoint>();
            config.ForType<ErpLoyaltyRewardPointLine, LoyaltyRewardPointLine>();
            //c.ForType<ErpLoyaltySchemeLineEarn, LoyaltySchemeLineEarn>();
            //c.ForType<ErpLoyaltySchemeLineRedeem, LoyaltySchemeLineRedeem>();
            config.ForType<ErpLoyaltyTier, LoyaltyTier>();
            //c.ForType<ErpMixAndMatchLineGroup, MixAndMatchLineGroup>();
            //c.ForType<ErpNotification, Notification>();//KAR
            config.ForType<ErpOperationPermission, OperationPermission>();
            config.ForType<ErpOrgUnitAvailability, OrgUnitAvailability>();
            //c.ForType<ErpOrgUnitContact, OrgUnitContact>();
            config.ForType<ErpOrgUnitLocation, OrgUnitLocation>();
            config.ForType<ErpParameterSet, ParameterSet>();
            config.ForType<ErpPaymentCard, PaymentCard>();
            //c.ForType<ErpPaymentConnectorConfiguration, PaymentConnectorConfiguration>();
            //c.ForType<ErpPeriodicDiscount, PeriodicDiscount>();
            config.ForType<ErpPickingList, PickingList>();
            config.ForType<ErpPickingListLine, PickingListLine>();
            //c.ForType<ErpPriceAdjustment, PriceAdjustment>();
            //c.ForType<ErpPriceGroup, PriceGroup>();
            //c.ForType<ErpPriceLine, PriceLine>();
            //c.ForType<ErpPriceParameters, PriceParameters>();
            config.ForType<ErpPrinter, Printer>();
            config.ForType<ErpProduct, Product>();
            //.ForMember(desc => desc.VariantId, opt => opt.MapFrom(src => string.Empty));
            //.ForMember(desc => desc.Image); //TODO: Remove Ignore Part
            //c.ForType<ErpProductAttributeSchemaEntry, ProductAttributeSchemaEntry>();
            config.ForType<ErpProductAvailableQuantity, ProductAvailableQuantity>();
            config.ForType<ErpProductCatalog, ProductCatalog>();
            //c.ForType<ErpProductCatalogAssociation, ProductCatalogAssociation>();
            //c.ForType<ErpProductCategoryAssociation, ProductCategoryAssociation>();
            //c.ForType<ErpProductChangeTrackingAnchorSet, ProductChangeTrackingAnchorSet>();
            config.ForType<ErpProductChangeTrackingInformation, ProductChangeTrackingInformation>();
            config.ForType<ErpProductCompositionInformation, ProductCompositionInformation>();
            //c.ForType<ErpProductDimensionDictionary, ProductDimensionDictionary>();
            config.ForType<ErpProductDimensionSet, ProductDimensionSet>();

            //c.ForType<ErpProductDimensionValueDictionary, ProductDimensionValueDictionaryEntry>();
            //for CU 9 
            //c.ForType<ErpProductDimensionValueDictionaryEntry, ProductDimensionValueDictionaryEntry>();

            config.ForType<ErpProductDimensionValueSet, ProductDimensionValueSet>();
            //c.ForType<ErpProductExistenceId, ProductExistenceId>();
            //c.ForType<ErpProductIdentity, ProductIdentity>();
            config.ForType<ErpProductPrice, ProductPrice>();
            config.ForType<ErpProductProperty, ProductProperty>();
            //c.ForType<ErpProductPropertyDictionary, ProductPropertyDictionary>();
            //c.ForType<ErpProductPropertySchema, ProductPropertySchema>();
            config.ForType<ErpProductPropertyTranslation, ProductPropertyTranslation>();
            //c.ForType<ErpProductPropertyTranslationDictionary, ProductPropertyTranslationDictionary>();
            config.ForType<ErpProductRules, ProductRules>();
            //c.ForType<ErpProductToKitVariantDictionary, ProductToKitVariantDictionary>();
            //c.ForType<ErpProductVariant, ProductVariant>();
            config.ForType<ErpProductVariant, ProductVariant>()
                .ConstructUsing((Expression<Func<ErpProductVariant, ProductVariant>>)(x => new ProductVariant()))
                .Ignore(dest => dest.ExtensionProperties)
                //.ForMember(dest => dest.IndexedProperties)
                .Ignore(dest => dest.Images)
                .Ignore(dest => dest.PropertiesAsList);
            //c.ForType<ErpProductVariantDictionary, ProductVariantDictionary>();
            config.ForType<ErpProductVariantInformation, ProductVariantInformation>();
            config.ForType<ErpProjectionDomain, ProjectionDomain>();
            config.ForType<ErpPurchaseOrder, PurchaseOrder>();
            config.ForType<ErpPurchaseOrderLine, PurchaseOrderLine>();
            //c.ForType<ErpQuantityDiscountLevel, QuantityDiscountLevel>();
            config.ForType<ErpReasonCode, ReasonCode>();
            config.ForType<ErpReasonCodeLine, ReasonCodeLine>()
                    .Ignore(dest => dest.ExtensionProperties); //AF

            config.ForType<ErpReasonCodeRequirement, ReasonCodeRequirement>();
            //c.ForType<ErpReasonCodeSettings, ReasonCodeSettings>();
            //c.ForType<ErpReasonCodeSpecific, ReasonCodeSpecific>();
            config.ForType<ErpReasonSubCode, ReasonSubCode>();
            config.ForType<ErpReceipt, Receipt>();
            //c.ForType<ErpReceiptHeaderInfoIndia, ReceiptHeaderInfoIndia>();
            //c.ForType<ErpReceiptHeaderTaxInfoIndia, ReceiptHeaderTaxInfoIndia>();
            //c.ForType<ErpReceiptInfo, ReceiptInfo>();
            //c.ForType<ErpReceiptMask, ReceiptMask>();
            //c.ForType<ErpReceiptProfile, ReceiptProfile>();
            config.ForType<ErpRelatedProduct, RelatedProduct>();
            //c.ForType<ErpReportConfiguration, ReportConfiguration>();
            config.ForType<ErpReportZone, ReportZone>();
            //c.ForType<ErpRetailCategoryMember, RetailCategoryMember>();
            //c.ForType<ErpRetailDiscount, RetailDiscount>();
            //c.ForType<ErpRetailDiscountLine, RetailDiscountLine>();
            //c.ForType<ErpRetailDiscountPriceGroup, RetailDiscountPriceGroup>();
            //c.ForType<ErpRetailImage, RetailImage>();
            config.ForType<ErpRichMediaLocations, RichMediaLocations>();
            config.ForType<ErpRichMediaLocationsRichMediaLocation, RichMediaLocationsRichMediaLocation>();
            config.ForType<ErpSalesAffiliationLoyaltyTier, SalesAffiliationLoyaltyTier>();

            config.ForType<ErpSalesInvoice, SalesInvoice>();
            config.ForType<ErpSalesInvoiceLine, SalesInvoiceLine>();
            //NS:VW Payment
            config.ForType<ErpSalesOrder, SalesOrder>()
                .ConstructUsing((Expression<Func<ErpSalesOrder, SalesOrder>>)(x => new SalesOrder()))
                .Ignore(dest => dest.OverriddenDepositAmount);
            //c.ForType<ErpSalesOrder, SalesOrder>()
            //    .ConstructUsing((Func<ErpSalesOrder, SalesOrder>)(x => new SalesOrder()))
            //    .ForMember(dest => dest.ExtensionProperties)
            //    .ForMember(dest => dest.OverriddenDepositAmount);
            config.ForType<SalesOrder, ErpSalesOrder>()
                .ConstructUsing((Expression<Func<SalesOrder, ErpSalesOrder>>)(x => new ErpSalesOrder()))
                //.ForMember(dest => dest.ExtensionProperties)
                .Ignore(dest => dest.OverriddenDepositAmount);
            config.ForType<ErpSalesLine, SalesLine>();
            //CreateMap, SalesLine>()
            //c.ForType<ErpSalesParameters, SalesParameters>();
            config.ForType<ErpSalesTaxGroup, SalesTaxGroup>();
            //c.ForType<ErpSalesTransaction, SalesTransaction>();
            //c.ForType<ErpSalesTransactionData, SalesTransactionData>();
            config.ForType<ErpShift, Shift>();
            config.ForType<ErpShiftAccountLine, ShiftAccountLine>();
            config.ForType<ErpShiftTenderLine, ShiftTenderLine>();
            //c.ForType<ErpShipment, Shipment>();
            //c.ForType<ErpShipmentLine, ShipmentLine>();
            //c.ForType<ErpShipmentLineMapping, ShipmentLineMapping>();
            //c.ForType<ErpShipmentProgress, ShipmentProgress>();
            //c.ForType<ErpShipmentPublishingStatus, ShipmentPublishingStatus>();
            //c.ForType<ErpShippingAdapterConfig, ShippingAdapterConfig>();
            config.ForType<ErpStateProvinceInfo, StateProvinceInfo>();
            config.ForType<ErpStockCountJournal, StockCountJournal>();
            config.ForType<ErpStockCountJournalTransaction, StockCountJournalTransaction>();
            //c.ForType<ErpStorageLookup, StorageLookup>();
            config.ForType<ErpSupportedLanguage, SupportedLanguage>();
            //c.ForType<ErpTaxableItem, TaxableItem>();
            //c.ForType<ErpTaxCodeInterval, TaxCodeInterval>();
            //c.ForType<ErpTaxCodeUnit, TaxCodeUnit>();
            //c.ForType<ErpTaxComponentIndia, TaxComponentIndia>();
            config.ForType<ErpTaxLine, TaxLine>()
                .Ignore(dest => dest.ExtensionProperties);
            config.ForType<ErpTaxOverride, TaxOverride>();
            //c.ForType<ErpTaxParameters, TaxParameters>();
            //c.ForType<ErpTaxSummarySettingIndia, TaxSummarySettingIndia>();
            config.ForType<ErpTenderDetail, TenderDetail>();
            //NS:VW Payment
            config.ForType<ErpTenderLine, TenderLine>();
            //c.ForType<ErpTenderLine, TenderLine>()
            //    .ForMember(dest => dest.ExtensionProperties);
            //c.ForType<ErpTenderLineBase, TenderLineBase>();
            config.ForType<ErpTenderType, TenderType>();
            //c.ForType<ErpTerminal, Terminal>();
            config.ForType<ErpTextValueTranslation, TextValueTranslation>();
            //c.ForType<ErpThresholdDiscountTier, ThresholdDiscountTier>();
            config.ForType<ErpTillLayout, TillLayout>();
            //c.ForType<ErpTrackingInfo, TrackingInfo>();
            //c.ForType<ErpTradeAgreement, TradeAgreement>();
            config.ForType<ErpTransaction, Transaction>();
            //c.ForType<ErpTransactionProperty, TransactionProperty>();
            //c.ForType<ErpTransactionServiceProfile, TransactionServiceProfile>();
            config.ForType<ErpTransferOrder, TransferOrder>();
            config.ForType<ErpTransferOrderLine, TransferOrderLine>();
            config.ForType<ErpUnitOfMeasure, UnitOfMeasure>();
            config.ForType<ErpUnitOfMeasureConversion, UnitOfMeasureConversion>();
            //c.ForType<ErpValidationPeriod, ValidationPeriod>();
            //c.ForType<ErpWeight, Weight>();
            config.ForType<ErpZipCodeInfo, ZipCodeInfo>();
            //c.ForType<ErpZoneReference, ZoneReference>();



            //For Quotation
            //config.ForType<ErpCustomerOrderInfo, CustomerOrderInfo>();
            //config.ForType<ErpPreauthorization, Preauthorization>();
            //config.ForType<ErpAffiliationInfo, AffiliationInfo>();
            //config.ForType<ErpChargeInfo, ChargeInfo>();
            //config.ForType<ErpItemInfo, ItemInfo>();
            //config.ForType<ErpPaymentInfo, PaymentInfo>();
            //config.ForType<ErpTaxInfo, TaxInfo>();
            //config.ForType<ErpDiscountInfo, DiscountInfo>();
            //config.ForType<ERPQuotationReasonGroup, QuotationReasonGroup>();


            //NS: D365 Update 12 Platform change start

            config.ForType<ErpAuditEvent, AuditEvent>();
            config.ForType<ErpComment, Comment>();
            config.ForType<ErpDenominationDetail, DenominationDetail>();
            config.ForType<ErpExtensibleAuditEventType, ExtensibleAuditEventType>();
            config.ForType<ErpFulfillmentLine, FulfillmentLine>();
            config.ForType<ErpFulfillmentLineDeliveryType, FulfillmentLineDeliveryType>();
            config.ForType<ErpFulfillmentLineSearchCriteria, FulfillmentLineSearchCriteria>();
            config.ForType<ErpFulfillmentLineStatus, FulfillmentLineStatus>();
            config.ForType<ErpInsufficientCredentialLengthException, InsufficientCredentialLengthException>();
            config.ForType<ErpNotificationDetail, NotificationDetail>();
            config.ForType<ErpNotificationItem, NotificationItem>();
            config.ForType<ErpOrderSearchCriteria, OrderSearchCriteria>();
            config.ForType<ErpOrgUnit, OrgUnit>();
            config.ForType<ErpPaymentMerchantInformation, PaymentMerchantInformation>();
            config.ForType<ErpProductBehavior, ProductBehavior>();
            config.ForType<ErpProductDimension, ProductDimension>();
            config.ForType<ErpProductDimensionCombination, ProductDimensionCombination>();
            config.ForType<ErpProductDimensionValue, ProductDimensionValue>();
            config.ForType<ErpReceiptRetrievalCriteria, ReceiptRetrievalCriteria>();
            config.ForType<ErpReturnLabelContent, ReturnLabelContent>();
            config.ForType<ErpSearchFilter, SearchFilter>();
            config.ForType<ErpSearchFilterValue, SearchFilterValue>();
            config.ForType<ErpShipFulfillmentLine, ShipFulfillmentLine>();
            config.ForType<ErpStaffPasswordExpiredException, StaffPasswordExpiredException>();
            config.ForType<ErpThirdPartyGiftCardInfo, ThirdPartyGiftCardInfo>();
            config.ForType<ErpTransactionSearchCriteria, TransactionSearchCriteria>();
            config.ForType<ErpTransferOrderJournal, TransferOrderJournal>();
            config.ForType<ErpTransferOrderJournalLine, TransferOrderJournalLine>();
            //Enums
            config.ForType<ErpAuditEventType, AuditEventType>();
            config.ForType<ErpAutoExitMethodType, AutoExitMethodType>();
            config.ForType<ErpEmployeeLogonType, EmployeeLogonType>();
            config.ForType<ErpInfoCodeActivity, InfoCodeActivity>();
            config.ForType<ErpRetailDenominationsToDisplay, RetailDenominationsToDisplay>();
            config.ForType<ErpSearchFilterType, SearchFilterType>();

            //NS: D365 Update 12 Platform change end

            //NS: D365 Update 8.1 Application change start
            config.ForType<ErpTaxMeasure, TaxMeasure>();
            config.ForType<ErpTransactionServiceAuthenticationType, TransactionServiceAuthenticationType>();
            config.ForType<ErpTaxCalculationType, TaxCalculationType>();
            config.ForType<ErpInvoiceType, InvoiceType>();
            config.ForType<ErpCustomerSearchField, CustomerSearchField>();
            config.ForType<ErpAddressPurpose, AddressPurpose>();
            config.ForType<ErpAuditEventFiscalTransaction, AuditEventFiscalTransaction>();
            config.ForType<ErpAuditEventUploadType, AuditEventUploadType>();
            config.ForType<ErpCustomerSearchByFieldCriteria, CustomerSearchByFieldCriteria>();
            config.ForType<ErpCustomerSearchByFieldCriterion, CustomerSearchByFieldCriterion>();
            config.ForType<ErpEnvironmentConfiguration, EnvironmentConfiguration>();
            config.ForType<ErpFiscalTransaction, FiscalTransaction>();
            config.ForType<ErpInvoiceSearchCriteria, InvoiceSearchCriteria>();
            config.ForType<ErpOrderInvoice, OrderInvoice>();
            config.ForType<ErpOrgUnitAddress, OrgUnitAddress>();
            config.ForType<ErpOrgUnitAvailabilitySearchCriteria, OrgUnitAvailabilitySearchCriteria>();
            config.ForType<ErpOrgUnitLocationSearchCriteria, OrgUnitLocationSearchCriteria>();
            config.ForType<ErpPackingSlipData, PackingSlipData>();
            config.ForType<ErpProductRefinerValue, ProductRefinerValue>();
            config.ForType<ErpRejectFulfillmentLine, RejectFulfillmentLine>();
            config.ForType<ErpShiftTaxLine, ShiftTaxLine>();
            config.ForType<ErpTaxLineGTE, TaxLineGTE>();
            config.ForType<ErpTaxMeasure, TaxMeasure>();
            config.ForType<ErpWarehouseDetails, WarehouseDetails>();
            config.ForType<ErpCustomerSearchFieldType, CustomerSearchFieldType>();
            //config.ForType<List<ErpCreateActionLinkRequest>, List<CreateLicenseRequest>>();
            //NS: D365 Update 8.1 Application change end
            //HK: D365 Update 10.0 Application change start
            config.ForType<ErpOrderShipments, OrderShipments>();
            config.ForType<ErpSuspendedCart, SuspendedCart>();
            config.ForType<ErpCartSearchCriteria, CartSearchCriteria>();
            config.ForType<ErpChargeCode, ChargeCode>();
            config.ForType<ErpStoreSafe, StoreSafe>();
            config.ForType<ErpLoyaltyRewardPointActivity, LoyaltyRewardPointActivity>();
            config.ForType<ErpChargeLineOverride, ChargeLineOverride>();
            config.ForType<ErpScaleUnitConfiguration, ScaleUnitConfiguration>();
            config.ForType<ErpPaymentError, PaymentError>();
            config.ForType<ErpFiscalTransactionTenderLineAdjustment, FiscalTransactionTenderLineAdjustment>();
            config.ForType<ErpFiscalTransactionSalesLineAdjustment, FiscalTransactionSalesLineAdjustment>();
            config.ForType<ErpFiscalIntegrationDocumentContext, FiscalIntegrationDocumentContext>();
            config.ForType<ErpFiscalIntegrationDocumentAdjustment, FiscalIntegrationDocumentAdjustment>();
            config.ForType<ErpFiscalIntegrationRegistrationProcessLine, FiscalIntegrationRegistrationProcessLine>();
            config.ForType<ErpFiscalIntegrationRegistrationSettings, FiscalIntegrationRegistrationSettings>();
            config.ForType<ErpAttributeGroupDefinition, AttributeGroupDefinition>();
            config.ForType<ErpAttributeGroupDefinitionCriteria, AttributeGroupDefinitionCriteria>();
            config.ForType<ErpAttributeGroupTranslationDetails, AttributeGroupTranslationDetails>();

            config.ForType<ReceiptTransactionType, ErpReceiptTransactionType>();
            config.ForType<ErpReceiptTransactionType, ReceiptTransactionType>();
            config.ForType<CreditCardProcessorStatus, ErpCreditCardProcessorStatus>();
            config.ForType<ErpCreditCardProcessorStatus, CreditCardProcessorStatus>();
            config.ForType<ErpFiscalIntegrationRegistrationStatus, FiscalIntegrationRegistrationStatus>();
            config.ForType<FiscalIntegrationRegistrationStatus, ErpFiscalIntegrationRegistrationStatus>();
            //config.ForType<ErpCreditCardCust, CreditCardCust>();
            //HK: D365 Update 10.0 Application change end
            #endregion

            #region Mapping for CL-Cart to CRT
            config.ForType<CLCart, Cart>();
            config.ForType<CLCartLine, CartLine>();
            config.ForType<Cart, CLCart>();
            config.ForType<CartLine, CLCartLine>();
            config.ForType<CLContractCartLine, CartLine>();
            config.ForType<CLAddress, Address>();
            config.ForType<CLDeliverySpecification, DeliverySpecification>();
            #endregion

            #region Mapster fix


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

            #endregion
            return config;
        }
    }
}