import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "./user.service";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import { CatalogProduct } from "./../Entities/CatalogProduct";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import {
    WorkFlow,
    CatalogStats
} from "../Entities/WorkFlow";
import { FlowStep, KeyValue } from "../Entities/Common";
import { environment } from "../../environments/environment";
import { Store } from "../Entities/Store";

@Injectable()
export class StoreService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/store/";
    private readonly API_GET: string = this.API_ROUTE + "get";
    private readonly API_GETSTORE: string = this.API_ROUTE + "GetAll";
    private readonly API_DELETESTORE: string = this.API_ROUTE + "Delete";
    private readonly API_DISABLESTORE: string = this.API_ROUTE + "Disable";
    private readonly API_ADDSTORE: string = this.API_ROUTE + "Add";
    private readonly API_UPDATE: string = this.API_ROUTE + "Update";
    private readonly API_VERIFYSTORENAME: string = this.API_ROUTE + "Verify";
    private readonly API_VERIFYSTOREKEY: string = this.API_ROUTE + "VerifyStoreKey";
    private readonly API_GETPaymentConnectorAPIURL: string = this.API_ROUTE + "GetPaymentConnectorAPIURL";

    private req: RequestOptions = null;




    constructor(private http: Http, private _uService: UserService) { }

    public storelist: any[];

    setStore(storeName: string): void {
        localStorage.setItem("currentStore", storeName);
    }
    getStore(): string {
        return localStorage.getItem("currentStore");
    }
    setStoreID(storeId: string): void {
        localStorage.setItem("storeId", storeId);
    }
    setStoreKey(key: string): void {
        localStorage.setItem('storeKey', key);
    }
    getStoreKey(): string {
        return localStorage.getItem('storeKey');
    }

    getStoreID(): string {
        return localStorage.getItem("storeId");
    }

    clearStore(): void {
        localStorage.removeItem("currentStore");
        localStorage.removeItem("storeId");
        localStorage.removeItem("storeKey");
    }


    getStoreQuery(): string {
        return "?store=" + this.getStore();
    }

    getStoreConfig(): Observable<Store> {
        let localReq: RequestOptions = this.req;
        let store: Store = new Store();
        let urlQuery: string = "?storeName=" + name;
        return this.http
            .get(this.API_GET + urlQuery, this._uService.jwt())
            .map((res: Response) => {
                if (res.json()) {
                    return Object.assign<Store, Object>(store, res.json());
                } else Observable.throw(res);
            });
    }
    getAllStores(): Observable<Array<KeyValue<string>>> {
        let localReq: RequestOptions = new RequestOptions();
        localReq.headers = new Headers();
        localReq.headers.append("content-type", "application/json");
        //let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GET, localReq).map((res: Response) => {
            if (res.json()) {
                let lst: Array<KeyValue<string>> = new Array<KeyValue<string>>();
                return Object.assign<Array<KeyValue<string>>, Object>(lst, res.json());
            } else Observable.throw(res);
        });
    }

    getStoreDetail(storeId: number): Observable<Store> {
        let localReq: RequestOptions = new RequestOptions();
        localReq.headers = new Headers();
        localReq.headers.append("content-type", "application/json");
        let urlQuery: string = this.API_GET + "?storeId=" + storeId;
        return this.http.get(urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Store, Object>(new Store(), res.json());
            } else Observable.throw(res);
        });
    }

    get(): Observable<Array<Store>> {

        let urlQuery: string = this.API_GETSTORE;
        let storeConfigMethods: Array<Store> = new Array<
            Store
            >();

        return this.http
            .get(urlQuery, this.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<Array<Store>, Object>(
                        storeConfigMethods,
                        res.json()
                    );
                } else {
                    Observable.throw(res);
                }
            });
    }

    delete(storeId: number, modifiedBy: string): Observable<Response> {
        // this.mainQuery = this.getStoreQuery();
        let urlQuery: string =
            this.API_DELETESTORE +
            "&AppstoreId=" +
            storeId +
            "&ModifiedBy=" +
            modifiedBy;
        return this.http.delete(urlQuery, this._uService.jwt());
    }

    disable(storeId: number): Observable<Response> {
        let urlQuery: string = this.API_DISABLESTORE + "?storeId=" + storeId;
        return this.http.delete(urlQuery, this.createStoreRequest());
    }

    add(storeconfig: Store): Observable<Store> {
        let urlQuery: string = this.API_ADDSTORE; // + this.mainQuery;
        let storeConfiglocal: Store = new Store();

        //storeconfig.duplicatename = storeconfig.DuplicateOf.toString();

        return this.http
            .post(urlQuery, storeconfig, this.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<Store, Object>(
                        storeConfiglocal,
                        res.json()
                    );
                } else {
                    Observable.throw(res);
                }
            });
    }

    update(storeconfig: Store): Observable<Store> {
        let urlQuery: string = this.API_UPDATE; // + this.mainQuery;
        let store: Store = new Store();
        return this.http
            .put(urlQuery, storeconfig, this.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<Store, Object>(
                        store,
                        res.json()
                    );
                } else {
                    Observable.throw(res);
                }
            });
    }


    verifyStoreName(storeName: string, storeId: number): Observable<Response> {
        let urlQuery: string = this.API_VERIFYSTORENAME + "?storeName=" + storeName + "&storeId=" + storeId;
        return this.http.get(urlQuery, this.createStoreRequest());
    }
    verifyStoreKey(storeKey: string, storeId: number): Observable<Response> {
        let urlQuery: string = this.API_VERIFYSTOREKEY + "?storeKey=" + storeKey + "&storeId=" + storeId;
        return this.http.get(urlQuery, this.createStoreRequest());
    }

    getaymentConnectorAPIURL(): Observable<string> {
        return this.http.get(this.API_GETPaymentConnectorAPIURL, this.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    sendPaymentConnectorSyncRequest(apiURL: string, requestBody: string, storeKey: string): Observable<Response> {

        let reHeader: RequestOptions = new RequestOptions();
        reHeader.headers = new Headers();
        reHeader.headers.append("content-type", "application/json");
        reHeader.headers.append("x-api-key", storeKey);

        return this.http.post(apiURL, requestBody, reHeader);
    }

    createStoreRequest(): RequestOptions {
        let localReq: RequestOptions = this._uService.jwt();
        if (!localReq.headers.has('content-type')) {
            localReq.headers.append("content-type", "application/json");
        }
        if (!localReq.headers.has('storeKey')) {
            localReq.headers.append("storeKey", this.getStoreKey());
        }
        return localReq;
    }
}
