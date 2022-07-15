import { Subject } from "rxjs/Subject";
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit,
    OnDestroy,
    HostBinding,
    Input
} from "@angular/core";
import { Observable, Observer, Subscription } from "rxjs";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { Jobs } from "../../../../../Entities/Jobs";
import { JobsService } from "../../../../../_services/jobs.service";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";
import { jobsconfigModule } from "../jobsconfig.module";
import { timer } from "rxjs/observable/timer";
import { StoreService } from "../../../../../_services/store.service";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./filesyncjobs.component.html",
    styleUrls: ["./filesyncjobs.component.css"],
    encapsulation: ViewEncapsulation.None
})
export class FileSyncJobsComponent implements OnInit, AfterViewInit, OnDestroy {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    private dataTable: any = null;
    private jobsArr: Jobs[];
    public job: Jobs;
    private showError: boolean = false;
    private templateToDelete: number = -1;
    private timer: Observable<number> = null;
    private isModalShown: boolean = false;
    private timerSubscription: Subscription = null;
    private timeOut: number = 15000;
    private intervalError: boolean = false;
    private StartingHour: number = null;
    private StartingMin: number = null;
    constructor(private _jobService: JobsService, private _storeService: StoreService) {
        this.job = new Jobs();
        this.intervalError = false;
    }
    ngOnDestroy() {
        if (!this.timerSubscription.closed) {
            this.timerSubscription.unsubscribe();
        }
    }
    ngOnInit() {
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

        this.get();
    }
    ngAfterViewInit() {
        this.timer = Observable.timer(0, this.timeOut);
        this.timerSubscription = this.timer.subscribe((time: number) => this.get());
    }
    private get() {
        if (!this.isModalShown) {
            this._jobService.get(this._storeService.getStoreID(), true).subscribe(
                data => {
                    this.jobsArr = data;
                    this.showSettings();
                },
                e => {
                    toastr.error("Could not retrieve File Sync Jobs.");
                    this.handleError(e);
                }
            );
        }
    }
    private showSettings() {
        this.createTable();
        this.initEvents();
        this.dataTable.fullJsonData = this.jobsArr;
        this.dataTable.reload();
    }
    private createTable(): void {
        if (this.dataTable == null) {
            var self: FileSyncJobsComponent = this;
            this.dataTable = $("#FileSyncJobs").mDatatable({
                data: {
                    type: "local",
                    source: this.jobsArr,
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
                        field: "JobID",
                        title: "",
                        textAlign: "left",
                        width: 0
                    },
                    {
                        field: "",
                        title: "#",
                        width: 13,
                        textAlign: "left",
                        template: function(e: any): number {

                            let pagi: any = e.getDatatable().getDataSourceParam('pagination');
                            var perPage = e.getDatatable().getPageSize();
                            return ((pagi.page - 1) * perPage) + e.rowIndex + 1;

                        }
                    },
                    {
                        field: "JobName",
                        title: "Name",
                        textAlign: "left",
                        width: 200
                    },
                    {
                        field: "JobInterval",
                        title: "Interval (Min)",
                        textAlign: "left",
                        width: 100
                    },
                    {
                        field: "IsRepeatable",
                        title: "Repeatable",
                        width: 100,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.IsRepeatable) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='IsRepeatable'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='IsRepeatable'><span></span></label>";
                        }
                    },
                    {
                        field: "IsActive",
                        title: "Active",
                        width: 60,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.IsActive) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='IsActive'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='IsActive'><span></span></label>";
                        }
                    },
                    {
                        field: "StartTime",
                        title: "Start Time",
                        textAlign: "left",
                        width: 80,
                        template: function(e: any): string {
                            if (e.StartTime != null)
                                return e.StartTime.split(':')[0] + ":" + e.StartTime.split(':')[1];
                        }
                    },
                    {
                        field: "JobStatus",
                        title: "Status",
                        textAlign: "left",
                        width: 80,
                        template: function(e: any): string {
                            let text: string = null;
                            let badgeClass: string = null;

                            if (e.JobStatus == 1) {
                                text = "Idle";
                                badgeClass = " m-badge--brand";
                            }
                            else {
                                text = "Running";
                                badgeClass = " m-badge--success";
                            }

                            return "<a><span class=\"m-badge " +
                                badgeClass +
                                ' m-badge--wide">' +
                                text +
                                "</span></a>"
                        }
                    },
                    {
                        field: "edit",
                        title: "Actions",
                        textAlign: "left",
                        template: function(e: any): string {
                            return "<span><a data-toggle='modal' data-target='#FileSyncModal' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>";
                        }
                    }
                ]
            });
        }
    }

    private initEvents(): void {
        let self: FileSyncJobsComponent = this;
        (function(component: FileSyncJobsComponent) {
            $("#FileSyncJobs").on("click", "tr > td:nth-child(9) > span", function() {
                //edit button clicked

                var jobId = parseInt(
                    $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(1)")
                        .text()
                );
                self.editJob(jobId);
                return false;
            });
        })(this);
        (function(component: FileSyncJobsComponent) {
            $("#FileSyncModal").on("hidden.bs.modal", function() {
                self.isModalShown = false;
            });
        })(this);
    }

    public onSubmit() {
        if (this.job.JobInterval >= 0 && this.job.JobInterval <= 999) {
            this.update();
            $("#FileSyncModal").modal("hide");
        }
        else {
            this.intervalError = true;
        }
    }
    private editJob(jobId: number) {
        this.showError = false;
        this.intervalError = false;
        this.job = JSON.parse(
            JSON.stringify(this.jobsArr.find(e => e.JobID == jobId))
        );
        this.StartingHour = this.job.StartTime.split(':')[0];
        this.StartingMin = this.job.StartTime.split(':')[1];
        this.job.StartTime = new Date(2018, 12, 12, this.StartingHour, this.StartingMin);
        this.showModal();
    }
    private showModal() {
        this.isModalShown = true;
        $("#FileSyncModal").modal("show");
    }
    private update() {
        this.job.StartTime = this.job.StartTime.getHours() + ":" + this.job.StartTime.getMinutes();
        this._jobService.update(this.job).subscribe(
            et => {

                toastr.success(
                    this.job.JobName +
                    " job has been updated successfully."
                );
                let index: number = this.jobsArr.findIndex(
                    et => et.JobID == this.job.JobID
                );
                this.jobsArr.splice(index, 1, this.job);
                this.dataTable.fullJsonData = this.jobsArr;
                this.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. Refresh your browser and try again."
                );
                this.handleError(e);
            }
        );
    }
    handleError(e: any) {
        console.error(e);
    }
}
