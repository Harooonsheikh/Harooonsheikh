DECLARE @AppleAppStoreID INT;

SELECT	@AppleAppStoreID = StoreId
FROM	Store
WHERE	[Name] = 'Apple App'

-- SELECT @AppleAppStoreID

/* Update READ.ErpSalesOrder mapping template for all stores except Apple App */
UPDATE	MappingTemplate
SET		[XML] = N'<Targets>
  <!--ErpSalesOrder Properties-->
  <Target property="ErpSalesOrder~Id" source-path="//orders/order[@order-no]" attribute-name="order-no" />
  <Target property="ErpSalesOrder~OrderPlacedDate" source-path="//orders/order/order-date" />
  <!--<Target property="ErpSalesOrder~InventoryLocationId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~InventoryLocationId" constant-value="095097"/>-->
  <!--It should be used for store pick up-->
  <!--<Target property="ErpSalesOrder~StoreId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~StoreId" constant-value="095097"/>-->
  <!-- TMV has No Source-Codes-->
  <!--<Target property="ErpSalesOrder~SourceCode" source-path="//orders/order/source-code/code"/>-->
  <Target property="ErpSalesOrder~StaffId" source-path="//orders/order/staff-id" />
  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/original-order-no" />
  <Target property="ErpSalesOrder~CurrencyCode" source-path="//orders/order/currency" />
  <Target property="ErpSalesOrder~CustomerId" source-path="//orders/order/customer/customer-no" />
  <Target property="ErpSalesOrder~CustomerName" source-path="//orders/order/customer/customer-name" />
  <Target property="ErpSalesOrder~CustomerEmail" source-path="//orders/order/customer/customer-email" />
  <Target property="ErpSalesOrder~ReceiptEmail" source-path="//orders/order/customer/customer-email" />
  <Target property="ErpSalesOrder~Status" constant-value="Created" />
  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/current-order-no" />
  <Target property="ErpSalesOrder~TotalAmount" source-path="//orders/order/totals/order-total/gross-price" />
  <Target property="ErpSalesOrder~TaxAmount" source-path="//orders/order/totals/order-total/tax" />
  <Target property="ErpSalesOrder~NetAmountWithNoTax" source-path="//orders/order/totals/order-total/net-price" />
  <Target property="ErpSalesOrder~NetAmountWithTax" source-path="//orders/order/totals/order-total/gross-price" />
  <!--Reading discount of Order-->
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
  <!--ErpSalesOrder Products-->
  <Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/product-lineitems/product-lineitem" target-source="ErpSalesLine" repeat="true">
    <Properties>
      <Target property="ErpSalesLine~UnitOfMeasureSymbol" source-path="line-unit-of-measure" />
      <Target property="ErpSalesLine~SalesOrderUnitOfMeasure" source-path="line-unit-of-measure" />
      <Target property="ErpSalesLine~NetAmount" source-path="net-price" />
      <Target property="ErpSalesLine~TaxAmount" source-path="tax" />
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price" />
      <Target property="ErpSalesLine~BasePrice" source-path="base-price" />
      <Target property="ErpSalesLine~Price" source-path="base-price" />
      <Target property="ErpSalesLine~LineNumber" source-path="position" />
      <Target property="ErpSalesLine~Description" source-path="product-id" />
      <Target property="ErpSalesLine~ItemId" source-path="product-id" />
      <Target property="ErpSalesLine~Quantity" source-path="quantity" />
      <Target property="ErpSalesLine~QuantityOrdered" source-path="quantity" />
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate" />
      <Target property="ErpSalesLine~IsGiftCardLine" source-path="gift" />
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id" />
      <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']" />
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU" />
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''bogoPromotionId'']" is-custom-attribute="true" attribute-id="bogoPromotionId" />
      <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']" />
      <!--TVW FDD-007-->
      <!--Set in the "Synchronize orders" batch flow based Shipping Date Requested field on the SalesLin-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM" />
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom in the happy flow.-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM" />
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom + the length of the offer type from the deimension value - could also be empty if perpetual i.e. same logic implemented in Basic Contract FDD should be automatically triggerred. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO" />
      <!--No logic implemented at time of writing - leave default empty value-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE" />
      <!--Ensure that is set to false (i.e. the default value). This is only used in more advanced scenarios and not the happy flow. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF" />
      <!--Set in the "Synchronize orders" batch flow to value of "created". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
      <!--This field is required in order to post the confirmation but no logic implemented at time of writing - fill in with dummy value. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION" />
      <!--Set in the "Synchronize orders" batch flow to value of "year". Note there is no logic tied to this field at the moment. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD" />
      <!--Random GUID passed from Magento/Postman but is inserted in a related table to the SalesLine hence the field is an Int64 RefRecId field. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" default-value="" is-custom-attribute="true" attribute-id="PACLICENSE" />
      <!--Set in the "Synchronize orders" batch flow. Copy value from SalesLine.LineAmount-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT" />
      <!--Set in the "Synchronize orders" batch flow to value of "yes"-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED" />
      <!--Set in the "Synchronize orders" batch flow to value of "none". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE" />
      <!--Set in the "Synchronize orders" batch flow to value of "". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMIGRATEDSALESLINENUMBER" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''AffiliationRecId'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAFFILIATIONRECID" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVOLDSALESLINENUMBER" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINEACTION'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESLINEACTION" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVSOURCEID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSOURCEID" />
      <!--Reading options of product-->
      <!-- TMV has No Option Items-->
      <!--
      <Target property="ErpSalesOrder~Options" source-path="option-lineitems/option-lineitem" target-source="ErpSalesLine" repeat="true" >
        <Properties>
          <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
          <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
          <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
          <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
          <Target property="ErpSalesLine~Price" source-path="base-price"/>
          <Target property="ErpSalesLine~ItemId" source-path="product-id"/>
          <Target property="ErpSalesLine~Quantity" constant-value="1" />
          <Target property="ErpSalesLine~Comment" constant-value="OptionItem"/>

          <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']"/>

          Reading discount of option
          <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
            <Properties>
              <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
              <Target property="ErpDiscountLine~Tax" source-path="tax"/>
              <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
              <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
              <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
			<Target property="ErpDiscountLine~OfferName" source-path="OfferName" />
				<Target property="ErpDiscountLine~EffectiveAmount" source-path="EffectiveAmount" />
				<Target property="ErpDiscountLine~Percentage" source-path="Percentage" />
				<Target property="ErpDiscountLine~DiscountLineTypeValue" source-path="DiscountLineTypeValue" />
				<Target property="ErpDiscountLine~ManualDiscountTypeValue" source-path="ManualDiscountTypeValue" />
				<Target property="ErpDiscountLine~CustomerDiscountTypeValue" source-path="CustomerDiscountTypeValue" />
				<Target property="ErpDiscountLine~PeriodicDiscountTypeValue" source-path="PeriodicDiscountTypeValue" />
				
            </Properties>
          </Target>
          <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']"/>
        </Properties>
      </Target>
      -->
      <!--Reading discount of product-->
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
  <!--Shipping-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~DeliveryMode" source-path="//orders/order/shipments/shipment/shipping-method"/>
  <Target property="ErpSalesOrder~DeliveryModeChargeAmount" source-path="//orders/order/shipping-lineitems/shipping-lineitem/gross-price"/>
  -->
  <!-- Shipping Tax is commented out because we are taking gross amount to shipping charges which includes tax-->
  <!--<Target property="ErpSalesOrder~Shipping_Tax" source-path="//orders/order/shipping-lineitems/shipping-lineitem/tax"/>-->
  <!--Reading discount of Shipping-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~ShippingDiscounts" source-path="//orders/order/shipping-lineitems/shipping-lineitem/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
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
  -->
  <!--New mapping with orders/order/shipping-lineitems/shipping-lineitem-->
  <!--<Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/shipping-lineitems/shipping-lineitem" target-source="ErpSalesLine" repeat="true" >
    <Properties>
      <Target property="ErpSalesLine~Price" source-path="base-price"/>
      <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
      <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate"/>
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
      <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
      <Target property="ErpSalesLine~ItemId" source-path="item-id"/>
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id"/>
      <Target property="ErpSalesLine~Quantity" constant-value="1"/>
      <Target property="ErpSalesLine~Comment" constant-value="DeliveryItem"/>
    </Properties>
  </Target>-->
  <Target property="ErpSalesOrder~BillingAddress" source-path="//orders/order/customer/billing-address" target-source="ErpAddress" repeat="false">
    <Properties>
      <Target property="ErpAddress~Name" source-path="first-name" />
      <Target property="ErpAddress~Name" source-path="last-name" concatenate="true" />
      <Target property="ErpAddress~Street" source-path="address1" />
      <Target property="ErpAddress~City" source-path="city" />
      <Target property="ErpAddress~ZipCode" source-path="postal-code" />
      <Target property="ErpAddress~State" source-path="state-code" />
      <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code" />
      <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code" />
      <Target property="ErpAddress~Phone" source-path="phone" />
    </Properties>
  </Target>
  <!--ErpSalesOrder Shipments-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~Shipments" source-path="//orders/order/shipments/shipment" target-source="ErpShipment" repeat="true" >
    <Properties>
      <Target property="ErpShipment~ShipmentId" source-path="" attribute-name="shipment-id"/>
      <Target property="ErpShipment~DeliveryMode" source-path="shipping-method"/>
      <Target property="ErpShipment~ShippingStatus" source-path="status/shipping-status"/>
      <Target property="ErpShipment~IsGift" source-path="gift"/>

      <Target property="ErpShipment~DeliveryAddress" source-path="shipping-address" target-source="ErpAddress" repeat="false">
        <Properties>
          <Target property="ErpAddress~Name" source-path="first-name"/>
          <Target property="ErpAddress~Name" source-path="last-name" concatenate="true"/>
          <Target property="ErpAddress~Street" source-path="address1"/>
          <Target property="ErpAddress~City" source-path="city"/>
          <Target property="ErpAddress~ZipCode" source-path="postal-code"/>
          <Target property="ErpAddress~State" source-path="state-code"/>
          <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~Phone" source-path="phone"/>
        </Properties>
      </Target>
    </Properties>
  </Target>
  -->
  <!--ErpSalesOrder Payments-->
  <Target property="ErpSalesOrder~TenderLines" source-path="//orders/order/payments/payment" target-source="ErpTenderLine" repeat="true">
    <Properties>
      <!--Common for all Payment methods-->
      <Target property="ErpTenderLine~TenderTypeId" source-path="processor-id" />
      <Target property="ErpTenderLine~Amount" source-path="amount" />
	  <Target property="ErpTenderLine~UniqueCardId" source-path="uniqueCardId" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="transaction-id" is-custom-attribute="true" attribute-id="transaction-id" />
      <!--AllPago-->
      <Target property="ErpTenderLine~NumberOfInstallments" source-path="numberOfInstallments" />
	  <!--PayPal-->
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAck'']" is-custom-attribute="true" attribute-id="paypalAck" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAmount'']" is-custom-attribute="true" attribute-id="paypalAmount" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalCorrelationId'']" is-custom-attribute="true" attribute-id="paypalCorrelationId" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPayerID'']" is-custom-attribute="true" attribute-id="paypalPayerID" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentStatus'']" is-custom-attribute="true" attribute-id="paypalPaymentStatus" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalToken'']" is-custom-attribute="true" attribute-id="paypalToken" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalTransactionID'']" is-custom-attribute="true" attribute-id="paypalTransactionID" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''transactionsHistory'']" is-custom-attribute="true" attribute-id="transactionsHistory" />
      <!--CreditCard-->
      <Target property="ErpTenderLine~CardTypeId" source-path="credit-card/card-type" />
      <Target property="ErpTenderLine~MaskedCardNumber" source-path="credit-card/card-number" />
      <Target property="ErpTenderLine~CardOrAccount" source-path="credit-card/card-holder" />
      <!--Payment Connector info UY-->
      <Target property="ErpTenderLine~Authorization" source-path="credit-card/authorization" />
      <Target property="ErpTenderLine~CardToken" source-path="credit-card/card-token" />
      <Target property="ErpTenderLine~ExpMonth" source-path="credit-card/expiration-month" />
      <Target property="ErpTenderLine~ExpYear" source-path="credit-card/expiration-year" />
      <Target property="ErpTenderLine~PayerId" source-path="credit-card/payerid" />
      <Target property="ErpTenderLine~ParentTransactionId" source-path="credit-card/parent-transaction-id" />
      <Target property="ErpTenderLine~Email" source-path="credit-card/email" />
      <Target property="ErpTenderLine~Note" source-path="credit-card/note" />
      <!--Ending Payment Connector info UY-->
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''tokenNumber'']" is-custom-attribute="true" attribute-id="tokenNumber" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID" />
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod" />
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
  <!--TVW FDD-007-->
  <Target property="ErpSalesOrder~RequestedDeliveryDate" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" />
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLERACCOUNT'']" is-custom-attribute="true" attribute-id="TMVRESELLERACCOUNT" />
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVDISTRIBUTORACCOUNT'']" is-custom-attribute="true" attribute-id="TMVDISTRIBUTORACCOUNT" />
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMER'']" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMER" />
  <!--Set in the "Synchronize orders" batch flow based on the TMVMainOfferType of the Offer Type Group dimension line. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMAINOFFERTYPE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMAINOFFERTYPE" />
  <!--Set in the "Synchronize orders" batch flow - more information to be received about this field. Contract confirmation fails if this field has no value. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPRODUCTFAMILY'']" is-custom-attribute="true" attribute-id="TMVPRODUCTFAMILY" />
  <!--This should be mapped to value ‚1‘ i.e. contract in the commerce link mapper. Orders created from the RetailSynchOrdersSchedulerTask class should use the value coming from the mapper and ignore the default value set on Accounts Receivable. This is for future-compatabilty reasons. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORDERSUBTYPE'']" default-value="1" is-custom-attribute="true" attribute-id="TMVSALESORDERSUBTYPE" />
  <!--Ensure that is set to false (i.e. the default value). This is only used in Advanced Contract Management -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINVOICESCHEDULECOMPLETE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVINVOICESCHEDULECOMPLETE" />
  <!--Set in the "Synchronize orders" batch flow based on the lowest value of SalesLine.TMVContractStatusLine within all contract lines. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE" />
  <!--This should be passed from Magento when the order is created as part of a marketing campaing (such as email sent to customer from Marketo). Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSMMCAMPAIGNID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSMMCAMPAIGNID" />
  <!--This should be passed from Magento . Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPIT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPIT" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVQUOTATIONID'']" default-value="" is-custom-attribute="true" attribute-id="TMVQUOTATIONID" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDORDERNUMBER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESORDERNUMBER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOriginatingCountry'']" default-value="" is-custom-attribute="true" attribute-id="TMVOriginatingCountry" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFORORDER'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFORORDER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFOREMAIL'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFOREMAIL" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORIGIN'']" default-value="" is-custom-attribute="true" attribute-id="TMVSALESORIGIN" />

