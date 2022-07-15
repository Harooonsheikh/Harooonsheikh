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
import { StoreConfigurationTableComponent } from "./storeconfigurationtable/storeconfigurationtable.component";
import { StoreConfigMethod } from "../../../../../Entities/StoreConfigMethod";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./storeconfiguration.component.html",
    encapsulation: ViewEncapsulation.None
})
export class StoreConfigurationComponent implements OnInit, AfterViewInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    constructor() { }

    ngOnInit() { }

    handleError(e: any) {
        console.error(e);
    }

    ngAfterViewInit() { }
}
