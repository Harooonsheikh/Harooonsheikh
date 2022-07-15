import { CatalogProduct } from './../Entities/CatalogProduct';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';
import { CatalogModel } from '../Entities/Common';

@Injectable()
export class CatalogService {

    private readonly ROUTE_CATALOG = environment.baseUrl + "/api/catalog/";
    private readonly API_GETPRODUCTS: string = this.ROUTE_CATALOG + "GetProducts";
    private readonly API_GETMASTERPRODUCTS: string = this.ROUTE_CATALOG + "GetMasterProducts";
    private readonly API_GETPRODUCT: string = this.ROUTE_CATALOG + "GetProduct";
    private readonly API_GETCATALOGCOUNT: string = this.ROUTE_CATALOG + "GetCatalogCount";
    private readonly API_SEARCHRESULTCOUNT: string = this.ROUTE_CATALOG + "SearchResultCount";
    private readonly API_SEARCH: string = this.ROUTE_CATALOG + "Search";
    private readonly API_GETCATEGORY: string = this.ROUTE_CATALOG + "Categories";
    private readonly API_GETCATASS: string = this.ROUTE_CATALOG + "CategoriesAssignments";
    private readonly API_GETCATALOGSTATS: string = this.ROUTE_CATALOG + "CatalogStatisticsByName";

    private req: RequestOptions = null;
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._storeService.createStoreRequest();


    }

    getSearchResultCount(fileName: string, query: string, modelType: number): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName + "&query=" + query + "&model=" + modelType;
        return this.http.get(this.API_SEARCHRESULTCOUNT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getSearchResult(fileName: string, query: string, modelType: number, offSet: number, pageSize: number): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName + "&query=" + query + "&model=" + modelType + "&offSet=" + offSet + "&pageSize=" + pageSize;
        return this.http.get(this.API_SEARCH + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getCatalogModalCount(fileName: string, modal: CatalogModel): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName + "&modal=" + modal;
        return this.http.get(this.API_GETCATALOGCOUNT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return Number.parseInt(res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getProductByCatalog(fileName: string, offSet: number, limit: number): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName + "&offSet=" + offSet + "&pageSize=" + limit;
        return this.http.get(this.API_GETPRODUCTS + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }

    getCatagoriesByCatalog(fileName: string, offSet: number, limit: number): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName + "&offSet=" + offSet + "&pageSize=" + limit;
        return this.http.get(this.API_GETCATEGORY + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }

    getCatagoryAssignmentByCatalog(fileName: string, offSet: number, limit: number): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName + "&offSet=" + offSet + "&pageSize=" + limit;
        return this.http.get(this.API_GETCATASS + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }

    getMasterProductsByCatalog(fileName: string): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETMASTERPRODUCTS + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }
    getProductDetail(fileName: string, prodId: string): Observable<string> {

        let urlQuery: string = "?fileName=" + fileName + "&prodId=" + prodId;
        return this.http.get(this.API_GETPRODUCT + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }

    getCatalogStatistics(fileName: string): Observable<Object> {

        let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GETCATALOGSTATS + urlQuery, this.req).map((res: Response) => {
            if (res.json()) {
                return res.json();
            } else {
                Observable.throw(res);
            }
        });
    }
}