</Targets>'
WHERE	[Name] = 'READ.ErpSalesOrder'
AND		StoreId <> @AppleAppStoreID;

/* Update READ.ErpSalesOrder mapping template for only Apple App store */
UPDATE	MappingTemplate
SET		[XML] = N'<Targets>
  <!--ErpSalesOrder Properties-->
  <Target property="ErpSalesOrder~Id" source-path="//orders/order[@order-no]" attribute-name="order-no"/>
  <Target property="ErpSalesOrder~OrderPlacedDate" source-path="//orders/order/order-date"/>
  
  <!--<Target property="ErpSalesOrder~InventoryLocationId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~InventoryLocationId" constant-value="095097"/>-->
  
  <!--It should be used for store pick up-->
  <!--<Target property="ErpSalesOrder~StoreId" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''InventLocationId'']"/>-->
  <!--<Target property="ErpSalesOrder~StoreId" constant-value="095097"/>-->
  
  <!-- TMV has No Source-Codes-->
  <!--<Target property="ErpSalesOrder~SourceCode" source-path="//orders/order/source-code/code"/>-->
  <Target property="ErpSalesOrder~StaffId" source-path="//orders/order/staff-id" />
  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/original-order-no"/>
  <Target property="ErpSalesOrder~CurrencyCode" source-path="//orders/order/currency"/>
  <Target property="ErpSalesOrder~CustomerId" source-path="//orders/order/customer/customer-no"/>
  <Target property="ErpSalesOrder~CustomerName" source-path="//orders/order/customer/customer-name"/>
  <Target property="ErpSalesOrder~CustomerEmail" source-path="//orders/order/customer/customer-email"/>
  <Target property="ErpSalesOrder~ReceiptEmail" source-path="//orders/order/customer/customer-email"/>
  <Target property="ErpSalesOrder~Status" constant-value="Created"/>
  <Target property="ErpSalesOrder~ChannelReferenceId" source-path="//orders/order/current-order-no"/>

  <Target property="ErpSalesOrder~TotalAmount" source-path="//orders/order/totals/order-total/gross-price"/>
  <Target property="ErpSalesOrder~TaxAmount" source-path="//orders/order/totals/order-total/tax"/>
  <Target property="ErpSalesOrder~NetAmountWithNoTax" source-path="//orders/order/totals/order-total/net-price"/>
  <Target property="ErpSalesOrder~NetAmountWithTax" source-path="//orders/order/totals/order-total/gross-price"/>
    
  <!--Reading discount of Order-->
  <Target property="ErpSalesOrder~OrderDiscounts" source-path="//orders/order/totals/merchandize-total/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
    <Properties>
      <Target property="ErpDiscountLine~EffectiveAmount" source-path="net-price"/>
      <Target property="ErpDiscountLine~Tax" source-path="tax"/>
      <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
      <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
      <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
      <Target property="ErpDiscountLine~RebateCode" source-path="custom-attributes/custom-attribute[@attribute-id=''rebate-code'']"/>
      <Target property="ErpDiscountLine~Amount" source-path="custom-attributes/custom-attribute[@attribute-id=''discount'']"/>
      <Target property="ErpDiscountLine~SppNumber" source-path="custom-attributes/custom-attribute[@attribute-id=''spp-no'']"/>
      <Target property="ErpDiscountLine~OfferName" source-path="custom-attributes/custom-attribute[@attribute-id=''reason-code'']"/>
    </Properties>
  </Target>

  <!--ErpSalesOrder Products-->
  <Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/product-lineitems" target-source="ErpSalesLine" repeat="true" >
    <Properties>
	  <Target property="ErpSalesLine~UnitOfMeasureSymbol" source-path="line-unit-of-measure" />
	  <Target property="ErpSalesLine~SalesOrderUnitOfMeasure" source-path="line-unit-of-measure" />
      <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
      <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
      <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
      <Target property="ErpSalesLine~Price" source-path="base-price"/>
      <Target property="ErpSalesLine~LineNumber" source-path="position"/>
      <Target property="ErpSalesLine~Description" source-path="product-id"/>
      <Target property="ErpSalesLine~ItemId" source-path="product-id"/>
      <Target property="ErpSalesLine~Quantity" source-path="quantity"/>
      <Target property="ErpSalesLine~QuantityOrdered" source-path="quantity"/>
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate"/>
      <Target property="ErpSalesLine~IsGiftCardLine" source-path="gift"/>
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id"/>

      <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']"/>
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''SKU'']" is-custom-attribute="true" attribute-id="SKU"/>
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''bogoPromotionId'']" is-custom-attribute="true" attribute-id="bogoPromotionId"/>
      <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']"/>


      <!--TVW FDD-007-->
      <!--Set in the "Synchronize orders" batch flow based Shipping Date Requested field on the SalesLin-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDFROM"/>
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom in the happy flow.-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCALCULATEFROM'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTCALCULATEFROM"/>
      <!--Set in the "Synchronize orders" batch flow and should be equal to TMVContractValidFrom + the length of the offer type from the deimension value - could also be empty if perpetual i.e. same logic implemented in Basic Contract FDD should be automatically triggerred. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDTO'']" default-value="" is-custom-attribute="true" attribute-id="TMVCONTRACTVALIDTO"/>
      <!--No logic implemented at time of writing - leave default empty value-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSTERMDATE'']"  is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSTERMDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTCANCELDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTPOSSCANCELDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTPOSSCANCELDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATE"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTTERMDATEEFFECTIVE'']" is-custom-attribute="true" attribute-id="TMVCONTRACTTERMDATEEFFECTIVE"/>
      <!--Ensure that is set to false (i.e. the default value). This is only used in more advanced scenarios and not the happy flow. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVAUTOPROLONGATION'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAUTOPROLONGATION"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM"/>
      <!--No logic implemented at time of writing - leave default empty value -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCUSTOMERREF'']" is-custom-attribute="true" attribute-id="TMVCUSTOMERREF"/>
      <!--Set in the "Synchronize orders" batch flow to value of "created". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="10" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE"/>
      <!--This field is required in order to post the confirmation but no logic implemented at time of writing - fill in with dummy value. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVEULAVERSION'']" default-value="v1" is-custom-attribute="true" attribute-id="TMVEULAVERSION"/>
      <!--Set in the "Synchronize orders" batch flow to value of "year". Note there is no logic tied to this field at the moment. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVBILLINGPERIOD'']" default-value="1" is-custom-attribute="true" attribute-id="TMVBILLINGPERIOD"/>
      <!--Random GUID passed from Magento/Postman but is inserted in a related table to the SalesLine hence the field is an Int64 RefRecId field. -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''PACLICENSE'']" default-value="" is-custom-attribute="true" attribute-id="PACLICENSE"/>
      <!--Set in the "Synchronize orders" batch flow. Copy value from SalesLine.LineAmount-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="gross-price" is-custom-attribute="true" attribute-id="TMVORIGINALLINEAMOUNT"/>
      <!--Set in the "Synchronize orders" batch flow to value of "yes"-->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVLINEMODIFIED'']" default-value="1" is-custom-attribute="true" attribute-id="TMVLINEMODIFIED"/>
      <!--Set in the "Synchronize orders" batch flow to value of "none". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVREVERSEDLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVREVERSEDLINE"/>
	  <!--Set in the "Synchronize orders" batch flow to value of "". -->
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMIGRATEDSALESLINENUMBER"/>
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''AffiliationRecId'']" default-value="0" is-custom-attribute="true" attribute-id="TMVAFFILIATIONRECID" />
      <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINENUMBER'']" default-value="0" is-custom-attribute="true" attribute-id="TMVOLDSALESLINENUMBER" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESLINEACTION'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESLINEACTION" />
	  <Target property="ErpSalesLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''TMVSOURCEID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSOURCEID" />
      <!--Reading options of product-->
      <!-- TMV has No Option Items-->
      <!--
      <Target property="ErpSalesOrder~Options" source-path="option-lineitems/option-lineitem" target-source="ErpSalesLine" repeat="true" >
        <Properties>
          <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
          <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
          <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
          <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
          <Target property="ErpSalesLine~Price" source-path="base-price"/>
          <Target property="ErpSalesLine~ItemId" source-path="product-id"/>
          <Target property="ErpSalesLine~Quantity" constant-value="1" />
          <Target property="ErpSalesLine~Comment" constant-value="OptionItem"/>

          <Target property="ErpSalesLine~SalesTaxGroupId" source-path="custom-attributes/custom-attribute[@attribute-id=''taxJurisdictionID'']"/>

          Reading discount of option
          <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
            <Properties>
              <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
              <Target property="ErpDiscountLine~Tax" source-path="tax"/>
              <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
              <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
              <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
			<Target property="ErpDiscountLine~OfferName" source-path="OfferName" />
				<Target property="ErpDiscountLine~EffectiveAmount" source-path="EffectiveAmount" />
				<Target property="ErpDiscountLine~Percentage" source-path="Percentage" />
				<Target property="ErpDiscountLine~DiscountLineTypeValue" source-path="DiscountLineTypeValue" />
				<Target property="ErpDiscountLine~ManualDiscountTypeValue" source-path="ManualDiscountTypeValue" />
				<Target property="ErpDiscountLine~CustomerDiscountTypeValue" source-path="CustomerDiscountTypeValue" />
				<Target property="ErpDiscountLine~PeriodicDiscountTypeValue" source-path="PeriodicDiscountTypeValue" />
				
            </Properties>
          </Target>
          <Target property="ErpSalesLine~InventoryLocationId" source-path="custom-attributes/custom-attribute[@attribute-id=''fromStoreId'']"/>
        </Properties>
      </Target>
      -->
      
      <!--Reading discount of product-->
      <Target property="ErpSalesLine~DiscountLines" source-path="price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
        <Properties>
          <Target property="ErpDiscountLine~Amount" source-path="net-price"/>
          <Target property="ErpDiscountLine~Tax" source-path="tax"/>
          <Target property="ErpDiscountLine~DiscountCode" source-path="promotion-id"/>
          <Target property="ErpDiscountLine~OfferId" source-path="campaign-id"/>
          <Target property="ErpDiscountLine~CouponId" source-path="coupon-id"/>
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

  <!--Shipping-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~DeliveryMode" source-path="//orders/order/shipments/shipment/shipping-method"/>
  <Target property="ErpSalesOrder~DeliveryModeChargeAmount" source-path="//orders/order/shipping-lineitems/shipping-lineitem/gross-price"/>
  -->
  <!-- Shipping Tax is commented out because we are taking gross amount to shipping charges which includes tax-->
  <!--<Target property="ErpSalesOrder~Shipping_Tax" source-path="//orders/order/shipping-lineitems/shipping-lineitem/tax"/>-->

  <!--Reading discount of Shipping-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~ShippingDiscounts" source-path="//orders/order/shipping-lineitems/shipping-lineitem/price-adjustments/price-adjustment" target-source="ErpDiscountLine" repeat="true" >
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
  -->

  <!--New mapping with orders/order/shipping-lineitems/shipping-lineitem-->
  <!--<Target property="ErpSalesOrder~SalesLines" source-path="//orders/order/shipping-lineitems/shipping-lineitem" target-source="ErpSalesLine" repeat="true" >
    <Properties>
      <Target property="ErpSalesLine~Price" source-path="base-price"/>
      <Target property="ErpSalesLine~NetAmount" source-path="net-price"/>
      <Target property="ErpSalesLine~TaxAmount" source-path="tax"/>
      <Target property="ErpSalesLine~TaxRatePercent" source-path="tax-rate"/>
      <Target property="ErpSalesLine~TotalAmount" source-path="gross-price"/>
      <Target property="ErpSalesLine~BasePrice" source-path="base-price"/>
      <Target property="ErpSalesLine~ItemId" source-path="item-id"/>
      <Target property="ErpSalesLine~ShipmentId" source-path="shipment-id"/>
      <Target property="ErpSalesLine~Quantity" constant-value="1"/>
      <Target property="ErpSalesLine~Comment" constant-value="DeliveryItem"/>
    </Properties>
  </Target>-->

  <Target property="ErpSalesOrder~BillingAddress" source-path="//orders/order/customer/billing-address" target-source="ErpAddress" repeat="false">
    <Properties>
      <Target property="ErpAddress~Name" source-path="first-name"/>
      <Target property="ErpAddress~Name" source-path="last-name" concatenate="true"/>
      <Target property="ErpAddress~Street" source-path="address1"/>
      <Target property="ErpAddress~City" source-path="city"/>
      <Target property="ErpAddress~ZipCode" source-path="postal-code"/>
      <Target property="ErpAddress~State" source-path="state-code"/>
      <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code"/>
      <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code"/>
      <Target property="ErpAddress~Phone" source-path="phone"/>
    </Properties>
  </Target>

  <!--ErpSalesOrder Shipments-->
  <!-- TMV has No Shipments-->
  <!--
  <Target property="ErpSalesOrder~Shipments" source-path="//orders/order/shipments/shipment" target-source="ErpShipment" repeat="true" >
    <Properties>
      <Target property="ErpShipment~ShipmentId" source-path="" attribute-name="shipment-id"/>
      <Target property="ErpShipment~DeliveryMode" source-path="shipping-method"/>
      <Target property="ErpShipment~ShippingStatus" source-path="status/shipping-status"/>
      <Target property="ErpShipment~IsGift" source-path="gift"/>

      <Target property="ErpShipment~DeliveryAddress" source-path="shipping-address" target-source="ErpAddress" repeat="false">
        <Properties>
          <Target property="ErpAddress~Name" source-path="first-name"/>
          <Target property="ErpAddress~Name" source-path="last-name" concatenate="true"/>
          <Target property="ErpAddress~Street" source-path="address1"/>
          <Target property="ErpAddress~City" source-path="city"/>
          <Target property="ErpAddress~ZipCode" source-path="postal-code"/>
          <Target property="ErpAddress~State" source-path="state-code"/>
          <Target property="ErpAddress~TwoLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~ThreeLetterISORegionName" source-path="country-code"/>
          <Target property="ErpAddress~Phone" source-path="phone"/>
        </Properties>
      </Target>
    </Properties>
  </Target>
  -->
  
  <!--ErpSalesOrder Payments-->
  <Target property="ErpSalesOrder~TenderLines" source-path="//orders/order/payments/payment" target-source="ErpTenderLine" repeat="true" >
    <Properties>
      <!--Common for all Payment methods-->
      <Target property="ErpTenderLine~TenderTypeId" source-path="processor-id"/>
      <Target property="ErpTenderLine~Amount" source-path="amount"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="transaction-id" is-custom-attribute="true" attribute-id="transaction-id"/>
      <!--AllPago-->
      <Target property="ErpTenderLine~NumberOfInstallments" source-path="numberOfInstallments" />
	  <!--PayPal-->
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAck'']" is-custom-attribute="true" attribute-id="paypalAck"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalAmount'']" is-custom-attribute="true" attribute-id="paypalAmount"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalCorrelationId'']" is-custom-attribute="true" attribute-id="paypalCorrelationId"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPayerID'']" is-custom-attribute="true" attribute-id="paypalPayerID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentStatus'']" is-custom-attribute="true" attribute-id="paypalPaymentStatus"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalToken'']" is-custom-attribute="true" attribute-id="paypalToken"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-method/custom-attributes/custom-attribute[@attribute-id=''paypalTransactionID'']" is-custom-attribute="true" attribute-id="paypalTransactionID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="custom-attributes/custom-attribute[@attribute-id=''transactionsHistory'']" is-custom-attribute="true" attribute-id="transactionsHistory"/>

      <!--CreditCard-->
      <Target property="ErpTenderLine~CardTypeId" source-path="credit-card/card-type"/>
      <Target property="ErpTenderLine~MaskedCardNumber" source-path="credit-card/card-number"/>
      <Target property="ErpTenderLine~CardOrAccount" source-path="credit-card/card-holder"/>
      <!--Payment Connector info UY-->
      <Target property="ErpTenderLine~Authorization" source-path="credit-card/authorization" />
      <Target property="ErpTenderLine~CardToken" source-path="credit-card/card-token" />
      <Target property="ErpTenderLine~ExpMonth" source-path="credit-card/expiration-month" />
      <Target property="ErpTenderLine~ExpYear" source-path="credit-card/expiration-year" />
	  <Target property="ErpTenderLine~PayerId" source-path="credit-card/payerid" />
      <Target property="ErpTenderLine~ParentTransactionId" source-path="credit-card/parent-transaction-id" />
      <Target property="ErpTenderLine~Email" source-path="credit-card/email" />
      <Target property="ErpTenderLine~Note" source-path="credit-card/note" />


      <!--Ending Payment Connector info UY-->
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''tokenNumber'']" is-custom-attribute="true" attribute-id="tokenNumber"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID"/>
      <Target property="ErpTenderLine~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod"/>

    </Properties>
  </Target>

  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Fraud_Status'']" is-custom-attribute="true" attribute-id="Fraud_Status"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''Realtime_auth_and_fraud_check_done'']" is-custom-attribute="true" attribute-id="Realtime_auth_and_fraud_check_done"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authStatus'']" is-custom-attribute="true" attribute-id="authStatus"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''authorizationCode'']" is-custom-attribute="true" attribute-id="authorizationCode"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paymentProfileID'']" is-custom-attribute="true" attribute-id="paymentProfileID"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''responseDateTime'']" is-custom-attribute="true" attribute-id="responseDateTime"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceIndex'']" is-custom-attribute="true" attribute-id="transactionReferenceIndex"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''transactionReferenceNumber'']" is-custom-attribute="true" attribute-id="transactionReferenceNumber"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''webOrderID'']" is-custom-attribute="true" attribute-id="webOrderID"/>
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''paypalPaymentMethod'']" is-custom-attribute="true" attribute-id="paypalPaymentMethod"/>

  <!--TVW FDD-007-->
  <Target property="ErpSalesOrder~RequestedDeliveryDate" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTVALIDFROM'']"/>
  
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVRESELLERACCOUNT'']" is-custom-attribute="true" attribute-id="TMVRESELLERACCOUNT"/>
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVDISTRIBUTORACCOUNT'']" is-custom-attribute="true" attribute-id="TMVDISTRIBUTORACCOUNT"/>
  <!--Should remain empty: only applicable for use in call-center orders in BR1 -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINDIRECTCUSTOMER'']" is-custom-attribute="true" attribute-id="TMVINDIRECTCUSTOMER"/>
  <!--Set in the "Synchronize orders" batch flow based on the TMVMainOfferType of the Offer Type Group dimension line. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMAINOFFERTYPE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVMAINOFFERTYPE"/>
  <!--Set in the "Synchronize orders" batch flow - more information to be received about this field. Contract confirmation fails if this field has no value. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPRODUCTFAMILY'']" is-custom-attribute="true" attribute-id="TMVPRODUCTFAMILY"/>
  <!--This should be mapped to value ‚1‘ i.e. contract in the commerce link mapper. Orders created from the RetailSynchOrdersSchedulerTask class should use the value coming from the mapper and ignore the default value set on Accounts Receivable. This is for future-compatabilty reasons. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORDERSUBTYPE'']" default-value="1" is-custom-attribute="true" attribute-id="TMVSALESORDERSUBTYPE"/>
  <!--Ensure that is set to false (i.e. the default value). This is only used in Advanced Contract Management -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVINVOICESCHEDULECOMPLETE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVINVOICESCHEDULECOMPLETE"/>
  <!--Set in the "Synchronize orders" batch flow based on the lowest value of SalesLine.TMVContractStatusLine within all contract lines. -->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCONTRACTSTATUSLINE'']" default-value="0" is-custom-attribute="true" attribute-id="TMVCONTRACTSTATUSLINE"/>
  <!--This should be passed from Magento when the order is created as part of a marketing campaing (such as email sent to customer from Marketo). Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSMMCAMPAIGNID'']" default-value="" is-custom-attribute="true" attribute-id="TMVSMMCAMPAIGNID"/>
  <!--This should be passed from Magento . Part of work package WP-S07.-->
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPURCHORDERFORMNUM'']" default-value="" is-custom-attribute="true" attribute-id="TMVPURCHORDERFORMNUM" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVPIT'']" default-value="" is-custom-attribute="true" attribute-id="TMVPIT" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVQUOTATIONID'']" default-value="" is-custom-attribute="true" attribute-id="TMVQUOTATIONID" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVMIGRATEDORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVMIGRATEDORDERNUMBER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOLDSALESORDERNUMBER'']" default-value="" is-custom-attribute="true" attribute-id="TMVOLDSALESORDERNUMBER" />
  <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVOriginatingCountry'']" default-value="" is-custom-attribute="true" attribute-id="TMVOriginatingCountry" />  
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFORORDER'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFORORDER" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVCOMMENTFOREMAIL'']" default-value="" is-custom-attribute="true" attribute-id="TMVCOMMENTFOREMAIL" />
   <Target property="ErpSalesOrder~CustomAttributes" source-path="//orders/order/custom-attributes/custom-attribute[@attribute-id=''TMVSALESORIGIN'']" default-value="" is-custom-attribute="true" attribute-id="TMVSALESORIGIN" />
</Targets>'
WHERE	[Name] = 'READ.ErpSalesOrder'
AND		StoreId = @AppleAppStoreID;