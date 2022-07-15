import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "./user.service";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import { StoreService } from "./store.service";
import { Jobs } from "../Entities/Jobs";
import { environment } from "../../environments/environment";

@Injectable()
export class JobsService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/Jobs/";
    private readonly API_ROUTE_GET: string = this.API_ROUTE + "Get";
    private readonly API_ROUTE_UPDATE: string = this.API_ROUTE + "Update";

    constructor(
        private http: Http,
        private _uService: UserService,
        private _storeService: StoreService
    ) { }

    get(storeId: string, type: boolean): Observable<Array<Jobs>> {
        let urlQuery: string =
            this.API_ROUTE_GET +
            "?storeId=" +
            storeId +
            "&type=" +
            type;
        let jobsArr: Array<Jobs> = new Array<Jobs>();

        return this.http
            .get(urlQuery, this._storeService.createStoreRequest())
            .map((res: Response) => {

                if (res.json()) {
                    return Object.assign<Array<Jobs>, Object>(jobsArr, res.json());
                } else {
                    Observable.throw(res);
                }
            });
    }

    update(job: Jobs): Observable<Response> {
        let urlQuery: string = this.API_ROUTE_UPDATE;
        return this.http.put(urlQuery, job, this._storeService.createStoreRequest());
    }
}
