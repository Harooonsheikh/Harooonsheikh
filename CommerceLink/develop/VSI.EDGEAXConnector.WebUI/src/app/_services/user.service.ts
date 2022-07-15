import { Injectable } from "@angular/core";
import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable, Observer } from 'rxjs';
import { User } from "../Entities/User"
import { UserDetail, ApplicationUser } from "../Entities/UserDetail"
import { AuthenticationService } from "../auth/_services/authentication.service"
import { AppUser } from "../Entities/AppUser";
import { KeyValuePair } from "../Entities/Common";
import { environment } from "../../environments/environment";


@Injectable()
export class UserService {
    private currentUser: User = null;
    private userDetail: UserDetail = null;
    private currentAppUser: ApplicationUser = null;
    private readonly API_ROUTEUSERMANAGER: string = environment.baseUrl + "/api/usermanager/";
    private readonly API_ROUTE_ACCOUNT: string = environment.baseUrl + "/api/accounts/";
    private readonly API_GET: string = this.API_ROUTEUSERMANAGER + "Get";
    private readonly API_CREATE: string = this.API_ROUTEUSERMANAGER + "Create";
    private readonly API_VERIFYUSERNAME: string = this.API_ROUTEUSERMANAGER + "VerifyUserName";
    private readonly API_GETUSERROLE: string = this.API_ROUTEUSERMANAGER + "Role";
    private readonly API_GETUSERSTORE: string = this.API_ROUTEUSERMANAGER + "Store";
    private readonly API_GETUSERBYTOKEN: string = this.API_ROUTE_ACCOUNT + "GetUserByToken";
    private readonly API_FORGOTPASSWORD: string = this.API_ROUTE_ACCOUNT + "ForgotPassword";
    private readonly API_GETUSERBYID: string = this.API_ROUTE_ACCOUNT + "GetUserByUserId";
    private readonly API_ISUSERCONFIRMED: string = this.API_ROUTE_ACCOUNT + "IsUserConfirmed";
    private readonly API_RESETPASSWORD: string = this.API_ROUTE_ACCOUNT + "ResetPassword";
    private readonly API_ROUTE_USER: string = environment.baseUrl + "/api/users/";

    constructor(private http: Http) {
    }

    verify(): Observable<any> {
        return Observable.create((ob: Observer<any>) => {
            let option: RequestOptions = this.jwt();
            if (option) {
                option.headers.append("Content-Type", "application/json");
                let currentUser = JSON.parse(localStorage.getItem('currentUser'));
                this.http.get(this.API_GETUSERBYTOKEN + '?token=' + currentUser.access_token, option)
                    .map((response: Response) => {
                        return response.json();
                    }).catch(this.handleErrorObservable).subscribe((res: Array<Object>) => {
                        ob.next(res);
                        ob.complete();
                    }, b => {
                        ob.error(b);
                    });
            }
            else {
                ob.error(null);
            }
        });
    }

    handleErrorObservable(error: Response | any) {
        console.error(error.message || error);
        return Observable.throw(error.message || error);
    }

    forgotPassword(email: string): Observable<boolean> {
        let option: RequestOptions = new RequestOptions();
        option.headers = new Headers();
        option.headers.append("content-type", "application/json");
        if (option) {
            option.headers.append("Content-type", "application/json");
            return this.http.get(this.API_FORGOTPASSWORD + 'email=' + email, option).map((response: Response) => {
                return true;
            });
        }
    }

    getUserDetails(userId: string): Observable<Response> {
        let option: RequestOptions = new RequestOptions();
        option.headers = new Headers();
        option.headers.append("content-type", "application/json");
        return this.http.get(this.API_GETUSERBYID + '?Id=' + userId, option).map((ress: Response) => ress.json());
    }

    isUserConfirmed(userId: string, code: string) {
        let option: RequestOptions = new RequestOptions();
        option.headers = new Headers();
        option.headers.append("content-type", "application/json");
        return this.http.get(this.API_ISUSERCONFIRMED + '?userId=' + userId + '&code=' + code).map((res: Response) => { return true; });
    }

