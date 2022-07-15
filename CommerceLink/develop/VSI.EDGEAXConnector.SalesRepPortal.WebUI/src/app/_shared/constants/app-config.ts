import { environment } from '../../../environments/environment';

export class AppConfig {
    //CommerceLink EndPoints
    public static getCustomerInfo = environment.apiBaseUrl + "Customer/GetCustomer";
    public static createCustomer = environment.apiBaseUrl + "customer/CreateCustomer";
    public static updateCustomer = environment.apiBaseUrl + "Customer/MergeUpdateCustomerContactPerson";
    public static saveContactPerson = environment.apiBaseUrl + "ContactPerson/SaveContactPerson";
    public static getAllContactPerson = environment.apiBaseUrl + "ContactPerson/GetAllContactPersons";
    public static updateContactPerson = environment.apiBaseUrl + "ContactPerson/UpdateContactPerson";

    public static getAffiliations = environment.apiBaseUrl + "SalesOrder/GetRetailAffiliations";
    public static createSalesOrder = environment.apiBaseUrl + "salesorder/CreateSalesOrderTransaction";

    public static createQoutation = environment.apiBaseUrl + "Quotation/CreateQuotation";
    public static getQoute = environment.apiBaseUrl + "Quotation/GetQuotation";
    public static rejectQuote = environment.apiBaseUrl + "Quotation/RejectCustomerQuotation"; 

    public static createCartMerged = environment.apiBaseUrl + "Cart/CreateMergedCart";
    public static GetCart = environment.apiBaseUrl + "GetCart?cartId=";
    public static CreateOrUpdateCart = environment.apiBaseUrl + "Cart/CreateOrUpdateCart";
    public static AddCartLines = environment.apiBaseUrl + "Cart/AddCartLines";
    public static UpdateCartLines = environment.apiBaseUrl + "Cart/UpdateCartLines"; 
    public static RemoveCartLines = environment.apiBaseUrl + "Cart/RemoveCartLines";
    public static AddCouponsToCart = environment.apiBaseUrl + "Cart/AddCouponsToCart";

    public static GetDiscountThreshold = environment.apiBaseUrl + "Store/GetDiscountThreshold"; 

    //Mongo EndPoints
    public static getProducts = environment.MongoBaseURL + "Catalog/GetProducts";
    public static getProductSearchByName = environment.MongoBaseURL + "Search";
    public static getProductSearchBySKU = environment.MongoBaseURL + "Search";
    public static getPriceBook = environment.MongoBaseURL + "Catalog/GetUpdatedFileNames/";
    public static getPrice = environment.MongoBaseURL + "Price/Search/";
    public static getAllPrices = environment.MongoBaseURL + "Price/Get/";
    public static getStore = environment.MongoBaseURL + "Store/StoreKeyOfCountry/";
    public static getCatalog = environment.MongoBaseURL + "Catalog/GetUpdatedFileNames/?type=Catalog";
    public static SalesOrderSave = environment.MongoBaseURL + "SalesOrder/Save";
    public static SalesOrderGetById = environment.MongoBaseURL + "SalesOrder/GetById";
    public static SalesOrderGetAll = environment.MongoBaseURL + "SalesOrder/GetAll";

    //Validate Vat Number
    public static ValidateVat = environment.apiBaseUrl + "SalesOrder/ValidateVATNumber";
    static dropdownSettings = {
        singleSelection: false,
            idField: 'Value',
            textField: 'Text',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true,
            limitSelection: 7
    };
}
