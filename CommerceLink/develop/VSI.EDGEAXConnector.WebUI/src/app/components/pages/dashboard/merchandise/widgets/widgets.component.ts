import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import {
    Component,
    OnInit,
    OnChanges,
    ViewEncapsulation,
    Input,
    SimpleChanges,
    SimpleChange,
    OnDestroy
} from "@angular/core";
import { Helpers } from "../../../../../helpers";
import { WorkFlowService } from "../../../../../_services/workflow.service";
import {
    WorkFlow,
    CatalogStats
} from "../../../../../Entities/WorkFlow";
import { KeyValue, FlowStep } from "../../../../../Entities/Common";
import { Subscription } from "rxjs";

@Component({
    //selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    selector: "dashboard-widgets",
    templateUrl: "./widgets.component.html",
    encapsulation: ViewEncapsulation.None
})
export class MerchandiseWidgetComponent implements OnInit, OnChanges, OnDestroy {
    @Input() filterBy: Subject<string>;
    @Input() filterByDayCountSubject: Subject<number>;
    @Input() allEntitiesSubject: Subject<Array<KeyValue<string>>>;
    @Input() timeOut: number;
    public jobsProcess: number = 0;
    public jobsFailed: number = 0;
    public jobsCompleted: number = 0;
    public allJobs: number = 0;
    public jobsFailedPercent: string = "";
    public jobsProcessPercent: string = "";
    public jobsCompletedPercent: string = "";
    public totalJobPercent: string = "";
    public daysCount: number = 7;
    public allEntities: Array<KeyValue<string>> = null;
    private timer: Observable<number> = null;
    private timerSubscription: Subscription = null;

    constructor(private wkService: WorkFlowService) { }
    ngOnChanges(changes: SimpleChanges): void {
        //throw new Error("Method not implemented.");
        // let stat: SimpleChange = changes["status"];
        // if (stat.currentValue && stat.currentValue.length > 0) {
        //     let statsArray: Array<KeyValue<FlowStep>> = new Array<KeyValue<FlowStep>>();
        //     statsArray = Object.assign(statsArray, stat.currentValue);
        //     if (statsArray.length > 0) {
        //         this.jobsProcess = statsArray.filter(m => m.Value == 3).length;
        //         this.jobsFailed = statsArray.filter(m => m.Value == 2).length;
        //         this.jobsCompleted = statsArray.filter(m => m.Value == 1).length;
        //         this.allJobs = statsArray.length;
        //         this.jobsCompletedPercent = (this.jobsCompleted / this.allJobs * 100) | 0;
        //         this.jobsFailedPercent = (this.jobsFailed / this.allJobs * 100) | 0;
        //         this.jobsProcessPercent = (this.jobsProcess / this.allJobs * 100) | 0;
        //     }
        // }
    }

    ngOnInit() {

        this.daysCount = this.wkService.daysCount == 7 ? this.daysCount : this.wkService.daysCount;
        this.allEntities = new Array<KeyValue<string>>();
        this.filterByDayCountSubject.subscribe(m => {
            this.daysCount = m;
            this.processStats();
            if (this.timer == null) {
                this.timer = Observable.timer(0, this.timeOut);
                this.timerSubscription = this.timer.subscribe((time: number) => this.processStats());
            }

        });
        this.allEntitiesSubject.subscribe(m => {
            this.allEntities = m;
            this.processStats();
            if (this.timer == null) {
                this.timer = Observable.timer(0, this.timeOut);
                this.timerSubscription = this.timer.subscribe((time: number) => this.processStats());
            }
        });
    }
    ngAfterViewInit(): void {

    }

    processStats() {
        this.wkService.getStatistics(this.daysCount, this.allEntities).subscribe(
            (stat: Object) => {
                this.jobsProcess = stat["InProcessWorkFlowsCount"];
                this.jobsFailed = stat["FailedWorkFlowsCount"];
                this.jobsCompleted = stat["CompletedWorkFlowsCount"];
                this.allJobs = stat["UniqueWorkFlowsCount"];
                if (this.allJobs > 0) {
                    this.jobsCompletedPercent = (
                        this.jobsCompleted /
                        this.allJobs *
                        100
                    ).toFixed(1);
                    this.jobsFailedPercent = (
                        this.jobsFailed /
                        this.allJobs *
                        100
                    ).toFixed(1);
                    this.jobsProcessPercent = (
                        this.jobsProcess /
                        this.allJobs *
                        100
                    ).toFixed(1);
                    this.totalJobPercent = (this.allJobs / this.allJobs * 100).toString();
                } else {
                    this.jobsCompletedPercent = "0";
                    this.jobsFailedPercent = "0";
                    this.jobsProcessPercent = "0";
                    this.totalJobPercent = "0";
                }
            },
            (err: Response) => {

                console.error(err);
            }
        );
    }
    public filterByStatus(stat: string): void {
        this.filterBy.next(stat);
    }

    public ngOnDestroy() {
        this.timerSubscription.unsubscribe();
    }
}
