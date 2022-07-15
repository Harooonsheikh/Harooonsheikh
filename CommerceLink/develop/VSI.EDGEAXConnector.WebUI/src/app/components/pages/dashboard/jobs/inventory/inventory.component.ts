import { Subject } from "rxjs/Subject";
import { InventoryService } from "./../../../../../_services/inventory.service";
import { ViewEncapsulation } from "@angular/core";
import { AfterViewInit } from "@angular/core";
import { WorkFlowService } from "./../../../../../_services/workflow.service";
import { ActivatedRoute, Router } from "@angular/router";
import { ScriptLoaderService } from "./../../../../../_services/script-loader.service";
import { Component, OnInit, HostBinding } from "@angular/core";

@Component({
    selector: "inventory",
    templateUrl: "inventory.component.html",
    encapsulation: ViewEncapsulation.None
})
export class InventoryComponent implements OnInit, AfterViewInit {
    public selectedFile: string = "";
    public inventStat: Subject<Object> = null;
    public eventType: Subject<string> = null;
    private selectedView: string = "";
    public currentEntity: number = 0;
    public instanceId: number = 0;
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    constructor(
        private _script: ScriptLoaderService,
        private route: ActivatedRoute,
        private router: Router,
        private _iService: InventoryService
    ) { }

    ngOnInit() {
        if (this.route.snapshot.queryParams["file"]) {
            this.selectedFile = this.route.snapshot.queryParams["file"];
            this.currentEntity = this.route.snapshot.queryParams["entity"];
            this.inventStat = new Subject<Object>();
            this.eventType = new Subject<string>();
        }
        if (this.route.snapshot.queryParams["id"]) {
            this.instanceId = this.route.snapshot.queryParams["id"];
        }
    }

    ngAfterViewInit() {
        this._iService
            .getInventStats(this.selectedFile)
            .subscribe(
            (m: Object) => this.processStats(m),
            (e: Response) => this.handleError(e)
            );
        this.eventType.subscribe((m: string) => this.changeView(m));
    }
    private processStats(stat: Object): void {

        this.inventStat.next(stat);
    }

    private handleError(err: Response): void {
        this.inventStat.next(null);
        console.error(err);
    }
    public changeView(type: string): boolean {
        this.selectedView = type;
        return false;
    }

    public showWFLogs(): boolean {
        this.router.navigate(["/logs/workflow"], {
            queryParams: { file: this.selectedFile, id: this.instanceId }
        });
        return false;
    }
    public showErrorLogs(): boolean {
        this.router.navigate(["/logs/error"], {
            queryParams: { file: this.selectedFile, id: this.instanceId }
        });
        return false;
    }
}
