import { CatalogProduct } from './../Entities/CatalogProduct';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { WorkFlow, CatalogStats } from "../Entities/WorkFlow";
import { FlowStep, KeyValue } from '../Entities/Common';
import { StoreService } from './store.service';
import { PaymentMethod } from '../Entities/PaymentMethod';
import { environment } from '../../environments/environment';

@Injectable()
export class PaymentMethodService {

    private readonly API_PAYMENTMETHOD: string = environment.baseUrl + "/api/PaymentMethod/";
    private readonly API_GETPAYMENTMETHODS: string = this.API_PAYMENTMETHOD + "Get";
    private readonly API_DELETEPAYMENTMETHOD: string = this.API_PAYMENTMETHOD + "Delete";
    private readonly API_ADDPAYMENTMETHOD: string = this.API_PAYMENTMETHOD + "Add";
    private readonly API_UPDATEPAYMENTMETHOD: string = this.API_PAYMENTMETHOD + "Update";

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
    }

    get(): Observable<Array<PaymentMethod>> {

        let urlQuery: string = this.API_GETPAYMENTMETHODS;
        let paymentMethods: Array<PaymentMethod> = new Array<PaymentMethod>();
        return this.http.get(urlQuery, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<PaymentMethod>, Object>(paymentMethods, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    delete(paymentMethodId: number): Observable<Response> {
        let urlQuery: string = this.API_DELETEPAYMENTMETHOD + "?paymentMethodId=" + paymentMethodId;
        return this.http.delete(urlQuery, this._storeService.createStoreRequest());
    }

    add(paymentmethod: PaymentMethod): Observable<PaymentMethod> {
        let urlQuery: string = this.API_ADDPAYMENTMETHOD;
        let paymentMethodlocal: PaymentMethod = new PaymentMethod();
        return this.http.post(urlQuery, paymentmethod, this._storeService.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<PaymentMethod, Object>(paymentMethodlocal, res.json());
                }
                else {
                    Observable.throw(res);
                }
            });
    }

    update(paymentmethod: PaymentMethod): Observable<PaymentMethod> {
        let urlQuery: string = this.API_UPDATEPAYMENTMETHOD;
        let paymentMethodlocal: PaymentMethod = new PaymentMethod();
        return this.http.put(urlQuery, paymentmethod, this._storeService.createStoreRequest())
            .map((res: Response) => {
                if (res.json()) {
                    return Object.assign<PaymentMethod, Object>(paymentMethodlocal, res.json());
                }
                else {
                    Observable.throw(res);
                }
            });
    }
}