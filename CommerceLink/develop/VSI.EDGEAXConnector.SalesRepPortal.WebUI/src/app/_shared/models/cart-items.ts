import * as uuid from 'uuid';
import { Address } from './address';
import { timingSafeEqual } from 'crypto';
import { environment } from 'src/environments/environment';

export class Cart{
    CartId: string;
    CalculationModes: string;
    AffiliationId: number;
    DeliverySpecification: DeliverySpecification = new DeliverySpecification();
    isLegacyDiscountCode: boolean;
    CartVersion: number;
    CartLines: CartItem []= [];
    CartSummary: Summary = new Summary();
    CouponCodes: any;
    IsCartCreated: boolean;

    constructor(){
      this.CartId = environment.SaleOrderPrefix + uuid.v4();
      this.CalculationModes = "All";
      this.isLegacyDiscountCode = false;
    }
}

export class Summary{
    BasePrice: number = 0;
    Price: number = 0;
    PriodicDiscount: number = 0;
    ManualDiscount: number = 0;
    PriceWithPriodicDiscount: number = 0;
    DiscountAmount: number = 0;
    Tax: number = 0;
    ExtendedPrice: number = 0;
    TaxRatePercent: number = 0;
    // TotalLicense: number = 0;
    // TotalTax: number = 0;
    NetAmountWithoutTax: number = 0;
    Total: number = 0;
    TotalWithManualPriodicDiscount: number = 0;
    GrandTotal: number = 0;
}

export class CartItem {
    rowId: number;
    name: string; // TODO: remove this property and will use Description only.
    
    Quantity: number = 0;
    QuantityIcrement: number;
    QuantityMin: number;
    QuantityMax: number;

    UnitOfMeasureSymbol: string;
    billingInterval: string;
    offerType: string;
    currency: string;
    tax: number = 0;
    parentProduct: boolean;
    discount: boolean;
    discountAmount: number = 0;
    ManualDiscountAmount: number = 0;
    license: string;
    price: number;
    basePrice: number;
    validFrom: string;
    validTo: string;
    adOnsAvailable: boolean;
    IsSelected: boolean;
    IsAddonSelected: boolean;
    adOnDetails: Array<CartItem> = new Array<CartItem>();

    LineId: string;
    Description: string;
    // UnitOfMeasure: string;
    ItemId: string;
    EntryMethodTypeValue: number;
    CommissionSalesGroup: null;
    CatalogId: 0;
    IsCreateCartWithLine: boolean;

    IsLineItem: boolean;
    CrossSellSku: string;
    totalAmountWithTax: number = 0;
    IsLineItemCreated: boolean;
    IsLineItemChange: boolean;
    IsValidQuantity:boolean=true;

    MasterItemId: string; 

    IsChannel: boolean;
    ConcurrentUsers: number;

    DiscountDetails: Array<Discount> = [];
    LineSummary: Summary = new Summary();
    AdonLineSummary: Summary = new Summary();
    RecId: string;

    LineManualDiscountAmount: number = 0;
    LineManualDiscountPercentage: number = 0;

}


export class Discount {
    discountOrigin: string;
    reasonCode: number;
    discountMethodPer: boolean;
    discountMethodCash: boolean;
    discountMethodTarget: boolean;
    discountPercentage: number;
    sku: string;
    periodicDiscount: boolean;
    DiscountAmount: number;
    Method: number;
    Amount: number;

    MonthlyPrice: number;
    Interval: number;
    NextPrice: number;
    Tax: number;
    OfferId: number;
    DiscountCode: string;
    OfferName: string;
    Percentage: number;
    DiscountLineTypeValue: number;
    ManualDiscountTypeValue: number;
    CustomerDiscountTypeValue: number;
    PeriodicDiscountTypeValue: number;

    constructor(){
        this.periodicDiscount =false;
        this.Interval = 1;
    }
}

export class Product{
    sku : string;
    name: string;
    price: number;
    qty: number;
    addOns : CartItem[] = [];
    biot: Variations[] = [];
}

export class Variations{
    billingInterval: string;
    offerType: string;
}

export class AddOns{
    adOns: CartItem []= [];
}

export class DeliverySpecification {
    DeliveryModeId: string;
    DeliveryPreferenceTypeValue: number;
    DeliveryAddress: Address;

    constructor(){
        this.DeliveryModeId = '';
        this.DeliveryPreferenceTypeValue = 1;
    }
}

export class PopularProductsGrid{
    IsSelected: boolean;
    ItemId: string;
    ProductName: string;
    BillingInterval: string;
    OfferType: string;
    MasterItemId: string;
    constructor(){
        this.IsSelected = false;
    }
}