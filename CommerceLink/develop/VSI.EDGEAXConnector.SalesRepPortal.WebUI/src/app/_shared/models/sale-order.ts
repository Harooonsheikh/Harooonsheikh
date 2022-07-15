import * as uuid from 'uuid';
import { formatDate } from '@angular/common';
import { environment } from 'src/environments/environment';
export class Order {
    order: SaleOrderRequest = new SaleOrderRequest();
}
export class SaleOrderRequest {
    // order-no : string;
    "@order-no": string;
    "order-date": string;
    "created-by": string;
    "original-order-no": string;
    "currency": string;
    "staff-id": string;
    "customer-locale": string;
    "taxation": string;
    "invoice-no": string;
    "customer": CustomerList = new CustomerList();
    "status": OrderStatus = new OrderStatus();
    "current-order-no": string;
    "product-lineitems": ProductLineItemList = new ProductLineItemList();
    "shipping-lineitems": [] = [];
    "shipments": [] = [];
    "totals": TotalsList = new TotalsList();
    "payments": PaymentList = new PaymentList();
    "remoteHost": string;
    "custom-attributes": CustomAttribute = new CustomAttribute();

    constructor() {
        this["@order-no"] = environment.SaleOrderPrefix + uuid.v4();
        this["created-by"] = "NAL-Shop"
        this["original-order-no"] = this["@order-no"];
        this["customer-locale"] = "en_US";
        this.taxation = "net";
        this["current-order-no"] = this["@order-no"];
        this["invoice-no"] = "";

    }
}

export class CustomerList {
    "customer-no": string;
    "customer-name": string;
    "customer-email": string;
    "billing-address": CustomerAddress = new CustomerAddress();
}

export class CustomerAddress {

    "first-name": string;
    "last-name": string;
    "address1": string;
    "city": string;
    "postal-code": string;
    "state-code": string;
    "country-code": string;
    "phone": string;
    constructor() {
        this["state-code"] = "";
    }
}

export class OrderStatus {
    "order-status": string;
    "shipping-status": string;
    "confirmation-status": string;
    "payment-status": string;
    constructor() {
        this["order-status"] = "OPEN";
        this["shipping-status"] = "NOT_SHIPPED";
        this["confirmation-status"] = "CONFIRMED";
        this["payment-status"] = "NOT_PAID";
    }
}

export class ProductLineItemList {
    "product-lineitem": PorductLineItem[] = [];
}
export class PorductLineItem {
    "net-price": number;
    "tax": number;
    "gross-price": number;
    "base-price": number;
    "lineitem-text": string;
    "tax-basis": string
    "position": string;
    "product-id": string;
    "product-name": string;
    "quantity": Quantity = new Quantity();
    "tax-rate": number;
    "shipment-id": string;
    "line-unit-of-measure": string;
    "gift": string;
    "price-adjustments" : PriceAdjustmentList = new PriceAdjustmentList();
    "custom-attributes": CustomAttribute = new CustomAttribute();
    constructor() {
        this.gift = "false";
        this["shipment-id"] = "";
        this["product-name"] = this["lineitem-text"];
        // this["tax-rate"] = (this.tax / this["net-price"]) * 100;
    }
}
export class PriceAdjustmentList {
    "price-adjustment": PriceAdjustment[] = [];
}
export class PriceAdjustment {
    "net-price": number;
    "tax": number;
    "promotion-id": string;
    "DiscountCode": string;
    "campaign-id": string;
    "OfferName": string;
    "Percentage": number;
    "DiscountLineTypeValue": number;
    "ManualDiscountTypeValue": number;
    "CustomerDiscountTypeValue": number;
    "PeriodicDiscountTypeValue": number;
}

export class CustomAttribute {
    "custom-attribute": CustomAttributesArray[] = [];

    GetProductLineCustomAttribute(): CustomAttributesArray[] {
        let customAttrArray: CustomAttributesArray[] = [];
        var customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVBILLINGPERIOD";
        customAttr["#text"] = "1";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "PACLICENSE";
        customAttr["#text"] = "83D9CB9F-C7B1-4F15-AED1-FD3B53D17696";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVSOURCEID";
        customAttr["#text"] = "Direct Sales";
        customAttrArray.push(customAttr);

        return customAttrArray;
    }

    GetSalesOrderCustomAttribute(): CustomAttributesArray[] {
        let customAttrArray: CustomAttributesArray[] = [];
        var customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVCONTRACTVALIDFROM";
        customAttr["#text"] = formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss', 'en-US', '+0530');
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVMAINOFFERTYPE";
        customAttr["#text"] = "2";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVSMMCAMPAIGNID";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVPURCHORDERFORMNUM";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVPIT";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVQUOTATIONID";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVCOMMENTFORORDER";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVCOMMENTFOREMAIL";
        customAttr["#text"] = "";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVSALESORIGIN";
        customAttr["#text"] = "Inbound Call";
        customAttrArray.push(customAttr);

        customAttr = new CustomAttributesArray();
        customAttr["@attribute-id"] = "TMVPaymentTerms";
        customAttr["#text"] = "14d";
        customAttrArray.push(customAttr);

        return customAttrArray;
    }
}

export class CustomAttributesArray {
    "@attribute-id": string;
    "#text": string;
}

export class Quantity {
    "#text": string;
    constructor() {
        this["#text"] = "1";
    }
}

export class TotalsList {
    "merchandize-total": TotalGenericList = new TotalGenericList();
    "adjusted-merchandize-total": TotalGenericList = new TotalGenericList();
    "shipping-total": TotalGenericList[] = [];
    "adjusted-shipping-total": TotalGenericList[] = [];
    "order-total": TotalGenericList = new TotalGenericList();
}

export class TotalGenericList {
    "net-price": number;
    "tax": number;
    "gross-price": number;
}

export class PaymentList {
    payment: Payment = new Payment();
}

export class Payment {
    "credit-card": CreditCardPayment = new CreditCardPayment();
    "amount": number;
    "processor-id": string;
    "transaction-id": string;
    constructor() {
        this["processor-id"] = "PURCHASEORDER";
        // this["transaction-id"] = "1000003";
    }
}

export class CreditCardPayment {
    "card-type": string;
    "card-number": string;
    "card-holder": string;
    "expiration-month": string;
    "expiration-year": string;
    "authorization": string;
    "card-token": string;
    constructor() {
        this["card-type"] = "";
        this["card-number"] = "";
        this["card-holder"] = "";
        this["expiration-month"] = "0";
        this["expiration-year"] = "0";
        // this.authorization = "afb9abc1-38cc-4af8-8eb4-a3546cd93ec8";
        // this["card-token"] = "460902671660111d1";
    }
}

export class QuoteRejectRequest{
    ReasonId: string;
    QuotationId: string;
    constructor(){
        this.ReasonId = "super";
    }
}