import { WorkFlowService } from "./../../../../../_services/workflow.service";
import { Observable } from "rxjs/Observable";
import { Component, OnInit, HostBinding } from "@angular/core";
import {
    WorkFlow
} from "../../../../../Entities/WorkFlow";
import { Router, ActivatedRoute } from "@angular/router";
import { DataFileService } from "../../../../../_services/datafiles.service";
import { OnChanges } from "@angular/core/src/metadata/lifecycle_hooks";
declare var jquery: any;
declare var $: any;
declare var toastr: any;

@Component({

    selector: "setting-page",
    templateUrl: "setting.component.html"
})
export class SettingComponent implements OnInit {
    public screenName: string = "";
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    public model: Array<any> = null;
    public hasError: boolean = false;
    constructor(
        private _dataFiles: DataFileService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.route.params.subscribe(routeParam => {

            this.router.routeReuseStrategy.shouldReuseRoute = function() {
                return false;
            };
        });
        if (this.route.snapshot.queryParams["screen"]) {
            this.screenName = this.route.snapshot.queryParams["screen"];
        }
        this.hasError = false;
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
        if (this.screenName != "") {
            let selectedScreen = this.screenName.includes("&") ? this.screenName.replace("&", "||") : this.screenName;
            this._dataFiles
                .getScreenNameData(selectedScreen)
                .subscribe(m => this.createForm(m), e => this.handleError(e));
        }
    }
    createForm(m) {

        this.model = m;
    }
    getRouteData() {
        var ScreenNameQuery: string;
        ScreenNameQuery = this.route.snapshot.queryParams["screen"];
        this._dataFiles
            .getScreenNameData(ScreenNameQuery)
            .subscribe(m => this.createForm(m), e => this.handleError(e));
    }
    OnSwitch(setting) {
        if (setting.Value == "TRUE") {
            setting.Value = "FALSE";
        } else {
            setting.Value = "TRUE";
        }
    }
    saveForm(isFormValid: boolean) {
        if (isFormValid) {
            this._dataFiles.setAppSettings(this.model).subscribe(
                m => {
                    this.confirm();
                },
                e => {
                    toastr.error(
                        "Update failed due to internal error. " + e.json().Message
                    );
                    this.handleError(e);
                }
            );
        }
        else {
            toastr.error("Please provide valid value in URL/Email fields.");
        }
    }
    public confirm() {
        toastr.success("Settings are Updated.");
    }

    private handleError(err: Response): void {
        console.error(err);
    }
}
