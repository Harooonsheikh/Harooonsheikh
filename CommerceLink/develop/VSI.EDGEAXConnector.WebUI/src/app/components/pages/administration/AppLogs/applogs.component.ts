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
import { LogsService } from "../../../../_services/log.service";
import { Helpers } from "../../../../helpers";
declare var jquery: any;
declare var $: any;
declare var toastr: any;


@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./applogs.component.html",
    encapsulation: ViewEncapsulation.None
})
export class AppLogsComponent implements OnInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    public dataTable: any = null;
    public appLogs: Array<any> = null;
    public title: string = null;
    public exceptionDetails: string = null;
    public daysCount = 1;
    constructor(private _logService: LogsService) {
        this.appLogs = new Array<any>();
    }
    ngOnInit() {
        this.initializeToaster();
        this.getLogs();
    }
    public getLogs() {
        Helpers.setLoading(true);
        this._logService.getLogs(this.daysCount).subscribe(
            data => {
                this.appLogs = data;
                console.log(this.appLogs);
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
        if (this.dataTable == null) {
            var self: AppLogsComponent = this;
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
                        field: "LogId",
                        title: "",
                        textAlign: "left",
                        width: 0
                    },
                    {
                        field: "EventLevel",
                        title: "Event Level",
                        textAlign: "left",
                        width: 80
                    },
                    {
                        field: "ErrorSource",
                        title: "Error Source",
                        textAlign: "left",
                        width: 200,
                        template: function(e: any) {

                            return ("<a title='Click to view details' href='#'><span style='text-overflow: ellipsis;overflow: hidden;white-space: nowrap;'>" + e.ErrorSource + "</span></a>");
                        }
                    },
                    {
                        field: "EventMessage",
                        title: "Event Message",
                        textAlign: "left",
                        width: 250,
                        template: function(e: any) {

                            return ("<a title='Click to view details' href='#'><span style='text-overflow: ellipsis;overflow: hidden;white-space: nowrap;'>" + e.EventMessage + "</span></a>");
                        }

                    },
                    {
                        field: "MachineName",
                        title: "Machine Name",
                        textAlign: "left",
                        width: 150

                    },
                    {
                        field: "new Date(CreatedOn).toLocaleTimeString()",
                        title: "Created On",
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
                        field: "ErrorMessage",
                        title: "Error Message",
                        textAlign: "left",
                        width: 150,
                        template: function(e: any) {

                            return ("<a title='Click to view details' href='#'><span style='text-overflow: ellipsis;overflow: hidden;white-space: nowrap;'>" + e.ErrorMessage + "</span></a>");
                        }

                    },
                    {
                        field: "InnerErrorMessage",
                        title: "Inner Error Message",
                        textAlign: "left",
                        width: 150,
                        template: function(e: any) {

                            return ("<a title='Click to view details' href='#'><span style='text-overflow: ellipsis;overflow: hidden;white-space: nowrap;'>" + e.InnerErrorMessage + "</span></a>");
                        }

                    },
                    {
                        field: "CreatedBy",
                        title: "Created By",
                        width: 100,
                        textAlign: "left"
                    }
                ]
            });
        }
    }


    public initEvents(): void {
        let self: AppLogsComponent = this;
        (function(component: AppLogsComponent) {
            $("#appLogs").on("click", "tr > td:nth-child(3) > span", function() {

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

        (function(component: AppLogsComponent) {
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
        console.error(e);
        Helpers.setLoading(false);
    }
}
