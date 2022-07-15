import { CatalogProduct } from './../Entities/CatalogProduct';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class InventoryService {

    private readonly ROUTE_INVENTORY: string = environment.baseUrl + "/api/inventory/";
    private readonly API_GETINVENTSTAT: string = this.ROUTE_INVENTORY + "StatisticsByName";
    private readonly API_RECORDCOUNT: string = this.ROUTE_INVENTORY + "GetCount";
    private readonly API_RECORDS: string = this.ROUTE_INVENTORY + "Get";
    private readonly API_SEARCHRECORD: string = this.ROUTE_INVENTORY + "Search";
    private readonly API_SEARCHRECORDCOUNT: string = this.ROUTE_INVENTORY + "SearchCount";

    private req: RequestOptions = null;

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._storeService.createStoreRequest();


    }

    getInventStats(fileName: string): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETINVENTSTAT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getInventoryCount(fileName: string): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETINVENTSTAT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return Number.parseInt(res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getSearchResultCount(fileName: string, query: string): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName + "&query=" + query + "&model=2";
        return this.http.get(this.API_SEARCHRECORDCOUNT + urlQuery, this.req).map((res: Response) => {
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
        return this.http.get(this.API_SEARCHRECORD + urlQuery, this.req).map((res: Response) => {
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
        return this.http.get(this.API_RECORDCOUNT + urlQuery, this.req).map((res: Response) => {
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
        return this.http.get(this.API_RECORDS + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else {
                Observable.throw(res);
            }
        });
    }
}