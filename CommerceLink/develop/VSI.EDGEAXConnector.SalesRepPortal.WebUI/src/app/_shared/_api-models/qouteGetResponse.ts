export class GetQouteResponse{
    AddressRecordId: string;
    ChannelReferenceId: string;
    CreationDateString: string;
    CurrencyCode: string;
    CustomerAccount: string;
    CustomerRecordId: string;
    Email: string;
    QuotationId: string;
    SalespersonName: string;
    SalespersonStaffId: string;
    TotalNetAmount: string;
    TotalDiscount: string;
    TotalPrice: string;
    TotalTax: string;    
    TMVMAINOFFERTYPE: string;
    TMVPRODUCTFAMILY: string;
    TMVSALESORDERSUBTYPE: string;
    TMVOFFERTYPE: string;
    TMVBILLINGINTERVAL: string;
    TMVCONTRACTENDDATE: string;
    SHIPPINGDATEREQUESTED: string;
    CONTACTPERSONID: string;
    SALESID: string;
}

export class TestCart{
    Items: TestItems[] = [];
}

export class TestItems{
    sku: string;
}
