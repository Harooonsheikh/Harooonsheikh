update MappingTemplate
set IngramTemplate = N'<Targets>
   <Target property="ErpSalesOrder~Id" source-path="//orders/id" />
   <Target property="ErpSalesOrder~OrderPlacedDate" source-path="//orders/created" />
   <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/id" />
   <Target property="ErpSalesOrder~CurrencyCode" constant-value="" />
   <Target property="ErpSalesOrder~Status" constant-value="Created" />
   <Target property="ErpSalesOrder~TotalAmount" constant-value="0" />
   <Target property="ErpSalesOrder~TaxAmount" constant-value="0" />
   <Target property="ErpSalesOrder~NetAmountWithNoTax" constant-value="0" />
   <Target property="ErpSalesOrder~NetAmountWithTax" constant-value="0" />
   <Target property="ErpSalesOrder~IngramAssetId" source-path="//orders/asset/id" />
   <Target property="ErpSalesOrder~IngramAssetType" source-path="//orders/asset/connection/type" />
   <Target property="ErpSalesOrder~IngramContractId" source-path="//orders/contract/id" />
   <Target property="ErpSalesOrder~IngramMarketPlaceId" source-path="//orders/marketplace/id" />
   <Target property="ErpSalesOrder~OrderDiscounts" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true">
      <Properties>
         <Target property="ErpDiscountLine~EffectiveAmount" source-path="net-price" />
         <Target property="ErpDiscountLine~Tax" source-path="tax" />
         <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id" />
         <Target property="ErpDiscountLine~OfferId" source-path="campaign-id" />
         <Target property="ErpDiscountLine~CouponId" source-path="coupon-id" />
         <Target property="ErpDiscountLine~RebateCode" source-path="custom-attributes/custom-attribute[@attribute-id=''rebate-code'']" />
         <Target property="ErpDiscountLine~Amount" source-path="custom-attributes/custom-attribute[@attribute-id=''discount'']" />
         <Target property="ErpDiscountLine~SppNumber" source-path="custom-attributes/custom-attribute[@attribute-id=''spp-no'']" />
         <Target property="ErpDiscountLine~OfferName" source-path="custom-attributes/custom-attribute[@attribute-id=''reason-code'']" />
      </Properties>
   </Target>
   <Target property="ErpSalesOrder~SalesLines" source-path="//orders/asset/items" target-source="ErpSalesLine" repeat="true">
      <Properties>
         <Target property="ErpSalesLine~UnitOfMeasureSymbol" constant-value="" />
         <Target property="ErpSalesLine~SalesOrderUnitOfMeasure" constant-value="" />
         <Target property="ErpSalesLine~NetAmount" constant-value="0" />
         <Target property="ErpSalesLine~TaxAmount" constant-value="0" />
         <Target property="ErpSalesLine~TotalAmount" constant-value="0" />
         <Target property="ErpSalesLine~BasePrice" constant-value="0" />
         <Target property="ErpSalesLine~Price" constant-value="0" />
         <Target property="ErpSalesLine~LineNumber" source-path="line_number" />
         <Target property="ErpSalesLine~Description" source-path="mpn" />
         <Target property="ErpSalesLine~ItemId" source-path="mpn" />
		 <Target property="ErpSalesLine~OldQuantity" source-path="old_quantity" />
         <Target property="ErpSalesLine~Quantity" source-path="quantity" />
         <Target property="ErpSalesLine~QuantityOrdered" source-path="quantity" />
         <Target property="ErpSalesLine~TaxRatePercent" constant-value="0" />
         <Target property="ErpSalesLine~IsGiftCardLine" constant-value="false" />
         <Target property="ErpSalesLine~ShipmentId" constant-value="0" />
         <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''bogoPromotionId'']" is-custom-attribute="true" attribute-id="bogoPromotionId" />
         <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="//orders/created" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" default-value="" is-custom-attribute="true" attribute-id="PACLICENSE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMIGRATEDSALESLINENUMBER" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''AffiliationRecId'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAFFILIATIONRECID" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVOLDSALESLINENUMBER" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINEACTION'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESLINEACTION" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVSOURCEID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSOURCEID" />
		 <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPARENT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPARENT" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCOUPONCODE'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOUPONCODE" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERLINENUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCUSTOMERLINENUM" />
         <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVDisablePACLicense'']" default-value="0" is-custom-attribute="true" attribute-id="TMVDisablePACLicense" />
         <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true">
            <Properties>
               <Target property="ErpDiscountLine~Amount" source-path="net-price" />
               <Target property="ErpDiscountLine~Tax" source-path="tax" />
               <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id" />
               <Target property="ErpDiscountLine~OfferId" source-path="campaign-id" />
               <Target property="ErpDiscountLine~CouponId" source-path="coupon-id" />
               <Target property="ErpDiscountLine~OfferName" source-path="OfferName" />
               <Target property="ErpDiscountLine~EffectiveAmount" source-path="EffectiveAmount" />
               <Target property="ErpDiscountLine~Percentage" source-path="Percentage" />
               <Target property="ErpDiscountLine~DiscountLineTypeValue" source-path="DiscountLineTypeValue" />
               <Target property="ErpDiscountLine~ManualDiscountTypeValue" source-path="ManualDiscountTypeValue" />
               <Target property="ErpDiscountLine~CustomerDiscountTypeValue" source-path="CustomerDiscountTypeValue" />
               <Target property="ErpDiscountLine~PeriodicDiscountTypeValue" source-path="PeriodicDiscountTypeValue" />
            </Properties>
         </Target>
      </Properties>
   </Target>
   <Target property="ErpSalesOrder~Customer" source-path="//orders/asset/tiers/customer" target-source="ErpCustomer" repeat="false">
      <Properties>
         <Target property="ErpCustomer~EcomCustomerId" source-path="id" />
         <Target property="ErpCustomer~Name" source-path="name" />
         <Target property="ErpCustomer~Email" source-path="contact_info/contact/email" />
         <Target property="ErpCustomer~FirstName" source-path="contact_info/contact/first_name" />
         <Target property="ErpCustomer~LastName" source-path="contact_info/contact/last_name" />
         <Target property="ErpCustomer~Street" source-path="contact_info/address_line1" />
         <Target property="ErpCustomer~City" source-path="contact_info/city" />
         <Target property="ErpCustomer~ZipCode" source-path="postal_code" />
         <Target property="ErpCustomer~State" source-path="state" />
         <Target property="ErpCustomer~TwoLetterISORegionName" source-path="country" />
         <Target property="ErpCustomer~ThreeLetterISORegionName" source-path="country" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/country_code" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/area_code" concatenate="true" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/phone_number" concatenate="true" />
         <Target property="ErpCustomer~Address" source-path="//orders/asset/tiers/customer/contact_info" target-source="ErpAddress" repeat="false">
            <Properties>
               <Target property="ErpAddress~Name" source-path="contact/first_name" />
               <Target property="ErpAddress~Name" source-path="contact/last_name" concatenate="true" />
               <Target property="ErpAddress~Street" source-path="address_line1" />
			   <Target property="ErpAddress~BuildingCompliment" source-path="address_line2" />
               <Target property="ErpAddress~City" source-path="city" />
               <Target property="ErpAddress~ZipCode" source-path="postal_code" />
               <Target property="ErpAddress~State" source-path="state" />
               <Target property="ErpAddress~StateName" source-path="state" />
               <Target property="ErpAddress~TwoLetterISORegionName" source-path="country" />
               <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/country_code" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/area_code" concatenate="true" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/phone_number" concatenate="true" />
            </Properties>
         </Target>
      </Properties>
   </Target>
   <Target property="ErpSalesOrder~Reseller" source-path="//orders/asset/tiers/tier1" target-source="ErpCustomer" repeat="false">
      <Properties>
         <Target property="ErpCustomer~EcomCustomerId" source-path="id" />
         <Target property="ErpCustomer~Name" source-path="name" />
         <Target property="ErpCustomer~Email" source-path="contact_info/contact/email" />
         <Target property="ErpCustomer~FirstName" source-path="contact_info/contact/first_name" />
         <Target property="ErpCustomer~LastName" source-path="contact_info/contact/last_name" />
         <Target property="ErpCustomer~Street" source-path="contact_info/address_line1" />
         <Target property="ErpCustomer~City" source-path="contact_info/city" />
         <Target property="ErpCustomer~ZipCode" source-path="postal_code" />
         <Target property="ErpCustomer~State" source-path="state" />
         <Target property="ErpCustomer~TwoLetterISORegionName" source-path="country" />
         <Target property="ErpCustomer~ThreeLetterISORegionName" source-path="country" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/country_code" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/area_code" concatenate="true" />
         <Target property="ErpCustomer~Phone" source-path="contact_info/contact/phone_number/phone_number" concatenate="true" />
         <Target property="ErpCustomer~Address" source-path="//orders/asset/tiers/tier1/contact_info" target-source="ErpAddress" repeat="false">
            <Properties>
               <Target property="ErpAddress~Name" source-path="contact/first_name" />
               <Target property="ErpAddress~Name" source-path="contact/last_name" concatenate="true" />
               <Target property="ErpAddress~Street" source-path="address_line1" />
			   <Target property="ErpAddress~BuildingCompliment" source-path="address_line2" />
               <Target property="ErpAddress~City" source-path="city" />
               <Target property="ErpAddress~ZipCode" source-path="postal_code" />
               <Target property="ErpAddress~State" source-path="state" />
               <Target property="ErpAddress~StateName" source-path="state" />
               <Target property="ErpAddress~TwoLetterISORegionName" source-path="country" />
               <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/country_code" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/area_code" concatenate="true" />
               <Target property="ErpAddress~Phone" source-path="contact/phone_number/phone_number" concatenate="true" />
            </Properties>
         </Target>
      </Properties>
   </Target>

   <Target property="ErpSalesOrder~Parameters" source-path="//orders/asset/params" target-source="ErpIngramOrderParameter" repeat="true">
      <Properties>
         <Target property="ErpIngramOrderParameter~Description" source-path="description" />
         <Target property="ErpIngramOrderParameter~Id" source-path="id" />
         <Target property="ErpIngramOrderParameter~Name" source-path="name" />
         <Target property="ErpIngramOrderParameter~Type" source-path="type" />
         <Target property="ErpIngramOrderParameter~Value" source-path="value" />
         <Target property="ErpIngramOrderParameter~ValueError" source-path="value_error" />
      </Properties>
   </Target>

   <Target property="ErpSalesOrder~TenderLines" source-path="//orders" target-source="ErpTenderLine" repeat="true">
      <Properties>
         <Target property="ErpTenderLine~TenderTypeId" constant-value="PURCHASEORDER" />
		 <Target property="ErpTenderLine~Amount" constant-value="0" />
      </Properties>
   </Target>
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod" />
   <Target property="ErpSalesOrder~RequestedDeliveryDate" source-path="//orders/created" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLERACCOUNT'']" is-custom-attribute="true" attribute-id="TMVRESELLERACCOUNT" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVDISTRIBUTORACCOUNT'']" is-custom-attribute="true" attribute-id="TMVDISTRIBUTORACCOUNT" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMER'']" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMAINOFFERTYPE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMAINOFFERTYPE" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPRODUCTFAMILY'']" is-custom-attribute="true" attribute-id="TMVPRODUCTFAMILY" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORDERSUBTYPE'']" default-value="1" is-custom-attribute="true" attribute-id="TMVSALESORDERSUBTYPE" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINVOICESCHEDULECOMPLETE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVINVOICESCHEDULECOMPLETE" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSMMCAMPAIGNID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSMMCAMPAIGNID" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPIT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPIT" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVQUOTATIONID'']" default-value="" is-custom-attribute="true" attribute-id="TMVQUOTATIONID" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDORDERNUMBER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESORDERNUMBER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOriginatingCountry'']" default-value="" is-custom-attribute="true" attribute-id="TMVOriginatingCountry" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFORORDER'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFORORDER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFOREMAIL'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFOREMAIL" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORIGIN'']" default-value="Ingram" is-custom-attribute="true" attribute-id="TMVSALESORIGIN" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPARTNERID'']" default-value="" is-custom-attribute="true" attribute-id="TMVPARTNERID" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPaymentTerms'']" default-value="" is-custom-attribute="true" attribute-id="TMVPaymentTerms" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVFraudReviewStatus'']" default-value="" is-custom-attribute="true" attribute-id="TMVFraudReviewStatus" />
    <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVKountScore'']" default-value="" is-custom-attribute="true" attribute-id="TMVKountScore" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLER'']" default-value="INGRAM" is-custom-attribute="true" attribute-id="TMVRESELLER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMEREMAIL'']" default-value="" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMEREMAIL" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVTransferOrderAsPerOldDate'']" default-value="0" is-custom-attribute="true" attribute-id="TMVTransferOrderAsPerOldDate" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVActivationLinkEmail'']" default-value="" is-custom-attribute="true" attribute-id="TMVActivationLinkEmail" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCustomerReference'']" default-value="" is-custom-attribute="true" attribute-id="TMVCustomerReference" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCurrencyCode'']" default-value="" is-custom-attribute="true" attribute-id="TMVCurrencyCode" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVContactPersonId'']" default-value="" is-custom-attribute="true" attribute-id="TMVContactPersonId" />																																																										 
</Targets>'
WHERE	[Name] = 'READ.ErpSalesOrder'

