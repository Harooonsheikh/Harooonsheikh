import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { Helpers } from "../../../../helpers";
import { UserService } from "../../../../_services/user.service";
import { ApplicationUser } from "../../../../Entities/UserDetail";
import { FormsModule } from "@angular/forms";
import { AppUser } from "../../../../Entities/AppUser";
import { KeyValue, KeyValuePair } from "../../../../Entities/Common";
import { Message } from "@angular/compiler/src/i18n/i18n_ast";
declare var jquery: any;
declare var $: any;
declare var toastr: any;

@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./header-profile.component.html",
    encapsulation: ViewEncapsulation.None
})
export class HeaderProfileComponent implements OnInit {
    public appUser: ApplicationUser = null;
    public newUser: AppUser = null;
    public selectedRole: string = "PS";
    public selectedStore: number = -1;
    public showError: boolean = false;
    public isUserNameTaken: boolean = false;
    public isSuperAdmin: boolean = false;
    public currentUserRoles: Array<string>;
    public isEmailRegistered: boolean = false;
    public invalidForm: boolean = false;
    public UserRoles: Array<KeyValue<string>> = null;
    public AppStores: Array<KeyValuePair<string>> = null;

    constructor(private _userService: UserService) {
        this.appUser = new ApplicationUser();
        this.isSuperAdmin = false;
        this.newUser = new AppUser();
        this.selectedRole = "PS";
        this.selectedStore = -1;
        this.showError = false;
        this.isUserNameTaken = false;
        this.invalidForm = false;
        this.currentUserRoles = new Array<string>();
        this.UserRoles = new Array<KeyValue<string>>();
        this.AppStores = new Array<KeyValuePair<string>>();
    }
    ngOnInit() {
        this.verifySuperAdmin();
        this.initializeToaster();
        this._userService.get().subscribe(
            user => {
                this.newUser = user;
                this.AppStores = JSON.parse(JSON.stringify(user.AppStores));
                this.UserRoles = JSON.parse(JSON.stringify(user.UserRoles));
            },
            e => {
                let result = this.processAPIResponse(e.json());
                this.handleError(e);
            }
        );
    }
    initializeToaster() {
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
    verifySuperAdmin() {
        this.currentUserRoles = null;
        this.currentUserRoles = new Array<string>();
        this._userService.getCurrentUser()
            .subscribe((data: ApplicationUser) => {
                Object.assign(this.appUser, data);
                this.getUserRole(data.Id)
            }, (err: Response) => {
                toastr.error("Could not get current user.")
                this.handleError(err);
            });

    }
    getUserRole(userId: string) {
        this._userService.getUserRole(userId).subscribe(userRoles => {
            this.currentUserRoles = userRoles;
            let role = this.currentUserRoles.find(r => r == "SuperAdmin");
            if (role != null) {
                this.isSuperAdmin = true;
            }
        }, e => {
            toastr.error("Could not retrieve user roles.")
            this.handleError(e);
        });
    }
    private onSubmit(isFormValid: boolean) {
        this.isEmailRegistered = false;

        if (isFormValid == false || this.selectedRole == "PS" || (this.selectedRole != "SuperAdmin" && this.selectedStore == -1)) {
            this.showError = true;
            return false;
        }
        else {
            this.createUser();
        }

    }
    createUser() {
        this.newUser.AppStores[0].Key = this.selectedRole == "SuperAdmin" ? null : this.selectedStore;
        this.newUser.UserRoles[0].Value = this.selectedRole;
        this.newUser.EmailConfirmed = true;
        this._userService.create(this.newUser).subscribe(
            sa => {
                if (sa.json() == "Success") {
                    toastr.success("User has been added successfully.");
                    this.resetForm();
                }
            },
            e => {
                let result = this.processAPIResponse(e.json());
                this.handleError(e);
            }
        );
    }
    verifyUserName(userName: string, isValid: boolean) {
        if (isValid) {
            this.isUserNameTaken = false;
            this._userService.verifyUserName(userName).subscribe(
                res => {
                    if (res.json()) {
                        this.isUserNameTaken = true;
                    }
                },
                e => {
                    toastr.error("Some error occured while verifying user name.");
                    this.handleError(e);
                }
            );
        }
    }

    private processAPIResponse(response: any): boolean {
        let result = false;
        switch (response.Message) {
            case "UserNameTaken":
                this.isUserNameTaken = true;
                this.invalidForm = true;
                break;

            case "EmailRegistered":
                this.isEmailRegistered = true;
                this.invalidForm = true;
                break;
            case "EmailRegisteredUserNameTaken":
                this.isEmailRegistered = true;
                this.isUserNameTaken = true;
                this.invalidForm = true;
                break;

            case "StoreNotFound":
                toastr.error("Kindly enable any store first before creating user.");
                break;
        }
        return result;
    }
    resetForm() {
        this.selectedRole = "PS";
        this.selectedStore = -1;
        this.newUser = null;
        this.newUser = new AppUser();
        this.newUser.AppStores = JSON.parse(JSON.stringify(this.AppStores));
        this.newUser.UserRoles = JSON.parse(JSON.stringify(this.UserRoles));
        this.showError = false;
        this.isUserNameTaken = false;
        this.invalidForm = false;
    }
    resetError() {
        this.isEmailRegistered = false;
    }
    handleError(e: any) {
        console.error(e);
    }
}
