import { Subject, SubjectSubscriber } from "rxjs/Subject";
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit,
    ViewChild,
    HostBinding
} from "@angular/core";
import { Observable, Observer } from "rxjs";
import { Router, ActivatedRoute, Params } from "@angular/router";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";
import { Action, ActionParam } from "../../../../../Entities/StoreActions";
import { KeyValue } from "../../../../../Entities/Common";
import { ActionService } from "../../../../../_services/storeaction.service";
import { StoreService } from "../../../../../_services/store.service";
import { StoreConfigMethod } from "../../../../../Entities/StoreConfigMethod";
import { Response } from "@angular/http";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import * as vkbeautify from "vkbeautify";
import * as SwaggerParser from 'swagger-parser/dist/swagger-parser';
import { AceEditorComponent } from "ng2-ace-editor";
import { Helpers } from "./../../../../../helpers";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./action.component.html",
    styleUrls: ["./action.component.css"],
    encapsulation: ViewEncapsulation.None
})
export class ActionComponent implements OnInit {
    @ViewChild("editor") editor;
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    public storeName: string = null;
    public storeId: number = null;
    public storeKey: string = null;
    public APIURL: string = null;
    public actions: Action[] = null;
    public selectedAction: Action = null;
    public store: StoreConfigMethod = null;
    public response: string = "";
    public hasError: boolean = false;
    public newParam: ActionParam = null;
    public actiontoEdit: Action = null;
    public appStore: StoreConfigMethod = null;
    public swaggerUrl: string = null;
    public swaggerUrlSanitized: SafeResourceUrl = null;
    public requestInProgress: boolean = false;
    public readonly swaggerIndex: string = "/swagger/ui/index";

    globalBodyClass = 'm-page--loading-non-block m-page--fluid m--skin- m-content--skin-light2 m-header--fixed m-header--fixed-mobile m-aside-left--enabled m-aside-left--skin-dark m-aside-left--offcanvas m-footer--push m-aside--offcanvas-default';
    constructor(
        private _storeActionSer: ActionService,
        private _storeService: StoreService,
        private route: ActivatedRoute,
        public sanitizer: DomSanitizer
    ) { }

