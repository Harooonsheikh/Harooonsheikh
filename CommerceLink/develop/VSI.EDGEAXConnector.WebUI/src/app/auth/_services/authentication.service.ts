import { Injectable } from "@angular/core";
import { Http, Response, RequestOptions, Headers, RequestMethod, URLSearchParams } from "@angular/http";
import "rxjs/add/operator/map";
import { Observable, Observer } from 'rxjs';
import { User } from "../../Entities/User"
import { StoreService } from "../../_services/store.service";
import { UserService } from "../../_services/user.service";
import { environment } from "../../../environments/environment";

@Injectable()
export class AuthenticationService {

    constructor(private http: Http, private _storeService: StoreService, private _userService: UserService) {
    }

    login(username: string, password: string) {
        let option: RequestOptions = new RequestOptions();
        option.headers = new Headers();
        option.headers.append('Content-Type', 'application/x-www-form-urlencoded');
        return this.http.post(environment.baseUrl + '/oauth/token', 'grant_type=password&username=' + username + '&password=' + password, option)
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user: User = response.json();
                if (user != null && user.access_token != null) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
            });
    }

    logout() {
        // remove user from local storage to log user out
        this._userService.clearUser();
        this._storeService.clearStore();
    }
}