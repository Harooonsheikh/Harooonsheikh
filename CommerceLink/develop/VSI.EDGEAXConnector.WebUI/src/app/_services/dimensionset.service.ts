import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable, Observer } from 'rxjs';
import { DimensionSet } from "../Entities/DimensionSet";
import { UserService } from "./user.service";
import { StoreService } from "./store.service";
import { environment } from "../../environments/environment";

@Injectable()
export class DimensionSetService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/DimensionSet/";
    private readonly API_GET: string = this.API_ROUTE + "Get";
    private readonly API_Update: string = this.API_ROUTE + "Update";

    constructor(private http: Http, private _userService: UserService, private _storeService: StoreService) {
    }

    get(): Observable<Array<DimensionSet>> {
        let urlQuery: string = this.API_GET;
        let dimensionSet: Array<DimensionSet> = new Array<DimensionSet>();

        return this.http.get(urlQuery, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<DimensionSet>, Object>(dimensionSet, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }
    update(dimensionSet: DimensionSet): Observable<Response> {
        let urlQuery: string = this.API_Update;
        return this.http.put(urlQuery, dimensionSet, this._storeService.createStoreRequest())
    }
}