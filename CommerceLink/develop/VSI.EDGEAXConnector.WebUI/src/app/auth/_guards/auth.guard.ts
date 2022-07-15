import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { UserService } from "../../_services/user.service";
import { Observable, Observer } from "rxjs/Rx";
import { UserDetail } from "../../Entities/UserDetail"

@Injectable()
export class AuthGuard implements CanActivate {

    private userDetails: UserDetail = null;
    constructor(private _router: Router, private _userService: UserService) {
        this.userDetails = new UserDetail();
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        //return Observable.of(true);
        return this._userService.verify().switchMap((result: Array<Object>) => {
            let user: UserDetail = new UserDetail();
            Object.keys(user).forEach(key => {
                let listValue = result.filter(m => m["m_type"] == key);
                if (listValue.length > 0) {
                    user[key] = result.filter(m => m["m_type"] == key)[0]["m_value"];
                }
            });
            this.userDetails = user;
            return Observable.of(true);
        }).catch(() => {
            this._router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return Observable.of(false);
        });
    }
}
