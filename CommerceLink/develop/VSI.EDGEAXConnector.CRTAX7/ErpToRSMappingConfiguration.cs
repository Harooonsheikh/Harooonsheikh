using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;


namespace VSI.EDGEAXConnector.CRTAX7
{
    public class ErpToRSMappingConfiguration : Profile
    {
        public ErpToRSMappingConfiguration()
        {
            Configure();
        }
              
        protected override void Configure()
        {
            //=============================
            //Mapping for RS to ERP
            //=============================
            #region Mapping for CRT to ERP
            Mapper.CreateMap<CommercePropertyValue, ErpCommercePropertyValue>();
            Mapper.CreateMap<CommerceProperty, ErpCommerceProperty>();
            Mapper.CreateMap<ClassInfo, ClassInfo>();
            //Mapper.CreateMap<AddressFilter, ErpAddressFilter>();
            Mapper.CreateMap<AddressFormatLineType, ErpAddressFormatLineType>();
            Mapper.CreateMap<AddressType, ErpAddressType>();
            Mapper.CreateMap<Address, ErpAddress>();
      
            //Mapper.CreateMap<Address, ErpAddress>()
            //    .ForMember(x => x.Name, opt => opt.UseValue("Address"))
            //    .ForMember(x => x.ExtensionProperties, opt => opt.Ignore());
            //Mapper.CreateMap<Alignment, ErpAlignment>();
            Mapper.CreateMap<AttributeDataType, ErpAttributeDataType>();
            Mapper.CreateMap<AttributeGroupType, ErpAttributeGroupType>();
            Mapper.CreateMap<BarcodeEntryMethodType, ErpBarcodeEntryMethodType>();
            Mapper.CreateMap<BarcodeMaskType, ErpBarcodeMaskType>();
            //Mapper.CreateMap<BarcodeSegmentType, ErpBarcodeSegmentType>();
            //Mapper.CreateMap<CalculationModes, ErpCalculationModes>();
            Mapper.CreateMap<CardType, ErpCardType>();
            Mapper.CreateMap<CartType, ErpCartType>();
            Mapper.CreateMap<CashType, ErpCashType>();
            //Mapper.CreateMap<ChangeAction, ErpChangeAction>();
            //Mapper.CreateMap<ChargeAccountType, ErpChargeAccountType>();
            //Mapper.CreateMap<ChargeDeliveryType, ErpChargeDeliveryType>();
            //Mapper.CreateMap<ChargeItemType, ErpChargeItemType>();
            //Mapper.CreateMap<ChargeLevel, ErpChargeLevel>();
            Mapper.CreateMap<ChargeMethod, ErpChargeMethod>();
            Mapper.CreateMap<ChargeModule, ErpChargeModule>();
            Mapper.CreateMap<ChargeType, ErpChargeType>();
            Mapper.CreateMap<CommerceEntityDataLevel, ErpCommerceEntityDataLevel>();
            //Mapper.CreateMap<CommerceListOperationType, ErpCommerceListOperationType>();
            Mapper.CreateMap<CommerceListType, ErpCommerceListType>();
            Mapper.CreateMap<ConcurrencyMode, ErpConcurrencyMode>();
            Mapper.CreateMap<ContactInformationType, ErpContactInformationType>();
            //Mapper.CreateMap<ContactInfoType, ErpContactInfoType>();
            //Mapper.CreateMap<CountryRegionISOCode, ErpCountryRegionISOCode>();
            Mapper.CreateMap<CustomerDiscountType, ErpCustomerDiscountType>();
            Mapper.CreateMap<CustomerOrderMode, ErpCustomerOrderMode>();
            Mapper.CreateMap<CustomerOrderType, ErpCustomerOrderType>();
            Mapper.CreateMap<CustomerType, ErpCustomerType>();
            //Mapper.CreateMap<DatabaseErrorCodes, ErpDatabaseErrorCodes>();
            //Mapper.CreateMap<DataStoreType, ErpDataStoreType>();
            //Mapper.CreateMap<DateValidationType, ErpDateValidationType>();
            //Mapper.CreateMap<DayMonthYear, ErpDayMonthYear>();
            Mapper.CreateMap<DeviceType, ErpDeviceType>();
            //Mapper.CreateMap<DiscountCalculationMode, ErpDiscountCalculationMode>();
            Mapper.CreateMap<DiscountLineType, ErpDiscountLineType>();
            //Mapper.CreateMap<DiscountOfferMethod, ErpDiscountOfferMethod>();
            Mapper.CreateMap<DistanceUnit, ErpDistanceUnit>();
            Mapper.CreateMap<DocumentStatus, ErpDocumentStatus>();
            Mapper.CreateMap<DualDisplayType, ErpDualDisplayType>();
            Mapper.CreateMap<EmployeeActivityType, ErpEmployeeActivityType>();
            Mapper.CreateMap<EmployeePriceOverrideType, ErpEmployeePriceOverrideType>();
            Mapper.CreateMap<GiftCardOperationType, ErpGiftCardOperationType>();
            Mapper.CreateMap<IncomeExpenseAccountType, ErpIncomeExpenseAccountType>();
            //Mapper.CreateMap<ItemAvailabilityPreferences, ErpItemAvailabilityPreferences>();
            //Mapper.CreateMap<KeyInPrices, ErpKeyInPrices>();
            //Mapper.CreateMap<KeyInQuantities, ErpKeyInQuantities>();
            Mapper.CreateMap<LineDiscountCalculationType, ErpLineDiscountCalculationType>();
            Mapper.CreateMap<LineMultilineDiscountOnItem, ErpLineMultilineDiscountOnItem>();
            //Mapper.CreateMap<ListingPublishingActionStatus, ErpListingPublishingActionStatus>();
            Mapper.CreateMap<LogOnConfiguration, ErpLogOnConfiguration>();
            //Mapper.CreateMap<LogOnType, ErpLogOnType>();//KAR
            //Mapper.CreateMap<LoyaltyActivityType, ErpLoyaltyActivityType>();
            Mapper.CreateMap<LoyaltyCardTenderType, ErpLoyaltyCardTenderType>();
            Mapper.CreateMap<LoyaltyRewardPointEntryType, ErpLoyaltyRewardPointEntryType>();
            Mapper.CreateMap<LoyaltyRewardPointType, ErpLoyaltyRewardPointType>();
            //Mapper.CreateMap<LoyaltyRewardType, ErpLoyaltyRewardType>();
            //Mapper.CreateMap<LoyaltyTransactionType, ErpLoyaltyTransactionType>();
            Mapper.CreateMap<ManualDiscountType, ErpManualDiscountType>();
            //Mapper.CreateMap<MultipleBuyDiscountMethod, ErpMultipleBuyDiscountMethod>();
            //Mapper.CreateMap<NonSalesTenderType, ErpNonSalesTenderType>();//KAR
            Mapper.CreateMap<NumberSequenceSeedType, ErpNumberSequenceSeedType>();
            //Mapper.CreateMap<OnlineChannelPublishStatusType, ErpOnlineChannelPublishStatusType>();
            //Mapper.CreateMap<PendingOrderStatus, ErpPendingOrderStatus>();
            Mapper.CreateMap<PeriodicDiscountOfferType, ErpPeriodicDiscountOfferType>();
            //Mapper.CreateMap<PriceDiscountAccountCode, ErpPriceDiscountAccountCode>();
            //Mapper.CreateMap<PriceDiscountItemCode, ErpPriceDiscountItemCode>();
            //Mapper.CreateMap<PriceDiscountType, ErpPriceDiscountType>();
            //Mapper.CreateMap<PriceMethod, ErpPriceMethod>();
            //Mapper.CreateMap<PriceType, ErpPriceType>();
            //Mapper.CreateMap<PricingCalculationMode, ErpPricingCalculationMode>();
            Mapper.CreateMap<PrintBehavior, ErpPrintBehavior>();
            Mapper.CreateMap<PrinterLogoAlignmentType, ErpPrinterLogoAlignmentType>();
            Mapper.CreateMap<PrinterLogotype, ErpPrinterLogotype>();
            //Mapper.CreateMap<ProductAttributeRelationType, ErpProductAttributeRelationType>();
            //Mapper.CreateMap<ProductAttributeValueSource, ErpProductAttributeValueSource>();
            Mapper.CreateMap<ProductPropertyType, ErpProductPropertyType>();
            Mapper.CreateMap<ProductSource, ErpProductSource>();
            //Mapper.CreateMap<PublishingAction, ErpPublishingAction>();
            Mapper.CreateMap<PurchaseTransferOrderType, ErpPurchaseTransferOrderType>();
            Mapper.CreateMap<ReasonCodeInputRequiredType, ErpReasonCodeInputRequiredType>();
            Mapper.CreateMap<ReasonCodeInputType, ErpReasonCodeInputType>();
            Mapper.CreateMap<ReasonCodeLineType, ErpReasonCodeLineType>();
            Mapper.CreateMap<ReasonCodeSourceType, ErpReasonCodeSourceType>();
            //Mapper.CreateMap<ReasonCodeTableRefType, ErpReasonCodeTableRefType>();
            //Mapper.CreateMap<ReceiptTaxDetailsTypeIndia, ErpReceiptTaxDetailsTypeIndia>();
            //Mapper.CreateMap<ReceiptTransactionType, ErpReceiptTransactionType>();
            Mapper.CreateMap<ReceiptType, ErpReceiptType>();
            Mapper.CreateMap<RetailAffiliationType, ErpRetailAffiliationType>();
            //Mapper.CreateMap<RetailChannelProductAttributeId, ErpRetailChannelProductAttributeId>();
            Mapper.CreateMap<RetailChannelType, ErpRetailChannelType>();
            Mapper.CreateMap<RetailOperation, ErpRetailOperation>();
            //Mapper.CreateMap<RetailProductChannelProductAttributeId, ErpRetailProductChannelProductAttributeId>();
            Mapper.CreateMap<RoundingMethod, ErpRoundingMethod>();
            Mapper.CreateMap<SalesInvoiceType, ErpSalesInvoiceType>();
            Mapper.CreateMap<SalesStatus, ErpSalesStatus>();
            Mapper.CreateMap<SalesTransactionType, ErpSalesTransactionType>();
            Mapper.CreateMap<SearchLocation, ErpSearchLocation>();
            Mapper.CreateMap<ShiftStatus, ErpShiftStatus>();
            //Mapper.CreateMap<ShipmentPublishingActionStatus, ErpShipmentPublishingActionStatus>();
            //Mapper.CreateMap<ShippingOptionType, ErpShippingOptionType>();NewCRT
            //Mapper.CreateMap<SortOrderType, ErpSortOrderType>();
            Mapper.CreateMap<StatementMethod, ErpStatementMethod>();
            //Mapper.CreateMap<TaxableBasisIndia, ErpTaxableBasisIndia>();
            Mapper.CreateMap<TaxOverrideBy, ErpTaxOverrideBy>();
            Mapper.CreateMap<TaxOverrideType, ErpTaxOverrideType>();
            //Mapper.CreateMap<TaxTypeIndia, ErpTaxTypeIndia>();
            //Mapper.CreateMap<TenderDropAndDeclareType, ErpTenderDropAndDeclareType>();//KAR
            Mapper.CreateMap<TenderLineStatus, ErpTenderLineStatus>();
            //Mapper.CreateMap<ThresholdDiscountMethod, ErpThresholdDiscountMethod>();
            //Mapper.CreateMap<TransactionOperationType, ErpTransactionOperationType>();
            //Mapper.CreateMap<TransactionServiceProtocol, ErpTransactionServiceProtocol>();
            Mapper.CreateMap<TransactionStatus, ErpTransactionStatus>();
            Mapper.CreateMap<TransactionType, ErpTransactionType>();
            Mapper.CreateMap<TriggerFunctionType, ErpTriggerFunctionType>();
            //Mapper.CreateMap<AccentColor, ErpAccentColor>();
            //Mapper.CreateMap<AddressBookPartyData, ErpAddressBookPartyData>();
            Mapper.CreateMap<AddressFormattingInfo, ErpAddressFormattingInfo>();
            Mapper.CreateMap<Affiliation, ErpAffiliation>();
            Mapper.CreateMap<PagedResult<Affiliation>, PagedResult<ErpAffiliation>>();
            Mapper.CreateMap<AffiliationLoyaltyTier, ErpAffiliationLoyaltyTier>();
            Mapper.CreateMap<ARGBColor, ErpARGBColor>();
            //Mapper.CreateMap<AttributeBase, ErpAttributeBase>();
            //Mapper.CreateMap<AttributeCategory, ErpAttributeCategory>();
            //Mapper.CreateMap<AttributeGroup, ErpAttributeGroup>();
            //.CreateMap<AttributeNameTranslation, ErpAttributeNameTranslation>();
            Mapper.CreateMap<AttributeValueBase, ErpAttributeValueBase>();
               // .ConstructUsing((Func<AttributeValueBase, ErpAttributeValueBase>)(x => new ErpAttributeValueBase()));
            Mapper.CreateMap<Barcode, ErpBarcode>();
            //Mapper.CreateMap<BarcodeMask, ErpBarcodeMask>();
            //Mapper.CreateMap<BarcodeMaskSegment, ErpBarcodeMaskSegment>();
            Mapper.CreateMap<ButtonGrid, ErpButtonGrid>();
            Mapper.CreateMap<ButtonGridButton, ErpButtonGridButton>();
            Mapper.CreateMap<ButtonGridZone, ErpButtonGridZone>();
            Mapper.CreateMap<CardTypeInfo, ErpCardTypeInfo>();
            Mapper.CreateMap<Cart, ErpCart>();
            Mapper.CreateMap<CartLine, ErpCartLine>();
            //Mapper.CreateMap<CartLineData, ErpCartLineData>();
            Mapper.CreateMap<CartTenderLine, ErpCartTenderLine>();
            Mapper.CreateMap<CashDeclaration, ErpCashDeclaration>();
            Mapper.CreateMap<Category, ErpCategory>();
            //.ForMember(desc => desc.Image, opt => opt.Ignore())
            //.ForMember(desc => desc.ExtensionProperties, opt => opt.Ignore())
            //.ForMember(desc => desc.ExtensionData, opt => opt.Ignore())
            //.ForMember(desc => desc.NameTranslations, opt => opt.Ignore()); //TODO: Remove Ignore Part;
            //Mapper.CreateMap<CategoryNameTranslation, ErpCategoryNameTranslation>();
            //Mapper.CreateMap<Channel, ErpChannel>();
            Mapper.CreateMap<ChannelConfiguration, ErpChannelConfiguration>();
            Mapper.CreateMap<ChannelLanguage, ErpChannelLanguage>();
            //Mapper.CreateMap<ChannelPriceConfiguration, ErpChannelPriceConfiguration>();
            //Mapper.CreateMap<ChannelProfile, ErpChannelProfile>();
            Mapper.CreateMap<ChannelProfileProperty, ErpChannelProfileProperty>();
            Mapper.CreateMap<ChannelProperty, ErpChannelProperty>();
            //Mapper.CreateMap<ChargeConfiguration, ErpChargeConfiguration>();
            Mapper.CreateMap<ChargeLine, ErpChargeLine>();
            Mapper.CreateMap<CityInfo, ErpCityInfo>();
            Mapper.CreateMap<CommerceEntity, ErpCommerceEntity>();
            //Mapper.CreateMap<CommerceEntityChangeTrackingInformation, ErpCommerceEntityChangeTrackingInformation>();
            Mapper.CreateMap<CommerceList, ErpCommerceList>();
            Mapper.CreateMap<CommerceListLine, ErpCommerceListLine>();
            Mapper.CreateMap<CommerceProperty, ErpCommerceProperty>();
            Mapper.CreateMap<CommercePropertyValue, ErpCommercePropertyValue>();
            Mapper.CreateMap<ComponentKitVariantSet, ErpComponentKitVariantSet>();
            //Mapper.CreateMap<ContactInfo, ErpContactInfo>();
            Mapper.CreateMap<ContactInformation, ErpContactInformation>();
            Mapper.CreateMap<CountryRegionInfo, ErpCountryRegionInfo>();
            Mapper.CreateMap<CountyInfo, ErpCountyInfo>();
            Mapper.CreateMap<CreditMemo, ErpCreditMemo>();
            Mapper.CreateMap<Currency, ErpCurrency>();
            Mapper.CreateMap<CurrencyAmount, ErpCurrencyAmount>();
            Mapper.CreateMap<CurrencyRequest, ErpCurrencyRequest>();
            Mapper.CreateMap<Customer, ErpCustomer>();

            Mapper.CreateMap<ErpCustomer, Customer>().ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<CustomerAffiliation, ErpCustomerAffiliation>();
            Mapper.CreateMap<CustomerGroup, ErpCustomerGroup>();
            //Mapper.CreateMap<CustomerLoyaltyCard, ErpCustomerLoyaltyCard>();
            Mapper.CreateMap<DeliveryOption, ErpDeliveryOption>();
            Mapper.CreateMap<Device, ErpDevice>();
            Mapper.CreateMap<DeviceConfiguration, ErpDeviceConfiguration>();
            Mapper.CreateMap<DiscountCode, ErpDiscountCode>();
            Mapper.CreateMap<DiscountLine, ErpDiscountLine>();
            Mapper.CreateMap<DistrictInfo, ErpDistrictInfo>();
            Mapper.CreateMap<Employee, ErpEmployee>();
            Mapper.CreateMap<EmployeeActivity, ErpEmployeeActivity>();
            Mapper.CreateMap<EmployeePermissions, ErpEmployeePermissions>();
            //Mapper.CreateMap<ExchangeRate, ErpExchangeRate>();
            //Mapper.CreateMap<FormulaIndia, ErpFormulaIndia>();
            Mapper.CreateMap<GiftCard, ErpGiftCard>();
            Mapper.CreateMap<GlobalCustomer, ErpGlobalCustomer>();
            Mapper.CreateMap<HardwareProfile, ErpHardwareProfile>();
            Mapper.CreateMap<HardwareProfileCashDrawer, ErpHardwareProfileCashDrawer>();
            Mapper.CreateMap<HardwareProfilePrinter, ErpHardwareProfilePrinter>();
            Mapper.CreateMap<HardwareProfileScanner, ErpHardwareProfileScanner>();
            Mapper.CreateMap<ImageZone, ErpImageZone>();
            Mapper.CreateMap<IncomeExpenseAccount, ErpIncomeExpenseAccount>();
            Mapper.CreateMap<IncomeExpenseLine, ErpIncomeExpenseLine>();
            //Mapper.CreateMap<Item, ErpItem>();
            Mapper.CreateMap<ItemAvailability, ErpItemAvailability>();
            //Mapper.CreateMap<ItemAvailableQuantity, ErpItemAvailableQuantity>();
            Mapper.CreateMap<ItemBarcode, ErpItemBarcode>();
            //Mapper.CreateMap<ItemDimensions, ErpItemDimensions>();
            //Mapper.CreateMap<ItemMaxRetailPriceIndia, ErpItemMaxRetailPriceIndia>();
            //Mapper.CreateMap<ItemReservation, ErpItemReservation>();
            Mapper.CreateMap<KitComponent, ErpKitComponent>();
            Mapper.CreateMap<KitComponentKey, ErpKitComponentKey>();
            //Mapper.CreateMap<KitConfigToComponentAssociation, ErpKitConfigToComponentAssociation>();
            Mapper.CreateMap<KitDefinition, ErpKitDefinition>();
            Mapper.CreateMap<KitLineDefinition, ErpKitLineDefinition>();
            Mapper.CreateMap<KitLineProductProperty, ErpKitLineProductProperty>();
            //Mapper.CreateMap<KitLineProductPropertyDictionary, ErpKitLineProductPropertyDictionary>();
            Mapper.CreateMap<KitTransactionLine, ErpKitTransactionLine>();
            Mapper.CreateMap<KitVariantContent, ErpKitVariantContent>();
            //Mapper.CreateMap<KitVariantToComponentDictionary, ErpKitVariantToComponentDictionary>();
            Mapper.CreateMap<LinkedProduct, ErpLinkedProduct>();
            //Mapper.CreateMap<ListingPublishStatus, ErpListingPublishStatus>();
            Mapper.CreateMap<LocalizedString, ErpLocalizedString>();
            Mapper.CreateMap<LoyaltyCard, ErpLoyaltyCard>();
            Mapper.CreateMap<LoyaltyCardTier, ErpLoyaltyCardTier>();
            Mapper.CreateMap<LoyaltyGroup, ErpLoyaltyGroup>();
            Mapper.CreateMap<LoyaltyRewardPoint, ErpLoyaltyRewardPoint>();
            Mapper.CreateMap<LoyaltyRewardPointLine, ErpLoyaltyRewardPointLine>();
            //Mapper.CreateMap<LoyaltySchemeLineEarn, ErpLoyaltySchemeLineEarn>();
            //Mapper.CreateMap<LoyaltySchemeLineRedeem, ErpLoyaltySchemeLineRedeem>();
            Mapper.CreateMap<LoyaltyTier, ErpLoyaltyTier>();
            //Mapper.CreateMap<MixAndMatchLineGroup, ErpMixAndMatchLineGroup>();
            //Mapper.CreateMap<Notification, ErpNotification>();//KAR
            Mapper.CreateMap<OperationPermission, ErpOperationPermission>();
            Mapper.CreateMap<OrgUnitAvailability, ErpOrgUnitAvailability>();
            //Mapper.CreateMap<OrgUnitContact, ErpOrgUnitContact>();
            Mapper.CreateMap<OrgUnitLocation, ErpOrgUnitLocation>();
            Mapper.CreateMap<ParameterSet, ErpParameterSet>();
            Mapper.CreateMap<PaymentCard, ErpPaymentCard>();
            //Mapper.CreateMap<PaymentConnectorConfiguration, ErpPaymentConnectorConfiguration>();
            //Mapper.CreateMap<PeriodicDiscount, ErpPeriodicDiscount>();
            Mapper.CreateMap<PickingList, ErpPickingList>();
            Mapper.CreateMap<PickingListLine, ErpPickingListLine>();
            //Mapper.CreateMap<PriceAdjustment, ErpPriceAdjustment>();
            //Mapper.CreateMap<PriceGroup, ErpPriceGroup>();
            //Mapper.CreateMap<PriceLine, ErpPriceLine>();
            //Mapper.CreateMap<PriceParameters, ErpPriceParameters>();
            Mapper.CreateMap<Printer, ErpPrinter>();
            Mapper.CreateMap<Product, ErpProduct>()
                .ForMember(desc => desc.VariantId, opt => opt.MapFrom(src => string.Empty));

            Mapper.CreateMap<ProductSearchResult, ErpProduct>()
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.Name));

