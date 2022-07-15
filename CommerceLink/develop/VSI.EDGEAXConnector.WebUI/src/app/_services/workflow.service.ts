import { CatalogProduct } from "./../Entities/CatalogProduct";
import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { UserService } from "./user.service";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import {
    WorkFlow,
    CatalogStats,
    WorkflowTransition
} from "../Entities/WorkFlow";
import { FlowStep, KeyValue, KeyValuePair } from "../Entities/Common";
import { StoreService } from "./store.service";
import { WorkflowState } from "../Entities/WorkflowState";
import { environment } from "../../environments/environment";

@Injectable()
export class WorkFlowService {
    private readonly API_ROUTE: string = environment.baseUrl + "/api/Workflow/";
    private readonly API_GETWORKFLOWSTATES: string = this.API_ROUTE + "States";
    private readonly API_GETWORKFLOWTRANSITION: string = this.API_ROUTE +
    "Transition";
    private readonly API_WFSTATS: string = this.API_ROUTE + "Statistic";

    private readonly API_WORKFLOWCOUNT: string = this.API_ROUTE + "Count";
    private readonly API_WORKFLOWS: string = this.API_ROUTE + "Get";
    private readonly API_WORKFLOWSREALTIMESTATUS: string = this.API_ROUTE +
    "GetWorkFlowStatusRealTime";
    private readonly API_SEARCH: string = this.API_ROUTE + "Search";
    private readonly API_SEARCHCOUNT: string = this.API_ROUTE + "SearchCount";
    private readonly API_GETFILESTATUS: string = this.API_ROUTE + "WorkflowStatus";

    private req: RequestOptions = null;
    public entityId: number = -1;
    public daysCount: number = 7;
    public searchQuery: string = "";
    public statusFilter: FlowStep = null;
    public offSet: number = -1;
    public pageSize: number = -1;
    public pageNumber: number = -1;

    constructor(
        private http: Http,
        private _uService: UserService,
        private _storeService: StoreService
    ) {
        this.req = this._storeService.createStoreRequest();
    }

    //////

    private createQueryFromList<T>(
        list: Array<T>,
        valueKey: string,
        arrayName: string
    ) {
        let query: string = "";
        list.forEach((item: T, index: number) => {
            let currentItem: T = item;
            query =
                query +
                arrayName +
                "[" +
                index +
                "]=" +
                currentItem[valueKey].toString();
            if (index < list.length - 1) {
                query = query + "&";
            }
        });
        return query;
    }

