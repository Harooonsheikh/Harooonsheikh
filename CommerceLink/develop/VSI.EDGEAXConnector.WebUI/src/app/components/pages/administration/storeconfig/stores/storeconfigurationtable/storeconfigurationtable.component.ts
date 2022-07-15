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
import { ScriptLoaderService } from "../../../../../../_services/script-loader.service";
import { KeyValuePair } from "../../../../../../Entities/Common";
import { UserService } from "../../../../../../_services/user.service";
import { UserDetail, ApplicationUser } from "../../../../../../Entities/UserDetail";
import { StoreService } from "../../../../../../_services/store.service";
import { AppSettingService } from "../../../../../../_services/appsettings.service";
import { Store, SyncStorePaymentConnector, PaymentConnectorRequest } from "../../../../../../Entities/Store";
import { EcomTypeService } from "../../../../../../_services/ecomtype.service";
import { ApplicationSetting } from "../../../../../../Entities/ApplicanSetting";
import { NgModel } from "@angular/forms";
import { Helpers } from "../../../../../../helpers";
declare var jquery: any;
declare var $: any;
declare var toastr: any;

@Component({
    selector: "storeconfigurationtable-table",
    templateUrl: "storeconfigurationtable.component.html",
    styleUrls: ["./storeconfigurationtable.component.css"]
})
export class StoreConfigurationTableComponent implements OnInit, AfterViewInit {
    public storeConfigMethods: Store[];

    public AppSettingsList: any[];
    public ApplicationSettingList: ApplicationSetting[];
    public duplicateIdLocal: string;

    public dataTable: any = null;
    public modalMode: string = "";
    public buttonText: string = "";
    public storeConfigMethod: Store;
    public readonly editMode = "Edit";
    public readonly addMode = "Add";
    public formMode: string = null;
    public showError: boolean = false;
    public storeConfigToDelete: number = -1;
    public ecomTypeKeyValue: Array<KeyValuePair<string>>;
    public errorMessage: string = "";
    public showERPCodeError: boolean = false;
    public isStoreNameTaken = false;
    public isStoreKeyTaken = false;
    public selectedEcomID: number = -1;
    public syncStorePaymentConnector: SyncStorePaymentConnector[];
    public selectAll: boolean = true;
    public NoStoreSelectedError: boolean = false;
    public paymentConnectorAPIURL: string = null;
    public arrayLength: number = null;
    public callInProgress: boolean = false;

    @ViewChild("f") storeConfigMethodForm;

    globalBodyClass = 'm-page--loading-non-block m-page--fluid m--skin- m-content--skin-light2 m-header--fixed m-header--fixed-mobile m-aside-left--enabled m-aside-left--skin-dark m-aside-left--offcanvas m-footer--push m-aside--offcanvas-default';
    private userDetails: UserDetail = null;
    private currentAppUser: ApplicationUser = null;
    appUser: ApplicationUser = null;
    store: string = "";
    constructor(
        private storeConfigService: StoreService,
        private _userService: UserService,
        private _storeService: StoreService,
        private _appSettingsService: AppSettingService,
        private _ecomTypeService: EcomTypeService,
        private router: Router
    ) {

        this.userDetails = new UserDetail();
        this.storeConfigMethod = new Store();

        this.AppSettingsList = [];
        this.ApplicationSettingList = [];
        this.duplicateIdLocal = null;

        this.ecomTypeKeyValue = new Array<KeyValuePair<string>>();
        this.appUser = new ApplicationUser();
        this.isStoreNameTaken = false;
        this.isStoreKeyTaken = false;
        this.syncStorePaymentConnector = new Array<SyncStorePaymentConnector>();
    }
    ngAfterViewInit() { }

