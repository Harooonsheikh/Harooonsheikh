import {
  Component,
  ComponentFactoryResolver,
  OnInit,
  AfterViewInit,
  ViewChild,
  ViewContainerRef,
  ViewEncapsulation
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ScriptLoaderService } from "../_services/script-loader.service";
import { AuthenticationService } from "./_services/authentication.service";
import { AlertService } from "./_services/alert.service";
import { UserService } from "../_services/user.service";
import { AlertComponent } from "./_directives/alert.component";
import { LoginCustom } from "./_helpers/login-custom";
import { Helpers } from "../helpers";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable, Observer } from "rxjs";
import { ApplicationUser } from "../Entities/UserDetail";
import { StoreService } from "../_services/store.service";
import { KeyValue, KeyValuePair } from "../Entities/Common";
declare var jquery: any;
declare var $: any;
declare var toastr: any;

@Component({
  selector: ".m-grid.m-grid--hor.m-grid--root.m-page",
  templateUrl: "./templates/login.component.html",
  encapsulation: ViewEncapsulation.None
})
export class AuthComponent implements OnInit, AfterViewInit {
  model: any = {};
  loading = false;
  returnUrl: string;
  appUser: ApplicationUser = new ApplicationUser();
  code: string = "";
  store: KeyValuePair<string> = new KeyValuePair<string>();

  @ViewChild("alertSignin", { read: ViewContainerRef })
  alertSignin: ViewContainerRef;
  @ViewChild("alertSignup", { read: ViewContainerRef })
  alertSignup: ViewContainerRef;
  @ViewChild("alertForgotPass", { read: ViewContainerRef })
  alertForgotPass: ViewContainerRef;
  @ViewChild("alertChangePass", { read: ViewContainerRef })
  alertChangePass: ViewContainerRef;

  constructor(
    private _router: Router,
    private _script: ScriptLoaderService,
    private _userService: UserService,
    private _route: ActivatedRoute,
    private _authService: AuthenticationService,
    private _alertService: AlertService,
    private _storeService: StoreService,
    private cfr: ComponentFactoryResolver
  ) {}

  ngOnInit() {
    this.model.store = "";
    this.model.remember = true;
    let userId: string = this._route.snapshot.queryParams["userId"];
    this.code = this._route.snapshot.queryParams["code"];
    // get return url from route parameters or default to '/'
    this.returnUrl = this._route.snapshot.queryParams["returnUrl"] || "/";
    if (userId != null && this.code != null) {
      this.code = this.code;
      this._router.navigate([this.returnUrl]);
    }

    this._script
      .load(
        "body",
        "assets/vendors/base/vendors.bundle.js",
        "assets/demo/default/base/scripts.bundle.js"
      )
      .then(() => {
        Helpers.setLoading(false);
        LoginCustom.init();
        if (userId && this.code) {
          LoginCustom.displayChangePasswordForm();
          this._userService.getUserDetails(userId).subscribe(
            (res: Response) => {
              this.appUser = new ApplicationUser();
              Object.assign<ApplicationUser, Object>(this.appUser, res);
              this._userService
                .isUserConfirmed(this.appUser.Id, this.code)
                .subscribe(
                  (confirm: boolean) => {
                    //this.code = confirm;
                  },
                  (error: Response) => {
                    this.showAlert("alertSignin");
                    this._alertService.error(
                      "User is either not confirmed or existed.",
                      true
                    );
                    LoginCustom.displaySignInForm();
                  }
                );
            },
            err => {
              this.showAlert("alertSignin");
              this._alertService.error("Unable to get User details.", true);
              LoginCustom.displaySignInForm();
            }
          );
        }
      });
  }
  ngAfterViewInit() {
    this.initializeToaster();
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
  signin() {
    this.loading = true;
    this._authService.login(this.model.username, this.model.password).subscribe(
      data => {
        this.getUserID();
      },
      (error: Response) => {
        this.showAlert("alertSignin");
        let errJson: any = error.json();
        if (errJson.error == "invalid_grant") {
          this._alertService.error("Please enter valid credentials");
        } else if (errJson.error == "locked_out") {
          this._alertService.error(errJson.error_description);
        } else {
          this._alertService.error(errJson.error);
        }

        this.loading = false;
      }
    );
  }
  getUserID() {
    let appUser: ApplicationUser = new ApplicationUser();
    this._userService.getCurrentUser().subscribe(
      (data: ApplicationUser) => {
        Object.assign(appUser, data);
        this.setUserStore(appUser.Id);
      },
      (err: Response) => {
        toastr.error("Could not get current user.");
        this.handleError(err);
      }
    );
  }
  setUserStore(userId: string) {
    this._userService.getUserStore(userId).subscribe(
      store => {
        this.store = store;
        if (this.store != null) {
          this._storeService
            .getStoreDetail(this.store.Key)
            .subscribe(storeDetail => {
              this._storeService.setStoreKey(storeDetail.StoreKey);
              this._storeService.setStore(storeDetail.Name);
              this._storeService.setStoreID(storeDetail.StoreId.toString());
              this._router.navigate([this.returnUrl]);
            });
        } else {
          toastr.error("Could not get user store.");
        }
      },
      e => {
        let result = this.processAPIResponse(e.json());
        this.handleError(e);
      }
    );
  }
  processAPIResponse(response: any): boolean {
    let result = false;
    this.showAlert("alertSignin");
    this.loading = false;
    this._userService.clearUser();
    this._storeService.clearStore();
    switch (response.Message) {
      case "UserNotFound":
        this._alertService.error("Could not find user.");
        break;

      case "StoreDisabled":
        this._alertService.error(
          "User can't be logged-in, as user store is disabled."
        );
        break;
    }
    return result;
  }
  signup() {
    this.loading = true;
    this._userService.create(this.model).subscribe(
      data => {
        this.showAlert("alertSignin");
        this._alertService.success(
          "Thank you. To complete your registration please check your email.",
          true
        );
        this.loading = false;
        LoginCustom.displaySignInForm();
        this.model = {};
      },
      error => {
        this.showAlert("alertSignup");
        this._alertService.error(error);
        this.loading = false;
      }
    );
  }

  forgotPass() {
    this.loading = true;
    this._userService.forgotPassword(this.model.email).subscribe(
      data => {
        this.showAlert("alertSignin");
        this._alertService.success(
          "Email has been sent. Please check your email.",
          true
        );
        this.loading = false;
        LoginCustom.displaySignInForm();
        this.model = {};
      },
      error => {
        this.showAlert("alertForgotPass");
        this._alertService.error(error);
        this.loading = false;
      }
    );
  }

  changePass() {
    if (this.model.password != this.model.confirmPassword) {
      this.showAlert("alertChangePass");
      this._alertService.error("Password doesn't match.");
      this.loading = false;
    } else {
      this._userService
        .changePassword(this.appUser.Email, this.code, this.model.password)
        .subscribe(
          (data: boolean) => {
            this.showAlert("alertSignin");
            this._alertService.success(
              "Pasword has been change. Login with new password."
            );
            LoginCustom.displaySignInForm();
          },
          (err: Response) => {
            this.showAlert("alertChangePass");
            this._alertService.error(
              "Error in Changing Password. Password should be of 6 length and must contain upper case character."
            );
          }
        );
    }
  }

  showAlert(target) {
    this[target].clear();
    let factory = this.cfr.resolveComponentFactory(AlertComponent);
    let ref = this[target].createComponent(factory);
    ref.changeDetectorRef.detectChanges();
  }
  handleError(e: any) {
    console.error(e);
  }
}