    getStatistics(
        days: number,
        requiredEntities: Array<KeyValue<string>>
    ): Observable<Object> {
        let urlQuery: string = "?";
        if (days != 0) {
            urlQuery = urlQuery + "daysCount=" + days + "&";
        }
        urlQuery =
            urlQuery +
            this.createQueryFromList<KeyValue<string>>(
                requiredEntities,
                "Key",
                "entitiesArray"
            );

        return this.http
            .get(this.API_WFSTATS + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return res.json();
                } else {
                    Observable.throw(res);
                }
            });
    }

    getWorkflowCount(
        step: FlowStep,
        requiredEntities: Array<KeyValue<string>>,
        days: number
    ): Observable<number> {
        let urlQuery: string = "?";
        if (step != null) {
            urlQuery = urlQuery + "workFlowStepStatus=" + step.valueOf() + "&";;
        }

        if (days != 0) {
            urlQuery = urlQuery + "daysCount=" + days + "&";
        }
        urlQuery =
            urlQuery +
            this.createQueryFromList<KeyValue<string>>(
                requiredEntities,
                "Key",
                "entitiesArray"
            );

        return this.http
            .get(this.API_WORKFLOWCOUNT + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return Number.parseInt(res.json());
                } else {
                    Observable.throw(res);
                }
            });
    }

    getWorkflows(
        offSet: number,
        pageSize: number,
        step: FlowStep,
        requiredEntities: Array<KeyValue<string>>,
        days: number
    ): Observable<string> {
        this.offSet = offSet;
        this.pageSize = pageSize;
        let urlQuery: string = "?offSet=" + offSet + "&pageSize=" + pageSize;
        if (step != null) {
            urlQuery = urlQuery + "&workFlowStepStatus=" + step.valueOf();
        }

        if (days != 0) {
            urlQuery = urlQuery + "&daysCount=" + days + "&";
        }
        urlQuery =
            urlQuery +
            this.createQueryFromList<KeyValue<string>>(
                requiredEntities,
                "Key",
                "entitiesArray"
            );

        return this.http
            .get(this.API_WORKFLOWS + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return res.json();
                } else {
                    Observable.throw(res);
                }
            });
    }

    getSearchResultCount(
        fileName: string,
        step: FlowStep,
        requiredEntities: Array<KeyValue<string>>,
        days: number
    ): Observable<number> {

        let urlQuery: string = "?fileName=" + fileName;
        if (step != null) {
            urlQuery = urlQuery + "&workFlowStepStatus=" + step.valueOf();
        }

        if (days != 0) {
            urlQuery = urlQuery + "&daysCount=" + days + "&";
        }
        urlQuery =
            urlQuery +
            this.createQueryFromList<KeyValue<string>>(
                requiredEntities,
                "Key",
                "entitiesArray"
            );

        return this.http
            .get(this.API_SEARCHCOUNT + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return Number.parseInt(res.json());
                } else {
                    Observable.throw(res);
                }
            });
    }

    getSearchResult(
        fileName: string,
        offSet: number,
        pageSize: number,
        step: FlowStep,
        requiredEntities: Array<KeyValue<string>>,
        days: number
    ): Observable<string> {
        this.offSet = offSet;
        this.pageSize = pageSize;
        let urlQuery: string =
            "?fileName=" +
            fileName +
            "&offSet=" +
            offSet +
            "&pageSize=" +
            pageSize;
        if (step != null) {
            urlQuery = urlQuery + "&workFlowStepStatus=" + step.valueOf();
        }

        if (days != 0) {
            urlQuery = urlQuery + "&daysCount=" + days + "&";
        }
        urlQuery =
            urlQuery +
            this.createQueryFromList<KeyValue<string>>(
                requiredEntities,
                "Key",
                "entitiesArray"
            );

        return this.http
            .get(this.API_SEARCH + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return res.json();
                } else {
                    Observable.throw(res);
                }
            });
    }


    getWorkFlowByFileName(
        fileName: string,
        showHistory: boolean
    ): Observable<string> {

        let WorkFlowStepWithStatus: WorkFlow = new WorkFlow();
        let urlQuery: string =
            "?fileName=" +
            fileName +
            "&showHistory=" +
            showHistory;
        return this.http
            .get(this.API_WORKFLOWSREALTIMESTATUS + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return res.json();
                } else {
                    Observable.throw(res);
                }
            });
    }

    getWorkFlowByFile(
        fileName: string
    ): Observable<Array<WorkflowTransition>> {

        let workFlow: Array<WorkflowTransition> = new Array<WorkflowTransition>();
        let urlQuery: string = "?fileName=" + fileName;

        return this.http
            .get(this.API_WORKFLOWS + urlQuery, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    let array: Array<any> = res.json();
                    workFlow = array.map(p => {
                        let transition: WorkflowTransition = new WorkflowTransition();
                        transition.Created = p["Created"];
                        transition.StateID = p["StateID"];
                        transition.InstanceID = p["InstanceID"];
                        transition.WorkFlowTransitionID = p["WorkFlowTransitionID"];
                        return transition;
                    });
                    return workFlow;
                } else {
                    Observable.throw(res);
                }
            });
    }

    states(entity: number): Observable<Array<WorkflowState>> {

        let query = "?entity=" + entity;

        return this.http
            .get(this.API_GETWORKFLOWSTATES + query, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    let array: any = res.json();
                    let states: Array<WorkflowState> = new Array<WorkflowState>();
                    states = array.map(element => {
                        return new WorkflowState(element["Name"], element["Value"], element["Display"]);
                    });
                    return states;
                } else {
                    Observable.throw(res);
                }
            });
    }

    transition(
        entityId: number,
        fileName: string
    ): Observable<Array<WorkflowTransition>> {

        let query =
            "?entityId=" +
            entityId +
            "&fileName=" +
            fileName;

        return this.http
            .get(this.API_GETWORKFLOWTRANSITION + query, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    let array: any = res.json();
                    let states: Array<WorkflowTransition> = new Array<
                        WorkflowTransition
                        >();
                    states = array.map(element => {
                        return new WorkflowState(
                            element["Name"],
                            element["Value"],
                            element["Display"]
                        );
                    });
                    return states;
                } else {
                    Observable.throw(res);
                }
            });
    }

    getFileStatus(
        workflowName: Array<number>
    ): Observable<Array<KeyValuePair<string>>> {

        let urlQuery: string = this.API_GETFILESTATUS;
        let workflowStatus: Array<KeyValuePair<string>> = new Array<KeyValuePair<string>>();
        return this.http
            .post(urlQuery, workflowName, this.req)
            .map((res: Response) => {
                if (res.json()) {
                    return Object.assign<Array<KeyValuePair<string>>, Object>(
                        workflowStatus,
                        res.json()
                    );
                } else {
                    Observable.throw(res);
                }
            });
    }
}
