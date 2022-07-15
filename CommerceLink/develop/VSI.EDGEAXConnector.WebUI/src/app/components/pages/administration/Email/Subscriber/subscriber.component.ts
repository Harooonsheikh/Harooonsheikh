import { Subject, SubjectSubscriber } from "rxjs/Subject";
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
import { EmailSettingsService } from "../../../../../_services/emailsettings.service";
declare var jquery: any;
declare var $: any;
declare var toastr: any;
import { RequiredValidator } from "@angular/forms";
import {
    Subscriber,
    Template,
    EmailSubscriber
} from "../../../../../Entities/Subscriber";
import { EmailTemplate } from "../../../../../Entities/EmailTemplate";
import { UserService } from "../../../../../_services/user.service";
import { UserDetail, ApplicationUser } from "../../../../../Entities/UserDetail";
@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./subscriber.component.html",
    styleUrls: ["./subscriber.component.css"],
    encapsulation: ViewEncapsulation.None
})
export class SubscriberComponent implements OnInit {
    @HostBinding("class") classes = "m-grid__item m-grid__item--fluid m-wrapper";

    public dataTable: any = null;
    public subscribersArr: Subscriber[];
    public subscriber: Subscriber;
    public showError: boolean = false;
    public subscriberToDelete: number = -1;
    public modalMode: string = "";
    public buttonText: string = "";
    public readonly editMode = "Edit";
    public readonly addMode = "Add";
    public emailTemplateArr: EmailTemplate[];
    public templatesWithSubscriptions: Template[];
    private currentAppUser: ApplicationUser = null;
    appUser: ApplicationUser = null;
    constructor(private _emailTemplateService: EmailSettingsService, private _userService: UserService) {
        this.subscriber = new Subscriber();
        this.templatesWithSubscriptions = new Array<Template>();
        this.appUser = new ApplicationUser();
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
        this._emailTemplateService.GetSubscribers().subscribe(
            emailSub => {
                this.subscribersArr = emailSub;
                this.showSettings();
            },
            e => {
                toastr.error("Could not retrieve Email Subscribers.");
                this.HandleError(e);
            }
        );
        this._emailTemplateService.GetEmailTemplates().subscribe(
            emailtemp => {
                this.emailTemplateArr = emailtemp;
            },
            e => {
                toastr.error("Could not retrieve Email Template.");
                this.HandleError(e);
            }
        );
    }

    public showSettings() {
        this.createTable();
        this.initEvents();
    }