            //.ForMember(desc => desc.Image, opt => opt.Ignore()); //TODO: Remove Ignore Part
            //Mapper.CreateMap<ProductAttributeSchemaEntry, ErpProductAttributeSchemaEntry>();
            Mapper.CreateMap<ProductAvailableQuantity, ErpProductAvailableQuantity>();
            Mapper.CreateMap<ProductCatalog, ErpProductCatalog>();
            //Mapper.CreateMap<ProductCatalogAssociation, ErpProductCatalogAssociation>();
            //Mapper.CreateMap<ProductCategoryAssociation, ErpProductCategoryAssociation>();
            //Mapper.CreateMap<ProductChangeTrackingAnchorSet, ErpProductChangeTrackingAnchorSet>();
            Mapper.CreateMap<ProductChangeTrackingInformation, ErpProductChangeTrackingInformation>();
            Mapper.CreateMap<ProductCompositionInformation, ErpProductCompositionInformation>();
            //Mapper.CreateMap<ProductDimensionDictionary, ErpProductDimensionDictionary>();
            Mapper.CreateMap<ProductDimensionSet, ErpProductDimensionSet>();

            //Mapper.CreateMap<ProductDimensionValueDictionary, ErpProductDimensionValueDictionaryEntry>();
            //for CU 9 
            //Mapper.CreateMap<ProductDimensionValueDictionaryEntry, ErpProductDimensionValueDictionaryEntry>();

