import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppConfig } from '../../../../_shared/constants/app-config';
import { http } from '../../../../_shared/services/http';
import { first } from 'rxjs/operators';
import { MockData } from '../../../../_shared/constants/mock-data';
import { AppSetting } from '../../../../_shared/constants/app-setting';
import { Customer } from '../../../../_shared/models/customer';
import { ContactPerson } from '../../../../_shared/models/contact-person';
import { Address } from '../../../../_shared/models/address';
import { Vat } from '../../../../_shared/models/vat';
import { Dropdown } from 'src/app/_shared/models/common';
import { formatCurrency, formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { Utilities } from '../../../../_shared/constants/utilities';
import { BehaviourSubjectService } from '../../../../_shared/services/behaviour-subject-service';
@Component({
    selector: 'app-customer-information',
    templateUrl: './customer-information.component.html',
    styleUrls: ['./customer-information.component.scss']
})

export class CustomerInformationComponent implements OnInit {
    CustomerPopularProducts: Array<Dropdown> = [];
    CustomerPopularProductsIndex: any = [];
    customerInfo: Customer;
    postcustomerInfo: Customer;
    contactPerson: ContactPerson;
    postcontactPerson: ContactPerson;
    address: Address;
    invoiceAddress: Address;
    postaddress: Address;
    postInvoiceAddress: Address;
    vatnumber: Vat;
    erpId: any = '';
    salesRepName: any = '';
    contractId: any = '';
    currentAccount: any = "";
    getCustomerObj: any = '';
    updateCustomerRequest: any = '';
    updateContactPersonRequest: any = '';
    cpState: any = 'Inactive';
    customerState: any = 'Inactive';
    currencies: any = "";
    languages: any = "";
    countries: any = "";
    vatgroups: any = "";
    addressCountry: any = '';
    storeKey: any = '';
    updateCustomerInfo = new FormGroup({

    });
    titles: any = '';
    updateCustomerPencil: boolean = true;
    updateContactPencil: boolean = true;
    updateAdressPencil: boolean = true;
    updateTaxPencil: boolean = true;
    RecordId: any;
    country: string;
    loading: boolean = false;
    contactSection: FormGroup;
    addressSection: FormGroup;
    customerSection: FormGroup;
    taxSection: FormGroup;
    language: any = '';
    customerlanguage: any = '';
    vatno: any;
    utility: any = "";
    flag: boolean = false;
    invoicecountry: any = '';
    contactLanguage: any = '';
    contactCountry: any = '';
    customerResponse: any = '';
    addUpdateFlag: boolean = false;
    existingContact: ContactPerson;
    existingContactOriginal: ContactPerson;
    existingCustomer: Customer;
    contactPersons: ContactPerson[];
    existingContactPersons: ContactPerson[];
    AllProductsFromCatalog: any = '';
    constructor(
        private customerService: http,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private spinner: NgxSpinnerService,
        public fb: FormBuilder,
        private behaviorService: BehaviourSubjectService
    ) {
        this.contactSection = fb.group({
            "cpLanguage": ["",],
            "cpSalutation": ["",],
            "cpFirstName": ["",],
            "cpLastName": ["",],
            "cpEmail": ["",],
            "cpPhone": ["",],
            "cpAddress": ["",],
            "cpZipCode": ["",],
            "cpCity": ["",],
            "cpCountry": ["",],
            "cpInvoiceLetter": ["",],

        })
        this.addressSection = fb.group({
            "addressLine1": ["",],
            "zipCode": ["",],
            "addressCity": ["",],
            "addressCountry": ["",],
            "addressState": ["",],
            "addressInvoiceLetterEmail": ["",],
            "currencyCode": ["",],
            "language": ["",],
        })
        this.customerSection = fb.group({
            "accountNumber": ["",],
            "customerName": ["",],
            "customerAddress": ["",],
            "customerCity": ["",],
            "customerZipCode": ["",],
            "customerCountry": ["",],
            "customerState": ["",],
            "customerCurrencyCode": ["",],
            "customerLanguage": ["",]
        })
        this.taxSection = fb.group({
            "vatGroup": ["",],
            "taxExemptNumber": ["",],
        })
        this.vatnumber = new Vat();
        this.customerInfo = new Customer();
        this.address = new Address();
        this.invoiceAddress = new Address();
        this.existingContact = this.contactPerson;
        this.utility = new Utilities();
    }
    ngOnInit() {
        this.loading = true;
        this.currencies = MockData.CURRENCIES;
        this.languages = MockData.LANGUAGES;
        this.countries = MockData.COUNTRIES;
        this.vatgroups = MockData.VATGROUP;
        this.titles = MockData.Title;
        this.updateCustomerInfo.disable();
        this.currentAccount = this.getCustomerAccount();
        this.erpId = this.getSalesRepId();
        this.salesRepName = this.getSalesRepName();
        this.contractId = this.getContractId();
        this.getCustomerObj = {
            "customerId": this.currentAccount,
            "useMapping": false,
            "searchLocation": 2
        };
        this.getCustomer();
        this.getContactPerson();
    }

    getCustomer() {
        if (this.currentAccount > 0) {
            this.spinner.show();
            this.customerService.post(AppConfig.getCustomerInfo, this.getCustomerObj).subscribe((res) => {
                if (res['Status']) {
                    this.customerResponse = res['CustomerInfo'];
                    this.fillCustomerInfo();
                }
            });
        }
    }

    fillCustomerInfo() {
        this.customerInfo.AccountNumber = this.customerResponse.AccountNumber;
        this.customerInfo.SalesTaxGroup = this.customerResponse.SalesTaxGroup;
        this.customerInfo.DirectoryPartyRecordId = this.customerResponse.DirectoryPartyRecordId;
        this.customerInfo.Phone = this.customerResponse.Phone;
        this.customerInfo.Email = this.customerResponse.Email;
        this.customerInfo.Name = this.customerResponse.Name;
        this.customerInfo.FirstName = this.customerResponse.FirstName;
        this.customerInfo.MiddleName = this.customerResponse.MiddleName;
        this.customerInfo.LastName = this.customerResponse.LastName;
        this.customerInfo.VatNumber = this.customerResponse.VatNumber;
        this.customerInfo.Language = this.customerResponse.Language;
        this.customerInfo.SwapLanguage = this.customerResponse.SwapLanguage;
        this.customerInfo.CurrencyCode = this.customerResponse.CurrencyCode;
        this.customerInfo.Url = this.customerResponse.Url;
        this.customerInfo.UrlRecordId = this.customerResponse.UrlRecordId;
        this.customerInfo.CustomerType = this.customerResponse.CustomerType;
        this.customerInfo.IdentificationNumber = this.customerResponse.IdentificationNumber;
        this.customerInfo.Addresses = this.customerResponse.Addresses;
        this.customerInfo.Image = this.customerResponse.Image;
        this.customerInfo.EntityName = this.customerResponse.EntityName;
        this.customerInfo.ExtensionData = this.customerResponse.ExtensionData;
        this.customerInfo.ExtensionProperties = this.customerResponse.ExtensionProperties;
        this.customerInfo.Item = this.customerResponse.Item;
        this.customerInfo.EcomCustomerId = this.customerResponse.EcomCustomerId;
        this.customerInfo.SLBirthMonth = this.customerResponse.SLBirthMonth;
        this.customerInfo.CustomerAddresses = this.customerResponse.CustomerAddresses;
        this.customerInfo.IsAsyncCustomer = this.customerResponse.IsAsyncCustomer;
        this.customerInfo.Attributes = [];
        if (this.customerInfo.Email == "" || this.customerInfo.Email == undefined) {
            this.customerInfo.Email = "Neymar@customer.com";
        }
        if (this.customerInfo.FirstName == "" || this.customerInfo.FirstName == undefined) {
            this.customerInfo.FirstName = "Customer";
        }
        if (this.customerInfo.LastName == "" || this.customerInfo.LastName == undefined) {
            this.customerInfo.LastName = "Customer";
        }
        if (this.customerInfo.Language == "" || this.customerInfo.Language == undefined) {
            this.customerInfo.Language = "en_GB";
        }
        if (this.customerInfo.CustomerType == 0) {
            this.customerInfo.CustomerType = 1;
        }
        this.setCustomerInfo();
        localStorage.setItem('salesTax', this.customerInfo.SalesTaxGroup);
        localStorage.setItem('cc', this.customerInfo.CurrencyCode);
        this.loading = false;
        this.currentAccount = this.customerInfo.AccountNumber;
        //TODO: replace this.address with this.customerInfo.BusinessAddress. and change below line accordingly.
        this.customerInfo.BusinessAddress = this.address = this.customerInfo.Addresses.find(adress => adress.IsPrimary && adress.AddressTypeValue == 9);
        if (this.address == undefined) {
            this.customerInfo.BusinessAddress = this.address = new Address();
        }
        //TODO: replace this.invoiceAddress with this.customerInfo.InvoiceAddress. and change below line accordingly.
        this.customerInfo.InvoiceAddress = this.invoiceAddress = this.customerInfo.Addresses.filter(cl => !cl.IsPrimary && cl.AddressTypeValue == 1).sort(cl => cl.RecordId).pop();
        if (this.invoiceAddress == undefined) {
            this.customerInfo.InvoiceAddress = this.invoiceAddress = new Address();
        }
        this.customerInfo.ExtensionProperties.forEach((extensions, index) => {
            if (extensions.Key == "TermsOfPayment") {
                this.customerInfo.TermsOfPayment = (extensions.Value['IntegerValue'] != null ? parseInt(extensions.Value['IntegerValue']) : 14);
                this.customerInfo.ExpirtyDate = formatDate(this.utility.addDays(this.customerInfo.TermsOfPayment), 'yyyy-MM-dd', 'en-US', '+0530');
            }
        });
        this.setStoreKeyCountry();
        if (this.invoiceAddress != undefined) {
            this.invoicecountry = this.getCountry(this.invoiceAddress.ThreeLetterISORegionName);
        }
        if (this.customerInfo.SalesTaxGroup == "") {
            this.customerInfo.SalesTaxGroup = this.getVatgroup(this.address.ThreeLetterISORegionName);
        }
        if (this.address != undefined) {
            this.addressCountry = this.getCountry(this.address.ThreeLetterISORegionName);
        }
        if (this.customerInfo.Language != undefined) {
            this.customerlanguage = this.getLanguage(this.customerInfo.Language);
        }
        this.existingCustomer = JSON.parse(JSON.stringify(this.customerInfo));
    }

    getContactPerson() {
        this.customerService.get(AppConfig.getAllContactPerson + "?customerAccount=" + this.currentAccount).subscribe((res) => {
            if (res['Status']) {
                this.loading = false;
                this.contactPersons = res.ContactPersonNALList;
                this.existingContactPersons = JSON.parse(JSON.stringify(this.contactPersons));
                if (this.contactPersons != undefined || this.contactPersons != null) {
                    this.contactPerson = this.contactPersons[0];
                    if (this.contactPerson != undefined || this.contactPerson != null) {
                        this.contactCountry = this.getCountry(this.contactPerson.Country);
                    }
                }
            }
            else {
                this.contactPersons = null;
                this.contactPerson = null;
            }
        });
    }
    getSelectedPerson(Id) {
        this.contactPerson = new ContactPerson();
        this.contactPerson = this.contactPersons.find(contactId => contactId.ContactPersonId == Id);
        this.contactCountry = this.getCountry(this.contactPerson.Country);

    }
    setCustomerInfo() {
        localStorage.setItem("CI", JSON.stringify(this.customerInfo));
    }
    setStoreKeyCountry() {
        if (this.address.ThreeLetterISORegionName !== '') {
            this.customerInfo.CountryCode = this.address.ThreeLetterISORegionName;
            if (localStorage.getItem('popular-products-' + this.customerInfo.CountryCode) !== null) {
                this.customerInfo.CustomerPopularProducts = JSON.parse(localStorage.getItem('popular-products-' + this.customerInfo.CountryCode));
            }
            if (JSON.parse(localStorage.getItem('selected-' + this.customerInfo.CountryCode)) != null) {
                this.customerInfo.SelectedRowsIndexes = JSON.parse(localStorage.getItem('selected-' + this.customerInfo.CountryCode));
            }

            this.LoadProducts();
        }
    }

    private LoadProducts(): void {
        this.customerService.get(AppConfig.getStore + "?countryCode=" + this.address.ThreeLetterISORegionName)
            .subscribe((res) => {
                this.storeKey = res;
                AppSetting.StoreKey = this.storeKey;

                this.LoadDiscountThreshold();

                localStorage.setItem("CurrentCountry", this.address.ThreeLetterISORegionName);
                this.customerService.get(AppConfig.getCatalog).subscribe((res) => {
                    res = JSON.parse(res);
                    let CatalogName = res.collectionname;
                    if (res != "") {
                        this.customerInfo.CatalogName = CatalogName;
                        this.customerService.get(AppConfig.getProducts + "?fileName=" + CatalogName + "&offSet=0&pageSize=0").subscribe((Products) => {
                            if (Products != "[]") {
                                this.customerInfo.CustomerLoaded = true;
                                this.customerInfo.CustomerAllProducts = JSON.parse(Products);
                                this.behaviorService.setBehaviorView(this.customerInfo.CustomerAllProducts);
                                localStorage.removeItem(localStorage.getItem("Catalog-" + this.address.ThreeLetterISORegionName));
                                localStorage.setItem("Catalog-" + this.address.ThreeLetterISORegionName, CatalogName);
                            } else {
                                this.toastr.error('Product Catalog is Empty', 'Error', {
                                    timeOut: 3000
                                });
                            }
                        });
                    }
                });
            });
    }

    private LoadDiscountThreshold() {
        this.customerService.get(AppConfig.GetDiscountThreshold)
            .subscribe(res => {
                if (res["Success"]) {
                    AppSetting.DiscountThreshold = res["Result"];
                }
            });
    }

    getCustomerAccount() {
        const id = +this.route.snapshot.paramMap.get('id');
        return id;
    }

    getSalesRepName() {
        const name = this.route.snapshot.paramMap.get('salesRap');
        return name;
    }

    getSalesRepId() {
        const erpId = +this.route.snapshot.paramMap.get('erpId');
        return erpId;
    }
    getContractId() {
        const contractId = +this.route.snapshot.paramMap.get('contractId');
        return contractId;
    }

    validateVatNumber(param) {
        this.vatnumber.CountryId = this.address.ThreeLetterISORegionName;
        this.vatnumber.VATNumber = param;
        var pos1 = this.customerInfo.SalesTaxGroup.indexOf("-");
        var pos2 = this.customerInfo.SalesTaxGroup.indexOf("-", pos1 + 1);
        this.customerService.post(AppConfig.ValidateVat, this.vatnumber).subscribe((res) => {
            if (res['Status']) {
                this.flag = true;
                this.customerInfo.SalesTaxGroup = this.customerInfo.SalesTaxGroup.substring(0, pos2 + 1);
                this.customerInfo.SalesTaxGroup = this.customerInfo.SalesTaxGroup.replace(this.customerInfo.SalesTaxGroup, this.customerInfo.SalesTaxGroup + 'RC');
            }
            else {
                this.flag = false;
                this.customerInfo.SalesTaxGroup = this.customerInfo.SalesTaxGroup.substring(0, pos2 + 1);
                this.customerInfo.SalesTaxGroup = this.customerInfo.SalesTaxGroup.replace(this.customerInfo.SalesTaxGroup, this.customerInfo.SalesTaxGroup + 'STA');
            }


        });
    }
    onSubmitAccount(flag): void {
        this.loading = true;
        if (flag == 1) {
            this.invoiceAddress.Street = this.address.Street;
            this.invoiceAddress.City = this.address.City;
            this.invoiceAddress.ZipCode = this.address.ZipCode;
            this.invoiceAddress.ThreeLetterISORegionName = this.address.ThreeLetterISORegionName;
            this.invoiceAddress.AddressType = 1;
            this.invoiceAddress.IsPrimary = false;
        }
        this.postcustomerInfo = new Customer();
        this.postcustomerInfo = JSON.parse(JSON.stringify(this.customerInfo));
        this.postcustomerInfo.CustomerAllProducts = [];
        this.postcustomerInfo.CustomerPopularProducts = [];
        this.postcustomerInfo.SelectedRowsIndexes = [];
        this.updateCustomerRequest = {
            "CustomerAccountNumber": this.currentAccount,
            "useMapping": false,
            "customer": this.postcustomerInfo,
            "ContactPerson": ""
        };
        this.customerService.post(AppConfig.updateCustomer, this.updateCustomerRequest).subscribe((res) => {
            if (res['Status']) {
                this.loading = false;
                this.toastr.success("Customer Information Saved");
                this.customerResponse = res['CustomerInfo'];
                this.fillCustomerInfo();
            } else {
                this.toastr.error('Could not save data', 'Error', {
                    timeOut: 3000
                });
                this.getCustomer();
            }
        });
    }
    saveContact(flag) {
        this.loading = true;
        if (flag == true) {
            this.contactPerson.CustAccount = this.customerInfo.AccountNumber;
            this.contactPerson.ContactForParty = this.customerInfo.DirectoryPartyRecordId;
        }
        this.updateContactPersonRequest =
            {
                "ContactPerson": this.contactPerson
            }
        this.customerService.post(AppConfig.saveContactPerson, this.updateContactPersonRequest).subscribe((res) => {
            if (res['Status']) {
                this.loading = false;
                this.toastr.success("Contact Person Information Saved");
                this.getContactPerson();

            } else {
                this.toastr.error('Could not save data', 'Error', {
                    timeOut: 3000
                });
                this.getContactPerson();
            }
        });

    }
    getCountry(Id) {
        for (let i = 0; i < this.countries.length; i++) {
            let country = this.countries;
            if (country[i].Value == Id) {
                return country[i].Text;
            }
        }
    }
    getLanguage(Id) {
        for (let i = 0; i < this.languages.length; i++) {
            let languge = this.languages;
            if (languge[i].Value == Id) {
                return languge[i].Text;
            }
        }
    }
    getVatgroup(Id) {
        for (let i = 0; i < this.vatgroups.length; i++) {
            let vat = this.vatgroups;
            if (vat[i].Value == Id) {
                this.vatno = vat[i].Text;
                return vat[i].Text;
            }
        }

    }
    CancelSave() {
        this.updateAdressPencil = true;
        this.getCustomer();
    }

    cancelContactPerson() {
        if (this.addUpdateFlag == true) {
            this.updateContactPencil = true;
            this.contactPerson = this.existingContact;
        }
        else {
            this.updateContactPencil = true;
            this.contactPerson = JSON.parse(JSON.stringify(this.existingContactOriginal));
        }
        this.contactPersons = JSON.parse(JSON.stringify(this.existingContactPersons));
    }
    UpdateCustomerInfo() {
        this.updateCustomerInfo.enable();
        this.updateCustomerPencil = false;
    }
    SaveCustomerInfo() {
        this.onSubmitAccount(1)
        this.updateCustomerPencil = true;
    }
    UpdateContactPerson(flag) {
        if (flag == false) {
            this.existingContactOriginal = JSON.parse(JSON.stringify(this.contactPerson));
            this.addUpdateFlag = false;
        }
        else {
            this.addUpdateFlag = true;
            this.existingContact = this.contactPerson;
            this.contactPerson = new ContactPerson();
            this.contactPerson.Street = this.address.Street;
            this.contactPerson.ZipCode = this.address.ZipCode;
            this.contactPerson.City = this.address.City;
            this.contactPerson.Country = this.address.ThreeLetterISORegionName;
        }
        this.updateCustomerInfo.enable();
        this.updateContactPencil = false;
        this.existingContactOriginal = JSON.parse(JSON.stringify(this.contactPerson));
    }
    SaveContactPerson() {
        if (this.addUpdateFlag == true) {
            this.saveContact(true);
        }
        else {
            this.saveContact(false);
        }
        this.updateContactPencil = true;
    }
    UpdateContactAdress() {
        this.updateCustomerInfo.enable();
        this.updateAdressPencil = false;
    }
    SaveContactAdress() {
        this.onSubmitAccount(2)
        this.updateAdressPencil = true;
    }
    CancelCustomerInfo() {
        this.updateCustomerPencil = true;
        this.getCustomer();
    }
    UpdateTaxInfo() {
        this.updateCustomerInfo.enable();
        this.updateTaxPencil = false;
    }
    CancelTaxInfo() {
        this.updateTaxPencil = true;
        this.customerInfo = JSON.parse(JSON.stringify(this.existingCustomer));
    }
    SaveTaxInfo() {
        localStorage.setItem('salesTax', this.customerInfo.SalesTaxGroup);
        if (this.flag == true || this.customerInfo.VatNumber == "") {
            this.onSubmitAccount(3);
            this.updateTaxPencil = true;
        }
        else {
            this.toastr.error("TaxExemptNumber is Not Valid");
            this.updateTaxPencil = false;
        }

    }

}
