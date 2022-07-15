import { Component, OnInit, AfterViewInit, HostListener, ViewChild, ElementRef, Input, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Renderer } from '@angular/core';
import { http } from '../../../../_shared/services/http';
import { AppConfig } from '../../../../_shared/constants/app-config';
import { Cart, Discount, CartItem, Product, Variations, AddOns } from '../../../../_shared/models/cart-items';
import { Dropdown } from 'src/app/_shared/models/common';
import { MockData } from '../../../../_shared/constants/mock-data';
import { Utilities } from '../../../../_shared/constants/utilities';
import { CartService } from 'src/app/_shared/services/cart-service';
import { QouteService } from 'src/app/_shared/services/qoute-service';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import * as uuid from 'uuid';
import { CartLinesCreateRequest } from 'src/app/_shared/_api-models/cartLinesCreateRequest';
import { jqxGridComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid';
import { jqxDropDownButtonComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxdropdownbutton';
import { IfStmt } from '@angular/compiler';
import { GetQouteRequest } from 'src/app/_shared/_api-models/qouteGetRequest';
import { TestCart, GetQouteResponse } from '../../../../_shared/_api-models/qouteGetResponse';
import { formatDate } from '@angular/common';
import { Customer } from 'src/app/_shared/models/customer';
import { environment } from '../../../../../environments/environment';
import { AppSetting } from 'src/app/_shared/constants/app-setting';
import { BehaviourSubjectService } from '../../../../_shared/services/behaviour-subject-service'
import { ItemsList } from 'src/app/_shared/models/qoute';

@Component({
  selector: 'app-purchase-cart',
  templateUrl: './purchase-cart.component.html',
  styleUrls: ['./purchase-cart.component.scss']
})
export class PurchaseCartComponent implements OnInit {
  @ViewChild('ProductGrid') ProductGrid: jqxGridComponent;
  @ViewChild('myDropDownButton') myDropDownButton: jqxDropDownButtonComponent;
  @ViewChild('selectedRowIndex') selectedRowIndex: ElementRef;
  @ViewChild('unselectedRowIndex') unselectedRowIndex: ElementRef;
  // @ViewChild('SelectProductModal') selectPorductModal: ElementRef;
  @ViewChild('SelectProductModal') modalRef: NgbModalRef;


  @Input() customerInfo: Customer;
  @Input() IsQuote: boolean;

  data = [];
  selectedRowsIndexes = [];
  source: any =
    {
      localdata: this.data,
      datafields:
        [
          // { name: 'IsSelected', type: 'bool'},
          { name: 'MasterItemId', type: 'string' },
          // { name: 'ItemId', type: 'string' },
          { name: 'name', type: 'string' },
          { name: 'billingInterval', type: 'string' },
          { name: 'offerType', type: 'string' },
          { name: 'ItemId', type: 'string', hidden: true },
          { name: 'LineId', type: 'string', hidden: true }
        ],
      datatype: 'array'
    };
  ticks: any;
  getWidth(): any {
    if (document.body.offsetWidth < 850) {
      return '90%';
    }

    return 850;
  }
  dataAdapter: any = new jqx.dataAdapter(this.source);
  columns: any[] =
    [
      // { text: '', columntype: 'checkbox', datafield: 'IsSelected' },
      { text: 'Item Number', columntype: 'textbox', datafield: 'MasterItemId', width: '178' },
      // { text: 'Item Number', columntype: 'textbox', datafield: 'ItemId', width: '210' },
      { text: 'Product Name', columntype: 'textbox', datafield: 'name', width: '220' },
      { text: 'Billing Interval', columntype: 'textbox', datafield: 'billingInterval', width: '110' },
      { text: 'Offer Type', columntype: 'textbox', datafield: 'offerType', width: '80' },
      { text: 'Item Number', columntype: 'textbox', datafield: 'ItemId', width: '210', hidden: true },
      { text: 'LineId', columntype: 'textbox', datafield: 'LineId', hidden: true }
    ];

  popularProducts: Array<Dropdown> = MockData.POPULARPRODUCTS;
  ddlDiscountReason: Array<Dropdown> = MockData.DiscountReason;
  ddlPopularProducts: Array<Dropdown> = [];
  discountMethods: Array<Dropdown> = [];
  discountOrigins: Array<Dropdown> = [];
  formattedDate: any = '';
  selectedRows: any = [];
  dateUtilities: any = "";
  closeResult: string;

  editRowId: any = "";
  purchaseType: any = '';
  productsObject: any = '';
  testing: any = '';
  priceBook: any = '';
  Affiliations: any = '';
  refinedSources: any = '';
  public cartItems: CartItem;
  public cart: Cart = new Cart();
  public discount: Discount;
  public product: Product;
  public variations: Variations;
  public adOns: AddOns;
  public AllPorductItems: Array<CartItem> = [];
  public ddlLineItem: Array<Dropdown> = [];
  public selectedCartLineId: string;
  public qoutationId: any = "";
  public isQoute: boolean = true;
  public IsEnableDelete: boolean = false;
  dropdownList = [];
  dropdownSettings = {};
  CouponCode: string;

  counter: number;
  timerRef;
  running: boolean = false;
  isLoadingSummary: boolean = false;
  isLoadingProducts: boolean = false;
  quotationStatus: number = 0;
  expirayDateString: string;


  constructor(
    private http: http,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private render: Renderer,
    private cartService: CartService,
    private modalService: NgbModal,
    private qouteService: QouteService,
    private cdr: ChangeDetectorRef,
    private behaviorService: BehaviourSubjectService
  ) {
    this.cart = new Cart();
    this.dateUtilities = new Utilities();
  }

  ngOnInit() {
    if (JSON.parse(localStorage.getItem('selected')) != null) {
      this.selectedRowsIndexes = JSON.parse(localStorage.getItem('selected'));
    }

    this.discountMethods = MockData.DISCOUNTMETHODS;
    this.discountOrigins = MockData.DISCOUNTORIGINS;
    this.ddlDiscountReason = MockData.DiscountReason;
    this.editRowId = -1;
    this.dropdownSettings = AppConfig.dropdownSettings;
    this.LoadAffiliations();
    this.LoadPriceBook();

    this.behaviorService.getBehaviorView().subscribe(v => {
      if (Object.keys(v).length) {
        this.productsObject = v;
        this.FillCartItems();
        this.LoadCart();
      }

    })
  }



  onModelChange(e) {
    // this.isLoadingSummary = true;
    clearInterval(this.timerRef);
    this.counter = 0;
    this.running = true;

    if (this.running) {
      const startTime = Date.now() - (this.counter || 0);
      this.timerRef = setInterval(() => {
        this.counter = Date.now() - startTime;
        if (this.counter > 3000 && this.counter < 3100) {
          this.running = false;
          this.counter = 0;
          clearInterval(this.timerRef);
          //this.CreateCartLine();
        }
      });
    } else {
      clearInterval(this.timerRef);
    }
  }
  AddAdOn(adons) {
    adons.IsAddonSelected = true;
    this.onModelChange("");
  }

  SelectPopularProductsFromGrid(e) {
    if (this.popularProducts.length < 6) {
      let isAdded = this.popularProducts.find(cl => cl.Value == e.args.row.ItemId);

      if (isAdded == undefined) {
        let newDDL = new Dropdown();
        newDDL.Text = e.args.row.name;
        newDDL.Value = e.args.row.ItemId;
        this.selectedRowsIndexes.push(e.args.rowindex);

        this.customerInfo.SelectedRowsIndexes.push(e.args.rowindex);
        localStorage.setItem('selected-' + this.customerInfo.CountryCode, JSON.stringify(this.customerInfo.SelectedRowsIndexes));
        this.popularProducts.push(newDDL);

        this.customerInfo.CustomerPopularProducts.push(newDDL);
        localStorage.setItem('popular-products-' + this.customerInfo.CountryCode, JSON.stringify(this.customerInfo.CustomerPopularProducts));
      }
    }
  }

  UnSelectPopularProductsFromGrid(e) {
    this.customerInfo.SelectedRowsIndexes.forEach((item, index) => {
      if (e.args.rowindex == item) {
        this.customerInfo.SelectedRowsIndexes.splice(index, 1);
      }
    });
    localStorage.setItem('selected-' + this.customerInfo.CountryCode, JSON.stringify(this.customerInfo.SelectedRowsIndexes));


    let isAdded = this.popularProducts.find(cl => cl.Value == e.args.row.ItemId);
    this.customerInfo.CustomerPopularProducts.forEach((item, index) => {
      if (item.Value == e.args.row.ItemId) {
        this.customerInfo.CustomerPopularProducts.splice(index, 1);
      }
    });
    localStorage.setItem('popular-products-' + this.customerInfo.CountryCode, JSON.stringify(this.customerInfo.CustomerPopularProducts));

  }

  SelectProductForCart(e) {
    if (this.AllPorductItems.length == 0) {
      return;
    }
    var cartLine = this.cart.CartLines.find(cl => cl.LineId == this.selectedCartLineId);
    let lineItem = this.AllPorductItems.find(cl => cl.ItemId == e.args.row.ItemId);

    cartLine.ItemId = lineItem.ItemId;
    cartLine.MasterItemId = lineItem.MasterItemId;
    this.onItemChange(cartLine);
    this.modalService.dismissAll('Cross click')
  }

  openModal(content) {
    // if (this.data.length == 0) {
    //   this.productsObject = this.customerInfo.CustomerAllProducts;
    //   this.FillCartItems();
    // }
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  selectProductModal(SelectProductModal, LineId) {
    this.selectedCartLineId = LineId;
    this.modalService.open(SelectProductModal, { ariaLabelledBy: 'modal-basic-title1 ' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  LoadCart() {
    this.qoutationId = this.route.snapshot.paramMap.get('qouteId');
    if (this.qoutationId != "0") {
      let now = new Date();
      let NowDateTime = formatDate(now, 'yyyy-MM-dd', 'en-US', '+0530');
      this.isQoute = false;
      let QouteGetRequest = new GetQouteRequest();
      QouteGetRequest.QuotationId = this.qoutationId;

      this.http.post(AppConfig.getQoute, QouteGetRequest).subscribe((res) => {
        if (res['Success']) {
          res['Quotations'].forEach((value, key) => {
            this.quotationStatus = value.Status;
            this.expirayDateString = value.ExpiryDateString;
            this.behaviorService.setQuoteStatus(value.Status);
            this.cart.CartSummary.Price = value.TotalPrice;
            this.cart.CartSummary.Tax = value.TotalTax;
            this.cart.CartSummary.Total = parseInt(value.TotalPrice) + parseFloat(value.TotalTax);
            let parentItems = value.Items.filter(cl => cl.TMVPARENT == "0");
            parentItems.forEach(lineItem => {
              let cl = this.MapLineItemToCartLine(lineItem, true);

              let adons = value.Items.filter(adonLineItem => adonLineItem.TMVPARENT == lineItem.RecId);
              adons.forEach(adon => {
                let adonCL = this.MapLineItemToCartLine(adon, false);
                cl.adOnDetails.push(adonCL);
              });
              this.cart.CartLines.push(cl);
              this.cart.CartSummary.Tax = value.TotalTax;
            });
          });
          
          this.UpdateCartSummary();
        }
      })
    }

  }

  private MapLineItemToCartLine(lineItem: any, isParent: boolean): CartItem {
    let cl = new CartItem();
    cl.ItemId = lineItem.ITEMID;
    cl.name = lineItem.NAME;
    cl.billingInterval = lineItem.ColorId;
    cl.offerType = lineItem.StyleId;
    cl.Quantity = lineItem.Quantity;
    cl.UnitOfMeasureSymbol = lineItem.SALESUNIT;
    cl.MasterItemId = lineItem.ITEMID.split('_')[0];
    cl.validFrom = lineItem.TMVCONTRACTVALIDFROM;
    cl.validTo = lineItem.TMVCONTRACTVALIDTO;
    cl.CrossSellSku = lineItem.CrossSellSku;

    cl.RecId = lineItem.RecId;
    cl.IsLineItemCreated = true;
    cl.IsAddonSelected = true;
    cl.parentProduct = isParent;

    if(isParent){
      cl.LineSummary.BasePrice = parseInt(lineItem.TotalPrice);
      cl.LineSummary.ExtendedPrice = parseInt(lineItem.TotalPrice);
      cl.LineSummary.Price = parseInt(lineItem.TotalPrice);
      cl.LineSummary.Total = parseInt(lineItem.TotalPrice);
      cl.LineSummary.ManualDiscount = lineItem.Discount * lineItem.Quantity;
      cl.LineSummary.NetAmountWithoutTax = parseInt(lineItem.TotalPrice);
    }else{
      cl.LineSummary.BasePrice = parseInt(lineItem.TotalPrice);
      cl.LineSummary.ExtendedPrice = parseInt(lineItem.TotalPrice);
      cl.LineSummary.Price = parseInt(lineItem.TotalPrice);
      cl.LineSummary.Total = lineItem.NetAmount;
      cl.LineSummary.ManualDiscount = lineItem.Discount * lineItem.Quantity;
      cl.LineSummary.NetAmountWithoutTax = parseInt(lineItem.TotalPrice);

      cl.AdonLineSummary.BasePrice = parseInt(lineItem.TotalPrice);
      cl.AdonLineSummary.ExtendedPrice = parseInt(lineItem.TotalPrice);
      cl.AdonLineSummary.Price = parseInt(lineItem.TotalPrice);
      cl.AdonLineSummary.Total = lineItem.NetAmount;
      cl.AdonLineSummary.ManualDiscount = lineItem.Discount * lineItem.Quantity;
      cl.AdonLineSummary.NetAmountWithoutTax = parseInt(lineItem.TotalPrice);
    }
    lineItem.Discounts.length > 0 && lineItem.Discounts.forEach(discountLine => {
            var discount = new Discount();
            discount.discountOrigin = discountLine.DiscountOriginType;
            discount.discountMethodPer = false;
            discount.discountMethodTarget = false;
            discount.discountPercentage = discountLine.Percentage;
            discount.Interval = 1;
            discount.DiscountCode = "01";
            discount.DiscountLineTypeValue = 3;
            discount.ManualDiscountTypeValue = discountLine.ManualDiscountType;
            discount.CustomerDiscountTypeValue = discountLine.CustomerDiscountType;
            discount.PeriodicDiscountTypeValue = discountLine.PeriodicDiscountType;
            discount.Method = 2;
            discount.Amount = discountLine.Amount;
            discount.MonthlyPrice = lineItem.TotalPrice;
            discount.NextPrice = lineItem.TMVORIGINALLINEAMOUNT;
            discount.DiscountAmount = discountLine.Amount;
            cl.DiscountDetails.push(discount);
    });
    return cl;
  }

  LoadPriceBook() {
    this.priceBook = localStorage.getItem('priceBook');
    if (this.priceBook == null || this.priceBook == 'undefined') {
      this.http.get(AppConfig.getPriceBook + "?type=pricebook").subscribe((priceBookResponse) => {
        this.priceBook = priceBookResponse;
        localStorage.setItem('priceBook', priceBookResponse);
      });
    }
  }

  onItemChange(cartLineItem: CartItem) {
    cartLineItem.adOnDetails = [];
    let lineItem = this.AllPorductItems.find(cl => cl.ItemId == cartLineItem.ItemId);
    cartLineItem.name = lineItem.name;
    cartLineItem.ItemId = lineItem.ItemId;
    cartLineItem.MasterItemId = lineItem.MasterItemId;
    cartLineItem.Quantity = cartLineItem.Quantity;
    cartLineItem.billingInterval = lineItem.billingInterval;
    cartLineItem.offerType = lineItem.offerType;
    cartLineItem.validFrom = lineItem.validFrom;
    cartLineItem.validTo = lineItem.validTo;
    cartLineItem.currency = lineItem.currency;
    cartLineItem.license = lineItem.license;
    cartLineItem.price = lineItem.price;
    cartLineItem.UnitOfMeasureSymbol = lineItem.UnitOfMeasureSymbol;
    cartLineItem.QuantityMin = lineItem.QuantityMin;
    cartLineItem.QuantityMax = lineItem.QuantityMax;
    cartLineItem.IsChannel = lineItem.IsChannel;

    if (lineItem.CrossSellSku != "") {
      lineItem.CrossSellSku.split(",").forEach(itemId => {
        let test = this.AllPorductItems.filter(i => i.ItemId == itemId);
        let adOnLineItem = this.AllPorductItems.find(cl => cl.ItemId == itemId);
        if (adOnLineItem != undefined) {

          let adOnLineItemCopy: CartItem = new CartItem();
          Object.keys(adOnLineItem).map(function (key) {
            adOnLineItemCopy[key] = adOnLineItem[key];
          });

          adOnLineItemCopy.LineId = uuid.v4();
          adOnLineItemCopy.IsAddonSelected = false;

          if (adOnLineItemCopy.Quantity <= 0) {
            adOnLineItemCopy.Quantity = 1;
          }
          cartLineItem.adOnDetails.push(adOnLineItemCopy);
        }
      });
    }

    cartLineItem.IsLineItemChange = true;
    this.editRowId = -1;
    this.onModelChange("");
    this.UpdateCartSummary();

    //temporary data overwrite for MDS quantity Validation
    this.SetMDSStaticData(cartLineItem);

    this.CalculateMDSQuantity(cartLineItem);
  }

  OnItemQuantityChange(cartLineItem: CartItem) {
    if (cartLineItem.Quantity >= cartLineItem.QuantityMin && cartLineItem.Quantity <= cartLineItem.QuantityMax) {
      cartLineItem.IsValidQuantity = true;
    }
    else {
      cartLineItem.IsValidQuantity = false;
      return;
    }

    cartLineItem.IsLineItemChange = true;
    this.onModelChange("");
  }

  CalculateMDSQuantity(product: CartItem) {
    let adChannel = product.adOnDetails.find(ad => ad.IsChannel);

    if (adChannel != undefined) {
      product.IsLineItemChange = true;

      var productChannelCount = product.ConcurrentUsers * product.Quantity;
      var adonChannelCount = 0;

      product.adOnDetails.forEach(ad => {
        if (ad.IsAddonSelected && !ad.IsChannel) {
          ad.IsLineItemChange = true;
          adonChannelCount += ad.ConcurrentUsers * ad.Quantity;
        }
      });
      adChannel.Quantity = productChannelCount + adonChannelCount;
    }
  }

  addNewLineItem(sku?: string) {

    // if (this.customerInfo == undefined || this.customerInfo.CustomerAllProducts == "") {
    //   return;
    // }

    // if (this.data.length == 0 && this.customerInfo != undefined) {
    //   this.productsObject = this.customerInfo.CustomerAllProducts;
    //   this.FillCartItems();
    // }
    if (sku != undefined) {
      var cartLine = this.cart.CartLines.find(cl => cl.ItemId == sku);
      if (cartLine != undefined) {

        cartLine.Quantity++;
        if (Number(cartLine.QuantityMax) != 0 && Number(cartLine.Quantity) >= cartLine.QuantityMax) {
          cartLine.IsValidQuantity = false;
          return;
        }

        cartLine.IsValidQuantity = true;
        cartLine.IsLineItemChange = true;
        return;
      }
    }

    let item = new CartItem();
    item.LineId = uuid.v4();
    item.ItemId = 'Please Select';
    item.name = "Please Select";
    item.Quantity = 0;
    item.billingInterval = "---";
    item.offerType = "---";
    item.validFrom = "";
    item.validTo = "";
    item.currency = "---";
    item.tax = 0;
    item.price = 0;
    item.basePrice = 0;
    item.Quantity = 1;
    item.UnitOfMeasureSymbol = 'pcs';
    item.EntryMethodTypeValue = 3;
    item.CommissionSalesGroup = null;
    item.CatalogId = 0;

    item.parentProduct = true;
    this.cart.CartLines.push(item);

    if (sku != undefined) {
      item.ItemId = sku;
      this.onItemChange(item);
    }
  }

  toggleEdit(id) {
    this.editRowId = id;
  }

  //Add Discount Line to the product line item
  addDiscountRow(cartItem: CartItem, sku): void {
    // if item is not valid, than discount will not added.
    if (!cartItem.ItemId.includes("_")) {
      return;
    }

    let disc = cartItem.DiscountDetails.find(d => !d.periodicDiscount);
    // Only one discount can applied.
    if (disc != undefined) {
      return;
    }

    let discount = new Discount();
    discount.discountOrigin = "---";
    // discount.reasonCode = sku;
    discount.discountMethodPer = true;
    discount.discountMethodTarget = false;
    discount.discountPercentage = 0;
    discount.MonthlyPrice = 0;
    discount.Interval = 1;
    discount.NextPrice = 0;
    discount.DiscountCode = "01";
    discount.Method = 1;
    discount.DiscountLineTypeValue = 3;
    discount.ManualDiscountTypeValue = 1;
    discount.CustomerDiscountTypeValue = 0;
    discount.PeriodicDiscountTypeValue = 0;

    cartItem.DiscountDetails.push(discount);
  }

  OnManualDiscountChange(lineItem: CartItem, disc: Discount) {
    if (disc.Amount == undefined || disc.Amount <= 0) {
      lineItem.LineSummary.ManualDiscount = 0;
      lineItem.ManualDiscountAmount = 0;
      disc.NextPrice = disc.MonthlyPrice;
      disc.Amount = 0;
      disc.DiscountAmount = 0;
      this.UpdateCartSummary();
      // this.cdr.detectChanges();
      return;
    }

    let amount = disc.Amount.toString().replace(/^[0]+/g, "");
    if (amount !== "" && Number(amount) != NaN) {
      disc.Amount = parseFloat(amount);
    }

    if (Number(disc.Amount) == NaN) {
      this.toastr.error('Invalid Discount Amount', 'Error', { timeOut: 3000 });
      lineItem.LineSummary.ManualDiscount = 0;
      lineItem.ManualDiscountAmount = 0;
      disc.NextPrice = disc.MonthlyPrice;
      disc.Amount = 0;
      disc.DiscountAmount = 0;
      this.UpdateCartSummary();
      return;
    }

    if (disc.Method == 1) { //Percentage
      if (disc.Amount > AppSetting.DiscountThreshold) {
        this.toastr.error('Discount must be less than ' + AppSetting.DiscountThreshold + ' percent.', 'Error', { timeOut: 3000 });
        lineItem.LineSummary.ManualDiscount = 0;
        lineItem.ManualDiscountAmount = 0;
        disc.NextPrice = disc.MonthlyPrice;
        disc.Amount = 0;
        disc.DiscountAmount = 0;
        this.UpdateCartSummary();
        return;
      }

      disc.ManualDiscountTypeValue = 2;
      disc.Percentage = disc.Amount;
      disc.MonthlyPrice = lineItem.LineSummary.Total;

      let discountAmount = Number(((lineItem.LineSummary.Total * disc.Amount) / 100).toFixed(2));
      disc.NextPrice = Number((lineItem.LineSummary.Total - discountAmount).toFixed(2));
      disc.DiscountAmount = discountAmount;
    }
    else if (disc.Method == 2) { //Cash Amount
      if (lineItem.LineSummary.Total == 0) {
        return;
      }
      let checkAmount = (disc.Amount / lineItem.LineSummary.Total) * 100;
      if (checkAmount > AppSetting.DiscountThreshold) {
        this.toastr.error('Amount must be less than ' + AppSetting.DiscountThreshold + ' percent.', 'Error', { timeOut: 3000 });
        lineItem.LineSummary.ManualDiscount = 0;
        lineItem.ManualDiscountAmount = 0;
        disc.NextPrice = disc.MonthlyPrice;
        disc.Amount = 0;
        disc.DiscountAmount = 0;
        this.UpdateCartSummary();
        return;
      }

      disc.NextPrice = Number((lineItem.LineSummary.Total - disc.Amount).toFixed(2));
      disc.MonthlyPrice = Number((lineItem.LineSummary.Total).toFixed(2));
      disc.ManualDiscountTypeValue = 1;
      disc.Percentage = checkAmount;
      disc.DiscountAmount = disc.Amount;
    }
    else if (disc.Method == 3) { //Target Amount
      if (lineItem.LineSummary.Total == 0) {
        return;
      }

      let amountDiff = lineItem.LineSummary.Total - disc.Amount;
      let checkAmount = (amountDiff / lineItem.LineSummary.Total) * 100;

      if (amountDiff < 0 || checkAmount > AppSetting.DiscountThreshold) {
        this.toastr.error('Target amount was invalid so set to origional product price.', 'Error', { timeOut: 3000 });
        lineItem.LineSummary.ManualDiscount = 0;
        lineItem.ManualDiscountAmount = 0;
        disc.NextPrice = lineItem.LineSummary.Total;
        disc.MonthlyPrice = lineItem.LineSummary.Total;
        disc.Amount = lineItem.LineSummary.Total;
        disc.DiscountAmount = 0;
        this.UpdateCartSummary();
        // this.cdr.markForCheck();
        return;
      }

      disc.NextPrice = disc.Amount;
      disc.MonthlyPrice = lineItem.LineSummary.Total;
      disc.ManualDiscountTypeValue = 3;
      disc.Percentage = checkAmount;
      disc.DiscountAmount = lineItem.LineSummary.Total - disc.Amount;
    }

    this.UpdateCartSummary();
    // this.cdr.detectChanges();
  }

  removeAdon(adonCartItem: CartItem) {
    let linesToRemove: CartItem[] = [];
    this.cart.CartLines.forEach((cl, index) => {
      cl.adOnDetails.forEach(adon => {
        if (adonCartItem.LineId == adon.LineId && adon.IsLineItemCreated) {
          linesToRemove.push(adon);
        }
        else if (adonCartItem.LineId == adon.LineId) {
          adon.IsAddonSelected = false;
          adon.IsLineItemCreated = false;
          adon.LineSummary.Price = 0;
          adon.LineSummary.Tax = 0;
          adon.LineSummary.PriodicDiscount = 0;
          adon.LineSummary.ManualDiscount = 0;

          this.CalculateMDSQuantity(cl);
        }
      });
    });

    if (linesToRemove.length > 0) {
      this.ProcessRemoveCartLines(linesToRemove);
    }
  }

  //Remove Discount line
  removeDiscountLine(discounts: Discount[], adonIndex) {
    discounts.splice(adonIndex, 1);
    this.onModelChange("");
  }

  removeSelectedRows() {
    let linesToRemove: CartItem[] = [];
    for (var index = 0; index < this.cart.CartLines.length; index++) {
      let cl = this.cart.CartLines[index];
      if (cl.IsSelected) {
        if (cl.IsLineItemCreated) {
          linesToRemove.push(cl);

          cl.adOnDetails.forEach(ad => {
            if (ad.IsLineItemCreated) {
              linesToRemove.push(ad);
            }
          });
        }
        else {
          this.cart.CartLines.splice(index, 1);
          index--;
        }
      }
    }

    this.EnableDelete();

    if (linesToRemove.length > 0) {
      this.ProcessRemoveCartLines(linesToRemove);
    }
  }

  ClearCart() {
    this.cart = new Cart();
    this.onModelChange("");
  }

  isAllChecked() {
    this.cart.CartLines.every(cl => cl.IsSelected);
  }

  selectAllRows(event) {
    this.cart.CartLines.forEach(cl => {
      cl.IsSelected = event.target.checked;
    });
    this.EnableDelete();
  }

  EnableDelete() {
    let seletedCount = this.cart.CartLines.filter(cl => cl.IsSelected).length;
    if (seletedCount > 0) {
      this.IsEnableDelete = true;
    }
    else {
      this.IsEnableDelete = false;
    }
  }

  //Change Purchase Type i.e: New Order, Migrate, Transfer
  changePurchaseType(selected) {
    if (selected == 'new-order')
      this.purchaseType = 'new-order';
    else if (selected == 'swtich')
      this.purchaseType = 'swtich';
    else if (selected == 'transfer')
      this.purchaseType = 'transfer';
    else if (selected == 'update')
      this.purchaseType = 'update';
    else if (selected == 'upgrade')
      this.purchaseType = 'upgrade';
    else if (selected == 'migrate')
      this.purchaseType = 'migrate';

  }

  //Get Sources 
  LoadAffiliations() {
    // this.Affiliations = JSON.parse(localStorage.getItem('affiliations'));
    // if(this.Affiliations == null || this.Affiliations == 'undefined'){
    //     this.http.get(AppConfig.getAffiliations).subscribe((res)=>{
    //       if(res['Status']){
    //         this.Affiliations = JSON.stringify(res['Result']);
    //         localStorage.setItem('affiliations', this.Affiliations);
    //         this.Affiliations = res['Result'];
    //       }
    //   });
    // }
  }

  FillCartItems(): void {

    this.productsObject.length > 0 &&
      this.productsObject.forEach((item, index) => {

        let cartItem = new CartItem();
        let biot: string;
        let biotStr: string;

        cartItem.validFrom = this.formattedDate;
        cartItem.ItemId = item.Value['sku'];
        cartItem.name = item.Value['name'];
        cartItem.basePrice = parseFloat(item.Value['price']);
        cartItem.Quantity = parseFloat(item.Value['qty']);
        cartItem.QuantityIcrement = parseFloat(item.Value['qty_increments']);
        cartItem.QuantityMin = parseFloat(item.Value['use_config_min_sale_qty']);
        cartItem.QuantityMax = parseFloat(item.Value['use_config_max_sale_qty']);
        cartItem.UnitOfMeasureSymbol = item.Value['default_unit_of_measure'];
        cartItem.CrossSellSku = item.Value['crosssell_skus'];
        cartItem.validFrom = this.dateUtilities.addDays(0);
        cartItem.MasterItemId = cartItem.ItemId.split("_")[0];

        cartItem.IsChannel = item.Value['is_channel'] == 'Yes' ? true : false;
        cartItem.ConcurrentUsers = Number(item.Value['concurrent_users']);

        if (item.Value['sku'].includes("_")) {
          if (item.Value['sku'].includes("_") && !item.Value['name'].includes("Addon") && !item.Value['name'].includes("Support") && item.Value['store_view_code'].includes("en_")) {
            cartItem.IsLineItem = true;
          }
          else {
            cartItem.IsLineItem = false;
          }

          biotStr = item.Value['additional_attributes'];
          if (biotStr != "") {
            let biot = biotStr.split(",");
            if (biot[0] != "") {
              let bi = biot[0].split("=");
              cartItem.billingInterval = bi[1];
            }
            if (biot[1] != "") {
              let ot = biot[1].split("=");
              cartItem.offerType = ot[1];
            }
          }

          switch (cartItem.offerType) {
            case "PER": {
              cartItem.license = "Perpetual";
              cartItem.validTo = "";
              break;
            }
            case "SUB-3Y": {
              cartItem.license = "3 Year";
              cartItem.validTo = this.dateUtilities.addDays(365 * 3);
              break;
            }
            case "SUB-2Y": {
              cartItem.license = "2 Year";
              cartItem.validTo = this.dateUtilities.addDays(365 * 2);
              break;
            }
            case "SUB-Y": {
              cartItem.license = "1 Year";
              cartItem.validTo = this.dateUtilities.addDays(365);
              break;
            }
            case "SUB-6M": {
              cartItem.license = "6 Months";
              cartItem.validTo = this.dateUtilities.addDays(30 * 6);
              break;
            }
            case "SUB-3M": {
              cartItem.license = "3 Months";
              cartItem.validTo = this.dateUtilities.addDays(30 * 3);
              break;
            }
            case "SUB-M": {
              cartItem.license = "1 Month";
              cartItem.validTo = this.dateUtilities.addDays(30);
              break;
            }
          }

          this.AllPorductItems.push(cartItem);
        }
      });

    this.AllPorductItems.forEach((pd, index) => {
      if (pd.IsLineItem) {
        var ddl = new Dropdown();
        ddl.Value = pd.ItemId;
        ddl.Text = pd.name;
        this.ddlLineItem.push(ddl);
        this.ddlPopularProducts.push(ddl);

        //For Grid Poriducts
        let popularProductsGrid = new CartItem();
        popularProductsGrid.ItemId = pd.ItemId;
        popularProductsGrid.name = pd.name;
        popularProductsGrid.billingInterval = pd.billingInterval;
        popularProductsGrid.offerType = pd.offerType;
        popularProductsGrid.MasterItemId = pd.MasterItemId;
        popularProductsGrid.LineId = pd.LineId;
        this.data.push(popularProductsGrid);

      }
    });
  }

  UpdateCartSummary() {
    this.cart.CartSummary.ManualDiscount = 0;

    this.cart.CartLines.forEach(cl => {
      cl.AdonLineSummary.Price = 0;
      cl.AdonLineSummary.PriodicDiscount = 0;
      cl.AdonLineSummary.Total = 0;
      cl.AdonLineSummary.ManualDiscount = 0;
      cl.LineSummary.ManualDiscount = 0;

      cl.adOnDetails.forEach(ad => {
        if (ad.IsAddonSelected) {
          cl.AdonLineSummary.Price += ad.LineSummary.Price;
          cl.AdonLineSummary.PriodicDiscount += ad.LineSummary.PriodicDiscount;
          cl.AdonLineSummary.Total += ad.LineSummary.NetAmountWithoutTax;

          // Process adon Manual Discount
          ad.DiscountDetails.forEach(pdDisc => {
            if (!pdDisc.periodicDiscount && Number(pdDisc.DiscountAmount) != NaN && Number(pdDisc.DiscountAmount) > 0) {
              cl.AdonLineSummary.ManualDiscount += Number(pdDisc.DiscountAmount);
            }
          });
        }
      });

      // manual Discount
      cl.DiscountDetails.forEach(pdDisc => {
        if (!pdDisc.periodicDiscount && Number(pdDisc.DiscountAmount) != NaN && Number(pdDisc.DiscountAmount) > 0) {
          cl.LineSummary.ManualDiscount += Number(pdDisc.DiscountAmount);
        }
      });
      console.log("SUmmary", cl);
      cl.LineSummary.TotalWithManualPriodicDiscount = cl.LineSummary.Total - cl.LineSummary.ManualDiscount;
      cl.AdonLineSummary.TotalWithManualPriodicDiscount = cl.AdonLineSummary.Total - cl.AdonLineSummary.ManualDiscount;
      cl.LineSummary.GrandTotal = cl.LineSummary.TotalWithManualPriodicDiscount + cl.AdonLineSummary.TotalWithManualPriodicDiscount;

      this.cart.CartSummary.ManualDiscount += (cl.LineSummary.ManualDiscount + cl.AdonLineSummary.ManualDiscount);

    });


  }

  ValidateCartLines(): boolean {
    // Validate Cart Quantity
    let isValid: boolean = true;
    let validationMsg: string = "";
    this.cart.CartLines.forEach(cl => {
      if (cl.Quantity <= 0 || !cl.IsValidQuantity) {
        isValid = false;
        validationMsg += "\n" + cl.name;
      }

      if (cl.adOnDetails.length > 0) {
        cl.adOnDetails.forEach(ad => {
          if (ad.IsAddonSelected && (ad.Quantity <= 0 || !cl.IsValidQuantity)) {
            isValid = false;
            validationMsg += "\n" + ad.name;
          }
        });
      }
    });

    if (!isValid) {
      this.toastr.error(validationMsg, 'Cart lines validation error for quantity.', { timeOut: 5000 });
    }

    return isValid;
  }

  /* Api Call start */
  CreateCartLine() {

    if (!this.ValidateCartLines()) {
      return;
    }


    let addCartLines = [];
    let updateCartLines = [];

    this.cart.CartLines.forEach(cl => {
      if (!cl.IsLineItemCreated) {
        addCartLines.push(cl);
      }
      else if (cl.IsLineItemChange) {
        updateCartLines.push(cl);
      }

      // process adon
      cl.adOnDetails.forEach(ad => {
        if (ad.IsAddonSelected) {
          if (!ad.IsLineItemCreated) {
            addCartLines.push(ad);
          }
          else if (ad.IsLineItemChange) {
            updateCartLines.push(ad);
          }
        }
      });
    });

    if (!this.cart.IsCartCreated) {
      this.ProcessCreateMergeCart(addCartLines);
    }
    else {

      if (addCartLines.length > 0) {
        this.ProcessAddCartLines(addCartLines);
      }

      if (updateCartLines.length > 0) {
        this.ProcessUpdateCartLines(updateCartLines);
      }
    }
  }

  ProcessCreateMergeCart(addCartLines: CartItem[]) {
    if (this.customerInfo == undefined || this.customerInfo.InvoiceAddress == undefined) {
      this.toastr.error('Invoice Address not found', 'Error', {
        timeOut: 3000
      });
      return;
    }
    this.isLoadingSummary = true;
    let deliveryAddress = this.customerInfo.InvoiceAddress;
    deliveryAddress.TaxGroup = this.customerInfo.SalesTaxGroup;
    deliveryAddress.AddressType = 2;

    this.cart.DeliverySpecification.DeliveryAddress = deliveryAddress;

    let cartCreateMergeRequest = new Cart();
    cartCreateMergeRequest.CartId = this.cart.CartId;
    cartCreateMergeRequest.CalculationModes = "All";
    cartCreateMergeRequest.CartVersion = this.cart.CartVersion;
    cartCreateMergeRequest.CartLines = addCartLines;
    cartCreateMergeRequest.DeliverySpecification = this.cart.DeliverySpecification;

    this.http.post(AppConfig.createCartMerged, cartCreateMergeRequest).subscribe((res) => {
      if (res['Status']) {
        this.isLoadingSummary = false;
        res['Cart'].CartLines.forEach(lineItem => {
          this.cart.CartLines.forEach(cl => {
            if (cl.LineId == lineItem.LineId) {
              cl.IsLineItemCreated = true;
              cl.IsLineItemChange = false;
            }
            else {
              cl.adOnDetails.forEach(ad => {
                if (ad.LineId == lineItem.LineId) {
                  ad.IsLineItemCreated = true;
                  ad.IsLineItemChange = false;
                }
              });
            }
          });
        });

        this.ProcessCartSuccessResponse(res['Cart'], "Cart Lines Created Successfully.");

      } else {
        this.cart.IsCartCreated = false;

        this.toastr.error('Could not save cart lines', 'Error', {
          timeOut: 3000
        });
      }
    });
  }

  ProcessAddCartLines(addCartLines: CartItem[]) {
    this.isLoadingSummary = true;
    let cartLinesCreateRequest = new CartLinesCreateRequest();
    cartLinesCreateRequest.CartId = this.cart.CartId;
    cartLinesCreateRequest.CalculationModes = "All";
    cartLinesCreateRequest.CartVersion = this.cart.CartVersion;
    cartLinesCreateRequest.CartLines = addCartLines;

    this.cartService.AddCartLines(cartLinesCreateRequest).subscribe((res) => {
      if (res['Status']) {
        this.isLoadingSummary = false;
        res['Cart'].CartLines.forEach(lineItem => {
          this.cart.CartLines.forEach(cl => {
            if (cl.LineId == lineItem.LineId) {
              cl.IsLineItemCreated = true;
              cl.IsLineItemChange = false;
            }
            else {
              cl.adOnDetails.forEach(ad => {
                if (ad.LineId == lineItem.LineId) {
                  ad.IsLineItemCreated = true;
                  ad.IsLineItemChange = false;
                }
              });
            }
          });
        });

        this.ProcessCartSuccessResponse(res['Cart'], "Cart Lines Added Successfully.");

      }
      else {
        this.cart.CartLines.forEach(cl => {
          addCartLines.forEach(ad => {
            if (cl.LineId == ad.LineId) {
              cl.IsLineItemCreated = false;
            }
            else if (cl.CrossSellSku != "") {
              cl.adOnDetails.forEach(adcl => {
                if (adcl.LineId == ad.LineId) {
                  ad.IsLineItemCreated = false;
                }
              });
            }
          });
        });
        this.toastr.error('Could not save cart lines', 'Error', {
          timeOut: 3000
        });
      }
    });
  }

  ProcessUpdateCartLines(updateCartLines: CartItem[]) {
    this.isLoadingSummary = true;
    let cartLinesCreateRequest = new CartLinesCreateRequest();
    cartLinesCreateRequest.CartId = this.cart.CartId;
    cartLinesCreateRequest.CalculationModes = "All";
    cartLinesCreateRequest.CartVersion = this.cart.CartVersion;
    cartLinesCreateRequest.CartLines = updateCartLines;

    this.cartService.UpdateCartLines(cartLinesCreateRequest).subscribe((res) => {
      if (res['Status']) {
        this.isLoadingSummary = false;
        res['Cart'].CartLines.forEach(lineItem => {
          this.cart.CartLines.forEach(cl => {
            if (cl.LineId == lineItem.LineId) {
              cl.IsLineItemChange = false;
            }
            else {
              cl.adOnDetails.forEach(ad => {
                if (ad.LineId == lineItem.LineId) {
                  ad.IsLineItemChange = false;
                }
              });
            }
          });
        });

        this.ProcessCartSuccessResponse(res['Cart'], "Cart Lines Updated Successfully");

      } else {
        this.cart.CartLines.forEach(cl => {
          updateCartLines.forEach(ucl => {
            if (cl.LineId == ucl.LineId) {
              cl.IsLineItemChange = true;
            }
            else {
              cl.adOnDetails.forEach(adcl => {
                if (adcl.LineId == ucl.LineId) {
                  adcl.IsLineItemChange = true;
                }
              });
            }
          });
        });

        this.toastr.error('Could not update cart lines', 'Error', {
          timeOut: 3000
        });
      }
    });
  }

  ProcessRemoveCartLines(cartLines: CartItem[]) {
    this.isLoadingSummary = true;
    let linesToRemove = cartLines.map(cl => cl.LineId);

    this.cartService.RemoveCartLines(this.cart.CartId, linesToRemove).subscribe(res => {
      if (res['Status']) {
        this.isLoadingSummary = false;
        this.ProcessCartSuccessResponse(res['Cart'], "Cart Lines Removed Successfully.")
        linesToRemove.forEach(LineId => {
          this.cart.CartLines.forEach((cl, index) => {
            if (cl.LineId == LineId) {
              this.cart.CartLines.splice(index, 1);
            }
            else {
              cl.adOnDetails.forEach((adon, aIndex) => {
                if (adon.LineId == LineId) {
                  adon.IsAddonSelected = false;
                  adon.IsLineItemCreated = false;
                  adon.LineSummary.Price = 0;
                  adon.LineSummary.Tax = 0;
                  adon.LineSummary.PriodicDiscount = 0;
                  adon.LineSummary.ManualDiscount = 0;
                }
              });
            }

            this.CalculateMDSQuantity(cl);
          });
        });
        this.EnableDelete();
      }
      else {
        this.toastr.error('Could not Create Sales Order', 'Error', {
          timeOut: 3000
        });
      }
      this.UpdateCartSummary();
    });
  }

  ProcessCartSuccessResponse(cartResponse, message: string) {
    this.cart.IsCartCreated = true;
    this.cart.CartVersion = cartResponse.Version;
    // // localStorage.setItem("CL", JSON.stringify(this.cart));
    // this.Affiliations = JSON.stringify(res['Result']);
    // localStorage.setItem('affiliations', this.Affiliations);
    // this.Affiliations = res['Result'];

    this.cart.CartSummary.Price = cartResponse.SubtotalAmount;
    this.cart.CartSummary.Tax = cartResponse.TaxAmount;
    this.cart.CartSummary.PriodicDiscount = cartResponse.DiscountAmount;
    this.cart.CartSummary.Total = cartResponse.TotalAmount;

    cartResponse.CartLines.forEach(lineItem => {
      this.cart.CartLines.forEach(cl => {
        if (cl.LineId == lineItem.LineId) {
          let monthlyPrice: number = 0;

          cl.LineSummary.BasePrice = lineItem.Price;
          cl.LineSummary.Price = lineItem.Price * lineItem.Quantity;
          cl.LineSummary.Tax = lineItem.TaxAmount;
          cl.LineSummary.PriodicDiscount = lineItem.DiscountAmount;

          cl.LineSummary.NetAmountWithoutTax = lineItem.NetAmountWithoutTax;
          cl.LineSummary.Total = lineItem.NetAmountWithoutTax;

          cl.LineSummary.ExtendedPrice = lineItem.ExtendedPrice;
          cl.LineSummary.TaxRatePercent = lineItem.TaxRatePercent;

          cl.LineSummary.ManualDiscount = 0;

          if (cl.DiscountDetails != undefined) {
            cl.DiscountDetails = cl.DiscountDetails.filter(dd => !dd.periodicDiscount);
            if (cl.DiscountDetails[0] != undefined) {
              this.OnManualDiscountChange(cl, cl.DiscountDetails[0]);
              // this.cdr.detectChanges();
            }
          }
          else {
            cl.DiscountDetails = [];
          }

          monthlyPrice = cl.LineSummary.Price;

          lineItem.DiscountLines.forEach(discLineItem => {
            if (discLineItem.EffectiveAmount > 0) {
              let discountline = new Discount();

              discountline.Interval = 1;
              discountline.periodicDiscount = true;
              discountline.sku = lineItem.ItemId;
              discountline.DiscountLineTypeValue = discLineItem.DiscountLineTypeValue;
              discountline.DiscountAmount = discLineItem.EffectiveAmount;
              discountline.Tax = discLineItem.Tax;
              discountline.OfferId = discLineItem.OfferId;
              discountline.DiscountCode = "08";
              discountline.OfferName = discLineItem.OfferName;
              discountline.Percentage = discLineItem.Percentage;
              discountline.DiscountLineTypeValue = discLineItem.DiscountLineTypeValue;
              discountline.ManualDiscountTypeValue = discLineItem.ManualDiscountTypeValue;
              discountline.CustomerDiscountTypeValue = discLineItem.CustomerDiscountTypeValue;
              discountline.PeriodicDiscountTypeValue = discLineItem.PeriodicDiscountTypeValue;

              discountline.MonthlyPrice = Number(monthlyPrice.toFixed(2));
              monthlyPrice = discountline.NextPrice = Number((monthlyPrice - discountline.DiscountAmount).toFixed(2));

              cl.DiscountDetails.unshift(discountline);
            }
          });
        }
        else // if line item is adon
        {
          cl.adOnDetails.forEach(adcl => {
            if (adcl.LineId == lineItem.LineId) {
              let monthlyPrice: number = 0;

              adcl.LineSummary.BasePrice = lineItem.Price;
              adcl.LineSummary.Price = lineItem.Price * lineItem.Quantity;
              adcl.LineSummary.Tax = lineItem.TaxAmount;
              adcl.LineSummary.PriodicDiscount = lineItem.DiscountAmount;

              adcl.LineSummary.NetAmountWithoutTax = lineItem.NetAmountWithoutTax;
              adcl.LineSummary.Total = lineItem.NetAmountWithoutTax;

              adcl.LineSummary.ExtendedPrice = lineItem.ExtendedPrice;
              adcl.LineSummary.TaxRatePercent = lineItem.TaxRatePercent;
              adcl.LineSummary.ManualDiscount = 0;

              if (adcl.DiscountDetails != undefined) {
                adcl.DiscountDetails = adcl.DiscountDetails.filter(dd => !dd.periodicDiscount);
                if (adcl.DiscountDetails[0] != undefined) {
                  this.OnManualDiscountChange(adcl, adcl.DiscountDetails[0]);
                  // this.cdr.detectChanges();
                }
              }
              else {
                adcl.DiscountDetails = [];
              }

              monthlyPrice = adcl.LineSummary.Price;

              lineItem.DiscountLines.forEach(discLineItem => {
                if (discLineItem.EffectiveAmount > 0) {
                  let discountline = new Discount();

                  discountline.Interval = 1;
                  discountline.periodicDiscount = true;
                  discountline.sku = lineItem.ItemId;
                  discountline.DiscountLineTypeValue = discLineItem.DiscountLineTypeValue;
                  discountline.DiscountAmount = discLineItem.EffectiveAmount;
                  discountline.Tax = discLineItem.Tax;
                  discountline.OfferId = discLineItem.OfferId;
                  discountline.DiscountCode = "08";
                  discountline.OfferName = discLineItem.OfferName;
                  discountline.Percentage = discLineItem.Percentage;
                  discountline.DiscountLineTypeValue = discLineItem.DiscountLineTypeValue;
                  discountline.ManualDiscountTypeValue = discLineItem.ManualDiscountTypeValue;
                  discountline.CustomerDiscountTypeValue = discLineItem.CustomerDiscountTypeValue;
                  discountline.PeriodicDiscountTypeValue = discLineItem.PeriodicDiscountTypeValue;

                  discountline.MonthlyPrice = Number(monthlyPrice.toFixed(2));
                  monthlyPrice = discountline.NextPrice = Number((monthlyPrice - discountline.DiscountAmount).toFixed(2));

                  adcl.DiscountDetails.unshift(discountline);
                }
              });
            }
          });

        }
      });
    });

    this.UpdateCartSummary();
    this.toastr.success(message);
  }

  ApplyCoupon() {

    if (this.CouponCode == undefined) {
      this.toastr.error('No coupon code added', 'Error', {
        timeOut: 3000
      });
      return;
    }
    var cartCouponRequest: any = {};
    cartCouponRequest.CartId = this.cart.CartId;
    cartCouponRequest.isLegacyDiscountCode = false;
    cartCouponRequest.CouponCodes = [];
    cartCouponRequest.CouponCodes.push(this.CouponCode);

    this.http.post(AppConfig.AddCouponsToCart, cartCouponRequest).subscribe((res) => {
      if (res['Status']) {
        this.ProcessCartSuccessResponse(res['Cart'], "Coupon Applied Successfully.");
      } else {
        this.toastr.error('Invalid coupon code', 'Error', {
          timeOut: 3000
        });
      }
    });
  }



  /* Api Call end */

  private SetMDSStaticData(cartLineItem: CartItem) {
    // TVC
    if (cartLineItem.ItemId == "TVC0001_000000091") {
      cartLineItem.ConcurrentUsers = 3;
      var adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD001_000000086");
      if (adon != undefined) {
        adon.ConcurrentUsers = 1;
      }
      adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD003_000000088");
      if (adon != undefined) {
        adon.IsChannel = true;
      }
    }

    // TVB
    if (cartLineItem.ItemId == "TVB0001_000000089") {
      cartLineItem.ConcurrentUsers = 1;
      var adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD001_000000086");
      if (adon != undefined) {
        adon.ConcurrentUsers = 1;
      }
      adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD003_000000088");
      if (adon != undefined) {
        adon.IsChannel = true;
      }
    }
    
    // TVP
    if (cartLineItem.ItemId == "TVP0001_000000093") {
      cartLineItem.ConcurrentUsers = 1;
      var adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD001_000000086");
      if (adon != undefined) {
        adon.ConcurrentUsers = 1;
      }
      adon = cartLineItem.adOnDetails.find(ad => ad.ItemId == "TVAD003_000000088");
      if (adon != undefined) {
        adon.IsChannel = true;
      }
    }

  }

}
