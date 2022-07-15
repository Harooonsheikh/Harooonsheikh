import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Cart, Discount, CartItem } from '../models/cart-items';
import { Order, SaleOrderRequest, PorductLineItem, PriceAdjustment } from '../models/sale-order';
import { Customer } from '../models/customer';
import { formatDate } from '@angular/common';
import { http } from './http';
import { AppConfig } from '../constants/app-config';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ReadModelAsStringFloatingFilterComp } from 'ag-grid-community/dist/lib/filter/floatingFilter';

@Injectable({
    providedIn: 'root'
})
export class SaleOrderService {

    public saleOrderRequest: SaleOrderRequest = new SaleOrderRequest();
    public productLineItems: Array<PorductLineItem> = [];
    public OrderJSON: Order = new Order();
    saleOrderStr: any = {};
    SalesRepId: string;
    salesRepName: string;
    saleOrderNumber: any = {};

    constructor(private httpService: http, private toastr: ToastrService, private route: ActivatedRoute) {
        this.salesRepName = this.route.snapshot.paramMap.get('salesRap');
        this.SalesRepId = this.route.snapshot.paramMap.get('erpId');
    }

    public CreateSaleOrderTransaction(cart: Cart, customerInfo: Customer, OrderInfo, userId?: string): Observable<any> {

        let now = new Date();
        let NowDateTime = formatDate(now, 'yyyy-MM-dd hh:mm:ss', 'en-US', '+0530');

        //Mapping of Customer & Cart Lines into SalesOrder
        this.saleOrderRequest["order-date"] = NowDateTime;
        this.saleOrderRequest.currency = customerInfo.CurrencyCode;
        this.saleOrderRequest["staff-id"] = OrderInfo['SalesRepId'];

        //Sale order Customer Start
        this.saleOrderRequest.customer["customer-no"] = customerInfo.AccountNumber;
        this.saleOrderRequest.customer["customer-name"] = customerInfo.Name;
        this.saleOrderRequest.customer["customer-email"] = customerInfo.Email;
        this.saleOrderRequest.customer["billing-address"]["first-name"] = customerInfo.FirstName;
        this.saleOrderRequest.customer["billing-address"]["last-name"] = customerInfo.LastName;
        this.saleOrderRequest.customer["billing-address"].address1 = customerInfo.InvoiceAddress.FullAddress;
        this.saleOrderRequest.customer["billing-address"].city = customerInfo.InvoiceAddress.City;
        this.saleOrderRequest.customer["billing-address"]["postal-code"] = customerInfo.InvoiceAddress.ZipCode;
        this.saleOrderRequest.customer["billing-address"]["country-code"] = customerInfo.InvoiceAddress.ThreeLetterISORegionName;
        this.saleOrderRequest.customer["billing-address"].phone = customerInfo.InvoiceAddress.Phone;
        //Sale order Customer End

        //Sale order Price Start
        this.saleOrderRequest.totals["merchandize-total"]["net-price"] = cart.CartSummary.Price;
        this.saleOrderRequest.totals["merchandize-total"].tax = cart.CartSummary.Tax;
        this.saleOrderRequest.totals["merchandize-total"]["gross-price"] = cart.CartSummary.Total;

        this.saleOrderRequest.totals["adjusted-merchandize-total"]["net-price"] = cart.CartSummary.Price;
        this.saleOrderRequest.totals["adjusted-merchandize-total"].tax = cart.CartSummary.Tax;
        this.saleOrderRequest.totals["adjusted-merchandize-total"]["gross-price"] = cart.CartSummary.Total;

        this.saleOrderRequest.totals["order-total"]["net-price"] = cart.CartSummary.Price;
        this.saleOrderRequest.totals["order-total"].tax = cart.CartSummary.Tax;
        this.saleOrderRequest.totals["order-total"]["gross-price"] = cart.CartSummary.Total;

        this.saleOrderRequest.payments.payment.amount = cart.CartSummary.Total;
        this.saleOrderRequest.remoteHost = "";

        // Product lines mapping Start
        this.productLineItems = [];
        cart.CartLines.forEach((line, index) => {
            let productLine = this.MapCartItemToProductLineItem(line);
            this.productLineItems.push(productLine);

            line.adOnDetails.forEach(ad => {
                if (ad.IsAddonSelected) {
                    let productLine = this.MapCartItemToProductLineItem(ad);
                    this.productLineItems.push(productLine);
                }
            });
        });

        this.saleOrderRequest["product-lineitems"]["product-lineitem"] = this.productLineItems;
        // Product lines mapping End

        // Payment attribute Start
        this.saleOrderRequest.payments.payment["transaction-id"] = this.saleOrderRequest["@order-no"];
        this.saleOrderRequest.payments.payment.amount = cart.CartSummary.Total;
        // Payment attribute End

        // Sales order Custom Attributes Start
        let customAtrrs = this.saleOrderRequest["custom-attributes"].GetSalesOrderCustomAttribute();
        let commentForOrder = customAtrrs.find(attr => attr["@attribute-id"] == "TMVCOMMENTFORORDER");
        if (commentForOrder != undefined) {
            commentForOrder["#text"] = OrderInfo['commentsForOrder'];
        }

        let commentForEmail = customAtrrs.find(attr => attr["@attribute-id"] == "TMVCOMMENTFOREMAIL");
        if (commentForEmail != undefined) {
            commentForEmail["#text"] = OrderInfo['commentsForEmail'];
        }

        let quotationRefrence = customAtrrs.find(attr => attr["@attribute-id"] == "TMVQUOTATIONID");
        if (quotationRefrence != undefined) {
            quotationRefrence["#text"] = OrderInfo['quotationId'];
        }

        this.saleOrderRequest["custom-attributes"]["custom-attribute"] = customAtrrs;
        // Sales order Custom Attributes End


        this.OrderJSON.order = this.saleOrderRequest;
        this.saleOrderStr['salesOrderJSON'] = JSON.stringify(this.OrderJSON);

        return this.httpService.post(AppConfig.createSalesOrder, this.saleOrderStr).pipe(
            map(res => {
                // this.extractData
                this.SaleOrderSave(res['salesOrderTransactionId'], cart, customerInfo, userId);
                return res;
            }),
            catchError(this.handleErrorObservable)
        );
    }

