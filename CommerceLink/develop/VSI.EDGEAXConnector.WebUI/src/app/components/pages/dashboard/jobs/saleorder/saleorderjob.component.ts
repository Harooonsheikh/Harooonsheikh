import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { ScriptLoaderService } from "./../../../../../_services/script-loader.service";
import { PriceService } from "./../../../../../_services/price.service";
import { Subject } from "rxjs/Subject";
import { AfterViewInit, HostBinding, ViewChild } from "@angular/core";
import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { SaleOrderService } from "../../../../../_services/saleorder.service";
import * as vkbeautify from 'vkbeautify';

@Component({
    selector: "saleorderjob",
    templateUrl: "saleorderjob.component.html",
    encapsulation: ViewEncapsulation.None
})
export class SaleOrderJobComponent implements OnInit, AfterViewInit {
    @ViewChild("editor") editor;
    public selectedFile: string = "";
    private selectedView: string = "";
    public currentEntity: number = 0;
    public xml: string = "";
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    constructor(
        private _script: ScriptLoaderService,
        private route: ActivatedRoute,
        private router: Router,
        private _soSer: SaleOrderService
    ) { }

    ngOnInit() {
        if (this.route.snapshot.queryParams["file"]) {
            this.selectedFile = this.route.snapshot.queryParams["file"];
            this.currentEntity = this.route.snapshot.queryParams["entity"];
            this.editor.nativeElement.env.editor.setOption("wrap", true);
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
    }

    ngAfterViewInit() {

        this._soSer
            .getOrderXML(this.selectedFile)
            .subscribe(
            (m: Object) => this.processStats(m),
            (e: Response) => this.handleError(e)
            );
    }
    private processStats(xml: any): void {
        let arr: Array<Array<Object>> = JSON.parse(xml.substr(1, xml.length - 2));
        arr.forEach(m => {
            const ele = m;
            if (ele["Name"] == "XmlOfSalesOrder") {
                this.xml = this.xml + ele["Value"]["@Xml"];
            }
        });
        this.xml = vkbeautify.xml(this.xml);
        this.editor.nativeElement.env.editor.getSession().setValue(this.xml);
        this.editor.nativeElement.env.editor.resize();
    }

    private handleError(err: Response): void {
        console.error(err);
    }

    public showWFLogs(): boolean {
        this.router.navigate(["/logs/workflow"], {
            queryParams: { file: this.selectedFile }
        });
        return false;
    }
    public showErrorLogs(): boolean {
        this.router.navigate(["/logs/error"], {
            queryParams: { file: this.selectedFile }
        });
        return false;
    }
}
