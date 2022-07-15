import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { FlowStep, KeyValue } from '../Entities/Common';
import { StoreService } from './store.service';
import { ConfigurableObject } from '../Entities/ConfigurableObject';
import { environment } from '../../environments/environment';

@Injectable()
export class ConfigObjectService {

    private readonly API_ROUTE: string = environment.baseUrl + "/api/ConfigObjects/";
    private readonly API_GETCONFIGOBJECTS: string = this.API_ROUTE + "Get";
    private readonly API_UPDATECONFIGOBJECTS: string = this.API_ROUTE + "Update";

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
    }

    private createQueryFromList<T>(list: Array<T>, valueKey: string, arrayName: string) {
        let query: string = "";
        list.forEach((item: T, index: number) => {
            let currentItem: T = item;
            query = query + arrayName + "[" + index + "]=" + (currentItem[valueKey]).toString();
            if (index < list.length - 1) {
                query = query + "&";
            }
        });
        return query;
    }

    get(): Observable<Array<ConfigurableObject>> {
        let configObjectsArr: Array<ConfigurableObject> = new Array<ConfigurableObject>();
        return this.http.get(this.API_GETCONFIGOBJECTS, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<ConfigurableObject>, Object>(configObjectsArr, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    update(configObject: ConfigurableObject): Observable<ConfigurableObject> {
        let configObjectlocal: ConfigurableObject = new ConfigurableObject();
        return this.http.put(this.API_UPDATECONFIGOBJECTS, configObject, this._storeService.createStoreRequest())
            .map((res: Response) => {
                if (res.json()) {
                    return Object.assign<ConfigurableObject, Object>(configObjectlocal, res.json());
                }
                else {
                    Observable.throw(res);
                }
            });
    }
}