    ngOnInit() {
        let self: ActionComponent = this;
        self.initToastor();
        self.selectedAction = new Action();
        self.actiontoEdit = new Action();
        self.newParam = new ActionParam();
        self.storeId = parseInt(self._storeService.getStoreID());
        self.storeKey = self._storeService.getStoreKey();
        self.storeName = self._storeService.getStore();
        self._storeActionSer.getAPIURL().subscribe(m => {
            Helpers.setLoading(true);
            Helpers.bodyClass(this.globalBodyClass);
            self.APIURL = m;
            self.validateAPI();
        });

    }
    validateAPI() {
        let self: ActionComponent = this;
        SwaggerParser.validate(self.APIURL, function(err, api) {
            if (err) {
                self.handleError(err);
                toastr.error("Failed to validate API.");
            }
            else {
                SwaggerParser.bundle(self.APIURL)
                    .then(function(api) {
                        self.setActionsFromAPI(api);
                    });
            }
        });
    }
    setActionsFromAPI(api: any) {
        let self: ActionComponent = this;
        var pathArray = Object.keys(api.paths);
        self.actions = new Array<Action>();

        pathArray.forEach((path, index) => {
            let action: Action = new Action();
            action.ActionId = index + 1;
            action.StoreId = self.storeId;
            action.APIKey = self.storeKey;
            action.ActionPath = path;
            var pathArr = path.split("/");

            let i: number = path.lastIndexOf("/");
            action.ActionName = path.substring(i + 1, path.length);

            action.APIURL = "http://" + api.host + "/" + pathArr[1] + "/" + pathArr[2] + "/";
            action.RequestType = (Object.keys(api.paths[path])[0]).toUpperCase();
            action.ActionRoute = pathArr[3] + "/" + pathArr[4];

            var paramsArr = api.paths[path][(Object.keys(api.paths[path])[0])].parameters;
            self.setActionParamsFromAPI(paramsArr, action);
            self.actions.push(action);
        });
        self.sortActions();
        self.setSwaggerURL();
        self.selectedAction = self.actions[0];
        Helpers.setLoading(false);
    }
    setActionParamsFromAPI(paramsArr: any, action: Action) {
        if (paramsArr != null) {
            paramsArr.forEach((param, indx) => {
                let actionParam = new ActionParam();
                actionParam.ActionId = action.ActionId;
                actionParam.ParamId = indx + 1;
                actionParam.Key = param.name;
                actionParam.Type = param.in;
                action.ActionParams.push(actionParam);
            });
        }
    }
    sortActions() {
        let self: ActionComponent = this;
        self.actions = self.actions.sort((a, b) => {

            if (a.ActionName > b.ActionName) {
                return 1;
            }
            if (a.ActionName < b.ActionName) {
                return -1;
            }
            return 0;
        });
    }
    setSwaggerURL() {
        let self: ActionComponent = this;
        self.swaggerUrl = new URL(self.actions[0].APIURL).origin + self.swaggerIndex;
        self.swaggerUrlSanitized = self.sanitizer.bypassSecurityTrustResourceUrl(self.swaggerUrl);
    }
    setAction(act: Action) {
        this.selectedAction = act;
        this.editor.nativeElement.env.editor.getSession().setValue("");
    }
    setEditorValue(val: string) {
        this.editor.nativeElement.env.editor.setOption("wrap", true);
        this.editor.nativeElement.env.editor.setOption("readOnly", true);
        this.editor.nativeElement.env.editor.setShowPrintMargin(false);
        this.editor.nativeElement.env.editor.getSession().setMode("ace/mode/json");
        this.response = vkbeautify.json(val);
        this.editor.nativeElement.env.editor.getSession().setValue(this.response);
        this.editor.nativeElement.env.editor.resize();
    }
    getSanitizeURL(url: string) {
        return this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }
    callApi() {
        if (this.selectedAction.ActionParams.length > 0) {
            this.populateActionParams();
        } else {
            this.sendRequest();
        }
    }
    sendRequest() {
        try {
            $("#m_modal_5").modal("hide");
            this.requestInProgress = true;
            this.response = "Waiting for Response.";
            this.editor.nativeElement.env.editor.getSession().setValue(this.response);
            this.editor.nativeElement.env.editor.resize();
            toastr.success("Requesting Action in Progress.");
            this.makeJSONRequest(this.selectedAction);
            this._storeActionSer.callAction(this.selectedAction).subscribe(
                m => {

                    if (m.status == 200 && this.selectedAction.Request != null) {
                        this.saveRequest(this.selectedAction, m.text());
                    }
                    else {
                        this.setEditorValue(m.text());
                        this.requestInProgress = false;
                    }
                },
                err => {
                    this.editor.nativeElement.env.editor.getSession().setValue("");
                    this.editor.nativeElement.env.editor.resize();
                    toastr.error("Failed to Send Request.");
                    this.handleError(err);
                }
            );

        } catch (error) {
            this.handleError(error);
        }

    }
    makeJSONRequest(action: Action) {
        try {
            if (action.ActionParams.length > 0) {

                let request = new Object();

                for (let i = 0; i < action.ActionParams.length; i += 1) {
                    request[action.ActionParams[i].Key] = action.ActionParams[i].Value;
                }

                action.Request = JSON.stringify(request);
            }
        } catch (error) {
            this.handleError(error);
        }
    }
    saveRequest(action: Action, response: string) {
        try {
            this._storeActionSer.save(action).subscribe(
                m => {
                    this.requestInProgress = false;
                    this.setEditorValue(response);
                },
                err => {
                    this.setEditorValue(response);
                    this.handleError(err);
                }
            );
        } catch (error) {
            this.handleError(error);
        }
    }
    populateActionParams() {
        let self: ActionComponent = this;
        try {
            this._storeActionSer.get(this.selectedAction.ActionName, this.selectedAction.StoreId).subscribe(
                m => {
                    let request: any = m;
                    if (request.Request != null) {
                        self.setActionParams(request.Request);
                    }
                    $("#m_modal_5").modal("show");
                },
                err => {
                    $("#m_modal_5").modal("show");
                    this.handleError(err);
                }
            );
        } catch (error) {
            this.handleError(error);
        }
    }
    setActionParams(request: any) {
        try {

            let self: ActionComponent = this;
            var actionRequest = JSON.parse(request);
            let keyArr = Object.keys(actionRequest);
            keyArr.forEach(key => {

                let param = this.selectedAction.ActionParams.find(p => p.Key == key);
                if (param != null) {
                    param.Value = actionRequest[key];
                    let index: number = this.selectedAction.ActionParams.findIndex(p => p.Key == key);
                    self.selectedAction.ActionParams.splice(index, 1, param);
                }

            });

        } catch (error) {
            this.handleError(error);
        }
    }
    openSwaggerUI() {
        window.open(this.swaggerUrl, "_blank");
    }

    initToastor() {
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
    }

    handleError(e: any) {
        this.requestInProgress = false;
        Helpers.setLoading(false);
        console.error(e);
    }
}
