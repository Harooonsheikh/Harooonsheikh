import {
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit
} from "@angular/core";
import { Helpers } from "../../../helpers";
import { ScriptLoaderService } from "../../../_services/script-loader.service";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./index.component.html",
    encapsulation: ViewEncapsulation.None
})
export class IndexComponent implements OnInit, AfterViewInit {
    constructor(private _script: ScriptLoaderService) { }
    ngOnInit() { }
    ngAfterViewInit() {
        this._script.load(
            ".m-grid__item.m-grid__item--fluid.m-wrapper",
            "assets/app/js/dashboard.js"
        );
    }
}
