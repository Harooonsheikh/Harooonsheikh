import { ScriptLoaderService } from "./../../../../../_services/script-loader.service";
import { Router, ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs/Subject";
import { CatalogService } from "./../../../../../_services/catalog.service";
import { WorkFlowService } from "./../../../../../_services/workflow.service";
import { KeyValue, KeyValuePair } from "./../../../../../Entities/Common";
import { Observable, Subscription } from "rxjs";
import { Component, OnInit, AfterViewInit, Input, OnDestroy } from "@angular/core";
import {

    WorkFlow
} from "../../../../../Entities/WorkFlow";
import { FlowStep } from "../../../../../Entities/Common";
import { forkJoin } from "rxjs/observable/forkJoin";
import { EntityService } from "../../../../../_services/entity.service";
import { timer } from "rxjs/observable/timer";
declare var jquery: any;
declare var $: any;

@Component({
    selector: "workflow-table",
    templateUrl: "workflowtable.component.html"
})
export class MerchandiseTableComponent implements OnInit, AfterViewInit, OnDestroy {

    @Input() public filterBy: Subject<string>;
    @Input() filterByDayCountSubject: Subject<number>;
    @Input() allEntitiesSubject: Subject<Array<KeyValue<string>>>;
    @Input() public timeOut: number;
    public totalRows: number = null;
    private workflowsOb: Observable<Object> = null;
    public workflows: Array<WorkFlow> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<WorkFlow> = null;
    public selectedJobId: Subject<number> = null;
    public currentFilter: FlowStep = null;
    public filterResults: Array<WorkFlow> = null;
    public currentEntityId: number = 0;
    public entities: Array<KeyValue<string>> = null;
    public defaultSetting: boolean = null;
    private timer: Observable<number> = null;
    private workflowStatusIDArr: Array<number> = null;
    private workflowStatus: Array<KeyValuePair<string>> = null;
    private timerSubscription: Subscription = null;

    public daysCount: number = 7;

    constructor(
        private _wfService: WorkFlowService,
        private _script: ScriptLoaderService,
        private route: ActivatedRoute,
        private router: Router,
        private _eService: EntityService
    ) {
        this.defaultSetting = true;
        this.workflowStatusIDArr = new Array<number>();
        this.workflowStatus = new Array<KeyValuePair<string>>();
    }

    ngOnInit() {
        this.allEntitiesSubject.subscribe(m => {
            this.entities = m;
            this.startProcess();
            this.timer = Observable.timer(0, this.timeOut);
            this.timerSubscription = this.timer.subscribe((time: number) => this.getFileStatus());
        });
    }

    ngAfterViewInit() {
    }

    getFileStatus() {
        var self: MerchandiseTableComponent = this;
        self.workflowStatusIDArr = null;
        self.workflowStatusIDArr = new Array<number>();
        $("#fileLog").find("tbody > tr").each(function(row, index) {
            var fileName = $(this).find("td:nth-child(2) span").html();
            var workFlowStatus = $(this).find("td:nth-child(5) span span").html();
            var workFlowtStatusId = $(this).find("td:nth-child(6) span").html();
            if (fileName != "" && (workFlowStatus == "Processing" || workFlowStatus == undefined)) {
                self.workflowStatusIDArr.push(workFlowtStatusId);
            }
        })
        if (self.workflowStatusIDArr.length > 0) {
            self._wfService.getFileStatus(self.workflowStatusIDArr).subscribe(res => {
                self.workflowStatus = null;
                self.workflowStatus = new Array<KeyValuePair<string>>();
                self.workflowStatus = res;
                this.updateStatus(self.workflowStatus);
            });
        }
    }
    private updateStatus(wkfStatus: Array<KeyValuePair<string>>): void {
        var self: MerchandiseTableComponent = this;
        if (wkfStatus != null) {
            wkfStatus.forEach((m, index) => {
                $("#fileLog").find("tbody > tr").each(function(row, index) {
                    var workflowStatusID = $(this).find("td:nth-child(6) span").html();
                    if (workflowStatusID == m.Key) {

                        var selectedClass: string = "";
                        switch (m.Value) {
                            case "Processing":
                                selectedClass = "m-badge--brand";
                                break;

                            case "Success":
                                selectedClass = " m-badge--success";
                                break;

                            case "Failure":
                                selectedClass = " m-badge--danger";
                                break;
                        }
                        var fileName = $(this).find("td:nth-child(5) span").html("<a href='#'><span class=\"m-badge " +
                            selectedClass +
                            ' m-badge--wide">' +
                            m.Value +
                            "</span></a>");
                    }
                });
            });
        }
    }
    public startProcess() {

        this.daysCount = this._wfService.daysCount == 7 ? this.daysCount : this._wfService.daysCount;
        this.searchQuery = this._wfService.searchQuery == "" ? this.searchQuery : this._wfService.searchQuery;
        this.currentEntityId = this._wfService.entityId == -1 ? this.currentEntityId : this._wfService.entityId;
        this.currentFilter = this._wfService.statusFilter == null ? this.currentFilter : this._wfService.statusFilter;

        let entityKeys = this.entities.map(m => parseInt(m.Key));
        this.createTable();
        this.bindEvents();
        this.setupProductTable();
        this.filterBy.subscribe(m =>
            this.filterByStatus(m));
        this.selectedJobId = new Subject<number>();
        this.selectedJobId.subscribe(m =>
            this.filterByJob(m));
        this.filterByDayCountSubject.subscribe((m: number) =>
            this.filterByDay(m)
        );
    }

    public setFilter(jobId: number) {
        this.selectedJobId.next(jobId);
        this._wfService.entityId = jobId;
        this.resetPageNumber();
        return false;
    }

    public filterByDay(days: number) {
        this.daysCount = days;
        this.resetPageNumber();
        this.setupProductTable();
    }

    public filterByJob(jobId: number) {
        this.currentEntityId = jobId;
        this.setupProductTable();
    }

    public filterByStatus(m: string) {
        if (m == "Processing") {
            this.currentFilter = FlowStep.Processing;
        } else if (m == "Success") {
            this.currentFilter = FlowStep.Success;
        } else if (m == "Failure") {
            this.currentFilter = FlowStep.Failure;
        } else {
            this.currentFilter = null;
        }
        this._wfService.statusFilter = this.currentFilter;
        this.resetPageNumber();
        this.setupProductTable();
    }
    public createTable(): void {
        if (this.dataTable == null) {
            var self: MerchandiseTableComponent = this;
            this.workflows = new Array<WorkFlow>();
            for (var i = 0; i < 10; i++) {
                var element = new WorkFlow();
                this.workflows.push(element);
            }
            let pageSize = this._wfService.pageSize == -1 ? 10 : this._wfService.pageSize;
            this.dataTable = $("#fileLog").mDatatable({
                data: {
                    type: "local",
                    source: this.workflows,
                    pageSize: pageSize
                },
                layout: {
                    theme: "default",
                    class: "",
                    scroll: !1,
                    height: 450,
                    footer: !1
                },

                filterable: !1,
                pagination: !0,
                align: "center",
                columns: [
                    {
                        field: "EntityId",
                        title: "Type",
                        width: 150,
                        template: function(e: any) {
                            var type = "";
                            var nu = parseInt(e.EntityId);
                            if (nu === 0) {
                                return "";
                            }
                            if (nu === 2) {
                                type = "Catalog Sync";
                            } else if (nu === 6) {
                                type = "Inventory Sync";
                            } else if (nu === 12) {
                                type = "Price Sync";
                            } else if (nu === 10) {
                                type = "Price Sync";
                            } else if (nu === 14) {
                                type = "Price Sync";
                            } else if (nu === 13) {
                                type = "Discount Sync";
                            } else {
                                type = e.JobId;
                            }

                            return type;
                        }
                    },
                    {
                        field: "InstanceName",
                        title: "File Name",
                        width: 250,
                        responsive: {
                            visible: "lg"
                        }
                    },
                    {
                        field: "new Date(e.Created).toLocaleDateString()",
                        title: "Date",
                        textAlign: "center",
                        width: 100,
                        template: function(e: any): string {
                            if (e.Created == null) {
                                return "";
                            }
                            return new Date(e.Created).toLocaleDateString();
                        }
                    },
                    {
                        field: "new Date(Created).toLocaleTimeString()",
                        title: "Time",
                        textAlign: "center",
                        width: 100,
                        template: function(e: any): string {
                            if (e.Created == null) {
                                return "";
                            }
                            return new Date(e.Created).toLocaleTimeString();
                        }
                    },
                    {
                        field: "Statictics",
                        title: "Status",
                        width: 90,
                        textAlign: "center",
                        filterable: true,
                        template: function(e: any) {
                            //return "Updating...";
                            var title: string = e.Status;
                            var selectedClass: string = "";
                            switch (title) {
                                case "Processing":
                                    selectedClass = "m-badge--brand";
                                    break;

                                case "Success":
                                    selectedClass = " m-badge--success";
                                    break;

                                case "Failure":
                                    selectedClass = " m-badge--danger";
                                    break;
                            }
                            if (selectedClass == "") {
                                return title;
                            }
                            return (
                                "<a href='#'><span class=\"m-badge " +
                                selectedClass +
                                ' m-badge--wide">' +
                                title +
                                "</span></a>"
                            );
                        }
                    },
                    {
                        field: "Id",
                        title: "",
                        textAlign: "center",
                        width: 0
                    }
                ]
            });
        }
        this.setPage(1);
        this.defaultSetting = false;
    }

    private setupPagination(): void {
        this.dataTable.reload();
        this.workflows = new Array<WorkFlow>();
        for (var i = 0; i < this.totalRows; i++) {
            var element = new WorkFlow();
            this.workflows.push(element);
        }
        this.dataTable.fullJsonData = this.workflows;
        this.dataTable.jsonData = this.workflows;
        this.dataTable.load();
    }

    private setPage(page: number): void {
        let pagi: any = this.dataTable.getDataSourceParam("pagination");
        pagi.page = this._wfService.pageNumber == -1 ? page : this._wfService.pageNumber;

        if (this.defaultSetting) {
            pagi.perpage = this._wfService.pageSize == -1 ? 10 : this._wfService.pageSize;
        }
        this.dataTable.setDataSourceParam("pagination", pagi);
        this.dataTable.load();
    }

    private setupProductTable(): void {
        if (this.searchQuery == "") {
            this._wfService
                .getWorkflowCount(
                this.currentFilter,
                this.getFilteredEntity(),
                this.daysCount
                )
                .subscribe(
                (number: number) => {
                    this.totalRows = number;
                    this.setupPagination();
                    this.setPage(1);
                    var perPage = this.dataTable.getPageSize();
                    var offSet = this._wfService.offSet == -1 ? 0 : this._wfService.offSet;
                    this.workflowsOb = this._wfService.getWorkflows(
                        offSet,
                        perPage,
                        this.currentFilter,
                        this.getFilteredEntity(),
                        this.daysCount
                    );
                    this.initEvents();
                },
                (error: Response) => this.handleError(error)
                );
        } else {
            this.searchData(13);
        }
    }

    private initEvents(): void {
        this.setPage(1);

        var pages = this.dataTable.getCurrentPage();
        if (pages - 1 < 0) {
            pages = 0;
        } else {
            pages = pages - 1;
        }

        (function(component: MerchandiseTableComponent) {
            $("#fileLog").on("click", "tr > td:nth-child(5) > span", function() {

                var data = $(this)
                    .parent()
                    .parent()
                    .children("td:nth-child(2)")
                    .text();
                var datatoSearch: Array<Object> = null;
                if (component.searchQuery == "") {
                    datatoSearch = component.workflows;
                } else {
                    datatoSearch = component.searchResults;
                }
                var selectedWorkFlow: Array<Object> = datatoSearch.filter(function(
                    a: WorkFlow
                ) {
                    return a.InstanceName === data;
                });
                var route = "";
                var nu = parseInt(selectedWorkFlow[0]["JobId"]);
                if (nu == 1) {
                    route = "/jobs/inventory";
                } else if (nu == 2) {
                    route = "/jobs/catalog";
                } else if (nu == 4) {
                    route = "/jobs/price";
                } else if (nu == 19) {
                    route = "/jobs/discount";
                }
                component.router.navigate([route], {
                    queryParams: {
                        file: selectedWorkFlow[0]["InstanceName"],
                        entity: selectedWorkFlow[0]["EntityId"],
                        id: selectedWorkFlow[0]["Id"]
                    }
                });
                return false;
            });
        })(this);

        var perPage = this.dataTable.getPageSize();
        let self: MerchandiseTableComponent = this;
        this.workflowsOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );
    }

    bindEvents(): void {
        let self: MerchandiseTableComponent = this;
        self.dataTable.on("m-datatable--on-goto-page", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            self._wfService.pageNumber = pages + 1;
            var perPage = settings.perpage;

            if (self.searchQuery == "") {
                self._wfService
                    .getWorkflows(
                    pages * perPage,
                    perPage,
                    self.currentFilter,
                    self.getFilteredEntity(),
                    self.daysCount
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._wfService
                    .getSearchResult(
                    self.searchQuery,
                    pages * perPage,
                    perPage,
                    self.currentFilter,
                    self.getFilteredEntity(),
                    self.daysCount
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });
        self.dataTable.on("m-datatable--on-layout-updated", function(
            e: any,
            args: any
        ) {
            //self.updateStatus();
        });

        self.dataTable.on("m-datatable--on-update-perpage", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            var perPage = settings.perpage;
            if (self.searchQuery == "") {
                self._wfService
                    .getWorkflows(
                    pages * perPage,
                    perPage,
                    self.currentFilter,
                    self.getFilteredEntity(),
                    self.daysCount
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._wfService
                    .getSearchResult(
                    self.searchQuery,
                    pages * perPage,
                    perPage,
                    self.currentFilter,
                    self.getFilteredEntity(),
                    self.daysCount
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });

        self.dataTable.on("m-datatable--on-reloaded", function(
            e: any,
            settings: any
        ) {
            setTimeout(function() {
                $("#fileLog")
                    .find("tbody > tr")
                    .each(function(a, b) {
                        if ($(b).find("td:nth-child(1) span").length > 0) {
                            if (
                                $(b)
                                    .find("td:nth-child(1) span")
                                    .first()
                                    .html() == ""
                            ) {
                                $(b).hide();
                            }
                        } else {
                            if (a != 0) {
                                $(b).hide();
                            }
                        }
                    });
            }, 50);
        });
    }
    public searchFile(event: any) {
        this.resetPageNumber();
        this.searchData(event);
    }
    public resetPageNumber() {
        this._wfService.pageNumber = -1
    }
    public searchData(event: any): void {
        if (event == 13 || event.which == 13 || event.which == 1) {
            this._wfService.searchQuery = this.searchQuery;
            if (this.searchQuery != "") {
                this._wfService
                    .getSearchResultCount(
                    this.searchQuery,
                    this.currentFilter,
                    this.getFilteredEntity(),
                    this.daysCount
                    )
                    .subscribe(
                    (res: number) => {
                        let numb: number = res;
                        this.totalRows = numb;
                        this.searchResults = new Array<WorkFlow>();
                        for (var n = 0; n < numb; n++) {
                            let prod: WorkFlow = new WorkFlow();
                            this.searchResults.push(prod);
                        }
                        this.setupPagination();
                        this.setPage(1);

                        var pages = 0;
                        pages = this._wfService.pageNumber == -1 ? 0 : this._wfService.pageNumber - 1;

                        var perPage = this.dataTable.getPageSize();
                        this._wfService
                            .getSearchResult(
                            this.searchQuery,
                            pages * perPage,
                            perPage,
                            this.currentFilter,
                            this.getFilteredEntity(),
                            this.daysCount
                            )
                            .subscribe(
                            (m: string) => {
                                this.setPage(1);
                                this.reloadTable.apply(this, [m, pages, perPage]);
                            },
                            (err: Response) => {
                                this.handleError(err);
                            }
                            );
                    },
                    (err: Response) => { }
                    );
            } else {
                this.setupProductTable();
            }
        }
    }

    public fillViewModel(proObj: Object): WorkFlow {
        let catVM = new WorkFlow();
        Object.assign(catVM, proObj);
        return catVM;
    }

    private reloadTable(
        subRecords: Array<Object>,
        page: number,
        perPage: number
    ): void {
        let prodArray: Array<WorkFlow> = new Array<WorkFlow>();
        subRecords.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.workflows.splice(page * perPage, perPage, ...prodArray);
            Object.assign(this.dataTable.fullJsonData, this.workflows);
        } else {
            this.searchResults.splice(page * perPage, perPage, ...prodArray);
            Object.assign(this.dataTable.fullJsonData, this.searchResults);
        }
        this.dataTable.jsonData = Object.assign({}, prodArray);
        this.dataTable.load();
    }
    private handleError(err: Response): void {
        console.error(err);
    }

    public getFilteredEntity() {
        if (this.currentEntityId != 0) {
            return this.entities.filter(
                m => m.Key == this.currentEntityId.toString()
            );
        }
        return this.entities;
    }

    ngOnDestroy(): void {
        if (!this.timerSubscription.closed) {
            this.timerSubscription.unsubscribe();
        }
    }
}
