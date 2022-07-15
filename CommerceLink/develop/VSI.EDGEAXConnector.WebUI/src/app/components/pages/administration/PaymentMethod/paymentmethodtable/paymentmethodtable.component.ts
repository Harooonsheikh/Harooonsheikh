import { Router, ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs";
import {
    Component,
    OnInit,
    AfterViewInit,
    Input,
    ViewChild
} from "@angular/core";
import { forkJoin } from "rxjs/observable/forkJoin";
import { DataTable, Dialog, Tree } from "primeng/primeng";
import { INTERNAL_BROWSER_DYNAMIC_PLATFORM_PROVIDERS } from "@angular/platform-browser-dynamic/src/platform_providers";
import { PaymentMethod } from "../../../../../Entities/PaymentMethod";
import { PaymentMethodService } from "../../../../../_services/paymentmethod.service";
import { ScriptLoaderService } from "../../../../../_services/script-loader.service";
import { KeyValuePair } from "../../../../../Entities/Common";
declare var jquery: any;
declare var $: any;
declare var toastr: any;

@Component({
    selector: "paymentmethod-table",
    templateUrl: "paymentmethodtable.component.html",
    styleUrls: ["./paymentmethodtable.component.css"]
})
export class PaymentMethodTableComponent implements OnInit, AfterViewInit {
    public paymentMethods: PaymentMethod[];
    public dataTable: any = null;
    public modalMode: string = "";
    public buttonText: string = "";
    public paymentMethod: PaymentMethod;
    public readonly editMode = "Edit";
    public readonly addMode = "Add";
    public selectedPMID: number = -1;
    public formMode: string = null;
    public showError: boolean = false;
    public paymentMethodToDelete: number = -1;
    public paymnetMethodKeyValue: Array<KeyValuePair<string>>;
    public errorMessage: string = "";
    public showERPCodeError: boolean = false;
    @ViewChild("f") paymentMethodForm;

    constructor(private paymentMethodService: PaymentMethodService) {
        this.paymentMethod = new PaymentMethod();
        this.paymnetMethodKeyValue = new Array<KeyValuePair<string>>();
    }
    ngAfterViewInit() { }

    ngOnInit() {
        this.initialzeToastr();
        this.paymentMethodService.get().subscribe(
            data => {
                this.paymentMethods = data;
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve payment methods.");
                this.HandleError(e);
            }
        );
    }

    private showSettings() {
        this.createTable();
        this.initEvents();
    }
    private initialzeToastr() {
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
    private createTable(): void {
        if (this.dataTable == null) {
            var self: PaymentMethodTableComponent = this;
            this.dataTable = $("#paymentMethod").mDatatable({
                data: {
                    type: "local",
                    source: this.paymentMethods,
                    pageSize: 10
                },
                layout: {
                    theme: "default",
                    class: "",
                    scroll: !1,
                    //height: 450,
                    footer: !1
                },

                filterable: !1,
                pagination: !0,
                align: "left",
                columns: [
                    {
                        field: "PaymentMethodId",
                        title: "",
                        textAlign: "left",
                        width: 0
                    },
                    {
                        field: "",
                        title: "#",
                        width: 13,
                        textAlign: "left",
                        template: function(e: any): number {

                            let pagi: any = e.getDatatable().getDataSourceParam('pagination');
                            var perPage = e.getDatatable().getPageSize();
                            return ((pagi.page - 1) * perPage) + e.rowIndex + 1;

                        }
                    },
                    {
                        field: "ErpCode",
                        title: "ERP Code",
                        width: 80,
                        textAlign: "left"
                    },
                    {
                        field: "parentMethodName",
                        title: "Parent Method",
                        textAlign: "left",
                        width: 100,
                        template: function(e: any): string {
                            let parentMethodName: string = null;
                            if (e.ParentPaymentMethodId == null) {

                                return parentMethodName;
                            } else {

                                parentMethodName = self.paymentMethods.find(pm => e.ParentPaymentMethodId == pm.PaymentMethodId).ErpCode;
                                return parentMethodName;

                            }

                        }
                    },
                    {
                        field: "ECommerceValue",
                        title: "ECommerce Value",
                        textAlign: "left",
                        width: 105
                    },
                    {
                        field: "ErpValue",
                        title: "ERP Value",
                        textAlign: "left",
                        width: 41
                    },
                    {
                        field: "ServiceAccountId",
                        title: "Service Account Id",
                        textAlign: "left",
                        width: 100
                    },
                    {
                        field: "UsePaymentConnector",
                        title: "Payment Connector",
                        width: 105,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.UsePaymentConnector) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='HasSubMethod'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='HasSubMethod'><span></span></label>";
                        }
                    },
                    {
                        field: "HasSubMethod",
                        title: "Has SubMethod",
                        width: 105,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.HasSubMethod) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='HasSubMethod'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='HasSubMethod'><span></span></label>";
                        }
                    },
                    {
                        field: "IsPrepayment",
                        title: "Prepayment",
                        width: 90,
                        textAlign: "left",
                        template: function(e: any): string {
                            if (e.IsPrepayment) {
                                return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled checked type='checkbox' name='IsPrepayment'><span></span></label>";
                            }

                            return "<label style='margin-bottom: -6.5px;' class='m-checkbox m-checkbox--single m-checkbox--brand'><input disabled type='checkbox' name='IsPrepayment'><span></span></label>";
                        }
                    },
                    {
                        field: "edit",
                        title: "Actions",
                        width: 70,
                        textAlign: "left",
                        template: function(e: any): string {
                            return ("<span><a data-toggle='modal' data-target='#paymentMethodModal' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>"
                                + "<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill' title='Delete'><i class='la la-trash'></i></a></span>");
                        }
                    }
                ]
            });
        }
    }

    private initEvents(): void {
        let self: PaymentMethodTableComponent = this;
        (function(component: PaymentMethodTableComponent) {
            $("#paymentMethod").on(
                "click",
                "tr > td:nth-child(11) > span > span:nth-child(1)",
                function() {
                    //edit button clicked
                    var paymentMethodId = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.editPaymentMethod(paymentMethodId);

                    return false;
                }
            );
        })(this);

        (function(component: PaymentMethodTableComponent) {
            $("#paymentMethod").on(
                "click",
                "tr > td:nth-child(11) > span > span:nth-child(2)",
                function() {
                    //delete button clicked
                    self.paymentMethodToDelete = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.showConfirmationModal();

                    return false;
                }
            );
        })(this);

        (function(component: PaymentMethodTableComponent) {
            $("#paymentMethodModal").on("hidden.bs.modal", function() {
                self.paymentMethodForm.controls["ECommerceValue"]._touched = false;
                self.paymentMethodForm.controls["ErpValue"]._touched = false;
                self.paymentMethodForm.controls["ErpCode"]._touched = false;
            });
        })(this);
    }

    private deletePaymentMethod(paymentMethodId: number) {
        this.paymentMethodService
            .delete(paymentMethodId)
            .subscribe(res => {
                this.processAPIResponse(res.json(), paymentMethodId);
            });
    }

    private editPaymentMethod(paymentMethodId: number) {
        this.setAllPaymentIDs();
        this.paymentMethod = null;
        this.paymentMethod = JSON.parse(
            JSON.stringify(
                this.paymentMethods.find(pm => pm.PaymentMethodId == paymentMethodId)
            )
        );
        this.selectedPMID =
            this.paymentMethod.ParentPaymentMethodId == null
                ? -1
                : this.paymentMethod.ParentPaymentMethodId;
        this.showModal(this.editMode);
    }

    public showModalForAdd() {
        this.selectedPMID = -1;
        this.paymentMethod = null;
        this.paymentMethod = new PaymentMethod();
        this.paymentMethod.HasSubMethod = false;
        this.paymentMethod.IsPrepayment = false;
        this.paymentMethod.StoreId_FK = 1;
        this.setAllPaymentIDs();
        this.showModal(this.addMode);
    }

    private showModal(modalMode) {
        this.showError = false;
        this.showERPCodeError = false;
        if (modalMode == this.editMode) {
            this.buttonText = "Update";
            this.formMode = "Edit";
        } else {
            this.buttonText = "Save";
            this.formMode = "Add";
        }

        this.modalMode = modalMode;
        $("#paymentMethodModal").modal("show");
    }

    private showConfirmationModal() {
        $("#paymentMethodDeleteModal").modal("show");
    }

    public deleteConfirmedByUser() {
        this.deletePaymentMethod(this.paymentMethodToDelete);
        this.paymentMethodToDelete = -1;
    }
    public onSubmit(isFormValid: boolean, erpCode: string) {
        if (isFormValid) {
            if (this.validateERPCode(erpCode.trim())) {
                if (this.formMode == "Add") {
                    this.addPaymentMethod();
                } else if (this.formMode == "Edit") {
                    this.updatePaymentMethod();
                }
                $("#paymentMethodModal").modal("hide");
            }
        } else {
            this.showError = true;
            this.showERPCodeError = false;
            return false;
        }
    }
    private validateERPCode(erpCode): boolean {
        if (this.formMode == "Edit") {
            for (let index = 0; index < this.paymnetMethodKeyValue.length; index++) {
                let existingERPCode = this.paymnetMethodKeyValue[
                    index
                ].Value.toLowerCase();
                let paymentMethodId = this.paymnetMethodKeyValue[index].Key;

                if (
                    existingERPCode == erpCode.toLowerCase() &&
                    paymentMethodId != this.paymentMethod.PaymentMethodId
                ) {
                    this.errorMessage =
                        "Given ERP code already exists. Please provide another ERP code.";
                    this.showERPCodeError = true;
                    return false;
                }
            }
        } else {
            let IsERPCodeAlreadyExists = this.paymnetMethodKeyValue.find(
                pm => pm.Value.toLowerCase() == erpCode.toLowerCase()
            );

            if (IsERPCodeAlreadyExists != null) {
                this.errorMessage =
                    "Given ERP code already exists. Please provide another ERP code.";
                this.showERPCodeError = true;
                return false;
            }
        }

        if (this.selectedPMID >= 0) {
            if (this.selectedPMID == this.paymentMethod.PaymentMethodId) {
                this.errorMessage = "ERP Code and Parent Method can't be same.";
                this.showERPCodeError = true;
                return false;
            }
        }

        return true;
    }
    private processAPIResponse(response: string, paymentMethodId: number): void {
        switch (response) {
            case "Success":
                this.paymentMethods = this.paymentMethods.filter(
                    pm => pm.PaymentMethodId !== paymentMethodId
                );
                this.dataTable.fullJsonData = this.paymentMethods;
                this.dataTable.reload();
                toastr.success("Paymnet Method has been deleted successfully.");
                break;

            case "ParentMethod":
                toastr.error(
                    "This Payment Method has been used as Parent Payment Method, First delete its child records."
                );
                break;

            case "NotFound":
                toastr.error(
                    "Payment Method does not exists. Please refresh your page."
                );
                break;

            case "Failure":
                toastr.error("There is a problem with server. Please try again!.");
                break;
        }
    }

    private updatePaymentMethod() {
        this.paymentMethod.ParentPaymentMethodId =
            this.selectedPMID == -1 ? null : this.selectedPMID;
        this.paymentMethodService.update(this.paymentMethod).subscribe(
            pm => {
                toastr.success("Payment Method has been updated successfully.");
                let index: number = this.paymentMethods.findIndex(
                    pm => pm.PaymentMethodId == this.paymentMethod.PaymentMethodId
                );
                this.paymentMethods.splice(index, 1, this.paymentMethod);
                this.dataTable.fullJsonData = this.paymentMethods;
                this.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. " + e.json().Message
                );
                this.HandleError(e);
            }
        );
    }

    private addPaymentMethod() {
        this.paymentMethod.ParentPaymentMethodId =
            this.selectedPMID == -1 ? null : this.selectedPMID;
        this.paymentMethodService.add(this.paymentMethod).subscribe(
            pm => {
                toastr.success("Payment Method has been added successfully.");
                this.paymentMethods.push(pm);
                this.dataTable.fullJsonData = this.paymentMethods;
                this.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. " + e.json().Message
                );
                this.HandleError(e);
            }
        );
    }

    setAllPaymentIDs() {
        this.paymnetMethodKeyValue = null;
        this.paymnetMethodKeyValue = new Array<KeyValuePair<string>>();
        let paymentMethods: PaymentMethod[] = this.paymentMethods;

        for (let index = 0; index < paymentMethods.length; index++) {
            let parentMethod = new KeyValuePair<string>();
            parentMethod.Key = paymentMethods[index].PaymentMethodId;
            parentMethod.Value = paymentMethods[index].ErpCode;
            //this.paymnetMethodKeyValue[index].Key  = paymentMethods[index].PaymentMethodId;
            //this.paymnetMethodKeyValue[index].Value = paymentMethods[index].ErpCode;
            this.paymnetMethodKeyValue[index] = parentMethod;
        }
    }
    private HandleError(err: Response): void {
        console.error(err);
    }
}
