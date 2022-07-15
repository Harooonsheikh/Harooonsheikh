import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class DiscountService {

    private readonly ROUTE_DISCOUNT: string = environment.baseUrl + "/api/discount/";
    private readonly API_GETDISCOUNTSTAT: string = this.ROUTE_DISCOUNT + "Statistics";
    private readonly API_GETSEARCH: string = this.ROUTE_DISCOUNT + "Search";
    private readonly API_GETSEARCHCOUNT: string = this.ROUTE_DISCOUNT + "SearchCount";
    private readonly API_GETREC: string = this.ROUTE_DISCOUNT + "Get";
    private readonly API_GETRECCOUNT: string = this.ROUTE_DISCOUNT + "GetCount";

    private req: RequestOptions = null;
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._storeService.createStoreRequest();


    }

    getDiscountStats(fileName: string): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETDISCOUNTSTAT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }


    getSearchResultCount(fileName: string, query: string): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName + "&query=" + query + "&model=" + 2;
        return this.http.get(this.API_GETSEARCHCOUNT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return Number.parseInt(res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getSearchResult(fileName: string, query: string, offSet: number, pageSize: number): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName + "&query=" + query + "&model=2" + "&offSet=" + offSet + "&pageSize=" + pageSize;
        return this.http.get(this.API_GETSEARCH + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return (res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getRecordCount(fileName: string): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETRECCOUNT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return Number.parseInt(res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getRecords(fileName: string, offSet: number, pageSize: number): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName + "&offSet=" + offSet + "&pageSize=" + pageSize;
        return this.http.get(this.API_GETREC + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else {
                Observable.throw(res);
            }
        });
    }



}