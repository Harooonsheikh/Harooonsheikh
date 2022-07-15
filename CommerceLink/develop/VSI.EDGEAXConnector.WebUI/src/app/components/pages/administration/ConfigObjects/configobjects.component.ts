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
import { ConfigurableObject } from "../../../../Entities/ConfigurableObject";
import { ConfigObjectService } from "../../../../_services/ConfigObjects.service";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./configobjects.component.html",
    encapsulation: ViewEncapsulation.None
})
export class ConfigObjectsComponent implements OnInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    public dataTable: any = null;
    public configObjectsArr: ConfigurableObject[];
    public configObject: ConfigurableObject;
    public showError: boolean = false;
    public showErrorEntityType: boolean = false;
    constructor(private _configObjectService: ConfigObjectService) {
        this.configObject = new ConfigurableObject();
    }
    public createTable(): void {
        if (this.dataTable == null) {
            var self: ConfigObjectsComponent = this;
            this.dataTable = $("#configObjects").mDatatable({
                data: {
                    type: "local",
                    source: this.configObjectsArr,
                    pageSize: 10
                },
                layout: {
                    theme: "default",
                    class: "",
                    scroll: !1,
                    height: 450,
                    footer: !1
                },

                filterable: !1,
                pagination: !0,
                align: "left",
                columns: [
                    {
                        field: "Id",
                        title: "",
                        textAlign: "left",
                        width: 0
                    },
                    {
                        field: "",
                        title: "#",
                        textAlign: "left",
                        width: 13,
                        template: function(e: any): number {
                            let pagi: any = e.getDatatable().getDataSourceParam('pagination');
                            var perPage = e.getDatatable().getPageSize();
                            return ((pagi.page - 1) * perPage) + e.rowIndex + 1;

                        }
                    },
                    {
                        field: "ComValue",
                        title: "Commerce Value",
                        textAlign: "left",
                        width: 190
                    },
                    {
                        field: "ErpValue",
                        title: "ERP Value",
                        textAlign: "left",
                        width: 100
                    },
                    {
                        field: "EntityType",
                        title: "Entity Type",
                        textAlign: "left",
                        width: 100
                    },
                    {
                        field: "ConnectorKey",
                        title: "Connector Key",
                        textAlign: "left",
                        width: 110
                    },
                    {
                        field: "edit",
                        title: "Actions",
                        width: 70,
                        textAlign: "left",
                        template: function(e: any): string {
                            return "<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>";
                        }
                    }
                ]
            });
        }
    }

    public initEvents(): void {
        let self: ConfigObjectsComponent = this;
        (function(component: ConfigObjectsComponent) {
            $("#configObjects").on(
                "click",
                "tr > td:nth-child(7) > span",
                function() {
                    //edit button clicked

                    var configObjectId = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.editConfigObject(configObjectId);

                    return false;
                }
            );
        })(this);
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
        this._configObjectService.get().subscribe(
            data => {
                this.configObjectsArr = data;
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve configurable objects.");
                this.handleError(e);
            }
        );
    }
    public showSettings() {
        this.createTable();
        this.initEvents();
    }
    onSubmit(isCommereceFieldValid: boolean, isERPFieldValid, EntityType) {
        if (EntityType) {
            if (isCommereceFieldValid || isERPFieldValid) {
                this.updateconfigObject();
                $("#configObjectModal").modal("hide");
            } else {
                this.showError = true;
            }
        }
        else {
            this.showErrorEntityType = true;
        }
    }
    handleError(e: any) {
        console.error(e);
    }
    editConfigObject(configObjectId: number) {
        this.showError = false;
        this.configObject = JSON.parse(
            JSON.stringify(this.configObjectsArr.find(co => co.Id == configObjectId))
        );
        this.showModal();
    }
    showModal() {
        $("#configObjectModal").modal("show");
    }
    updateconfigObject() {
        this._configObjectService
            .update(this.configObject)
            .subscribe(
            co => {

                toastr.success("Configurable Object has been updated successfully.");
                let index: number = this.configObjectsArr.findIndex(
                    co => co.Id == this.configObject.Id
                );
                this.configObjectsArr.splice(index, 1, this.configObject);
                this.dataTable.fullJsonData = this.configObjectsArr;
                this.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. " + e.json().Message
                );
                this.handleError(e);
            }
            );
    }
}
