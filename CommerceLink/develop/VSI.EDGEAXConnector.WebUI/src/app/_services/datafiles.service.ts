import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class DataFileService {

    private readonly ROUTE_DISCOUNT: string = environment.baseUrl + "/api/ScreenNames/";
    private readonly API_GET: string = this.ROUTE_DISCOUNT + "Get";
    private readonly API_Update: string = this.ROUTE_DISCOUNT + "Update";


    private req: RequestOptions = null;
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._uService.jwt();
    }

    getScreenNameData(screenName: string): Observable<Array<any>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        localReq.headers.append("content-type", "application/json");
        let urlQuery: string = "?screenName=" + screenName;
        return this.http.get(this.API_GET + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
    setAppSettings(settings: Array<any>): Observable<boolean> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();

        return this.http.post(this.API_Update, settings, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

}