    public createTable(): void {
        if (this.dataTable == null) {
            var self: SubscriberComponent = this;
            this.dataTable = $("#emailSubscriber").mDatatable({
                data: {
                    type: "local",
                    source: this.subscribersArr,
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
                        width: 13,
                        textAlign: "left",
                        template: function(e: any): number {

                            let pagi: any = e.getDatatable().getDataSourceParam('pagination');
                            var perPage = e.getDatatable().getPageSize();
                            return ((pagi.page - 1) * perPage) + e.rowIndex + 1;

                        }
                    },
                    {
                        field: "Email",
                        title: "Email",
                        textAlign: "left",
                        width: 300
                    },
                    {
                        field: "Name",
                        title: "Name",
                        textAlign: "left",
                        width: 200
                    },
                    {
                        field: "IsActive",
                        title: "Active",
                        width: 80,
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
                        width: 80,
                        textAlign: "left",
                        template: function(e: any): string {
                            return ("<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='Edit'><i class='la la-edit'></i></a></span>"
                                + "<span><a class='m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill' title='Delete'><i class='la la-trash'></i></a></span>");
                        }
                    }
                ]
            });
        }
    }

    public initEvents(): void {
        let self: SubscriberComponent = this;
        (function(component: SubscriberComponent) {
            $("#emailSubscriber").on(
                "click",
                "tr > td:nth-child(6) > span > span:nth-child(1)",
                function() {
                    //edit button clicked

                    var subscriberId = parseInt(
                        $(this)
                            .parent()
                            .parent()
                            .parent()
                            .children("td:nth-child(1)")
                            .text()
                    );
                    self.editEmailSubscriber(subscriberId);

                    return false;
                }
            );
        })(this);

        (function(component: SubscriberComponent) {
            $("#emailSubscriber").on(
                "click",
                "tr > td:nth-child(6) > span> span:nth-child(2)",
                function() {
                    //delete button clicked
                    self.subscriberToDelete = parseInt(
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
            this.trimTemplateFields();
            if (this.modalMode == this.addMode) {
                this.addSubscriber();
            } else if (this.modalMode == this.editMode) {
                this.updateSubscriber();
            }
            $("#emailSubscriberModal").modal("hide");
        } else {
            this.showError = true;
            return false;
        }
    }
    private addSubscriber() {
        let self: SubscriberComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                // self.store = self._storeService.getStore();
                this.subscriber.CreatedByUser = self.currentAppUser.LastName;
            },
            (err: Response) => { }
        );

        this.setSubscribedTemplates();
        this._emailTemplateService.AddSubscriber(this.subscriber).subscribe(
            sub => {
                toastr.success("Email Subscriber has been added successfully.");
                this.subscribersArr.push(sub);
                this.dataTable.fullJsonData = this.subscribersArr;
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
    updateSubscriber() {
        let self: SubscriberComponent = this;
        self.currentAppUser = new ApplicationUser();
        this._userService.getCurrentUser().subscribe(
            (data: ApplicationUser) => {
                Object.assign(self.currentAppUser, data);
                // self.store = self._storeService.getStore();
                this.subscriber.ModifiedByUser = self.currentAppUser.LastName;
            },
            (err: Response) => { }
        );
        this.setSubscribedTemplates();
        this._emailTemplateService.UpdateSubscriber(this.subscriber).subscribe(
            sub => {

                toastr.success("Email Subscriber has been updated successfully.");
                let index: number = this.subscribersArr.findIndex(
                    es => es.Id == this.subscriber.Id
                );
                this.subscribersArr.splice(index, 1, this.subscriber);
                this.dataTable.fullJsonData = this.subscribersArr;
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
    setSubscribedTemplates() {
        this.subscriber.EmailSubscribers = null;
        this.subscriber.EmailSubscribers = new Array<EmailSubscriber>();
        this.templatesWithSubscriptions.forEach(temp => {
            if (temp.IsSelected) {
                let emailSubscriber = new EmailSubscriber();
                emailSubscriber.SubscriberId = this.subscriber.Id;
                emailSubscriber.TemplateId = temp.TemplateId;
                this.subscriber.EmailSubscribers.push(emailSubscriber);
            }
        });
    }
    editEmailSubscriber(subscriberId: number) {
        this.showError = false;
        this.subscriber = JSON.parse(
            JSON.stringify(this.subscribersArr.find(e => e.Id == subscriberId))
        );
        this.setEmailTemplates();
        this.showModal(this.editMode);
    }
    setEmailTemplates() {
        this.templatesWithSubscriptions = null;
        this.templatesWithSubscriptions = new Array<Template>();
        this.emailTemplateArr.forEach(temp => {
            let tempObj: Template = new Template();
            tempObj.TemplateId = temp.Id;
            tempObj.Name = temp.Name;
            var sub = this.subscriber.EmailSubscribers.find(
                es => es.TemplateId == temp.Id
            );
            tempObj.IsSelected = sub == null ? false : true;
            this.templatesWithSubscriptions.push(tempObj);
        });
    }
    trimTemplateFields() {
        this.subscriber.Name = this.subscriber.Name.trim();
        this.subscriber.Email = this.subscriber.Email.trim();
    }
    public showModalForAdd() {
        this.subscriber = null;
        this.subscriber = new Subscriber();
        this.subscriber.StoreId_FK = 1;
        this.setEmailTemplates();
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
        $("#emailSubscriberModal").modal("show");
    }
    private showConfirmationModal() {
        $("#subsciberDeleteModal").modal("show");
    }
    public deleteConfirmedByUser() {

        this.subscriber = this.subscribersArr.find(
            sub => sub.Id == this.subscriberToDelete
        );
        this.deleteSubscriber(this.subscriberToDelete);
        this.subscriberToDelete = -1;
    }
    private deleteSubscriber(subscriberToDelete: number) {
        this._emailTemplateService.DeleteSubscriber(this.subscriber).subscribe(
            res => {

                this.subscribersArr = this.subscribersArr.filter(
                    es => es.Id !== subscriberToDelete
                );
                this.dataTable.fullJsonData = this.subscribersArr;
                this.dataTable.reload();
                toastr.success("Subscriber has been deleted successfully.");
            },
            e => {
                toastr.error(
                    "Delete failed due to internal error. " + e.json().Message
                );
                this.HandleError(e);
            }
        );
    }
    private processAPIResponse(
        response: string,
        subscriberToDelete: number
    ): void {
        switch (response) {
            case "Success":
                this.subscribersArr = this.subscribersArr.filter(
                    es => es.Id !== subscriberToDelete
                );
                this.dataTable.fullJsonData = this.subscribersArr;
                this.dataTable.reload();
                toastr.success("Email subscriber has been deleted successfully.");
                break;

            case "NotFound":
                toastr.error(
                    "Email subscriber does not exists. Please refresh your page."
                );
                break;

            // case "Subscribed":
            //     toastr.error("Subscribed email template cannot be deleted. Please remove template subscription first.");
            //     break;

            case "Failure":
                toastr.error("There is a problem with server. Please try again!.");
                break;
        }
    }
    HandleError(e: any) {
        console.error(e);
    }
}
