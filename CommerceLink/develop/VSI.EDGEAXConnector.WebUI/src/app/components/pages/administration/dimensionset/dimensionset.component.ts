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
import { ConfigObjectService } from "../../../../_services/ConfigObjects.service";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";
import { DimensionSet } from "../../../../Entities/DimensionSet";
import { DimensionSetService } from "../../../../_services/dimensionset.service";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./dimensionset.component.html",
    encapsulation: ViewEncapsulation.None
})
export class DimensionSetComponent implements OnInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    public dataTable: any = null;
    public dimensionSetArr: DimensionSet[];
    public dimensionSet: DimensionSet;
    public showError: boolean = false;
    public modalMode: string = "";
    public readonly editMode = "Edit";

    constructor(private _dimensionSetService: DimensionSetService) {
        this.dimensionSet = new DimensionSet();
    }
    public createTable(): void {
        if (this.dataTable == null) {
            var self: DimensionSetComponent = this;
            this.dataTable = $("#dimensionSet").mDatatable({
                data: {
                    type: "local",
                    source: this.dimensionSetArr,
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
                        field: "ErpValue",
                        title: "Erp Value",
                        textAlign: "left",
                        width: 150
                    },
                    {
                        field: "ComValue",
                        title: "Com Value",
                        textAlign: "left",
                        width: 150
                    },
                    {
                        field: "AdditionalErpValue",
                        title: "Additional Erp Value",
                        textAlign: "left",
                        width: 150
                    },
                    {
                        field: "IsActive",
                        title: "Active",
                        width: 50,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.IsActive) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='IsActive'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='IsActive'><span></span></label>";
                        }
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
        let self: DimensionSetComponent = this;
        (function(component: DimensionSetComponent) {
            $("#dimensionSet").on("click", "tr > td:nth-child(7) > span", function() {
                //edit button clicked
                var dimensionSetId = parseInt(
                    $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(1)")
                        .text()
                );
                self.editDimensionSet(dimensionSetId);
                return false;
            });
        })(this);
    }
    ngOnInit() {
        this.initializeToaster();
        this._dimensionSetService.get().subscribe(
            data => {
                this.dimensionSetArr = data;
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve dimension sets.");
                this.handleError(e);
            }
        );
    }
    public initializeToaster() {
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
    public showSettings() {
        this.createTable();
        this.initEvents();
    }
    onSubmit(isFormValid: boolean) {
        if (isFormValid) {
            this.updateDimensionSet();
            $("#dimensionSetModal").modal("hide");
        } else {
            this.showError = true;
        }
    }
    editDimensionSet(dimensionSetId: number) {
        this.showError = false;
        this.modalMode = this.editMode;
        this.dimensionSet = JSON.parse(
            JSON.stringify(this.dimensionSetArr.find(ds => ds.Id == dimensionSetId))
        );
        this.showModal();
    }
    showModal() {
        $("#dimensionSetModal").modal("show");
    }
    updateDimensionSet() {
        this._dimensionSetService.update(this.dimensionSet).subscribe(
            ds => {
                if (ds.json() == "Success") {
                    toastr.success("Dimension Set has been updated successfully.");
                    let index: number = this.dimensionSetArr.findIndex(
                        ds => ds.Id == this.dimensionSet.Id
                    );
                    this.dimensionSetArr.splice(index, 1, this.dimensionSet);
                    this.dataTable.fullJsonData = this.dimensionSetArr;
                    this.dataTable.reload();
                }
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. " + e.json().Message
                );
                this.handleError(e);
            }
        );
    }
    handleError(e: any) {
        console.error(e);
    }
}