    ngOnInit() {
        this.initialzeToastr();
        let self: StoreConfigurationTableComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                let test = self.currentAppUser.LastName;
            },
            (err: Response) => { }
        );


        this.storeConfigService.get().subscribe(
            data => {
                this.storeConfigMethods = data;
                this.storeConfigMethod.appList = [];
                this.ApplicationSettingList = [];
                this.setStoresForPaymentConnector();
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve store configurations.");
                this.handleError(e);
            }
        );
        this._ecomTypeService.get().subscribe(
            data => {
                this.ecomTypeKeyValue = data;
            },
            e => {
                toastr.error("Could not retrieve Ecom Types.");
                this.handleError(e);
            }
        );

        this._storeService.getaymentConnectorAPIURL().subscribe(
            apiURL => {

                this.paymentConnectorAPIURL = apiURL;
                console.log(this.paymentConnectorAPIURL);
            },
            e => {
                toastr.error("Could not retrieve Payment Connector API URL.");
                this.handleError(e);
            });
    }

    private updateSelectAll() {
        var selectedStores = this.syncStorePaymentConnector.filter(s => s.IsSelected == true);
        this.selectAll = selectedStores.length == this.syncStorePaymentConnector.length ? true : false;
    }
    private setStoresForPaymentConnector() {

        this.syncStorePaymentConnector = null;
        this.syncStorePaymentConnector = new Array<SyncStorePaymentConnector>();
        this.storeConfigMethods.forEach(store => {
            let syncStore: SyncStorePaymentConnector = new SyncStorePaymentConnector();
            syncStore.StoreId = store.StoreId;
            syncStore.Name = store.Name;
            syncStore.IsSelected = true;
            syncStore.StoreKey = store.StoreKey;
            this.syncStorePaymentConnector.push(syncStore);
        });

        this.sortStores();
        this.arrayLength = this.syncStorePaymentConnector.length - 1;
    }

    private UpdateStoreSelection() {
        this.syncStorePaymentConnector.forEach(store => {
            store.IsSelected = this.selectAll == true ? true : false;
        });
    }

    sortStores() {
        let self: StoreConfigurationTableComponent = this;
        self.syncStorePaymentConnector = self.syncStorePaymentConnector.sort((a, b) => {

            if (a.Name > b.Name) {
                return 1;
            }
            if (a.Name < b.Name) {
                return -1;
            }
            return 0;
        });
    }

    private syncPaymentConnectors() {

        this.NoStoreSelectedError = false;
        var selectedStore = this.syncStorePaymentConnector.find(s => s.IsSelected == true);

        if (selectedStore != null) {

            $("#storePaymentConnectorModal").modal("hide");
            Helpers.setLoading(true);
            this.callInProgress = true;
            Helpers.bodyClass(this.globalBodyClass);

            var request = this.makeJSONRequest();

            this._storeService.sendPaymentConnectorSyncRequest(this.paymentConnectorAPIURL, request, selectedStore.StoreKey).subscribe(
                res => {

                    Helpers.setLoading(false);
                    this.callInProgress = false;
                    console.log(res);

                    let message: string = null;
                    if (res.json().Status == true) {

                        toastr.success("Payment connector sync is successful.");
                    }
                    else {
                        toastr.error("Payment connector sync request failed. Please contact system administrator.");
                    }
                },
                err => {
                    toastr.error("Unable to send payment connector sync call to WEBAPI.");
                    this.handleError(err);
                }
            );
        }
        else {
            this.NoStoreSelectedError = true;
        }

    }

    private makeJSONRequest(): string {
        try {

            let paymentConnectorRequest: PaymentConnectorRequest = new PaymentConnectorRequest();

            paymentConnectorRequest.SynchronizeAll = this.selectAll == true ? true : false;
            if (paymentConnectorRequest.SynchronizeAll == false) {

                var selectedStores = this.syncStorePaymentConnector.filter(s => s.IsSelected == true);

                selectedStores.forEach(store => {
                    paymentConnectorRequest.StoreIds.push(store.StoreId);
                });
            }

            return JSON.stringify(paymentConnectorRequest);

        } catch (error) {
            this.handleError(error);
        }
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
            var self: StoreConfigurationTableComponent = this;
            this.dataTable = $("#storeConfigMethod").mDatatable({
                data: {
                    type: "local",
                    source: this.storeConfigMethods,
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
                align: "center",
                columns: [
                    {
                        field: "StoreId",
                        title: "",
                        width: 0,
                        textAlign: "center"
                    },
                    {
                        field: "StoreId",
                        title: "#",
                        width: 30,
                        textAlign: "center",
                        template: function(e: any): number {


                            let pagi: any = e.getDatatable().getDataSourceParam('pagination');
                            var perPage = e.getDatatable().getPageSize();
                            return ((pagi.page - 1) * perPage) + e.rowIndex + 1;

                        }
                    },
                    {
                        field: "Name",
                        title: "Name",
                        width: 120,
                        textAlign: "center"
                    },
                    {
                        field: "Description",
                        title: "Description",
                        textAlign: "center",
                        width: 170
                    },
                    {
                        field: "StoreKey",
                        title: "Store Key",
                        textAlign: "center",
                        width: 300
                    },
                    {
                        field: "edit",
                        title: "",
                        width: 35,
                        textAlign: "center",
                        template: function(e: any): string {
                            return "<span><a data-toggle='modal' data-target='#storeConfigModal' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>";
                        }
                    },
                    {
                        field: "delete",
                        title: "",
                        width: 35,
                        textAlign: "center",
                        template: function(e: any): string {
                            return "<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill' title='Disable'><i class='la la-trash'></i></a></span>";
                        }
                    }
                ]
            });
        }
    }

    private initEvents(): void {
        let self: StoreConfigurationTableComponent = this;
        (function(component: StoreConfigurationTableComponent) {
            $("#storeConfigMethod").on(
                "click",
                "tr > td:nth-child(6) > span",
                function() {
                    //edit button clicked
                    var storeId = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.editStoreConfig(storeId);

                    return false;
                }
            );
            $("#storeConfigMethod").on(
                "click",
                "tr > td:nth-child(7) > span",
                function() {
                    //delete button clicked
                    self.storeConfigToDelete = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.ShowConfirmationModal();
                    return false;
                }
            );
        })(this);

        (function(component: StoreConfigurationTableComponent) {
            $("#storeConfigMethod").on(
                "click",
                "tr > td:nth-child(11) > span",
                function() {
                    //action button clicked
                    var storeName = $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(2)")
                        .text();
                    var storeId = $(this)
                        .parent()
                        .parent()
                        .children("td:nth-child(1)")
                        .text();
                    var route = "/storeconfig/storeaction";
                    component.router.navigate([route], {
                        queryParams: { storeName: storeName, storeId: storeId }
                    });
                    return false;
                }
            );
        })(this);
    }

    private editStoreConfig(storeId: number) {
        this.storeConfigMethod = null;
        this.storeConfigMethod = JSON.parse(
            JSON.stringify(this.storeConfigMethods.find(pm => pm.StoreId == storeId))
        );
        //this.selectedEcomID = this.storeConfigMethod.EcomType == null ? -1 : this.storeConfigMethod.EcomType.Key;
        this.showModal(this.editMode);
    }

    public showModalForAdd() {
        this.selectedEcomID = -1;
        this.storeConfigMethod = null;
        this.storeConfigMethod = new Store();
        this.storeConfigMethod.IsActive = true;
        this.storeConfigMethod.OrganizationId = 1;
        this.storeConfigMethod.ERPType.Key = 1;
        this.storeConfigMethod.DuplicateOf = -1;
        this.duplicateIdLocal = null;
        this.ApplicationSettingList = [];
        this.storeConfigMethod.appList = [];
        this.showModal(this.addMode);
    }

    private showModalForSyncPaymentConnectors() {
        this.NoStoreSelectedError = false;
        $("#storePaymentConnectorModal").modal("show");
    }

    private showModal(modalMode) {
        this.showError = false;
        this.isStoreNameTaken = false;
        this.isStoreKeyTaken = false;
        // this.showERPCodeError = false;
        if (modalMode == this.editMode) {
            this.buttonText = "Update";
            this.formMode = "Edit";
        } else {
            this.buttonText = "Save";
            this.formMode = "Add";
        }

        this.modalMode = modalMode;
        $("#storeConfigModal").modal("show");
    }

    private ShowConfirmationModal() {
        $("#storeConfigDeleteModal").modal("show");
    }

    public disableConfirmedByUser() {
        this.disableStore(this.storeConfigToDelete);
        this.storeConfigToDelete = -1;
    }
    public onSubmit(isFormValid: boolean) {
        if (isFormValid) {
            this.storeConfigService.verifyStoreName(this.storeConfigMethod.Name, this.storeConfigMethod.StoreId).subscribe(
                res => {
                    if (res.json()) {
                        this.isStoreNameTaken = true;
                    }
                    else {
                        if (this.formMode == "Add") {
                            this.add();

                        } else if (this.formMode == "Edit") {
                            this.update();
                        }
                        $("#storeConfigModal").modal("hide");
                    }
                },
                e => {
                    toastr.error("Store with this name already exists.");
                    this.handleError(e);
                }
            );
        } else {
            this.showError = true;
            return false;
        }

    }
    verifyStoreName(storeName: string, isValid: boolean, storeId: number) {
        if (isValid) {
            this.isStoreNameTaken = false;
            this.storeConfigService.verifyStoreName(storeName, storeId).subscribe(
                res => {
                    if (res.json()) {
                        this.isStoreNameTaken = true;
                    }
                },
                e => {
                    toastr.error("Some error occured while verifying store name.");
                    this.handleError(e);
                }
            );
        }
    }

    getDuplicateId(selectedValue: string) {
        this.ApplicationSettingList = [];
        this.duplicateIdLocal = selectedValue;
        this._appSettingsService.getStoreByDuplicateId(parseInt(this.duplicateIdLocal)).subscribe(
            data => {
                this.AppSettingsList = data;

                for (let settings of this.AppSettingsList) {
                    this.ApplicationSettingList.push(settings);
                }
            },
            e => {
                toastr.error("Could not retrieve store configurations.");
                this.handleError(e);
            }
        );



    }


    verifyStoreKey(storeKey: string, isValid: boolean, storeId: number) {
        if (isValid) {
            this.isStoreKeyTaken = false;
            this.storeConfigService.verifyStoreKey(storeKey, storeId).subscribe(
                res => {
                    if (res.json()) {
                        this.isStoreKeyTaken = true;
                    }
                },
                e => {
                    toastr.error("Some error occured while verifying store key.");
                    this.handleError(e);
                }
            );
        }
    }
    resetStoreKeyError() {
        this.isStoreKeyTaken = false;
    }
    resetError() {
        this.isStoreNameTaken = false;
    }
    handleError(e: any) {
        Helpers.setLoading(false);
        this.callInProgress = false;
        console.error(e);
    }


    public disableStore(storeId: number) {
        let self: StoreConfigurationTableComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                //this.storeConfigMethod.ModifiedBy = self.currentAppUser.FirstName;
            },
            (err: Response) => { }
        );
        this.storeConfigService.disable(storeId).subscribe(
            s => {
                toastr.success("Store has been disabled successfully.");
                let index: number = self.storeConfigMethods.findIndex(
                    s => s.StoreId == storeId
                );
                self.storeConfigMethods.splice(index, 1);
                self.dataTable.fullJsonData = self.storeConfigMethods;
                self.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. Refresh your browser and try again."
                );
                this.handleError(e);
            }
        );
    }

    private update() {
        let self: StoreConfigurationTableComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                //this.storeConfigMethod.ModifiedBy = self.currentAppUser.FirstName;
            },
            (err: Response) => { }
        );
        // this.storeConfigMethod.EcomType.Key = this.selectedEcomID == -1 ? null : this.selectedEcomID;
        this.storeConfigMethod.EcomType.Key = 1;
        // this.storeConfigMethod.EcomType.Value = this.ecomTypeKeyValue.find(e => e.Key == this.selectedEcomID).Value;
        this.storeConfigService.update(this.storeConfigMethod).subscribe(
            pm => {
                if (this.storeConfigMethod.StoreId.toString() == this._storeService.getStoreID()) {
                    this._storeService.setStore(this.storeConfigMethod.Name);
                    this._storeService.setStoreKey(this.storeConfigMethod.StoreKey);
                }
                toastr.success("Store Configuration has been updated successfully.");
                let index: number = this.storeConfigMethods.findIndex(
                    pm => pm.StoreId == this.storeConfigMethod.StoreId
                );
                this.storeConfigMethods.splice(index, 1, this.storeConfigMethod);
                this.dataTable.fullJsonData = this.storeConfigMethods;
                this.dataTable.reload();
            },
            e => {
                toastr.error(
                    "Update failed due to internal error. Refresh your browser and try again."
                );
                this.handleError(e);
            }
        );
    }

    private add() {
        let self: StoreConfigurationTableComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                //this.storeConfigMethod.ModifiedBy = self.currentAppUser.FirstName;
            },
            (err: Response) => { }
        );
        //this.storeConfigMethod.EcomType.Key = this.selectedEcomID == -1 ? null : this.selectedEcomID;
        this.storeConfigMethod.EcomType.Key = 1;


        if (this.duplicateIdLocal != null) {
            this.storeConfigMethod.DuplicateOf = parseInt(this.duplicateIdLocal);
        }

        if (this.duplicateIdLocal == null) {
            this.storeConfigMethod.DuplicateOf = 1;
        }

        this.storeConfigMethod.appList = [];
        this.storeConfigMethod.appList = this.ApplicationSettingList

        this.storeConfigService.add(this.storeConfigMethod).subscribe(
            pm => {
                if (this.storeConfigMethod.StoreId.toString() == this._storeService.getStoreID()) {
                    this._storeService.setStore(this.storeConfigMethod.Name);
                    this._storeService.setStoreKey(this.storeConfigMethod.StoreKey);
                }
                toastr.success("Store Configuration has been added successfully.");
                this.storeConfigMethods.push(pm);
                this.dataTable.fullJsonData = this.storeConfigMethods;
                this.storeConfigMethod.appList = [];
                this.dataTable.reload();

            },
            e => {
                toastr.error(
                    "Update failed due to internal error. Refresh your browser and try again."
                );
                this.handleError(e);
            }
        );

    }

    currentLoggedUser() {
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(this.currentAppUser, data);
                this.store = this._storeService.getStore();
            },
            (err: Response) => { }
        );
    }
}
