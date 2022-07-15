import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoaderService } from '../_shared/services/loader.service';
import { environment } from '../../environments/environment';
import { AppSetting } from '../_shared/constants/app-setting';
@Injectable()
export class fwcAPIInterceptor implements HttpInterceptor {
    constructor(public loaderService: LoaderService) {
        this.loaderService.show();
    }
    authReq: any = '';
    storeKeyMongo = environment.defaultMongoKey;
    storeKey = environment.defaultStoreKey;
    
    intercept (req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        if(AppSetting.StoreKey != ""){
            this.storeKey = AppSetting.StoreKey;
        }
        
        if (req.url.match(/v1\//)) {
            this.authReq = req.clone({
                headers: new HttpHeaders({
                    'Content-Type' : 'application/json',
                   'x-api-key' : this.storeKey
                })
            });
        }else{
            this.authReq = req.clone({
                headers: new HttpHeaders({
                    'Content-Type' : 'application/json',
                    'storekey' : this.storeKey
                })
            });
        }
        
        return next.handle(this.authReq);
        
    }
}