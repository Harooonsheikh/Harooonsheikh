import { KeyValue, KeyValuePair } from "./Common";

export class AppUser {
    constructor() {
        this.UserRoles = new Array<KeyValue<string>>();
        this.AppStores = new Array<KeyValuePair<string>>();
    }
    public UserId: string = null;
    public FirstName: string = null;
    public LastName: string = null;
    public UserName: string = null;
    public Email: string = null;
    public EmailConfirmed: boolean = false;
    public Password: string = null;
    public UserRoles: Array<KeyValue<string>> = null;
    public AppStores: Array<KeyValuePair<string>> = null;
}

