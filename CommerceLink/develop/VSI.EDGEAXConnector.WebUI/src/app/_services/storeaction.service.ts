import { Injectable } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "./user.service";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import { ActionParam, Action } from "../Entities/StoreActions";
import { StoreConfigMethod } from "../Entities/StoreConfigMethod";
import { environment } from "../../environments/environment";
import { StoreService } from "./store.service";

@Injectable()
export class ActionService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/action/";
    private readonly API_GET: string = this.API_ROUTE + "Get";
    private readonly API_SAVE: string = this.API_ROUTE + "Save";
    private readonly API_GETAPIURL: string = this.API_ROUTE + "GetAPIURL";
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) { }

    getAPIURL(): Observable<string> {
        return this.http.get(this.API_GETAPIURL, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
    get(actionName: string, storeId: number): Observable<Response> {
        let url: string = this.API_GET + "?actionName=" + actionName + "&storeId=" + storeId;
        return this.http.get(url, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
    save(action: Action): Observable<Response> {
        return this.http.put(this.API_SAVE, action, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
    callAction(
        action: Action
    ): Observable<Response> {
        let rtype: string = action.RequestType.toLowerCase();
        let url: string = action.APIURL + action.ActionRoute; // + "?x-api-key=" + store.APIKey;

        let reHeader: RequestOptions = new RequestOptions();
        reHeader.headers = new Headers();
        reHeader.headers.append("content-type", "application/json");
        reHeader.headers.append("x-api-key", action.APIKey);
        let query: string = "";
        let postQuery: string = "?";
        let isQueryParam = false;
        let body: any = null;
        switch (rtype) {
            case "post":
                action.ActionParams.forEach((m, index) => {
                    if (m.Type.toLowerCase() == "query") {
                        postQuery += m.Key + "=" + m.Value;
                        isQueryParam = true;
                        if (index != action.ActionParams.length - 1) {
                            postQuery += "&";
                        }
                    }
                    else if (m.Type.toLowerCase() == "body") {
                        body = m.Value;
                    }
                });
                if (isQueryParam) {
                    url = url + postQuery;
                }

                return this.http.post(url, body, reHeader);
            case "put":
                action.ActionParams.forEach((m, index) => {
                    if (m.Type.toLowerCase() == "query") {
                        postQuery += m.Key + "=" + m.Value;
                        isQueryParam = true;
                        if (index != action.ActionParams.length - 1) {
                            postQuery += "&";
                        }
                    }
                    else if (m.Type.toLowerCase() == "body") {
                        body = m.Value;
                    }
                });
                if (isQueryParam) {
                    url = url + postQuery;
                }

                return this.http.put(url, body, reHeader);
            case "get":
                if (action.ActionParams.length > 0) {
                    query = "?";
                }
                action.ActionParams.forEach((m, index) => {
                    query += m.Key + "=" + m.Value;
                    if (index < action.ActionParams.length - 1) {
                        query += "&";
                    }
                });
                url = url + query;
                return this.http.get(url, reHeader);
            case "delete":
                if (action.ActionParams.length > 0) {
                    query = "?";
                }
                action.ActionParams.forEach((m, index) => {
                    query += m.Key + "=" + m.Value;
                    if (index < action.ActionParams.length - 1) {
                        query += "&";
                    }
                });
                url = url + query;
                return this.http.delete(url, reHeader);
            default:
                return Observable.of(null);
        }
    }
}
