import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { KeyValue } from '../Entities/Common';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class EntityService {

    private readonly ROUTE_ENTITY: string = environment.baseUrl + "/api/Entity/";

    private readonly API_READENTITY: string = this.ROUTE_ENTITY + "get";
    private readonly API_SALEORDERENTITY: string = this.ROUTE_ENTITY + "SaleOrderEntities";
    private readonly API_MERCHANDIZEENTITY: string = this.ROUTE_ENTITY + "MerchandizingEntities";

    private req: RequestOptions = null;

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._storeService.createStoreRequest();


    }

    get(): Observable<Array<KeyValue<string>>> {

        return this.http.get(this.API_READENTITY, this.req).map((res: Response) => {
            if (res.json()) {
                let result = Object.assign<Array<KeyValue<string>>, Object>(new Array<KeyValue<string>>(), res.json());
                return result;
            }
            else Observable.throw(res);
        });
    }

    getSaleOrderEntity(): Observable<Array<KeyValue<string>>> {

        return this.http.get(this.API_SALEORDERENTITY, this.req).map((res: Response) => {
            if (res.json()) {
                let result = Object.assign<Array<KeyValue<string>>, Object>(new Array<KeyValue<string>>(), res.json());
                return result;
            }
            else Observable.throw(res);
        });
    }

    getMerchandizeEntity(): Observable<Array<KeyValue<string>>> {

        return this.http.get(this.API_MERCHANDIZEENTITY, this.req).map((res: Response) => {
            if (res.json()) {
                let result = Object.assign<Array<KeyValue<string>>, Object>(new Array<KeyValue<string>>(), res.json());
                return result;
            }
            else Observable.throw(res);
        });
    }


}