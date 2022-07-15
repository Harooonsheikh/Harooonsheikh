import { Subject } from "rxjs/Subject";
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit
} from "@angular/core";
import { Helpers } from "../../../../helpers";
import { ScriptLoaderService } from "../../../../_services/script-loader.service";
import { WorkFlowService } from "../../../../_services/workflow.service";
import { KeyValue, FlowStep } from "../../../../Entities/Common";
import {
    WorkFlow,
    CatalogStats
} from "../../../../Entities/WorkFlow";
import { Observable, Observer } from "rxjs";
import { EntityService } from "../../../../_services/entity.service";
import { Router, ActivatedRoute, Params } from "@angular/router";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./merchandise.component.html",
    encapsulation: ViewEncapsulation.None
})
export class MerchandiseComponent implements OnInit, AfterViewInit {
    public filterBy: Subject<string> = null;
    public store: string = null;
    public days: number = 15;
    public filterByDayCount: Subject<number> = null;
    public allEntities: Subject<Array<KeyValue<string>>> = null;
    public timeOut: number = 15000;
    public daysCount: number = 7;
    constructor(
        private _script: ScriptLoaderService,
        private _eService: EntityService,
        private _wfService: WorkFlowService
    ) {
        this.filterBy = new Subject<string>();
        this.filterByDayCount = new Subject<number>();
    }

    ngOnInit() {
        this.daysCount = this._wfService.daysCount == 7 ? this.daysCount : this._wfService.daysCount;
        this.allEntities = new Subject<Array<KeyValue<string>>>();
        this._eService
            .getMerchandizeEntity()
            .subscribe((en: Array<KeyValue<string>>) => {
                this.allEntities.next(en);
            });
    }

    changeMenu(val: number) {
        this.days = val;
        this._wfService.daysCount = val;
        this.filterByDayCount.next(this.days);
    }
    handleError(e: any) {
        console.error(e);
    }

    ngAfterViewInit() {
        this._script.load(
            ".m-grid__item.m-grid__item--fluid.m-wrapper",
            "assets/app/js/dashboard.js"
        );
    }
}
