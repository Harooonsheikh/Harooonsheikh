import { Address } from '../models/address';
import { ContactPerson } from '../models/contact-person';
import { Dropdown } from './common';
export class Customer {
    Phone: string;
    Email: string;
    Name: string;
    FirstName:string;
    MiddleName:string;
    LastName:string;
    VatNumber:string;
    Language:string;
    SwapLanguage:boolean;
    Url:string;
    UrlRecordId:number;
    CustomerType:number;
    CustomerTypeValue:number;
    IdentificationNumber:string;
    Addresses:Address[];
    Image:string;
    EntityName:string;
    ExtensionData:string;
    ExtensionProperties:any[];
    Item:string;
    EcomCustomerId:string;
    SLBirthMonth:string;
    CustomerAddresses:string;
    IsAsyncCustomer:boolean;
    Attributes:any[];
    ContactPerson:ContactPerson;
    AccountNumber?:string;
    CurrencyCode:string;
    ContactPersonId:string;
    SalesTaxGroup:string;
    DirectoryPartyRecordId:string;

    InvoiceAddress: Address;
    BusinessAddress: Address;

    TermsOfPayment: number;
    ExpirtyDate: string;
    CatalogName: string;
    CountryCode: string;
    CustomerPopularProducts:any = [];
    SelectedRowsIndexes: any = [];
    CustomerAllProducts: any = '';
    CustomerLoaded: boolean = false;
}