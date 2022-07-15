import { KeyValuePair } from "./Common";
import { ApplicationSetting } from "../Entities/ApplicanSetting";
export class Store {
    public StoreId: number = -1;
    public Name: string = null;
    public EcomType: KeyValuePair<string> = null;
    public ERPType: KeyValuePair<string> = null;
    public OrganizationId: number = null;
    public IsActive: boolean = null;
    public StoreKey: string = null;
    public Description: string = null;
    public CreatedOn: Date = null;
    public CreatedBy: number = null;
    public ModifiedOn: Date = null;
    public ModifiedBy: number = null;
    public DuplicateOf: number = -1;


    public appList: ApplicationSetting[];


    constructor() {
        this.EcomType = new KeyValuePair<string>();
        this.ERPType = new KeyValuePair<string>();
    }
}

export class SyncStorePaymentConnector {
    public StoreId: number = 0;
    public Name: string = null;
    public IsSelected: boolean = true;
    public StoreKey: string = null;
}

export class PaymentConnectorRequest {
    public SynchronizeAll: boolean = false;
    public StoreIds: Array<number> = null;
    constructor() {
        this.StoreIds = new Array<number>();
    }
}
