import {
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit,
    ViewChild
} from "@angular/core";
import { Helpers } from "../../../../../helpers";
import { ScriptLoaderService } from "../../../../../_services/script-loader.service";
import { MappingService } from "../../../../../_services/mapping.service";
import { KeyValue } from "../../../../../Entities/Common";
import { XMLMap } from "../../../../../Entities/XMLMap";
import { Router, ActivatedRoute } from "@angular/router";
import { FileUploader } from "ng2-file-upload";
import * as vkbeautify from 'vkbeautify';
declare var jQuery: any;
declare var $: any;
declare var toastr: any;

import { saveAs as importedSaveAs } from "file-saver";
import { StoreService } from "../../../../../_services/store.service";
import { environment } from "../../../../../../environments/environment";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./home.component.html",
    encapsulation: ViewEncapsulation.None
})
export class MapHomeComponent implements OnInit, AfterViewInit {
    @ViewChild("editor") editor;
    text: string = "";
    public templates: Array<string> = null;
    public selectedTemplate: string = "";
    public xmlTxt: string = "";

    public uploader: FileUploader = null;
    constructor(
        private _script: ScriptLoaderService,
        private _sSer: StoreService,
        private _mapSer: MappingService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
        let storeKey = this._sSer.createStoreRequest().headers.get("storeKey");
        var storeHeader: any = [{
            name: "storeKey",
            value: storeKey
        }];
        this.uploader = new FileUploader({
            url: environment.baseUrl + "/api/map/Upload/",
            headers: storeHeader
        });
        this.templates = new Array<string>();
        this.loadMapList();
        this.editor.nativeElement.env.editor.setOption("wrap", true);
        this.editor.nativeElement.env.editor.setOption("readOnly", true);
        this.editor.nativeElement.env.editor.setShowPrintMargin(false);
        var session = this.editor.nativeElement.env.editor.getSession();

        session.on("changeAnnotation", function() {
            var annotations = session.getAnnotations() || [], i = annotations.length;
            var len = annotations.length;
            while (i--) {
                if (/doctype first\. Expected/.test(annotations[i].text)) {
                    annotations.splice(i, 1);
                }
            }
            if (len > annotations.length) {
                session.setAnnotations(annotations);
            }
        });
    }

    loadMapList() {
        this._mapSer.getTemplates().subscribe(m => {
            this.templates = m;
            this.loadInitialTemplate();
        });
    }

    loadInitialTemplate() {
        if (this.templates.length > 0) {
            this.setTemplate(this.templates[this._mapSer.UI_SELECTEDMAPINDEX]);
        }
    }

    setTemplate(a: string) {
        this.selectedTemplate = a;
        let self: MapHomeComponent = this;
        this._mapSer.getXML(this.selectedTemplate).subscribe(m => {
            let val: string = vkbeautify.xml(m);
            self.editor.nativeElement.env.editor.getSession().setValue(val);
            this.editor.nativeElement.env.editor.resize();
        });
        this._mapSer.UI_SELECTEDMAPINDEX = this.templates.indexOf(a);
    }

    ngAfterViewInit() {
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

    handleError(err: any) {
        if (err.status == 400) {
            toastr.error("Invalid XML Format");
        } else {
            toastr.error("Unable to Perform Required Action.");
        }
        console.error(err);
    }

    loadNewMap(typ: string) {
        this.router.navigate(["/mapping/edit"], { queryParams: { type: typ } });
    }
    editTemplate() {
        this.router.navigate(["/mapping/edit"], {
            queryParams: { template: this.selectedTemplate }
        });
    }
    openDialog() {
        $("#TemplateDeleteModal").modal("show");
    }
    deleteTemplate() {
        this._mapSer.delete(this.selectedTemplate).subscribe(m => {
            if (m == true) {
                toastr.success("Successfully Deleted.");
                this.templates.splice(this.templates.indexOf(this.selectedTemplate), 1);
                this.selectedTemplate = "";
                this.xmlTxt = "";
                this.editor.nativeElement.env.editor.getSession().setValue(this.xmlTxt);
            } else {
                toastr.error("Deletion Failed.");
            }
        });
    }
    backup() {
        this._mapSer.backup().subscribe(m => {
            importedSaveAs(m.blob(), "templates_backup.zip");
        });
    }

    upload() {
        this.uploader.uploadAll();
        this.uploader.response.subscribe(m => {
            try {
                let a: any = JSON.parse(m);
                if (a == true) {
                    toastr.success("Template Uploaded Successfully");
                }
                let msg: string = a["Message"];
                if (msg == "Invalid File" || msg == "Invalid XML") {
                    toastr.error("Upload Failed : Invalid Template");
                }
                if (msg == "FAIL") {
                    toastr.error("Upload Failed.");
                }
            } catch (err) {
                if (m == "true") {
                    toastr.success("Template Uploaded");
                }
            }
            $("#m_modal_5").modal("hide");
        }, err => {
            this.handleError(err);
        });
    }
}
