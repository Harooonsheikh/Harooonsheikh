import * as uuid from 'uuid';
import { SpawnSyncOptionsWithBufferEncoding } from 'child_process';
export class Qoutation{
    customerQuotation: CustomerQoutation = new CustomerQoutation();
    IsEcomCustomerId: boolean;
    constructor(){
        this.IsEcomCustomerId = false;
    }
}
export class CustomerQoutation{
    OrderType: string;
    CustomerAccount: string;
    TransactionId: string;
    RequestedDeliveryDateString: string;
    ChannelReferenceId: string;
    ExpiryDateString: string;
    Items: ItemsList[] = []
    CustomAttributes: CustomAttributesList[] = [];
    constructor(){
        this.OrderType = "Quote";
        this.TransactionId = "TMV-BEN-NS-"+ uuid.v4();
        // this.ChannelReferenceId = this.TransactionId;
    }
}

export class ItemsList{
    AddressRecordId: string;
    LineNumber: number;
    ItemId: string;
    Discount: number;
    DiscountPercent: number;
    Discounts: DiscountList[] = []; 
    NetAmount: number;
    Price: number;
    Quantity: number;
    Taxes: TaxList[] = [];
    RequestedDeliveryDateString: string;
    SourceId: string;
    TMVCONTRACTVALIDFROM: string;
    TMVCONTRACTCALCULATEFROM: string;
    TMVCONTRACTVALIDTO: string;
    TMVORIGINALLINEAMOUNT: string;
    UNIT: string;
    StaffId: string;
    CustomAttributes: CustomAttributesList[] = [];

    constructor(){
        // this.DiscountPercent = 0;
        this.UNIT = "pcs";
    }
}

export class DiscountList{
    Amount: number;
    DiscountAmount: number;
    DiscountCode: string;
    PromotionId: string;
    OfferName: string;
    Percentage: number;
    PeriodicDiscountOfferId: number;
    CustomerDiscountType: number;
    DiscountOriginType: number;
    ManualDiscountType: number;
}

export class CustomAttributesList{
    Key: string;
    Value: string;
}

export class TaxList{
    Amount: number;
}
