import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { ApplicationSetting } from "../Entities/ApplicanSetting";
import { environment } from '../../environments/environment';

@Injectable()
export class AppSettingService {

    private readonly ROUTE_DISCOUNT: string = environment.baseUrl + "/api/AppSetting/";
    private readonly API_GET: string = this.ROUTE_DISCOUNT + "Get";
    private readonly API_GET_APPSETTINGS: string = this.ROUTE_DISCOUNT + "GetApplicationSettingsByStoreId";
    private readonly API_Update: string = this.ROUTE_DISCOUNT + "Update";

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
    }

    getSectionSetting(sectionName: string, subSection: string): Observable<Array<any>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        let urlQuery: string = "sectionName=" + sectionName;
        if (subSection != "" && subSection != null) {
            urlQuery = urlQuery + "&subSection=" + subSection;
        }
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

    getStoreByDuplicateId(storeId: number): Observable<any[]> {

        let urlQuery: string = this.API_GET_APPSETTINGS + "?storeId=" + storeId;
        //let paymentMethods: Array<Object> = new Array<Object>();

        return this.http.get(urlQuery, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }


}