import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { StoreService } from './store.service';
import { EmailTemplate } from '../Entities/EmailTemplate';
import { EmailSubscriber, Subscriber } from '../Entities/Subscriber';
import { environment } from '../../environments/environment';

@Injectable()
export class EmailSettingsService {

    private readonly API_ROUTE: string = environment.baseUrl + "/api/EmailSettings/";
    private readonly API_GETSUBSCRIBER: string = this.API_ROUTE + "GetSubscribers";
    private readonly API_ADDEMAILTEMPLATE: string = this.API_ROUTE + "AddEmailTemplate";
    private readonly API_DELETEMAILTEMPLATE: string = this.API_ROUTE + "DeleteEmailTemplate";
    private readonly API_UPDATEEMAILTEMPLATE: string = this.API_ROUTE + "UpdateEmailTemplate";
    private readonly API_UPDATESUBSCRIBER: string = this.API_ROUTE + "UpdateSubscriber";
    private readonly API_ADDSUBSCRIBER: string = this.API_ROUTE + "AddSubscriber";
    private readonly API_DELETESUBSCRIBER: string = this.API_ROUTE + "DeleteSubscriber";
    private readonly API_GETEMAILTEMPLATE: string = this.API_ROUTE + "GetEmailTemplates";

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {

    }

    GetEmailTemplates(): Observable<Array<EmailTemplate>> {

        let emailTemplates: Array<EmailTemplate> = new Array<EmailTemplate>();
        return this.http.get(this.API_GETEMAILTEMPLATE, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<EmailTemplate>, Object>(emailTemplates, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    DeleteEmailTemplate(templateToDelete: number): Observable<Response> {
        let urlQuery: string = this.API_DELETEMAILTEMPLATE + "?templateToDelete=" + templateToDelete;
        return this.http.delete(urlQuery, this._storeService.createStoreRequest());
    }

    AddEmailTemplate(emailTemplate: EmailTemplate): Observable<EmailTemplate> {
        let urlQuery: string = this.API_ADDEMAILTEMPLATE;
        let tempEmailTemplate: EmailTemplate = new EmailTemplate();
        return this.http.post(urlQuery, emailTemplate, this._storeService.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<EmailTemplate, Object>(tempEmailTemplate, res.json());
                }
                else {
                    Observable.throw(res);
                }
            });
    }

    UpdateEmailTemplate(emailTemplate: EmailTemplate): Observable<Response> {
        let urlQuery: string = this.API_UPDATEEMAILTEMPLATE;
        return this.http.put(urlQuery, emailTemplate, this._storeService.createStoreRequest());
    }

    GetSubscribers(): Observable<Array<Subscriber>> {
        let subscribers: Array<Subscriber> = new Array<Subscriber>();
        return this.http.get(this.API_GETSUBSCRIBER, this._storeService.createStoreRequest()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<Subscriber>, Object>(subscribers, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    UpdateSubscriber(subsriber: Subscriber): Observable<Response> {
        let urlQuery: string = this.API_UPDATESUBSCRIBER;
        return this.http.put(urlQuery, subsriber, this._storeService.createStoreRequest());
    }
    AddSubscriber(subscriber: Subscriber): Observable<Subscriber> {
        let urlQuery: string = this.API_ADDSUBSCRIBER;
        let sub: Subscriber = new Subscriber();
        return this.http.post(urlQuery, subscriber, this._storeService.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<Subscriber, Object>(sub, res.json());
                }
                else {
                    Observable.throw(res);
                }
            });
    }
    DeleteSubscriber(subsriber: Subscriber): Observable<Response> {
        let urlQuery: string = this.API_DELETESUBSCRIBER;
        return this.http.post(urlQuery, subsriber, this._storeService.createStoreRequest());
    }
}