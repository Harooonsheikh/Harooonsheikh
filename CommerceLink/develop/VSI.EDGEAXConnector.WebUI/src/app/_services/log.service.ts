import { UserService } from './user.service';
import { TransactionLog } from './../Entities/TransactionLog';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { environment } from '../../environments/environment';

@Injectable()
export class LogsService {
    private readonly API_GETTRANSACTIONLOGS: string = environment.baseUrl + "/api/log/GetLogs";
    private readonly API_GETLOGS: string = environment.baseUrl + "/api/log/Get";
    private req: RequestOptions = null;
    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
        this.req = this._storeService.createStoreRequest();
    }

    getTransactionLogs(instanceId: string): Observable<Array<TransactionLog>> {

        let query: string = "?InstanceId=" + instanceId;
        return this.http.get(this.API_GETTRANSACTIONLOGS + query, this.req).map((res: Response) => {
            if (res.json()) {
                let logs: Array<TransactionLog> = new Array<TransactionLog>();
                return Object.assign<Array<TransactionLog>, Object>(logs, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    getLogs(daysCount: number): Observable<Array<any>> {
        let query: string = "?daysCount=" + daysCount;
        return this.http.get(this.API_GETLOGS + query, this.req).map((res: Response) => {
            if (res.json()) {
                let logs: Array<any> = new Array<any>();
                return Object.assign<Array<any>, Object>(logs, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }
}