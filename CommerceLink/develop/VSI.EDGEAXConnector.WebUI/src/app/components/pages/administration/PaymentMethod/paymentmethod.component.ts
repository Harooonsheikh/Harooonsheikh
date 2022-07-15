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
import { PaymentMethodTableComponent } from "./paymentmethodtable/paymentmethodtable.component";
import { PaymentMethod } from "../../../../Entities/PaymentMethod";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./paymentmethod.component.html",
    encapsulation: ViewEncapsulation.None
})
export class PaymentMethodComponent implements OnInit, AfterViewInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    constructor() { }

    ngOnInit() { }

    handleError(e: any) {
        console.error(e);
    }

    ngAfterViewInit() { }
}
