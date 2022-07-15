import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { Helpers } from '../../../helpers';
import { UserService } from '../../../_services/user.service';
import { ApplicationUser } from '../../../Entities/UserDetail';
import { ChangeDetectorRef } from '@angular/core';
import { StoreService } from '../../../_services/store.service';
import { KeyValue } from '../../../Entities/Common';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

declare let mLayout: any;
@Component({
    selector: "app-header-nav",
    templateUrl: "./header-nav.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class HeaderNavComponent implements OnInit, AfterViewInit {
    appUser: ApplicationUser = null;
    store: string = "";
    appStore: Array<KeyValue<string>>;
    selectedStore: string = "";
    public isSuperAdmin: boolean = false;
    public currentUserRoles: Array<string>;

    constructor(private _userService: UserService, private cdRef: ChangeDetectorRef, private _storeService: StoreService, private router: Router, private location: Location) {
        this.appUser = new ApplicationUser();
        this.appStore = new Array<KeyValue<string>>();
        this.isSuperAdmin = false;
        this.currentUserRoles = new Array<string>();
    }
    ngOnInit() {
        this.verifySuperAdmin();
    }
    verifySuperAdmin() {
        this.currentUserRoles = null;
        this.currentUserRoles = new Array<string>();
        this._userService.getCurrentUser()
            .subscribe((data: ApplicationUser) => {
                Object.assign(this.appUser, data);
                this.getUserRole(data.Id)
            }, (err: Response) => {
                this.handleError(err);
            });

    }
    getUserRole(userId: string) {
        this._userService.getUserRole(userId).subscribe(userRoles => {
            this.currentUserRoles = userRoles;
            let superAdmin = this.currentUserRoles.find(r => r == "SuperAdmin");
            if (superAdmin != null) {
                this.isSuperAdmin = true;
                this.setAppStores();
            }
        }, e => {
            this.handleError(e);
        });
    }

    goBack() {
        if (window.history.length > 1) {
            this.location.back();
        } else {
            this.router.navigate(['/']);
        }
    }

    setAppStores() {
        // this._storeService.getAllStores().subscribe(
        //     m => this.appStore = m);
        this._storeService.getAllStores().subscribe(s => {
            this.appStore = s;
            this.selectedStore = this._storeService.getStore();
            var store = this.appStore.find(s => s.Value == this.selectedStore);
            if (store == null) {
                this.selectedStore = this.appStore[0].Value;
                this._storeService.getStoreDetail(parseInt(this.appStore[0].Key)).subscribe(m => {
                    this._storeService.setStore(this.selectedStore);
                    this._storeService.setStoreID(this.appStore[0].Key);
                    this.store = this._storeService.getStore();
                    this._storeService.setStoreKey(m.StoreKey);
                    //this.router.navigate(["/merchandise"]);
                    let location = window.location.origin;
                    window.location.href = location;
                });

            }
        }, e => {
            this.handleError(e);
        });
    }
    storeChanged(store: string) {

        let storeId = parseInt(this.appStore.find(s => s.Value == store).Key);
        this._storeService.getStoreDetail(storeId).subscribe(m => {
            this._storeService.setStoreID(storeId.toString());
            this._storeService.setStore(store);
            this._storeService.setStoreKey(m.StoreKey);
            this.store = this._storeService.getStore();
            let location = window.location.href;
            window.location.href = location;
        });

    }
    ngAfterViewInit() {

        mLayout.initHeader();
        this._userService.getCurrentUser()
            .subscribe((data: ApplicationUser) => {
                Object.assign(this.appUser, data);
                this.store = this._storeService.getStore();
                this.cdRef.detectChanges();
            }, (err: Response) => {

            });
        //this._userService.getUserDetails
    }
    handleError(e: any) {
        console.error(e);
    }

}