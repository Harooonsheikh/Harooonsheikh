import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable, Observer } from 'rxjs';
import { DimensionSet } from "../Entities/DimensionSet";
import { UserService } from "./user.service";
import { StoreService } from "./store.service";
import { environment } from "../../environments/environment";
import { KeyValuePair } from "../Entities/Common";

@Injectable()
export class EcomTypeService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/Ecom/";
    private readonly API_GET: string = this.API_ROUTE + "Get";


    constructor(private http: Http, private _storeService: StoreService) {
    }

    get(): Observable<Array<KeyValuePair<string>>> {

        let ecomTypes: Array<KeyValuePair<string>> = new Array<KeyValuePair<string>>();
        let urlQuery: string = this.API_GET;
        return this.http.get(urlQuery, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<KeyValuePair<string>>, Object>(ecomTypes, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

}