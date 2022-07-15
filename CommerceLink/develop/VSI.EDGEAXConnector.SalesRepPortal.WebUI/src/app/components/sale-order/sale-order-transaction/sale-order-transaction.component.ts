import { Component, OnInit, AfterViewInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SaleOrderService } from '../../../_shared/services/sale-order-service';
import { ActivatedRoute } from '@angular/router';
import { Order, SaleOrderRequest, PorductLineItem, PriceAdjustment } from '../../../_shared/models/sale-order';
import { PurchaseCartComponent } from '../cart/purchase-cart/purchase-cart.component';
import { Cart } from 'src/app/_shared/models/cart-items';
import { CustomerInformationComponent } from '../customer/customer-information/customer-information.component';
import { Customer } from 'src/app/_shared/models/customer';
import { http } from 'src/app/_shared/services/http';
import { AppConfig } from 'src/app/_shared/constants/app-config';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { jqxGridComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid';
import { Dropdown } from 'src/app/_shared/models/common';
import { MockData } from 'src/app/_shared/constants/mock-data';

@Component({
  selector: 'app-sale-order-transaction',
  templateUrl: './sale-order-transaction.component.html',
  styleUrls: ['./sale-order-transaction.component.scss']
})
export class SaleOrderTransactionComponent implements OnInit, AfterViewInit {

  @ViewChild(PurchaseCartComponent) purchaseCartComponent;
  @ViewChild(CustomerInformationComponent) CustomerInformationComponent;
  @ViewChild('GridMyOrders') GridMyOrders: jqxGridComponent;

  public saleOrderRequest: SaleOrderRequest = new SaleOrderRequest();
  public OrderJSON: Order = new Order();
  public InvoiceAddress: any = "";
  public customerInfo: Customer;
  public productLineItems: Array<PorductLineItem> = [];
  saleOrderStr: any = {};
  saleOrderNumber: any = "";
  orderCreated: boolean = false;

  salesRepName: any = '';
  salesRepFirstletter: any = '';
  SalesRepId: string;
  CustomerId: string;

  commentsForOrder: string;
  commentsForEmail: string;

  public customerCollapsed = true;
  public cartCollapsed = false;
  public SaleOrderCollapsed = true;
  public cart: Cart;
  public buttonDisabled: boolean = false;

  dataAdapter: any;
  columns: any[] =
    [
      { text: 'Reference Number', columntype: 'textbox', datafield: 'Id', width: '40%' },
      { text: 'Type', columntype: 'textbox', datafield: 'Type', width: '15%' },
      { text: 'Customer Name', columntype: 'textbox', datafield: 'CustomerName', width: '20%' },
      { text: 'Date', columntype: 'textbox', datafield: 'CreatedOn', width: '25%' }
    ];

  ddlPaymentTerms: Dropdown[];
  paymentTerms: string;

  constructor(private toastr: ToastrService,
    private saleOrderService: SaleOrderService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private modalService: NgbModal,
    private httpService: http) { }

  ngOnInit() {
    this.salesRepName = this.route.snapshot.paramMap.get('salesRap');
    this.salesRepFirstletter = this.salesRepName.charAt(0);
    this.SalesRepId = this.route.snapshot.paramMap.get('erpId');
    this.CustomerId = this.route.snapshot.paramMap.get('id');
    
    this.ddlPaymentTerms = MockData.PaymentTerms;
  }

  ngAfterViewInit() {
    this.cart = this.purchaseCartComponent.cart;
    this.customerInfo = this.CustomerInformationComponent.customerInfo;
    this.LoadSaleOrders();
    this.cdr.detectChanges();
  }

  createSalesOrder() {
    this.buttonDisabled = true;
    if (!this.ValidateSaleOrder()) {
      this.toastr.error('Invalid or Empty cart, Cannot Create Sale Order', 'Error', {
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
    
    let OrderInfo = {};
    OrderInfo['commentsForOrder'] = this.commentsForOrder;
    OrderInfo['commentsForEmail'] = this.commentsForEmail;
    OrderInfo['SalesRepId'] = this.SalesRepId;

    this.saleOrderService.CreateSaleOrderTransaction(this.cart, this.customerInfo, OrderInfo, this.SalesRepId).subscribe((res) => {
      if (res['status']) {
        this.orderCreated = true;
        this.saleOrderNumber = res['salesOrderTransactionId'];
        localStorage.setItem("lastSaleOrder", res['salesOrderTransactionId']);
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
      if (!cl.IsLineItemCreated || !cl.IsValidQuantity) {
        isValid = false;
      }
      else if (cl.IsLineItemChange || !cl.IsValidQuantity) {
        isValid = false;
      }

      // process adon
      cl.adOnDetails.forEach(ad => {
        if (ad.IsAddonSelected) {
          if (!ad.IsLineItemCreated || !cl.IsValidQuantity) {
            isValid = false;
          }
          else if (ad.IsLineItemChange || !cl.IsValidQuantity) {
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
    this.httpService.get(url).subscribe(res => {

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

  openModal(content) {
    this.modalService.open(content, { size: 'lg' });
  }
}
