import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class ScreenNamesService {

    private readonly ROUTE_SCREENNAME: string = environment.baseUrl + "/api/ScreenNames/";
    private readonly API_GET: string = this.ROUTE_SCREENNAME + "Get";
    public API_LISTSCREENNAMES: Array<any> = null;


    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
    }

    getScreens(): Observable<Array<any>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        return this.http.get(this.API_GET, localReq).map((res: Response) => {
            if (res.json()) {
                this.API_LISTSCREENNAMES = res.json();
                return res.json();

            }
            else Observable.throw(res);
        });
    }
    getScreenNames(): Array<any> {
        return this.API_LISTSCREENNAMES;
    }
    setScreenName(screenName: string): void {
        localStorage.setItem("screenName", screenName);
    }

    getScreenName(): string {
        return localStorage.getItem("screenName");
    }




}