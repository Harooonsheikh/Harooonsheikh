import {
    Component,
    OnInit,
    ViewEncapsulation,
    AfterViewInit
} from "@angular/core";
import { Helpers } from "../../../helpers";
import { WorkFlowService } from "../../../_services/workflow.service";
import { Observable } from "rxjs/Observable";

import { WorkFlow } from "../../../Entities/WorkFlow";
import { Router, ActivatedRoute } from "@angular/router";
import { ScreenNamesService } from "../../../_services/screennames.service";
import { UserService } from "../../../_services/user.service";
import { ApplicationUser } from "../../../Entities/UserDetail";
// import { DatafileComponent } from '../../components/pages/configuration/datafile/datafile.component';
declare let mLayout: any;
declare var jquery: any;
declare var $: any;
declare var toastr: any;
@Component({
    selector: "nav-bar",
    templateUrl: "./nav-bar.component.html",
    encapsulation: ViewEncapsulation.None
})
export class NavBarComponent implements OnInit, AfterViewInit {
    public model: Array<any> = null;
    public isSuperAdmin: boolean = false;
    public isAdmin: boolean = false;
    public currentUserRoles: Array<string>;
    public appUser: ApplicationUser = null;
    public selectedAppSetting: string = null;
    public expandAppSetting: boolean = false;
    public screenList: Array<any>;
    constructor(
        private _screenNames: ScreenNamesService,
        private route: ActivatedRoute,
        private router: Router,
        private _userService: UserService
    ) {
        this.isSuperAdmin = false;
        this.isAdmin = false;
        this.appUser = new ApplicationUser();
        this.currentUserRoles = new Array<string>();
    }
    ngOnInit() {
        if (window.location.pathname == "/ApplicationSettings/setting") {
            this.selectedAppSetting = this._screenNames.getScreenName();
            if (this.selectedAppSetting != null) {
                this.expandAppSetting = true;
            }
        }
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

        if (this._screenNames.getScreenNames() == null) {
            this._screenNames
                .getScreens().subscribe(m => this.createForm(m), e => this.handleError(e));
            this.verifySuperAdmin();
        }
        else {
            this.screenList = new Array<any>();
            this.screenList = this._screenNames.getScreenNames();
            this.createForm(this.screenList);
            this.verifySuperAdmin();
        }
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
            let superAdmin = this.currentUserRoles.find(r => r == "SuperAdmin");
            if (superAdmin != null) {
                this.isSuperAdmin = true;
            }
            let admin = this.currentUserRoles.find(r => r == "Admin");
            if (admin != null) {
                this.isAdmin = true;
            }
        }, e => {
            toastr.error("Could not retrieve user roles.")
            this.handleError(e);
        });
    }
    createForm(m) {

        this.model = m;
    }
    private handleError(err: Response): void {
        toastr.error("Unable to load screen names.");
        console.error(err);
    }
    routeScreen(screen: string) {
        this.selectedAppSetting = screen
        this._screenNames.setScreenName(screen);
        this.expandAppSetting = true;
        this.router.navigate(["/ApplicationSettings/setting/"], {
            queryParams: { screen: screen }
        });
        this.expandAppSetting = true;
    }
    ngAfterViewInit() {
        mLayout.initAside();

        let menu = (<any>$("#m_aside_left")).mMenu();
        let item = $(menu)
            .find('a[href="' + window.location.pathname + '"]')
            .parent(".m-menu__item");
        (<any>$(menu).data("menu")).setActiveItem(item);
    }
}
