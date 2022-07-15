import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
@Injectable({
    providedIn: 'root'
})
export class http {

    constructor(private http: HttpClient, private ts: ToastrService) {
    }
    get(url: string): Observable<any> {
        return this.http.get<any>(url).pipe(
            map(this.extractData),
            catchError(this.handleErrorObservable)
        );
    }
    post(url: string, model: any) : Observable<any>{
        return this.http.post<any>(url, model).pipe(
            map(this.extractData),
            catchError(this.handleErrorObservable)
        );
    }

    del(url: string): Observable<any> {
        return this.http.delete<any>(url).pipe(
            map(this.extractData),
            catchError(this.handleErrorObservable)
        );

    }
    handleErrorObservable(error: Response | any) {
        console.error(error.message || error);
        this.ts.error("Failed to Perform Operation");
        return Observable.throw(error.message || error);
    }
    extractData(res: Response) {
        const body = res;
        return res;
    }


}
