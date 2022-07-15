import { Observable } from "rxjs";
import { Response } from "@angular/http";
import { TransactionLog } from "./../../../../../Entities/TransactionLog";
import { LogsService } from "./../../../../../_services/log.service";
import { Router, ActivatedRoute } from "@angular/router";
import { Component, OnInit, HostBinding } from "@angular/core";
declare var jquery: any;
declare var $: any;
@Component({
    selector: "error-logs",
    templateUrl: "error.component.html"
})
export class WorkflowErrorLogComponent implements OnInit {
    public fileName: string = "";
    public data: TransactionLog[] = null;
    public selectedErrorMessage: string = "";
    public searchQuery: string = "";
    public searchMessage: string = "";
    private sub: any;
    public logsFound: boolean = true;
    public showSearch: boolean = false;
    public instanceId: number = 0;

    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private _ls: LogsService
    ) {
        if (this.route.snapshot.queryParams["file"]) {
            this.fileName = this.route.snapshot.queryParams["file"];
        }
        if (this.route.snapshot.queryParams["id"]) {
            this.instanceId = this.route.snapshot.queryParams["id"];
        }
    }

    ngOnInit() {
        if (this.fileName != "") {
            this._ls
                .getTransactionLogs(this.instanceId.toString())
                .subscribe(
                (data: TransactionLog[]) => this.createTable(data),
                (res: Response) => this.handleError(res)
                );
        }

        //this.fileName = "A";
    }

    public search(): void {

        var self: Object = this;
        // this._ls
        //     .getTransactionLogs(this.searchQuery)
        //     .subscribe(
        //     (data: TransactionLog[]) => this.createTable.apply(self, [data]),
        //     (res: Response) => this.handleError(res)
        //     );
    }
    private createTable(data: TransactionLog[]): void {

        if (data.length == 0) {
            this.searchMessage = "No Logs Found.";
            this.logsFound = false;
        } else {
            this.data = data;
            if (this.fileName == "") {
                this.fileName = this.searchQuery;
            }
            let self: WorkflowErrorLogComponent = this;
            var table: any = $("#logs").mDatatable({
                data: {
                    type: "local",
                    source: data,
                    pageSize: 10
                },
                layout: {
                    theme: "default",
                    class: "",
                    scroll: !1,
                    height: 450,
                    footer: !1
                },
                pagination: !0,
                columns: [
                    {
                        field: "EventLevel",
                        title: "Event Type",
                        width: 105,
                        textAlign: "center",
                        template: function(e: any) {
                            return (
                                "<span style='visibility:hidden' id='eventId'>" +
                                e.LogId +
                                "</span>" +
                                e.EventLevel
                            );
                        }
                    },
                    {
                        field: "TimeStamp",
                        title: "Date",
                        width: 90,
                        textAlign: "center",
                        template: function(e: any) {
                            return new Date(e.EventDateTime).toLocaleDateString();
                        }
                    },
                    {
                        field:
                        "new Date(e.getDatatable().jsonData[e.rowIndex].Created).toLocaleTimeString()",
                        title: "Time",
                        width: 90,
                        textAlign: "center",
                        template: function(e: any) {
                            return new Date(e.EventDateTime).toLocaleTimeString();
                        }
                    },
                    {
                        field: "EventMessage",
                        title: "Description",
                        width: 300,
                        template: function(e: any) {

                            if (e.EventMessage.length > 200) {
                                return e.EventMessage.substring(0, 200) + "...";
                            }

                            return e.EventMessage;
                        }
                    },
                    {
                        field: "Actions",
                        title: "Actions",
                        textAlign: "center",
                        width: 70,
                        template: function(e: any) {
                            if (e.EventMessage.length > 200) {
                                return "<a href='#' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='View'><i class='flaticon-interface-6'></i></a>";
                            } else {
                                return "";
                            }
                        }
                    }
                ]
            });

            (function(component: any) {
                $("#logs").on("click", "tr > td:nth-child(5) a", function() {

                    var data = $(this)
                        .closest("tr")
                        .find("td:nth-child(1) span#eventId")
                        .first()
                        .html();
                    component.showLogModel.apply(component, [
                        component.data.filter(m => m.LogId == data)[0].EventMessage
                    ]);
                    return false;
                });
            })(self);
        }
    }
    private handleError(res: Response): void {

        console.error(res);
    }

    public showLogModel(error: string): boolean {

        this.selectedErrorMessage = error;
        $("#m_modal_1_2").modal("show");
        return false;
    }
}
