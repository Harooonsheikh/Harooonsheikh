import { Subject } from "rxjs/Subject";
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit,
    HostBinding
} from "@angular/core";
import { Observable, Observer } from "rxjs";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { ConfigObjectService } from "../../../../_services/ConfigObjects.service";
import { ApiLogsService } from "../../../../_services/apilog.service";
import { KeyValuePair } from "../../../../Entities/Common";
import { Helpers } from "../../../../helpers";
import * as moment from 'moment-timezone';
declare var jquery: any;
declare var $: any;
declare var toastr: any;


@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./apilogs.component.html",
    encapsulation: ViewEncapsulation.None
})
export class ApiLogsComponent implements OnInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    public dataTable: any = null;
    public appLogs: Array<any> = null;
    public apiMethods: Array<any> = null;
    public title: string = null;
    public exceptionDetails: string = null;
    public daysCount = 1;
    public fromDate: Date = null;
    public startDateFilter: string = null;
    public toDate: Date = null;
    public endDateFilter: string = null;
    public MethodKeyValue: Array<KeyValuePair<string>>;
    public searchQuery: string = "";
    public selectedMethod = -1;
    public selectedLogType = 0;
    public selectedType = -1;
    public selectedMethodText: string = null;
    public selectedLogTypeText: string = null;
    public selectedTypeText: string = null;
    constructor(private _logService: ApiLogsService) {
        this.appLogs = new Array<any>();
        this.apiMethods = new Array<any>();
    }
    ngOnInit() {
        this.selectedLogTypeText = "ACTIVE";
        this.selectedMethodText = "-1";
        this.selectedTypeText = "-1";
        this.setDateParams();
        this.initializeToaster();
        this.getMethods();
        this.getLogs();

    }
    public setDateParams() {
        var self: ApiLogsComponent = this;
        let todayDate: Date = new Date();
        self.fromDate = new Date();
        self.fromDate.setDate(todayDate.getDate() - 1);
        self.toDate = todayDate;
        self.startDateFilter = moment(self.fromDate).format("YYYY-MM-DD");
        self.endDateFilter = moment(self.toDate).format("YYYY-MM-DD");
    }
    updateDates() {
        console.log("Im in", this);
        var self: ApiLogsComponent = this;
        // this.days = val;
        // this._wfService.daysCount = val;
        // this.filterByDayCount.next(this.days);
        let todayDate: Date = new Date();
        console.log(todayDate);
        if (self.selectedLogType == 0) {
            console.log('In 0');
            self.fromDate = new Date();
            self.fromDate.setDate(todayDate.getDate() - 1);
            self.toDate = todayDate;
            console.log(self.fromDate + ' ' + self.toDate);
            self.startDateFilter = moment(self.fromDate).format("YYYY-MM-DD");
            self.endDateFilter = moment(self.toDate).format("YYYY-MM-DD");
        }
        else if (self.selectedLogType == 1) {
            console.log('In 1');
            self.fromDate.setDate(todayDate.getDate() - 91);
            self.toDate.setDate(todayDate.getDate() - 90);
            console.log(self.fromDate + ' ' + self.toDate);
            self.startDateFilter = moment(self.fromDate).format("YYYY-MM-DD");
            self.endDateFilter = moment(self.toDate).format("YYYY-MM-DD");
        }
        // self.startDateFilter = moment(self.fromDate).format("YYYY-MM-DD");
        // self.endDateFilter = moment(self.toDate).format("YYYY-MM-DD");
    }
    public getMethods() {
        Helpers.setLoading(true);
        this._logService.getMethods().subscribe(
            data => {
                this.apiMethods = data;
                this.setMethods();
                Helpers.setLoading(false);
            },
            e => {
                toastr.error("Could not retrieve application logs.");
                this.handleError(e);
            }
        );
    }

    setMethods() {
        this.MethodKeyValue = null;
        this.MethodKeyValue = new Array<KeyValuePair<string>>();
        let apiMethodsList: Array<any>[] = this.apiMethods;

        for (let index = 0; index < apiMethodsList.length; index++) {
            let parentMethod = new KeyValuePair<string>();
            parentMethod.Key = index;
            parentMethod.Value = apiMethodsList[index].toString();
            this.MethodKeyValue[index] = parentMethod;
        }
    }
    public getLogs() {
        Helpers.setLoading(true);
        this._logService.getFilterApiLogs(this.selectedLogTypeText, this.selectedMethodText, this.selectedTypeText, this.startDateFilter, this.endDateFilter, this.searchQuery).subscribe(
            data => {
                this.appLogs = data;
                this.showLogs();
                Helpers.setLoading(false);
            },
            e => {
                toastr.error("Could not retrieve application logs.");
                this.handleError(e);
            }
        );
    }
    public showLogs() {
        this.createTable();
        this.initEvents();
    }
    public createTable(): void {
        var logss = this.appLogs;

        if (this.dataTable == null) {
            var self: ApiLogsComponent = this;
            this.dataTable = $("#appLogs").mDatatable({
                data: {
                    type: "local",
                    source: this.appLogs,
                    pageSize: 10
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
                align: "left",
                columns: [
                    {
                        field: "RequestResponseId",
                        title: "Id",
                        textAlign: "left",
                        width: 50
                    },
                    {
                        field: "ApplicationName",
                        title: "Application Name",
                        textAlign: "left",
                        width: 130
                    },
                    {
                        field: "MethodName",
                        title: "Method",
                        textAlign: "left",
                        width: 100
                    },
                    {
                        field: "DataDirectionName",
                        title: "Type",
                        textAlign: "left",
                        width: 150

                    },
                    {
                        field: "new Date(CreatedOn).toLocaleTimeString()",
                        title: "Created Date",
                        width: 150,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.CreatedOn == null) {
                                return "";
                            }
                            return new Date(e.CreatedOn).toLocaleString();
                        }
                    },
                    {
                        field: "DataPacket",
                        title: "Data Packet",
                        width: 150,
                        textAlign: "left",
                        template: function(e: any) {

                            return ("<a title='Click to view details' href='#'><span style='text-overflow: ellipsis;overflow: hidden;white-space: nowrap;'>" + e.DataPacket + "</span></a>");
                        }
                    }
                ]
            });
        }
    }


    public initEvents(): void {
        let self: ApiLogsComponent = this;
        (function(component: ApiLogsComponent) {
            $("#appLogs").on("click", "tr > td:nth-child(6) > span", function() {
                var logId = parseInt(
                    $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(1)")
                        .text()
                );
                self.setExceptionDetails("Response", self.appLogs.find(l => l.RequestResponseId == logId).DataPacket);
                return false;
            });
        })(this);

        (function(component: ApiLogsComponent) {
            $("#appLogs").on("click", "tr > td:nth-child(4) > span", function() {

                var logId = parseInt(
                    $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(1)")
                        .text()
                );
                self.setExceptionDetails("Event Source", self.appLogs.find(l => l.LogId == logId).EventMessage);

                return false;
            });
        })(this);
    }
    public setExceptionDetails(title: string, exceptionDetails) {
        this.title = title;
        this.exceptionDetails = exceptionDetails;
        this.showModal();
    }
    public searchFile(event: any) {

        var self: ApiLogsComponent = this;

        if (this.selectedLogType != -1) {
            var selector = document.getElementById('LogType');
            this.selectedLogTypeText = selector[this.selectedLogType].text;
        }

        if (this.selectedMethod != -1) {
            var selector = document.getElementById('Method');
            this.selectedMethodText = selector[this.selectedMethod + 1].text;
        }
        else {
            this.selectedMethodText = "-1";
        }

        if (this.selectedType != -1) {
            var selector = document.getElementById('TypeResponse');
            this.selectedTypeText = selector[this.selectedType].text;
        }
        else {
            this.selectedTypeText = "-1";
        }
        if (this.startDateFilter == "" || this.endDateFilter == "") {
            this.startDateFilter = "1/1/0001";
            this.endDateFilter = "1/1/3000";
        }
        Helpers.setLoading(true);
        this._logService.getFilterApiLogs(this.selectedLogTypeText, this.selectedMethodText, this.selectedTypeText, this.startDateFilter, this.endDateFilter, this.searchQuery).subscribe(
            data => {
                this.appLogs = data;
                this.dataTable.fullJsonData = this.appLogs;
                this.dataTable.reload();
                Helpers.setLoading(false);
            },
            e => {
                toastr.error("Could not retrieve application logs.");
                this.handleError(e);
            }
        );
    }
    changeMenu(val: number) {
        this.daysCount = val;
        this.filterLogs();

    }
    public filterLogs() {
        Helpers.setLoading(true);
        this._logService.getLogs(this.daysCount).subscribe(
            data => {
                this.appLogs = data;
                this.dataTable.fullJsonData = this.appLogs;
                this.dataTable.reload();
                Helpers.setLoading(false);
            },
            e => {
                toastr.error("Could not retrieve application logs.");
                this.handleError(e);
            }
        );
    }
    public initializeToaster() {
        toastr.options = {
            closeButton: false,
            debug: false,
            newestOnTop: false,
            progressBar: false,
            positionClass: "toast-top-right",
            preventDuplicates: true,
            onclick: null,
            showDuration: "300",
            hideDuration: "1000",
            timeOut: "5000",
            extendedTimeOut: "1000",
            showEasing: "swing",
            hideEasing: "linear",
            showMethod: "fadeIn",
            hideMethod: "fadeOut"
        };
    }
    showModal() {
        $("#appLogsModal").modal("show");
    }

    handleError(e: any) {
        Helpers.setLoading(false);
    }
}
