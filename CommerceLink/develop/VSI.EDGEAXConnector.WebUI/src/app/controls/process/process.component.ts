import { ViewEncapsulation, OnDestroy } from "@angular/core";
import { WorkFlowService } from "./../../_services/workflow.service";
import { Observable } from "rxjs/Observable";
import { WorkFlow, WorkflowTransition } from "./../../Entities/WorkFlow";
import { Input } from "@angular/core";
import { AfterViewInit } from "@angular/core";
import { Component, OnInit } from "@angular/core";
import { EntityService } from "../../_services/entity.service";
import { KeyValue } from "../../Entities/Common";
import { WorkflowStatComponent } from "../stats/stats.component";
import { WorkflowState } from "../../Entities/WorkflowState";
import { Subscription } from "rxjs";

@Component({
    selector: "workflow-process",
    templateUrl: "process.component.html",
    encapsulation: ViewEncapsulation.None
})
export class WorkflowProcessComponent
    implements OnInit, AfterViewInit, OnDestroy {
    @Input() public fileName: string = "";
    @Input() public entity: number;
    private timer: Observable<number> = null;
    public steps: WorkflowState[] = null;
    public allSteps: WorkflowState[] = null;
    public stepClasses: Array<string> = null;
    public timerSubscription: Subscription;

    constructor(
        private _wfService: WorkFlowService,
        private _eService: EntityService
    ) { }

    ngOnInit() { }

    ngAfterViewInit() {
        var self: WorkflowProcessComponent = this;

        this._wfService
            .states(this.entity)
            .subscribe(states => self.renderStates(states));


        // this._wfService.getWorkFlowStepsByEntity(this.entity).subscribe(m => this.renderSteps(m));
    }

    private renderStates(result: WorkflowState[]) {
        this.allSteps = result;
        this.steps = this.allSteps.filter(m => m.Display == true);
        this.stepClasses = new Array<string>(this.steps.length);
        this.stepClasses.forEach(m => (m = "btn-default"));
        this.timer = Observable.timer(0, 10000);
        this.readSteps();
        this.timerSubscription = this.timer.subscribe((time: number) =>
            this.readSteps()
        );
    }

    private readSteps() {
        this._wfService
            .getWorkFlowByFile(this.fileName)
            .subscribe(m => this.processSteps(m));
    }

    private processSteps(wf: Array<WorkflowTransition>) {

        let statesArray = wf.map(m => m.StateID);
        let maxState = Math.max(...statesArray);

        if (maxState == 10) {
            for (let i = 0; i < this.stepClasses.length; i++) {
                this.stepClasses[i] = this.changeStatusColor("Success");
            }
            this.timerSubscription.unsubscribe();
        }
        else if (maxState != 9) {
            for (let i = 0; i <= maxState; i++) {
                let visibleStep = this.steps.filter(m => parseInt(m.Name) == i);
                if (visibleStep.length > 0) {
                    let index = this.steps.indexOf(visibleStep[0]);
                    this.stepClasses[index] = this.changeStatusColor("InProgress");
                    if (index - 1 >= 0) {
                        this.stepClasses[index - 1] = this.changeStatusColor("Success");
                    }
                }
            }
        }
        else if (maxState == 9) {
            var sorted = wf.sort((a, b) => { return b.StateID - a.StateID });
            var prev = sorted[1];
            var current = sorted[0];

            for (let i = maxState; i > 0; i--) {
                let visibleStep = this.steps.filter(m => parseInt(m.Name) == i);
                if (visibleStep.length > 0) {
                    let index = this.steps.indexOf(visibleStep[0]);
                    this.stepClasses[index] = this.changeStatusColor("");
                }
            }

            for (let i = 0; i <= prev.StateID; i++) {
                let visibleStep = this.steps.filter(m => parseInt(m.Name) == i);
                if (visibleStep.length > 0) {
                    let index = this.steps.indexOf(visibleStep[0]);
                    this.stepClasses[index] = this.changeStatusColor("Failure");
                    break;

                }
            }
        }
    }

    changeStatusColor(status: string): string {
        switch (status) {
            case "Success":
                return "btn-success";
            case "Failure":
                return "btn-danger";
            case "InProgress":
                return "btn-warning";
            default:
                return "";
        }
    }
    private handleError(err: Response): void {
        console.error(err);
    }

    public ngOnDestroy() {

        if (!this.timerSubscription.closed) {
            this.timerSubscription.unsubscribe();
        }
    }
}