    extractData(res: Response) {
        const body = res;
        return res;
    }

    handleErrorObservable(error: Response | any) {
        console.error(error.message || error);
        this.toastr.error("Failed to Perform Operation");
        return Observable.throw(error.message || error);
    }

    private MapCartItemToProductLineItem(line: CartItem): PorductLineItem {
        let productLine = new PorductLineItem();
        productLine["base-price"] = line.LineSummary.BasePrice;
        productLine["net-price"] = line.LineSummary.ExtendedPrice;
        productLine.tax = line.LineSummary.Tax;
        productLine["gross-price"] = line.LineSummary.Total;
        productLine["tax-rate"] = line.LineSummary.TaxRatePercent;
        productLine["product-id"] = line.ItemId;
        productLine["product-name"] = line.name;
        productLine["lineitem-text"] = line.name;
        productLine["line-unit-of-measure"] = line.UnitOfMeasureSymbol;
        productLine.position = "1";
        productLine.quantity["#text"] = line.Quantity.toString();
        productLine["custom-attributes"]["custom-attribute"] = this.saleOrderRequest["custom-attributes"].GetProductLineCustomAttribute();

        // Price Adjustment start
        productLine["price-adjustments"]["price-adjustment"] = [];
        line.DiscountDetails.forEach(dd => {
            let priceAdjustment = this.MapDiscountToPriceAdjustment(dd);
            productLine["price-adjustments"]["price-adjustment"].push(priceAdjustment);
        });
        // Price Adjustment End

        return productLine;
    }

    private MapDiscountToPriceAdjustment(discount: Discount): PriceAdjustment {
        let priceAdjustment = new PriceAdjustment();
        priceAdjustment["net-price"] = discount.DiscountAmount;
        priceAdjustment.tax = discount.Tax;
        priceAdjustment["promotion-id"] = discount.DiscountCode;
        priceAdjustment["DiscountCode"] = discount.DiscountCode;
        priceAdjustment["campaign-id"] = "";
        priceAdjustment["OfferName"] = discount.OfferName;
        priceAdjustment["Percentage"] = discount.Percentage;
        priceAdjustment["DiscountLineTypeValue"] = discount.DiscountLineTypeValue;
        priceAdjustment["ManualDiscountTypeValue"] = discount.ManualDiscountTypeValue;
        priceAdjustment["CustomerDiscountTypeValue"] = discount.CustomerDiscountTypeValue;
        priceAdjustment["PeriodicDiscountTypeValue"] = discount.PeriodicDiscountTypeValue;

        return priceAdjustment;
    }

    //#region Sale Order
    SaleOrderSave(saleOrderId: string, cart: Cart, customerInfo: Customer, userId?: string) {
        let order: any = {};
        order.Id = saleOrderId;
        order.CustomerId = customerInfo.AccountNumber;
        order.CustomerName = customerInfo.Name;
        order.CreatedOn = new Date();
        order.UserId = userId;
        order.Content = JSON.stringify(this.saleOrderRequest);
        order.CartContent = JSON.stringify(cart);
        order.Type = 1;

        this.httpService.post(AppConfig.SalesOrderSave, order).subscribe(res => {
            
        });
    }
    //#endregion

}