            Mapper.CreateMap<ProductDimensionValueSet, ErpProductDimensionValueSet>();
            //Mapper.CreateMap<ProductExistenceId, ErpProductExistenceId>();
            //Mapper.CreateMap<ProductIdentity, ErpProductIdentity>();
            Mapper.CreateMap<DateTimeOffset, DateTime>()
              .ConvertUsing<NullableDateTimeConverter>();
            Mapper.CreateMap<ProductPrice, ErpProductPrice>();
           // .ForMember(dest => dest.ValidFrom,opt => opt.MapFrom(src => Convert.ToDateTime(src.ValidFrom)));
            Mapper.CreateMap<ProductProperty, ErpProductProperty>();
            //Mapper.CreateMap<ProductPropertyDictionary, ErpProductPropertyDictionary>();
            //Mapper.CreateMap<ProductPropertySchema, ErpProductPropertySchema>();
            Mapper.CreateMap<ProductPropertyTranslation, ErpProductPropertyTranslation>();
            //Mapper.CreateMap<ProductPropertyTranslationDictionary, ErpProductPropertyTranslationDictionary>();
            Mapper.CreateMap<ProductRules, ErpProductRules>();
            //Mapper.CreateMap<ProductToKitVariantDictionary, ErpProductToKitVariantDictionary>();
            //Mapper.CreateMap<ProductVariant, ErpProductVariant>();
            Mapper.CreateMap<ProductVariant, ErpProductVariant>()
                .ConstructUsing((Func<ProductVariant, ErpProductVariant>)(x => new ErpProductVariant()))
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore())
                //.ForMember(dest => dest.IndexedProperties, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.PropertiesAsList, opt => opt.Ignore());
            //Mapper.CreateMap<ProductVariantDictionary, ErpProductVariantDictionary>();
            Mapper.CreateMap<ProductVariantInformation, ErpProductVariantInformation>();
            Mapper.CreateMap<ProjectionDomain, ErpProjectionDomain>();
            Mapper.CreateMap<PurchaseOrder, ErpPurchaseOrder>();
            Mapper.CreateMap<PurchaseOrderLine, ErpPurchaseOrderLine>();
            //Mapper.CreateMap<QuantityDiscountLevel, ErpQuantityDiscountLevel>();
            Mapper.CreateMap<ReasonCode, ErpReasonCode>();
            Mapper.CreateMap<ReasonCodeLine, ErpReasonCodeLine>();
              
            Mapper.CreateMap<ReasonCodeRequirement, ErpReasonCodeRequirement>();
            //Mapper.CreateMap<ReasonCodeSettings, ErpReasonCodeSettings>();
            //Mapper.CreateMap<ReasonCodeSpecific, ErpReasonCodeSpecific>();
            Mapper.CreateMap<ReasonSubCode, ErpReasonSubCode>();
            Mapper.CreateMap<Receipt, ErpReceipt>();
            //Mapper.CreateMap<ReceiptHeaderInfoIndia, ErpReceiptHeaderInfoIndia>();
            //Mapper.CreateMap<ReceiptHeaderTaxInfoIndia, ErpReceiptHeaderTaxInfoIndia>();
            //Mapper.CreateMap<ReceiptInfo, ErpReceiptInfo>();
            //Mapper.CreateMap<ReceiptMask, ErpReceiptMask>();
            //Mapper.CreateMap<ReceiptProfile, ErpReceiptProfile>();
            Mapper.CreateMap<RelatedProduct, ErpRelatedProduct>();
            //Mapper.CreateMap<ReportConfiguration, ErpReportConfiguration>();
            Mapper.CreateMap<ReportZone, ErpReportZone>();
            //Mapper.CreateMap<RetailCategoryMember, ErpRetailCategoryMember>();
            //Mapper.CreateMap<RetailDiscount, ErpRetailDiscount>();
            //Mapper.CreateMap<RetailDiscountLine, ErpRetailDiscountLine>();
            //Mapper.CreateMap<RetailDiscountPriceGroup, ErpRetailDiscountPriceGroup>();
            //Mapper.CreateMap<RetailImage, ErpRetailImage>();
            Mapper.CreateMap<RichMediaLocations, ErpRichMediaLocations>();
            Mapper.CreateMap<RichMediaLocationsRichMediaLocation, ErpRichMediaLocationsRichMediaLocation>();
            Mapper.CreateMap<SalesAffiliationLoyaltyTier, ErpSalesAffiliationLoyaltyTier>();
                // .ConstructUsing((Func<SalesAffiliationLoyaltyTier, ErpSalesAffiliationLoyaltyTier>)(x => new ErpSalesAffiliationLoyaltyTier()));
            Mapper.CreateMap<SalesInvoice, ErpSalesInvoice>();
            Mapper.CreateMap<SalesInvoiceLine, ErpSalesInvoiceLine>();
            Mapper.CreateMap<SalesLine, ErpSalesLine>();
            //Mapper.CreateMap<SalesParameters, ErpSalesParameters>();
            Mapper.CreateMap<SalesTaxGroup, ErpSalesTaxGroup>();
            //Mapper.CreateMap<SalesTransaction, ErpSalesTransaction>();
            //Mapper.CreateMap<SalesTransactionData, ErpSalesTransactionData>();
            Mapper.CreateMap<Shift, ErpShift>();
            Mapper.CreateMap<ShiftAccountLine, ErpShiftAccountLine>();
            Mapper.CreateMap<ShiftTenderLine, ErpShiftTenderLine>();
            //Mapper.CreateMap<Shipment, ErpShipment>();
            //Mapper.CreateMap<ShipmentLine, ErpShipmentLine>();
            //Mapper.CreateMap<ShipmentLineMapping, ErpShipmentLineMapping>();
            //Mapper.CreateMap<ShipmentProgress, ErpShipmentProgress>();
            //Mapper.CreateMap<ShipmentPublishingStatus, ErpShipmentPublishingStatus>();
            //Mapper.CreateMap<ShippingAdapterConfig, ErpShippingAdapterConfig>();
            Mapper.CreateMap<StateProvinceInfo, ErpStateProvinceInfo>();
            Mapper.CreateMap<StockCountJournal, ErpStockCountJournal>();
            Mapper.CreateMap<StockCountJournalTransaction, ErpStockCountJournalTransaction>();
            //Mapper.CreateMap<StorageLookup, ErpStorageLookup>();
            Mapper.CreateMap<SupportedLanguage, ErpSupportedLanguage>();
            //Mapper.CreateMap<TaxableItem, ErpTaxableItem>();
            //Mapper.CreateMap<TaxCodeInterval, ErpTaxCodeInterval>();
            //Mapper.CreateMap<TaxCodeUnit, ErpTaxCodeUnit>();
            //Mapper.CreateMap<TaxComponentIndia, ErpTaxComponentIndia>();
            Mapper.CreateMap<TaxLine, ErpTaxLine>();
            Mapper.CreateMap<TaxOverride, ErpTaxOverride>();
            //Mapper.CreateMap<TaxParameters, ErpTaxParameters>();
            //Mapper.CreateMap<TaxSummarySettingIndia, ErpTaxSummarySettingIndia>();
            Mapper.CreateMap<TenderDetail, ErpTenderDetail>();
            Mapper.CreateMap<TenderLine, ErpTenderLine>();
            //Mapper.CreateMap<TenderLineBase, ErpTenderLineBase>();
            Mapper.CreateMap<TenderType, ErpTenderType>();
            //Mapper.CreateMap<Terminal, ErpTerminal>();
            Mapper.CreateMap<TextValueTranslation, ErpTextValueTranslation>();
            //Mapper.CreateMap<ThresholdDiscountTier, ErpThresholdDiscountTier>();
            Mapper.CreateMap<TillLayout, ErpTillLayout>();
            //Mapper.CreateMap<TrackingInfo, ErpTrackingInfo>();
            //Mapper.CreateMap<TradeAgreement, ErpTradeAgreement>();
            Mapper.CreateMap<Transaction, ErpTransaction>();
            //Mapper.CreateMap<TransactionProperty, ErpTransactionProperty>();
            //Mapper.CreateMap<TransactionServiceProfile, ErpTransactionServiceProfile>();
            Mapper.CreateMap<TransferOrder, ErpTransferOrder>();
            Mapper.CreateMap<TransferOrderLine, ErpTransferOrderLine>();
            Mapper.CreateMap<UnitOfMeasure, ErpUnitOfMeasure>();
            Mapper.CreateMap<UnitOfMeasureConversion, ErpUnitOfMeasureConversion>();
            //Mapper.CreateMap<ValidationPeriod, ErpValidationPeriod>();
            //Mapper.CreateMap<Weight, ErpWeight>();
            Mapper.CreateMap<ZipCodeInfo, ErpZipCodeInfo>();
            //Mapper.CreateMap<ZoneReference, ErpZoneReference>();


            Mapper.CreateMap<List<KeyValuePair<long, IEnumerable<Product>>>, List<KeyValuePair<long, IEnumerable<ErpProduct>>>>();

            #endregion

            //=============================
            //Mapping for ERP to RS
            //=============================
            #region Mapping for ERP to CRT
            Mapper.CreateMap<ErpCommercePropertyValue, CommercePropertyValue>();
            Mapper.CreateMap<ErpCommerceProperty, CommerceProperty>();
            //Mapper.CreateMap<ErpClassInfo, ClassInfo>();
            //Mapper.CreateMap<ErpAddressFilter, AddressFilter>();
            Mapper.CreateMap<ErpAddressFormatLineType, AddressFormatLineType>();
            Mapper.CreateMap<ErpAddressType, AddressType>();
            //Mapper.CreateMap<ErpAlignment, Alignment>();
            Mapper.CreateMap<ErpAttributeDataType, AttributeDataType>();
            Mapper.CreateMap<ErpAttributeGroupType, AttributeGroupType>();
            Mapper.CreateMap<ErpBarcodeEntryMethodType, BarcodeEntryMethodType>();
            Mapper.CreateMap<ErpBarcodeMaskType, BarcodeMaskType>();
            //Mapper.CreateMap<ErpBarcodeSegmentType, BarcodeSegmentType>();
            //Mapper.CreateMap<ErpCalculationModes, CalculationModes>();
            Mapper.CreateMap<ErpCardType, CardType>();
            Mapper.CreateMap<ErpCartType, CartType>();
            Mapper.CreateMap<ErpCashType, CashType>();
            //Mapper.CreateMap<ErpChangeAction, ChangeAction>();
            //Mapper.CreateMap<ErpChargeAccountType, ChargeAccountType>();
            //Mapper.CreateMap<ErpChargeDeliveryType, ChargeDeliveryType>();
            //Mapper.CreateMap<ErpChargeItemType, ChargeItemType>();
            //Mapper.CreateMap<ErpChargeLevel, ChargeLevel>();
            Mapper.CreateMap<ErpChargeMethod, ChargeMethod>();
            Mapper.CreateMap<ErpChargeModule, ChargeModule>();
            Mapper.CreateMap<ErpChargeType, ChargeType>();
            Mapper.CreateMap<ErpCommerceEntityDataLevel, CommerceEntityDataLevel>();
            //Mapper.CreateMap<ErpCommerceListOperationType, CommerceListOperationType>();
            Mapper.CreateMap<ErpCommerceListType, CommerceListType>();
            Mapper.CreateMap<ErpConcurrencyMode, ConcurrencyMode>();
            Mapper.CreateMap<ErpContactInformationType, ContactInformationType>();
            //Mapper.CreateMap<ErpContactInfoType, ContactInfoType>();
            //Mapper.CreateMap<ErpCountryRegionISOCode, CountryRegionISOCode>();
            Mapper.CreateMap<ErpCustomerDiscountType, CustomerDiscountType>();
            Mapper.CreateMap<ErpCustomerOrderMode, CustomerOrderMode>();
            Mapper.CreateMap<ErpCustomerOrderType, CustomerOrderType>();
            Mapper.CreateMap<ErpCustomerType, CustomerType>();
            //Mapper.CreateMap<ErpDatabaseErrorCodes, DatabaseErrorCodes>();
            //Mapper.CreateMap<ErpDataStoreType, DataStoreType>();
            //Mapper.CreateMap<ErpDateValidationType, DateValidationType>();
            //Mapper.CreateMap<ErpDayMonthYear, DayMonthYear>();
            Mapper.CreateMap<ErpDeviceType, DeviceType>();
            //Mapper.CreateMap<ErpDiscountCalculationMode, DiscountCalculationMode>();
            Mapper.CreateMap<ErpDiscountLineType, DiscountLineType>();
            //Mapper.CreateMap<ErpDiscountOfferMethod, DiscountOfferMethod>();
            Mapper.CreateMap<ErpDistanceUnit, DistanceUnit>();
            Mapper.CreateMap<ErpDocumentStatus, DocumentStatus>();
            Mapper.CreateMap<ErpDualDisplayType, DualDisplayType>();
            Mapper.CreateMap<ErpEmployeeActivityType, EmployeeActivityType>();
            Mapper.CreateMap<ErpEmployeePriceOverrideType, EmployeePriceOverrideType>();
            Mapper.CreateMap<ErpGiftCardOperationType, GiftCardOperationType>();
            Mapper.CreateMap<ErpIncomeExpenseAccountType, IncomeExpenseAccountType>();
            //Mapper.CreateMap<ErpItemAvailabilityPreferences, ItemAvailabilityPreferences>();
            //Mapper.CreateMap<ErpKeyInPrices, KeyInPrices>();
            //Mapper.CreateMap<ErpKeyInQuantities, KeyInQuantities>();
            Mapper.CreateMap<ErpLineDiscountCalculationType, LineDiscountCalculationType>();
            Mapper.CreateMap<ErpLineMultilineDiscountOnItem, LineMultilineDiscountOnItem>();
            //Mapper.CreateMap<ErpListingPublishingActionStatus, ListingPublishingActionStatus>();
            Mapper.CreateMap<ErpLogOnConfiguration, LogOnConfiguration>();
            //Mapper.CreateMap<ErpLogOnType, LogOnType>();//KAR
            //Mapper.CreateMap<ErpLoyaltyActivityType, LoyaltyActivityType>();
            Mapper.CreateMap<ErpLoyaltyCardTenderType, LoyaltyCardTenderType>();
            Mapper.CreateMap<ErpLoyaltyRewardPointEntryType, LoyaltyRewardPointEntryType>();
            Mapper.CreateMap<ErpLoyaltyRewardPointType, LoyaltyRewardPointType>();
            //Mapper.CreateMap<ErpLoyaltyRewardType, LoyaltyRewardType>();
            //Mapper.CreateMap<ErpLoyaltyTransactionType, LoyaltyTransactionType>();
            Mapper.CreateMap<ErpManualDiscountType, ManualDiscountType>();
            //Mapper.CreateMap<ErpMultipleBuyDiscountMethod, MultipleBuyDiscountMethod>();
            //Mapper.CreateMap<ErpNonSalesTenderType, NonSalesTenderType>();//KAR
            Mapper.CreateMap<ErpNumberSequenceSeedType, NumberSequenceSeedType>();
            //Mapper.CreateMap<ErpOnlineChannelPublishStatusType, OnlineChannelPublishStatusType>();
            //Mapper.CreateMap<ErpPendingOrderStatus, PendingOrderStatus>();
            Mapper.CreateMap<ErpPeriodicDiscountOfferType, PeriodicDiscountOfferType>();
            //Mapper.CreateMap<ErpPriceDiscountAccountCode, PriceDiscountAccountCode>();
            //Mapper.CreateMap<ErpPriceDiscountItemCode, PriceDiscountItemCode>();
            //Mapper.CreateMap<ErpPriceDiscountType, PriceDiscountType>();
            //Mapper.CreateMap<ErpPriceMethod, PriceMethod>();
            //Mapper.CreateMap<ErpPriceType, PriceType>();
            //Mapper.CreateMap<ErpPricingCalculationMode, PricingCalculationMode>();
            Mapper.CreateMap<ErpPrintBehavior, PrintBehavior>();
            Mapper.CreateMap<ErpPrinterLogoAlignmentType, PrinterLogoAlignmentType>();
            Mapper.CreateMap<ErpPrinterLogotype, PrinterLogotype>();
            //Mapper.CreateMap<ErpProductAttributeRelationType, ProductAttributeRelationType>();
            //Mapper.CreateMap<ErpProductAttributeValueSource, ProductAttributeValueSource>();
            Mapper.CreateMap<ErpProductPropertyType, ProductPropertyType>();
            Mapper.CreateMap<ErpProductSource, ProductSource>()
                .ConstructUsing((Func<ErpProductSource, ProductSource>)(x => new ProductSource()));
            //Mapper.CreateMap<ErpPublishingAction, PublishingAction>();
            Mapper.CreateMap<ErpPurchaseTransferOrderType, PurchaseTransferOrderType>();
            Mapper.CreateMap<ErpReasonCodeInputRequiredType, ReasonCodeInputRequiredType>();
            Mapper.CreateMap<ErpReasonCodeInputType, ReasonCodeInputType>();
            Mapper.CreateMap<ErpReasonCodeLineType, ReasonCodeLineType>();
            Mapper.CreateMap<ErpReasonCodeSourceType, ReasonCodeSourceType>();
            //Mapper.CreateMap<ErpReasonCodeTableRefType, ReasonCodeTableRefType>();
            //Mapper.CreateMap<ErpReceiptTaxDetailsTypeIndia, ReceiptTaxDetailsTypeIndia>();
            //Mapper.CreateMap<ErpReceiptTransactionType, ReceiptTransactionType>();
            Mapper.CreateMap<ErpReceiptType, ReceiptType>();
            Mapper.CreateMap<ErpRetailAffiliationType, RetailAffiliationType>();
            //Mapper.CreateMap<ErpRetailChannelProductAttributeId, RetailChannelProductAttributeId>();
            Mapper.CreateMap<ErpRetailChannelType, RetailChannelType>();
            Mapper.CreateMap<ErpRetailOperation, RetailOperation>();
            //Mapper.CreateMap<ErpRetailProductChannelProductAttributeId, RetailProductChannelProductAttributeId>();
            Mapper.CreateMap<ErpRoundingMethod, RoundingMethod>();
            Mapper.CreateMap<ErpSalesInvoiceType, SalesInvoiceType>();
            Mapper.CreateMap<ErpSalesStatus, SalesStatus>();
            Mapper.CreateMap<ErpSalesTransactionType, SalesTransactionType>();
            Mapper.CreateMap<ErpSearchLocation, SearchLocation>();
            Mapper.CreateMap<ErpShiftStatus, ShiftStatus>();
            //Mapper.CreateMap<ErpShipmentPublishingActionStatus, ShipmentPublishingActionStatus>();
            //Mapper.CreateMap<ErpShippingOptionType, ShippingOptionType>();NewCRT
            //Mapper.CreateMap<ErpSortOrderType, SortOrderType>();
            Mapper.CreateMap<ErpStatementMethod, StatementMethod>();
            //Mapper.CreateMap<ErpTaxableBasisIndia, TaxableBasisIndia>();
            Mapper.CreateMap<ErpTaxOverrideBy, TaxOverrideBy>();
            Mapper.CreateMap<ErpTaxOverrideType, TaxOverrideType>();
            //Mapper.CreateMap<ErpTaxTypeIndia, TaxTypeIndia>();
            //Mapper.CreateMap<ErpTenderDropAndDeclareType, TenderDropAndDeclareType>();//KAR
            Mapper.CreateMap<ErpTenderLineStatus, TenderLineStatus>();
            //Mapper.CreateMap<ErpThresholdDiscountMethod, ThresholdDiscountMethod>();
            //Mapper.CreateMap<ErpTransactionOperationType, TransactionOperationType>();
            //Mapper.CreateMap<ErpTransactionServiceProtocol, TransactionServiceProtocol>();
            Mapper.CreateMap<ErpTransactionStatus, TransactionStatus>();
            Mapper.CreateMap<ErpTransactionType, TransactionType>();
            Mapper.CreateMap<ErpTriggerFunctionType, TriggerFunctionType>();
            //Mapper.CreateMap<ErpAccentColor, AccentColor>();
            Mapper.CreateMap<ErpAddress, Address>()
                .ForMember(x => x.ExtensionProperties, opt => opt.Ignore()); // AF
            //Mapper.CreateMap<ErpAddress, Address>()
            //    .ForMember(x => x.Name, opt => opt.UseValue("Address"))
            //    .ForMember(x => x.ExtensionProperties, opt => opt.Ignore());
            //Mapper.CreateMap<ErpAddressBookPartyData, AddressBookPartyData>();
            Mapper.CreateMap<ErpAddressFormattingInfo, AddressFormattingInfo>();
            Mapper.CreateMap<ErpAffiliation, Affiliation>();
            Mapper.CreateMap<PagedResult<ErpAffiliation>, PagedResult<Affiliation>>();
            Mapper.CreateMap<ErpAffiliationLoyaltyTier, AffiliationLoyaltyTier>();
            Mapper.CreateMap<ErpARGBColor, ARGBColor>();
            //Mapper.CreateMap<ErpAttributeBase, AttributeBase>();
            //Mapper.CreateMap<ErpAttributeCategory, AttributeCategory>();
            //Mapper.CreateMap<ErpAttributeGroup, AttributeGroup>();
            //Mapper.CreateMap<ErpAttributeNameTranslation, AttributeNameTranslation>();
            Mapper.CreateMap<ErpAttributeValueBase, AttributeValueBase>();
               //  .ConstructUsing((Func<ErpAttributeValueBase, AttributeValueBase>)(x => new AttributeValueBase()));
            Mapper.CreateMap<ErpBarcode, Barcode>();
            //Mapper.CreateMap<ErpBarcodeMask, BarcodeMask>();
            //Mapper.CreateMap<ErpBarcodeMaskSegment, BarcodeMaskSegment>();
            Mapper.CreateMap<ErpButtonGrid, ButtonGrid>();
            Mapper.CreateMap<ErpButtonGridButton, ButtonGridButton>();
            Mapper.CreateMap<ErpButtonGridZone, ButtonGridZone>();
            Mapper.CreateMap<ErpCardTypeInfo, CardTypeInfo>();
            Mapper.CreateMap<ErpCart, Cart>();
            Mapper.CreateMap<ErpCartLine, CartLine>();
            //Mapper.CreateMap<ErpCartLineData, CartLineData>();
            Mapper.CreateMap<ErpCartTenderLine, CartTenderLine>();
            Mapper.CreateMap<ErpCashDeclaration, CashDeclaration>();
            Mapper.CreateMap<ErpCategory, Category>();
            //.ForMember(desc => desc.Image, opt => opt.Ignore())
            //.ForMember(desc => desc.ExtensionProperties, opt => opt.Ignore())
            //.ForMember(desc => desc.ExtensionData, opt => opt.Ignore())
            //.ForMember(desc => desc.NameTranslations, opt => opt.Ignore()); //TODO: Remove Ignore Part;
            //Mapper.CreateMap<ErpCategoryNameTranslation, CategoryNameTranslation>();
            //Mapper.CreateMap<ErpChannel, Channel>();
            Mapper.CreateMap<ErpChannelConfiguration, ChannelConfiguration>();
            Mapper.CreateMap<ErpChannelLanguage, ChannelLanguage>();
            //Mapper.CreateMap<ErpChannelPriceConfiguration, ChannelPriceConfiguration>();
            //Mapper.CreateMap<ErpChannelProfile, ChannelProfile>();
            Mapper.CreateMap<ErpChannelProfileProperty, ChannelProfileProperty>();
            Mapper.CreateMap<ErpChannelProperty, ChannelProperty>();
            //Mapper.CreateMap<ErpChargeConfiguration, ChargeConfiguration>();
            Mapper.CreateMap<ErpChargeLine, ChargeLine>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpCityInfo, CityInfo>();
            Mapper.CreateMap<ErpCommerceEntity, CommerceEntity>();
            //Mapper.CreateMap<ErpCommerceEntityChangeTrackingInformation, CommerceEntityChangeTrackingInformation>();
            Mapper.CreateMap<ErpCommerceList, CommerceList>();
            Mapper.CreateMap<ErpCommerceListLine, CommerceListLine>();
            Mapper.CreateMap<ErpCommerceProperty, CommerceProperty>();
            Mapper.CreateMap<ErpCommercePropertyValue, CommercePropertyValue>();
            Mapper.CreateMap<ErpComponentKitVariantSet, ComponentKitVariantSet>();
            //Mapper.CreateMap<ErpContactInfo, ContactInfo>()
            //    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpContactInformation, ContactInformation>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpCountryRegionInfo, CountryRegionInfo>();
            Mapper.CreateMap<ErpCountyInfo, CountyInfo>();
            Mapper.CreateMap<ErpCreditMemo, CreditMemo>();
            Mapper.CreateMap<ErpCurrency, Currency>();
            Mapper.CreateMap<ErpCurrencyAmount, CurrencyAmount>();
            Mapper.CreateMap<ErpCurrencyRequest, CurrencyRequest>();
            Mapper.CreateMap<ErpCustomer, Customer>();

            Mapper.CreateMap<ErpCustomer, Customer>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpCustomerAffiliation, CustomerAffiliation>();
            Mapper.CreateMap<ErpCustomerGroup, CustomerGroup>();
            //Mapper.CreateMap<ErpCustomerLoyaltyCard, CustomerLoyaltyCard>();
            Mapper.CreateMap<ErpDeliveryOption, DeliveryOption>();
            Mapper.CreateMap<ErpDevice, Device>();
            Mapper.CreateMap<ErpDeviceConfiguration, DeviceConfiguration>();
            Mapper.CreateMap<ErpDiscountCode, DiscountCode>();
            Mapper.CreateMap<ErpDiscountLine, DiscountLine>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpDistrictInfo, DistrictInfo>();
            Mapper.CreateMap<ErpEmployee, Employee>();
            Mapper.CreateMap<ErpEmployeeActivity, EmployeeActivity>();
            Mapper.CreateMap<ErpEmployeePermissions, EmployeePermissions>();
            //Mapper.CreateMap<ErpExchangeRate, ExchangeRate>();
            //Mapper.CreateMap<ErpFormulaIndia, FormulaIndia>();
            Mapper.CreateMap<ErpGiftCard, GiftCard>();
            Mapper.CreateMap<ErpGlobalCustomer, GlobalCustomer>();
            Mapper.CreateMap<ErpHardwareProfile, HardwareProfile>();
            Mapper.CreateMap<ErpHardwareProfileCashDrawer, HardwareProfileCashDrawer>();
            Mapper.CreateMap<ErpHardwareProfilePrinter, HardwareProfilePrinter>();
            Mapper.CreateMap<ErpHardwareProfileScanner, HardwareProfileScanner>();
            Mapper.CreateMap<ErpImageZone, ImageZone>();
            Mapper.CreateMap<ErpIncomeExpenseAccount, IncomeExpenseAccount>();
            Mapper.CreateMap<ErpIncomeExpenseLine, IncomeExpenseLine>();
            //Mapper.CreateMap<ErpItem, Item>();
            Mapper.CreateMap<ErpItemAvailability, ItemAvailability>();
            //Mapper.CreateMap<ErpItemAvailableQuantity, ItemAvailableQuantity>();
            Mapper.CreateMap<ErpItemBarcode, ItemBarcode>();
            //Mapper.CreateMap<ErpItemDimensions, ItemDimensions>();
            //Mapper.CreateMap<ErpItemMaxRetailPriceIndia, ItemMaxRetailPriceIndia>();
            //Mapper.CreateMap<ErpItemReservation, ItemReservation>();
            Mapper.CreateMap<ErpKitComponent, KitComponent>();
            Mapper.CreateMap<ErpKitComponentKey, KitComponentKey>();
            //Mapper.CreateMap<ErpKitConfigToComponentAssociation, KitConfigToComponentAssociation>();
            Mapper.CreateMap<ErpKitDefinition, KitDefinition>();
            Mapper.CreateMap<ErpKitLineDefinition, KitLineDefinition>();
            Mapper.CreateMap<ErpKitLineProductProperty, KitLineProductProperty>();
            //Mapper.CreateMap<ErpKitLineProductPropertyDictionary, KitLineProductPropertyDictionary>();
            Mapper.CreateMap<ErpKitTransactionLine, KitTransactionLine>();
            Mapper.CreateMap<ErpKitVariantContent, KitVariantContent>();
            //Mapper.CreateMap<ErpKitVariantToComponentDictionary, KitVariantToComponentDictionary>();
            Mapper.CreateMap<ErpLinkedProduct, LinkedProduct>();
            //Mapper.CreateMap<ErpListingPublishStatus, ListingPublishStatus>();
            Mapper.CreateMap<ErpLocalizedString, LocalizedString>();
            Mapper.CreateMap<ErpLoyaltyCard, LoyaltyCard>();
            Mapper.CreateMap<ErpLoyaltyCardTier, LoyaltyCardTier>();
            Mapper.CreateMap<ErpLoyaltyGroup, LoyaltyGroup>();
            Mapper.CreateMap<ErpLoyaltyRewardPoint, LoyaltyRewardPoint>();
            Mapper.CreateMap<ErpLoyaltyRewardPointLine, LoyaltyRewardPointLine>();
            //Mapper.CreateMap<ErpLoyaltySchemeLineEarn, LoyaltySchemeLineEarn>();
            //Mapper.CreateMap<ErpLoyaltySchemeLineRedeem, LoyaltySchemeLineRedeem>();
            Mapper.CreateMap<ErpLoyaltyTier, LoyaltyTier>();
            //Mapper.CreateMap<ErpMixAndMatchLineGroup, MixAndMatchLineGroup>();
            //Mapper.CreateMap<ErpNotification, Notification>();//KAR
            Mapper.CreateMap<ErpOperationPermission, OperationPermission>();
            Mapper.CreateMap<ErpOrgUnitAvailability, OrgUnitAvailability>();
            //Mapper.CreateMap<ErpOrgUnitContact, OrgUnitContact>();
            Mapper.CreateMap<ErpOrgUnitLocation, OrgUnitLocation>();
            Mapper.CreateMap<ErpParameterSet, ParameterSet>();
            Mapper.CreateMap<ErpPaymentCard, PaymentCard>();
            //Mapper.CreateMap<ErpPaymentConnectorConfiguration, PaymentConnectorConfiguration>();
            //Mapper.CreateMap<ErpPeriodicDiscount, PeriodicDiscount>();
            Mapper.CreateMap<ErpPickingList, PickingList>();
            Mapper.CreateMap<ErpPickingListLine, PickingListLine>();
            //Mapper.CreateMap<ErpPriceAdjustment, PriceAdjustment>();
            //Mapper.CreateMap<ErpPriceGroup, PriceGroup>();
            //Mapper.CreateMap<ErpPriceLine, PriceLine>();
            //Mapper.CreateMap<ErpPriceParameters, PriceParameters>();
            Mapper.CreateMap<ErpPrinter, Printer>();
            Mapper.CreateMap<ErpProduct, Product>();
            //.ForMember(desc => desc.VariantId, opt => opt.MapFrom(src => string.Empty));
            //.ForMember(desc => desc.Image, opt => opt.Ignore()); //TODO: Remove Ignore Part
            //Mapper.CreateMap<ErpProductAttributeSchemaEntry, ProductAttributeSchemaEntry>();
            Mapper.CreateMap<ErpProductAvailableQuantity, ProductAvailableQuantity>();
            Mapper.CreateMap<ErpProductCatalog, ProductCatalog>();
            //Mapper.CreateMap<ErpProductCatalogAssociation, ProductCatalogAssociation>();
            //Mapper.CreateMap<ErpProductCategoryAssociation, ProductCategoryAssociation>();
            //Mapper.CreateMap<ErpProductChangeTrackingAnchorSet, ProductChangeTrackingAnchorSet>();
            Mapper.CreateMap<ErpProductChangeTrackingInformation, ProductChangeTrackingInformation>();
            Mapper.CreateMap<ErpProductCompositionInformation, ProductCompositionInformation>();
            //Mapper.CreateMap<ErpProductDimensionDictionary, ProductDimensionDictionary>();
            Mapper.CreateMap<ErpProductDimensionSet, ProductDimensionSet>();

            //Mapper.CreateMap<ErpProductDimensionValueDictionary, ProductDimensionValueDictionaryEntry>();
            //for CU 9 
            //Mapper.CreateMap<ErpProductDimensionValueDictionaryEntry, ProductDimensionValueDictionaryEntry>();

            Mapper.CreateMap<ErpProductDimensionValueSet, ProductDimensionValueSet>();
            //Mapper.CreateMap<ErpProductExistenceId, ProductExistenceId>();
            //Mapper.CreateMap<ErpProductIdentity, ProductIdentity>();
            Mapper.CreateMap<ErpProductPrice, ProductPrice>();
            Mapper.CreateMap<ErpProductProperty, ProductProperty>();
            //Mapper.CreateMap<ErpProductPropertyDictionary, ProductPropertyDictionary>();
            //Mapper.CreateMap<ErpProductPropertySchema, ProductPropertySchema>();
            Mapper.CreateMap<ErpProductPropertyTranslation, ProductPropertyTranslation>();
            //Mapper.CreateMap<ErpProductPropertyTranslationDictionary, ProductPropertyTranslationDictionary>();
            Mapper.CreateMap<ErpProductRules, ProductRules>();
            //Mapper.CreateMap<ErpProductToKitVariantDictionary, ProductToKitVariantDictionary>();
            //Mapper.CreateMap<ErpProductVariant, ProductVariant>();
            Mapper.CreateMap<ErpProductVariant, ProductVariant>()
                .ConstructUsing((Func<ErpProductVariant, ProductVariant>)(x => new ProductVariant()))
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore())
                //.ForMember(dest => dest.IndexedProperties, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.PropertiesAsList, opt => opt.Ignore());
            //Mapper.CreateMap<ErpProductVariantDictionary, ProductVariantDictionary>();
            Mapper.CreateMap<ErpProductVariantInformation, ProductVariantInformation>();
            Mapper.CreateMap<ErpProjectionDomain, ProjectionDomain>();
            Mapper.CreateMap<ErpPurchaseOrder, PurchaseOrder>();
            Mapper.CreateMap<ErpPurchaseOrderLine, PurchaseOrderLine>();
            //Mapper.CreateMap<ErpQuantityDiscountLevel, QuantityDiscountLevel>();
            Mapper.CreateMap<ErpReasonCode, ReasonCode>();
            Mapper.CreateMap<ErpReasonCodeLine, ReasonCodeLine>()
                    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore()); //AF

            Mapper.CreateMap<ErpReasonCodeRequirement, ReasonCodeRequirement>();
            //Mapper.CreateMap<ErpReasonCodeSettings, ReasonCodeSettings>();
            //Mapper.CreateMap<ErpReasonCodeSpecific, ReasonCodeSpecific>();
            Mapper.CreateMap<ErpReasonSubCode, ReasonSubCode>();
            Mapper.CreateMap<ErpReceipt, Receipt>();
            //Mapper.CreateMap<ErpReceiptHeaderInfoIndia, ReceiptHeaderInfoIndia>();
            //Mapper.CreateMap<ErpReceiptHeaderTaxInfoIndia, ReceiptHeaderTaxInfoIndia>();
            //Mapper.CreateMap<ErpReceiptInfo, ReceiptInfo>();
            //Mapper.CreateMap<ErpReceiptMask, ReceiptMask>();
            //Mapper.CreateMap<ErpReceiptProfile, ReceiptProfile>();
            Mapper.CreateMap<ErpRelatedProduct, RelatedProduct>();
            //Mapper.CreateMap<ErpReportConfiguration, ReportConfiguration>();
            Mapper.CreateMap<ErpReportZone, ReportZone>();
            //Mapper.CreateMap<ErpRetailCategoryMember, RetailCategoryMember>();
            //Mapper.CreateMap<ErpRetailDiscount, RetailDiscount>();
            //Mapper.CreateMap<ErpRetailDiscountLine, RetailDiscountLine>();
            //Mapper.CreateMap<ErpRetailDiscountPriceGroup, RetailDiscountPriceGroup>();
            //Mapper.CreateMap<ErpRetailImage, RetailImage>();
            Mapper.CreateMap<ErpRichMediaLocations, RichMediaLocations>();
            Mapper.CreateMap<ErpRichMediaLocationsRichMediaLocation, RichMediaLocationsRichMediaLocation>();
            Mapper.CreateMap<ErpSalesAffiliationLoyaltyTier, SalesAffiliationLoyaltyTier>();

            Mapper.CreateMap<ErpSalesInvoice, SalesInvoice>();
            Mapper.CreateMap<ErpSalesInvoiceLine, SalesInvoiceLine>();
            //NS:VW Payment
            Mapper.CreateMap<ErpSalesOrder, SalesOrder>()
                .ConstructUsing((Func<ErpSalesOrder, SalesOrder>)(x => new SalesOrder()))
                .ForMember(dest => dest.OverriddenDepositAmount, opt => opt.Ignore());
            //Mapper.CreateMap<ErpSalesOrder, SalesOrder>()
            //    .ConstructUsing((Func<ErpSalesOrder, SalesOrder>)(x => new SalesOrder()))
            //    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore())
            //    .ForMember(dest => dest.OverriddenDepositAmount, opt => opt.Ignore());
            Mapper.CreateMap<SalesOrder, ErpSalesOrder>()
                .ConstructUsing((Func<SalesOrder, ErpSalesOrder>)(x => new ErpSalesOrder()))
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore())         
                .ForMember(dest => dest.OverriddenDepositAmount, opt => opt.Ignore()); 
            Mapper.CreateMap<ErpSalesLine, SalesLine>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            //Mapper.CreateMap, SalesLine>()
            //Mapper.CreateMap<ErpSalesParameters, SalesParameters>();
            Mapper.CreateMap<ErpSalesTaxGroup, SalesTaxGroup>();
            //Mapper.CreateMap<ErpSalesTransaction, SalesTransaction>();
            //Mapper.CreateMap<ErpSalesTransactionData, SalesTransactionData>();
            Mapper.CreateMap<ErpShift, Shift>();
            Mapper.CreateMap<ErpShiftAccountLine, ShiftAccountLine>();
            Mapper.CreateMap<ErpShiftTenderLine, ShiftTenderLine>();
            //Mapper.CreateMap<ErpShipment, Shipment>();
            //Mapper.CreateMap<ErpShipmentLine, ShipmentLine>();
            //Mapper.CreateMap<ErpShipmentLineMapping, ShipmentLineMapping>();
            //Mapper.CreateMap<ErpShipmentProgress, ShipmentProgress>();
            //Mapper.CreateMap<ErpShipmentPublishingStatus, ShipmentPublishingStatus>();
            //Mapper.CreateMap<ErpShippingAdapterConfig, ShippingAdapterConfig>();
            Mapper.CreateMap<ErpStateProvinceInfo, StateProvinceInfo>();
            Mapper.CreateMap<ErpStockCountJournal, StockCountJournal>();
            Mapper.CreateMap<ErpStockCountJournalTransaction, StockCountJournalTransaction>();
            //Mapper.CreateMap<ErpStorageLookup, StorageLookup>();
            Mapper.CreateMap<ErpSupportedLanguage, SupportedLanguage>();
            //Mapper.CreateMap<ErpTaxableItem, TaxableItem>();
            //Mapper.CreateMap<ErpTaxCodeInterval, TaxCodeInterval>();
            //Mapper.CreateMap<ErpTaxCodeUnit, TaxCodeUnit>();
            //Mapper.CreateMap<ErpTaxComponentIndia, TaxComponentIndia>();
            Mapper.CreateMap<ErpTaxLine, TaxLine>()
                .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            Mapper.CreateMap<ErpTaxOverride, TaxOverride>();
            //Mapper.CreateMap<ErpTaxParameters, TaxParameters>();
            //Mapper.CreateMap<ErpTaxSummarySettingIndia, TaxSummarySettingIndia>();
            Mapper.CreateMap<ErpTenderDetail, TenderDetail>();
            //NS:VW Payment
            Mapper.CreateMap<ErpTenderLine, TenderLine>();
            //Mapper.CreateMap<ErpTenderLine, TenderLine>()
            //    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            //Mapper.CreateMap<ErpTenderLineBase, TenderLineBase>();
            Mapper.CreateMap<ErpTenderType, TenderType>();
            //Mapper.CreateMap<ErpTerminal, Terminal>();
            Mapper.CreateMap<ErpTextValueTranslation, TextValueTranslation>();
            //Mapper.CreateMap<ErpThresholdDiscountTier, ThresholdDiscountTier>();
            Mapper.CreateMap<ErpTillLayout, TillLayout>();
            //Mapper.CreateMap<ErpTrackingInfo, TrackingInfo>();
            //Mapper.CreateMap<ErpTradeAgreement, TradeAgreement>();
            Mapper.CreateMap<ErpTransaction, Transaction>();
            //Mapper.CreateMap<ErpTransactionProperty, TransactionProperty>();
            //Mapper.CreateMap<ErpTransactionServiceProfile, TransactionServiceProfile>();
            Mapper.CreateMap<ErpTransferOrder, TransferOrder>();
            Mapper.CreateMap<ErpTransferOrderLine, TransferOrderLine>();
            Mapper.CreateMap<ErpUnitOfMeasure, UnitOfMeasure>();
            Mapper.CreateMap<ErpUnitOfMeasureConversion, UnitOfMeasureConversion>();
            //Mapper.CreateMap<ErpValidationPeriod, ValidationPeriod>();
            //Mapper.CreateMap<ErpWeight, Weight>();
            Mapper.CreateMap<ErpZipCodeInfo, ZipCodeInfo>();
            //Mapper.CreateMap<ErpZoneReference, ZoneReference>();

            #endregion
        }
    }

    public class NullableDateTimeConverter : ITypeConverter<DateTimeOffset, DateTime>
    {
        // Since both are nullable date times and this handles converting
        // nullable to datetime I would not expect this to be called. 
        public DateTime Convert(ResolutionContext context)
        {
            var sourceDate = context.SourceValue as DateTime?;
            if (sourceDate.HasValue)
                return sourceDate.Value;
            else
                return default(DateTime);
        }
    }
}
