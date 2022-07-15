import { Subject } from "rxjs/Subject";
import { KeyValue } from "./../../../../../Entities/Common";
import { CatalogService } from "./../../../../../_services/catalog.service";
import { CatalogProduct } from "./../../../../../Entities/CatalogProduct";
import { Response } from "@angular/http";
import { NgModel } from "@angular/forms";
import { WorkFlow, CatalogStats } from "./../../../../../Entities/WorkFlow";
import { Observable } from "rxjs";
import { WorkFlowService } from "./../../../../../_services/workflow.service";
import { ScriptLoaderService } from "./../../../../../_services/script-loader.service";
import {
    Component,
    OnInit,
    AfterViewInit,
    ViewEncapsulation,
    HostBinding
} from "@angular/core";
import { Helpers } from "../../../../../helpers";
import { Router, ActivatedRoute, Params } from "@angular/router";
declare var jquery: any;
declare var $: any;

export class Product {
    public Name: string = "";
    public ProdId: string = "";
    public longDescription: string = "";
    public shortDescription: string = "";
    public taxClass: string = "";
    public sku: string = "";
    public variations: Array<KeyValue<string>> = null;
    public customAttribute: Array<KeyValue<string>> = null;
    public variants: Array<string> = null;
    constructor() {
        this.variations = new Array<KeyValue<string>>();
        this.customAttribute = new Array<KeyValue<string>>();
        this.variants = new Array<string>();
    }
}

@Component({
    selector: "catalog-view",
    templateUrl: "./catalog.component.html",
    //host: {'class': '.m-grid__item.m-grid__item--fluid.m-wrapper'},
    encapsulation: ViewEncapsulation.None
})
export class CatalogComponent implements OnInit, AfterViewInit {
    public selectedCatalogFile: string = null;
    public catStatsSubject: Subject<Object> = null;
    public eventType: Subject<string> = null;
    public selectedView = "product";
    public currentEntity: number = 0;
    public instanceId: number = 0;

    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    constructor(
        private _script: ScriptLoaderService,
        private route: ActivatedRoute,
        private router: Router,
        private _wfService: WorkFlowService,
        private _cService: CatalogService
    ) { }

    ngOnInit() {
        if (this.route.snapshot.queryParams["file"]) {
            this.selectedCatalogFile = this.route.snapshot.queryParams["file"];
            this.currentEntity = this.route.snapshot.queryParams["entity"];
            this.catStatsSubject = new Subject<Object>();
            this.eventType = new Subject<string>();
        }
        if (this.route.snapshot.queryParams["id"]) {
            this.instanceId = this.route.snapshot.queryParams["id"];
        }
    }

    ngAfterViewInit() {
        this._cService
            .getCatalogStatistics(this.selectedCatalogFile)
            .subscribe(
            (m: Object) => this.processStats(m),
            (e: Response) => this.handleError(e)
            );
        this.eventType.subscribe((m: string) => this.changeView(m));
    }

    public changeView(type: string): boolean {
        this.selectedView = type;
        return false;
    }

    private processStats(stat: Object): void {
        this.selectedView = "Total Products";
        this.catStatsSubject.next(stat);
    }

    private handleError(err: Response): void {
        this.catStatsSubject.next(null);
        console.error(err);
    }

    public showWFLogs(): boolean {
        this.router.navigate(["/logs/workflow"], {
            queryParams: { file: this.selectedCatalogFile, id: this.instanceId }
        });
        return false;
    }
    public showErrorLogs(): boolean {
        this.router.navigate(["/logs/error"], {
            queryParams: { file: this.selectedCatalogFile, id: this.instanceId }
        });
        return false;
    }
}
