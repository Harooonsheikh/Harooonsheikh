import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { KeyValue } from '../Entities/Common';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class SaleOrderService {

    private readonly ROUTE: string = environment.baseUrl + "/api/SalesOrder/";
    private readonly API_GET: string = this.ROUTE + "Get";

    private req: RequestOptions = null;
    private mainQuery: string = "";
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._uService.jwt();


    }


    getOrderXML(fileName: string): Observable<any> {
        this.mainQuery = this._storeService.getStoreQuery();
        let localReq: RequestOptions = this.req;
        //localReq.headers = new Headers();
        localReq.headers.append("content-type", "application/json");
        let urlQuery: string = this.mainQuery + "&fileName=" + fileName;
        //let urlQuery: string = "?fileName=" + fileName;
        return this.http.get(this.API_GET + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
}