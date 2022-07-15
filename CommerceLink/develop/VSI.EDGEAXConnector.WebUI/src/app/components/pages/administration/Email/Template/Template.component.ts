import { Subject } from "rxjs/Subject";
import {
    ChangeDetectorRef,
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit,
    HostBinding,
    ViewChild
} from "@angular/core";
import { Observable, Observer } from "rxjs";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { EmailTemplate } from "../../../../../Entities/EmailTemplate";
import { EmailSettingsService } from "../../../../../_services/emailsettings.service";
import { UserService } from "../../../../../_services/user.service";
import { UserDetail, ApplicationUser } from "../../../../../Entities/UserDetail";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";
import { EmailComponent } from "../email.component";
import { StoreService } from "../../../../../_services/store.service";

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./Template.component.html",
    styleUrls: ["./Template.component.css"],
    encapsulation: ViewEncapsulation.None
})
export class TemplateComponent implements OnInit {

    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";
    @ViewChild('editor') editor;
    @ViewChild('footerAce') footerAce;
    public dataTable: any = null;
    public emailTemplateArr: EmailTemplate[];
    public emailTemplate: EmailTemplate;
    public showError: boolean = false;
    public templateToDelete: number = -1;
    public modalMode: string = "";
    public buttonText: string = "";
    public readonly editMode = "Edit";
    public readonly addMode = "Add";
    private currentAppUser: ApplicationUser = null;
    appUser: ApplicationUser = null;

