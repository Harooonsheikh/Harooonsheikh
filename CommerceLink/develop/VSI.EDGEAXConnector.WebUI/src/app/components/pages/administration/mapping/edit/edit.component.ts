import {
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit
} from "@angular/core";
import { Helpers } from "../../../../../helpers";
import { ScriptLoaderService } from "../../../../../_services/script-loader.service";
import { MappingService } from "../../../../../_services/mapping.service";
import { KeyValue } from "../../../../../Entities/Common";
import { XMLMap } from "../../../../../Entities/XMLMap";
import { Router, ActivatedRoute, Params } from "@angular/router";
declare var jQuery: any;
declare var $: any;
declare var toastr: any;

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./edit.component.html",
    encapsulation: ViewEncapsulation.None
})
export class EditMapComponent implements OnInit, AfterViewInit {
    public mapType: string = "";
    public templateName: string = "";
    public map: XMLMap = null;
    public screenTitle: string = "";
    constructor(
        private _script: ScriptLoaderService,
        private _mapSer: MappingService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
        if (this.route.snapshot.queryParams["template"]) {
            this.screenTitle = "Edit XML Map";
            this.templateName = this.route.snapshot.queryParams["template"];
            this._mapSer.getMap(this.templateName).subscribe(m => {
                this.map = m;
                this.mapType = m.Type;
            });
        } else if (this.route.snapshot.queryParams["type"]) {
            this.map = null;
            this.mapType = this.route.snapshot.queryParams["type"];
            if (this.mapType == "READ") {
                this.screenTitle = "Read XML Map";
            }
            else {
                this.screenTitle = "New XML Map";
            }
        }
    }

    ngAfterViewInit() { }

    handleError(err: any) {
        if (err.status == 400) {
            toastr.error("Invalid XML Format");
        } else {
            toastr.error("Unable to Perform Required Action.");
        }
        console.error(err);
    }
}
