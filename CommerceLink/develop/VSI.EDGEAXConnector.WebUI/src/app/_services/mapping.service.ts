import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserService } from "./user.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { KeyValue } from '../Entities/Common';
import { StoreService } from './store.service';
import { XMLMap } from '../Entities/XMLMap';
import { environment } from '../../environments/environment';

@Injectable()
export class MappingService {

    private readonly ROUTE: string = environment.baseUrl + "/api/map/";
    private readonly API_SOURCEENTITIES: string = this.ROUTE + "Entities";
    private readonly API_SOURCEPROPERTIES: string = this.ROUTE + "Properties";
    private readonly API_ACTIONS: string = this.ROUTE + "Actions";
    private readonly API_TEMPLATESXML: string = this.ROUTE + "TemplateXML";
    private readonly API_SAVE: string = this.ROUTE + "Save";
    private readonly API_READXML: string = this.ROUTE + "ReadXML";
    private readonly API_TEMPLATES: string = this.ROUTE + "Templates";
    private readonly API_GET: string = this.ROUTE + "Get";
    private readonly API_DELETE: string = this.ROUTE + "Delete";
    private readonly API_BACKUP: string = this.ROUTE + "Backup";

    public UI_SELECTEDMAPINDEX: number = 0;

    constructor(private http: Http, private _uService: UserService, private _storeService: StoreService) {
    }

    getSourceEntities(): Observable<Array<any>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        return this.http.get(this.API_SOURCEENTITIES, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getSourceProperties(entity: string): Observable<Array<any>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        let urlQuery: string = "?entity=" + entity;
        return this.http.get(this.API_SOURCEPROPERTIES + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getActions(generateXML: boolean): Observable<Array<KeyValue<string>>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        let urlQuery: string = "?generateXML=" + generateXML;
        return this.http.get(this.API_ACTIONS + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getTemplatesXML(): Observable<Array<KeyValue<string>>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        return this.http.get(this.API_TEMPLATESXML, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getXML(templateName: string): Observable<string> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        localReq.responseType = ResponseContentType.Json;
        let urlQuery: string = "?templateName=" + templateName;
        return this.http.get(this.API_READXML + urlQuery, localReq).map((res: Response) => {

            if (res.statusText == 'OK') {
                return res.text();
            }
            else Observable.throw(res);
        });
    }


    getTemplates(): Observable<Array<string>> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        localReq.responseType = ResponseContentType.Json;
        return this.http.get(this.API_TEMPLATES, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }
    Save(map: XMLMap): Observable<string> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        return this.http.post(this.API_SAVE, map, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    getMap(templateName: string): Observable<XMLMap> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        let urlQuery: string = "?templateName=" + templateName;
        return this.http.get(this.API_GET + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return Object.assign(new XMLMap(), res.json());
            }
            else Observable.throw(res);
        });
    }

    delete(templateName: string): Observable<boolean> {
        let localReq: RequestOptions = this._storeService.createStoreRequest();
        let urlQuery: string = "?templateName=" + templateName;
        return this.http.get(this.API_DELETE + urlQuery, localReq).map((res: Response) => {
            if (res.json()) {
                return res.json();
            }
            else Observable.throw(res);
        });
    }

    backup(): Observable<Response> {

        let localReq: RequestOptions = this._storeService.createStoreRequest();
        localReq.responseType = ResponseContentType.Blob;
        localReq.headers.delete("content-type");
        return this.http.get(this.API_BACKUP, localReq).map((res: Response) => {
            if (res) {
                return res;
            }
            else Observable.throw(res);
        });
    }


}