    constructor(private _emailTemplateService: EmailSettingsService, private _userService: UserService, private _storeService: StoreService) {
        this.appUser = new ApplicationUser();
        this.emailTemplate = new EmailTemplate();
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

        this.hideXmlTooltipBody();
        this.hideXmlTooltipFooter();
        this._emailTemplateService.GetEmailTemplates().subscribe(
            emailtemp => {
                this.emailTemplateArr = emailtemp;
                this.modifyBodyHTMLForUI();
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve Email Template.");
                this.HandleError(e);
            }
        );
    }
    public hideXmlTooltipBody() {
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
    public hideXmlTooltipFooter() {
        var session = this.footerAce.nativeElement.env.editor.getSession();

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
    public showSettings() {
        this.createTable();
        this.initEvents();
    }

    public createTable(): void {
        if (this.dataTable == null) {
            var self: TemplateComponent = this;
            this.dataTable = $("#emailTemplate").mDatatable({
                data: {
                    type: "local",
                    source: this.emailTemplateArr,
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
                        title: "Id",
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
                        field: "Name",
                        title: "Name",
                        textAlign: "left",
                        width: 130
                    },
                    {
                        field: "Subject",
                        title: "Subject",
                        textAlign: "left",
                        width: 150
                    },
                    {
                        field: "Body",
                        title: "Body",
                        textAlign: "left",
                        width: 270
                    },
                    {
                        field: "Footer",
                        title: "Footer",
                        textAlign: "left",
                        width: 90
                    },
                    {
                        field: "IsActive",
                        title: "Active",
                        width: 60,
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
                        width: 75,
                        textAlign: "left",
                        template: function(e: any): string {
                            return ("<span><a data-toggle='modal' data-target='#emailTemplateModal' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>" +
                                "<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill' title='Delete'><i class='la la-trash'></i></a></span>");
                        }
                    }
                ]
            });
        }
    }

    public initEvents(): void {
        let self: TemplateComponent = this;
        (function(component: TemplateComponent) {
            $("#emailTemplate").on(
                "click",
                "tr > td:nth-child(8) > span> span:nth-child(1)",
                function() {
                    //edit button clicked

                    var templateId = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.editEmailTemplate(templateId);

                    return false;
                }
            );
        })(this);

        (function(component: TemplateComponent) {
            $("#emailTemplate").on(
                "click",
                "tr > td:nth-child(8) > span > span:nth-child(2)",
                function() {
                    //delete button clicked
                    self.templateToDelete = parseInt(
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
    }
    public onSubmit(isFormValid: boolean) {

        if (isFormValid) {
            // console.log(this.emailTemplate);
            this.trimTemplateFields();
            if (this.emailTemplate.Body == "" || this.emailTemplate.Footer == "") {
                this.showError = true;
                return false;
            }

            if (this.modalMode == this.addMode) {

                this.addEmailTemplate();

            } else if (this.modalMode == this.editMode) {

                this.updateEmailTemplate();
            }
            $("#emailTemplateModal").modal("hide");
        } else {
            this.showError = true;
            return false;
        }
    }
    private addEmailTemplate() {

        let self: TemplateComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                // self.store = self._storeService.getStore();
                this.emailTemplate.CreatedBy = self.currentAppUser.LastName;
            },
            (err: Response) => { }
        );
        this._emailTemplateService.AddEmailTemplate(this.emailTemplate).subscribe(
            et => {
                toastr.success("Email Template has been added successfully.");
                et.Body = et.Body.replace(/</g, "&lt;");
                this.emailTemplateArr.push(et);
                this.dataTable.fullJsonData = this.emailTemplateArr;
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
    editEmailTemplate(emailTemplateId: number) {
        this.showError = false;
        this.emailTemplate = JSON.parse(
            JSON.stringify(this.emailTemplateArr.find(e => e.Id == emailTemplateId))
        );
        this.emailTemplate.Body = this.emailTemplate.Body.replace(/&lt;/g, "<");
        this.showModal(this.editMode);
    }
    updateEmailTemplate() {
        let self: TemplateComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                // self.store = self._storeService.getStore();
                this.emailTemplate.ModifiedBy = self.currentAppUser.LastName;
            },
            (err: Response) => { }
        );
        this._emailTemplateService
            .UpdateEmailTemplate(this.emailTemplate)
            .subscribe(
            et => {
                toastr.success("Email Template has been updated successfully.");
                this.emailTemplate.Body = this.emailTemplate.Body.replace(
                    /</g,
                    "&lt;"
                );
                let index: number = this.emailTemplateArr.findIndex(
                    et => et.Id == this.emailTemplate.Id
                );
                this.emailTemplateArr.splice(index, 1, this.emailTemplate);
                this.dataTable.fullJsonData = this.emailTemplateArr;
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
    trimTemplateFields() {
        this.emailTemplate.Name = this.emailTemplate.Name.trim();
        this.emailTemplate.Subject = this.emailTemplate.Subject.trim();
        this.emailTemplate.Body = this.emailTemplate.Body.trim();
        this.emailTemplate.Footer = this.emailTemplate.Footer.trim();
    }
    public showModalForAdd() {
        this.emailTemplate = null;
        this.emailTemplate = new EmailTemplate();
        this.emailTemplate.StoreId = parseInt(this._storeService.getStoreID());
        this.showModal(this.addMode);
    }
    private showModal(modalMode) {
        this.showError = false;
        if (modalMode == this.editMode) {
            this.buttonText = "Update";
        } else {
            this.buttonText = "Save";
        }

        this.modalMode = modalMode;
        $("#emailTemplateModal").modal("show");
    }
    private showConfirmationModal() {
        $("#templateDeleteModal").modal("show");
    }
    public deleteConfirmedByUser() {
        this.deleteEmailTemplate(this.templateToDelete);
        this.templateToDelete = -1;
    }
    private deleteEmailTemplate(templateToDelete: number) {
        this._emailTemplateService
            .DeleteEmailTemplate(templateToDelete)
            .subscribe(res => {
                console.log(res.json());
                this.processAPIResponse(res.json(), templateToDelete);
            });
    }
    private processAPIResponse(response: string, templateToDelete: number): void {
        switch (response) {
            case "Success":
                this.emailTemplateArr = this.emailTemplateArr.filter(
                    et => et.Id !== templateToDelete
                );
                this.dataTable.fullJsonData = this.emailTemplateArr;
                this.dataTable.reload();
                toastr.success("Email template has been deleted successfully.");
                break;

            case "NotFound":
                toastr.error(
                    "Email template does not exists. Please refresh your page."
                );
                break;

            case "Subscribed":
                toastr.error(
                    "Subscribed email template cannot be deleted. Please remove template subscription first."
                );
                break;

            case "Failure":
                toastr.error("There is a problem with server. Please try again!.");
                break;
        }
    }
    private modifyBodyHTMLForUI() {
        this.emailTemplateArr.forEach(et => {
            et.Body = et.Body.replace(/</g, "&lt;");
        });
    }
    HandleError(e: any) {
        console.error(e);
    }
}
