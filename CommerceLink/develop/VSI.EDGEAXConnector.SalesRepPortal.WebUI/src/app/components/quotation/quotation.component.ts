import { Component, OnInit, AfterViewInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Location } from '@angular/common';
import { http } from '../../_shared/services/http';
import { ActivatedRoute } from '@angular/router';
import { SaleOrderService } from '../../_shared/services/sale-order-service';
import { Order, SaleOrderRequest, PorductLineItem, QuoteRejectRequest } from '../../_shared/models/sale-order';
import { Qoutation, ItemsList, CustomAttributesList, TaxList, DiscountList } from '../../_shared/models/qoute';
import { Opportunity } from '../../_shared/models/opportunity';
import { ToastrService } from 'ngx-toastr';
import { AppConfig } from '../../_shared/constants/app-config';
import { formatDate } from '@angular/common';
import { PurchaseCartComponent } from '../sale-order/cart/purchase-cart/purchase-cart.component';
import { Cart } from 'src/app/_shared/models/cart-items';
import { CustomerInformationComponent } from '../sale-order/customer/customer-information/customer-information.component';
import { Customer } from 'src/app/_shared/models/customer';
import { Utilities } from '../../_shared/constants/utilities';
import { BehaviourSubjectService } from '../../_shared/services/behaviour-subject-service'
import { MockData } from 'src/app/_shared/constants/mock-data';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { jqxGridComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid';
import { Dropdown } from 'src/app/_shared/models/common';


@Component({
  selector: 'app-quotation',
  templateUrl: './quotation.component.html',
  styleUrls: ['./quotation.component.scss']
})
export class QuotationComponent implements OnInit, AfterViewInit {
  @ViewChild(PurchaseCartComponent) purchaseCart;
  @ViewChild(CustomerInformationComponent) CustomerInformationComponent;
  @ViewChild('GridMyOrders') GridMyOrders: jqxGridComponent;

  public saleOrderRequest: SaleOrderRequest = new SaleOrderRequest();
  public quotationRejectRequest: QuoteRejectRequest = new QuoteRejectRequest();
  public customerInfo: Customer;
  InvoiceAddress: any = "";
  salesRepName: any = '';
  salesRepFirstletter: any = '';
  public customerCollapsed = true;
  public cartCollapsed = false;
  public SaleOrderCollapsed = true;
  public cartLinesInfo: any = "";
  public qoutationId: any = "";
  public productLineItems: Array<PorductLineItem> = [];
  saleOrderStr: any = {};
  saleOrderNumber: any = "";
  commentsForOrder: string;
  commentsForEmail: string;

  ddlPaymentTerms: Dropdown[];
  paymentTerms: string;

  orderCreated: boolean = false;
  public OrderJSON: Order = new Order();
  public paymentTermsDays: any = "";
  public cart: Cart;

  //Qoutation
  public QouteJSON: Qoutation = new Qoutation();
  public oppurtunity: Opportunity = new Opportunity();
  qoutationCreated: boolean = false;
  qouteId: string;
  expiryDateValue: Date;
  minDate: Date;
  maxDate: Date;
  changePaymentTermsandExpiray: boolean = false;
  expiryDate: string;
  utility: any = "";
  contactPersonsInfo: any = '';
  public oppurtunityId: any = '';
  public SalesRepId: string = '';
  public CustomerId: string;
  private buttonDisabled: boolean = false;
  private expDateSet: boolean = true;
  quotationStatus: string = '';
  quotationStatusSelected: boolean = false;
  disablePage: boolean = false;
  LoadQuote: boolean = false;
  firststep: boolean = true;
  expirayDateLoad: string;
  dataAdapter: any;
  columns: any[] =
    [
      { text: 'Reference Number', columntype: 'textbox', datafield: 'Id', width: '40%' },
      { text: 'Type', columntype: 'textbox', datafield: 'Type', width: '15%' },
      { text: 'Customer Name', columntype: 'textbox', datafield: 'CustomerName', width: '20%' },
      { text: 'Date', columntype: 'textbox', datafield: 'CreatedOn', width: '25%' }
    ];

  constructor(private location: Location, private toastr: ToastrService,
    private http: http,
    private saleOrderService: SaleOrderService,
    private route: ActivatedRoute,
    private behaviorService: BehaviourSubjectService,
    private modalService: NgbModal,
    private cdr: ChangeDetectorRef) {
    this.utility = new Utilities();
  }

  ngOnInit() {
    this.ddlPaymentTerms = MockData.PaymentTerms;

    this.expiryDateValue = this.minDate = new Date;
    this.maxDate = new Date();
    this.minDate.setDate(this.minDate.getDate() - 1);
    this.expiryDateValue.setDate(this.expiryDateValue.getDate() + MockData.ExpirayDays);
    this.maxDate.setDate(this.maxDate.getDate() + 30);

    this.InvoiceAddress = JSON.parse(localStorage.getItem("inoice-address"));
    this.salesRepName = this.route.snapshot.paramMap.get('salesRap');
    this.oppurtunityId = this.route.snapshot.paramMap.get('oppurtunityId');
    this.oppurtunity.oppurtunity = this.route.snapshot.paramMap.get('oppurtunityId');
    this.salesRepFirstletter = this.salesRepName.charAt(0);
    this.SalesRepId = this.route.snapshot.paramMap.get('erpId');
    this.CustomerId = this.route.snapshot.paramMap.get('id');
    this.qoutationId = this.route.snapshot.paramMap.get('qouteId');
    if (this.qoutationId != "0") {
      this.qoutationCreated = true;
      this.qouteId = this.qoutationId;
      this.oppurtunity.qoute = this.qoutationId;
      this.LoadQuote = true;
    }

    this.ProcessQuotation();
  }

  setExpirayDate(e) {
    let dateSplit = this.customerInfo.ExpirtyDate.split("-");
    this.expiryDateValue.setDate(parseInt(dateSplit[2]));
    this.expiryDateValue.setFullYear(parseInt(dateSplit[0]));
    this.expiryDateValue.setMonth(parseInt(dateSplit[1]) - 1);
  }

  changePaymentTerms(e) {
    this.expDateSet = false;
    if (e.target.checked) {
      this.changePaymentTermsandExpiray = true;
      let dateSplit = this.customerInfo.ExpirtyDate.split("-");
      this.expiryDateValue.setDate(parseInt(dateSplit[2]));
      this.expiryDateValue.setFullYear(parseInt(dateSplit[0]));
      this.expiryDateValue.setMonth(parseInt(dateSplit[1]) - 1);
    } else {
      this.changePaymentTermsandExpiray = false;
    }
  }

  ngAfterViewInit() {
    this.cart = this.purchaseCart.cart;
    this.customerInfo = this.CustomerInformationComponent.customerInfo;
    this.expirayDateLoad = this.purchaseCart.expirayDateString;
    this.LoadSaleOrders();
    this.cdr.detectChanges();
  }

  ProcessQuotation() {
    this.behaviorService.getQuoteStatus().subscribe(v => {
      if (v > 0) {
        if (v == 2) {
          this.disablePage = true;
          this.quotationStatusSelected = true;
          this.quotationStatus = 'won';
          this.orderCreated = true;
          this.buttonDisabled = true;
          this.firststep = false;
        } else if (v == 3) {
          this.disablePage = true;
          this.quotationStatusSelected = true;
          this.quotationStatus = 'lost';
          this.buttonDisabled = true;
          this.firststep = false;
        }
      }
    });
  }
  onChangeFollowUpQuote(e,content2){
      let quotationFollowUp = JSON.parse(e);
      if(!quotationFollowUp){
          this.openQuoteModal(content2);
      }else if(quotationFollowUp){
        this.quotationStatus = 'won';
        this.quotationStatusSelected = false;
      }
  }

  rejectQuotation() {
    this.quotationStatus = 'lost';
    this.disablePage = true;
    this.quotationStatusSelected = true;
    this.quotationRejectRequest.QuotationId = this.qoutationId;
    this.http.post(AppConfig.rejectQuote, this.quotationRejectRequest).subscribe((res) => {
      if (res['Success']) {
        this.toastr.success('Quotation has been rejected');
      } else {
        this.toastr.error('Error occured while rejecting this quotation', 'Error', {
          timeOut: 3000
        });
      }
    });
    this.modalService.dismissAll();
  }

  reloadPage() {
    location.reload();
  }

  CreateQoute() {
    if (this.expDateSet) {
      this.setExpirayDate(this.expiryDateValue);
    }
    this.buttonDisabled = true;

    this.contactPersonsInfo = this.CustomerInformationComponent.contactPerson;

    if (this.contactPersonsInfo.ContactPersonId == "") {
      this.toastr.error('No Contact Person Found, Please Add a Contact Person', 'Error', {
        timeOut: 3000
      });
      this.buttonDisabled = false;
      return;
    }

    if (!this.ValidateSaleOrder()) {
      this.toastr.error('Please Save Cart before create Quotation.', 'Error', {
        timeOut: 3000
      });
      this.buttonDisabled = false;
      return;
    }

    if(!this.ValidateOfferType()){
      this.toastr.error('Offer Type/Billing Interval Mismatch. Please select items with same Offer Type/Billing Interval', 'Error', {
        timeOut: 5000
      });
      this.buttonDisabled = false;
      return;
    }

    //Now Date
    let now = new Date();
    let NowDateTime = formatDate(now, 'yyyy-MM-dd', 'en-US', '+0530');
    let channelRef = formatDate(now, 'yyyyMMddThh:mm:ss', 'en-US', '+0530');
    let expirayDateString = formatDate(this.expiryDateValue, 'yyyy-MM-dd', 'en-US', '+0530');
    this.QouteJSON.customerQuotation.ChannelReferenceId = "QUOT" + channelRef; //We need to change this Channel Ref Id in future
    this.QouteJSON.customerQuotation.RequestedDeliveryDateString = NowDateTime;
    this.QouteJSON.customerQuotation.CustomerAccount = this.customerInfo.AccountNumber;
    this.QouteJSON.customerQuotation.ExpiryDateString = expirayDateString;

    this.QouteJSON.customerQuotation.CustomAttributes = [];

    let ShipmentDate = formatDate(now, 'dd-MM-yyyy', 'en-US', '+0530');

    //static
    let customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "ShippingDateRequested";
    customAttributeList.Value = ShipmentDate;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);

    //static
    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "ContactPersonId";
    customAttributeList.Value = this.contactPersonsInfo.ContactPersonId;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);


    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "TMVContractValidFrom";
    customAttributeList.Value = NowDateTime;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);


    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "TMVCommentForQuote";
    customAttributeList.Value = this.commentsForOrder;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);

    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "TMVEditEmailText";
    customAttributeList.Value = this.commentsForEmail;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);

    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "SalesOriginId";
    customAttributeList.Value = "Inbnd Call";
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);

    if (this.oppurtunityId != null && this.oppurtunityId != undefined && this.oppurtunityId != "0") {
      customAttributeList = new CustomAttributesList();
      customAttributeList.Key = "TMVOpportunityId";
      customAttributeList.Value = this.oppurtunityId.toString();
      this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);
    }

    if (this.paymentTerms == undefined) {
      this.paymentTerms = this.customerInfo.TermsOfPayment + 'd';
    }

    customAttributeList = new CustomAttributesList();
    customAttributeList.Key = "TMVPaymentTerms";
    customAttributeList.Value = this.paymentTerms;
    this.QouteJSON.customerQuotation.CustomAttributes.push(customAttributeList);

    this.cart.CartLines.forEach((LineItem, index) => {
      //Calculate Taxes here
      let taxes = new TaxList();
      taxes.Amount = LineItem.LineSummary.Tax;

      let itemsList = new ItemsList();
      itemsList.ItemId = LineItem.ItemId;
      itemsList.AddressRecordId = this.customerInfo.InvoiceAddress.RecordId.toString();

      itemsList.Discount = LineItem.LineSummary.DiscountAmount;

      itemsList.LineNumber = parseInt(LineItem.LineId);
      itemsList.Quantity = LineItem.Quantity;
      itemsList.Taxes.push(taxes);
      itemsList.RequestedDeliveryDateString = NowDateTime;
      itemsList.SourceId = "Quotation Sales";
      itemsList.TMVCONTRACTVALIDFROM = NowDateTime;
      itemsList.TMVCONTRACTCALCULATEFROM = NowDateTime;
      itemsList.TMVCONTRACTVALIDTO = "";
      itemsList.UNIT = LineItem.UnitOfMeasureSymbol;

      itemsList.TMVORIGINALLINEAMOUNT = LineItem.LineSummary.BasePrice.toString();
      itemsList.Price = LineItem.LineSummary.BasePrice;
      itemsList.NetAmount = LineItem.LineSummary.Total;
      itemsList.Discount = LineItem.LineSummary.ManualDiscount + LineItem.LineSummary.PriodicDiscount;



      LineItem.DiscountDetails.forEach((discountLine, discountIndex) => {
        let discountQouteList = new DiscountList();
        discountQouteList.Amount = discountLine.DiscountAmount;
        discountQouteList.DiscountAmount = discountLine.DiscountAmount;
        discountQouteList.DiscountCode = discountLine.DiscountCode;
        discountQouteList.OfferName = discountLine.OfferName;
        discountQouteList.Percentage = discountLine.Percentage;
        discountQouteList.PeriodicDiscountOfferId = discountLine.OfferId;
        discountQouteList.CustomerDiscountType = discountLine.CustomerDiscountTypeValue;
        discountQouteList.DiscountOriginType = discountLine.DiscountLineTypeValue;
        discountQouteList.ManualDiscountType = discountLine.ManualDiscountTypeValue;
        itemsList.Discounts.push(discountQouteList);
      });

      let ShipmentDate = formatDate(LineItem.validFrom, 'dd-MM-yyyy', 'en-US', '+0530');

      customAttributeList = new CustomAttributesList();
      customAttributeList.Key = "TMVContractValidFrom";
      customAttributeList.Value = ShipmentDate;
      itemsList.CustomAttributes.push(customAttributeList);
      this.QouteJSON.customerQuotation.Items.push(itemsList);

      if (LineItem.adOnDetails.length > 0) {
        LineItem.adOnDetails.forEach(ad => {
          if (ad.IsAddonSelected) {
            let taxes = new TaxList();
            taxes.Amount = ad.LineSummary.Tax;

            let productLine = new ItemsList();
            productLine.ItemId = ad.ItemId;
            productLine.AddressRecordId = this.customerInfo.InvoiceAddress.RecordId.toString();

            // itemsList.Discount = LineItem.discountAmount;
            productLine.LineNumber = parseInt(ad.LineId);
            // productLine.NetAmount = ad.price;
            // productLine.Price = ad.price;
            productLine.Quantity = ad.Quantity;
            productLine.Taxes.push(taxes);
            productLine.RequestedDeliveryDateString = NowDateTime;
            productLine.SourceId = "Quotation Sales";
            productLine.TMVCONTRACTVALIDFROM = NowDateTime;
            productLine.TMVCONTRACTCALCULATEFROM = NowDateTime;
            productLine.TMVCONTRACTVALIDTO = "";

            productLine.UNIT = ad.UnitOfMeasureSymbol;
            productLine.TMVORIGINALLINEAMOUNT = ad.LineSummary.Price.toString();
            productLine.Price = ad.LineSummary.BasePrice;
            productLine.NetAmount = ad.LineSummary.Total;
            productLine.Discount = ad.LineSummary.ManualDiscount + LineItem.AdonLineSummary.PriodicDiscount;

            ad.DiscountDetails.forEach((discountLine, discountIndex) => {
              let discountQouteList = new DiscountList();
              discountQouteList.Amount = discountLine.DiscountAmount;
              discountQouteList.DiscountAmount = discountLine.DiscountAmount;
              discountQouteList.DiscountCode = discountLine.DiscountCode;
              discountQouteList.OfferName = discountLine.OfferName;
              discountQouteList.Percentage = discountLine.Percentage;
              discountQouteList.PeriodicDiscountOfferId = discountLine.OfferId;
              discountQouteList.CustomerDiscountType = discountLine.CustomerDiscountTypeValue;
              discountQouteList.DiscountOriginType = discountLine.DiscountLineTypeValue;
              discountQouteList.ManualDiscountType = discountLine.ManualDiscountTypeValue;
              productLine.Discounts.push(discountQouteList);
            });


            let ShipmentDateAdon = formatDate(ad.validFrom, 'dd-MM-yyyy', 'en-US', '+0530');
            let customAttributeListAdon = new CustomAttributesList();
            customAttributeListAdon.Key = "TMVContractValidFrom";
            customAttributeListAdon.Value = ShipmentDateAdon;
            productLine.CustomAttributes.push(customAttributeListAdon);

            this.QouteJSON.customerQuotation.Items.push(productLine);
          }
        });
      }

    });

    //Sale Order Service 
    this.http.post(AppConfig.createQoutation, this.QouteJSON).subscribe((res) => {
      if (res['Success']) {
        this.qoutationCreated = true;
        this.qouteId = res['QuotationId'];
        this.qoutationId = res['QuotationId'];
        this.oppurtunity.qoute = res['QuotationId'];

        this.QuoteSave(this.qouteId, this.cart, this.customerInfo, this.SalesRepId);

        this.toastr.success("Quotation Created Successfully");
        this.buttonDisabled = false;
        this.firststep = false;
      } else {
        this.toastr.error('Could not Create Quotation', 'Error', {
          timeOut: 3000
        });
      }
    });
  }

  createSalesOrder() {
    this.buttonDisabled = true;
    if (!this.ValidateSaleOrder()) {
      this.toastr.error('Cannot Create Sale Order on Empty Cart', 'Error', {
        timeOut: 3000
      });
      this.buttonDisabled = false;
      return;
    }
    let QuotationInfo = {};
    QuotationInfo['commentsForOrder'] = this.commentsForOrder;
    QuotationInfo['commentsForEmail'] = this.commentsForEmail;
    QuotationInfo['quotationId'] = this.qoutationId;
    QuotationInfo['SalesRepId'] = this.SalesRepId;

    this.saleOrderService.CreateSaleOrderTransaction(this.cart, this.customerInfo, QuotationInfo).subscribe((res) => {
      if (res['status']) {
        this.orderCreated = true;
        this.quotationStatusSelected = true;
        this.saleOrderNumber = res['salesOrderTransactionId'];
        localStorage.setItem("lastSaleOrder", res['salesOrderTransactionId']);

        this.disablePage = true;
        this.quotationStatus = 'won';
        this.buttonDisabled = true;
        this.firststep = false;

        this.toastr.success("Order Placed Successfully");
      } else {
        this.toastr.error('Could not Create Sales Order', 'Error', {
          timeOut: 3000
        });
      }
    });
  }

  private ValidateOfferType(): boolean{
    let isValid: boolean = true;
    let billingInterval = this.cart.CartLines[0].billingInterval;
    this.cart.CartLines.forEach(cl => {
        if(billingInterval != cl.billingInterval){
          isValid = false;
        }
    }); 
    return isValid;
  }
  
  private ValidateSaleOrder(): boolean {
    let isValid: boolean = true;
    if (this.cart.CartLines.length == 0) {
      isValid = false;
    }

    this.cart.CartLines.forEach(cl => {
      if (!cl.IsLineItemCreated) {
        isValid = false;
      }
      else if (cl.IsLineItemChange) {
        isValid = false;
      }

      // process adon
      cl.adOnDetails.forEach(ad => {
        if (ad.IsAddonSelected) {
          if (!ad.IsLineItemCreated) {
            isValid = false;
          }
          else if (ad.IsLineItemChange) {
            isValid = false;
          }
        }
      });
    });

    return isValid;
  }


  LoadSaleOrders() {
    if (this.SalesRepId == undefined || Number(this.SalesRepId) <= 0) {
      return;
    }

    let url = AppConfig.SalesOrderGetAll + `?userId=${this.SalesRepId}&pageSize=100&pageNumber=0`;
    this.http.get(url).subscribe(res => {

      var source: any =
      {
        localdata: res,
        datafields:
          [
            { name: 'Id', type: 'string' },
            { name: 'Type', type: 'string' },
            { name: 'CustomerName', type: 'string' },
            { name: 'CreatedOn', type: 'string' }

          ],
        datatype: 'array'
      };

      this.dataAdapter = new jqx.dataAdapter(source);
    });
  }

  getWidth(): any {
    if (document.body.offsetWidth < 850) {
      return '90%';
    }

    return 1000;
  }

  handlekeyboardnavigation = (event: any): boolean => {
    let key = event.charCode ? event.charCode : event.keyCode ? event.keyCode : 0;
    if (key == 13) {
      return true;
    }
    else if (key == 27) {
      return true;
    }
    return false;
  };

  openModal(content1) {
    this.modalService.open(content1, { size: 'lg' });
  }

  openQuoteModal(content2) {
    this.modalService.open(content2, { size: 'sm' });
  }

  //#region Sale Order
  QuoteSave(saleOrderId: string, cart: Cart, customerInfo: Customer, userId?: string) {
    let order: any = {};
    order.Id = saleOrderId;
    order.CustomerId = customerInfo.AccountNumber;
    order.CustomerName = customerInfo.Name;
    order.CreatedOn = new Date();
    order.UserId = userId;
    order.Content = JSON.stringify(this.saleOrderRequest);
    order.CartContent = JSON.stringify(cart);
    order.Type = 2;

    this.http.post(AppConfig.SalesOrderSave, order).subscribe(res => {

    });
  }
  //#endregion

}