    changePassword(email: string, code: string, newPass: string) {
        let option: RequestOptions = new RequestOptions();
        option.headers = new Headers();
        option.headers.append("content-type", "application/json");
        return this.http.get(this.API_RESETPASSWORD + '?email=' + email + '&code=' + code + '&newPassword=' + newPass, option).map((ress: Response) => { return true; });

    }
    getAll() {
        return this.http.get(this.API_ROUTE_USER, this.jwt()).map((response: Response) => response.json());
    }

    getById(id: number) {
        return this.http.get(this.API_ROUTE_USER + id, this.jwt()).map((response: Response) => response.json());
    }

    update(user: User) {
        return this.http.put(this.API_ROUTE_USER + user.access_token, user, this.jwt()).map((response: Response) => response.json());
    }

    delete(id: number) {
        return this.http.delete(this.API_ROUTE_USER + id, this.jwt()).map((response: Response) => response.json());
    }

    getCurrentUser(): Observable<ApplicationUser> {
        let self: UserService = this;
        let curUser = self.getUser();
        if (curUser == null) {
            return this.verify().map((data: any) => {
                let user: UserDetail = new UserDetail();
                Object.keys(user).forEach(key => {
                    let listValue = data.filter(m => m["m_type"] == key);
                    if (listValue.length > 0) {
                        user[key] = data.filter(m => m["m_type"] == key)[0]["m_value"];
                    }
                });
                return user;
            }).switchMap((usr: UserDetail) => {
                return this.getUserDetails(usr.nameid).map((usrRes: Response) => {
                    self.setCurrentUser(usrRes);
                    return self.getUser();
                });
            });
        }
        else {
            return Observable.of(curUser);
        }
    }
    setCurrentUser(appUser: Object) {
        let user: ApplicationUser = new ApplicationUser();
        user.Email = appUser["Email"];
        user.EmailConfirmed = appUser["EmailConfirmed"];
        user.FirstName = appUser["FirstName"];
        user.Id = appUser["Id"];
        user.LastName = appUser["LastName"];
        user.PasswordHash = appUser["PasswordHash"];
        user.UserName = appUser["UserName"];

        localStorage.removeItem('currentUserDetail');
        localStorage.setItem('currentUserDetail', JSON.stringify(user));
    }
    getUser(): ApplicationUser {
        let user: ApplicationUser = JSON.parse(localStorage.getItem('currentUserDetail'));
        return user;
    }
    clearUser(): void {
        localStorage.removeItem('currentUser');
        localStorage.removeItem('currentUserDetail');
    }
    public jwt(): RequestOptions {
        // create authorization header with jwt token
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        let user: User = new User();
        user = Object.assign<User, Object>(user, currentUser);
        if (user.access_token) {
            this.currentUser = user;
            let headers = new Headers({ 'Authorization': 'Bearer ' + user.access_token });
            return new RequestOptions({ headers: headers });
        }
        return null;
    }

    get(): Observable<AppUser> {

        let urlQuery: string = this.API_GET;
        let user: AppUser = new AppUser();

        return this.http.get(urlQuery, this.jwt()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<AppUser, Object>(user, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }

    create(user: AppUser): Observable<Response> {
        let urlQuery: string = this.API_CREATE;
        return this.http.post(urlQuery, user, this.jwt())
    }
    verifyUserName(userName: string): Observable<Response> {
        let urlQuery: string = this.API_VERIFYUSERNAME + "?userName=" + userName;
        return this.http.get(urlQuery, this.jwt())
    }
    getUserRole(userId: string): Observable<Array<string>> {
        let urlQuery: string = this.API_GETUSERROLE + "?userId=" + userId;
        let userRoles: Array<string> = new Array<string>();
        return this.http.get(urlQuery, this.jwt()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<Array<string>, Object>(userRoles, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }
    getUserStore(userId: string): Observable<KeyValuePair<string>> {
        let urlQuery: string = this.API_GETUSERSTORE + "?userId=" + userId;
        let userStore: KeyValuePair<string> = new KeyValuePair<string>();
        return this.http.get(urlQuery, this.jwt()).map((res: Response) => {
            if (res.json()) {
                return Object.assign<KeyValuePair<string>, Object>(userStore, res.json());
            }
            else {
                Observable.throw(res);
            }
        });
    }
    private handleError(err: Response): void {
        console.error(err);
    }
}